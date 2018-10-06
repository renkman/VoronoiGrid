using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Structures;

namespace VoronoiTests
{
    [TestFixture]
    public class VoronoiMapTest
    {
        [Test]
        public void TestAdd()
        {
            var sites = new[] {
                new Site { Point = new Point { X = 1, Y = 2 } },
                new Site { Point = new Point {  X = 1, Y = 1 } },
                new Site { Point = new Point { X = 3, Y = 2 } }
            };

            var map = new VoronoiMap();
            map.Add(sites[0]);
            map.Add(sites[1]);
            map.Add(sites[2]);

            Assert.AreEqual(sites.Count(), map.Count());
        }

        [Test]
        public void TestAddRange()
        {
            var sites = new[] {
                new Site { Point = new Point { X = 1, Y = 2 } },
                new Site { Point = new Point {  X = 1, Y = 1 } },
                new Site { Point = new Point { X = 3, Y = 2 } }
            };

            var map = new VoronoiMap();
            map.AddRange(sites);

            Assert.AreEqual(sites.Count(), map.Count());
        }

        [Test]
        public void TestToArray()
        {
            var sites = new IGeometry[]
            {
                new Site{ Point = new Point  { X = 40, Y = 60 } },
                new Site { Point = new Point { X = 20, Y = 40 } },
                new Site { Point = new Point { X = 60, Y = 40 } },
                new HalfEdge(new Point { X = 30, Y = 50 }, new Point { X = 20, Y = 40 }, new Point { X = 40, Y = 60 }  ),
                new Vertex { Point = new Point { X = 40, Y = 40 } }
            };

            var map = new VoronoiMap();
            map.AddRange(sites);

            var result = map.ToArray();

            Assert.IsNotNull(result);
            Assert.AreEqual(70, result.GetLength(0));
            Assert.AreEqual(70, result.GetLength(1));
            Assert.AreEqual(new Point { X = 40, Y = 60 }, result[45, 65].Point);
            Assert.AreEqual(new Point { X = 20, Y = 40 }, result[25, 45].Point);
            Assert.AreEqual(new Point { X = 60, Y = 40 }, result[65, 45].Point);
            Assert.AreEqual(new Point { X = 40, Y = 40 }, result[45, 45].Point);
        }
        
        [Test]
        [Ignore("Obsolete test case")]
        public void TestAddVertex()
        {
            var left = new HalfEdge(new Point { X = 74, Y = 139 }, new Point { X = 170, Y = 140 }, new Point { X = 130, Y = 160 });
            var center = new HalfEdge(new Point { X = 61, Y = 139 }, new Point { X = 170, Y = 140 }, new Point { X = 110, Y = 150 });
            var right = new HalfEdge(new Point { X = 49, Y = 149 }, new Point { X = 130, Y = 160 }, new Point { X = 110, Y = 150 });
            var vertex = new Vertex {
                Point = new Point { X = 136, Y = 121 },
            };
            vertex.HalfEdges.Add(left);
            left.End = vertex;
            vertex.HalfEdges.Add(center);
            center.Start = vertex;
            vertex.HalfEdges.Add(right);
            right.End = vertex;

            var map = new VoronoiMap();
            map.Add(vertex);

            Assert.AreEqual(4, map.Count());
            Assert.AreEqual(1, map.Count(g => g is Vertex));
            Assert.AreEqual(3, map.Count(g => g is HalfEdge));

            // Found circle event for arcs: 
            // Leaf: Point: X: 95, Y: 75, CircleEvent: , 
            // Leaf: Point: X: 130, Y: 160, CircleEvent: Point: X: 146, Y: 46, 
            // Leaf: Point: X: 110, Y: 150, CircleEvent: Point: X: 136, Y: 83 
            // at Point: Point: X: 146, Y: 46 and Vertex: Point: X: 146, Y: 104
            // Add Vertex: Point: X: 146, Y: 104, Half Edges: Point: X: 85, Y: 74, Point: X: 49, Y: 149, Point: X: 78, Y: 74

            var left2 = new HalfEdge(new Point { X = 85, Y = 74 }, new Point { X = 95, Y = 75 }, new Point { X = 130, Y = 160 });
            var center2 = new HalfEdge(new Point { X = 78, Y = 74 }, new Point { X = 95, Y = 75 }, new Point { X = 110, Y = 150 });
            var right2 = new HalfEdge(new Point { X = 49, Y = 149 }, new Point { X = 130, Y = 160 }, new Point { X = 110, Y = 150 });
            var vertex2 = new Vertex
            {
                Point = new Point { X = 146, Y = 104 },
            };
            vertex2.HalfEdges.Add(left2);
            left2.End = vertex2;
            vertex2.HalfEdges.Add(center2);
            center2.Start = vertex2;
            vertex2.HalfEdges.Add(right2);
            right2.End = vertex2;

            map.Add(vertex2);

            Assert.AreEqual(7, map.Count());
            Assert.AreEqual(2, map.Count(g => g is Vertex));
            Assert.AreEqual(5, map.Count(g => g is HalfEdge));
        }
    }
}