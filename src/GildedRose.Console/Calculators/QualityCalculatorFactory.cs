namespace GildedRose.Console.Calculators
{
    public interface IQualityCalculatorFactory
    {
        IQualityCalculator CreateFor(string productName);
    }

    public class QualityCalculatorFactory : IQualityCalculatorFactory
    {
        public IQualityCalculator CreateFor(string productName)
        {
            if (productName.StartsWith(Constants.AgedBrie))
                return new IncrementingQualityCalculator(new QualityIncrementor(), -1);

            if (productName.StartsWith(Constants.BackstagePasses))
                return new StepIncrementingQualityCalculator(new QualityIncrementor());

            if (productName.StartsWith(Constants.Sulfuras))
                return new NoChangeQualityCalculator();

            return new IncrementingQualityCalculator(new QualityIncrementor(), 1);
        }
    }
}
