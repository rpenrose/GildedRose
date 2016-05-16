using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {

        public const string AgedBrie = "Aged Brie";
        public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        public const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";

        public const int MaximumQuality = 50;

        // Do not alter this property due to Goblin ownership..
        IList<Item> Items;

        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
            {
                Items = new List<Item>
                                          {
                                              new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                              new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                              new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                              new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = "Backstage passes to a TAFKAL80ETC concert",
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                          }

            };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                item.SellIn = item.SellIn - 1;

                switch (item.Name)
                {
                    case AgedBrie:
                        AdjustQualityUsingStepIncrement(item, -1);
                        break;
                    case BackstagePasses:
                        AdjustQualityUsingSlidingScale(item);
                        break;
                    case Sulfuras:
                        break;
                    default:
                        AdjustQualityUsingStepIncrement(item, 1);
                        break;
                }
            }
        }

        public void AdjustQualityUsingSlidingScale(Item item)
        {
            // Work out the increment.
            var increment = 1;

            if (item.SellIn < 10)
                increment = 2;
            if (item.SellIn < 5)
                increment = 3;

            item.Quality = IncrementQuality(item, increment);

            if (item.SellIn < 0)
            {
                item.Quality = 0;
            }
        }

        private static void AdjustQualityUsingStepIncrement(Item item, int initialIncrement)
        {
            var increment = initialIncrement * ((item.SellIn < 0) ? -2 : -1);

            item.Quality = IncrementQuality(item, increment);
        }

        private static int IncrementQuality(Item item, int increment = 1)
        {
            var quality = item.Quality + increment;

            if (quality < 0)
                quality = 0;

            return quality > MaximumQuality ? MaximumQuality : quality;
        }

        public void UpdateQualityOld()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name != "Aged Brie" && Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
                {
                    if (Items[i].Quality > 0)
                    {
                        if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                        {
                            Items[i].Quality = Items[i].Quality - 1;
                        }
                    }
                }
                else
                {
                    if (Items[i].Quality < 50)
                    {
                        Items[i].Quality = Items[i].Quality + 1;

                        if (Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
                        {
                            if (Items[i].SellIn < 11)
                            {
                                if (Items[i].Quality < 50)
                                {
                                    Items[i].Quality = Items[i].Quality + 1;
                                }
                            }

                            if (Items[i].SellIn < 6)
                            {
                                if (Items[i].Quality < 50)
                                {
                                    Items[i].Quality = Items[i].Quality + 1;
                                }
                            }
                        }
                    }
                }

                if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                {
                    Items[i].SellIn = Items[i].SellIn - 1;
                }

                if (Items[i].SellIn < 0)
                {
                    if (Items[i].Name != "Aged Brie")
                    {
                        if (Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
                        {
                            if (Items[i].Quality > 0)
                            {
                                if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                                {
                                    Items[i].Quality = Items[i].Quality - 1;
                                }
                            }
                        }
                        else
                        {
                            Items[i].Quality = Items[i].Quality - Items[i].Quality;
                        }
                    }
                    else
                    {
                        if (Items[i].Quality < 50)
                        {
                            Items[i].Quality = Items[i].Quality + 1;
                        }
                    }
                }
            }
        }

        public IList<Item> CurrentItems
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    // Do not alter this class due to Goblin ownership..
    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
