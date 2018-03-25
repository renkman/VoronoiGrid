using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void AddRange<TGeometry>(IEnumerable<TGeometry> elements) where TGeometry : IGeometry
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

        public IGeometry[,] ToArray()
        {
            var x = _map.Keys.Select(k => k.X).Max() + 10;
            var y = _map.Keys.Select(k => k.Y).Max() + 10;

            var result = new IGeometry[x,y];

            foreach(var geometryItem in _map)
            {
                result[geometryItem.Key.X + 5, geometryItem.Key.Y + 5] = geometryItem.Value;
            }

            return result;
        }
    }
}