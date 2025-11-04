namespace APSStarterPackSwaggerUI.Middleware;

/// <summary>
/// Security Headers Middleware: Adds HTTP security headers to all responses
/// 
/// WHAT IT DOES:
/// - Adds 6 essential security headers to protect against common web vulnerabilities
/// - Runs for every HTTP response before it's sent to the browser
/// - Helps protect users from XSS, clickjacking, MIME sniffing, and other attacks
/// 
/// WHY WE NEED THIS:
/// - Required for production deployments (especially Azure)
/// - Protects against common security vulnerabilities
/// - Industry best practice for web applications
/// - Helps pass security audits and compliance requirements
/// 
/// HOW IT WORKS:
/// - Middleware intercepts the response pipeline
/// - Adds security headers before response is sent to browser
/// - Browser enforces these security policies
/// 
/// WHEN TO USE:
/// - Always enabled for production (Azure)
/// - Can be disabled for local development if needed
/// - Required for enterprise deployments
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;
    private readonly bool _isDevelopment;

    public SecurityHeadersMiddleware(
        RequestDelegate next, 
        ILogger<SecurityHeadersMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _isDevelopment = env.IsDevelopment();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers before processing the request
        AddSecurityHeaders(context);
        
        // Continue to next middleware
        await _next(context);
    }

    private void AddSecurityHeaders(HttpContext context)
    {
        var headers = context.Response.Headers;

        // ============================================
        // 1. STRICT-TRANSPORT-SECURITY (HSTS)
        // ============================================
        // WHAT: Forces browsers to use HTTPS only (no HTTP)
        // WHY: Prevents man-in-the-middle attacks by ensuring all traffic is encrypted
        // WHEN: Always in production, Azure handles HTTPS
        // 
        // max-age=31536000 = Enforce HTTPS for 1 year
        // includeSubDomains = Apply to all subdomains
        if (!_isDevelopment)
        {
            headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
        }

        // ============================================
        // 2. CONTENT-SECURITY-POLICY (CSP)
        // ============================================
        // WHAT: Controls which resources the browser can load (scripts, styles, images)
        // WHY: Prevents Cross-Site Scripting (XSS) attacks
        // NOTE: Relaxed for Swagger UI to function properly
        //
        // default-src 'self' = Only load resources from same origin
        // script-src = Allow inline scripts (Swagger needs this)
        // style-src = Allow inline styles (Swagger needs this)
        // img-src = Allow images from HTTPS and data URIs
        headers["Content-Security-Policy"] = 
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +  // Swagger requires inline scripts
            "style-src 'self' 'unsafe-inline'; " +                  // Swagger requires inline styles
            "img-src 'self' data: https:; " +                       // Allow images from secure sources
            "font-src 'self' data:; " +                             // Allow fonts
            "connect-src 'self' https://developer.api.autodesk.com https://api.userprofile.autodesk.com;"; // Allow APS API calls

        // ============================================
        // 3. X-FRAME-OPTIONS
        // ============================================
        // WHAT: Prevents your site from being embedded in an iframe
        // WHY: Protects against clickjacking attacks
        // OPTIONS: DENY (never allow), SAMEORIGIN (allow same domain)
        //
        // DENY = Never allow this site to be framed
        headers["X-Frame-Options"] = "DENY";

        // ============================================
        // 4. X-CONTENT-TYPE-OPTIONS
        // ============================================
        // WHAT: Prevents browsers from "guessing" content types
        // WHY: Stops MIME sniffing attacks
        //
        // nosniff = Browser must respect the Content-Type header
        // Example: If server says "text/html", browser won't interpret it as JavaScript
        headers["X-Content-Type-Options"] = "nosniff";

        // ============================================
        // 5. REFERRER-POLICY
        // ============================================
        // WHAT: Controls how much referrer information is sent when navigating away
        // WHY: Protects user privacy and prevents information leakage
        //
        // strict-origin-when-cross-origin = Send full URL for same-origin, only origin for cross-origin
        // This balances security and analytics needs
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // ============================================
        // 6. PERMISSIONS-POLICY (formerly Feature-Policy)
        // ============================================
        // WHAT: Controls which browser features and APIs can be used
        // WHY: Reduces attack surface by disabling unnecessary features
        //
        // Disable features we don't use: geolocation, microphone, camera, payment
        // This prevents malicious scripts from accessing these features
        headers["Permissions-Policy"] = 
            "geolocation=(), " +      // Disable geolocation
            "microphone=(), " +       // Disable microphone
            "camera=(), " +           // Disable camera
            "payment=(), " +          // Disable payment APIs
            "usb=(), " +              // Disable USB access
            "magnetometer=()";        // Disable magnetometer

        // ============================================
        // BONUS: X-XSS-PROTECTION (Legacy, but still useful)
        // ============================================
        // WHAT: Enables browser's built-in XSS filter
        // WHY: Extra layer of protection for older browsers
        // NOTE: Modern browsers use CSP instead, but this helps legacy browsers
        //
        // 1; mode=block = Enable XSS filter and block the page if attack detected
        headers["X-XSS-Protection"] = "1; mode=block";

        // Log that security headers were added (useful for debugging)
        if (_isDevelopment)
        {
            _logger.LogDebug("Security headers added to response");
        }
    }
}

/// <summary>
/// Extension method to easily add SecurityHeadersMiddleware to the pipeline
/// This makes it cleaner to use in Program.cs
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    /// <summary>
    /// Adds security headers middleware to the application pipeline
    /// 
    /// USAGE in Program.cs:
    /// app.UseSecurityHeaders(); // Add this before app.Run()
    /// 
    /// IMPORTANT: Add this EARLY in the middleware pipeline
    /// It should run before most other middleware to ensure headers are set
    /// </summary>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SecurityHeadersMiddleware>();
    }
}

