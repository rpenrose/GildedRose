using GildedRose.Console;
using GildedRose.Console.Calculators;
using Xunit;

namespace GildedRose.Tests.Calculators
{
    public class StepIncrementingQualityCalculatorTests
    {
        private const int InitialQuality = 10;

        [Fact]
        public void CalculateShouldAddOneWhenMoreThanTenDaysToGo()
        {
            // Act
            var newQuality = CallCalculate(InitialQuality, sellIn: 12);

            // Assert
            Assert.Equal(newQuality, InitialQuality + 1);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(6)]
        public void CalculateShouldAddTwoWhenTenOrLessDaysToGo(int sellIn)
        {
            // Act
            var newQuality = CallCalculate(InitialQuality, sellIn);

            // Assert
            Assert.True(newQuality == InitialQuality + 2, $"Failed with sellin: {sellIn}");
        }

        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        public void CalculateShouldAddThreeWhenFiveOrLessDaysToGo(int sellIn)
        {
            // Act
            var newQuality = CallCalculate(InitialQuality, sellIn);

            // Assert
            Assert.True(newQuality == InitialQuality + 3, $"Failed with sellin: {sellIn}");
        }

        [Fact]
        public void CalculateShouldReturnZeroAfterSellByDate()
        {
            // Act
            var newQuality = CallCalculate(InitialQuality, sellIn: -1);

            // Assert
            Assert.True(newQuality == 0);
        }

        private static int CallCalculate(int initialQuality, int sellIn)
        {
            var calculator = new StepIncrementingQualityCalculator(new QualityIncrementor());
            var newQuality = calculator.Calculate(new Item { Quality = initialQuality, SellIn = sellIn });
            return newQuality;
        }
    }
}
