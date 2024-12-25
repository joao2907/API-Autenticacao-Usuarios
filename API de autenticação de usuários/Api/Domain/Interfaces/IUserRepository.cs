using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> IsTokenRevokedAsync(string token);
        Task RevokeTokenAsync(string token, DateTime expiresAt);
    }
}
