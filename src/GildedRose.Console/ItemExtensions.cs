namespace GildedRose.Console
{
    public static class ItemExtensions
    {
        public static bool IsPassedSellbyDate(this Item item)
        {
            return item.SellIn < 0;
        }
    }
}
