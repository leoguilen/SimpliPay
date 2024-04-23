namespace PaymentGateway.Utils;

internal interface IApiKeyValidation
{
    Task<bool> IsValidAsync(
        string apiKey,
        CancellationToken cancellationToken = default);
}

internal class ApiKeyValidation(
    IMemoryCache memoryCache,
    IDbConnection dbConnection,
    IHttpContextAccessor httpContextAccessor)
    : IApiKeyValidation
{
    private static readonly string s_query = @"
        SELECT client_id
        FROM clients.client_api_keys
        WHERE api_key = @ApiKey AND
            expiration_date > CURRENT_TIMESTAMP AND
            is_active = TRUE";

    public async Task<bool> IsValidAsync(
        string apiKey,
        CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(apiKey, out Guid? clientId))
        {
            return true;
        }

        var query = new CommandDefinition(
            commandText: s_query,
            parameters: new { ApiKey = apiKey },
            cancellationToken: cancellationToken);

        clientId = await dbConnection.ExecuteScalarAsync<Guid?>(query);
        if (!clientId.HasValue)
        {
            return false;
        }

        httpContextAccessor.HttpContext?.Items.Add("ClientId", clientId);
        memoryCache.Set(apiKey, clientId, TimeSpan.FromMinutes(5));

        return true;
    }
}
