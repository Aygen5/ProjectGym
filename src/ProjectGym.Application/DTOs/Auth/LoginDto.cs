using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage ="Email zorunludur.")]
    [EmailAddress(ErrorMessage ="Geçerli bir email adresi girin.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage ="Şifre zorunludur.")]
    public string Password { get; set; } = string.Empty;
}
