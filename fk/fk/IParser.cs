using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    abstract class IParser
    {
        Dictionary<string, string> Cities { get; set; }

        public abstract string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh);
        public string GetRegion(string City) {
            return Cities[City];
        }
    }
}
