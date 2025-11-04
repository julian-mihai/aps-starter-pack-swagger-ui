using APSStarterPackSwaggerUI.Services;
using APSStarterPackSwaggerUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Companies Controller - Retrieve companies from ACC/BIM 360
/// </summary>
[ApiController]
[Route("companies")]
[Tags("7. Companies 🏢 ACC API")]
public class CompaniesController : ControllerBase
{
    private readonly CompaniesService _companiesService;

    public CompaniesController(CompaniesService companiesService)
    {
        _companiesService = companiesService;
    }

    /// <summary>
    /// Get All Companies in an Account
    /// </summary>
    /// <remarks>
    /// **ACC API** - Retrieves all companies in an ACC or BIM 360 account.
    /// 
    /// **What is a Company?**
    /// Companies represent organizations that participate in projects (contractors, architects, etc.).
    /// 
    /// **Account ID vs Hub ID:**
    /// - Hub ID: "b.12345-67890" (with "b." prefix)
    /// - Account ID: "12345-67890" (without "b." prefix)
    /// 
    /// **Steps:**
    /// 1. Get hubId from /hubs (e.g., "b.3def0ac0-6276...")
    /// 2. Remove "b." prefix → "3def0ac0-6276..."
    /// 3. Use that as accountId here
    /// 
    /// **Note:** This endpoint automatically strips "b." if you accidentally include it!
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID (hubId without "b." prefix)</param>
    /// <param name="region">Region: US or EMEA (default: US)</param>
    /// <response code="200">Returns list of companies</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompanies(
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
            var companies = await _companiesService.GetCompaniesAsync(token, accountId, region.ToString());
            return Ok(companies);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve companies from APS API",
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
    /// Get Companies for a Specific Project
    /// </summary>
    /// <remarks>
    /// **ACC API** - Retrieves companies that have access to a specific project.
    /// 
    /// This is useful for setting up permissions - you need company IDs to grant folder access.
    /// 
    /// **Note:** This endpoint automatically strips "b." prefix from both accountId and projectId!
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="accountId">Account ID (without "b." prefix)</param>
    /// <param name="projectId">Project ID</param>
    /// <param name="region">Region: US or EMEA (default: US)</param>
    /// <response code="200">Returns list of companies with access to the project</response>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectCompanies(
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
            var companies = await _companiesService.GetProjectCompaniesAsync(token, accountId, projectId, region.ToString());
            return Ok(companies);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve project companies from APS API",
                details = ex.Message,
                tip = "Make sure the project exists and your token has access to it"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

