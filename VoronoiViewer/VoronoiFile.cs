using System.Collections.Generic;
using VoronoiEngine.Elements;

namespace VoronoiViewer
{
    public class VoronoiFile
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public ICollection<Site> Sites {get;set;}
    }
}
