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

        public void SetDistricts(List<Apartment> apartments, IWebDriver driver)
        {
            driver.Url = "https://raionpoadresu.ru/";

            driver.FindElement(By.XPath("//input[@id='address']")).SendKeys("Ульяновск, Северный венец 32");
            driver.FindElement(By.XPath("//button[@id='getDistrictButton']")).Click();

            while (driver.FindElements(By.XPath("//*[@id='result-district-element']/span")).Count == 0) { }

            int i = 0;
            foreach (Apartment apartment in apartments)
            {
                bool isNext = false;
                while (!isNext)
                {
                    try
                    {
                        string previousDistrict = driver.FindElement(By.XPath("//*[@id='result-district-element']/span")).Text;

                        driver.FindElement(By.XPath("//input[@id='address']")).Clear();
                        driver.FindElement(By.XPath("//input[@id='address']")).SendKeys(apartment.Address);
                        driver.FindElement(By.XPath("//button[@id='getDistrictButton']")).Click();

                        var elem = driver.FindElements(By.XPath("//*[@id='result-district-element']/span"));
                        while (elem[0].Text == previousDistrict)
                            elem = driver.FindElements(By.XPath("//*[@id='result-district-element']/span"));

                        apartment.District = elem[0].Text;
                        isNext = true;
                        Console.WriteLine(i);
                    }
                    catch (Exception) { }
                }
                i++;
            }
        }
    }
}
