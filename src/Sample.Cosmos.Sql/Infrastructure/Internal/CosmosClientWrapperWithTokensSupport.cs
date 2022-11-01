using Azure.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore.Cosmos.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Cosmos.Storage.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sample.Cosmos.Sql.Infrastructure.Internal;

internal class CosmosClientWrapperWithTokensSupport : ISingletonCosmosClientWrapper
{
    private static readonly string UserAgent = " Microsoft.EntityFrameworkCore.Cosmos/" + ProductInfo.GetVersion();
    private readonly CosmosClientOptions options;
    private readonly string endpoint;
    private readonly string key;
    private readonly string connectionString;
    private readonly TokenCredential tokenCredential;
    private CosmosClient client;

    public CosmosClientWrapperWithTokensSupport(ICosmosSingletonOptions options, CosmosTokenCredentialSingletonOptions singletonOptions)
    {
        endpoint = options.AccountEndpoint;
        key = options.AccountKey;
        connectionString = options.ConnectionString;
        var configuration = new CosmosClientOptions { ApplicationName = UserAgent, Serializer = new JsonCosmosSerializer() };

        if (options.Region != null)
        {
            configuration.ApplicationRegion = options.Region;
        }

        if (options.LimitToEndpoint != null)
        {
            configuration.LimitToEndpoint = options.LimitToEndpoint.Value;
        }

        if (options.ConnectionMode != null)
        {
            configuration.ConnectionMode = options.ConnectionMode.Value;
        }

        if (options.WebProxy != null)
        {
            configuration.WebProxy = options.WebProxy;
        }

        if (options.RequestTimeout != null)
        {
            configuration.RequestTimeout = options.RequestTimeout.Value;
        }

        if (options.OpenTcpConnectionTimeout != null)
        {
            configuration.OpenTcpConnectionTimeout = options.OpenTcpConnectionTimeout.Value;
        }

        if (options.IdleTcpConnectionTimeout != null)
        {
            configuration.IdleTcpConnectionTimeout = options.IdleTcpConnectionTimeout.Value;
        }

        if (options.GatewayModeMaxConnectionLimit != null)
        {
            configuration.GatewayModeMaxConnectionLimit = options.GatewayModeMaxConnectionLimit.Value;
        }

        if (options.MaxTcpConnectionsPerEndpoint != null)
        {
            configuration.MaxTcpConnectionsPerEndpoint = options.MaxTcpConnectionsPerEndpoint.Value;
        }

        if (options.MaxRequestsPerTcpConnection != null)
        {
            configuration.MaxRequestsPerTcpConnection = options.MaxRequestsPerTcpConnection.Value;
        }

        if (options.HttpClientFactory != null)
        {
            configuration.HttpClientFactory = options.HttpClientFactory;
        }

        tokenCredential = singletonOptions.TokenCredential;

        this.options = configuration;
    }

    public virtual CosmosClient Client
        => client ??= CreateCosmosClient();

    /// <inheritdoc />
    public void Dispose()
    {
        client?.Dispose();
        client = null;
    }

    private CosmosClient CreateCosmosClient()
    {
        return !string.IsNullOrEmpty(connectionString)
            ? new CosmosClient(connectionString, options)
            : !string.IsNullOrEmpty(key)
            ? new CosmosClient(endpoint, key, options)
            : new CosmosClient(endpoint, tokenCredential, options);
    }
}
