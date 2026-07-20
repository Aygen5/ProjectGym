using System;
using System.Numerics;
using AutoMapper;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.Trainer;
using ProjectGym.Application.Interfaces;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interfaces;

namespace ProjectGym.Application.Services;

public class TrainerService : ITrainerService
{

    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper; 

    public TrainerService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
        
    }

    public async Task<Result<TrainerDto>> CreateAsync(CreateTrainerDto dto, CancellationToken cancellationToken = default)
    {
        var trainer = _mapper.Map<Trainer>(dto);
        await _uow.Trainers.AddAsync(trainer, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<TrainerDto>.Success(_mapper.Map<TrainerDto>(trainer));
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
     var trainer = await _uow.Trainers.GetByIdAsync(id, cancellationToken);
        if(trainer is null)
            return Result<bool>.NotFound($"{id} id'li eğitmen bulunamadı.");
        
        
        _uow.Trainers.Delete(trainer);
        await _uow.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<TrainerDto>>> GetActiveTrainersAsync(CancellationToken cancellationToken = default)
    {
       var trainers = await _uow.Trainers.GetAllAsync(cancellationToken);
        return Result<IEnumerable<TrainerDto>>.Success(_mapper.Map<IEnumerable<TrainerDto>>(trainers));   
    }

    public async Task<Result<PagedResponseDto<TrainerDto>>> GetAllPagedAsync(PaginationDto dto, CancellationToken cancellationToken = default)
    {
        
        var query = _uow.Trainers.GetQueryable();
        var totalCount = await _uow.Trainers.CountAsync(null!, cancellationToken);

        var items = query
            .OrderBy(t=>t.FirstName)
            .ThenBy(t=>t.LastName)
            .Skip((dto.PageNumber - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .ToList();

         return Result<PagedResponseDto<TrainerDto>>.Success(new PagedResponseDto<TrainerDto>
        {
            Items=_mapper.Map<IEnumerable<TrainerDto>>(items),
            TotalCount = totalCount,
            PageNumber=dto.PageNumber,
            PageSize=dto.PageSize
        });
    }

    public async Task<Result<TrainerDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _uow.Trainers.GetByIdAsync(id, cancellationToken);
        if(trainer is null) return Result<TrainerDto>.NotFound($"{id} id'li eğitmen bulunamadı.");
        return Result<TrainerDto>.Success(_mapper.Map<TrainerDto>(trainer));
    }

    public async Task<Result<TrainerDto>> UpdateAsync(int id, UpdateTrainerDto dto, CancellationToken cancellationToken = default)
    {
        var trainer = await _uow.Trainers.GetByIdAsync(id, cancellationToken);
        if(trainer is null) return Result<TrainerDto>.NotFound($"{id} id'li eğitmen bulunamadı.");

        trainer.FirstName = dto.FirstName;
        trainer.LastName=dto.LastName;
        trainer.Specialty=dto.Specialty;

        _uow.Trainers.Update(trainer);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<TrainerDto>.Success(_mapper.Map<TrainerDto>(trainer));
    }

}
