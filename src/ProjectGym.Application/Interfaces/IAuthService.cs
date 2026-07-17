using System;
using ProjectGym.Application.DTOs.Auth;
using ProjectGym.Application.DTOs.Common;

namespace ProjectGym.Application.Interfaces;

public interface IAuthService
{
     Task<Result<UserDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken=default);

    Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);

    Task<Result<UserDto>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
}