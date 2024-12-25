using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Api.Application.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Application.Interfaces;
using Api.Application.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository repository, JwtSettings jwtSettings)
        {
            _repository = repository;
            _jwtSettings = jwtSettings;
        }

        public async Task RegisterAsync(UserDto userDto)
        {
            // Verificar se todos os campos foram preenchidos 
            if (string.IsNullOrWhiteSpace(userDto.Name))
                throw new InvalidOperationException("O campo 'name' é obrigatório.");

            if (string.IsNullOrWhiteSpace(userDto.Email))
                throw new InvalidOperationException("O campo 'email' é obrigatório.");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new InvalidOperationException("O campo 'password' é obrigatório.");

            // Verificar se o email já está em uso
            var existingUser = await _repository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Este email já está registrado.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            await _repository.AddAsync(user);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            // Verificar se todos os campos foram preenchidos 
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("O campo 'email' é obrigatório.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("O campo 'password' é obrigatório.");

            var user = await _repository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
            }

            return user;
        }

        public async Task<string> GenerateJwtAsync(string email, string name)
        {
            // Define as claims (adicionando nome e e-mail)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("name", name), // Claim personalizada para o nome
                new Claim("email", email), // Claim personalizada para o e-mail
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Este serviço faz a lógica de revogar o token, adicionando-o à lista de tokens revogados no banco de dados para evitar seu uso futuro.
        public async Task LogoutAsync(string token)
        {
            // Cria uma instância do "JwtSecurityTokenHandler" (Este objeto é responsável por manipular tokens JWT, incluindo validação, leitura e decodificação.)
            var tokenHandler = new JwtSecurityTokenHandler();

            // Usa o método "ReadToken" para interpretar a string do token e transformá-la em um objeto "JwtSecurityToken" (O "as JwtSecurityToken" faz uma conversão segura, permitindo verificar se a leitura foi bem-sucedida.)
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Caso "jwtToken" seja null, significa que o token é inválido ou malformado.
            if (jwtToken == null)
                throw new InvalidOperationException("Token inválido.");

            // Obtém a data e hora de expiração do token a partir do campo ValidTo.
            var expiration = jwtToken.ValidTo;

            // Chama o método do repositório responsável por salvar o token como "revogado" no banco de dados.
            await _repository.RevokeTokenAsync(token, expiration); // Passa o token e sua data de expiração como parâmetros para persistir essas informações.
        }

        // O método chama o repositório para verificar no banco de dados se o token foi revogado.(O repositório retorna true se o token está na lista de tokens revogados(ou seja, é inválido).)
        public async Task<bool> ValidateTokenAsync(string token)
        {
            return !await _repository.IsTokenRevokedAsync(token);
        }

    }
}
