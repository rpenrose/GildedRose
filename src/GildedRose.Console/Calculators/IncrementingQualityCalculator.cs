namespace GildedRose.Console.Calculators
{
    public class IncrementingQualityCalculator : IQualityCalculator
    {
        private readonly int _increment;

        public IncrementingQualityCalculator(int increment)
        {
            _increment = increment;
        }

        public int Calculate(Item item)
        {
            var increment = _increment * ((item.SellIn < 0) ? -2 : -1);

            var newQuality = IncrementQuality(item, increment);

            return newQuality;
        }

        private static int IncrementQuality(Item item, int increment = 1)
        {
            var quality = item.Quality + increment;

            if (quality < 0)
                quality = 0;

            return quality > Program.MaximumQuality ? Program.MaximumQuality : quality;
        }
    }
}
