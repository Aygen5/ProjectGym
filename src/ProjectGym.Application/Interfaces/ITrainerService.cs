using System;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.Trainer;

namespace ProjectGym.Application.Interfaces;

public interface ITrainerService
{
    Task<Result<IEnumerable<TrainerDto>>> GetActiveTrainersAsync(CancellationToken cancellationToken=default);

    Task<Result<TrainerDto>> GetByIdAsync(int id, CancellationToken cancellationToken=default);

    Task<Result<PagedResponseDto<TrainerDto>>> GetAllPagedAsync(PaginationDto dto, CancellationToken cancellationToken = default);

    Task<Result<TrainerDto>> CreateAsync(CreateTrainerDto dto, CancellationToken cancellationToken = default);

    Task<Result<TrainerDto>> UpdateAsync(int id, UpdateTrainerDto dto, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken=default);
}
