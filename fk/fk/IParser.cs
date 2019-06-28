using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    abstract class IParser
    {
        public Dictionary<string, string> Cities { get; set; }

        public abstract string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public string GetRegion(string City) {
            return Cities[City];
        }

        public string GetDistrict(string address, IWebDriver driver)
        {
            if (driver.FindElements(By.XPath("//*[@id='result-district-element']/span")).Count != 0)
            {

            }

            driver.FindElement(By.XPath("//input[@id='address']")).Clear();
            driver.FindElement(By.XPath("//input[@id='address']")).SendKeys(address);
            driver.FindElement(By.XPath("//button[@id='getDistrictButton']")).Click();

            var elem = driver.FindElements(By.XPath("//*[@id='result-district-element']/span"));
            while (elem.Count == 0)
                elem = driver.FindElements(By.XPath("//*[@id='result-district-element']/span"));

            var district = elem[0].Text;

            return district;
        }
    }
}
