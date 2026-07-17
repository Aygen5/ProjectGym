using System;

namespace ProjectGym.Application.DTOs.Report;

public class ActiveMemberReportItemDto
{
    public int MemberId { get; set; }
    public string FullName { get; set; }=string.Empty;
    public string Email { get; set; }=string.Empty;
    public string PlanName { get; set; }=string.Empty;
    public DateTime MembershipEndDate { get; set; }
}

public class ActiveMemberReportDto
{
    public DateTime GeneratedAtUtc { get; set; }
    public int TotalActiveMembers { get; set; }
    public IReadOnlyList<ActiveMemberReportItemDto> Members { get; set; } = [];
}
