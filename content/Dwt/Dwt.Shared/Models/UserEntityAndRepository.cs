﻿using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Dwt.Shared.Models;

/// <summary>
/// (Sample) This entity represents a user account.
/// </summary>
public class User
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonIgnore]
    public string Password { get; set; } = "";

    [JsonIgnore]
    public string Role { get; set; } = "";

    private static readonly string _delimPattern = @"[,;:\s]+";
    private static readonly string _prefixPattern = @"^[,;:\s]+";
    private static readonly string _suffixPattern = @"[,;:\s]+$";

    /// <summary>
    /// Convenience method to convert comma-delimited role string to an array of roles.
    /// </summary>
    /// <param name="_role"></param>
    /// <returns></returns>
    public static string[] RoleStrToArr(string _role)
    {
        var role = Regex.Replace(Regex.Replace(_role, _prefixPattern, ""), _suffixPattern, "");
        var result = role != "" ? Regex.Split(role, _delimPattern) : [];
        return result;
    }

    [JsonPropertyName("roles")]
    public string[] Roles
    {
        get
        {
            return RoleStrToArr(Role);
        }
        set
        {
            Role = string.Join(',', value ?? []);
        }
    }

    public bool HasRole(string role)
    {
        return Roles.Contains(role);
    }

    public bool Authenticate(string? password)
    {
        return Password == password;
    }
}

/// <summary>
/// Repository interface for user accounts.
/// </summary>
/// <remarks>Entity and repository interface are in the same file just for simplicity only.</remarks>
public interface IUserRepository : IGenericRepository<User>
{
}
