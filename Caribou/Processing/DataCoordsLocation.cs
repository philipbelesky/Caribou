using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caribou.Processing
{
    public readonly struct Coords
    {
        public Coords(double x, double y)
        {
            Latitude = x;
            Longitude = y;
        }

        public double Latitude { get; }
        public double Longitude { get; }
        public override string ToString() => $"({Latitude}, {Longitude})";
    }
}
