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
}