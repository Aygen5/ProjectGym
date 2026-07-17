using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.WorkoutSession;

public class UpdateWorkoutSessionDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage ="Antrenör seçimi zorunludur.")]
    [Range(1,int.MaxValue,ErrorMessage ="Geçerli bir antrenör seçin.")]
    public int TrainerId { get; set; }
    
    [Required(ErrorMessage ="Seans adı zorunludur.")]
    [StringLength(150, MinimumLength =2, ErrorMessage ="Seans adı 2-150 karakter arasında olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage ="Planlanan zaman zorunludur.")]
    public DateTime ScheduledTime { get; set; }

    [Required(ErrorMessage ="Kapasite zorunludur.")]
    [Range(1, 500, ErrorMessage ="Kapasite 1-500 arasında olmalıdır.")]
    public int Capacity { get; set; }
}
