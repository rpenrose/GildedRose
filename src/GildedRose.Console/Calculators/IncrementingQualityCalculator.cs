namespace GildedRose.Console.Calculators
{
    public class IncrementingQualityCalculator : IQualityCalculator
    {
        private readonly IQualityIncrementor _incrementor;
        private readonly int _increment;

        public IncrementingQualityCalculator(IQualityIncrementor incrementor, int increment)
        {
            _incrementor = incrementor;
            _increment = increment;
        }

        public int Calculate(Item item)
        {
            var increment = _increment * ((item.SellIn < 0) ? -2 : -1);

            var newQuality = _incrementor.Increment(item, increment);

            return newQuality;
        }

    }
}
