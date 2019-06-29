using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

        public List<Apartment> apartments;

        public string prodam = "/kvartiry/prodam?cd=1";

        public string sdam = "/kvartiry/sdam?cd=1";

        public string city = "Москва";

        public AvitoParser()
        {
            cityes = new Dictionary<string, string>();
            apartments = new List<Apartment>();
        }

        public void InputCityes()
        {
            cityes.Add("Москва", "/moskva");
            cityes.Add("Санкт-Петербург", "/sankt-peterburg");
            cityes.Add("Новосибирск", "/novosibirsk");
            cityes.Add("Екатеринбург ", "/ekaterinburg");
            cityes.Add("Нижний Новгород", "/nizhniy_novgorod");
            cityes.Add("Казань", "/kazan");
            cityes.Add("Самара", "/samara");
            cityes.Add("Челябинск", "/chelyabinsk");
            cityes.Add("Омск", "/omsk");
            cityes.Add("Ростов-на-Дону", "/rostov-na-donu");
            cityes.Add("Уфа", "/ufa");
            cityes.Add("Красноярск", "/krasnoyarsk");
            cityes.Add("Пермь", "/perm");
            cityes.Add("Волгоград", "/volgograd");
            cityes.Add("Ульяновск", "/ulyanovsk");
        }

        static string LoadPage(string url)
        {
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            return result;
        }

        HtmlDocument GetHtml(string url)
        {
            var pageContent = LoadPage(url);
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);
            return document;
        }

        public void Parsing()
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("headless");
            IWebDriver driver = new ChromeDriver(options);
            for (int i = 0; i < 10; i++)
            {
                HtmlDocument document = GetHtml(GetURL(true, cityes[city], new int[2], 2, 3, i + 1));
                HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//div[@class='description item_table-description']");
                foreach (HtmlNode htmlNode in links)
                {
                    string info = htmlNode.SelectNodes(".//span[@itemprop='name']")[0].InnerHtml;
                    string rooms = info.Split(',')[0].Split('-')[0];
                    string square = info.Split(',')[1].Split(' ')[1];
                    string price = htmlNode.SelectNodes(".//span[@itemprop='price']")[0].GetAttributeValue("content", "");
                    HtmlDocument htmlDocument = GetHtml(urlAvito + htmlNode.SelectNodes(".//a[@class='item-description-title-link']")[0].GetAttributeValue("href", ""));
                    string address = htmlDocument.DocumentNode.SelectNodes(".//span[@itemprop='streetAddress']")[0].InnerHtml;
                    if (address.Split(',').Length < 3)
                        address = city + ", " + address;
                    string district = GetDistrict(address);
                    apartments.Add(
                        Apartment.Builder()
                        .SetAddress(address)
                        .SetPrice(price)
                        .SetSquare(square)
                        .SetRooms(rooms)
                        .SetDistrict(district)
                        .Build()
                    );
                }
            }
            driver.Dispose();
        }

        public override string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page)
        {
            string ExtraInfo = isBuy ? prodam : sdam;
            return urlAvito + City + ExtraInfo + "?" + "p=" + page;
        }

        public override Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
