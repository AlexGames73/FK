using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace fk
{
    class CianParser : IParser
    {
        public CianParser()
        {
            AddCity("Москва", "1");
            AddCity("Санкт-Петербург", "2");
            AddCity("Новосибирск", "4897");
            AddCity("Екатеринбург", "4743");
            AddCity("Нижний Новгород", "4885");
            AddCity("Казань", "4777");
            AddCity("Самара", "4966");
            AddCity("Челябинск", "5048");
            AddCity("Омск", "5016");
            AddCity("Ростов-на-Дону", "4959");
            AddCity("Уфа", "176245");
            AddCity("Красноярск", "4827");
            AddCity("Пермь", "4927");
            AddCity("Волгоград", "4704");
            AddCity("Ульяновск", "5027");
        }

        public override string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1)
        {
            var deal_type = isBuy ? "sale" : "rent";
            List<string> rooms = new List<string>();
            foreach (int room in RoomsCount)
                rooms.Add($"room{room}=1");
            string roomsGET = string.Join("&", rooms.ToArray());
            string url = $"https://cian.ru/cat.php?deal_type={deal_type}&engine_version=2&quality=0&offer_type=flat&maxprice={PriceHigh}&minprice={PriceLow}&region={GetRegion(City)}&{roomsGET}&sort=price_object_order&p={page}";
            return url;
        }

        public override Apartment[] Parse(PanelAds panelAds, bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int pages = 1)
        {
            List<Apartment> res = new List<Apartment>();
            
            HtmlDocument document = GetHtml(GetURL(isBuy, City, RoomsCount, PriceLow, PriceHigh));
            var test = document.DocumentNode.SelectNodes(".//*[@class='_93444fe79c--totalOffers--22-FL']");
            int totalCount = int.Parse(test[0].InnerText.Split(' ')[0]);
            int count = 0;
            HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes("//*[@class='c6e8ba5398--info--WcX5M']");
            foreach (HtmlNode node in htmlNodes)
            {
                Apartment apartment = Parse(node);
                res.Add(apartment);
                panelAds.AddToQueue(apartment);
                count++;
            }
            for (int i = 1; count < totalCount && i < pages; i++)
            {
                document = GetHtml(GetURL(isBuy, City, RoomsCount, PriceLow, PriceHigh, i + 1));
                htmlNodes = document.DocumentNode.SelectNodes("//*[@class='c6e8ba5398--info--WcX5M']");
                foreach (HtmlNode node in htmlNodes)
                {
                    Apartment apartment = Parse(node);
                    res.Add(apartment);
                    panelAds.AddToQueue(apartment);
                    count++;
                }
            }
            res.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));
            return res.ToArray();
        }

        public Apartment Parse(HtmlNode node)
        {
            string[] title = node.SelectSingleNode(".//*[@class='c6e8ba5398--title--2CW78']").InnerText.Split(',');
            string rooms = title[0][0].ToString();
            string square = title[1].Trim().Split(' ')[0];
            List<string> prices = new List<string>(node.SelectSingleNode(".//*[@class='c6e8ba5398--header--1df-X']").InnerText.Split(' '));
            prices.RemoveAt(prices.Count - 1);
            string price = string.Join("", prices.ToArray());
            string address = node.SelectSingleNode("//*[@class='c6e8ba5398--address-links--1pHHO']/span").GetAttributeValue("content", "-");
            return new Apartment
            {
                Address = address,
                Price = price,
                Rooms = rooms,
                Square = square
            };
        }
    }
}
