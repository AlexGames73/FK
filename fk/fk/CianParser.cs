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
            Cities = new Dictionary<string, string>();
            Cities.Add("Москва", "1");
            Cities.Add("Санкт-Петербург", "2");
            Cities.Add("Новосибирск", "4897");
            Cities.Add("Екатеринбург", "4743");
            Cities.Add("Нижний Новгород", "4885");
            Cities.Add("Казань", "4777");
            Cities.Add("Самара", "4966");
            Cities.Add("Челябинск", "5048");
            Cities.Add("Омск", "5016");
            Cities.Add("Ростов-на-Дону", "4959");
            Cities.Add("Уфа", "176245");
            Cities.Add("Красноярск", "4827");
            Cities.Add("Пермь", "4927");
            Cities.Add("Волгоград", "4704");
            Cities.Add("Ульяновск", "5027");
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

        public override Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int pages = 1)
        {
            List<Apartment> res = new List<Apartment>();

            HtmlWeb web = new HtmlWeb();
            web.CachePath = Directory.GetCurrentDirectory();
            web.UsingCache = true;
            HtmlDocument document = web.Load(GetURL(isBuy, City, RoomsCount, PriceLow, PriceHigh));
            var test = document.DocumentNode.SelectNodes(".//*[@class='_93444fe79c--totalOffers--22-FL']");
            int totalCount = int.Parse(test[0].InnerText.Split(' ')[0]);
            int count = 0;
            HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes("//*[@class='c6e8ba5398--info--WcX5M']");
            foreach (HtmlNode node in htmlNodes)
            {
                res.Add(Parse(node));
                count++;
            }

            for (int i = 1; count < totalCount && i < pages; i++)
            {
                document = web.Load(GetURL(isBuy, City, RoomsCount, PriceLow, PriceHigh, i + 1));
                htmlNodes = document.DocumentNode.SelectNodes("//*[@class='c6e8ba5398--info--WcX5M']");
                foreach (HtmlNode node in htmlNodes)
                {
                    res.Add(Parse(node));
                    count++;
                }
            }
            res.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));
            return res.ToArray();
        }

        public Apartment Parse(HtmlNode node)
        {
            string[] title = node.SelectSingleNode(".//*[@class='c6e8ba5398--title--2CW78']").InnerText.Split(',');
            string Rooms = title[0][0].ToString();
            string Square = title[1].Trim().Split(' ')[0];
            List<string> price = new List<string>(node.SelectSingleNode(".//*[@class='c6e8ba5398--header--1df-X']").InnerText.Split(' '));
            price.RemoveAt(price.Count - 1);
            string Price = string.Join("", price.ToArray());
            Console.WriteLine(Price);
            HtmlNodeCollection nodes = node.SelectNodes(".//div[@class='c6e8ba5398--address-links--1pHHO']/div");
            string Address = nodes[0].InnerText + ", " + nodes[nodes.Count - 2].InnerText + " " + nodes[nodes.Count - 1].InnerText;
            string District = GetDistrict(Address);
            return Apartment.Builder()
                .SetAddress(Address)
                .SetPrice(Price)
                .SetSquare(Square)
                .SetRooms(Rooms)
                .SetDistrict(District)
                .Build();
        }
    }
}
