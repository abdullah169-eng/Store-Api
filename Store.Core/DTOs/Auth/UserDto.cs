using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.DTOs.Auth
{
    public class UserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}
