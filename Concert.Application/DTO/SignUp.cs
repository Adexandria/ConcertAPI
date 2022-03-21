using System.ComponentModel.DataAnnotations;

namespace Concert.Application.DTO
{
    
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public  string Email { get; set; }
        [Required]
        public  string Password { get; set; }
        [Required]
        public string RetypePassword { get; set; }
    }
}
