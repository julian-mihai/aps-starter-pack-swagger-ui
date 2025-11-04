using APSStarterPackSwaggerUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Projects Controller - Retrieve projects from hubs
/// </summary>
[ApiController]
[Route("projects")]
[Tags("3. Projects 📁 Data Management API")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectsService _projectsService;

    public ProjectsController(ProjectsService projectsService)
    {
        _projectsService = projectsService;
    }

    /// <summary>
    /// Get All Projects in a Hub
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Retrieves all projects within a specific hub.
    /// 
    /// **What is a Project?**
    /// A project is a container for files, folders, and data in BIM 360 or ACC.
    /// 
    /// **⚠️ IMPORTANT: hubId MUST include the "b." prefix!**
    /// - ✅ Correct: "b.3def0ac0-6276..."
    /// - ❌ Wrong: "3def0ac0-6276..." (missing "b." prefix)
    /// 
    /// **Steps:**
    /// 1. Get your token from /auth/token
    /// 2. Get a hubId from /hubs (it will include "b." prefix)
    /// 3. Use the FULL hubId here (with "b.")
    /// 
    /// **Response includes:** Project IDs, names, and types
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="hubId">Hub ID WITH "b." prefix (e.g., "b.3def0ac0-6276...")</param>
    /// <response code="200">Returns list of projects</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjects(
        [FromQuery] string token,
        [FromQuery] string hubId)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { error = "Token is required", tip = "Get a token from /api/auth/token" });
        }

        if (string.IsNullOrEmpty(hubId))
        {
            return BadRequest(new { error = "HubId is required", tip = "Get a hubId from /hubs (must include 'b.' prefix)" });
        }

        if (!hubId.StartsWith("b."))
        {
            return BadRequest(new { 
                error = "HubId must include 'b.' prefix", 
                tip = "Correct format: 'b.3def0ac0-6276...'",
                yourValue = hubId
            });
        }

        try
        {
            var projects = await _projectsService.GetProjectsAsync(token, hubId);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get Project by ID
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Get details of a specific project.
    /// 
    /// **⚠️ IMPORTANT: hubId MUST include the "b." prefix!**
    /// - ✅ Correct hubId: "b.3def0ac0-6276..."
    /// - ✅ Correct projectId: "b.e66ece9f-5035..."
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="hubId">Hub ID WITH "b." prefix (e.g., "b.3def0ac0-6276...")</param>
    /// <param name="projectId">Project ID WITH "b." prefix (e.g., "b.e66ece9f-5035...")</param>
    /// <response code="200">Returns project details</response>
    [HttpGet("{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProject(
        [FromQuery] string token,
        [FromQuery] string hubId,
        string projectId)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(hubId))
        {
            return BadRequest(new { error = "Token and HubId are required" });
        }

        if (!hubId.StartsWith("b."))
        {
            return BadRequest(new { 
                error = "HubId must include 'b.' prefix", 
                tip = "Correct format: 'b.3def0ac0-6276...'",
                yourValue = hubId
            });
        }

        try
        {
            var project = await _projectsService.GetProjectByIdAsync(token, hubId, projectId);
            return Ok(project);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get Project Top Folders
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Retrieves the root folders of a project.
    /// 
    /// **What are Top Folders?**
    /// These are the main folders you see when you open a project (e.g., "Project Files", "Plans").
    /// 
    /// **⚠️ IMPORTANT: hubId MUST include the "b." prefix!**
    /// - ✅ Correct hubId: "b.3def0ac0-6276..."
    /// - ✅ Correct projectId: "b.e66ece9f-5035..."
    /// 
    /// **Next steps:**
    /// Use the folder URNs from this response with /folders endpoints to navigate deeper.
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="hubId">Hub ID WITH "b." prefix (e.g., "b.3def0ac0-6276...")</param>
    /// <param name="projectId">Project ID WITH "b." prefix (e.g., "b.e66ece9f-5035...")</param>
    /// <response code="200">Returns list of top folders</response>
    [HttpGet("{projectId}/topFolders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFolders(
        [FromQuery] string token,
        [FromQuery] string hubId,
        string projectId)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(hubId))
        {
            return BadRequest(new { error = "Token and HubId are required" });
        }

        if (!hubId.StartsWith("b."))
        {
            return BadRequest(new { 
                error = "HubId must include 'b.' prefix", 
                tip = "Correct format: 'b.3def0ac0-6276...'",
                yourValue = hubId
            });
        }

        try
        {
            var folders = await _projectsService.GetProjectTopFoldersAsync(token, hubId, projectId);
            return Ok(folders);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

