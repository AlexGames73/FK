using HtmlAgilityPack;
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
        public Dictionary<string, string> Cities { get; set; }

        public abstract string GetURL(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public abstract Apartment[] Parse(bool isBuy, string City, int[] RoomsCount, int PriceLow, int PriceHigh, int page = 1);
        public string GetRegion(string City) {
            return Cities[City];
        }

        public string GetDistrict(string address)
        {
            string url = "https://geocode-maps.yandex.ru/1.x/?kind=district&format=json&geocode=";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            WebClient web = new WebClient();
            try
            {
                dynamic sdsa = serializer.Deserialize<dynamic>(Encoding.UTF8.GetString(web.DownloadData(url + address)));
                string nextURL = sdsa["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"];
                sdsa = serializer.Deserialize<dynamic>(Encoding.UTF8.GetString(web.DownloadData(url + nextURL)));
                sdsa = sdsa["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["metaDataProperty"]["GeocoderMetaData"]["Address"]["Components"];
                var district = "-";
                for (int i = sdsa.Length - 1; i >= 0; i--)
                {
                    if (sdsa[i]["kind"] == "district")
                    {
                        district = sdsa[i]["name"];
                        break;
                    }
                }
                return district;
            }
            catch (Exception) { return "-"; }
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
