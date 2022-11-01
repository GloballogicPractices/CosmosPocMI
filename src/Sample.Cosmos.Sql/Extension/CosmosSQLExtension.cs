using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sample.Cosmos.Sql.Infrastructure.Internal;

namespace Sample.Cosmos.Sql.Extension;

public static class CosmosSqlExtension
{

    public static DbContextOptionsBuilder UseCosmos(
    this DbContextOptionsBuilder optionsBuilder,
    string accountEndpoint,
    string accountKey,
    string databaseName,
    TokenCredential tokenCredential,
    Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
    {
        var tokenCredentialExtension = new CosmosTokenCredentialOptionsExtension(tokenCredential);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(tokenCredentialExtension);

        var defaultExtension = optionsBuilder.Options.FindExtension<CosmosOptionsExtension>()
                                          ?? new CosmosOptionsExtension();

        defaultExtension = defaultExtension
            .WithAccountEndpoint(accountEndpoint)
            .WithDatabaseName(databaseName)
            .WithAccountKey(accountKey);

        // There are hardcoded dependencies in EF Core Cosmos internally to CosmosOptionsExtension. So you HAVE to register it.
        // The generic parameters are not used under the hood unfortunately (in DbContextOptions<TContext> it does GetType()).
        // If they were used, we could create a cleaner solution.
        // Now we register both, first one for the custom credentials, and second one for the hardcoded dependencies
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(defaultExtension);

        cosmosOptionsAction?.Invoke(new CosmosDbContextOptionsBuilder(optionsBuilder));

        return optionsBuilder;
    }
}
