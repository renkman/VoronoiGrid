using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public interface IBreakpointCalculationService
    {
        Point CalculateBreakpoint(Point left, Point right, double y);
        double GetY(Point p, Point newP);
    }
}