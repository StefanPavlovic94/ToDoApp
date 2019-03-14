using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Model
{
    public class Password
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public virtual User User {get; set;}
        public int UserId { get; set; }
    }
}
