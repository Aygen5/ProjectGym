using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ProjectGym.Application.DTOs.Auth;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.Interfaces;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Identity;


namespace ProjectGym.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppIdentityUser> userManager, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }


    public async Task<Result<UserDto>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
         cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
            return Result<UserDto>.NotFound("Kullanıcı bulunamadı.");
        var roles = await _userManager.GetRolesAsync(user);
        var member = await _unitOfWork.Members.GetByUserIdAsync(userId, cancellationToken);
        return Result<UserDto>.Success(MapToUserDto(user, roles, member?.Id));
    }



    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if(user is null)
            return Result<LoginResponseDto>.Unauthorized("Email ya da şifre hatalı!");
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if(!isPasswordValid)
            return Result<LoginResponseDto>.Unauthorized("Email ya da şifre hatalı!");
        
        if(await _userManager.IsLockedOutAsync(user))
            return Result<LoginResponseDto>.Unauthorized("Hesabınız geçici olarak kilitlenmiştir. Lütfen daha sonra deneyiniz.");

        var roles = await _userManager.GetRolesAsync(user);
        var member = await _unitOfWork.Members.GetByUserIdAsync(user.Id, cancellationToken);
        var token = GenerateJwtToken(user, roles, member?.Id);
        var expirationMinutes = GetExpirationMinutes(_configuration.GetSection("JwtSettings"));
        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = token,
            Expiration= DateTime.UtcNow.AddMinutes(expirationMinutes),
            User= MapToUserDto(user, roles, member?.Id) 
        });
    }


    public async Task<Result<UserDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if(existingUser is not null)
             return Result<UserDto>.Conflict("Bu email adresi zaten kayıtlı.");
        var identityUser = new AppIdentityUser
        {
            FirstName= dto.FirstName,
            LastName=  dto.LastName,
            Email= dto.Email,
            UserName= dto.Email,
            CreatedAt=DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(identityUser, dto.Password);
        if(!result.Succeeded)
        {
            
            var errors = result.Errors.Select(e=>e.Description);
            return Result<UserDto>.ValidationFailure(string.Join(",", errors));
        }

        await _userManager.AddToRoleAsync(identityUser, "Member");
        var member = new Member
        {
            UserId= identityUser.Id,
            FirstName=dto.FirstName,
            LastName=dto.LastName,
            Email=dto.Email,
            JoinDate=DateTime.UtcNow,
            IsActive=true,
        };

        await _unitOfWork.Members.AddAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var roles = await _userManager.GetRolesAsync(identityUser);
        var userDto = MapToUserDto(identityUser, roles, member.Id);

        return Result<UserDto>.Success(userDto);   
    }

     private static UserDto MapToUserDto(AppIdentityUser user, IList<string> roles, int? memberId = null) => 
        new()
        {
            Id= user.Id,
            MemberId=memberId,
            FirstName = user.FirstName,
            LastName=user.LastName,
            Email = user.Email ?? string.Empty,
            Roles = roles
        };

    private string GenerateJwtToken(AppIdentityUser user, IList<string> roles, int? memberId = null)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var auidence = jwtSettings["Auidence"];
        var expirationMinutes = GetExpirationMinutes(jwtSettings);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            new("firstName",user.FirstName),
            new("lastName",user.LastName),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if(memberId.HasValue)
            claims.Add(new Claim("memberId",memberId.Value.ToString()));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: auidence,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetExpirationMinutes(IConfigurationSection? jwtSettings)
    {
        var minutes = jwtSettings!.GetValue<int>("ExpirationMinutes");
        return minutes == 0 ? 60 : minutes;
    }
}


