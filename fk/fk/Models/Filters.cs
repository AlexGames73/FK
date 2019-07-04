using fk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace fk.Models
{
    public class Filters
    {
        public bool IsBuy { get; set; }
        public string City { get; set; }
        public bool Is2Room { get; set; }
        public bool Is3Room { get; set; }
        public string PriceFrom { get; set; }
        public string PriceTo { get; set; }

        public Filters()
        {
            IsBuy = true;
            City = "Ульяновск";
            Is2Room = true;
            Is2Room = true;
            PriceFrom = "0";
            PriceTo = "10000000";
        }
    }
}
