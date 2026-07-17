using System;

namespace ProjectGym.Application.DTOs.Member;

public class MemberSummaryDto
{
    public int Id { get; set; }
    public string FullName => string.Empty;
    public string Email { get; set; }=string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public bool HasActiveMembership { get; set; }
}
