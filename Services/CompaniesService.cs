using System.Net.Http.Headers;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Companies Service: Retrieves Companies from ACC Admin API
/// 
/// WHAT IS A COMPANY in ACC/BIM 360?
/// - A Company represents an organization/contractor that has access to projects
/// - Example: "General Contractor Inc.", "Architect Studio", "MEP Engineering"
/// - Companies can be assigned to multiple projects
/// - Users belong to companies and inherit company permissions
/// 
/// REAL-WORLD SCENARIO:
/// Construction project with:
/// - General Contractor (your company)
/// - Architect firm
/// - Mechanical/Electrical engineers
/// - Specialty contractors
/// Each is a "Company" in ACC
/// 
/// WHAT THIS SERVICE DOES:
/// - Gets all companies in an account
/// - Gets companies assigned to a specific project
/// 
/// API ENDPOINT: https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/companies
/// REQUIRES: Token with "account:read" scope
/// 
/// IMPORTANT - ID FORMAT:
/// accountId does NOT have "b." prefix (different from Data Management API)
/// If hubId = "b.12345-67890", then accountId = "12345-67890" (no "b.")
/// </summary>
public class CompaniesService
{
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://developer.api.autodesk.com/hq/v1/accounts";

    public CompaniesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get All Companies in an Account
    /// 
    /// WHAT IT RETURNS:
    /// - List of all companies that have access to the account
    /// - Each company has: ID, name, trade (specialty), country, status
    /// 
    /// TYPICAL USE CASE:
    /// - Admin wants to see all organizations with account access
    /// - Setting up a new project and selecting which companies to include
    /// - Auditing company access for security/compliance
    /// 
    /// IMPORTANT NOTES:
    /// - accountId = hubId WITHOUT "b." prefix
    ///   Example: hubId "b.3def0ac0-6276..." → accountId "3def0ac0-6276..."
    /// - region = "US" or "EMEA" (where the account is hosted)
    /// - Requires "account:read" scope in token
    /// 
    /// WHY REGION MATTERS:
    /// ACC/BIM 360 data is stored in regional data centers
    /// Must specify correct region to access the data
    /// </summary>
    /// <param name="accessToken">APS access token with account:read scope</param>
    /// <param name="accountId">Account ID WITHOUT "b." prefix</param>
    /// <param name="region">Region (US or EMEA)</param>
    /// <returns>JSON document with list of companies</returns>
    public async Task<JsonDocument> GetCompaniesAsync(string accessToken, string accountId, string region = "US")
    {
        // Build URL with region and accountId
        // Example: /hq/v1/regions/US/accounts/3def0ac0-6276.../companies
        var url = $"https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/companies";
        
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
    /// Get Companies Assigned to a Specific Project
    /// 
    /// WHAT IT RETURNS:
    /// - List of companies that have been given access to this project
    /// - Subset of account companies (not all account companies have project access)
    /// 
    /// TYPICAL USE CASE:
    /// - Project manager wants to see which contractors are working on this project
    /// - Need to verify company access before assigning users to project
    /// - Checking if a new subcontractor needs to be added
    /// 
    /// IMPORTANT - PROJECT ID:
    /// projectId can be provided WITH or WITHOUT "b." prefix
    /// We automatically strip "b." if present (ACC API doesn't use it)
    /// Example: "b.e66ece9f-5035..." → "e66ece9f-5035..."
    /// 
    /// WHY COMPANIES MATTER:
    /// Users must belong to a company to be added to a project
    /// Company access is the first step in the permission chain:
    /// Account → Company → Project → User → Folder
    /// </summary>
    /// <param name="accessToken">APS access token with account:read scope</param>
    /// <param name="accountId">Account ID WITHOUT "b." prefix</param>
    /// <param name="projectId">Project ID (we'll strip "b." if present)</param>
    /// <param name="region">Region (US or EMEA)</param>
    /// <returns>JSON document with list of companies assigned to project</returns>
    public async Task<JsonDocument> GetProjectCompaniesAsync(string accessToken, string accountId, string projectId, string region = "US")
    {
        // Strip "b." prefix from projectId if present
        // ACC Admin API doesn't use "b." prefix (unlike Data Management API)
        projectId = projectId.StartsWith("b.") ? projectId.Substring(2) : projectId;
        
        // Build URL with region, accountId, and projectId
        var url = $"https://developer.api.autodesk.com/hq/v1/regions/{region}/accounts/{accountId}/projects/{projectId}/companies";
        
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

