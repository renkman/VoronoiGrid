using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiEngine.EventHandler
{
    public interface IEventHandlerStrategy<TEvent, TGeometry>
        where TEvent : IEvent
        where TGeometry : IGeometry
    {
        ICollection<TGeometry> HandleEvent(TEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine);
    }
}