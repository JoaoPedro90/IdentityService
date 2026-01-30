using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string Generate(User user)
        {
            // implementação provisória (só pra testar)
            return "token_fake";
        }
    }
}
