using NUnit.Framework;
using System.Linq;
using VoronoiEngine.Elements;
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
                new HalfEdge(new Point { X = 20, Y = 40 }, new Point { X = 20, Y = 40 }, new Point { X = 20, Y = 40 }  ),
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
    }
}