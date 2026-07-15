using System;
using Microsoft.AspNetCore.Identity;

namespace ProjectGym.Infrastructure.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
    
}