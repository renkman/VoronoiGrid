using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine;
using VoronoiEngine.Elements;
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
            Assert.AreEqual(new Point { X = 136.25, Y = 122.5 }, vertex.Point);

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
                new Site { Point = new Point {X = 95, Y = 130 } }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);
            //Logger.Instance.ToFile();

            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count(g => g is Vertex));
            var vertices = map.Where(g => g is Vertex).Cast<Vertex>().ToList();
            Assert.AreEqual(new Point { X = 136.25, Y = 122.5 }, vertices[0].Point);

            var edgePoints = vertices[0].HalfEdges.Select(e => e.Point).OrderByDescending(p => p.Y).ToList();
            Assert.AreEqual(3, edgePoints.Count);

            //Assert.AreEqual(new Point { X = 170, Y = 190 }, edgePoints[0]);
            //Assert.AreEqual(new Point { X = 110, Y = 175 }, edgePoints[1]);
            //Assert.AreEqual(new Point { X = 61, Y = 139 }, edgePoints[2]);

            //Assert.AreEqual(new Point { X = 146, Y = 104 }, vertices[1].Point);

            edgePoints = vertices[1].HalfEdges.Select(e => e.Point).OrderByDescending(p => p.Y).ToList();
            Assert.AreEqual(3, edgePoints.Count);

            //Assert.AreEqual(new Point { X = 49, Y = 149 }, edgePoints[0]);
            //Assert.AreEqual(new Point { X = 85, Y = 74 }, edgePoints[1]);
            //Assert.AreEqual(new Point { X = 78, Y = 74 }, edgePoints[2]);
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
            Assert.AreEqual(128, vertex.Point.XInt);
            Assert.AreEqual(140, vertex.Point.YInt);
        }

        [Test]
        public void TestCreateVoronoiMapGenerate()
        {
            var factory = new VoronoiFactory();
            var map = factory.CreateVoronoiMap(200, 200, 20);

            Assert.IsNotNull(map);
            //Logger.Instance.ToFile();

            Assert.AreEqual(20, map.Count(g => g is Site));
            Assert.IsTrue(map.Count(g => g is Vertex) >= 10);
        }

        [Test]
        public void TestCreateVoronoiMapGenerateWorld()
        {
            var majorCountriyRegions = 8 * 8;
            var minorCountrieRegions = 8 * 4;
            var seaRegions = 12;
            var total = seaRegions + majorCountriyRegions + minorCountrieRegions;

            var factory = new VoronoiFactory();
            var map = factory.CreateVoronoiMap(total * 8, total * 8, total);

            Assert.IsNotNull(map);
            //Logger.Instance.ToFile();

            Assert.AreEqual(total, map.Count(g => g is Site));
            Assert.IsTrue(map.Count(g => g is Vertex) >= total / 2);
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
        [Ignore("Negative vertex is not allowed")]
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

        [Test]
        public void TestCreateVoroniMapTwiceUsageEdgeVertex()
        {
            var factory = new VoronoiFactory();
            // Point: X: 137, Y: 44, Point: X: 28, Y: 56, Point: X: 120, Y: 180, Point: X: 115, Y: 108, Point: X: 133, Y: 29, Point: X: 109, Y: 52
            var sites = new List<Site>
            {
                new Site { Point = new Point(137, 44) },
                new Site { Point = new Point (28, 56) },
                new Site { Point = new Point (120, 180 ) },
                new Site { Point = new Point (115, 108 ) },
                new Site { Point = new Point (133, 29 )},
                new Site { Point = new Point (109, 52 ) }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);
        }

        [Test]
        public void TestTwentySites()
        {
            var factory = new VoronoiFactory();
            // Point: X: 123, Y: 34, Point: X: 128, Y: 24, Point: X: 70, Y: 157, Point: X: 164, Y: 109, Point: X: 67, Y: 38, Point: X: 152, Y: 196, Point: X: 146, Y: 67, Point: X: 193, Y: 186, Point: X: 136, Y: 94, Point: X: 175, Y: 130, Point: X: 2, Y: 131, Point: X: 19, Y: 174, Point: X: 171, Y: 25, Point: X: 157, Y: 47, Point: X: 65, Y: 80, Point: X: 85, Y: 57, Point: X: 46, Y: 90, Point: X: 58, Y: 116, Point: X: 82, Y: 53, Point: X: 110, Y: 48
            var sites = new List<Site>
            {
               new Site { Point =new Point(123, 34) },
               new Site{Point =new Point(128, 24) },
               new Site{Point =new Point(70, 157)},
               new Site{Point =new Point(164, 109)},
               new Site{Point =new Point(67, 38)},
               new Site{Point =new Point(152, 196)},
               new Site{Point =new Point(146, 67)},
               new Site{Point =new Point(193, 186)},
               new Site{Point =new Point(136, 94)},
               new Site{Point =new Point(175, 130)},
               new Site{Point =new Point(2, 131)},
               new Site{Point =new Point(19, 174)},
               new Site{Point =new Point(171, 25)},
               new Site{Point =new Point(157, 47)},
               new Site{Point =new Point(65, 80)},
               new Site{Point =new Point(85, 57)},
               new Site{Point =new Point(46, 90)},
               new Site{Point =new Point(58, 116)},
               new Site{Point =new Point(82, 53)},
               new Site{Point =new Point(110, 48)}
            };
            try
            {
                var map = factory.CreateVoronoiMap(200, 200, sites);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(e.Message);
                Logger.Instance.Log(e.StackTrace);
                Logger.Instance.ToFile();

                throw e;
            }
        }

        [Test]
        public void TestCreateVoroniMapCase5()
        {
            var factory = new VoronoiFactory();
            var sites = new List<Site>
            {
                new Site { Point = new Point(38, 25) },
                new Site { Point = new Point (3, 4 )},
                new Site { Point = new Point (32, 9 ) },
                new Site { Point = new Point (6, 18 ) }
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);

            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Where(g => g is Vertex).Count());
            Assert.IsTrue(map.Where(g => g is HalfEdge).Cast<HalfEdge>().All(v => v.Point != null), "HalfEdges without start point found");
            Assert.IsTrue(map.Where(g => g is HalfEdge).Cast<HalfEdge>().All(v => v.EndPoint != null), "HalfEdges without end point found");
        }

        [Test]
        public void TestCreateVoroniMapCase7()
        {
            var factory = new VoronoiFactory();
            var sites = new List<Site>
            {
                new Site { Point = new Point(10, 14) },
                new Site { Point = new Point (77, 44 )},
                new Site { Point = new Point (25, 78 )}
            };

            var map = factory.CreateVoronoiMap(200, 200, sites);

            Assert.IsNotNull(map);
            Assert.AreEqual(3, map.Where(g => g is Site).Count());
            Assert.AreEqual(1, map.Where(g => g is Vertex).Count());
            Assert.AreEqual(3, map.Where(g => g is HalfEdge).Count());
            Assert.IsTrue(map.Where(g => g is HalfEdge).Cast<HalfEdge>().All(v => v.Point != null), "HalfEdges without start point found");
            Assert.IsTrue(map.Where(g => g is HalfEdge).Cast<HalfEdge>().All(v => v.EndPoint != null), "HalfEdges without end point found");
        }

        [Test]
        public void TestCreateVoroniMapReference_4()
        {
            var factory = new VoronoiFactory();
            var sites = new List<Site>
            {
                new Site { Point = new Point(3144.322031, 1178.930021)},
                new Site { Point = new Point(3488.265633, 7350.077822)},
                new Site { Point = new Point(2765.587329, 4289.376507)},
                new Site { Point = new Point(5102.694784, 1361.430708)}
            };

            var map = factory.CreateVoronoiMap(10000, 10000, sites);
            Logger.Instance.ToFile();
            Assert.IsNotNull(map);
            Assert.AreEqual(4, map.Where(g => g is Site).Count());
            Assert.AreEqual(2, map.Where(g => g is Vertex).Count());

            var edges = map.Where(g => g is HalfEdge).Cast<HalfEdge>().OrderBy(e => e.Point?.Y).ThenBy(e => e.Point?.X).ThenBy(e => e.EndPoint?.Y).ThenBy(e => e.EndPoint?.X).ToList();
            Assert.AreEqual(5, edges.Count);

            Assert.AreEqual(new Point(3975.500678, 2858.417155), edges[0].Point);
            Assert.AreEqual(new Point(10000, -61789.094684), edges[0].EndPoint);
            Assert.AreEqual(new Point(3975.500678, 2858.417155), edges[1].Point);
            Assert.AreEqual(new Point(0, 2374.351581), edges[1].EndPoint);
            Assert.AreEqual(new Point(6644.831322, 4989.096129), edges[2].Point);
            Assert.AreEqual(new Point(3975.500678, 2858.417155), edges[2].EndPoint);
            Assert.AreEqual(new Point(6644.831322, 4989.096129), edges[3].Point);
            Assert.AreEqual(new Point(10000.000000, 5893.587914), edges[3].EndPoint);
            Assert.AreEqual(new Point(6644.831322, 4989.096129), edges[4].Point);
            Assert.AreEqual(new Point(0, 6558.042241), edges[4].EndPoint);
        }

        [Test]
        public void TestCreateVoroniMapReference_10()
        {
            var factory = new VoronoiFactory();
            var sites = new List<Site>
            {
                new Site { Point = new Point(3018.280587,5848.567156)},
                new Site { Point = new Point(5414.288766,7961.058382)},
                new Site { Point = new Point(2826.929533,3501.693777)},
                new Site { Point = new Point(2722.861415,4667.806024)},
                new Site { Point = new Point(3614.001892,8095.645009)},
                new Site { Point = new Point(318.308054, 5330.668050)},
                new Site { Point = new Point(5021.820734,5896.176031)},
                new Site { Point = new Point(8200.628681,769.066439)},
                new Site { Point = new Point(8472.243416,9083.223975)},
                new Site { Point = new Point(5433.515427,5861.995300)}
            };

            var map = factory.CreateVoronoiMap(10000, 10000, sites);
            Logger.Instance.ToFile();
            Assert.IsNotNull(map);
            Assert.AreEqual(10, map.Where(g => g is Site).Count());
            Assert.AreEqual(13, map.Where(g => g is Vertex).Count());

            var edges = map.Where(g => g is HalfEdge).Cast<HalfEdge>().OrderBy(e => e.Point?.Y).ThenBy(e => e.Point?.X).ThenBy(e => e.EndPoint?.Y).ThenBy(e => e.EndPoint?.X).ToList();
            Assert.AreEqual(22, edges.Count);

            Assert.AreEqual(new Point(5845.460792, 2787.630623), edges[0].Point);
            Assert.AreEqual(new Point(0, -8707.441259), edges[0].EndPoint);
            Assert.AreEqual(new Point(5044.444914, 3672.228064), edges[1].Point);
            Assert.AreEqual(new Point(5845.460792, 2787.630623), edges[1].EndPoint);
            Assert.AreEqual(new Point(1230.493526, 3946.921828), edges[2].Point);
            Assert.AreEqual(new Point(0, 2259.176662), edges[2].EndPoint);
            Assert.AreEqual(new Point(4432.981152, 4232.723525), edges[3].Point);
            Assert.AreEqual(new Point(5044.444914, 3672.228064), edges[3].EndPoint);
            Assert.AreEqual(new Point(4432.981152, 4232.723525), edges[4].Point);
            Assert.AreEqual(new Point(1230.493526, 3946.921828), edges[4].EndPoint);
            Assert.AreEqual(new Point(9699.48373, 4881.615881), edges[5].Point);
            Assert.AreEqual(new Point(5845.460792, 2787.630623), edges[5].EndPoint);
            Assert.AreEqual(new Point(9699.48373, 4881.615881), edges[6].Point);
            Assert.AreEqual(new Point(10000, 4871.798332), edges[6].EndPoint);
            Assert.AreEqual(new Point(4041.607201, 4965.200537), edges[7].Point);
            Assert.AreEqual(new Point(4432.981152, 4232.723525), edges[7].EndPoint);
            Assert.AreEqual(new Point(4041.607201, 4965.200537), edges[8].Point);
            Assert.AreEqual(new Point(1674.465592, 5557.444806), edges[8].EndPoint);
            Assert.AreEqual(new Point(1674.465592, 5557.444806), edges[9].Point);
            Assert.AreEqual(new Point(1230.493526, 3946.921828), edges[9].EndPoint);
            Assert.AreEqual(new Point(3998.215168, 6791.28192), edges[10].Point);
            Assert.AreEqual(new Point(4041.607201, 4965.200537), edges[10].EndPoint);
            Assert.AreEqual(new Point(3998.215168, 6791.28192), edges[11].Point);
            Assert.AreEqual(new Point(1300.61617, 7506.440584), edges[11].EndPoint);
            Assert.AreEqual(new Point(5313.301846, 6910.513783), edges[12].Point);
            Assert.AreEqual(new Point(5044.444914, 3672.228064), edges[12].EndPoint);
            Assert.AreEqual(new Point(5313.301846, 6910.513783), edges[13].Point);
            Assert.AreEqual(new Point(4442.944585, 7075.940835), edges[13].EndPoint);
            Assert.AreEqual(new Point(7527.23701, 6930.79263), edges[14].Point);
            Assert.AreEqual(new Point(9699.48373, 4881.615881), edges[14].EndPoint);
            Assert.AreEqual(new Point(7527.23701, 6930.79263), edges[15].Point);
            Assert.AreEqual(new Point(5313.301846, 6910.513783), edges[15].EndPoint);
            Assert.AreEqual(new Point(7527.23701, 6930.79263), edges[16].Point);
            Assert.AreEqual(new Point(4955.922475, 13937.747117), edges[16].EndPoint);
            Assert.AreEqual(new Point(4442.944585, 7075.940835), edges[17].Point);
            Assert.AreEqual(new Point(3998.215168, 6791.28192), edges[17].EndPoint);
            Assert.AreEqual(new Point(1300.61617, 7506.440584), edges[18].Point);
            Assert.AreEqual(new Point(1674.465592, 5557.444806), edges[18].EndPoint);
            Assert.AreEqual(new Point(1300.61617, 7506.440584), edges[19].Point);
            Assert.AreEqual(new Point(0, 9056.700409), edges[19].EndPoint);
            Assert.AreEqual(new Point(4955.922475, 13937.747117), edges[20].Point);
            Assert.AreEqual(new Point(4442.944585, 7075.940835), edges[20].EndPoint);
            Assert.AreEqual(new Point(4955.922475, 13937.747117), edges[21].Point);
            Assert.AreEqual(new Point(0, 38317.638919), edges[21].EndPoint);
        }

        [Test]
        public void TestCreateVoronoiMapEdgeCaseRootSameY()
        {
            var factory = new VoronoiFactory();

            var sites = new List<Site>
            {
                new Site { Point = new Point { X = 60, Y = 60 } },
                new Site { Point = new Point { X = 20, Y = 60 } }
            };

            var map = factory.CreateVoronoiMap(80, 80, sites);

            Assert.IsNotNull(map);
            var halfEdge = map.Single(g => g is HalfEdge) as HalfEdge;
            Assert.AreEqual(new Point { X = 40, Y = 80 }, halfEdge.Point);
            Assert.AreEqual(new Point { X = 40, Y = 0 }, halfEdge.EndPoint);
        }

        [Test]
        [Ignore("Integer numbers generate edge cases for sweepline Y comparision in the circle event determination")]
        public void TestCreateVoronoiMapCorruptWith3()
        {
            var sitesOk = new List<Site>
            {
                new Site { Point = new Point {X=60.0,Y=70.0 } },
                new Site { Point = new Point {X=38.0,Y=73.0 } },
                new Site { Point = new Point {X=50.0,Y=65.0 } },
            };

            var factory = new VoronoiFactory();

            var sitesCorrupt = new List<Site>
            {
                new Site { Point = new Point {X=60.0,Y=70.0 } },
                new Site { Point = new Point {X=38.0,Y=74.0 } },
                new Site { Point = new Point {X=50.0,Y=65.0 } },
            };

            var mapOk = factory.CreateVoronoiMap(100, 100, sitesOk);
            Logger.Instance.ToFile();

            var mapCorrupt = factory.CreateVoronoiMap(100, 100, sitesCorrupt);
            Logger.Instance.ToFile();

            Assert.IsNotNull(mapOk);
            Assert.IsNotNull(mapCorrupt);

            var halfEdgesOk = mapOk.Where(g => g is HalfEdge).Cast<HalfEdge>();
            var halfEdgesCorrupt = mapCorrupt.Where(g => g is HalfEdge).Cast<HalfEdge>();

            var verticesOk = mapOk.Where(g => g is Vertex).Cast<Vertex>();
            var verticesCorrupt = mapCorrupt.Where(g => g is Vertex).Cast<Vertex>();

            //Assert.AreEqual(halfEdgesOk.Count(), halfEdgesCorrupt.Count());
            Assert.AreEqual(verticesOk.Count(), verticesCorrupt.Count());
        }
    }
}