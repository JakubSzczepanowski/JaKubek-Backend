using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Models
{
    public class RegisterUserViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public byte RoleId { get; set; } = 1;
    }
}
