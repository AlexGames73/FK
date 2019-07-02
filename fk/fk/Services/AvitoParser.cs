using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace fk
{
    class AvitoParser : IParser
    {
        public string urlAvito = @"https://www.avito.ru";

        public string prodam = "/kvartiry/prodam?cd=1";

        public string sdam = "/kvartiry/sdam?cd=1";

        public AvitoParser()
        {
            AddCity("Москва", "/moskva");
            AddCity("Санкт-Петербург", "/sankt-peterburg");
            AddCity("Новосибирск", "/novosibirsk");
            AddCity("Екатеринбург ", "/ekaterinburg");
            AddCity("Нижний Новгород", "/nizhniy_novgorod");
            AddCity("Казань", "/kazan");
            AddCity("Самара", "/samara");
            AddCity("Челябинск", "/chelyabinsk");
            AddCity("Омск", "/omsk");
            AddCity("Ростов-на-Дону", "/rostov-na-donu");
            AddCity("Уфа", "/ufa");
            AddCity("Красноярск", "/krasnoyarsk");
            AddCity("Пермь", "/perm");
            AddCity("Волгоград", "/volgograd");
            AddCity("Ульяновск", "/ulyanovsk");
        }

        public override string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page)
        {
            string ExtraInfo = isBuy ? prodam : sdam;
            string roomsCount = "549_";
            for (int i = 0; i < RoomsCount.Length; i++)
                roomsCount += 5695 + RoomsCount[i] + "-";
            string urlInfo = $"&pmax={PriceHigh}&pmin={PriceLow}&f={roomsCount}&p={page}";
            return urlAvito + City + ExtraInfo + urlInfo;
        }
        
        public Apartment GetApartment(HtmlNode htmlNode)
        {
            string info = htmlNode.SelectNodes(".//span[@itemprop='name']")[0].InnerHtml;
            string rooms = info.Split(',')[0].Split('-')[0];
            string square = info.Split(',')[1].Split(' ')[1];
            string price = htmlNode.SelectNodes(".//span[@itemprop='price']")[0].GetAttributeValue("content", "");
            string address = htmlNode.SelectNodes(".//p[@class='address']")[0].InnerText.Trim().Replace("&nbsp;", " ");
            return new Apartment {
                Address = address,
                Price = price,
                Rooms = rooms,
                Square = square
            };
        }

        public override Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int pages = 1)
        {
            List<Apartment> apartments = new List<Apartment>();
            HtmlDocument MainPage = GetHtml(GetURL(true, GetRegion(City), RoomsCount, PriceLow, PriceHigh, 1));
            if (MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']") != null)
            {
                pages = Math.Min(pages,
                    MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']")[0].SelectNodes(".//a[@class='pagination-page']").Count);
            }
            for (int i = 0; i < pages; i++)
            {
                HtmlDocument document = GetHtml(GetURL(true, GetRegion(City), RoomsCount, PriceLow, PriceHigh, i + 1));
                HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//div[@class='description item_table-description']");
                foreach (HtmlNode htmlNode in links)
                {
                    try {
                        apartments.Add(GetApartment(htmlNode));
                    } catch (Exception) { }
                }
            }
            return apartments.ToArray();
        }
    }
}
