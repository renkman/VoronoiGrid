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

        public void Add(Vertex vertex)
        {
            if (_map.ContainsKey(vertex.Point))
                return;

            ConnectEdges(vertex);
            foreach(var halfEdge in vertex.HalfEdges)
            {
                Add(halfEdge);
            }
            _map[vertex.Point] = vertex;
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

            var result = new IGeometry[x, y];

            foreach (var geometryItem in _map)
            {
                result[geometryItem.Key.X + 5, geometryItem.Key.Y + 5] = geometryItem.Value;
            }

            return result;
        }

        private void ConnectEdges(Vertex vertex)
        {
            var halfEdges = _map.Values.Where(g => g is HalfEdge).Cast<HalfEdge>()
                .Where(h => vertex.HalfEdges.Contains(h)).ToList();

            if (!halfEdges.Any())
                return;

            vertex.Replace(halfEdges);
        }
    }
}