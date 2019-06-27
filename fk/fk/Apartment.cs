using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace filter_kvartir
{
    class Apartment
    {
        public string Address { get; set; }
        public int Price { get; set; }
        public int Square { get; set; }
        public int Rooms { get; set; }
        public string District { get; set; }

        public Apartment(string address, int price, int square, int rooms, string district)
        {
            Address = address;
            Price = price;
            Square = square;
            Rooms = rooms;
            District = district;
        }
    }
}
