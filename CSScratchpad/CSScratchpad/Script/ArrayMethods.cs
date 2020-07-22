using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Scratch;

namespace CSScratchpad.Script {
    public class ArrayMethods : Common, IRunnable {
        class FruitsAndVegsJsonParsed {
            public String src { get; set; }
            public IList<FruitsAndVegs> data { get; set; }
        }

        class FruitsAndVegs {
            public String Type { get; set; }
            public String Color { get; set; }
            public String Name { get; set; }
        };

        readonly List<FruitsAndVegs> data = new List<FruitsAndVegs> {
            new FruitsAndVegs { Type = "Fruit", Color = "Red", Name = "Red Apples" },
            new FruitsAndVegs { Type = "Fruit", Color = "Red", Name = "Blood Oranges" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Red", Name = "Beets" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Red", Name = "Red Peppers" },
            new FruitsAndVegs { Type = "Fruit", Color = "Yellow/Orange", Name = "Yellow Apples" },
            new FruitsAndVegs { Type = "Fruit", Color = "Yellow/Orange", Name = "Apricots" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Yellow/Orange", Name = "Yellow Apples" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Yellow/Orange", Name = "Apricots" },
            new FruitsAndVegs { Type = "Fruit", Color = "Blue/Purple", Name = "Blackberries" },
            new FruitsAndVegs { Type = "Fruit", Color = "Blue/Purple", Name = "Blueberries" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Blue/Purple", Name = "Black Olives" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Blue/Purple", Name = "Purple Asparagus" },
            new FruitsAndVegs { Type = "Fruit", Color = "White/Tan/Brown", Name = "Bananas" },
            new FruitsAndVegs { Type = "Fruit", Color = "White/Tan/Brown", Name = "Dates" },
            new FruitsAndVegs { Type = "Vegetables", Color = "White/Tan/Brown", Name = "Cauliflower" },
            new FruitsAndVegs { Type = "Vegetables", Color = "White/Tan/Brown", Name = "Garlic" },
            new FruitsAndVegs { Type = "Fruit", Color = "Green", Name = "Avocados" },
            new FruitsAndVegs { Type = "Fruit", Color = "Green", Name = "Green Apples" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Green", Name = "Artichokes" },
            new FruitsAndVegs { Type = "Vegetables", Color = "Green", Name = "Arugula" }
        };

        public void Run() {
            //IList<FruitsAndVegs> data = JsonConvert.DeserializeObject<FruitsAndVegsJsonParsed>(File.ReadAllText(GetDataPath("fruits-vegs.json"))).data;

            /*
            var newJson = String.Join(Environment.NewLine, data
                .GroupBy(f => f.Type.ToLowerInvariant() + "_" + f.Color.ToLowerInvariant())
                .SelectMany(grp => grp.Take(2))
                .Select(f => $"new FruitsAndVegs {{ Type = \"{f.Type}\", Color = \"{f.Color}\", Name = \"{f.Name}\" }},")
            );

            Dbg(newJson);
            */

            /* .:: ForEach ::. */
            data.ForEach(item => Console.WriteLine(item));


            data
                .Where(item => item.Type == "Fruit" && item.Color == "Green")
                .Select(fruit => fruit.Name)
                .ToList()
                .ForEach(fName => Console.WriteLine(fName));

            /*
            var greenFruitNames = data
                .Where(item => item.Type == "Fruit" && item.Color == "Green")
                .Select(fruit => fruit.Name);

            Console.WriteLine(JsonConvert.SerializeObject(greenFruitNames, Formatting.Indented));
            */
        }
    }
}
