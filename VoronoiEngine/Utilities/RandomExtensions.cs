namespace VoronoiEngine.Utilities
{
    public static class RandomExtensions
    {
        public static double Range(this Random random, double min, double max)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            if (min >= max)
                throw new ArgumentException($"{nameof(min)} is greater equal {nameof(max)}");

            return min + (max - min) * random.NextDouble();
        }
    }
}