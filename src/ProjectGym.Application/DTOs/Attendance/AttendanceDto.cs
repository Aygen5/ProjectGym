using System;

namespace ProjectGym.Application.DTOs.Attendance;

public class AttendanceDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int WorkoutSessionId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string SessionName { get; set; } = string.Empty;
}
