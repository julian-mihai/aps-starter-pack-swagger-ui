using APSStarterPackSwaggerUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Permissions Controller - Manage folder permissions
/// </summary>
[ApiController]
[Route("permissions")]
[Tags("9. Permissions 🔐 ACC API")]
public class PermissionsController : ControllerBase
{
    private readonly PermissionsService _permissionsService;

    public PermissionsController(PermissionsService permissionsService)
    {
        _permissionsService = permissionsService;
    }

    /// <summary>
    /// Get Example Permissions Structure
    /// </summary>
    /// <remarks>
    /// Returns an example of how permissions are structured in ACC/BIM 360.
    /// 
    /// **This is educational!** Use this endpoint to understand:
    /// - How permissions are formatted
    /// - What fields are required
    /// - Available permission actions
    /// - Subject types (USER, COMPANY, ROLE)
    /// 
    /// **Before setting permissions:**
    /// 1. Get company IDs from /api/companies
    /// 2. Get role IDs from /api/roles
    /// 3. Use those IDs in the permission structure
    /// </remarks>
    /// <response code="200">Returns example permission structure with explanations</response>
    [HttpGet("example")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetPermissionsExample()
    {
        var example = _permissionsService.GetPermissionsExample();
        return Ok(example);
    }

    /// <summary>
    /// Get Folder Permissions
    /// </summary>
    /// <remarks>
    /// Retrieves current permissions for a folder.
    /// 
    /// **What you'll see:**
    /// - Which users/companies/roles have access
    /// - What actions they can perform (view, edit, delete, etc.)
    /// - Permission inheritance settings
    /// 
    /// **Steps:**
    /// 1. Get a folder URN from /api/folders endpoints
    /// 2. Use it here to see who has access
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="projectId">Project ID (e.g., "b.12345-67890")</param>
    /// <param name="folderUrn">Folder URN</param>
    /// <response code="200">Returns folder permissions</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolderPermissions(
        [FromQuery] string token,
        [FromQuery] string projectId,
        [FromQuery] string folderUrn)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(folderUrn))
        {
            return BadRequest(new { error = "Token, ProjectId, and FolderUrn are required" });
        }

        try
        {
            var permissions = await _permissionsService.GetFolderPermissionsAsync(token, projectId, folderUrn);
            return Ok(permissions);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Set Folder Permissions
    /// </summary>
    /// <remarks>
    /// Sets permissions for a folder - controls who can access it and what they can do.
    /// 
    /// **Request Body Example:**
    /// ```json
    /// [
    ///   {
    ///     "subjectType": "COMPANY",
    ///     "subjectId": "12345",
    ///     "actions": ["view", "download", "collaborate"],
    ///     "permission": "ALLOW"
    ///   }
    /// ]
    /// ```
    /// 
    /// **Subject Types:**
    /// - USER: Individual user (need user ID)
    /// - COMPANY: Entire company (get ID from /api/companies)
    /// - ROLE: Users with specific role (get ID from /api/roles)
    /// 
    /// **Available Actions:**
    /// view, download, collaborate, create, upload, delete, admin
    /// 
    /// **Permission:** ALLOW or DENY
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="projectId">Project ID</param>
    /// <param name="folderUrn">Folder URN</param>
    /// <param name="permissions">List of permission entries</param>
    /// <response code="200">Returns confirmation of permission changes</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetFolderPermissions(
        [FromQuery] string token,
        [FromQuery] string projectId,
        [FromQuery] string folderUrn,
        [FromBody] List<PermissionEntry> permissions)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(folderUrn))
        {
            return BadRequest(new { error = "Token, ProjectId, and FolderUrn are required" });
        }

        if (permissions == null || permissions.Count == 0)
        {
            return BadRequest(new { 
                error = "Permissions list is required",
                tip = "Check /api/permissions/example for the correct format"
            });
        }

        try
        {
            var result = await _permissionsService.SetFolderPermissionsAsync(token, projectId, folderUrn, permissions);
            return Ok(new
            {
                message = "✅ Permissions updated successfully!",
                result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

