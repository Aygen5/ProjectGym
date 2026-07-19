using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.DTOs.Trainer;
using ProjectGym.Application.Interfaces;

namespace ProjectGym.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class TrainersController : ApiControllerBase
    {
        private readonly ITrainerService _service;

        public TrainersController(ITrainerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationDto dto, CancellationToken cancellationToken)=> FromResult(await _service.GetAllPagedAsync(dto, cancellationToken));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)=>FromResult(await _service.GetByIdAsync(id, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainerDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(dto, cancellationToken);
            if(!result.IsSuccess) return FromResult(result);
            return CreatedAtAction(nameof(GetById),new {id=result.Value!.Id},result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainerDto dto, CancellationToken cancellationToken)=>FromResult(await _service.UpdateAsync(id, dto, cancellationToken));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)=>FromResult(await _service.DeleteAsync(id, cancellationToken));
    }
}
