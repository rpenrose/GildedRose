using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        private const string AgedBrie = "Aged Brie";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string OtherProduct = "Any other Product";

        private static readonly IEnumerable<string> AllProductNames = new List<string> { AgedBrie, Sulfuras, BackstagePasses, OtherProduct };

        [Theory]
        [InlineData(Sulfuras, 20)]
        [InlineData(OtherProduct, 20)]
        public void TheQualityDegradesTwiceAsFastAfterSellByDate(string productName, int initialQuality)
        {
            // Arrange
            var itemWithPositiveSellin = CreateItemWith(productName, initialQuality, sellIn: 1);
            var itemWithNegativeSellin = CreateItemWith(productName, initialQuality, sellIn: 0);

            // Act
            ExecuteUpdateQuality(itemWithPositiveSellin, itemWithNegativeSellin);

            // Assert
            var degradationOfPositive = initialQuality - itemWithPositiveSellin.Quality;
            var degradationOfNegative = initialQuality - itemWithNegativeSellin.Quality;
            Assert.True(degradationOfNegative == (2 * degradationOfPositive),
                $"Product: {productName}, Initial Quantity : {initialQuality}");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TheQualityOfAnItemIsNeverBeNegative(int sellInDays)
        {
            // Arrange
            var items = AllProductNames.Select(name => CreateItemWith(name, initialQuality: 0, sellIn: sellInDays)).ToArray();

            // Act
            ExecuteUpdateQuality(items);

            // Assert
            Assert.False(items.Any(i => i.Quality < 0), $"Negative Quantity found with Sell in days = {sellInDays}");
        }

        [Fact]
        public void BackStagePassQualityIncreasesByTwoWhenTenDaysOrLess()
        {
            // Arrange
            const int initialQuality = 20;
            var item = CreateItemWith(BackstagePasses, initialQuality, sellIn: 10);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(initialQuality + 2, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIncreasesByThreeWhenFiveDaysOrLess()
        {
            // Arrange
            const int initialQuality = 20;
            var item = CreateItemWith(BackstagePasses, initialQuality, sellIn: 5);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(initialQuality + 3, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIsZeroAfterConcert()
        {
            // Arrange
            var item = CreateItemWith(BackstagePasses, initialQuality: 10, sellIn: 0);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(0, item.Quality);
        }

        [Theory]
        [InlineData(AgedBrie, 20)]
        [InlineData(BackstagePasses, 20)]
        public void AgedBrieAndBackStagePassesIncreaseInQualityAsTheyGetOlder(string productName, int initialQuality)
        {
            // Arrange
            var item = CreateItemWith(productName, initialQuality, 20);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(item.Quality > initialQuality, $"Product: {productName}, Initial Quantity : {initialQuality}");
        }

        //TODO - This is not a specified requirement, but is a feature that has been detected in testing. Need to check with product owner.
        [Fact]
        public void AgedBrieQualityIncreaseTwiceAsFastAfterSellByDate()
        {
            // Arrange
            var item = CreateItemWith(AgedBrie, 0, 0);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(2, item.Quality);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void SulfurasNeverDecreasesInQuality(int sellInDays)
        {
            // Arrange
            const int initialQuality = 10;
            var item = CreateItemWith(Sulfuras, initialQuality, sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(initialQuality == item.Quality, $"Failed with SellIn days: {sellInDays}");
        }

        [Theory]
        [InlineData(AgedBrie, 1)]
        [InlineData(AgedBrie, 0)]
        [InlineData(BackstagePasses, 10)]
        [InlineData(BackstagePasses, 5)]
        [InlineData(BackstagePasses, 10)]
        [InlineData(BackstagePasses, 0)]
        public void TheQualityOfAnItemIsNeverMoreThan50(string productName, int sellInDays)
        {
            // Arrange
            const int qualityOfFifty = 50;
            var item = CreateItemWith(productName, qualityOfFifty, sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(item.Quality <= qualityOfFifty, $"Failed for product: {productName}, with SellIn days: {sellInDays}");
        }

        private static Item CreateItemWith(string productName, int initialQuality, int sellIn)
        {
            return new Item { Name = productName, Quality = initialQuality, SellIn = sellIn };
        }

        private void ExecuteUpdateQuality(params Item[] items)
        {
            var program = new Program { CurrentItems = items };
            program.UpdateQuality();
        }
    }
}