using System;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class MembershipPlanRepository : Repository<MembershipPlan>, IMembershipPlanRepository
{
    public MembershipPlanRepository(ProjectGymDbContext context) : base(context)
    {
    }
}
