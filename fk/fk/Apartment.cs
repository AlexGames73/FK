using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    class ApartmentBuilder
    {
        private Apartment apartment;

        public ApartmentBuilder()
        {
            apartment = new Apartment();
        }

        public ApartmentBuilder SetAddress(string value)
        {
            apartment.Address = value;
            return this;
        }

        public ApartmentBuilder SetPrice(string value)
        {
            apartment.Price = value;
            return this;
        }

        public ApartmentBuilder SetSquare(string value)
        {
            apartment.Square = value;
            return this;
        }

        public ApartmentBuilder SetRooms(string value)
        {
            apartment.Rooms = value;
            return this;
        }

        public ApartmentBuilder SetDistrict(string value)
        {
            apartment.District = value;
            return this;
        }

        public Apartment Build()
        {
            return apartment;
        }
    }

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
