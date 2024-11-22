using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiEngine.EventHandler
{
    public interface IEventHandlerStrategy<TEvent>
        where TEvent : IEvent
    {
        ICollection<IGeometry> HandleEvent(TEvent sweepEvent, EventQueue eventQueue, BeachLine beachLine);
    }
}