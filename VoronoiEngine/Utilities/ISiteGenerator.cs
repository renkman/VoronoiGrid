using VoronoiEngine.Elements;

namespace VoronoiEngine.Utilities
{
    public interface ISiteGenerator
    {
        ICollection<Site> GenerateSites(int x, int y, int quantity);
    }
}