using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    class CianParser : IParser
    {
        public override string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh)
        {
            var deal_type = isBuy ? "sale" : "rent";
            string url = $"https://cian.ru/cat.php?deal_type={deal_type}&engine_version=2&region={GetRegion(City)}";
            return url;
        }
    }
}
