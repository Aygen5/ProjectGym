using System;

namespace ProjectGym.Application.DTOs.Membership;

public class MembershipDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int MembershipPlanId { get; set; }
    public string MemberName { get; set; }=string.Empty;
    public string PlanName { get; set; }=string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsCurrentlyActive { get; set; }
}
