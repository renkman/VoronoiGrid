using VoronoiEngine.Elements;
using VoronoiEngine.Geomerty;
using Xunit;

namespace VoronoiTests
{
    public class BreakpointCalculationServiceTest
    {
        [Fact]
        public void TestCalculateBreakpoint()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(4, 6), new Point(6, 4), 4);

            Assert.NotNull(result);
            Assert.Equal(new Point(6, 4), result);
        }

        [Fact]
        public void TestCalculateBreakpointLeftRight()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(170, 140), new Point(130, 160), 120);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestCalculateBreakpointRightLeft()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(130, 160), new Point(170, 140), 120);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void TestCalculateBreakpointStep3FirstNode()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(130, 160), new Point(110, 150), 140);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void TestCalculateBreakpointStep3SecondNode()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(110, 150), new Point(130, 160), 140);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void TestCalculateBreakpointStep4()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(110, 150), new Point(170, 140), 120);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestCalculateBreakpointStep5()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(2, 10), new Point(20, 12), 5);

            Assert.NotNull(result);
            Assert.Equal(11, result.XInt);
            Assert.Equal(5, result.YInt);
        }

        [Fact]
        public void TestCalculateBreakpointSameY()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(15, 120), new Point(20, 120), 40);

            Assert.NotNull(result);
            Assert.Equal(18, result.XInt);
            Assert.Equal(40, result.YInt);
        }

        [Fact]
        public void TestCalculateBreakpointLeftOnSweepline()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(15, 120), new Point(20, 12), 120);

            Assert.NotNull(result);
            Assert.Equal(15, result.X);
            Assert.Equal(120, result.Y);
        }

        [Fact]
        public void TestCalculateBreakpointRightOnSweepline()
        {
            var breakpointCalculationService = new BreakpointCalculationService();
            var result = breakpointCalculationService.CalculateBreakpoint(new Point(15, 120), new Point(20, 40), 40);

            Assert.NotNull(result);
            Assert.Equal(20, result.X);
            Assert.Equal(40, result.Y);
        }
    }
}
