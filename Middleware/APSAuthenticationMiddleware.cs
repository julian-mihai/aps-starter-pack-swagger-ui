namespace APSStarterPackSwaggerUI.Middleware;

/// <summary>
/// Custom Middleware: Ensures users are authenticated via Autodesk SSO before accessing protected resources
/// 
/// WHAT IT DOES:
/// - Intercepts every HTTP request before it reaches controllers or Swagger UI
/// - Checks if the user has a valid access token stored in their session
/// - Redirects unauthenticated users to the login page
/// - Allows public routes (login page, auth endpoints, static files) to pass through
/// 
/// WHY WE NEED THIS:
/// - Protects Swagger UI and API endpoints from unauthorized access
/// - Implements enterprise-grade security (SSO requirement)
/// - Provides better user experience than manual token management
/// 
/// HOW IT WORKS:
/// 1. Request comes in → Middleware checks session
/// 2. No token? → Redirect to /login.html
/// 3. Token expired? → Clear session and redirect to login
/// 4. Token valid? → Allow request to continue to Swagger/APIs
/// </summary>
public class APSAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<APSAuthenticationMiddleware> _logger;
    
    /// <summary>
    /// List of paths that anyone can access without authentication
    /// These are needed for the login flow to work
    /// </summary>
    private static readonly HashSet<string> PublicPaths = new()
    {
        "/auth/login",     // SSO login initiation
        "/auth/callback",  // OAuth callback from Autodesk
        "/login.html",     // Login page UI
        "/health",         // Health check endpoint
        "/favicon.ico"     // Browser icon
    };

    /// <summary>
    /// Constructor: Called once when app starts
    /// </summary>
    /// <param name="next">Next middleware in the pipeline (Dependency Injection)</param>
    /// <param name="logger">Logger for debugging and monitoring</param>
    public APSAuthenticationMiddleware(
        RequestDelegate next,
        ILogger<APSAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// InvokeAsync: Called for EVERY HTTP request
    /// This is where the authentication check happens
    /// </summary>
    /// <param name="context">Information about the current HTTP request</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Get the requested path (e.g., "/", "/help.html", "/hubs")
        var path = context.Request.Path.Value?.ToLower() ?? "";
        
        // STEP 1: Check if this is a public path that doesn't need authentication
        // Examples: /login.html, /auth/callback, /css/style.css
        if (PublicPaths.Any(p => path.StartsWith(p)) || 
            path.StartsWith("/css/") || 
            path.StartsWith("/js/") ||
            path.StartsWith("/swagger")) // Allow Swagger JSON to be read
        {
            // This is a public route - allow it to continue without checking authentication
            await _next(context);
            return;
        }
        
        // STEP 2: Check if user has an access token in their session
        // Session = server-side storage that persists across requests for the same user
        var token = context.Session.GetString("APS_AccessToken");
        var expiresAt = context.Session.GetString("APS_ExpiresAt");
        
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiresAt))
        {
            // No token found - user hasn't logged in yet
            _logger.LogInformation("User not authenticated, redirecting to login");
            context.Response.Redirect("/login.html");
            return; // Stop processing - don't call next middleware
        }
        
        // STEP 3: Check if the token has expired
        // Tokens typically expire after 1 hour (3600 seconds)
        if (DateTime.TryParse(expiresAt, out var expires) && expires < DateTime.UtcNow)
        {
            // Token expired - user needs to login again
            _logger.LogInformation("Token expired, redirecting to login");
            context.Session.Clear(); // Clear all session data
            context.Response.Redirect("/login.html");
            return;
        }
        
        // STEP 4: User is authenticated with a valid token - allow the request to continue
        // The request will now reach Swagger UI or the API controllers
        await _next(context);
    }
}

