using System;
using Azure.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore.Cosmos.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sample.Cosmos.Sql.Infrastructure.Internal;

internal class CosmosTokenCredentialSingletonOptions : CosmosSingletonOptions
{
    private const string Message = "Singleton options changed.";

    public TokenCredential TokenCredential { get; private set; }

    public CosmosClientOptions CosmosClientOptions { get; private set; }

    public override void Initialize(IDbContextOptions options)
    {
        var tokenCredentialOptions = options.FindExtension<CosmosTokenCredentialOptionsExtension>();
        if (tokenCredentialOptions != null)
        {
            TokenCredential = tokenCredentialOptions.TokenCredential;
        }

        base.Initialize(options);
    }

    public override void Validate(IDbContextOptions options)
    {
        var tokenCredentialOptions = options.FindExtension<CosmosTokenCredentialOptionsExtension>();
        if (tokenCredentialOptions != null && TokenCredential != tokenCredentialOptions.TokenCredential)
        {
            throw new InvalidOperationException(Message);
        }

        base.Validate(options);
    }
}
