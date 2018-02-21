using System;
using System.Collections;
using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class HalfEdge : IEnumerable<Point>, IGeometry
    {
        private HashSet<Point> _points;

        public HalfEdge(Point start)
        {
            Point = start;
            _points = new HashSet<Point>();
        }

        public Point Point { get; set; }

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