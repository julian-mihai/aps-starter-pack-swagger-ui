using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Projects Service: Retrieves Projects from APS Data Management API
/// 
/// WHAT IS A PROJECT?
/// - A Project is like a "folder" or "workspace" within a Hub
/// - It contains all files, folders, and data for a specific construction project
/// - Example: "Office Building Renovation 2024" project within your company's hub
/// 
/// HIERARCHY:
/// Hub (Account) → Projects → Folders → Files
/// 
/// WHAT THIS SERVICE DOES:
/// - Gets all projects within a hub
/// - Gets details of a specific project
/// - Gets the root (top-level) folders of a project
/// 
/// API ENDPOINT: https://developer.api.autodesk.com/project/v1/hubs/{hubId}/projects
/// REQUIRES: Token with "data:read" scope
/// </summary>
public class ProjectsService
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://developer.api.autodesk.com/project/v1";

    public ProjectsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get All Projects in a Hub
    /// 
    /// WHAT IT RETURNS:
    /// - List of all projects within the specified hub
    /// - Each project has: ID (with "b." prefix), name, type, status
    /// 
    /// TYPICAL USE CASE:
    /// 1. User selects a hub from GetHubs
    /// 2. Call this to show all projects in that hub
    /// 3. User selects a project
    /// 4. Use project ID to get folders and files
    /// 
    /// EXAMPLE RESPONSE:
    /// {
    ///   "data": [
    ///     {
    ///       "id": "b.e66ece9f-5035-44da...",
    ///       "attributes": { 
    ///         "name": "Office Building Project",
    ///         "status": "active"
    ///       }
    ///     }
    ///   ]
    /// }
    /// 
    /// NOTE: Both hubId and projectId have "b." prefix
    /// </summary>
    /// <param name="accessToken">APS access token</param>
    /// <param name="hubId">Hub ID with "b." prefix (from GetHubs)</param>
    /// <returns>JSON document with list of projects</returns>
    public async Task<JsonDocument> GetProjectsAsync(string accessToken, string hubId)
    {
        // Build URL: /project/v1/hubs/{hubId}/projects
        var url = $"{BASE_URL}/hubs/{hubId}/projects";
        
        // Create HTTP request with authorization
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Send request and handle response
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Parse and return JSON
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Get Specific Project by ID
    /// 
    /// WHAT IT RETURNS:
    /// - Detailed information about one project
    /// - Includes: name, status, type, creation date, last modified
    /// 
    /// WHEN TO USE:
    /// - To get updated project information
    /// - To verify project exists and token has access
    /// - To get project metadata before accessing files
    /// 
    /// NOTE: Both hubId and projectId must include "b." prefix
    /// </summary>
    /// <param name="accessToken">APS access token</param>
    /// <param name="hubId">Hub ID with "b." prefix</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <returns>JSON document with project details</returns>
    public async Task<JsonDocument> GetProjectByIdAsync(string accessToken, string hubId, string projectId)
    {
        // Build URL: /project/v1/hubs/{hubId}/projects/{projectId}
        var url = $"{BASE_URL}/hubs/{hubId}/projects/{projectId}";
        
        // Create and send HTTP request
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Parse and return JSON
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Get Top Folders of a Project (Root Folders)
    /// 
    /// WHAT IT RETURNS:
    /// - List of root-level folders in the project
    /// - These are the "entry points" into the project's file structure
    /// - Common folders: "Plans", "Project Files", "Photos", etc.
    /// 
    /// TYPICAL USE CASE:
    /// 1. User selects a project
    /// 2. Call this to show the main folders
    /// 3. User selects a folder
    /// 4. Use folder URN to navigate deeper (get contents, subfolders)
    /// 
    /// WHY IS THIS USEFUL?
    /// - Shows the project's organization structure
    /// - First step before accessing specific files
    /// - Helps users navigate to the right location
    /// 
    /// NEXT STEP: Use FoldersService.GetFolderContentsAsync() with a folder URN
    /// </summary>
    /// <param name="accessToken">APS access token</param>
    /// <param name="hubId">Hub ID with "b." prefix</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <returns>JSON document with list of top-level folders</returns>
    public async Task<JsonDocument> GetProjectTopFoldersAsync(string accessToken, string hubId, string projectId)
    {
        // Build URL: /project/v1/hubs/{hubId}/projects/{projectId}/topFolders
        var url = $"{BASE_URL}/hubs/{hubId}/projects/{projectId}/topFolders";
        
        // Create and send HTTP request
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Parse and return JSON
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

