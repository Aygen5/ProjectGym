using System;
using AutoMapper;
using ProjectGym.Application.DTOs.Attendance;
using ProjectGym.Application.DTOs.Member;
using ProjectGym.Application.DTOs.Membership;
using ProjectGym.Application.DTOs.MembershipPlan;
using ProjectGym.Application.DTOs.Trainer;
using ProjectGym.Application.DTOs.WorkoutSession;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Enums;

namespace ProjectGym.Application.Mappings;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<MembershipPlan, MembershipPlanDto>();
        CreateMap<CreateMembershipPlanDto, MembershipPlan>()
            .ForMember(dest=>dest.IsActive, opt=> opt.MapFrom(_ =>true));
        CreateMap<UpdateMembershipPlanDto, MembershipPlan>();

        CreateMap<Trainer, TrainerDto>();
        CreateMap<CreateTrainerDto, Trainer>();
        CreateMap<UpdateTrainerDto, Trainer>();

        CreateMap<WorkoutSession, WorkoutSessionDto>()
            .ForMember(
                dest=>dest.TrainerName, 
                opt=>opt.MapFrom(src=>
                    src.Trainer != null
                       ? $"{src.Trainer.FirstName} {src.Trainer.LastName}".Trim() 
                       : string.Empty))
            .ForMember(
                dest=>dest.RegisteredCount,
                opt=>opt.MapFrom(src=>
                    src.Attendances.Count(a=>a.Status==AttendanceStatus.Registered)));

        CreateMap<CreateWorkoutSessionDto, WorkoutSession>()
            .ForMember(dest=>dest.IsCancelled, opt=>opt.MapFrom(_ =>false));
            
        CreateMap<UpdateWorkoutSessionDto, WorkoutSession>();

        CreateMap<Membership, MembershipDto>()
            .ForMember(dest=>dest.MemberName,
                opt=>opt.MapFrom(src=>
                    src.Member != null
                    ? $"{src.Member.FirstName} {src.Member.LastName}".Trim()
                    : string.Empty))
            .ForMember(dest=>dest.PlanName,
                opt=>opt.MapFrom(src=>
                    src.MembershipPlan!=null
                    ? src.MembershipPlan.Name
                    : string.Empty))
            .ForMember(dest=>dest.Status, opt=>opt.MapFrom(src=>src.Status.ToString()))
            .ForMember(dest=>dest.IsCurrentlyActive, opt=>opt.MapFrom(src=>src.IsActiveOn(DateTime.UtcNow)));

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest=>dest.SessionName,
                opt=>opt.MapFrom(src=>
                    src.WorkoutSession !=null
                    ? src.WorkoutSession.Name
                    : string.Empty))
            .ForMember(dest=>dest.ScheduledTime,
                opt=>opt.MapFrom(src=>
                    src.WorkoutSession!=null
                    ? src.WorkoutSession.ScheduledTime
                    : DateTime.MinValue))
            .ForMember(dest=>dest.Status,opt=>opt.MapFrom(src=>src.Status.ToString()));

        CreateMap<Member, MemberDto>();
        CreateMap<Member, MemberSummaryDto>()
            .ForMember(dest=>dest.FullName, opt=>opt.MapFrom(src=>$"{src.FirstName} {src.LastName}"));
    }
}