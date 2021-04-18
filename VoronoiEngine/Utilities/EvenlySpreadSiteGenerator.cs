using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Utilities
{
    public class EvenlySpreadSiteGenerator : ISiteGenerator
    {
        const int Margin = 2;

        public ICollection<Site> GenerateSites(int x, int y, int quantity)
        {
            var max = Math.Max(x, y);
            var min = Math.Min(x, y);

            var divisors = quantity.GetGreatestDivisors();
            
            var stepX = x / divisors.Item1 * 1f;
            var stepY = y / divisors.Item2 * 1f;

            var positions = GeneratePositions(divisors.Item1, stepX, divisors.Item2, stepY).ToList();
            return positions.Select(p => new Site { Point = p }).ToList();
        }
        
        private static IEnumerable<Point> GeneratePositions(int countX, double stepX, int countY, double stepY)
        {
            var random = new Random();
            for (var i = 0; i < countX; i++)
            {
                for (var j = 0; j < countY; j++)
                {
                    var minX = i * stepX;
                    var maxX = minX + stepX;
                    var minY = j * stepY;
                    var maxY = minY + stepY;

                    var x = random.Range(minX + Margin, maxX - Margin);
                    var y = random.Range(minY + Margin, maxY - Margin);

                    yield return new Point(x, y);
                }
            }
        }
    }
}