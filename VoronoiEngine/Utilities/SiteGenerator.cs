using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Utilities
{
    public class SiteGenerator : ISiteGenerator
    {
        public ICollection<Site> GenerateSites(int x, int y, int quantity)
        {
            var random = new Random();
            var points = new List<Point>();

            while (points.Count < quantity)
            {
                var point = new Point { X = x * random.NextDouble(), Y = y * random.NextDouble() };
                if(!points.Contains(point))
                    points.Add(point);
            }

            var result = points.Select(p => new Site
            {
                Point = p
            });
            return result.ToList();
        }
    }
}