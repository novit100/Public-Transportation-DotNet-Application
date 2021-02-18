using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class User
    {
        public string UserName { get; set; }
        public UserStatuses UserStatus { get; set; }
        public string Password { get; set; }
        public IEnumerable<User> Users { set; get; }
    }
}
