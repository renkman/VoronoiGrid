using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public interface IBoundingBoxService
    {
        Vertex AttachHalfEdge(HalfEdge halfEdge, int height, int width);
    }
}