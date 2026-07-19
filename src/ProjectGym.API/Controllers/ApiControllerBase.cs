using Microsoft.AspNetCore.Mvc;
using ProjectGym.Application.DTOs.Common;

namespace ProjectGym.API.Controllers;

[ApiController]

public class ApiControllerBase : ControllerBase
    {
        protected IActionResult FromResult<T>(Result<T> result)
        {
            if(result.IsSuccess)
                return Ok(result.Value);

            var message = result.Error ?? "İşlem başarısız.";
            return result.ErrorType switch
            {
                ResultErrorType.NotFound=>NotFound(message),
                ResultErrorType.Validation=>BadRequest(message),
                ResultErrorType.Conflict=>Conflict(message),
                ResultErrorType.Unauthrorized=>Unauthorized(message),
                ResultErrorType.Forbidden=>StatusCode(StatusCodes.Status403Forbidden, message),
                ResultErrorType.Unexpected=>StatusCode(StatusCodes.Status500InternalServerError, message),
                _ => BadRequest(message)
            };
        }

        public IActionResult CreatedFromResult<T>(Result<T> result, string actionName, object? routeValues)
        {
            if(!result.IsSuccess)
                return FromResult(result);
            return CreatedAtAction(actionName,routeValues, result.Value);
        }
    }

