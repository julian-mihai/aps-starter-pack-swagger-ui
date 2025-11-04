using APSStarterPackSwaggerUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace APSStarterPackSwaggerUI.Controllers;

/// <summary>
/// Folders Controller - Manage folders in projects
/// </summary>
[ApiController]
[Route("folders")]
[Tags("4. Folders 📁 Data Management API")]
public class FoldersController : ControllerBase
{
    private readonly FoldersService _foldersService;

    public FoldersController(FoldersService foldersService)
    {
        _foldersService = foldersService;
    }

    /// <summary>
    /// Get Folder Contents
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Lists all subfolders and files within a folder.
    /// 
    /// **Getting a folder URN:**
    /// 1. Use `/projects/{projectId}/topFolders` to get root folders
    /// 2. Copy a folder's URN from the response
    /// 3. Use it here to see what's inside
    /// 
    /// **⚠️ IMPORTANT:**
    /// - projectId MUST include "b." prefix (e.g., "b.e66ece9f-5035...")
    /// - folderUrn format: "urn:adsk.wipprod:fs.folder:co.xxxxx"
    /// - The folderUrn will be automatically URL-encoded (don't encode it yourself!)
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="projectId">Project ID WITH "b." prefix (e.g., "b.e66ece9f-5035...")</param>
    /// <param name="folderUrn">Folder URN (e.g., "urn:adsk.wipprod:fs.folder:co.xxxxx")</param>
    /// <response code="200">Returns folder contents (subfolders and files)</response>
    [HttpGet("contents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolderContents(
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
            var contents = await _foldersService.GetFolderContentsAsync(token, projectId, folderUrn);
            return Ok(contents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get Folder Details
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Get detailed information about a specific folder.
    /// 
    /// **⚠️ IMPORTANT:**
    /// - projectId MUST include "b." prefix (e.g., "b.e66ece9f-5035...")
    /// - folderUrn format: "urn:adsk.wipprod:fs.folder:co.xxxxx"
    /// - The folderUrn will be automatically URL-encoded
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="projectId">Project ID WITH "b." prefix</param>
    /// <param name="folderUrn">Folder URN (e.g., "urn:adsk.wipprod:fs.folder:co.xxxxx")</param>
    /// <response code="200">Returns folder details</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolder(
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
            var folder = await _foldersService.GetFolderAsync(token, projectId, folderUrn);
            return Ok(folder);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Create a New Folder
    /// </summary>
    /// <remarks>
    /// Creates a new folder inside a parent folder.
    /// 
    /// ---
    /// 
    /// This endpoint requires a **3-legged token** (token from your SSO login or from the **GET /auth/current-token** endpoint).
    /// 
    /// ### 📋 Steps to Use:
    /// 1. **Get 3-legged token:** Call `/auth/current-token` → Copy the `access_token`
    /// 2. **Get parent folder URN:** Call `/projects/{projectId}/topFolders` → Copy a folder's URN
    /// 3. **Fill in parameters below:**
    ///    - ✅ **token**: Paste the 3-legged token from step 1
    ///    - ✅ **projectId**: Must start with `b.` (example: `b.3def0ac0-6276...`)
    ///    - ✅ **parentFolderUrn**: Must be format `urn:adsk.wipprod:fs.folder:co.xxxxx`
    ///    - ✅ **folderName**: Any name you want (example: `Workshop - 01`)
    /// 4. **Execute!**
    /// 
    /// ### ❓ Why 3-legged token?
    /// - Creating folders requires knowing WHO is creating it (for audit logs, permissions, ownership)
    /// - 3-legged token has your user identity built-in
    /// - 2-legged token will NOT work (no user context)
    /// 
    /// ### 💡 Example Values:
    /// ```
    /// token: eyJhbGc... (from /auth/current-token)
    /// projectId: b.3def0ac0-6276-4c72-89ec-xxxxx
    /// parentFolderUrn: urn:adsk.wipprod:fs.folder:co.BFJ-xxxxx
    /// folderName: Workshop - 01
    /// ```
    /// 
    /// ---
    /// </remarks>
    /// <param name="token">3-legged access token (from /auth/current-token)</param>
    /// <param name="projectId">Project ID WITH "b." prefix</param>
    /// <param name="parentFolderUrn">URN of the parent folder</param>
    /// <param name="folderName">Name for the new folder</param>
    /// <response code="200">Returns the created folder details</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFolder(
        [FromQuery] string token,
        [FromQuery] string projectId,
        [FromQuery] string parentFolderUrn,
        [FromQuery] string folderName)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(projectId) || 
            string.IsNullOrEmpty(parentFolderUrn) || string.IsNullOrEmpty(folderName))
        {
            return BadRequest(new { error = "Token, ProjectId, ParentFolderUrn, and FolderName are required" });
        }

        try
        {
            var folder = await _foldersService.CreateFolderAsync(token, projectId, parentFolderUrn, folderName);
            return Ok(new
            {
                message = $"✅ Folder '{folderName}' created successfully!",
                folder
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Search for Folders and Files (Recursive)
    /// </summary>
    /// <remarks>
    /// **Data Management API** - Searches for folders and files within a folder and all its subfolders.
    /// 
    /// **How it works:**
    /// 1. Get a top-level folder URN from `/projects/{projectId}/topFolders`
    /// 2. Use that folder URN as the starting point for search
    /// 3. Search recursively finds items in that folder and all subfolders
    /// 
    /// **⚠️ IMPORTANT:**
    /// - projectId MUST include "b." prefix (e.g., "b.e66ece9f-5035...")
    /// - folderUrn format: "urn:adsk.wipprod:fs.folder:co.xxxxx"
    /// - **Token MUST have `data:search` scope** (get a new token from `/auth/token` which now includes this scope by default)
    /// 
    /// **Search Tips:**
    /// - Try exact folder names first (e.g., "Folder 1" instead of "Folder")
    /// - Search might be case-sensitive
    /// - Leave searchFilter empty or use "*" to see all items
    /// 
    /// **Use case:** Find a specific folder/file without manually navigating the hierarchy.
    /// </remarks>
    /// <param name="token">Access token</param>
    /// <param name="projectId">Project ID WITH "b." prefix (e.g., "b.e66ece9f-5035...")</param>
    /// <param name="folderUrn">Starting folder URN to search from (e.g., "urn:adsk.wipprod:fs.folder:co.xxxxx")</param>
    /// <param name="searchFilter">Name to search for (e.g., "Folder" or "Plans")</param>
    /// <response code="200">Returns matching folders and files</response>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchFolders(
        [FromQuery] string token,
        [FromQuery] string projectId,
        [FromQuery] string folderUrn,
        [FromQuery] string searchFilter)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(folderUrn) || string.IsNullOrEmpty(searchFilter))
        {
            return BadRequest(new { error = "Token, ProjectId, FolderUrn, and SearchFilter are required" });
        }

        try
        {
            var results = await _foldersService.SearchFoldersAsync(token, projectId, folderUrn, searchFilter);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

