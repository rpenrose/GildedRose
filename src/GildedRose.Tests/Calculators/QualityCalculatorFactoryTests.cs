using System;
using GildedRose.Console.Calculators;
using Xunit;

namespace GildedRose.Tests.Calculators
{
    public class QualityCalculatorFactoryTests
    {
        [Theory]
        [InlineData("Aged Brie of some kind", typeof(IncrementingQualityCalculator))]
        [InlineData("Backstage passes of some kind", typeof(StepIncrementingQualityCalculator))]
        [InlineData("Sulfuras", typeof(NoChangeQualityCalculator))]
        [InlineData("Other", typeof(IncrementingQualityCalculator))]
        [InlineData("Conjured something or other", typeof(IncrementingQualityCalculator))]
        public void CreateForShouldCreateTheCorrectCalculatorForEachProduct(string productName, Type expectedType)
        {
            var calculator = (new QualityCalculatorFactory()).CreateFor(productName);

            Assert.True(calculator.GetType() == expectedType);
        }
    }
}
