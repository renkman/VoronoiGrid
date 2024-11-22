namespace VoronoiEngine.Utilities
{
    public static class IntExtensions
    {
        public static Tuple<int, int> GetGreatestDivisors(this int number)
        {
            if (number == 0)
                return Tuple.Create(1, 0);
            var result = Tuple.Create(number, 1);
            var divisor = number;
            while(divisor > result.Item2)
            {
                if (number % divisor == 0)
                    result = Tuple.Create(divisor, number / divisor);
                divisor--;
            }
            return result;
        }           
    }
}
