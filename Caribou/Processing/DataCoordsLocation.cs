namespace Caribou.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public readonly struct DataCoordsLocation
    {
        public DataCoordsLocation(double x, double y)
        {
            this.Latitude = x;
            this.Longitude = y;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        public override string ToString() => $"({this.Latitude}, {this.Longitude})";
    }
}
