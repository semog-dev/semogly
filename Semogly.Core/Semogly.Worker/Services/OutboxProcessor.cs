using Microsoft.EntityFrameworkCore;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Infrastructure.Messaging.Interfaces;
using Semogly.Core.Infrastructure.Persistence;

namespace Semogly.Worker.Services;

public class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    IDateTimeProvider dateTimeProvider) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try 
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                var dbContext = scope.ServiceProvider.GetRequiredService<SemoglyDbContext>();

                // 1. Processa em lotes para evitar pressão na memória
                var messages = await dbContext.OutboxMessages
                    .Where(x => x.ProcessedOnUtc == null && x.Error == null)
                    .OrderBy(x => x.OccurredOnUtc)
                    .Take(20) 
                    .ToListAsync(cancellationToken);

                if (messages.Count == 0)
                {
                    await Task.Delay(5000, cancellationToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    try
                    {                        
                        await eventBus.PublishAsync(message.Type, message.Content, cancellationToken);
                        message.MarkAsProcessed(dateTimeProvider);
                    }
                    catch (Exception ex)
                    {
                        message.MarkAsFailed(ex.Message);
                    }
                    finally
                    {
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                
            }
            catch (Exception ex)
            {
                // Log crítico: falha ao acessar o banco ou criar o escopo
                await Task.Delay(10000, cancellationToken); 
            }

            await Task.Delay(3000, cancellationToken);
        }
    }
}

