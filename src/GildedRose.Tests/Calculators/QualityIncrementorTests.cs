using GildedRose.Console;
using GildedRose.Console.Calculators;
using Xunit;

namespace GildedRose.Tests.Calculators
{
    public class QualityIncrementorTests
    {
        [Theory]
        [InlineData(49, 3)]
        [InlineData(50, 1)]
        public void QuantityCanNotExceedMaximum(int initialQuality, int increment)
        {
            var quality = CalculateQuality(initialQuality, increment);
            Assert.Equal(Constants.MaximumQuality, quality);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(2, -3)]
        public void QuantityCanBeLessThanZero(int initialQuality, int increment)
        {
            var quality = CalculateQuality(initialQuality, increment);
            Assert.Equal(0, quality);
        }

        private int CalculateQuality(int initialQuality, int increment)
        {
            var item = new Item { Quality = initialQuality };
            return new QualityIncrementor().Increment(item, increment);

        }
    }
}
