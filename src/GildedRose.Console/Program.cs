using System.Collections.Generic;
using GildedRose.Console.Calculators;

namespace GildedRose.Console
{
    public class Program
    {
        private readonly IQualityCalculatorFactory _qualityCalculatorFactory;
        // Do not alter this property due to Goblin ownership..
        IList<Item> Items;

        public Program(IQualityCalculatorFactory qualityCalculatorFactory)
        {
            _qualityCalculatorFactory = qualityCalculatorFactory;
        }

        public Program() : this(new QualityCalculatorFactory())
        {            
        }

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

                var qualityCalculator = _qualityCalculatorFactory.CreateFor(item.Name);

                item.Quality = qualityCalculator.Calculate(item);
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
