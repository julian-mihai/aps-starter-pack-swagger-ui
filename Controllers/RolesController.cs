using APSStarterPackSwaggerUI.Services;
using APSStarterPackSwaggerUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Roles Controller - Retrieve industry roles from ACC
/// </summary>
[ApiController]
[Route("roles")]
[Tags("8. Industry Roles 🎭 ACC API")]
public class RolesController : ControllerBase
{
    private readonly RolesService _rolesService;

    public RolesController(RolesService rolesService)
    {
        _rolesService = rolesService;
    }

    /// <summary>
    /// Get Industry Roles for a Project
    /// </summary>
    /// <remarks>
    /// **ACC API v2** - Retrieves all available industry roles for a project.
    /// 
    /// **What are Industry Roles?**
    /// Industry roles are predefined role templates that can be assigned to users, such as:
    /// - Architect
    /// - BIM Manager
    /// - Construction Manager
    /// - Document Manager
    /// - Engineer
    /// - Estimator
    /// - Executive
    /// - Foreman
    /// - IT
    /// - Project Engineer
    /// - Project Manager
    /// - Superintendent
    /// - VDC Manager
    /// - And more...
    /// 
    /// **Requirements:**
    /// - Token with `account:read` scope
    /// - Account ID without "b." prefix (auto-removed if present)
    /// - Project ID (can include "b." prefix, will be auto-removed)
    /// 
    /// **Note:** This is different from the "Project Users" endpoint which shows actual users and their assigned roles.
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID (hubId without "b." prefix)</param>
    /// <param name="projectId">Project ID</param>
    /// <response code="200">Returns list of industry roles</response>
    [HttpGet("projects/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectIndustryRoles(
        [FromQuery] string token,
        [FromQuery] string accountId,
        string projectId)
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
            var roles = await _rolesService.GetProjectIndustryRolesAsync(token, accountId, projectId);
            return Ok(roles);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve industry roles from APS API",
                details = ex.Message,
                tip = "Make sure your token has 'account:read' scope and the project exists"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

