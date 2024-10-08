﻿using Dwt.Api.Helpers;
using Dwt.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dwt.Api.Controllers;

public class UsersController(IUserRepository userRepo) : DwtBaseController
{
    /// <summary>
    /// Fetches all users.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Only ADMIN role can see all users.</remarks>
    [HttpGet("/api/users")]
    [JwtAuthorize(Roles = "ADMIN")]
    public ActionResult<ApiResp<User>> GetAll()
    {
        return ResponseOk(userRepo.GetAll());
    }

    /// <summary>
    /// Fetches a user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// - Users can only see their own information.
    /// - ADMIN role can see all users.
    /// </remarks>
    [HttpGet("/api/users/{id}")]
    [JwtAuthorize]
    public ActionResult<ApiResp<User>> GetByID(string id)
    {
        var user = userRepo.GetByID(id);
        if (user == null)
        {
            return _respNotFound;
        }

        var requestUser = userRepo.GetByID(GetRequestUserId() ?? "");
        if (requestUser == null)
        {
            //should not happen
            return _respAuthenticationRequired;
        }

        if (!requestUser.HasRole("ADMIN") && requestUser.Id != user.Id)
        {
            // return "Not Found" in this case to avoid leaking the fact that the user exists
            // return _respAccessDenied;
            return _respNotFound;
        }

        return ResponseOk(user);
    }

    /* User list is immutable, can not add/remove/update users for now */
}
