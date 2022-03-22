using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Concert.Application.DTO
{
    public class Password
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string RetypePassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
