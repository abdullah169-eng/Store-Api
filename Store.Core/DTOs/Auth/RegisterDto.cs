using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Core.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "DisplayName is Required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
