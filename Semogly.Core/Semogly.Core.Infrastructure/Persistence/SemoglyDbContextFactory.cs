using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Semogly.Core.Domain.Shared;

namespace Semogly.Core.Infrastructure.Persistence;

public class SemoglyDbContextFactory : IDesignTimeDbContextFactory<SemoglyDbContext>
{
    public SemoglyDbContext CreateDbContext(string[] args)
    {        
        var optionsBuilder = new DbContextOptionsBuilder<SemoglyDbContext>();
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__Postgres"));
        return new SemoglyDbContext(optionsBuilder.Options);
    }
}
