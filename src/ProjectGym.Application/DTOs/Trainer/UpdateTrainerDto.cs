using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.Trainer;

public class UpdateTrainerDto
{
    
    [Required(ErrorMessage ="Ad zorunludur.")]
    [StringLength(100, MinimumLength =2, ErrorMessage ="Ad 2-100 karakter arasında olmalıdır.")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage ="Soyad zorunludur.")]
    [StringLength(100, MinimumLength =2, ErrorMessage ="Soyad 2-100 karakter arasında olmalıdır.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage ="Uzmanlık zorunludur.")]
    [StringLength(150, MinimumLength =3, ErrorMessage ="Uzmanlık 3-150 karakter arasında olmalıdır.")]
    public string Specialty { get; set; } = string.Empty;
}
