using HtmlAgilityPack;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;

namespace fk
{
    abstract class IParser
    {
        private Dictionary<string, string> Cities = new Dictionary<string, string>();

        public abstract string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public abstract Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);

        public void AddCity(string city, string citycode)
        {
            Cities.Add(city, citycode);
        }

        public string GetRegion(string City) {
            return Cities[City];
        }

        public string LoadPage(string url)
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

        public HtmlDocument GetHtml(string url)
        {
            var pageContent = LoadPage(url);
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);
            return document;
        }
    }
}
