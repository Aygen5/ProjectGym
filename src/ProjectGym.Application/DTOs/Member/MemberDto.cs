using System;

namespace ProjectGym.Application.DTOs.Member;

public class MemberDto
{
    public int Id { get; set; }
    public string UserId { get; set; }=string.Empty;
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; }=string.Empty;

    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastVisitDate { get; set; }
}
