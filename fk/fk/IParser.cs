using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace fk
{
    abstract class IParser
    {
        public Dictionary<string, string> Cities { get; set; }

        public abstract string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public abstract Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public string GetRegion(string City) {
            return Cities[City];
        }

        public abstract string SetDistricts(string address);
    }
}
