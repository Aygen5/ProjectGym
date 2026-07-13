using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Enums;

namespace ProjectGym.Domain.Entites;

public class Membership
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int MembershipPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MembershipStatus Status { get; set; }=MembershipStatus.Active;


    public Member Member { get; set; } = null;
    public MembershipPlan MembershipPlan { get; set; } = null;

    public bool IsActiveOn(DateTime date) => 
     Status==MembershipStatus.Active && date>=StartDate && date <=EndDate;

    
}