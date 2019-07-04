using fk.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fk.Services
{
    class DomofondParser : IParser
    {
        public string MainUrl = @"https://www.domofond.ru";

        public string prodam = "/prodazha-kvartiry-";

        public string sdam = "/arenda-kvartiry-";

        public DomofondParser()
        {
            AddCity("Москва", "moskva-c3584");
            AddCity("Санкт-Петербург", "sankt_peterburg-c3414");
            AddCity("Новосибирск", "novosibirsk-c3285");
            AddCity("Екатеринбург ", "ekaterinburg-c2653");
            AddCity("Нижний Новгород", "nizhniy_novgorod-c1023");
            AddCity("Казань", "kazan-c1330");
            AddCity("Самара", "samara-c2415");
            AddCity("Челябинск", "chelyabinsk-c2358");
            AddCity("Омск", "omsk-c1406");
            AddCity("Ростов-на-Дону", "rostov_na_donu-c1759");
            AddCity("Уфа", "ufa-c1514");
            AddCity("Красноярск", "krasnoyarsk-c3174");
            AddCity("Пермь", "perm-c1667");
            AddCity("Волгоград", "volgograd-c400");
            AddCity("Ульяновск", "ulyanovsk-c2181");
        }

        public override string GetURL(Filters filters, int page = 1)
        {
            string ExtraInfo = filters.IsBuy ? prodam : sdam;
            string roomsCountUrl = "Rooms=";
            if (filters.Is2Room)
                roomsCountUrl += "Two%2C";
            if (filters.Is3Room)
                roomsCountUrl += "Three%2C";
            string priceUrl = $"PriceFrom={filters.PriceFrom}&PriceTo={filters.PriceTo}&";
            return MainUrl + ExtraInfo + GetRegion(filters.City) + "?" + priceUrl + roomsCountUrl + "&Page=" + page;
        }

        public override Apartment[] Parse(Filters filters, int pages = 1, PanelAds panelAds = null)
        {
            List<Apartment> apartments = new List<Apartment>();
            HtmlDocument MainPage = GetHtml(GetURL(filters));
            HtmlNodeCollection documentPages = MainPage.DocumentNode.SelectNodes(".//ul[@class='pagination__mainPages___2v12k']");
            if (documentPages != null)
            {
                int countPages = documentPages[0].SelectNodes(".//li[@class='pagination__page___2dfw0']").Count;
                pages = Math.Min(pages, int.Parse(documentPages[0].SelectNodes(".//li[@class='pagination__page___2dfw0']")[countPages - 1].InnerText));
            }
            for (int i = 0; i < pages; i++)
            {
                HtmlDocument document = GetHtml(GetURL(filters, i + 1));
                HtmlNodeCollection extraLinks = document.DocumentNode.SelectNodes(".//a[@class='long-item-card__item___ubItG']");
                foreach (HtmlNode htmlNode in extraLinks)
                {
                    try
                    {
                        Apartment apartment = GetApartment(htmlNode);
                        apartments.Add(apartment);
                        panelAds.AddToQueue(apartment);
                    }
                    catch { }
                }
                HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//a[@class='long-item-card__item___ubItG search-results__itemCardNotFirst___3fei6']");
                foreach (HtmlNode htmlNode in links)
                {
                    try
                    {
                        Apartment apartment = GetApartment(htmlNode);
                        apartments.Add(apartment);
                        panelAds.AddToQueue(apartment);
                    }
                    catch { }
                }
            }
            return apartments.ToArray();
        }
        
        public Apartment GetApartment(HtmlNode htmlNode)
        {
            string infoRoomsSquare = htmlNode.SelectNodes(".//div[@class='long-item-card__informationHeaderRight___3bkKw']")[0].InnerText;
            string rooms = infoRoomsSquare.Split(',')[0].Split('-')[0];
            string square = infoRoomsSquare.Split(',')[1].Split(' ')[1];
            string[] pricepieces = htmlNode.SelectNodes(".//div[@class='long-item-card__priceContainer___29DcY']")[0].InnerText.Split(' ');
            pricepieces[pricepieces.Length - 1] = "";
            string price = string.Join("", pricepieces);
            string address = htmlNode.SelectNodes(".//span[@class='long-item-card__address___PVI5p']")[0].InnerText;
            string title = htmlNode.SelectNodes(".//div[@class='long-item-card__informationHeaderRight___3bkKw']/span")[0].GetAttributeValue("title", "");
            try
            {
                string urlImage = htmlNode.SelectNodes(".//div[@class='img__cover___3zeI6 card-photo__cover___lxXXm']/img")[0].GetAttributeValue("src", "");
            }
            catch { }
            string link = MainUrl + htmlNode.GetAttributeValue("href", "");
            string info = htmlNode.SelectSingleNode(".//div[@class='description__descriptionBlock___3KWc1']/p[1]").InnerText;
            return new Apartment
            {
                Address = address,
                Price = price,
                Rooms = rooms,
                Square = square,
                Url = link,
                Info = info
            };
        }
    }
}
