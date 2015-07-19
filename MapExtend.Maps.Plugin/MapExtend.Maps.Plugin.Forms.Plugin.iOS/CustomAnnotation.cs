using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xam.Plugin.MapExtend.Abstractions;

namespace Xam.Plugin.MapExtend.iOS
{
    public class CustomAnnotation : MKAnnotation
    {
        public PinExtend PinE { get; set; }


        public override MonoTouch.CoreLocation.CLLocationCoordinate2D Coordinate
        {
            get
            {
                return coord;
            }
            set
            {
                coord = value;
            }
        }

        public CLLocationCoordinate2D coord { get; set; }
    }
}
