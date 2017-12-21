using VoronoiEngine.Elements;

namespace VoronoiEngine.Structures
{
    public class Sweepline
    {
        public Sweepline()
        {
            Root = new Node();
        }

        public Node Root { get; }

        public void Remove(INode leaf)
        {
            //TODO: Implementieren
        }
    }
}