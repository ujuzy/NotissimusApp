using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;

using Newtonsoft.Json;

namespace NotissimusApp
{
    [Activity(Label = "JsonActivity")]
    public class JsonActivity : Activity
    {
        public static int OfferPosition;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_json);

            var url = @"http://partner.market.yandex.ru/pages/help/YML.xml";
            var jsonString = Controller.FormatJsonString(
                Controller.GetJsonFromXml(
                    Controller.GetOffersFromXml(
                        Controller.GetXmlRootAsync(url).Result)));

            List<offer> offers = JsonConvert.DeserializeObject<List<offer>>(jsonString);
            jsonString = JsonConvert.SerializeObject(offers[OfferPosition], Newtonsoft.Json.Formatting.Indented);

            var jsonTextView = FindViewById<TextView>(Resource.Id.offerJson);
            jsonTextView.Text = jsonString;
        }
    }
}