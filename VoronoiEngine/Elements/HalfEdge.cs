using System.Collections;
using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class HalfEdge : IEnumerable<Point>
    {
        private HashSet<Point> _points;

        public HalfEdge()
        {
            _points = new HashSet<Point>();
        }

        public void Add(Point point)
        {
            _points.Add(point);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _points.GetEnumerator();
        }
    }
}