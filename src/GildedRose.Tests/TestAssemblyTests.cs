using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        private const int ArbitraryQuality = 20;

        private const string OtherProduct = "Any old product";
        private const string AgedBrie = "Aged Brie particularly smelly variety";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Conjured = "Conjured Mana Cake";

        private static readonly IEnumerable<string> AllProductNames = new List<string> { AgedBrie, Sulfuras, BackstagePasses, OtherProduct, Conjured };

        [Theory]
        [InlineData(Sulfuras)]
        [InlineData(OtherProduct)]
        public void TheQualityDegradesTwiceAsFastAfterSellByDate(string productName)
        {
            // Arrange
            var itemBeforeSellByDate = CreateItemWith(productName, ArbitraryQuality, sellIn: 1);
            var itemAfterSellByDate = CreateItemWith(productName, ArbitraryQuality, sellIn: 0);

            // Act
            ExecuteUpdateQuality(itemBeforeSellByDate, itemAfterSellByDate);

            // Assert
            AssertQualityChangeDoublesAfterSellByDate(productName, ArbitraryQuality, itemBeforeSellByDate, itemAfterSellByDate);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        public void TheQualityOfAnItemIsNeverNegative(int sellInDays, int quality)
        {
            // Arrange
            var items = AllProductNames.Select(name => CreateItemWith(name, quality, sellInDays)).ToArray();

            // Act
            ExecuteUpdateQuality(items);

            // Assert
            items.ToList().ForEach(AssertQualityGreaterThanOrEqualToZero);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void AgedBrieQualityIncreaseInQualityAsItGetsOlder(int sellInDays)
        {
            // Arrange
            var item = CreateItemWith(AgedBrie, ArbitraryQuality, sellIn: sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(item.Quality > ArbitraryQuality, $"Sell in days: {sellInDays}");
        }

        [Theory]
        [InlineData(1, Constants.MaximumQuality)]
        [InlineData(0, Constants.MaximumQuality)]
        [InlineData(-1, Constants.MaximumQuality)]
        [InlineData(1, Constants.MaximumQuality - 1)]
        [InlineData(0, Constants.MaximumQuality - 1)]
        [InlineData(-1, Constants.MaximumQuality - 1)]
        public void TheQualityOfAnItemDoesntIncreaseAboveThanMaximum(int sellInDays, int quality)
        {
            // Arrange
            var items = AllProductNames.Select(name => CreateItemWith(name, quality, sellInDays)).ToArray();

            // Act
            ExecuteUpdateQuality(items);

            // Assert
            items.ToList().ForEach(item => AssertQualityIsLessThanOrEqualTo(item, Constants.MaximumQuality));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void SulfurasNeverDecreasesInQuality(int sellInDays)
        {
            // Arrange
            const int qualityOf80 = 80;
            var item = CreateItemWith(Sulfuras, qualityOf80, sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(qualityOf80 == item.Quality, $"Failed with SellIn days: {sellInDays}");
        }

        [Fact]
        public void BackStagePassQualityIncreasesByOneWhenGreaterThanTenDaysToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(BackstagePasses, ArbitraryQuality, sellIn: 11);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(ArbitraryQuality + 1, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIncreasesByTwoWhenTenDaysOrLessToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(BackstagePasses, ArbitraryQuality, sellIn: 10);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(ArbitraryQuality + 2, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIncreasesByThreeWhenFiveDaysOrLessToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(BackstagePasses, ArbitraryQuality, sellIn: 5);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(ArbitraryQuality + 3, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIsZeroAfterSellByDate()
        {
            // Arrange
            var item = CreateItemWith(BackstagePasses, ArbitraryQuality, sellIn: 0);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(0, item.Quality);
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
        public void ConjuredItemsDegradeTwiceAsFast(int sellInDays)
        {
            var quality = 20;

            // Arrange
            var conjuredItem = CreateItemWith(Conjured, quality, sellInDays);
            var normalItem = CreateItemWith(OtherProduct, quality, sellInDays);

            // Act
            ExecuteUpdateQuality(conjuredItem, normalItem);

            // Assert
            var changeForConjured = quality - conjuredItem.Quality;
            var changeForNormal = quality - normalItem.Quality;
            Assert.True(changeForConjured == (2 * changeForNormal));
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

        private static void AssertQualityChangeDoublesAfterSellByDate(string productName,
                                int initialQuality, Item itemBeforeSellByDate, Item itemAfterSellByDate)
        {
            var changeBeforeSellByDate = initialQuality - itemBeforeSellByDate.Quality;
            var changeAfterSellByDate = initialQuality - itemAfterSellByDate.Quality;
            var changeAfterIsDouble = changeAfterSellByDate == (2 * changeBeforeSellByDate);

            Assert.True(changeAfterIsDouble, $"Product: {productName}, Initial Quantity : {initialQuality}");
        }

        private static void AssertQualityIsLessThanOrEqualTo(Item item, int quality)
        {
            Assert.True(item.Quality <= quality, $"Failed for product: {item.Name} with SellIn days: {item.SellIn}");
        }

        private static void AssertQualityGreaterThanOrEqualToZero(Item item)
        {
            Assert.True(item.Quality >= 0, $"Negative quality for product: {item.Name}, SellInDays: {item.SellIn}");
        }

    }
}