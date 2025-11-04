using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Folders Service: Manages Folders and Files in APS Data Management API
/// 
/// WHAT ARE FOLDERS?
/// - Folders are containers that organize files within a project
/// - Like Windows folders or Mac Finder folders, but stored in the cloud
/// - Example: "Plans", "Photos", "Specifications" folders in a construction project
/// 
/// IMPORTANT CONCEPT - Folder URN:
/// - Folder URN = Unique Resource Name (a special ID format)
/// - Example: "urn:adsk.wipprod:fs.folder:co.GdRoPl0LQxOl"
/// - Contains colons ":" which MUST be URL-encoded when making HTTP requests
/// 
/// WHAT THIS SERVICE DOES:
/// - Get folder contents (subfolders and files)
/// - Get folder details
/// - Create new folders
/// - Search within folders
/// 
/// API ENDPOINT: https://developer.api.autodesk.com/data/v1
/// REQUIRES: Token with appropriate scopes (data:read, data:write, data:create, data:search)
/// </summary>
public class FoldersService
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://developer.api.autodesk.com/data/v1";

    public FoldersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get Folder Contents (Subfolders and Files)
    /// 
    /// WHAT IT RETURNS:
    /// - List of items (subfolders and files) inside a folder
    /// - Each item has: name, type (folder/file), size, modified date, URN
    /// 
    /// TYPICAL USE CASE:
    /// 1. Get top folders from ProjectsService.GetProjectTopFoldersAsync()
    /// 2. Pick a folder and copy its URN
    /// 3. Call this to see what's inside that folder
    /// 4. Navigate deeper by calling this again with subfolder URNs
    /// 
    /// IMPORTANT - URL ENCODING:
    /// Folder URNs contain colons ":" (e.g., "urn:adsk.wipprod:fs.folder:co.xxxxx")
    /// Colons are special characters in URLs and MUST be encoded
    /// We use Uri.EscapeDataString() to convert ":" to "%3A"
    /// 
    /// WHY THIS MATTERS:
    /// Without encoding, Autodesk API will return 400 Bad Request
    /// With encoding, it works perfectly
    /// 
    /// EXAMPLE:
    /// Input:  urn:adsk.wipprod:fs.folder:co.GdRoPl0LQxOl
    /// Encoded: urn%3Aadsk.wipprod%3Afs.folder%3Aco.GdRoPl0LQxOl
    /// </summary>
    /// <param name="accessToken">APS access token with data:read scope</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <param name="folderUrn">Folder URN (will be URL-encoded automatically)</param>
    /// <returns>JSON document with list of subfolders and files</returns>
    public async Task<JsonDocument> GetFolderContentsAsync(string accessToken, string projectId, string folderUrn)
    {
        // CRITICAL: URL-encode the folderUrn because it contains colons
        // Example: "urn:adsk.wipprod:fs.folder:co.xxx" → "urn%3Aadsk.wipprod%3Afs.folder%3Aco.xxx"
        var encodedFolderUrn = Uri.EscapeDataString(folderUrn);
        var url = $"{BASE_URL}/projects/{projectId}/folders/{encodedFolderUrn}/contents";
        
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
    /// Get Folder Details (Metadata)
    /// 
    /// WHAT IT RETURNS:
    /// - Detailed information about a specific folder
    /// - Includes: name, creation date, modified date, permissions, relationships
    /// 
    /// WHEN TO USE:
    /// - To get folder properties without listing contents
    /// - To check folder metadata (when was it created, by whom)
    /// - To verify folder exists before performing operations
    /// 
    /// DIFFERENCE FROM GetFolderContentsAsync:
    /// - GetFolder = Folder info only (metadata)
    /// - GetFolderContents = List of items inside the folder
    /// 
    /// NOTE: Also requires URL encoding of folder URN
    /// </summary>
    /// <param name="accessToken">APS access token with data:read scope</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <param name="folderUrn">Folder URN (will be URL-encoded automatically)</param>
    /// <returns>JSON document with folder metadata</returns>
    public async Task<JsonDocument> GetFolderAsync(string accessToken, string projectId, string folderUrn)
    {
        // URL-encode the folderUrn (same reason as GetFolderContentsAsync)
        var encodedFolderUrn = Uri.EscapeDataString(folderUrn);
        var url = $"{BASE_URL}/projects/{projectId}/folders/{encodedFolderUrn}";
        
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
    /// Create a New Folder
    /// 
    /// WHAT IT DOES:
    /// - Creates a new subfolder inside an existing folder
    /// - You choose the name, we handle the complex JSON structure
    /// 
    /// TYPICAL USE CASE:
    /// 1. User navigates to a folder
    /// 2. Clicks "Create New Folder"
    /// 3. Enters a name (e.g., "Submittals")
    /// 4. Call this to create it
    /// 5. New folder appears in ACC/BIM 360 immediately
    /// 
    /// IMPORTANT - JSON API FORMAT:
    /// APS uses "JSON API" spec (http://jsonapi.org/)
    /// This is why the request body looks complex with "jsonapi", "data", "relationships"
    /// This is Autodesk's standard format - just follow the pattern
    /// 
    /// WHY THE COMPLEX STRUCTURE?
    /// - "jsonapi": Declares we're using JSON API format
    /// - "data": The actual folder data
    /// - "attributes": Folder properties (name, type)
    /// - "relationships": Links to parent folder (where to create it)
    /// 
    /// CRITICAL - USE 3-LEGGED TOKEN:
    /// Creating folders requires knowing WHO is creating the folder.
    /// You MUST use a 3-legged token (from user's SSO login).
    /// 3-legged token already contains user identity - no additional headers needed!
    /// 
    /// REQUIRES: 3-legged token with data:create scope
    /// </summary>
    /// <param name="accessToken">3-legged APS access token with data:create scope</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <param name="parentFolderUrn">URN of parent folder (where to create new folder)</param>
    /// <param name="folderName">Name for the new folder (e.g., "Submittals")</param>
    /// <returns>JSON document with created folder details</returns>
    public async Task<JsonDocument> CreateFolderAsync(string accessToken, string projectId, string parentFolderUrn, string folderName)
    {
        var url = $"{BASE_URL}/projects/{projectId}/folders";
        
        // Build the request body according to APS Data Management API specification
        // This follows the JSON API format (http://jsonapi.org/)
        var requestBody = new
        {
            jsonapi = new { version = "1.0" },              // JSON API version
            data = new
            {
                type = "folders",                            // Resource type
                attributes = new
                {
                    name = folderName,                       // Folder name (user input)
                    extension = new
                    {
                        type = "folders:autodesk.bim360:Folder",  // BIM 360/ACC folder type
                        version = "1.0.0",
                        data = new
                        {
                            folderType = "standard"          // Standard folder type
                        }
                    }
                },
                relationships = new                          // Where to create this folder
                {
                    parent = new
                    {
                        data = new
                        {
                            type = "folders",
                            id = parentFolderUrn             // Parent folder URN
                        }
                    }
                }
            }
        };

        // Serialize to JSON string
        var jsonContent = JsonSerializer.Serialize(requestBody);
        
        // Create POST request with content type "application/json"
        // Note: Using 3-legged token which already contains user identity
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Send request and handle response
        var response = await _httpClient.SendAsync(request);
        
        // Provide detailed error if creation fails
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Create folder failed with status {response.StatusCode}: {errorContent}. " +
                "Common causes: Using 2-legged token (use 3-legged token instead), insufficient permissions, or invalid parent folder URN.");
        }
        
        response.EnsureSuccessStatusCode();

        // Parse and return created folder details
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Search for Folders and Files (Recursive Search)
    /// 
    /// WHAT IT DOES:
    /// - Searches for folders/files within a folder AND its subfolders (recursive)
    /// - Like using the search box in Windows Explorer or Mac Finder
    /// - Searches by display name (the name you see in ACC/BIM 360)
    /// 
    /// TYPICAL USE CASE:
    /// 1. User is in a project with many nested folders
    /// 2. They remember the file is named "FloorPlan" but don't know where
    /// 3. Call this with searchFilter="FloorPlan"
    /// 4. API returns all matching items, even in deep subfolders
    /// 
    /// IMPORTANT - FILTER SYNTAX:
    /// We use "filter[attributes.displayName]={searchTerm}"
    /// This is APS's standard filter format for searching by name
    /// 
    /// WHY "attributes.displayName"?
    /// - displayName = the name shown to users in ACC/BIM 360
    /// - This is different from technical IDs or URNs
    /// - Common APS pattern across many APIs
    /// 
    /// REQUIRES: Token with data:search scope
    /// NOTE: Token MUST include data:search - if missing, you'll get 403 Forbidden
    /// </summary>
    /// <param name="accessToken">APS access token with data:search scope</param>
    /// <param name="projectId">Project ID with "b." prefix</param>
    /// <param name="folderUrn">Folder URN to start search from (will search this + subfolders)</param>
    /// <param name="searchFilter">Search term (e.g., "FloorPlan", "Folder", "RFI")</param>
    /// <returns>JSON document with search results</returns>
    public async Task<JsonDocument> SearchFoldersAsync(string accessToken, string projectId, string folderUrn, string searchFilter)
    {
        // URL-encode the folderUrn (contains colons)
        var encodedFolderUrn = Uri.EscapeDataString(folderUrn);
        
        // Build the search filter parameter
        // Format: filter[attributes.displayName]={searchTerm}
        // Example: filter[attributes.displayName]=FloorPlan
        var filterParam = $"filter[attributes.displayName]={Uri.EscapeDataString(searchFilter)}";
        var url = $"{BASE_URL}/projects/{projectId}/folders/{encodedFolderUrn}/search?{filterParam}";
        
        // Create and send HTTP request
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        
        // Provide detailed error message if search fails
        // Common errors: Missing data:search scope, invalid folder URN, no results
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Search failed with status {response.StatusCode}: {errorContent}. " +
                "Common causes: Missing 'data:search' scope, invalid folder URN, or search term not found.");
        }
        
        response.EnsureSuccessStatusCode();

        // Parse and return search results
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

