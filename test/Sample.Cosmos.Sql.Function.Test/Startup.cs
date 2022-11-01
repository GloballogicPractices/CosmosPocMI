using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Cosmos.Sql.Extension;
using Sample.Cosmos.Sql.Function.Test;
using Sample.Cosmos.Sql.Function.Test.DataAccess;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Sample.Cosmos.Sql.Function.Test;

public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddDbContext<ReportDBContext>(optionsBuilder =>
        {
            var cosmosSettings = builder.GetContext().Configuration.GetSection("CosmosDbSQL");

            var key = cosmosSettings["Key"];
            var account = cosmosSettings["Account"];
            var databaseName = cosmosSettings["DatabaseName"];

            optionsBuilder.UseCosmos(
                account,
                key,
                databaseName,
                new DefaultAzureCredential());
        });
        builder.Services.AddScoped<IReportRepository, ReportRepository>();
    }
}
