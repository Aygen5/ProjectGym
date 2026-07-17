using System;

namespace ProjectGym.Application.DTOs.Common;

public interface IApiResponse {}

public sealed class ApiResponse<T> : IApiResponse
{
    public bool IsSuccess {get; init;}

    public T? Value {get; init;}

    public IReadOnlyList<string> Errors {get; init;} = [];

    public string? Message {get; init;}

}