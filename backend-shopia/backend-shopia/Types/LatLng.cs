using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace backend_shopia.Types;

public class LatLng
{
    public double Lat { get; set; }
    public double Lng { get; set; }

    public LatLng(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }

    public LatLng(Point point)
    {
        Lat = point.Y;
        Lng = point.X;
    }

    public Point ToGeometryPoint()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        return geometryFactory.CreatePoint(new Coordinate(Lng, Lat));
    }
}
