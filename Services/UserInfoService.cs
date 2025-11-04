using System.Net.Http.Headers;
using System.Text.Json;
using APSStarterPackSwaggerUI.Models;

namespace APSStarterPackSwaggerUI.Services;

/// <summary>
/// Service to retrieve authenticated user profile information from Autodesk
/// This service is called after successful SSO login to get user details
/// The profile information is displayed in the Swagger UI (name, email, picture)
/// </summary>
public class UserInfoService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserInfoService> _logger;

    public UserInfoService(HttpClient httpClient, ILogger<UserInfoService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Get user profile information using the 3-legged access token
    /// This endpoint requires a user token (not a 2-legged app token)
    /// </summary>
    /// <param name="accessToken">3-legged OAuth access token (from SSO login)</param>
    /// <returns>User profile with name, email, picture, etc.</returns>
    public async Task<UserProfile?> GetUserProfileAsync(string accessToken)
    {
        try
        {
            // Create request to Autodesk UserProfile API
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.userprofile.autodesk.com/userinfo");
            
            // Add the user's access token to the request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the request to Autodesk
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Read and deserialize the JSON response
            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserProfile>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allow flexible JSON property matching
            });

            return userInfo;
        }
        catch (Exception ex)
        {
            // Log the error but don't crash - return null instead
            _logger.LogError(ex, "Failed to retrieve user profile from Autodesk");
            return null;
        }
    }
}

