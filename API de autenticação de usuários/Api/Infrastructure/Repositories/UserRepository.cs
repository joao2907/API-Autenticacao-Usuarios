using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string token, DateTime expiresAt)
        {
            // Cria uma nova instância da entidade "RevokedToken" (Essa classe representa um token revogado e contém informações como o próprio token, a data em que foi revogado e sua data de expiração.)
            var revokedToken = new RevokedToken
            {
                Token = token, 
                RevokedAt = DateTime.UtcNow, // Define a data e hora atual (em UTC) como o momento em que o token foi revogado.
                ExpiresAt = expiresAt // Define a data de expiração original do token, extraída previamente no "LogoutAsync" (É usada para indicar até quando o token seria válido, caso não tivesse sido revogado)
            };

            // Insere o objeto "revokedToken" na tabela "RevokedTokens" no banco de dados (Essa tabela armazena todos os tokens revogados, permitindo que o sistema verifique se um token já foi invalidado.)
            await _context.RevokedTokens.AddAsync(revokedToken);

            // Salva as alterações no banco de dados (Garante que o registro do token revogado seja persistido)
            await _context.SaveChangesAsync();
        }

        // Consulta a tabela do banco de dados RevokedTokens que contém os tokens revogados para verificar se o token existe
        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            // Consulta a tabela do banco de dados RevokedTokens que contém os tokens revogados para verificar se o token existe
            // Retorna o primeiro registro encontrado ou null se não existir nenhum.
            var revokedToken = await _context.RevokedTokens.FirstOrDefaultAsync(t => t.Token == token);
            // Retorna 'true' se o token revogado foi encontrado e ainda está dentro do período de validade.
            // Retorna 'false' caso o token não esteja na tabela ou se já expirou.
            return revokedToken != null && revokedToken.ExpiresAt > DateTime.UtcNow;
        }

    }
}