using System;

namespace ProjectGym.Application.DTOs.Common;

public enum ResultErrorType
{
    None = 0,
    NotFound = 1,
    Validation = 2,
    Conflict = 3,
    Unauthrorized = 4,
    Forbidden = 5,
    Unexpected = 6
}

public interface IResult
{
    bool IsSuccess { get; }
    string? Error { get; }
    ResultErrorType ErrorType { get; }
    object? GetValue();
}

public class Result<T> : IResult
{
    public bool IsSuccess { get; private init;}

    public string? Error { get; private init;}

    public ResultErrorType ErrorType { get; private init;}

    public T? Value {get; private set;}

    object?  IResult.GetValue()=>Value;

    private Result () {}


   public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value,
        ErrorType=ResultErrorType.None
    };

    public static Result<T> NotFound(string error = "Kaynak bulunamadı.")=> new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.NotFound
    };

    public static Result<T> Conflict(string error) => new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.Conflict
    };

    public static Result<T> ValidationFailure(string error) => new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.Validation
    };

    public static Result<T> Forbidden(string error="Bu işlem için yetkiniz yok.") => new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.Forbidden
    };

    public static Result<T> Unauthorized(string error="Yetkisiz erişim.") => new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.Unauthrorized
    };

    public static Result<T> Unexpected(string error="Beklenmedik bir hata oluştu.") => new()
    {
        IsSuccess=false,
        Error=error,
        ErrorType=ResultErrorType.Unexpected
    };

}

