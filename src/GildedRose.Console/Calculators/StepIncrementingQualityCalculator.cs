namespace GildedRose.Console.Calculators
{
    public class StepIncrementingQualityCalculator : IQualityCalculator
    {
        private readonly IQualityIncrementor _incrementor;

        public StepIncrementingQualityCalculator(IQualityIncrementor incrementor)
        {
            _incrementor = incrementor;
        }

        public int Calculate(Item item)
        {
            // Work out the increment.
            var increment = 1;

            if (item.SellIn < 10)
                increment = 2;
            if (item.SellIn < 5)
                increment = 3;

            var newQuality = _incrementor.Increment(item, increment);

            if (item.IsPassedSellbyDate())
            {
                newQuality = 0;
            }

            return newQuality;
        }
    }
}
