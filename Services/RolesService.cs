using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Service to retrieve Roles from ACC/BIM 360
/// Roles define what permissions users have in a project
/// </summary>
public class RolesService
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://developer.api.autodesk.com/hq/v1/accounts";

    public RolesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get industry roles for a project
    /// Returns available role templates like "Architect", "Engineer", "BIM Manager", etc.
    /// </summary>
    public async Task<JsonDocument> GetProjectIndustryRolesAsync(string accessToken, string accountId, string projectId)
    {
        // Remove "b." prefix from projectId if present
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        // ACC API v2 - industry_roles endpoint (project-level)
        var url = $"https://developer.api.autodesk.com/hq/v2/accounts/{accountId}/projects/{projectId}/industry_roles";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

