namespace Caribou.Data
{
    public readonly struct Coord
    {
        public Coord(double x, double y)
        {
            this.Latitude = x;
            this.Longitude = y;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        public override string ToString() => $"({this.Latitude}, {this.Longitude})";
    }
}
