﻿using Dwt.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Dwt.Api.Controllers;

/// <summary>
/// Base controller for all controllers in the application.
/// </summary>
/// <remarks>
/// - application/json as the default content type.
/// - Utility methods to conveniently return common responses.
/// </remarks>
[ApiController]
[Consumes("application/json")]
public abstract class DwtBaseController : ControllerBase
{
    /// <summary>
    /// Convenience method to return a 200 OK response.
    /// </summary>
    /// <returns></returns>
    protected static ObjectResult ResponseOk() => ResponseOk<object>(null);

    /// <summary>
    /// Generic response for "200 OK".
    /// </summary>
    protected static readonly ObjectResult _respOk = ResponseNoData(200, "Ok.");

    /// <summary>
    /// Convenience method to return a 200 OK response with data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected static ObjectResult ResponseOk<T>(T? data) => data == null ? _respOk : new OkObjectResult(new ApiResp<T>
    {
        Status = 200,
        Message = "Ok.",
        Data = data
    });

    /// <summary>
    /// Convenience method to return a response without attached data.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    protected static ObjectResult ResponseNoData(int statusCode) => ResponseNoData(statusCode, null);

    /// <summary>
    /// Convenience method to return a response with a message and without attached data.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected static ObjectResult ResponseNoData(int statusCode, string? message) => new(String.IsNullOrWhiteSpace(message) ? new ApiResp<object>
    {
        Status = statusCode
    }
    : new ApiResp<object>
    {
        Status = statusCode,
        Message = message
    })
    {
        StatusCode = statusCode
    };

    /// <summary>
    /// Generic response for "401 Unauthorized" errors.
    /// </summary>
    protected static readonly ObjectResult _respAuthenticationRequired = ResponseNoData(401, "Authentication required.");

    /// <summary>
    /// Generic response for "403 Forbidden" errors.
    /// </summary>
    protected static readonly ObjectResult _respAccessDenied = ResponseNoData(403, "Access denied.");

    /// <summary>
    /// Generic response for "404 Not Found" errors.
    /// </summary>
    protected static readonly ObjectResult _respNotFound = ResponseNoData(404, "Not found.");

    /// <summary>
    /// Generic response for "501 Not Implemented" errors.
    /// </summary>
    protected static readonly ObjectResult _respNotImplemented = ResponseNoData(501, "Not implemented.");

    /// <summary>
    /// Generic response for "503 Service Unavailable" errors.
    /// </summary>
    protected static readonly ObjectResult _respServiceUnavailable = ResponseNoData(503, "Server is unavailable to handle the request.");

    /// <summary>
    /// Get the attached user-id from the http-context.
    /// </summary>
    /// <returns></returns>
    protected string? GetRequestUserId()
    {
        HttpContext.Items.TryGetValue(GlobalVars.HTTP_CTX_ITEM_USERID, out var userId);
        return userId?.ToString();
    }
}
