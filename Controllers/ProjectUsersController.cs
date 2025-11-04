using APSStarterPackSwaggerUI.Services;
using APSStarterPackSwaggerUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Project Users Controller - Retrieve project-level users from ACC
/// </summary>
[ApiController]
[Route("projects/{projectId}/users")]
[Tags("6. Project Users 👤 ACC API")]
public class ProjectUsersController : ControllerBase
{
    private readonly ProjectUsersService _projectUsersService;

    public ProjectUsersController(ProjectUsersService projectUsersService)
    {
        _projectUsersService = projectUsersService;
    }

    /// <summary>
    /// Get All Users in a Project
    /// </summary>
    /// <remarks>
    /// **ACC Admin API v1** - Retrieves all users assigned to a specific project with their roles, products, and access levels.
    /// 
    /// **What are Project Users?**
    /// Users who have been specifically assigned to this project. The response includes:
    /// - User ID, email, name
    /// - Assigned roles (e.g., "Project Administrator", "Member")
    /// - Company assignment
    /// - Products they can access (Docs, Design Collaboration, etc.)
    /// - Status (active/inactive)
    /// 
    /// **Difference from Account Users:**
    /// - Account Users (`GET /users`): All users in the entire account
    /// - Project Users: Only users assigned to a specific project
    /// 
    /// **Requirements:**
    /// - Token with `account:read` scope
    /// - Project ID WITHOUT "b." prefix (will be auto-removed if included)
    /// 
    /// **Note:** accountId and region parameters are not needed for this endpoint (they're legacy parameters that will be ignored)
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">[NOT USED - Legacy parameter] Account ID</param>
    /// <param name="projectId">Project ID (e.g., "d65e0826-131f...")</param>
    /// <param name="region">[NOT USED - Legacy parameter] Region</param>
    /// <response code="200">Returns list of project users with their roles and access</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectUsers(
        [FromQuery] string token,
        [FromQuery] string accountId,
        string projectId,
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
            var users = await _projectUsersService.GetProjectUsersAsync(token, accountId, projectId, region.ToString());
            return Ok(users);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve project users from APS API",
                details = ex.Message,
                tip = "Make sure the project exists and your token has access to it"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a Specific User in a Project
    /// </summary>
    /// <remarks>
    /// **ACC Admin API v1** - Retrieves detailed information about a specific user in a project.
    /// 
    /// **What does this return?**
    /// - User's project-specific role and permissions
    /// - Access level
    /// - Company assignment in this project
    /// - Products they can access
    /// - Status and last login information
    /// 
    /// **Requirements:**
    /// - Token with `account:read` scope
    /// - Project ID WITHOUT "b." prefix (will be auto-removed if included)
    /// - User ID (can be found from `GET /projects/{projectId}/users`)
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">[NOT USED - Legacy parameter] Account ID</param>
    /// <param name="projectId">Project ID</param>
    /// <param name="userId">User ID</param>
    /// <param name="region">[NOT USED - Legacy parameter] Region</param>
    /// <response code="200">Returns user details in project</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectUserById(
        [FromQuery] string token,
        [FromQuery] string accountId,
        string projectId,
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
            var user = await _projectUsersService.GetProjectUserByIdAsync(token, accountId, projectId, userId, region.ToString());
            return Ok(user);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve project user from APS API",
                details = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

