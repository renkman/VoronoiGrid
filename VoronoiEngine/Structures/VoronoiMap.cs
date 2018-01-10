using System.Collections;
using System.Collections.Generic;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Structures
{
    public class VoronoiMap : IEnumerable<IGeometry>
    {
        private List<IGeometry> _map;

        public VoronoiMap()
        {
            _map = new List<Elements.IGeometry>();
        }

        public void Add(IGeometry element)
        {
            _map.Add(element);
        }

        public IEnumerator<IGeometry> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}