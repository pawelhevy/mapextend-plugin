using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;
using Xam.Plugin.MapExtend.Abstractions;
using Xam.Plugin.MapExtend.iOS;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapExtend), typeof(MapExtendRenderer))]
namespace Xam.Plugin.MapExtend.iOS
{
    /// <summary>
    /// MapExtend.Maps.Plugin Implementation
    /// </summary>
    public class MapExtendRenderer : ViewRenderer<Abstractions.MapExtend, MKMapView>
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            FormsMaps.Init();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Abstractions.MapExtend> e)
        {
            base.OnElementChanged(e);

            var formsMap = Element;

            if (formsMap == null)
            {
                return;
            }

            var mapView = new MKMapView
            {
                MapType = MKMapType.Standard,
                ShowsUserLocation = formsMap.IsShowingUser,
                ZoomEnabled = true,
                ScrollEnabled = true,
                ShowsBuildings = true,
                PitchEnabled = true,
            };

            var mapDelegate = new MapDelegate();

            mapView.Delegate = mapDelegate;

            SetNativeControl(mapView);


            ((ObservableCollection<Pin>)formsMap.Pins).CollectionChanged += OnPinsCollectionChanged;

            formsMap.polilenes.CollectionChanged += OnPolCollectionChanged;

            formsMap.EPins.CollectionChanged += OnEPinsCollectionChanged;
        }

        private void OnEPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEPins();
        }

        private void UpdateEPins()
        {
            var mkMapView = Control;
            var formsMap = Element;

            if (mkMapView.Annotations.Length > 0)
            {
                mkMapView.RemoveAnnotations(mkMapView.Annotations);
            }

            var items = formsMap.EPins;

            foreach (var item in items)
            {

                var coord = new CLLocationCoordinate2D(item.Location.Latitude, item.Location.Longitude);

                var point = new CustomAnnotation() { PinE = item };
                point.coord = coord;


                mkMapView.AddAnnotation(point);
            }
        }

        private void OnPolCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CreateLines();
        }

        private void CreateLines()
        {
            try
            {
                var mapView = Control;
                var formsMap = Element;

                var lineCords = new List<CLLocationCoordinate2D>();

                if (formsMap.polilenes.Count <= 0)
                {
                    return;
                }

                foreach (var item in formsMap.polilenes)
                {

                    lineCords.Add(new CLLocationCoordinate2D(item.Latitude, item.Longitude));
                }

                var line = MKPolyline.FromCoordinates(lineCords.ToArray());

                mapView.AddOverlay(line);

                mapView.SetVisibleMapRect(line.BoundingMapRect, true);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private void OnPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePins();
        }

        private void UpdatePins()
        {
            var mkMapView = Control;
            var formsMap = Element;

            if (mkMapView.Annotations.Length > 0)
            {
                mkMapView.RemoveAnnotations(mkMapView.Annotations);
            }

            var items = formsMap.Pins;

            foreach (var item in items)
            {
                var coord = new CLLocationCoordinate2D(item.Position.Latitude, item.Position.Longitude);

                var point = new MKPointAnnotation
                {
                    Title = item.Label,
                    Coordinate = coord
                };

                mkMapView.AddAnnotation(point);
            }
        }
    }

    class MapDelegate : MKMapViewDelegate
    {
        //Override OverLayRenderer to draw Polyline returned from directions
        public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            if (!(overlay is MKPolyline))
            {
                return null;
            }

            var route = (MKPolyline)overlay;
            var renderer = new MKPolylineRenderer(route) { StrokeColor = UIColor.Blue };

            return renderer;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, MonoTouch.Foundation.NSObject annotation)
        {
            var pet = ((CustomAnnotation)annotation);

            string pinImage = pet.PinE.ResourceNameImg;
            var pin = new MKAnnotationView();

            pin.Image = UIImage.FromBundle(pinImage);
            pin.CenterOffset = new PointF(0, -15);
            pin.Annotation = annotation;

            return pin;
        }
    }
}
