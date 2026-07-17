using System;

namespace ProjectGym.Application.DTOs.Report;

public class SessionOccupancyReportDto
{
    public int WorkoutSessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public DateTime ScheduledTime { get; set; }

    public string TrainerName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int RegisteredCount { get; set; }
    public int AvailableSpots=>Capacity - RegisteredCount;
    public double OccupancyPercent => Capacity==0 ? 0 : Math.Round(RegisteredCount * 100.0/Capacity, 0);
    public bool IsCancelled { get; set; }
}
