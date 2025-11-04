namespace APSStarterPackSwaggerUI.Models;

/// <summary>
/// Configuration for email whitelist authorization
/// 
/// WHAT: Controls which users can access the application after SSO login
/// WHY: Adds an extra security layer beyond Autodesk SSO authentication
/// HOW: Checks user's email against a whitelist before granting access
/// 
/// SUPPORTS TWO FORMATS:
/// 
/// FORMAT 1: Array (in appsettings.json)
///   "AllowedEmails": ["email1@company.com", "email2@company.com"]
/// 
/// FORMAT 2: Comma-separated string (perfect for Azure App Settings)
///   AuthorizedUsers__AllowedEmails = "email1@company.com, email2@company.com, email3@company.com"
/// 
/// Use Format 2 in Azure - much easier than multiple indexed entries!
/// </summary>
public class AuthorizedUsersConfiguration
{
    private List<string> _allowedEmails = new();
    
    /// <summary>
    /// List of authorized email addresses (array format OR comma-separated string)
    /// 
    /// WHAT: Emails that are allowed to access the application
    /// WHY: Only trusted users should access your APS data
    /// HOW: Case-insensitive matching (john@email.com = John@Email.com)
    /// 
    /// SUPPORTS WILDCARDS:
    /// - "*@autodesk.com" allows all Autodesk emails
    /// - "*@company.com" allows all company domain emails
    /// - "john.doe@gmail.com" allows specific user
    /// 
    /// SUPPORTS TWO FORMATS:
    /// 
    /// FORMAT 1 (appsettings.json - Array):
    /// "AllowedEmails": ["email1@company.com", "email2@company.com"]
    /// 
    /// FORMAT 2 (Azure App Settings - Comma-separated string):
    /// AuthorizedUsers__AllowedEmails = "email1@company.com, email2@company.com, email3@company.com"
    /// 
    /// This property is SMART - it detects if you're passing a single string with commas
    /// and automatically parses it! Works with BOTH Azure naming conventions!
    /// </summary>
    public List<string> AllowedEmails 
    { 
        get => _allowedEmails;
        set
        {
            if (value != null && value.Count == 1 && (value[0].Contains(',') || value[0].Contains(';')))
            {
                // Azure is passing a single string with comma-separated emails
                // Parse it automatically!
                ParseAndAddEmails(value[0]);
            }
            else
            {
                // Normal array assignment
                _allowedEmails = value ?? new();
            }
        }
    }
    
    /// <summary>
    /// Comma-separated list of authorized email addresses (string format)
    /// 
    /// WHAT: Alternative to AllowedEmails array - much simpler for Azure!
    /// WHY: Azure App Settings work better with single string values
    /// HOW: Automatically parses comma-separated emails into AllowedEmails list
    /// 
    /// PERFECT FOR AZURE APP SETTINGS:
    /// AuthorizedUsers__AllowedEmailsString = "iulian@autodesk.com, admin1@autodesk.com, admin2@company.com"
    /// 
    /// BENEFITS:
    /// - ✅ Single setting instead of multiple __0, __1, __2 entries
    /// - ✅ Easy to copy/paste
    /// - ✅ Easy to add/remove emails
    /// - ✅ Perfect for non-IT users
    /// 
    /// NOTE: If both AllowedEmails (array) and AllowedEmailsString are provided,
    ///       this will ADD to the existing emails (not replace them)
    /// </summary>
    public string? AllowedEmailsString
    {
        get => _allowedEmails.Any() ? string.Join(", ", _allowedEmails) : null;
        set => ParseAndAddEmails(value);
    }
    
    /// <summary>
    /// Helper method to parse comma-separated emails
    /// Used internally by both AllowedEmails setter and AllowedEmailsString setter
    /// </summary>
    private void ParseAndAddEmails(string? emailString)
    {
        if (!string.IsNullOrWhiteSpace(emailString))
        {
            // Check if it's a comma-separated string (simple heuristic: contains comma or semicolon)
            if (emailString.Contains(',') || emailString.Contains(';'))
            {
                // Parse comma or semicolon-separated string into individual emails
                var separators = new[] { ',', ';' };
                var emails = emailString.Split(separators)
                                      .Select(e => e.Trim())
                                      .Where(e => !string.IsNullOrWhiteSpace(e))
                                      .ToList();
                
                // Add to existing list (don't replace, in case both formats are used)
                foreach (var email in emails)
                {
                    if (!_allowedEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
                    {
                        _allowedEmails.Add(email);
                    }
                }
            }
            else
            {
                // Single email - add it directly
                var email = emailString.Trim();
                if (!string.IsNullOrWhiteSpace(email) && 
                    !_allowedEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
                {
                    _allowedEmails.Add(email);
                }
            }
        }
    }

    /// <summary>
    /// Enable or disable the whitelist feature
    /// 
    /// WHAT: Toggle the entire whitelist security feature
    /// WHY: Easy to disable for development/testing without removing emails
    /// HOW: Set to false to allow all authenticated users
    /// 
    /// DEFAULT: false (disabled for backward compatibility)
    /// PRODUCTION: Set to true in Azure App Settings
    /// </summary>
    public bool EnableWhitelist { get; set; } = false;

    /// <summary>
    /// Checks if an email is authorized to access the application
    /// </summary>
    /// <param name="email">The email address to check</param>
    /// <returns>True if authorized, false otherwise</returns>
    public bool IsEmailAuthorized(string email)
    {
        // If whitelist is disabled, allow everyone
        if (!EnableWhitelist)
        {
            return true;
        }

        // If email is null or empty, deny access
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        // If whitelist is empty but enabled, deny all (safest default)
        if (!AllowedEmails.Any())
        {
            return false;
        }

        // Normalize email to lowercase for case-insensitive comparison
        var normalizedEmail = email.Trim().ToLowerInvariant();

        // Check each allowed email/pattern
        foreach (var allowedEmail in AllowedEmails)
        {
            var normalizedAllowed = allowedEmail.Trim().ToLowerInvariant();

            // Exact match
            if (normalizedEmail == normalizedAllowed)
            {
                return true;
            }

            // Wildcard domain match (e.g., *@autodesk.com)
            if (normalizedAllowed.StartsWith("*@"))
            {
                var allowedDomain = normalizedAllowed.Substring(1); // Remove *
                if (normalizedEmail.EndsWith(allowedDomain))
                {
                    return true;
                }
            }
        }

        // No match found
        return false;
    }
}

