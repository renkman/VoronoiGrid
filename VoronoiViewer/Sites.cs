using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;

public class Sites : List<Point>
{
    public static Sites Create(ICollection<Site> sites)
    {
        var result = new Sites();
        result.AddRange(sites.Select(s => s.Point));
        return result;
    }
}