using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Service to retrieve Project-level Users from ACC
/// </summary>
public class ProjectUsersService
{
    private readonly HttpClient _httpClient;

    public ProjectUsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// GET projects/{projectId}/users - Get all users in a project
    /// ACC Admin API endpoint
    /// </summary>
    public async Task<JsonDocument> GetProjectUsersAsync(string accessToken, string accountId, string projectId, string region = "US")
    {
        // Remove "b." prefix from projectId if present
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        // ACC Admin API v1 - correct endpoint for project users
        var url = $"https://developer.api.autodesk.com/construction/admin/v1/projects/{projectId}/users";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// GET projects/{projectId}/users/{userId} - Get a specific user in a project
    /// ACC Admin API endpoint
    /// </summary>
    public async Task<JsonDocument> GetProjectUserByIdAsync(string accessToken, string accountId, string projectId, string userId, string region = "US")
    {
        // Remove "b." prefix from projectId if present
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        // ACC Admin API v1 - correct endpoint for specific project user
        var url = $"https://developer.api.autodesk.com/construction/admin/v1/projects/{projectId}/users/{userId}";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

