using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class LoginUserRequest
    {
        public string userNameorEmail { get; set; }
        public string Password { get; set; }
    }
}
