using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    interface IParser
    {
        void GetURL(string City, int[] RoomsCount, int PriceLow, int PriceHigh);
    }
}
