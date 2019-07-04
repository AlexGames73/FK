using fk.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace fk.Services
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

        public override string GetURL(Filters filters, int page = 1)
        {
            var deal_type = filters.IsBuy ? "sale" : "rent";
            List<string> rooms = new List<string>();
            if (filters.Is2Room)
                rooms.Add($"room2=1");
            if (filters.Is3Room)
                rooms.Add($"room3=1");
            string roomsGET = string.Join("&", rooms.ToArray());
            string url = $"https://cian.ru/cat.php?deal_type={deal_type}&engine_version=2&quality=0&offer_type=flat&maxprice={filters.PriceTo}&minprice={filters.PriceFrom}&region={GetRegion(filters.City)}&{roomsGET}&sort=price_object_order&p={page}";
            return url;
        }

        public override Apartment[] Parse(Filters filters, int pages = 1, PanelAds panelAds = null)
        {
            List<Apartment> res = new List<Apartment>();
            
            HtmlDocument document = GetHtml(GetURL(filters));
            var test = document.DocumentNode.SelectNodes(".//*[contains(@class, 'totalOffers')]")[0].InnerText.Split(' ');
            int i = 0;
            int totalCount = 0;
            int buf = 0;
            while (int.TryParse(test[i], out buf))
            {
                totalCount = totalCount * 1000 + buf;
                i++;
            }
            int count = 0;
            for (i = 0; count < totalCount && i < pages; i++)
            {
                document = GetHtml(GetURL(filters, i + 1));
                HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes("//*[contains(@class, 'card')]");
                foreach (HtmlNode node in htmlNodes)
                {
                    try
                    {
                        Apartment apartment = Parse(node);
                        res.Add(apartment);
                        count++;
                        panelAds.AddToQueue(apartment);
                    }
                    catch { }
                }
            }
            res.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));
            return res.ToArray();
        }

        public Apartment Parse(HtmlNode node)
        {
            string title = node.SelectSingleNode(".//*[contains(@class, 'title')]").InnerText;
            string[] titles = title.Split(',');
            string rooms = titles[0][0].ToString();
            string square = titles[1].Trim().Split(' ')[0];
            HtmlNode pricececes = node.SelectSingleNode(".//*[contains(@class, 'price') and contains(@class, 'flex') and contains(@class, 'container')]/div[1]/div[1]");
            List<string> prices = new List<string>(pricececes.InnerText.Split(' '));
            prices.RemoveAt(prices.Count - 1);
            string price = string.Join("", prices.ToArray());
            string address = node.SelectSingleNode(".//*[contains(@class, 'address-links')]/span").GetAttributeValue("content", "-");

            string avaUrl = node.SelectSingleNode(".//span[@itemprop='url']").GetAttributeValue("content", "");
            string url = node.SelectSingleNode(".//a[contains(@class, 'header')]").GetAttributeValue("href", "");

            return new Apartment
            {
                Address = address,
                Price = price,
                Rooms = rooms,
                Square = square,
                Title = title,
                AvaUrl = avaUrl,
                Url = url
            };
        }
    }
}
