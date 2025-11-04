using APSStarterPackSwaggerUI.Services;
using APSStarterPackSwaggerUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Account Users Controller - Retrieve account-level users from ACC
/// </summary>
[ApiController]
[Route("users")]
[Tags("5. Account Users 👥 ACC API")]
public class AccountUsersController : ControllerBase
{
    private readonly AccountUsersService _accountUsersService;

    public AccountUsersController(AccountUsersService accountUsersService)
    {
        _accountUsersService = accountUsersService;
    }

    /// <summary>
    /// Get All Users in an Account
    /// </summary>
    /// <remarks>
    /// **ACC API** - Retrieves all users in an ACC or BIM 360 account.
    /// 
    /// **What are Account Users?**
    /// These are all users who have been invited to your ACC/BIM 360 account, regardless of which projects they're assigned to.
    /// 
    /// **Requirements:**
    /// - Token with `account:read` scope
    /// - Account ID without "b." prefix (auto-removed if present)
    /// - Correct region (US or EMEA)
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID (hubId without "b." prefix)</param>
    /// <param name="region">Region: US or EMEA (default: US)</param>
    /// <response code="200">Returns list of users</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string token,
        [FromQuery] string accountId,
        [FromQuery] Region region = Region.US)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { error = "Token is required", tip = "Get a token from /auth/token" });
        }

        if (string.IsNullOrEmpty(accountId))
        {
            return BadRequest(new { error = "AccountId is required", tip = "Use hubId from /hubs, remove 'b.' prefix" });
        }

        // Auto-fix: Remove "b." prefix if user included it
        if (accountId.StartsWith("b."))
        {
            accountId = accountId.Substring(2);
        }

        try
        {
            var users = await _accountUsersService.GetUsersAsync(token, accountId, region.ToString());
            return Ok(users);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve users from APS API",
                details = ex.Message,
                tip = "Make sure your token has 'account:read' scope and the accountId is correct"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a Specific User by ID
    /// </summary>
    /// <remarks>
    /// **ACC API** - Retrieves detailed information about a specific user.
    /// 
    /// **What does this return?**
    /// - User's email
    /// - Name
    /// - Status (active/inactive)
    /// - Roles
    /// - Company association
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID</param>
    /// <param name="userId">User ID</param>
    /// <param name="region">Region: US or EMEA (default: US)</param>
    /// <response code="200">Returns user details</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(
        [FromQuery] string token,
        [FromQuery] string accountId,
        string userId,
        [FromQuery] Region region = Region.US)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(accountId))
        {
            return BadRequest(new { error = "Token and AccountId are required" });
        }

        // Auto-fix: Remove "b." prefix if user included it
        if (accountId.StartsWith("b."))
        {
            accountId = accountId.Substring(2);
        }

        try
        {
            var user = await _accountUsersService.GetUserByIdAsync(token, accountId, userId, region.ToString());
            return Ok(user);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve user from APS API",
                details = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Search for Users by Email
    /// </summary>
    /// <remarks>
    /// **ACC API** - Search for users in an account by their email address.
    /// 
    /// **Use Case:**
    /// Find a user's ID when you only know their email address.
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID</param>
    /// <param name="email">Email address to search for</param>
    /// <param name="region">Region: US or EMEA (default: US)</param>
    /// <response code="200">Returns matching users</response>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string token,
        [FromQuery] string accountId,
        [FromQuery] string email,
        [FromQuery] Region region = Region.US)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(email))
        {
            return BadRequest(new { error = "Token, AccountId, and Email are required" });
        }

        // Auto-fix: Remove "b." prefix if user included it
        if (accountId.StartsWith("b."))
        {
            accountId = accountId.Substring(2);
        }

        try
        {
            var users = await _accountUsersService.SearchUsersAsync(token, accountId, email, region.ToString());
            return Ok(users);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to search users from APS API",
                details = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

