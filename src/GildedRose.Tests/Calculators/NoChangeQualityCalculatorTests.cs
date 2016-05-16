using GildedRose.Console;
using GildedRose.Console.Calculators;
using Xunit;

namespace GildedRose.Tests.Calculators
{
    public class NoChangeQualityCalculatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void CalculateShouldAlwaysReturnTheOriginalQualityValue(int sellIn)
        {
            var initialQuality = 10;

            var newQuality = (new NoChangeQualityCalculator()).Calculate(new Item {Quality = initialQuality, SellIn = sellIn});

            Assert.True(newQuality==initialQuality, $"Failed for sellIn: {sellIn}");
        }

    }
}
