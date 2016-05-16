namespace GildedRose.Console.Calculators
{
    public interface IQualityIncrementor
    {
        int Increment(Item item, int increment = 1);
    }

    public class QualityIncrementor : IQualityIncrementor
    {
        public int Increment(Item item, int increment = 1)
        {
            var quality = item.Quality + increment;

            if (quality < 0)
                quality = 0;

            return quality > Constants.MaximumQuality ? Constants.MaximumQuality : quality;
        }
    }
}
