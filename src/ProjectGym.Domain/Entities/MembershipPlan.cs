using ProjectGym.Domain.Entites;

namespace ProjectGym.Domain.Entities;

public class MembershipPlan
{
    public int Id {get; set;} 
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int DurationMonths { get; set; }
    public bool IsActive { get; set; } =true;

    public ICollection<Membership> Memberships { get; set; } = [];

    
}
