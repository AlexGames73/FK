using fk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fk.Models
{
    public class User
    {
        public string Email { get; set; }
        public Filters Filters { get; set; }

        public User()
        {
            Filters = new Filters();
        }
    }
}
