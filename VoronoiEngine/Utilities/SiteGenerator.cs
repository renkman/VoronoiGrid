using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Utilities
{
    public class SiteGenerator
    {
        public ICollection<Site> GenerateSites(int x, int y, int quantity)
        {
            var random = new Random();

            var result = Enumerable.Range(1, quantity).Select(n => new Site {
                Point = new Point { X = random.Next(x), Y = random.Next(y) }
            });
            return result.ToList();
        }
    }
}