using Microsoft.Extensions.Options;
using APSStarterPackSwaggerUI.Models;

namespace APSStarterPackSwaggerUI.Middleware;

/// <summary>
/// Custom Middleware: Email Whitelist Authorization
/// 
/// WHAT: Checks if authenticated user's email is on the authorized list
/// WHY: Adds security layer - only specific users can access the app
/// HOW: Runs AFTER APSAuthenticationMiddleware, checks session email against whitelist
/// 
/// FLOW:
/// 1. User already authenticated via Autodesk SSO (APSAuthenticationMiddleware)
/// 2. This middleware reads user email from session
/// 3. Checks if email is in AllowedEmails list
/// 4. IF authorized → continue to Swagger UI
/// 5. IF NOT authorized → redirect to /access-denied.html
/// 
/// CONFIGURATION (appsettings.json):
/// "AuthorizedUsers": {
///   "AllowedEmails": ["user@company.com", "*@autodesk.com"],
///   "EnableWhitelist": true
/// }
/// 
/// FEATURES:
/// - Case-insensitive email matching
/// - Wildcard domain support (*@autodesk.com)
/// - Toggle on/off via EnableWhitelist flag
/// - Logs unauthorized access attempts
/// - Allows public routes (login, auth callbacks, static files)
/// </summary>
public class EmailWhitelistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EmailWhitelistMiddleware> _logger;
    private readonly AuthorizedUsersConfiguration _authorizedUsers;

    public EmailWhitelistMiddleware(
        RequestDelegate next,
        ILogger<EmailWhitelistMiddleware> logger,
        IOptions<AuthorizedUsersConfiguration> authorizedUsers)
    {
        _next = next;
        _logger = logger;
        _authorizedUsers = authorizedUsers.Value;
        
        // DEBUG: Log configuration on startup
        _logger.LogInformation("📧 Email Whitelist Configuration:");
        _logger.LogInformation("   EnableWhitelist: {Enabled}", _authorizedUsers.EnableWhitelist);
        _logger.LogInformation("   Allowed Emails Count: {Count}", _authorizedUsers.AllowedEmails.Count);
        foreach (var email in _authorizedUsers.AllowedEmails)
        {
            _logger.LogInformation("   ✅ Allowed: {Email}", email);
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        
        _logger.LogInformation("🔍 EmailWhitelistMiddleware: Checking path {Path}", path);

        // ========================================
        // ALLOW PUBLIC ROUTES (no whitelist check)
        // ========================================
        // These routes should be accessible to everyone:
        // - Login page
        // - Auth callbacks (OAuth flow)
        // - Access denied page
        // - Static files (CSS, JS, images)
        // - Swagger specification (needed for UI)
        if (path.StartsWithSegments("/login.html") ||
            path.StartsWithSegments("/auth/") ||
            path.StartsWithSegments("/access-denied.html") ||
            path.StartsWithSegments("/css") ||
            path.StartsWithSegments("/js") ||
            path.StartsWithSegments("/images") ||
            path.StartsWithSegments("/swagger"))
        {
            _logger.LogInformation("✅ Public route - allowing: {Path}", path);
            await _next(context);
            return;
        }

        // ========================================
        // CHECK WHITELIST FOR PROTECTED ROUTES
        // ========================================
        // Protected routes: Swagger UI, API endpoints, help pages
        if (path == "/" || 
            path == "/index.html" ||  // Swagger UI loads this!
            path.StartsWithSegments("/help.html") || 
            path.StartsWithSegments("/about.html") ||
            path.StartsWithSegments("/hubs") ||
            path.StartsWithSegments("/projects") ||
            path.StartsWithSegments("/folders") ||
            path.StartsWithSegments("/users") ||
            path.StartsWithSegments("/companies") ||
            path.StartsWithSegments("/roles") ||
            path.StartsWithSegments("/permissions"))
        {
            _logger.LogInformation("🔒 Protected route detected: {Path}", path);
            
            // If whitelist is disabled, allow all authenticated users
            if (!_authorizedUsers.EnableWhitelist)
            {
                _logger.LogWarning("⚠️ Email whitelist is DISABLED - allowing all authenticated users");
                await _next(context);
                return;
            }
            
            _logger.LogInformation("🔐 Whitelist is ENABLED - checking authorization");

            // Get user email from session
            var userEmail = context.Session.GetString("User_Email");
            
            _logger.LogInformation("📧 User email from session: {Email}", userEmail ?? "(null)");

            if (string.IsNullOrEmpty(userEmail))
            {
                // User is authenticated (has token) but no email in session
                // This shouldn't happen, but handle it gracefully
                _logger.LogWarning("❌ User has valid session but no email stored - denying access to {Path}", path);
                context.Response.Redirect("/access-denied.html");
                return;
            }

            // Check if email is authorized
            bool isAuthorized = _authorizedUsers.IsEmailAuthorized(userEmail);
            _logger.LogInformation("🔍 Authorization check for {Email}: {Result}", userEmail, isAuthorized ? "AUTHORIZED" : "DENIED");
            
            if (isAuthorized)
            {
                // Email is on whitelist - allow access
                _logger.LogInformation("✅ Authorized access: {Email} → {Path}", userEmail, path);
                await _next(context);
                return;
            }
            else
            {
                // Email is NOT on whitelist - deny access
                _logger.LogWarning("❌ UNAUTHORIZED ACCESS ATTEMPT: {Email} tried to access {Path}", userEmail, path);
                
                // Store denial reason in session for display on access-denied page
                context.Session.SetString("Access_Denied_Reason", "Your email address is not authorized to access this application.");
                
                context.Response.Redirect("/access-denied.html");
                return;
            }
        }

        // ========================================
        // ALLOW ALL OTHER ROUTES (fallback)
        // ========================================
        _logger.LogInformation("✅ Other route - allowing: {Path}", path);
        await _next(context);
    }
}

/// <summary>
/// Extension method to easily add EmailWhitelistMiddleware to the pipeline
/// 
/// USAGE in Program.cs:
/// app.UseSession();
/// app.UseMiddleware&lt;APSAuthenticationMiddleware&gt;();
/// app.UseEmailWhitelist(); // Add this line
/// </summary>
public static class EmailWhitelistMiddlewareExtensions
{
    public static IApplicationBuilder UseEmailWhitelist(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EmailWhitelistMiddleware>();
    }
}

