namespace GildedRose.Console.Calculators
{
    public class StepIncrementingQualityCalculator : IQualityCalculator
    {
        public int Calculate(Item item)
        {
            // Work out the increment.
            var increment = 1;

            if (item.SellIn < 10)
                increment = 2;
            if (item.SellIn < 5)
                increment = 3;

            var newQuality = IncrementQuality(item, increment);

            if (item.SellIn < 0)
            {
                newQuality = 0;
            }

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
