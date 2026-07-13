namespace ProjectGym.Domain.Entites;

public class Trainer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialty { get; set; }=string.Empty;

    public ICollection<WorkoutSession> WorkoutSessions { get; set; }=[];
}