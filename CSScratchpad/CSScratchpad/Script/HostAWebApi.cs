using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Scratch;

namespace CSScratchpad.Script {
    internal class HostAWebApi : Common, IRunnable {
        public void Run() {
            String address = "http://localhost:18090";
            var conf = new HttpSelfHostConfiguration(new Uri(address));
            conf.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver());

            conf.Routes.MapHttpRoute(name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
            );

            using (var server = new HttpSelfHostServer(conf)) {
                server.OpenAsync().Wait();
                Console.WriteLine("Hosted!");
                Console.ReadLine();
            }
        }

        class ControllerResolver : DefaultHttpControllerTypeResolver {
            public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver) =>
                Assembly
                    .GetExecutingAssembly()
                    .GetExportedTypes()
                    .Where(t => typeof(IHttpController).IsAssignableFrom(t))
                    .ToList();
        }
    }

    public class User {
        public String Id { get; set; }
        public String Username { get; set; }
    }

    public class UserController : ApiController {
        [HttpGet]
        public Object Get() {
            try {
                return Ok(GetDataList());
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        IList<User> GetDataList() => GetDataListEnum().ToList();

        IEnumerable<User> GetDataListEnum() {
            yield return new User { Id = "00102", Username = "Eburiwea" };
            yield return new User { Id = "37716", Username = "Gurettona" };
            yield return new User { Id = "83629", Username = "Bokuwa" };
        }
    }

    public class VegsController : ApiController {
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

        public IList<FruitsAndVegs> GetByColor(String color) =>
            data
                .Where(fruit => fruit.Color.ToUpperInvariant().Equals(color.ToUpperInvariant()))
                .ToList();
    }
}