using System;

namespace ProjectGym.Application.DTOs.Trainer;

public class TrainerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Specialty { get; set; } = string.Empty;
}
