using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Hubs Service: Retrieves Hubs from APS Data Management API
/// 
/// WHAT IS A HUB?
/// - A Hub represents a BIM 360, ACC (Autodesk Construction Cloud), or Fusion Team account
/// - Think of it as a "workspace" or "organization" that contains projects
/// - Example: Your company's BIM 360 account is one hub
/// 
/// WHAT THIS SERVICE DOES:
/// - Gets list of all hubs the user/app has access to
/// - Gets details of a specific hub by ID
/// 
/// API ENDPOINT: https://developer.api.autodesk.com/project/v1/hubs
/// REQUIRES: Token with "data:read" scope
/// </summary>
public class HubsService
{
    private readonly HttpClient _httpClient;
    private const string HUBS_URL = "https://developer.api.autodesk.com/project/v1/hubs";

    public HubsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get All Hubs
    /// 
    /// WHAT IT RETURNS:
    /// - List of hubs (accounts) the token has access to
    /// - Each hub has: ID (with "b." prefix), name, type, region
    /// 
    /// TYPICAL USE CASE:
    /// 1. User logs in or app gets a token
    /// 2. Call this to show list of available accounts
    /// 3. User selects a hub
    /// 4. Use hub ID to get projects within that hub
    /// 
    /// EXAMPLE RESPONSE:
    /// {
    ///   "data": [
    ///     {
    ///       "id": "b.3def0ac0-6276-4c72...",
    ///       "attributes": { "name": "My Company BIM 360" }
    ///     }
    ///   ]
    /// }
    /// </summary>
    /// <param name="accessToken">APS access token (2-legged or 3-legged)</param>
    /// <returns>JSON document with list of hubs</returns>
    public async Task<JsonDocument> GetHubsAsync(string accessToken)
    {
        // Create HTTP GET request
        using var request = new HttpRequestMessage(HttpMethod.Get, HUBS_URL);
        
        // Add authorization header with Bearer token
        // This is how we tell Autodesk "I have permission to access this data"
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Send request to Autodesk
        var response = await _httpClient.SendAsync(request);
        
        // If Autodesk returns error (401, 403, 404, etc.), throw exception
        response.EnsureSuccessStatusCode();

        // Read and parse JSON response
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Get Specific Hub by ID
    /// 
    /// WHAT IT RETURNS:
    /// - Detailed information about one hub
    /// - Includes: name, region, type, and metadata
    /// 
    /// WHEN TO USE:
    /// - When you already know the hub ID
    /// - To get updated information about a specific hub
    /// - To verify hub still exists and token has access
    /// 
    /// NOTE: Hub ID must include "b." prefix (e.g., "b.3def0ac0-6276...")
    /// </summary>
    /// <param name="accessToken">APS access token</param>
    /// <param name="hubId">Hub identifier with "b." prefix</param>
    /// <returns>JSON document with hub details</returns>
    public async Task<JsonDocument> GetHubByIdAsync(string accessToken, string hubId)
    {
        // Build URL with hub ID
        // Example: https://developer.api.autodesk.com/project/v1/hubs/b.3def0ac0-6276...
        var url = $"{HUBS_URL}/{hubId}";
        
        // Create and configure HTTP request
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Send request and handle response
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Parse and return JSON
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}

