using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.Geomerty;

namespace VoronoiTests
{
    [TestFixture]
    public class BreakpointCalculationServiceTest
    {
        [Test]
        public void TestCalculateBreakpoint()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(4, 6), new Point(6, 4), 4);

            Assert.IsNotNull(result);
            Assert.AreEqual(new Point(6, 4), result);
        }
    }
}
