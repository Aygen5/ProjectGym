using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.MembershipPlan;
using ProjectGym.Application.Interfaces;

namespace ProjectGym.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MembershipPlansController : ApiControllerBase
    {
        private readonly IMembershipPlanService _service;

        public MembershipPlansController(IMembershipPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken) => FromResult(await _service.GetActivePlansAsync(cancellationToken));

        [HttpGet("manage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationDto dto, CancellationToken cancellationToken) => FromResult(await _service.GetAllPagedAsync(dto, cancellationToken));

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)=>FromResult(await _service.GetByIdAsync(id, cancellationToken));

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMembershipPlanDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(dto, cancellationToken);
            if(!result.IsSuccess) return FromResult(result);
            return CreatedAtAction(nameof(GetById), new {id = result.Value!.Id}, result.Value);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMembershipPlanDto dto, CancellationToken cancellationToken) => FromResult(await _service.UpdateAsync(id, dto, cancellationToken));

        [HttpDelete("{id:int}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)=>FromResult(await _service.DeleteAsync(id, cancellationToken)); 
    }
}

