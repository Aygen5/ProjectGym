using System;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Interface;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(ProjectGymDbContext context) : base(context)
    {
    }
}
