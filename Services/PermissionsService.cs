using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Service to manage Permissions in ACC/BIM 360
/// Permissions control who can access folders and what they can do
/// </summary>
public class PermissionsService
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://developer.api.autodesk.com/bim360/docs/v1";

    public PermissionsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get permissions for a folder
    /// Returns which users/companies have access to a folder and their permission levels
    /// </summary>
    public async Task<JsonDocument> GetFolderPermissionsAsync(string accessToken, string projectId, string folderUrn)
    {
        // Remove "b." prefix from projectId if present
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        var url = $"{BASE_URL}/projects/{projectId}/folders/{folderUrn}/permissions";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Set permissions for a folder
    /// This allows you to grant or modify access to folders
    /// Example permission actions: "view", "collaborate", "create", "upload", etc.
    /// </summary>
    public async Task<JsonDocument> SetFolderPermissionsAsync(
        string accessToken, 
        string projectId, 
        string folderUrn, 
        List<PermissionEntry> permissions)
    {
        // Remove "b." prefix from projectId if present
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        var url = $"{BASE_URL}/projects/{projectId}/folders/{folderUrn}/permissions";
        
        var requestBody = new
        {
            permissions = permissions.Select(p => new
            {
                subjectType = p.SubjectType, // "USER", "COMPANY", or "ROLE"
                subjectId = p.SubjectId,
                actions = p.Actions,
                permission = p.Permission // "ALLOW" or "DENY"
            })
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Get an example permissions structure
    /// This helps understand how permissions are structured
    /// </summary>
    public object GetPermissionsExample()
    {
        return new
        {
            message = "Example permission structure for ACC/BIM 360",
            example = new
            {
                subjectType = "COMPANY", // Can be USER, COMPANY, or ROLE
                subjectId = "12345", // ID of the user, company, or role
                actions = new[] { "view", "download", "collaborate" }, // What they can do
                permission = "ALLOW" // ALLOW or DENY
            },
            availableActions = new[]
            {
                "view", "download", "collaborate", "create", 
                "upload", "delete", "admin"
            },
            notes = "Use GetCompanies or GetRoles endpoints to get valid subjectIds"
        };
    }
}

/// <summary>
/// Helper class for permission entries
/// </summary>
public class PermissionEntry
{
    public string SubjectType { get; set; } = string.Empty; // USER, COMPANY, ROLE
    public string SubjectId { get; set; } = string.Empty;
    public List<string> Actions { get; set; } = new();
    public string Permission { get; set; } = "ALLOW"; // ALLOW or DENY
}

