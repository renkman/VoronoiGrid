using System.Collections;
using System.Collections.Generic;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Structures
{
    public class VoronoiMap : IEnumerable<IGeometry>
    {
        private Dictionary<Point, IGeometry> _map;

        public VoronoiMap()
        {
            _map = new Dictionary<Point, IGeometry>();
        }

        public void Add(IGeometry element)
        {
            _map[element.Point] = element;
        }

        public void AddRange(IEnumerable<IGeometry> elements)
        {
            foreach (var element in elements)
                Add(element);
        }

        public IEnumerator<IGeometry> GetEnumerator()
        {
            return _map.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}