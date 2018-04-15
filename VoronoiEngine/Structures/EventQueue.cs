using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Structures
{
    public class EventQueue
    {
        private IList<IEvent> _events;

        public EventQueue()
        {
            _events = new List<IEvent>();
        }

        public bool HasEvents
        {
            get
            {
                return _events.Any();
            }
        }

        public void Initialize(IEnumerable<Point> sites)
        {
            if (sites == null)
                throw new ArgumentException(nameof(sites));

            _events = sites.OrderByDescending(s => s.Y).Select(s => new SiteEvent { Point = s }).Cast<IEvent>().ToList();
        }

        public IEvent GetNextEvent()
        {
            var next = _events.FirstOrDefault();
            if (next == null)
                return null;

            _events.Remove(next);
            return next;
        }

        public void Insert(CircleEvent circleEvent)
        {
            if (circleEvent == null)
                return;

            var successor = _events.Where(e => e.Point.Y < circleEvent.Point.Y).FirstOrDefault();
            var index = Math.Max(_events.IndexOf(successor), 0);
            _events.Insert(index, circleEvent);
        }

        public void Insert(IEnumerable<CircleEvent> circleEvents)
        {
            if (circleEvents == null)
                return;

            foreach (var circleEvent in circleEvents)
                Insert(circleEvent);
        }

        public void Remove(CircleEvent circleEvent)
        {
            // Remove circle event
            if (_events.Contains(circleEvent))
                _events.Remove(circleEvent);

            //// And remove circle events containing the current circle event center arc
            //var relatedEvents = _events.Where(e => e is CircleEvent).Cast<CircleEvent>().Where(c => c.LeftArc == circleEvent.CenterArc || c.RightArc == circleEvent.CenterArc);
            //foreach (var relatedEvent in relatedEvents)
            //    _events.Remove(relatedEvent);
        }
    }
}