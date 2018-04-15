using System.Collections.Generic;
using VoronoiGrid.Events;
using VoronoiGrid.Structures.Tree;

namespace VoronoiGrid.Services
{
    public interface ICircleEventCalculationService<TEvent> where TEvent : IEvent
    {
        TEvent DetermineCircleEvent(ICollection<INode> arcs);
    }
}