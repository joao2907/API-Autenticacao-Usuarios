using Api.Domain.Entities;
using System.Threading.Tasks;
using Api.Application.DTOs;

namespace Api.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(UserDto userDto);
        Task<User> AuthenticateAsync(string email, string password);
        Task<string> GenerateJwtAsync(string email, string name);
        Task LogoutAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
    }
}
