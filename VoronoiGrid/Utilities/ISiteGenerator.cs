using System.Collections.Generic;
using VoronoiGrid.Geometry;

namespace VoronoiEngine.Utilities
{
    public interface ISiteGenerator<TGeometry> where TGeometry : IGeometry
    {
        ICollection<TGeometry> GenerateSites(int x, int y, int quantity);
    }
}