using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Scratch;

namespace CSScratchpad.Script {
    public class SelfHostAspWebApi : Common, IRunnable {
        public void Run() {
            var config = new HttpSelfHostConfiguration("http://localhost:5003");
            config.Routes.MapHttpRoute("Api Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            using (var server = new HttpSelfHostServer(config)) {
                server.OpenAsync().Wait();
                Console.WriteLine("Hosted!");
                Console.ReadLine();
            }
        }
    }

    public class DataApiController : ApiController {
        public class FruitsAndVegs {
            public String Type { get; set; }
            public String Color { get; set; }
            public String Name { get; set; }
        };

        readonly IList<FruitsAndVegs> data = new List<FruitsAndVegs> {
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

        public IList<FruitsAndVegs> GetAll() => data;

        public  IList<FruitsAndVegs> GetByColor(String color) =>
            data
                .Where(fruit => fruit.Color.ToUpperInvariant().Equals(color.ToUpperInvariant()))
                .ToList();
    }
}
