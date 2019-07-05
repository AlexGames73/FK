using fk.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace fk.Services
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

        public override string GetURL(Filters filters, int page = 1)
        {
            string ExtraInfo = filters.IsBuy ? prodam : sdam;
            string roomsCount = filters.IsBuy ? "549_" : "550_";
            if (filters.Is2Room)
                roomsCount += filters.IsBuy ? "5697-" : "5704-";
            if (filters.Is3Room)
                roomsCount += filters.IsBuy ? "5698-" : "5705-";
            string urlInfo = $"&pmax={filters.PriceTo}&pmin={filters.PriceFrom}&f={roomsCount}&p={page}";
            return urlAvito + GetRegion(filters.City) + ExtraInfo + urlInfo;
        }
        
        public Apartment GetApartment(HtmlNode htmlNode)
        {
            string info = htmlNode.SelectNodes(".//span[@itemprop='name']")[0].InnerText;
            string rooms = info.Split(',')[0].Split('-')[0];
            string square = info.Split(',')[1].Split(' ')[1];
            string price = htmlNode.SelectNodes(".//span[@itemprop='price']")[0].GetAttributeValue("content", "");
            string address = htmlNode.SelectNodes(".//p[@class='address']")[0].InnerText.Trim().Replace("&nbsp;", " ");
            string urlImage = htmlNode.SelectNodes(".//li[@class='item-slider-item js-item-slider-item ']/div/img")[0].GetAttributeValue("src", "");
            if (urlImage.Count((x) => x == ':') != 0)
                urlImage = "https:" + htmlNode.SelectNodes(".//li[@class='item-slider-item js-item-slider-item ']/div/noscript/img")[0].GetAttributeValue("src", "");
            else
                urlImage = "https:" + urlImage;
            string link = urlAvito + htmlNode.SelectNodes(".//a[@class='js-item-slider item-slider']")[0].GetAttributeValue("href", "");
            return new Apartment {
                Address = address,
                Price = price,
                Rooms = rooms,
                Square = square,
                Title = info,
                AvaUrl = urlImage,
                Url = link
            };
        }

        public override Apartment[] Parse(Filters filters, int pages = 1, PanelAds panelAds = null)
        {
            List<Apartment> apartments = new List<Apartment>();
            HtmlDocument MainPage = GetHtml(GetURL(filters));
            if (MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']") != null)
            {
                pages = Math.Min(pages,
                    MainPage.DocumentNode.SelectNodes(".//div[@class='pagination js-pages']")[0].SelectNodes(".//a[@class='pagination-page']").Count);
            }
            for (int i = 0; i < pages; i++)
            {
                HtmlDocument document = GetHtml(GetURL(filters, i + 1));
                HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//div[@class='item item_table clearfix js-catalog-item-enum  item-with-contact js-item-extended']");
                foreach (HtmlNode htmlNode in links)
                {
                    try {
                        Apartment apartment = GetApartment(htmlNode);
                        apartments.Add(apartment);
                        panelAds.AddToQueue(apartment);
                    } catch { }
                }
            }
            return apartments.ToArray();
        }
    }
}
