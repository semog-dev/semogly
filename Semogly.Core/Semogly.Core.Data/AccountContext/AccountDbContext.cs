using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Data.AccountContext;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}