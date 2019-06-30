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

        public Dictionary<string, string> cityes;

        public string prodam = "/kvartiry/prodam?cd=1";

        public string sdam = "/kvartiry/sdam?cd=1";

        public string city = "";

        public AvitoParser()
        {
            cityes = new Dictionary<string, string>();
            cityes = InputCityes();
        }
        
        public Dictionary<string, string> InputCityes()
        {
            Dictionary<string, string> inputCityes = new Dictionary<string, string>();
            inputCityes.Add("Москва", "/moskva");
            inputCityes.Add("Санкт-Петербург", "/sankt-peterburg");
            inputCityes.Add("Новосибирск", "/novosibirsk");
            inputCityes.Add("Екатеринбург ", "/ekaterinburg");
            inputCityes.Add("Нижний Новгород", "/nizhniy_novgorod");
            inputCityes.Add("Казань", "/kazan");
            inputCityes.Add("Самара", "/samara");
            inputCityes.Add("Челябинск", "/chelyabinsk");
            inputCityes.Add("Омск", "/omsk");
            inputCityes.Add("Ростов-на-Дону", "/rostov-na-donu");
            inputCityes.Add("Уфа", "/ufa");
            inputCityes.Add("Красноярск", "/krasnoyarsk");
            inputCityes.Add("Пермь", "/perm");
            inputCityes.Add("Волгоград", "/volgograd");
            inputCityes.Add("Ульяновск", "/ulyanovsk");
            return inputCityes;
        }

        public override string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page)
        {
            string ExtraInfo = isBuy ? prodam : sdam;
            string roomsCount = "f=549_";
            for (int i = 0; i < RoomsCount.Length; i++)
                roomsCount += 5695 + roomsCount[i] + "-";
            string urlInfo = $"&pmax={PriceHigh}&pmin={PriceLow}&f={roomsCount}&p={page}";
            return urlAvito + City + ExtraInfo + urlInfo;
        }
        
        public Apartment GetApartment(HtmlNode htmlNode)
        {
            string info = htmlNode.SelectNodes(".//span[@itemprop='name']")[0].InnerHtml;
            string rooms = info.Split(',')[0].Split('-')[0];
            string square = info.Split(',')[1].Split(' ')[1];
            string price = htmlNode.SelectNodes(".//span[@itemprop='price']")[0].GetAttributeValue("content", "");
            string[] splitAddress = htmlNode.SelectNodes(".//p[@class='address']")[0].InnerText.Split(',');
            string address = "";
            for (int i = 1; i < splitAddress.Length; i++)
            {
                address += splitAddress[i];
                if (i != splitAddress.Length - 1)
                    address += ",";
            }
            address = address.Trim();
            if (address.Split(',').Length < 3)
                address = city + ", " + address;
            string district = GetDistrict(address);
            return Apartment.Builder()
                .SetAddress(address)
                .SetDistrict(district)
                .SetPrice(price)
                .SetRooms(rooms)
                .SetSquare(square)
                .Build();
        }

        public override Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int pages = 1)
        {
            List<Apartment> apartments = new List<Apartment>();
            city = City;
            HtmlDocument MainPage = GetHtml(GetURL(true, cityes[City], RoomsCount, PriceLow, PriceHigh, 1));
            if (MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']") != null)
            {
                pages = Math.Min(pages,
                    MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']")[0].SelectNodes(".//a[@class='pagination-page']").Count);
            }
            for (int i = 0; i < pages; i++)
            {
                HtmlDocument document = GetHtml(GetURL(true, cityes[City], RoomsCount, PriceLow, PriceHigh, i + 1));
                HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//div[@class='description item_table-description']");
                foreach (HtmlNode htmlNode in links)
                {
                    apartments.Add(GetApartment(htmlNode));
                }
            }
            return apartments.ToArray();
        }
    }
}
