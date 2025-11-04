using APSStarterPackSwaggerUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Hubs Controller - Retrieve APS Hubs (BIM 360, ACC accounts)
/// </summary>
[ApiController]
[Route("hubs")]
[Tags("2. Hubs 📁 Data Management API")]
public class HubsController : ControllerBase
{
    private readonly HubsService _hubsService;

    public HubsController(HubsService hubsService)
    {
        _hubsService = hubsService;
    }

    /// <summary>
    /// Get All Hubs
    /// </summary>
    /// <remarks>
    /// Retrieves all hubs (BIM 360 or ACC accounts) that your token has access to.
    /// 
    /// **What is a Hub?**
    /// A hub represents a BIM 360 Team, BIM 360 Docs, ACC, or Fusion Team account.
    /// 
    /// **Steps:**
    /// 1. Get a token from /api/auth/token
    /// 2. Copy the access_token
    /// 3. Paste it in the 'token' field below
    /// 4. Execute to see all hubs you have access to
    /// 
    /// **Response includes:** Hub IDs and names
    /// </remarks>
    /// <param name="token">Access token from /api/auth/token</param>
    /// <response code="200">Returns list of hubs</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHubs([FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { error = "Token is required", tip = "Get a token from /api/auth/token first" });
        }

        try
        {
            var hubs = await _hubsService.GetHubsAsync(token);
            return Ok(hubs);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get Hub by ID
    /// </summary>
    /// <remarks>
    /// Get details of a specific hub.
    /// 
    /// **Steps:**
    /// 1. Use GET /api/hubs to get list of hubs
    /// 2. Copy a hub ID (looks like "b.12345-67890")
    /// 3. Use it here to get hub details
    /// 
    /// **Important:** Hub ID must start with "b." prefix!
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="hubId">⚠️ Hub ID - MUST start with "b." (e.g., "b.3def0ac0-6276...")</param>
    /// <response code="200">Returns hub details</response>
    [HttpGet("{hubId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHub([FromQuery] string token, string hubId)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { error = "Token is required" });
        }

        if (string.IsNullOrEmpty(hubId))
        {
            return BadRequest(new { error = "HubId is required" });
        }

        // Validate hubId format
        if (!hubId.StartsWith("b."))
        {
            return BadRequest(new { 
                error = "Invalid Hub ID format",
                tip = "Hub ID must start with 'b.' (e.g., 'b.3def0ac0-6276...')",
                yourHubId = hubId,
                suggestedFix = $"b.{hubId}"
            });
        }

        try
        {
            var hub = await _hubsService.GetHubByIdAsync(token, hubId);
            return Ok(hub);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { 
                error = "Failed to retrieve hub from APS API",
                details = ex.Message,
                tip = "Check that the Hub ID exists and your token has access to it"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

