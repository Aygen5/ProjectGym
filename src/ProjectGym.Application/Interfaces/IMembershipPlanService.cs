using System;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.MembershipPlan;

namespace ProjectGym.Application.Interfaces;

public interface IMembershipPlanService
{
    Task<Result<IEnumerable<MembershipPlanDto>>> GetActivePlansAsync(CancellationToken cancellationToken = default);

    Task<Result<MembershipPlanDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<PagedResponseDto<MembershipPlanDto>>> GetAllPagedAsync(PaginationDto dto, CancellationToken cancellationToken = default);

    Task<Result<MembershipPlanDto>> CreateAsync(CreateMembershipPlanDto dto, CancellationToken cancellationToken = default);

    Task<Result<MembershipPlanDto>> UpdateAsync(int id, UpdateMembershipPlanDto dto, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken=default);
}
