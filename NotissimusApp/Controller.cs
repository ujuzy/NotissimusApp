using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Newtonsoft.Json;

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

                using (var stream = ((HttpWebResponse) response.Result).GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"), true))
                    {
                        return (T) XmlDeserializeFromString(reader.ReadToEnd(), typeof(T));
                    }
                }
            }
        }

        public static async Task<XmlElement> GetXmlRootAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (var response = request.GetResponseAsync())
            {
                response.Wait();

                using (var stream = ((HttpWebResponse)response.Result).GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"), true))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(reader.ReadToEnd());
                        XmlElement root = doc.DocumentElement;

                        return root;
                    }
                }
            }
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
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

        public static string GetJsonFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return new string(JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true));
        }

        public static string GetOffersFromXml(XmlElement root)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                foreach (XmlNode childnote in node)
                {
                    if (childnote.Name == "offers")
                    {
                        using (var sw = new StringWriter())
                        {
                            using (var xw = new XmlTextWriter(sw))
                            {
                                xw.Formatting = System.Xml.Formatting.Indented;
                                xw.Indentation = 2;
                                childnote.WriteTo(xw);
                            }

                            return sw.ToString();
                        }
                    }
                }
            }

            return "";
        }

        public static string FormatJsonString(string json)
        {
            int index = 0;
            while (json[index++] != '[');
                
            json = json.Remove(0, index - 1);

            index = json.Length - 1;
            while (json[index--] != ']');

            json = json.Remove(index + 2, json.Length - index - 2);

            return json;
        }
    }
}