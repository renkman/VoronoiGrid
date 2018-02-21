using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine;
using VoronoiEngine.Elements;

namespace VoronoiTests
{
    [TestFixture]
    public class VoronoiFactoryTest
    {
        [Test]
        public void TestCreateVoronoiMap()
        {
            var factory = VoronoiFactory.Instance;

            var sites = new List<Point>
            {
                new Point { X = 40, Y = 60 },
                new Point { X = 20, Y = 40 },
                new Point { X = 60, Y = 40 }
            };

            var map = factory.CreateVoronoiMap(sites);

            Assert.IsNotNull(map);
            var vertex = map.Single(g => g is Vertex);
            Assert.AreEqual(vertex.Point, new Point { X = 40, Y = 40 });
        }
    }
}