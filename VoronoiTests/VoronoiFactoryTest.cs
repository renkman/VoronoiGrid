using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine;
using VoronoiEngine.Elements;
using VoronoiEngine.EventHandler;
using VoronoiEngine.Utilities;

namespace VoronoiTests
{
    [TestFixture]
    public class VoronoiFactoryTest
    {
        [Test]
        public void TestCreateVoronoiMap()
        {
            var factory = new VoronoiFactory(
                new SiteEventHandlerStrategy(),
                new CircleEventHandlerStrategy(),
                new SiteGenerator());

            var sites = new List<Site>
            {
                new Site { Point = new Point { X = 40, Y = 60 } },
                new Site { Point = new Point { X = 20, Y = 40 } },
                new Site { Point = new Point { X = 60, Y = 40 } }
            };

            var map = factory.CreateVoronoiMap(sites);

            Assert.IsNotNull(map);
            var vertex = map.Single(g => g is Vertex);
            Assert.AreEqual(vertex.Point, new Point { X = 40, Y = 40 });
        }

        [Test]
        public void TestCreateVoronoiMapWithFourSites()
        {
            var factory = new VoronoiFactory(
                new SiteEventHandlerStrategy(),
                new CircleEventHandlerStrategy(),
                new SiteGenerator());

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 110, Y = 150 } },
                new Site { Point = new Point {X = 170, Y = 140 } },
                new Site { Point = new Point {X = 95, Y = 75 } }
            };

            var map = factory.CreateVoronoiMap(sites);

            Assert.IsNotNull(map);
            var vertex = map.First(g => g is Vertex);
            Assert.AreEqual(vertex.Point, new Point { X = 136, Y = 121 });
        }

        [Test]
        [Ignore("Runs too long!")]
        public void TestCreateVoronoiMapGenerate()
        {
            var factory = new VoronoiFactory(
                new SiteEventHandlerStrategy(),
                new CircleEventHandlerStrategy(),
                new SiteGenerator());
            var map = factory.CreateVoronoiMap(20, 20, 6);

            Assert.IsNotNull(map);
        }

        [Test]
        public void TestCreateVoronoiMapArgumentException()
        {
            var factory = new VoronoiFactory(
                   new SiteEventHandlerStrategy(),
                   new CircleEventHandlerStrategy(),
                   new SiteGenerator());

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 95, Y = 75 } },
                new Site { Point = new Point {X = 95, Y = 75 } }
            };

            Assert.Throws<ArgumentException>(() => factory.CreateVoronoiMap(sites));
        }
    }
}