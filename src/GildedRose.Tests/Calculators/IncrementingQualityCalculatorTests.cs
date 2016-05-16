using GildedRose.Console;
using GildedRose.Console.Calculators;
using Xunit;

namespace GildedRose.Tests.Calculators
{
    public class IncrementingQualityCalculatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public void CalculateBeforSellByDateShouldSubtractTheSpecifiedIncrement(int increment)
        {
            // Arrange
            var initialQuality = 2;

            // Act
            var newQuality = CallCalculate(increment, initialQuality, sellIn: 2);

            // Assert
            Assert.Equal(newQuality, initialQuality - increment);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        public void CalculateAfterSellByDateShouldSubtractDoubleTheSpecifiedIncrement(int increment)
        {
            // Arrange
            var initialQuality = 10;

            // Act
            var newQuality = CallCalculate(increment, initialQuality, sellIn: -1);

            // Assert
            Assert.Equal(newQuality, initialQuality - (2 * increment));
        }

        [Fact]
        public void CalculateCantReurnZero()
        {
            // Arrange
            var initialQuality = 0;

            // Act
            var newQuality = CallCalculate(2, initialQuality, sellIn: 2);

            // Assert
            Assert.Equal(0, newQuality);
        }


        private static int CallCalculate(int increment, int initialQuality, int sellIn)
        {
            var calculator = new IncrementingQualityCalculator(increment);
            var newQuality = calculator.Calculate(new Item { Quality = initialQuality, SellIn = sellIn });
            return newQuality;
        }
    }
}
