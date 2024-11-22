namespace VoronoiEngine.Geomerty
{
    public static class GeometryHelper
    {
        public static int Lerp(int start, int end, int t)
        {
            return start + t * (end - start);
        }
    }
}
