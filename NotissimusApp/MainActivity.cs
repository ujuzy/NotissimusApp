﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace NotissimusApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private ListView mainList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var url = @"http://partner.market.yandex.ru/pages/help/YML.xml";
            var xmlObject = Controller.GetXmlAsync<yml_catalog>(url).Result;
            

            var listId = Controller.GetOffersId(xmlObject);

            mainList = FindViewById<ListView>(Resource.Id.idListView);
            mainList.Adapter = new ArrayAdapter(this, Resource.Layout.content_main, listId);

            mainList.ItemClick += (sender, e) =>
            {
                JsonActivity.OfferPosition = e.Position;

                StartActivity(new Intent(Application.Context, typeof(JsonActivity)));
            };
        }
    }
}