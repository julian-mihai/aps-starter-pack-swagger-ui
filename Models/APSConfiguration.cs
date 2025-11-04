namespace APSStarterPackSwaggerUI.Models;

/// <summary>
/// Configuration class for APS (Autodesk Platform Services) credentials
/// These values are loaded from appsettings.json when the app starts
/// Think of this as the "ID card" your app uses to talk to Autodesk APIs
/// </summary>
public class APSConfiguration
{
    /// <summary>
    /// Your APS app's Client ID (like a username)
    /// Get this from https://aps.autodesk.com/ after creating an app
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Your APS app's Client Secret (like a password) - KEEP THIS SECURE!
    /// Never commit this to source control or share it publicly
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// URL where Autodesk sends users after successful login (OAuth callback)
    /// Example: http://localhost:8080/auth/callback
    /// Must match EXACTLY what's configured in your APS app settings
    /// </summary>
    public string CallbackUrl { get; set; } = string.Empty;

    /// <summary>
    /// Base URL for all Autodesk APS API calls
    /// Default: https://developer.api.autodesk.com
    /// </summary>
    public string BaseUrl { get; set; } = "https://developer.api.autodesk.com";

    /// <summary>
    /// OAuth 2.0 authorization endpoint (where users go to login)
    /// Used for 3-legged authentication (SSO)
    /// </summary>
    public string OAuth2AuthorizeUrl { get; set; } = "https://developer.api.autodesk.com/authentication/v2/authorize";

    /// <summary>
    /// OAuth 2.0 token endpoint (where you exchange credentials for tokens)
    /// Used for both 2-legged (app) and 3-legged (user) authentication
    /// </summary>
    public string OAuth2GetTokenUrl { get; set; } = "https://developer.api.autodesk.com/authentication/v2/token";
}

