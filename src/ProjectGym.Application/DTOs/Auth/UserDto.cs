using System;

namespace ProjectGym.Application.DTOs.Auth;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public int? MemberId { get; set; }
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public string Email { get; set; }=string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public IList<string> Roles { get; set; } = [];
}
