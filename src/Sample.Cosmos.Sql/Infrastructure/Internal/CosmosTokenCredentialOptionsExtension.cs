using Azure.Core;
using Microsoft.EntityFrameworkCore.Cosmos.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Cosmos.Storage.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Cosmos.Sql.Infrastructure.Internal;

internal class CosmosTokenCredentialOptionsExtension : CosmosOptionsExtension
{
    public CosmosTokenCredentialOptionsExtension()
    {
    }

    public CosmosTokenCredentialOptionsExtension(TokenCredential? tokenCredential)
    {
        TokenCredential = tokenCredential;
    }

    public CosmosTokenCredentialOptionsExtension(CosmosTokenCredentialOptionsExtension ext)
        : base(ext)
    {
        TokenCredential = ext.TokenCredential;
    }

    public TokenCredential TokenCredential { get; private set; }

    public override void ApplyServices(IServiceCollection services)
    {
        new EntityFrameworkServicesBuilder(services)
                 .TryAdd<ISingletonOptions, CosmosTokenCredentialSingletonOptions>(p => p.GetRequiredService<CosmosTokenCredentialSingletonOptions>())
                 .TryAddProviderSpecificServices(
                     b => b
                         ////Custom
                         .TryAddSingleton(x => new CosmosTokenCredentialSingletonOptions())
                         ////Custom
                         .TryAddSingleton<ISingletonCosmosClientWrapper, CosmosClientWrapperWithTokensSupport>());
    }

    protected override CosmosOptionsExtension Clone()
    {
        return new CosmosTokenCredentialOptionsExtension(this);
    }
}
