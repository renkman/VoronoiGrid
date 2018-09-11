using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public interface IBreakpointCalculationService
    {
        Point CalculateBreakpoint(Point left, Point right, int y);
        int GetY(Point p, Point newP);
    }
}