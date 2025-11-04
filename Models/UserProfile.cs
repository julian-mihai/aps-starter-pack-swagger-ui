using System.Text.Json.Serialization;

namespace APSStarterPackSwaggerUI.Models;

/// <summary>
/// User profile information from Autodesk UserProfile API
/// This is returned after a successful 3-legged (SSO) login
/// Used to display user's name, email, and picture in the UI
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Unique Autodesk user identifier (subject)
    /// Example: "abc123xyz"
    /// </summary>
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the user
    /// Example: "John Doe"
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// First name
    /// Example: "John"
    /// </summary>
    [JsonPropertyName("given_name")]
    public string GivenName { get; set; } = string.Empty;

    /// <summary>
    /// Last name
    /// Example: "Doe"
    /// </summary>
    [JsonPropertyName("family_name")]
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    /// Preferred username (usually email)
    /// Example: "john.doe@company.com"
    /// </summary>
    [JsonPropertyName("preferred_username")]
    public string PreferredUsername { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// Example: "john.doe@company.com"
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether the email has been verified by Autodesk
    /// </summary>
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    /// <summary>
    /// URL to user's Autodesk profile page
    /// </summary>
    [JsonPropertyName("profile")]
    public string Profile { get; set; } = string.Empty;

    /// <summary>
    /// URL to user's profile picture/avatar
    /// Used to display in the Swagger UI banner
    /// </summary>
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = string.Empty;

    /// <summary>
    /// User's locale/language preference
    /// Example: "en-US"
    /// </summary>
    [JsonPropertyName("locale")]
    public string Locale { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of last profile update (Unix epoch)
    /// </summary>
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }
}

