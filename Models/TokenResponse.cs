using System.Text.Json.Serialization;

namespace APSStarterPackSwaggerUI.Models;

/// <summary>
/// Response from APS OAuth token endpoint (authentication v2/token)
/// This is what you receive after successfully authenticating with Autodesk
/// The AccessToken is what you use to make API calls
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// The actual access token string - this is your "key" to call APS APIs
    /// Example: "eyJhbGciOiJSUzI1NiIsImtpZCI6..."
    /// Include this in API requests as: Authorization: Bearer {AccessToken}
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type - usually "Bearer"
    /// This tells you how to use the token in HTTP headers
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    /// <summary>
    /// How long the token is valid (in seconds)
    /// Typical value: 3600 (1 hour)
    /// After this time, the token expires and you need to get a new one
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Refresh token (only for 3-legged OAuth)
    /// Use this to get a new access token without requiring the user to login again
    /// Not provided for 2-legged tokens
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}

