using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unstore.DTOs
{
    public class UserReadDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}