using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);
    }
}
