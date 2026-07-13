using ProjectGym.Domain.Entities;

namespace ProjectGym.Domain.Entites;

public class WorkoutSession
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public string Name { get; set; }=string.Empty;
    public DateTime ScheduledTime { get; set; }
    public int Capacity { get; set; }
    public bool IsCancelled { get; set; }


    public Trainer Trainer { get; set; }=null!;

    public ICollection<Attendance> Attendances { get; set; }=[];
}
