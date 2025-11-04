using APSStarterPackSwaggerUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Authentication Controller - Handles OAuth authentication with APS
/// </summary>
[ApiController]
[Route("auth")]
[Tags("1. Authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authService;
    private readonly UserInfoService _userInfoService;

    public AuthenticationController(AuthenticationService authService, UserInfoService userInfoService)
    {
        _authService = authService;
        _userInfoService = userInfoService;
    }

    /// <summary>
    /// Get 2-Legged Token (APS APP - ClientID + ClientSecret)
    /// </summary>
    /// <remarks>
    /// This is the simplest way to authenticate. Use this token for server-to-server API calls.
    /// 
    /// **How it works:**
    /// 1. Your app (ClientId + ClientSecret) requests a token
    /// 2. APS validates and returns an access token
    /// 3. Use this token in the "Authorization: Bearer {token}" header for other API calls
    /// 
    /// **When to use:** Server-side operations that don't require user login
    /// </remarks>
    /// <param name="scope">Permissions requested (default: data:read data:write data:create)</param>
    /// <response code="200">Returns the access token and expiration time</response>
    [HttpGet("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get2LeggedToken([FromQuery] string scope = "data:read data:write data:create data:search account:read")
    {
        try
        {
            var token = await _authService.GetTwoLeggedTokenAsync(scope);
            return Ok(new
            {
                access_token = token.AccessToken,
                token_type = token.TokenType,
                expires_in = token.ExpiresIn,
                message = "✅ Token retrieved successfully! Copy the access_token and use it in other endpoints."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, tip = "Check your ClientId and ClientSecret in appsettings.json" });
        }
    }

    /// <summary>
    /// Get 3-Legged Token (User Login - Authorization Code)
    /// </summary>
    /// <remarks>
    /// Use this when you need a user to login with their Autodesk account.
    /// 
    /// **How it works:**
    /// 1. Generate an authorization URL
    /// 2. Redirect the user to this URL
    /// 3. User logs in with their Autodesk credentials
    /// 4. APS redirects back to your callback URL with an authorization code
    /// 5. Exchange the code for a token using the /api/auth/callback endpoint
    /// 
    /// **When to use:** When you need to access user-specific data or perform actions on behalf of a user
    /// </remarks>
    /// <param name="scope">Permissions requested</param>
    /// <param name="returnUrl">URL to redirect to after login</param>
    /// <response code="302">Redirects to Autodesk login page</response>
    [HttpGet("login")]
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger UI
    [ProducesResponseType(StatusCodes.Status302Found)]
    public IActionResult Get3LeggedToken(
        [FromQuery] string scope = "data:read data:write data:create data:search account:read",
        [FromQuery] string? returnUrl = "/")
    {
        // Store return URL in session
        HttpContext.Session.SetString("ReturnUrl", returnUrl ?? "/");
        
        // Generate Autodesk OAuth URL
        var authUrl = _authService.GetThreeLeggedAuthorizationUrl(scope);
        
        // Redirect to Autodesk login
        return Redirect(authUrl);
    }

    /// <summary>
    /// Exchange Authorization Code for Token (3-Legged OAuth Callback)
    /// </summary>
    /// <remarks>
    /// After a user logs in using the URL from /api/auth/login, they'll be redirected back with a code.
    /// Use this endpoint to exchange that code for an access token.
    /// 
    /// **Note:** This endpoint is hidden from Swagger UI as it's meant to be called automatically by the OAuth redirect flow.
    /// </remarks>
    /// <param name="code">The authorization code received from the callback</param>
    /// <response code="302">Redirects to the original URL after successful login</response>
    [HttpGet("callback")]
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger UI
    [ProducesResponseType(StatusCodes.Status302Found)]
    public async Task<IActionResult> HandleCallback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest(new { error = "Authorization code is required" });
        }

        try
        {
            // Exchange code for access token
            var token = await _authService.GetThreeLeggedTokenAsync(code);
            
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                return BadRequest(new { error = "Failed to obtain access token" });
            }

            // Store token in session
            HttpContext.Session.SetString("APS_AccessToken", token.AccessToken);
            
            if (!string.IsNullOrEmpty(token.RefreshToken))
            {
                HttpContext.Session.SetString("APS_RefreshToken", token.RefreshToken);
            }
            
            // Calculate expiration time
            var expiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
            HttpContext.Session.SetString("APS_ExpiresAt", expiresAt.ToString("O"));
            
            // Fetch and store user profile information
            var userProfile = await _userInfoService.GetUserProfileAsync(token.AccessToken);
            if (userProfile != null)
            {
                HttpContext.Session.SetString("User_Name", userProfile.Name ?? userProfile.PreferredUsername ?? "User");
                HttpContext.Session.SetString("User_Email", userProfile.Email ?? "");
                if (!string.IsNullOrEmpty(userProfile.Picture))
                {
                    HttpContext.Session.SetString("User_Picture", userProfile.Picture);
                }
            }
            
            // Redirect to original URL or home
            var returnUrl = HttpContext.Session.GetString("ReturnUrl") ?? "/";
            return Redirect(returnUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Authentication failed: {ex.Message}" });
        }
    }

    /// <summary>
    /// Logout and clear session
    /// </summary>
    [HttpGet("logout")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Redirect("/login.html");
    }

    /// <summary>
    /// Get current user's token (3-Legged Token)
    /// </summary>
    [HttpGet("current-token")]
    public IActionResult GetCurrentToken()
    {
        var token = HttpContext.Session.GetString("APS_AccessToken");
        var expiresAt = HttpContext.Session.GetString("APS_ExpiresAt");
        
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Not authenticated" });
        }
        
        return Ok(new
        {
            access_token = token,
            expires_at = expiresAt,
            message = "✅ Copy this token to use in Swagger UI endpoints if needed"
        });
    }

    /// <summary>
    /// Get current user information (name, email, picture)
    /// </summary>
    [HttpGet("current-user")]
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger UI
    public IActionResult GetCurrentUser()
    {
        var name = HttpContext.Session.GetString("User_Name");
        var email = HttpContext.Session.GetString("User_Email");
        var picture = HttpContext.Session.GetString("User_Picture");
        
        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email))
        {
            return Unauthorized(new { message = "Not authenticated" });
        }
        
        return Ok(new
        {
            name = name ?? "User",
            email = email ?? "",
            picture = picture ?? ""
        });
    }
}

