using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace AuthService.Application.Service
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthServiceImpl(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            //Busca user
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Credenciais inválidas");

            // Validar senha
            var isValid = _passwordHasher.Verify(
                request.Password,
                user.PasswordHash
            );

            if (!isValid)
                throw new UnauthorizedAccessException("Credenciais inválidas");

            // 3️⃣ Gerar token
            var token = _jwtTokenGenerator.Generate(user);

            // 4️⃣ Retornar DTO
            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public class UserService : IUserService
        {
            private readonly IUserRepository _userRepository;

            public UserService(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Guid> CreateUserAsync(UserRequestDto request)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);

                if (existingUser != null)
                    throw new Exception("Usuário já existe");

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User(request.Email, passwordHash);

                await _userRepository.AddAsync(user);

                return user.Id;
            }
        }


    }

}
