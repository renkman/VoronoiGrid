using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;
using Xunit;

namespace VoronoiTests
{
    public class EventQueueTest
    {
        [Fact]
        public void TestHasEvents()
        {
            var sites = new[] { new Point() { X = 1, Y = 2 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            Assert.True(eventQueue.HasEvents);
        }

        [Fact]
        public void TestHasNoEvents()
        {
            var eventQueue = new EventQueue();

            Assert.False(eventQueue.HasEvents);
        }

        [Fact]
        public void TestGetNextEvent()
        {
            var sites = new[] { new Point() { X = 1, Y = 2 }, new Point() { X = 1, Y = 1 }, new Point() { X = 1, Y = 5 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.Equal(resultSet[0].Point, sites[2]);
            Assert.Equal(resultSet[1].Point, sites[0]);
            Assert.Equal(resultSet[2].Point, sites[1]);
            Assert.Equal(resultSet[3], null);
        }

        [Fact]
        public void TestInsert()
        {
            var sites = new[] { new Point() { X = 1, Y = 16 }, new Point() { X = 1, Y = 64 } };
            var circleEvent = new CircleEvent { Point = new Point { X = 2, Y = 32 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            eventQueue.Insert(circleEvent);
            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.Equal(resultSet[0].Point, sites[1]);
            Assert.Equal(resultSet[1], circleEvent);
            Assert.Equal(resultSet[2].Point, sites[0]);
        }

        [Fact]
        public void TestRemove()
        { 
            var sites = new[] { new Point() { X = 1, Y = 64 }, new Point() { X = 1, Y = 16 } };
            var circleEvent = new CircleEvent { Point = new Point { X = 2, Y = 32 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            eventQueue.Insert(circleEvent);
            eventQueue.Remove(circleEvent);
            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.Equal(resultSet[0].Point, sites[0]);
            Assert.Equal(resultSet[1].Point, sites[1]);
        }
    }
}