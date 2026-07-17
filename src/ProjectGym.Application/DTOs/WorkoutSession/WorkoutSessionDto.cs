using System;

namespace ProjectGym.Application.DTOs.WorkoutSession;

public class WorkoutSessionDto
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime ScheduledTime { get; set; }
    public int Capacity { get; set; }
    public bool IsCancelled { get; set; }
    public int RegisteredCount { get; set; }
    public int AvailableSpots => Capacity-RegisteredCount;
}
