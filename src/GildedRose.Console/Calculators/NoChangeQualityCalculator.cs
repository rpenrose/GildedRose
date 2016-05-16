namespace GildedRose.Console.Calculators
{
    public class NoChangeQualityCalculator : IQualityCalculator
    {
        public int Calculate(Item item)
        {
            return item.Quality;
        }
    }
}
