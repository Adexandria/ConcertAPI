using Concert.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Infrastructure.Service
{
    public class IdentityServices
    {
        readonly UserManager<UserModel> userManager;
        public IdentityServices(UserManager<UserModel> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<string> EmailConfirmationToken(UserModel user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);

        }
        public async Task AddUserRole(UserModel user , string role)
        {
            await userManager.AddToRoleAsync(user, role);
        }
        public async Task AddUserClaim(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name,user.FirstName),
              new Claim(ClaimTypes.Role,"User"),
              new Claim(ClaimTypes.Email,user.Email)
            };
            await userManager.AddClaimsAsync(user, claims);
           
        }
    }
}
