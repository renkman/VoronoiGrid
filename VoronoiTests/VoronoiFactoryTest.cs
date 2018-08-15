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
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point { X = 40, Y = 60 } },
                new Site { Point = new Point { X = 20, Y = 40 } },
                new Site { Point = new Point { X = 60, Y = 40 } }
            };

            var map = factory.CreateVoronoiMap(80, 80, sites);

            Assert.IsNotNull(map);
            var vertex = map.Single(g => g is Vertex);
            Assert.AreEqual(new Point { X = 40, Y = 40 }, vertex.Point);

            //Logger.Instance.ToFile();
            var halfEges = map.Where(g => g is HalfEdge).ToList();
            Assert.AreEqual(3, halfEges.Count());
        }

        [Test]
        public void TestCreateVoronoiMapWithThreeSites()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 110, Y = 150 } },
                new Site { Point = new Point {X = 170, Y = 140 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);

            Assert.IsNotNull(map);
            var vertex = map.Single(g => g is Vertex);
            Assert.AreEqual(new Point { X = 136, Y = 121 }, vertex.Point);

            //Logger.Instance.ToFile();

            var halfEges = map.Where(g => g is HalfEdge).ToList();
            Assert.AreEqual(3, halfEges.Count());
        }

        [Test]
        public void TestCreateVoronoiMapWithFourSites()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 110, Y = 150 } },
                new Site { Point = new Point {X = 170, Y = 140 } },
                new Site { Point = new Point {X = 95, Y = 75 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);
            //Logger.Instance.ToFile();

            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count(g => g is Vertex));
            var vertices = map.Where(g => g is Vertex).Cast<Vertex>().ToList();
            Assert.AreEqual(new Point { X = 136, Y = 121 }, vertices[0].Point);

            var edgePoints = vertices[0].HalfEdges.Select(e => e.Point).OrderByDescending(p => p.Y).ToList();
            Assert.AreEqual(3, edgePoints.Count);

            Assert.AreEqual(new Point { X = 49, Y = 149 }, edgePoints[0]);
            Assert.AreEqual(new Point { X = 74, Y = 139 }, edgePoints[1]);
            Assert.AreEqual(new Point { X = 61, Y = 139 }, edgePoints[2]);

            Assert.AreEqual(new Point { X = 146, Y = 104 }, vertices[1].Point);

            edgePoints = vertices[1].HalfEdges.Select(e => e.Point).OrderByDescending(p => p.Y).ToList();
            Assert.AreEqual(3, edgePoints.Count);

            Assert.AreEqual(new Point { X = 49, Y = 149 }, edgePoints[0]);
            Assert.AreEqual(new Point { X = 85, Y = 74 }, edgePoints[1]);
            Assert.AreEqual(new Point { X = 78, Y = 74 }, edgePoints[2]);
        }

        [Test]
        public void TestCreateVoronoiMapWithFalseAlarm()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 110, Y = 150 } },
                new Site { Point = new Point {X = 170, Y = 140 } },
                new Site { Point = new Point {X = 130, Y = 120 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);

            Assert.IsNotNull(map);
            var vertex = map.First(g => g is Vertex);
            Assert.AreEqual(new Point { X = 136, Y = 121 }, vertex.Point);
        }

        [Test]
        //[Ignore("Runs too long!")]
        public void TestCreateVoronoiMapGenerate()
        {
            var factory = new VoronoiFactory();
            var map = factory.CreateVoronoiMap(200, 200, 6);

            Assert.IsNotNull(map);
            Assert.AreEqual(6, map.Count(g => g is Site));
            //Assert.AreEqual(6, map.Count(g => g is Vertex));
        }

        [Test]
        public void TestCreateVoronoiMapArgumentException()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 130, Y = 160 } },
                new Site { Point = new Point {X = 95, Y = 75 } },
                new Site { Point = new Point {X = 95, Y = 75 } }
            };

            Assert.Throws<ArgumentException>(() => factory.CreateVoronoiMap(200, 200, sites));
        }

        [Test]
        public void TestCreateVoronoiMapTwoVertices()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 24, Y = 0 } },
                new Site { Point = new Point {X = 13, Y = 2 } },
                new Site { Point = new Point {X = 7, Y = 10 } }
            };

            var map = factory.CreateVoronoiMap(50, 50, sites);

            var vertices = map.Where(g => g is Vertex).OrderBy(v => v.Point.X).ToList();
            //Assert.AreEqual(vertices[0].Point, new Point { X = 18, Y = 9 });
            //Assert.AreEqual(new Point { X = 21, Y = 14 }, vertices[0].Point);
        }

        [Test]
        public void TestCreateVoronoiMapTwoVertexNegative()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 11, Y = 21 } },
                new Site { Point = new Point {X = 10, Y = 3 } },
                new Site { Point = new Point {X = 12, Y = 7 } }
            };

            var map = factory.CreateVoronoiMap(30, 30, sites);

            var vertex = map.Single(g => g is Vertex);
            Assert.AreEqual(vertex.Point, new Point { X = -5, Y = 13 });
        }

        [Test]
        public void TestCreateVoronoiMapVertexOutOfBounds()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 4, Y = 0 } },
                new Site { Point = new Point {X = 11, Y = 19 } },
                new Site { Point = new Point {X = 2, Y = 5 } }
            };

            var map = factory.CreateVoronoiMap(40, 40, sites);
            //Logger.Instance.ToFile();

            //var vertex = map.Single(g => g is Vertex);
            //Assert.AreEqual(new Point { X = 14, Y = 7 }, vertex.Point);
        }

        [Test]
        public void TestCreateVoronoiMapVertexException()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 145, Y = 110 } },
                new Site { Point = new Point {X = 81, Y = 197 } },
                new Site { Point = new Point {X = 3, Y = 10 } },
                new Site { Point = new Point {X = 53, Y = 66 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);
            //Logger.Instance.ToFile();

            var vertices = map.Where(g => g is Vertex).OrderBy(v => v.Point.X).ToList();
            //Assert.AreEqual(new Point { X = 79, Y = 130 }, vertices[1].Point);
            //Assert.AreEqual(new Point { X = -122, Y = 172 }, vertices[0].Point);
        }

        [Test]
        public void TestCreateVoronoiMapInfiniteProcessing()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point {X = 128, Y = 81 } },
                new Site { Point = new Point {X = 149, Y = 129 } },
                new Site { Point = new Point {X = 116, Y = 99 } },
                new Site { Point = new Point {X = 56, Y = 113 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);
            //Logger.Instance.ToFile();

            //var vertices = map.Where(g => g is Vertex).ToList();
            //Assert.AreEqual(vertices[0].Point, new Point { X = 97, Y = 153 });
            //Assert.AreEqual(vertices[1].Point, new Point { X = 103, Y = 118 });
        }
    }
}