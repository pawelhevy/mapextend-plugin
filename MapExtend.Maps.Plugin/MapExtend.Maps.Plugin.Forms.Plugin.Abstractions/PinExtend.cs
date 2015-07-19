using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Xam.Plugin.MapExtend.Abstractions
{
    public class PinExtend
    {
        public PinExtend(string name, string details, double latitude, double longitude)
        {
            Name = name;
            Details = details;
            Location = new Position(latitude, longitude);
        }

        public PinExtend()
        {
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public string ResourceNameImg { get; set; }
        public Position Location { get; set; }

    }
}
