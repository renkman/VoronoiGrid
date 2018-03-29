using System;
using System.Collections.Generic;
using VoronoiEngine;
using VoronoiEngine.Elements;

namespace VoronoiConsole
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var factory = new VoronoiFactory();

            //var sites = new List<Point>
            //{
            //    new Point { X = 40, Y = 60 },
            //    new Point { X = 20, Y = 40 },
            //    new Point { X = 60, Y = 40 }
            //};

            var sites = new List<Site>
            {
               new Site { Point = new  Point { X = 130, Y = 160 } },
               new Site { Point = new Point { X = 110, Y = 150 } },
               new Site { Point = new Point { X = 170, Y = 140 } }
            };

            var map = factory.CreateVoronoiMap(sites);
            //var map = factory.CreateVoronoiMap(20, 20, 6);

            var mapArray = map.ToArray();

            for (var i = mapArray.GetLength(1) - 1; i >= 0 ; i--)
            {
                for (var j = mapArray.GetLength(0) - 1; j >= 0; j--)
                {
                    var geo = mapArray[j, i];
                    if (geo == null)
                    {
                        Console.Write(" ");
                        continue;
                    }
                    if (geo is Site)
                    {
                        Console.Write("S");
                        continue;
                    }
                    if (geo is HalfEdge)
                    {
                        Console.Write("H");
                        continue;
                    }
                    if (geo is Vertex)
                        Console.Write("V");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}