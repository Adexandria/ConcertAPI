using Concert.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Concert.Infrastructure.Service
{
    public class AuthDbService :IdentityDbContext<UserModel>
    {
        public AuthDbService(DbContextOptions<AuthDbService> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
