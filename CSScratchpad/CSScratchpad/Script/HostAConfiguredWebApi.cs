using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using System.Web.SessionState;
using Newtonsoft.Json;
using Scratch;

namespace CSScratchpad.Script {
    internal class HostAConfiguredWebApi : Common, IRunnable {
        public void Run() {
            var conf = new HttpSelfHostConfiguration(new Uri("http://localhost:18090"));
            WebApiConfiguration.ControllerResolve(conf);
            WebApiConfiguration.Route(conf);
            WebApiConfiguration.UseJsonOnly(conf);
            // WebApiConfiguration.EnableSessionInHttpContext(); // NOTE: got null. need to research.

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

        static class WebApiConfiguration {
            public static void ControllerResolve(HttpConfiguration config) {
                // NOTE: if already add as reference then no need to resolve as assemblies
                // config.Services.Replace(typeof(IAssembliesResolver), new ServiceAssembliesResolver());
                // config.Services.Replace(typeof(IHttpControllerActivator), new InjectedControllerActivator());
                config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver());
            }

            public static void Route(HttpConfiguration config) {
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}"
                );

                //config.MapHttpAttributeRoutes();
            }

            public static void EnableSessionInHttpContext() {
                // NOTE: to enable session on web api services
                /*Boolean isWebApiRequest = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("Services");
                if (isWebApiRequest)
                    HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);*/

                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }

            public static void UseJsonOnly(HttpConfiguration config) {
                MediaTypeFormatterCollection formatters = config.Formatters;
                formatters.Remove(formatters.XmlFormatter);

                JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;
                jsonFormatter.SerializerSettings = new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc };
            }
        }
    }

    public class Student {
        public String Id { get; set; }
        public String Name { get; set; }
    }

    public class StudentController : ApiController {
        [HttpGet]
        public Object Get() {
            try {
                return Ok(GetDataList());
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        IList<Student> GetDataList() => GetDataListEnum().ToList();

        IEnumerable<Student> GetDataListEnum() {
            yield return new Student { Id = "93846", Name = "Bokuno" };
            yield return new Student { Id = "95716", Name = "Maekara" };
            yield return new Student { Id = "91846", Name = "Kimigakiete" };
        }
    }

    public class FruitsController : ApiController {
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