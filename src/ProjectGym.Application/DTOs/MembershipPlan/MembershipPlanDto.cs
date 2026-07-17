using System;

namespace ProjectGym.Application.DTOs.MembershipPlan;

public class MembershipPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int DurationMonths { get; set; }
    public bool IsActive { get; set; }
}
