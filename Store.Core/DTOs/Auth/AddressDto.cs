using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.DTOs.Auth
{
    public class AddressDto
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
