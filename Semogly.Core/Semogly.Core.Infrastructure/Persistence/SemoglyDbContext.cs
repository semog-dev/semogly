using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Infrastructure.Common.Extensions;
using Semogly.Core.Infrastructure.Outbox;

namespace Semogly.Core.Infrastructure.Persistence;

public class SemoglyDbContext(DbContextOptions<SemoglyDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyBaseEntityConfiguration();
    }
}
