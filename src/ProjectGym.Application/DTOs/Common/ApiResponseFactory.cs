using System;

namespace ProjectGym.Application.DTOs.Common;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T value, string? message = null) => new()
    {
        IsSuccess=true,
        Value=value,
        Message=message
    };

    public static ApiResponse<object?> Success(string? message = null) => new()
    {
        IsSuccess=true,
        Message=message
    };

    public static ApiResponse<object?> Failure(string error, string? message = null) => new()
    {
        IsSuccess=false,
        Errors=[error],
        Message=message
    };

    public static ApiResponse<object?> Failure(IEnumerable<string> errors, string? message = null) => new()
    {
        IsSuccess=false,
        Errors= [.. errors],
        Message=message
    };

    public static ApiResponse<object?> FromResult(IResult result)
    {
        if(result.IsSuccess)
            return Success(result.GetValue());
        return Failure(result.Error ?? "Bir hata oluştu.");
    }

    private static ApiResponse<object?> Success(object? value) => new()
    {
        IsSuccess=true,
        Value=value
    };
}
