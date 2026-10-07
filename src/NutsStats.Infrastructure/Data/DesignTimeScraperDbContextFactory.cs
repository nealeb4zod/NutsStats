using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NutsStats.Infrastructure.Data;

public sealed class DesignTimeScraperDbContextFactory : IDesignTimeDbContextFactory<ScraperDbContext>
{
    public ScraperDbContext CreateDbContext(string[] args)
    {
        var dir = AppContext.BaseDirectory;
        var configurationBuilder = new ConfigurationBuilder();

        for (var i = 0; i < 5; i++)
        {
            var candidate = Path.Combine(dir, "appsettings.json");
            if (File.Exists(candidate))
            {
                configurationBuilder.AddJsonFile(candidate, optional: false, reloadOnChange: false);
                break;
            }

            dir = Path.GetDirectoryName(dir) ?? dir;
        }

        configurationBuilder.AddEnvironmentVariables();
        var config = configurationBuilder.Build();

        var connectionString = config.GetConnectionString("Default")
            ?? "Server=localhost,1433;Database=NutsStats;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true;";

        var optionsBuilder = new DbContextOptionsBuilder<ScraperDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ScraperDbContext(optionsBuilder.Options);
    }
}
