using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.MembershipPlan;

public class UpdateMembershipPlanDto
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Plan adı zorunludur.")]
    [StringLength(100, MinimumLength =2,ErrorMessage ="Plan adı 2-100 karakter arasında olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage ="Aylık fiyat zorunludur")]
    [Range(0.01, 999999.99,ErrorMessage ="Aylık fiyat 0.01-999999.99 arasında olmalıdır.")]
    public decimal PricePerMonth { get; set; }
    
    [Required(ErrorMessage ="Süre(ay) zorunludur.")]
    [Range(1,36, ErrorMessage ="Süre 1-36 ay arasında olmalıdır.")]
    public int DurationMonths { get; set; }

    public bool IsActive { get; set; } = true;
}
