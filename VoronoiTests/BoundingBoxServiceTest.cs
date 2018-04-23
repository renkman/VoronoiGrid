using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTests
{
    [TestFixture]
    public class BoundingBoxServiceTest
    {
        [Test]
        public void TestArcSinus()
        {
            var sinA = 5 / 10d;
            var angle = Math.Asin(sinA);
            var degrees = (int)(angle * (180 / Math.PI));

            Assert.AreEqual(30, degrees);
        }
    }
}
