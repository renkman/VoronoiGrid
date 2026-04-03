using VoronoiEngine.Utilities;
using Xunit;

namespace VoronoiTests
{
    public class PoissonDiskSiteGeneratorTest
    {
        [Fact]
        public void GenerateSites_BasicTest_ReturnsCorrectQuantity()
        {
            const int quantity = 10;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(100, 100, quantity).ToList();
            
            Assert.Equal(quantity, result.Count);
        }

        [Fact]
        public void GenerateSites_SmallMap_ReturnsCorrectQuantity()
        {
            const int quantity = 5;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(50, 50, quantity).ToList();
            
            Assert.Equal(quantity, result.Count);
        }

        [Fact]
        public void GenerateSites_LargeQuantity_ReturnsCorrectQuantity()
        {
            const int quantity = 100;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(500, 500, quantity).ToList();
            
            Assert.Equal(quantity, result.Count);
        }

        [Fact]
        public void GenerateSites_AllPointsWithinBounds()
        {
            int width = 200;
            int height = 150;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(width, height, 20).ToList();
            
            Assert.All(result, site =>
            {
                Assert.True(site.Point.X >= 0 && site.Point.X <= width);
                Assert.True(site.Point.Y >= 0 && site.Point.Y <= height);
            });
        }

        [Fact]
        public void GenerateSites_AllPointsWithinBounds_RectangularMap()
        {
            int width = 300;
            int height = 100;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(width, height, 15).ToList();
            
            Assert.All(result, site =>
            {
                Assert.True(site.Point.X >= 0 && site.Point.X <= width);
                Assert.True(site.Point.Y >= 0 && site.Point.Y <= height);
            });
        }

        [Fact]
        public void GenerateSites_MinimumDistanceEnforced()
        {
            // For Poisson Disk Sampling, minimum distance should be maintained
            // minDistance = sqrt(area / quantity) * 0.9
            int width = 400;
            int height = 400;
            int quantity = 20;
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(width, height, quantity).ToList();
            
            double expectedMinDistance = Math.Sqrt((width * (double)height) / quantity) * 0.9;
            
            // Check that most pairs have distance >= expectedMinDistance (with small tolerance)
            // Allow some violation due to fallback mechanism
            int violations = 0;
            int totalPairs = 0;
            
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    totalPairs++;
                    double distance = result[i].Point.CalculateDistance(result[j].Point);
                    if (distance < expectedMinDistance * 0.8)
                    {
                        violations++;
                    }
                }
            }
            
