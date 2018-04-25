using System;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public class BoundingBoxService : IBoundingBoxService
    {
        public Vertex AttachHalfEdge(HalfEdge halfEdge, int height, int width)
        {
            // Get the connected vertex
            if ((halfEdge.Start != null && halfEdge.End != null) || (halfEdge.Start == null && halfEdge.End == null))
                return null;

            var start = halfEdge.Start ?? halfEdge.End;

            // Get direction and calculate legs a and b
            var directionX = start.Point.X - halfEdge.Point.X;
            var directionY = start.Point.Y - halfEdge.Point.Y;
            var a = Math.Abs(directionX);
            var b = Math.Abs(directionY);

            // Calculate angle alpha
            var tanA = a / b;
            var angle = Math.Atan(tanA);

            // Calculate where the half edge connects to the bounding box:
            // Get y where x = 0

            b = (directionY > 0) ? start.Point.Y : height - 1 - start.Point.Y;
            a = (int)Math.Round(Math.Tan(angle) * b);

            var x = start.Point.X - a;

            var vertex = new Vertex
            {
                Point = new Point { X = x, Y = (directionY > 0) ? 0 : height - 1 },
            };
            vertex.HalfEdges.Add(halfEdge);
            if(halfEdge.End == null)
                halfEdge.End = vertex;
            else
                halfEdge.Start = vertex;
            return vertex;
        }
    }
}