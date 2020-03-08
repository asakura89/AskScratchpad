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

            var server = new HttpSelfHostServer(conf);
            server.OpenAsync().Wait();
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
}