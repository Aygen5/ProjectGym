using System;
using AutoMapper;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.MembershipPlan;
using ProjectGym.Application.Interfaces;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Enums;
using ProjectGym.Domain.Interfaces;

namespace ProjectGym.Application.Services;

public class MembershipPlanService : IMembershipPlanService
{

    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MembershipPlanService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    public async Task<Result<MembershipPlanDto>> CreateAsync(CreateMembershipPlanDto dto, CancellationToken cancellationToken = default)
    {
       var duplicate = await _uow.MembershipPlans.FirstOrDefaultAsync(p=>p.Name.ToLower()==dto.Name.ToLower(), cancellationToken);
        if(duplicate is not null)
        {
            return Result<MembershipPlanDto>.Conflict($"'{dto.Name}' adında bir üyelik planı zaten mevcut.");
        }
        var plan = _mapper.Map<MembershipPlan>(dto);
        await _uow.MembershipPlans.AddAsync(plan, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<MembershipPlanDto>.Success(_mapper.Map<MembershipPlanDto>(plan));
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _uow.MembershipPlans.GetByIdAsync(id, cancellationToken);
        if(plan is null)
        {
            return Result<bool>.NotFound($"{id} id'li üyelik planı bulunamadı.");
        }
        var hasActiveMemberships = await _uow.Memberships.ExistsAsync(
            m=>m.MembershipPlanId==id && m.Status==MembershipStatus.Active && m.EndDate>=DateTime.UtcNow,cancellationToken
        );

        if(hasActiveMemberships)
        {
            return Result<bool>.Conflict("Bu plana bağlı akfif üyelikler var. Plan pasifleştirilemez; önce üyelikleri sonlandırmanız gerekmektedir.");
        }

        plan.IsActive= false;
        _uow.MembershipPlans.Update(plan);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<MembershipPlanDto>>> GetActivePlansAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _uow.MembershipPlans.FindAsync(p=>p.IsActive, cancellationToken);
        return Result<IEnumerable<MembershipPlanDto>>.Success(_mapper.Map<IEnumerable<MembershipPlanDto>>(plans));
    }

    public async Task<Result<PagedResponseDto<MembershipPlanDto>>> GetAllPagedAsync(PaginationDto dto, CancellationToken cancellationToken = default)
    {
        var query = _uow.MembershipPlans.GetQueryable();
        var totalCount = await _uow.MembershipPlans.CountAsync(null!, cancellationToken);

        var items = query
            .OrderBy(p=>p.Name)
            .Skip((dto.PageNumber - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .ToList();
        


        return Result<PagedResponseDto<MembershipPlanDto>>.Success(new PagedResponseDto<MembershipPlanDto>
        {
            Items=_mapper.Map<IEnumerable<MembershipPlanDto>>(items),
            TotalCount = totalCount,
            PageNumber=dto.PageNumber,
            PageSize=dto.PageSize
        });
    }

    public async Task<Result<MembershipPlanDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _uow.MembershipPlans.GetByIdAsync(id, cancellationToken);
        if(plan is null)
        {
            return Result<MembershipPlanDto>.NotFound($"'{id}' id'li üyelik planı bulunamadı.");
        }
        return Result<MembershipPlanDto>.Success(_mapper.Map<MembershipPlanDto>(plan));
    }

    public async Task<Result<MembershipPlanDto>> UpdateAsync(int id, UpdateMembershipPlanDto dto, CancellationToken cancellationToken = default)
    {
        var plan = await _uow.MembershipPlans.GetByIdAsync(id, cancellationToken);
        if(plan is null)
        {
            return Result<MembershipPlanDto>.NotFound($"'{id}' id'li üyelik planı bulunamadı.");
        }

        var duplicate = await _uow.MembershipPlans.FirstOrDefaultAsync(p=>p.Name.ToLower()==dto.Name.ToLower() && p.Id!=id, cancellationToken);
        if(duplicate is not null)
        {
            return Result<MembershipPlanDto>.Conflict($"'{dto.Name}' adı başka bir plan tarafından kullanılıyor.");
        }

        plan.Name = dto.Name;
        plan.PricePerMonth = dto.PricePerMonth;
        plan.DurationMonths=dto.DurationMonths;
        plan.IsActive=dto.IsActive;

        _uow.MembershipPlans.Update(plan);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<MembershipPlanDto>.Success(_mapper.Map<MembershipPlanDto>(plan));
    }
}