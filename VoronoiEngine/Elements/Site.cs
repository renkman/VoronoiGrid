namespace VoronoiEngine.Elements
{
    public class Site : IGeometry
    {
        public Point Point { get; set; }
        
        public override string ToString()
        {
            return $"Site at Point: {Point}";
        }
    }
}