namespace Caribou.Data
{
    /// <summary>Just a convenient equivalent to Point for geographic coordinates.</summary>
    public struct Coord
    {
        public Coord(double x, double y)
        {
            this.Latitude = x;
            this.Longitude = y;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString() => $"({this.Latitude}, {this.Longitude})";
    }
}
