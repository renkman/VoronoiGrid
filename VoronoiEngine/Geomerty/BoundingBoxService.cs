using System;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public class BoundingBoxService : IBoundingBoxService
    {
        public Vertex AttachHalfEdge(HalfEdge halfEdge, int height, int width)
        {
            Vertex vertex = null;
            if (halfEdge.Start != null)
            {
                // Calculate legs a and b
                var a = Math.Abs(halfEdge.Start.Point.X - halfEdge.Point.X);
                var b = Math.Abs(halfEdge.Start.Point.Y - halfEdge.Point.Y);

                // Calculate angle alpha
                var tanA = a / b;
                var angle = Math.Atan(tanA);

                // Calculate where the half edge connects to the bounding box:
                // Get y where x = 0
                b = halfEdge.Start.Point.Y - 0;
                a = (int)Math.Round(Math.Tan(angle) * b);

                var x = halfEdge.Start.Point.X - a;

                vertex.Point = new Point { X = x, Y = 0 };
                vertex.HalfEdges.Add(halfEdge);
                halfEdge.End = vertex;
            }
            return vertex;
        }
    }
}