using APSStarterPackSwaggerUI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Authentication Service: Handles OAuth 2.0 authentication with Autodesk APS
/// 
/// WHAT IT DOES:
/// - Gets access tokens needed to call APS APIs
/// - Supports both 2-legged (app-only) and 3-legged (user login) authentication
/// 
/// WHY WE NEED THIS:
/// - Every APS API call requires an access token for security
/// - Different APIs need different authentication types:
///   * 2-legged: For server-to-server operations (reading data)
///   * 3-legged: For user-specific operations (SSO login)
/// 
/// HOW IT WORKS:
/// - Makes HTTP POST requests to Autodesk's OAuth endpoints
/// - Exchanges credentials (or authorization codes) for access tokens
/// - Returns tokens that are valid for ~1 hour
/// </summary>
public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly APSConfiguration _config;

    public AuthenticationService(HttpClient httpClient, IOptions<APSConfiguration> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
    }

    /// <summary>
    /// Get 2-Legged OAuth Token (Client Credentials Flow)
    /// 
    /// USE WHEN: Your app needs to access data without requiring user login
    /// EXAMPLES: Reading hubs, projects, folders (public data)
    /// 
    /// HOW IT WORKS:
    /// 1. Send ClientId + ClientSecret to Autodesk
    /// 2. Autodesk verifies your app's identity
    /// 3. Returns an access token valid for ~1 hour
    /// 
    /// NOTE: This token represents your APP, not a specific user
    /// </summary>
    /// <param name="scope">Permissions requested (e.g., "data:read data:write")</param>
    /// <returns>TokenResponse with AccessToken and expiration time</returns>
    public async Task<TokenResponse> GetTwoLeggedTokenAsync(string scope = "data:read data:write data:create")
    {
        // Prepare the request data (like filling out a form)
        var requestData = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },           // Your app's ID
            { "client_secret", _config.ClientSecret },   // Your app's password
            { "grant_type", "client_credentials" },      // Type of OAuth flow
            { "scope", scope }                           // What permissions you need
        };

        // Send POST request to Autodesk's token endpoint
        var content = new FormUrlEncodedContent(requestData);
        var response = await _httpClient.PostAsync(_config.OAuth2GetTokenUrl, content);
        
        // If Autodesk returns an error (400, 403, etc.), this will throw an exception
        response.EnsureSuccessStatusCode();
        
        // Parse the JSON response into a TokenResponse object
        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
        
        return tokenResponse ?? throw new Exception("Failed to deserialize token response");
    }

    /// <summary>
    /// Generate 3-Legged OAuth Authorization URL (Step 1 of SSO Login)
    /// 
    /// USE WHEN: You need a user to login with their Autodesk account
    /// EXAMPLES: Accessing user-specific data, showing who's logged in
    /// 
    /// HOW IT WORKS:
    /// 1. Generate a URL with your app's credentials
    /// 2. Redirect user's browser to this URL
    /// 3. User logs in with their Autodesk credentials
    /// 4. Autodesk redirects back to your CallbackUrl with an authorization code
    /// 5. Use GetThreeLeggedTokenAsync() to exchange code for token
    /// 
    /// NOTE: This is just step 1 - generating the URL. The actual login happens in the browser.
    /// </summary>
    /// <param name="scope">Permissions requested from the user</param>
    /// <returns>URL to redirect the user to for login</returns>
    public string GetThreeLeggedAuthorizationUrl(string scope = "data:read data:write data:create")
    {
        // Build the query parameters for the authorization URL
        var parameters = new Dictionary<string, string>
        {
            { "response_type", "code" },                 // We want an authorization code
            { "client_id", _config.ClientId },           // Your app's ID
            { "redirect_uri", _config.CallbackUrl },     // Where to send user after login
            { "scope", scope }                           // What permissions to request
        };

        // Build the query string (key1=value1&key2=value2)
        var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
        
        // Return the full URL (e.g., https://developer.api.autodesk.com/authentication/v2/authorize?...)
        return $"{_config.OAuth2AuthorizeUrl}?{queryString}";
    }

    /// <summary>
    /// Exchange Authorization Code for Access Token (Step 2 of SSO Login)
    /// 
    /// USE WHEN: User has logged in and Autodesk sent you an authorization code
    /// 
    /// HOW IT WORKS:
    /// 1. User logged in via the URL from GetThreeLeggedAuthorizationUrl()
    /// 2. Autodesk redirected them back to your CallbackUrl with a "code" parameter
    /// 3. This method exchanges that code for an actual access token
    /// 4. The token can now be used to make API calls on behalf of the user
    /// 
    /// NOTE: The authorization code is single-use and expires quickly (minutes)
    /// </summary>
    /// <param name="authorizationCode">The code received from Autodesk after user login</param>
    /// <returns>TokenResponse with AccessToken, RefreshToken, and expiration</returns>
    public async Task<TokenResponse> GetThreeLeggedTokenAsync(string authorizationCode)
    {
        // Prepare the request to exchange code for token
        var requestData = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },           // Your app's ID
            { "client_secret", _config.ClientSecret },   // Your app's password
            { "grant_type", "authorization_code" },      // Type of OAuth flow
            { "code", authorizationCode },               // The code from user login
            { "redirect_uri", _config.CallbackUrl }      // Must match what was used in step 1
        };

        // Send POST request to exchange the code for a token
        var content = new FormUrlEncodedContent(requestData);
        var response = await _httpClient.PostAsync(_config.OAuth2GetTokenUrl, content);
        
        // If exchange fails (invalid code, expired, etc.), this throws an exception
        response.EnsureSuccessStatusCode();
        
        // Parse the response - includes access_token, refresh_token, and expires_in
        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
        
        return tokenResponse ?? throw new Exception("Failed to deserialize token response");
    }
}

