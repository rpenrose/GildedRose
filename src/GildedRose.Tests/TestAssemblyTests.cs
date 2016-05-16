using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests
{
    public class TestAssemblyTests
    {
        private const int QualityOf20 = 20;

        private const string OtherProduct = "Any other Product";

        private static readonly IEnumerable<string> AllProductNames = new List<string> { Program.AgedBrie, Program.Sulfuras, Program.BackstagePasses, OtherProduct };

        [Theory]
        [InlineData(Program.Sulfuras)]
        [InlineData(OtherProduct)]
        public void TheQualityDegradesTwiceAsFastAfterSellByDate(string productName)
        {
            // Arrange
            var itemBeforeSellByDate = CreateItemWith(productName, QualityOf20, sellIn: 1);
            var itemAfterSellByDate = CreateItemWith(productName, QualityOf20, sellIn: 0);

            // Act
            ExecuteUpdateQuality(itemBeforeSellByDate, itemAfterSellByDate);

            // Assert
            AssertQualityChangeDoublesAfterSellByDate(productName, QualityOf20, itemBeforeSellByDate, itemAfterSellByDate);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TheQualityOfAnItemIsNeverNegative(int sellInDays)
        {
            // Arrange
            var items = AllProductNames.Select(name => CreateItemWith(name, initialQuality: 0, sellIn: sellInDays)).ToArray();

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
            var item = CreateItemWith(Program.AgedBrie, QualityOf20, sellIn: sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(item.Quality > QualityOf20, $"Sell in days: {sellInDays}");
        }

        [Theory]
        [InlineData(52)]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TheQualityOfAnItemIsNeverMoreThan50(int sellInDays)
        {
            // Arrange
            const int qualityOfFifty = 50;
            var items = AllProductNames.Select(name => CreateItemWith(name, qualityOfFifty, sellInDays)).ToArray();

            // Act
            ExecuteUpdateQuality(items);

            // Assert
            items.ToList().ForEach(item => AssertQualityIsLessThanOrEqualTo(item, qualityOfFifty));
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
            var item = CreateItemWith(Program.Sulfuras, qualityOf80, sellInDays);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.True(qualityOf80 == item.Quality, $"Failed with SellIn days: {sellInDays}");
        }

        [Fact]
        public void BackStagePassQualityIncreasesByOneWhenGreaterThanTenDaysToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(Program.BackstagePasses, QualityOf20, sellIn: 11);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(QualityOf20 + 1, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIncreasesByTwoWhenTenDaysOrLessToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(Program.BackstagePasses, QualityOf20, sellIn: 10);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(QualityOf20 + 2, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIncreasesByThreeWhenFiveDaysOrLessToSellByDate()
        {
            // Arrange
            var item = CreateItemWith(Program.BackstagePasses, QualityOf20, sellIn: 5);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(QualityOf20 + 3, item.Quality);
        }

        [Fact]
        public void BackStagePassQualityIsZeroAfterSellByDate()
        {
            // Arrange
            var item = CreateItemWith(Program.BackstagePasses, QualityOf20, sellIn: 0);

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
            var item = CreateItemWith(Program.AgedBrie, 0, 0);

            // Act
            ExecuteUpdateQuality(item);

            // Assert
            Assert.Equal(2, item.Quality);
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

        private static void AssertQualityIsLessThanOrEqualTo(Item item, int qualityOfFifty)
        {
            Assert.True(item.Quality <= qualityOfFifty, $"Failed for product: {item.Name} with SellIn days: {item.SellIn}");
        }

        private static void AssertQualityGreaterThanOrEqualToZero(Item item)
        {
            Assert.True(item.Quality >= 0, $"Negative quality for product: {item.Name}, SellInDays: {item.SellIn}");
        }

    }
}