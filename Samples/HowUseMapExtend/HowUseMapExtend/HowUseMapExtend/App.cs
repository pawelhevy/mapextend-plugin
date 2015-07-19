using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.MapExtend.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HowUseMapExtend
{
    public class App : Application
    {
        public App()
        {
            var map = new MapExtend()
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var entryEnd = new Entry
            {
                Placeholder = "Address",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var btnSearch = new Button
            {
                Text = "Add",
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent
            };

            var btnCreateRoute = new Button
            {
                Text = "Route",
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent
            };

            var barItens = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            btnSearch.Clicked += async (sender, args) =>
            {
                await map.SearchAdress(entryEnd.Text).ContinueWith(t =>
                {
                    var d = t;
                });

                entryEnd.Text = string.Empty;
                entryEnd.Unfocus();
            };

            barItens.Children.Add(entryEnd);
            barItens.Children.Add(btnSearch);

            btnCreateRoute.Clicked += async (sender, args) =>
            {
                await map.CreateRoute(map.Pins[0].Position, map.Pins[1].Position).ContinueWith(t =>
                {
                    var d = t;
                });
            };

            var btnNearbyLocation = new Button
            {
                Text = "Nearby Pleaces"
            };

            btnNearbyLocation.Clicked += async (sender, args) =>
            {
                await map.NearbyLocations("AIzaSyBuATAkE41ioaMXd6MvWOmFlG2p-MlE6HM", "").ContinueWith(t =>
                {
                    var d = t;
                });

            };

            var stack = new StackLayout
            {
                Spacing = 0,
                Children = { barItens, map, btnCreateRoute, btnNearbyLocation }
            };

            var locator = CrossGeolocator.Current;

            locator.DesiredAccuracy = 50;

            var geoLocation = locator.GetPositionAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    //System.Diagnostics.Debug.WriteLine("Error : {0}", ((GeolocationException)t.Exception.InnerException).Error.ToString());
                }
                else if (t.IsCanceled)
                {
                    System.Diagnostics.Debug.WriteLine("Error : The geolocation has got canceled !");
                }
                else
                {
                    var currentLocation = new Position(t.Result.Latitude, t.Result.Longitude);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(currentLocation, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));
                        map.EPins.Add(new PinExtend{ Name= "seu endereço", Location= currentLocation, Details = "seu endreço", ResourceNameImg = "icon" });
                    });
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            // The root page of your application
            MainPage = new ContentPage
            {
                Padding = new Thickness(5, Device.OnPlatform(20, 5, 5), 5, 5),
                Content = stack
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
