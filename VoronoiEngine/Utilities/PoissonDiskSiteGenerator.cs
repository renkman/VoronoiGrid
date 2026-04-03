using VoronoiEngine.Elements;

namespace VoronoiEngine.Utilities
{
    /// <summary>
    /// Generates sites using Poisson Disk Sampling algorithm.
    /// This ensures a minimum distance between all generated sites,
    /// resulting in more natural distribution without clustering.
    /// </summary>
    public class PoissonDiskSiteGenerator : ISiteGenerator
    {
        private const int MaxAttempts = 30;

        public ICollection<Site> GenerateSites(int x, int y, int quantity)
        {
            // Calculate minimum distance based on desired quantity and area
            // Approximation: if we want 'quantity' points in area x*y,
            // the minimum distance should be roughly sqrt(area / quantity)
            double area = x * (double)y;
            double minDistance = Math.Sqrt(area / quantity) * 0.9; // 0.9 factor for better packing

            var points = GeneratePoissonDiskPoints(x, y, minDistance);

            // If we couldn't generate enough points, try with slightly reduced minDistance
            if (points.Count < quantity && minDistance > 1)
            {
                minDistance *= 0.8;
                points = GeneratePoissonDiskPoints(x, y, minDistance);
            }

            // If still not enough, generate the remaining with random placement
            if (points.Count < quantity)
            {
                var random = new Random();
                int attempts = 0;
                int maxAttempts = quantity * 100; // Prevent infinite loop
                while (points.Count < quantity && attempts < maxAttempts)
                {
                    var newPoint = new Point 
                    { 
                        X = x * random.NextDouble(), 
                        Y = y * random.NextDouble() 
                    };
                    
                    if (!points.Any(p => Distance(p, newPoint) < minDistance * 0.7))
                    {
                        points.Add(newPoint);
                    }
                    attempts++;
                }
            }

            // If we generated more points than requested, trim the list randomly
            if (points.Count > quantity)
            {
                var random = new Random();
                points = points.OrderBy(_ => random.Next()).Take(quantity).ToList();
            }

            return points.Select(p => new Site { Point = p }).ToList();
        }

        /// <summary>
        /// Generates points using Fast Poisson Disk Sampling with grid acceleration.
        /// </summary>
        private List<Point> GeneratePoissonDiskPoints(int width, int height, double minDistance)
        {
            var random = new Random();
            double cellSize = minDistance / Math.Sqrt(2);

            int gridWidth = (int)(width / cellSize) + 1;
            int gridHeight = (int)(height / cellSize) + 1;

            var grid = new Point[gridWidth, gridHeight];
            var processingList = new List<Point>();
            var finalPoints = new List<Point>();

            // Start with a random point
            var firstPoint = new Point 
            { 
                X = width * random.NextDouble(), 
                Y = height * random.NextDouble() 
            };
            AddPointToGrid(grid, firstPoint, cellSize, width, height);
            processingList.Add(firstPoint);
            finalPoints.Add(firstPoint);

            while (processingList.Count > 0)
            {
                int idx = random.Next(processingList.Count);
                var point = processingList[idx];
                bool found = false;

                for (int attempt = 0; attempt < MaxAttempts; attempt++)
                {
                    // Random angle and distance
                    double angle = 2 * Math.PI * random.NextDouble();
                    double distance = minDistance + minDistance * random.NextDouble();

                    double newX = point.X + distance * Math.Cos(angle);
                    double newY = point.Y + distance * Math.Sin(angle);

                    // Check bounds
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                        continue;

                    var newPoint = new Point { X = newX, Y = newY };

                    // Check if point is valid (far enough from neighbors)
                    if (IsValidPoint(grid, newPoint, minDistance, cellSize, width, height))
                    {
                        AddPointToGrid(grid, newPoint, cellSize, width, height);
                        processingList.Add(newPoint);
                        finalPoints.Add(newPoint);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    processingList.RemoveAt(idx);
                }
            }

            return finalPoints;
        }

        private bool IsValidPoint(Point[,] grid, Point point, double minDistance, double cellSize, 
            int width, int height)
        {
            int gridX = (int)(point.X / cellSize);
            int gridY = (int)(point.Y / cellSize);

            // Search in neighboring cells
            int searchRadius = 2;
            for (int x = Math.Max(0, gridX - searchRadius); 
                 x <= Math.Min(grid.GetLength(0) - 1, gridX + searchRadius); x++)
            {
                for (int y = Math.Max(0, gridY - searchRadius); 
                     y <= Math.Min(grid.GetLength(1) - 1, gridY + searchRadius); y++)
                {
                    if (grid[x, y] != null)
                    {
                        if (Distance(point, grid[x, y]) < minDistance)
                            return false;
                    }
                }
            }

            return true;
        }

        private void AddPointToGrid(Point[,] grid, Point point, double cellSize, int width, int height)
        {
            int gridX = (int)(point.X / cellSize);
            int gridY = (int)(point.Y / cellSize);

            if (gridX >= 0 && gridX < grid.GetLength(0) && gridY >= 0 && gridY < grid.GetLength(1))
            {
                grid[gridX, gridY] = point;
            }
        }

        private double Distance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}

