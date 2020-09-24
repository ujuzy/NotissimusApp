using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NotissimusApp
{
    public class Controller
    {
        public static async Task<T> GetXmlAsync<T>(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (var response = request.GetResponseAsync())
            {
                response.Wait();

                using (var stream = ((HttpWebResponse)response.Result).GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                }
            }
        }

        public static List<string> GetOffersId(yml_catalog catalog)
        {
            List<string> idList = new List<string>();

            foreach (var offer in catalog.shop.offers)
            {
                idList.Add(offer.id.ToString());
            }

            return idList;
        }
    }
}