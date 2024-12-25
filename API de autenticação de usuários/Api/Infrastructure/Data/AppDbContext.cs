using Api.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<RevokedToken> RevokedTokens { get; set; }
    }
}
