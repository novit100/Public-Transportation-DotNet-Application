using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AppUser
    {
        public string UserName { get; set; }
        public UserStatuses UserStatus { get; set; }
        public string Password { get; set; }
    }
}
