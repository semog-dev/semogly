using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Infrastructure.Extensions;

namespace Semogly.Core.Infrastructure.Data;

public class SemoglyDbContext(DbContextOptions<SemoglyDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyBaseEntityConfiguration();
    }
}
