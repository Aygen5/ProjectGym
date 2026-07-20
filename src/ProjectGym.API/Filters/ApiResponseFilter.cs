using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectGym.Application.DTOs.Common;

namespace ProjectGym.API.Filters;

public class ApiResponseFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        throw new NotImplementedException();
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if(context.Result is ObjectResult {Value: IApiResponse}) return;

        switch (context.Result)
        {
            case CreatedAtActionResult createdAt:
                {
                    var httpContext = context.HttpContext;
                    var linkGenerator = httpContext.RequestServices.GetRequiredService<LinkGenerator>();
                    var location = linkGenerator.GetUriByAction(
                        httpContext,
                        createdAt.ActionName,
                        createdAt.ControllerName,
                        createdAt.RouteValues,
                        httpContext.Request.Scheme,
                        httpContext.Request.Host,
                        httpContext.Request.PathBase
                    );
                    if(!string.IsNullOrEmpty(location)) httpContext.Response.Headers.Location=location;

                    object wrapped = createdAt.Value switch
                    {
                        Application.DTOs.Common.IResult ir => ApiResponseFactory.FromResult(ir),
                        var payload => ApiResponseFactory.Success<object?>(payload)  
                    };

                    context.Result = new ObjectResult(wrapped)
                    {
                        StatusCode = createdAt.StatusCode ?? StatusCodes.Status201Created
                    };
                    break;
                }

                case ObjectResult { StatusCode: >= 200 and <300} successResult:
                {
                    var wrapped = new ApiResponse<object?>
                    {
                        IsSuccess=true,
                        Value=successResult.Value,
                        Message=null
                    };
                    context.Result = new ObjectResult(wrapped)
                    {
                        StatusCode=successResult.StatusCode
                    };
                    break;
                }

                case ObjectResult errorResult when errorResult.StatusCode>=400:
                {
                    var message = errorResult.Value?.ToString()??"Bir hata oluştu.";
                    context.Result = new ObjectResult(ApiResponseFactory.Failure(message))
                    {
                        StatusCode = errorResult.StatusCode
                    };
                    break;
                }

                case StatusCodeResult { StatusCode: 204 }:
                {
                    context.Result = new ObjectResult(ApiResponseFactory.Success("İşlem başarılı"))
                    {
                        StatusCode=200
                    };
                    break;
                }

                case StatusCodeResult statusCodeResult when statusCodeResult.StatusCode >= 400:
                {
                    var message = statusCodeResult.StatusCode switch
                    {
                        401=>"Kimlik doğrulama gereklidir.",
                        403=>"Bu işlem için yetkiniz yok.",
                        404=>"Kaynak bulunamadı",
                        _ => "Bir hata oluştu"
                    };
                    context.Result = new ObjectResult(ApiResponseFactory.Failure(message))
                    {
                        StatusCode=statusCodeResult.StatusCode
                    };
                    break;
                }
        }
    }
}