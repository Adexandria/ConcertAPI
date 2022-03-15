using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Concert.Domain.Entities
{
    public class UserModel:IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override string PasswordHash { get; set; }
        public override bool TwoFactorEnabled { get; set; } = false;
    }
}
