using NUnit.Framework;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Structures;

namespace VoronoiTests
{
    [TestFixture]
    public class EventQueueTest
    {
        [Test]
        public void TestHasEvents()
        {
            var sites = new[] { new Point() { X = 1, Y = 2 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            Assert.IsTrue(eventQueue.HasEvents);
        }

        [Test]
        public void TestHasNoEvents()
        {
            var eventQueue = new EventQueue();

            Assert.IsFalse(eventQueue.HasEvents);
        }

        [Test]
        public void TestGetNextEvent()
        {
            var sites = new[] { new Point() { X = 1, Y = 2 }, new Point() { X = 1, Y = 1 }, new Point() { X = 1, Y = 5 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.AreEqual(resultSet[0].Point, sites[2]);
            Assert.AreEqual(resultSet[1].Point, sites[0]);
            Assert.AreEqual(resultSet[2].Point, sites[1]);
            Assert.AreEqual(resultSet[3], null);
        }

        [Test]
        public void TestInsert()
        {
            var sites = new[] { new Point() { X = 1, Y = 16 }, new Point() { X = 1, Y = 64 } };
            var circleEvent = new CircleEvent { Point = new Point { X = 2, Y = 32 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            eventQueue.Insert(circleEvent);
            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.AreEqual(resultSet[0].Point, sites[1]);
            Assert.AreEqual(resultSet[1], circleEvent);
            Assert.AreEqual(resultSet[2].Point, sites[0]);
        }

        [Test]
        public void TestRemove()
        { 
            var sites = new[] { new Point() { X = 1, Y = 64 }, new Point() { X = 1, Y = 16 } };
            var circleEvent = new CircleEvent { Point = new Point { X = 2, Y = 32 } };
            var eventQueue = new EventQueue();
            eventQueue.Initialize(sites);

            eventQueue.Insert(circleEvent);
            eventQueue.Remove(circleEvent);
            var resultSet = new[] { eventQueue.GetNextEvent(), eventQueue.GetNextEvent(), eventQueue.GetNextEvent() };

            Assert.AreEqual(resultSet[0].Point, sites[0]);
            Assert.AreEqual(resultSet[1].Point, sites[1]);
        }
    }
}