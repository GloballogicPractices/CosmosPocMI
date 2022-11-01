using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sample.Cosmos.Sql.Function.Test.Model;

namespace Sample.Cosmos.Sql.Function.Test.DataAccess;

public class ReportDBContext : DbContext
{
    public readonly IConfiguration configuration;
    private IConfigurationSection cosmosSettings;
    public ReportDBContext(DbContextOptions options, IConfiguration configuration)
        : base(options)

    {
        this.configuration = configuration;
    }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        cosmosSettings = configuration.GetSection("CosmosDbSql");
        modelBuilder.HasDefaultContainer(cosmosSettings["ContainerName"]);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Report>().HasNoDiscriminator()
        .ToContainer("report").HasKey(x => x.ReportId);
        base.OnModelCreating(modelBuilder);
    }
}
