using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Enums;

namespace ProjectGym.Domain.Entities;

public class Attendance
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int WorkoutSessionId { get; set; }
    public DateTime CheckInTime { get; set; }=DateTime.UtcNow;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Registered;


    public Member Member { get; set; }=null!;
    public WorkoutSession WorkoutSession { get; set; }=null!;
}