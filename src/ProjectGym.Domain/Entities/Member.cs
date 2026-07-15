using ProjectGym.Domain.Entites;

namespace ProjectGym.Domain.Entities;

public class Member
{
    public int Id {get; set;}
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
     public string LastName { get; set; } = string.Empty;
     public string Email { get; set; } = string.Empty;

     public DateTime JoinDate { get; set; } = DateTime.UtcNow;

     public bool IsActive { get; set; } =true;

     public DateTime? LastVisitDate {get; set;}


     public ICollection<Membership> Memberships {get; set;} = [];

     public ICollection<Attendance> Attendances {get; set;}=[];

}