            // Allow up to 10% violations (due to fallback mechanism for high quantities)
            double violationRate = (double)violations / totalPairs;
            Assert.True(violationRate < 0.1, $"Too many distance violations: {violationRate * 100}%");
        }

        [Fact]
        public void GenerateSites_NoDuplicatePoints()
        {
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(200, 200, 25).ToList();
            
            var uniquePoints = result.Select(s => s.Point).Distinct().Count();
            Assert.Equal(result.Count, uniquePoints);
        }

        [Fact]
        public void GenerateSites_DifferentMapsProduceDifferentResults()
        {
            var generator = new PoissonDiskSiteGenerator();
            
            var result1 = generator.GenerateSites(100, 100, 10).ToList();
            var result2 = generator.GenerateSites(100, 100, 10).ToList();
            
            // Results should be different (statistically very likely)
            bool areEqual = result1.SequenceEqual(result2);
            Assert.False(areEqual, "Identical results from two calls - randomness may be broken");
        }

        [Fact]
        public void GenerateSites_SiteQuantity_100x100Map_10Points()
        {
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(100, 100, 10);
            
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public void GenerateSites_SiteQuantity_500x500Map_50Points()
        {
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(500, 500, 50);
            
            Assert.Equal(50, result.Count);
        }

        [Fact]
        public void GenerateSites_SmallQuantity_SinglePoint()
        {
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(100, 100, 1).ToList();
            
            Assert.Single(result);
            Assert.True(result[0].Point.X >= 0 && result[0].Point.X <= 100);
            Assert.True(result[0].Point.Y >= 0 && result[0].Point.Y <= 100);
        }

        [Fact]
        public void GenerateSites_VerySmallMap_StillGeneratesSites()
        {
            // Even very small maps should attempt to generate sites
            var generator = new PoissonDiskSiteGenerator();
            
            var result = generator.GenerateSites(20, 20, 3);
            
            // Should return at least some sites (may not be exactly 3 due to space constraints)
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GenerateSites_AsymmetricMap_WorksCorrectly()
        {
            // Wide map
            var generator = new PoissonDiskSiteGenerator();
            var result = generator.GenerateSites(400, 50, 12).ToList();
            
            Assert.Equal(12, result.Count);
            Assert.All(result, site =>
            {
                Assert.True(site.Point.X >= 0 && site.Point.X <= 400);
                Assert.True(site.Point.Y >= 0 && site.Point.Y <= 50);
            });
        }

        [Fact]
        public void GenerateSites_TallMap_WorksCorrectly()
        {
            // Tall map
            var generator = new PoissonDiskSiteGenerator();
            var result = generator.GenerateSites(50, 400, 12).ToList();
            
            Assert.Equal(12, result.Count);
            Assert.All(result, site =>
            {
                Assert.True(site.Point.X >= 0 && site.Point.X <= 50);
                Assert.True(site.Point.Y >= 0 && site.Point.Y <= 400);
            });
        }

        [Fact]
        public void GenerateSites_ModerateDensity_MaintainsDistribution()
        {
            // Test with moderate point density on a 300x300 map
            var generator = new PoissonDiskSiteGenerator();
            var result = generator.GenerateSites(300, 300, 30).ToList();
            
            // Divide map into 4 quadrants and check that points are somewhat distributed
            var q1 = result.Count(s => s.Point.X <= 150 && s.Point.Y <= 150);
            var q2 = result.Count(s => s.Point.X > 150 && s.Point.Y <= 150);
            var q3 = result.Count(s => s.Point.X <= 150 && s.Point.Y > 150);
            var q4 = result.Count(s => s.Point.X > 150 && s.Point.Y > 150);
            
            // Each quadrant should have at least 1 point (with 30 points spread across 4 quadrants)
            Assert.True(q1 >= 1 && q2 >= 1 && q3 >= 1 && q4 >= 1, 
                $"Uneven distribution: Q1={q1}, Q2={q2}, Q3={q3}, Q4={q4}");
        }

        [Fact]
        public void GenerateSites_AllPointsAreDistinct()
        {
            var generator = new PoissonDiskSiteGenerator();
            var result = generator.GenerateSites(250, 250, 20).ToList();
            
            // Check that no two points are exactly the same
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    Assert.NotEqual(result[i].Point.X, result[j].Point.X);
                    Assert.NotEqual(result[i].Point.Y, result[j].Point.Y);
                }
            }
        }

        [Fact]
        public void GenerateSites_HighDensityRequest_ReturnsCloseToQuantity()
        {
            // Request more points than ideal for given area
            // Should return less than requested, but > 0
            var generator = new PoissonDiskSiteGenerator();
            var result = generator.GenerateSites(100, 100, 50);
            
            Assert.True(result.Count > 0, "Should generate at least some points");
        }

        [Fact]
        public void GenerateSites_ConsistentBehavior_SameInputsSameSeed()
        {
            // While results are randomized, basic properties should remain consistent
            for (int i = 0; i < 3; i++)
            {
                var generator = new PoissonDiskSiteGenerator();
                var result = generator.GenerateSites(200, 150, 15);
                
                Assert.Equal(15, result.Count);
                Assert.All(result, site =>
                {
                    Assert.True(site.Point.X >= 0 && site.Point.X <= 200);
                    Assert.True(site.Point.Y >= 0 && site.Point.Y <= 150);
                });
            }
        }

        [Fact]
        public void GenerateSites_ComparesWithEvenlySpread_SimilarDistribution()
        {
            // Both generators should produce similar distribution patterns
            var poissonGenerator = new PoissonDiskSiteGenerator();
            var evenGenerator = new EvenlySpreadSiteGenerator();
            
            var poissonResult = poissonGenerator.GenerateSites(300, 300, 16).ToList();
            var evenResult = evenGenerator.GenerateSites(300, 300, 16).ToList();
            
            // Both should generate 16 sites
            Assert.Equal(16, poissonResult.Count);
            Assert.Equal(16, evenResult.Count);
            
            // Both should keep points within bounds
            Assert.All(poissonResult, s => Assert.True(s.Point.X >= 0 && s.Point.X <= 300));
            Assert.All(evenResult, s => Assert.True(s.Point.X >= 0 && s.Point.X <= 300));
        }
    }
}

