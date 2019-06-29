using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    class Apartment
    {
        public string Address { get; set; }
        public string Price { get; set; }
        public string Square { get; set; }
        public string Rooms { get; set; }
        public string District { get; set; }

        public static ApartmentBuilder Builder()
        {
            return new ApartmentBuilder();
        }
    }
}
