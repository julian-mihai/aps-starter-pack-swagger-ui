using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Service to retrieve Account-level Users from ACC
/// </summary>
public class AccountUsersService
{
    private readonly HttpClient _httpClient;

    public AccountUsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// GET users - Get all users in an account
    /// </summary>
    public async Task<JsonDocument> GetUsersAsync(string accessToken, string accountId, string region = "US")
    {
        var url = $"https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/users";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// GET users/:user_id - Get a specific user
    /// </summary>
    public async Task<JsonDocument> GetUserByIdAsync(string accessToken, string accountId, string userId, string region = "US")
    {
        var url = $"https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/users/{userId}";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// GET users/search - Search for users
    /// </summary>
    public async Task<JsonDocument> SearchUsersAsync(string accessToken, string accountId, string email, string region = "US")
    {
        var url = $"https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/users/search?email={Uri.EscapeDataString(email)}";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

