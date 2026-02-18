using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }

    public interface IUserService
    {
        Task<Guid> CreateUserAsync(UserRequestDto request);
    }
}
