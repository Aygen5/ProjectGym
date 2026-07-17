using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.Membership;

public class CreateMembershipDto
{
    [Required(ErrorMessage ="Üye seçimi zorunludur.")]
    [Range(1,int.MaxValue, ErrorMessage ="Geçerli bir üye seçin.")]
    public int MemberId { get; set; }
    
    [Required(ErrorMessage ="Üyelik planı seçimi zorunludur.")]
    [Range(1,int.MaxValue, ErrorMessage ="Geçerli bir plan seçin.")]
    public int MembershipPlanId { get; set; }

    [Required(ErrorMessage ="Başlangıç tarihi zorunludur.")]
    public DateTime StartDate { get; set; }
}
