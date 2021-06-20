using System;
using Scratch;

namespace CSScratchpad.Script {
    class Motoko : Common, IRunnable {
        public void Run() {
            IWebAppAnalytics appanx = new WebAppAnalytics(new InMemoryProvider(), new AspWebFormVisitorTracker());

            appanx.Track("site accessed");
            appanx.Page();
            appanx.Identify("yui");
            appanx.Page();

            appanx.Track("submit button clicked");
            appanx.Track("submit button clicked", "page: subscription");

            appanx.Reset();


            INonWebAppAnalytics desktanx = new NonWebAppAnalytics(new InMemoryProvider(), new WinFormVisitorTracker());

            desktanx.Track("app opened");
            desktanx.Screen();
            desktanx.Identify("cashier");
            desktanx.Screen("Product.Stock.Opname");

            desktanx.Track("update button clicked");
            desktanx.Track("update button clicked", "screen: stock-opname");
        }

        // tracker cookie representation
        public class AppAnxTracked {
            public String VisitorId { get; set; }
            public String UserId { get; set; }
        }

        // tracker cookie manager
        public interface IVisitorTracker {
            AppAnxTracked Get();
        }

        public class AspWebFormVisitorTracker : IVisitorTracker {
            public AppAnxTracked Get() => new AppAnxTracked();
        }

        public class WinFormVisitorTracker : IVisitorTracker {
            public AppAnxTracked Get() => new AppAnxTracked();
        }

        public interface IStorageProvider {
            
        }

        public class SqlServerProvider : IStorageProvider {
            
        }

        public class InMemoryProvider : IStorageProvider {
            
        }

        public interface IAppAnalyctics {
            void Track(String activity);
            void Track(String activity, String data);
            void Identify(String username);
            void Identity(String username, String firstname);
            void Identify(String username, String firstname, String lastname, String email);
            void IdentifyWithAdditional(String username, String data);
            void Reset();
        }

        public class AppAnalyctics : IAppAnalyctics {
            readonly IStorageProvider storage;
            readonly IVisitorTracker tracker;

            public AppAnalyctics(IStorageProvider storage, IVisitorTracker tracker) {
                this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
                this.tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
            }

            public void Track(String activity) {
                
            }

            public void Track(String activity, String data) {
                
            }

            public void Identify(String username) {
                
            }

            public void Identity(String username, String firstname) {
                
            }

            public void Identify(String username, String firstname, String lastname, String email) {
                
            }

            public void IdentifyWithAdditional(String username, String data) {

            }

            public void Reset() {
                
            }

            // I dont think this is necessary for now
            //public UserProfile GetUser() {

            //}
        }

        public interface IWebAppAnalytics : IAppAnalyctics {
            void Page();
            void Page(String url);
        }

        public class WebAppAnalytics : AppAnalyctics, IWebAppAnalytics {
            public WebAppAnalytics(IStorageProvider storage, IVisitorTracker tracker) : base(storage, tracker) { }

            public void Page() { }

            public void Page(String url) { }
        }

        // For Dekstop, Console, or Mobile?
        public interface INonWebAppAnalytics : IAppAnalyctics {
            void Screen();
            void Screen(String name);
        }

        public class NonWebAppAnalytics : AppAnalyctics, INonWebAppAnalytics {
            public NonWebAppAnalytics(IStorageProvider storage, IVisitorTracker tracker) : base(storage, tracker) { }

            public void Screen() { }

            public void Screen(String name) { }
        }

        // I dont think this is necessary for now
        //public class UserProfile {
        //    public String VisitorId { get; set; }
        //    public String UserId { get; set; }
        //    public String Language { get; set; }
        //    public String FirstName { get; set; }
        //    public String LastName { get; set; }
        //    public String Email { get; set; }
        //    public IDictionary<String, String> Data { get; set; } = new Dictionary<String, String>();
        //}

        // Register and generate new Id here before using analytics system
        public class AppAnxAplication {
            public String AnxId { get; set; } // Unique Id per analytics instance. Set this as unique per site if possible.
            public String App { get; set; } // Application name that host the analytics
        }

        // Will be added everytime identify is called
        public class AppAnxIdentifies {
            public String IdentificationId { get; set; }
            public String VisitorId { get; set; }
            public String UserId { get; set; }
            public DateTime IdentifiedAt { get; set; }
        }

        // Will be added everytime anonymous+userid is different
        // so if analytics cookies still there (visitorId remains intact and not generated anew) but the visitor logged in with different id
        // it will be treated as new record
        public class AppAnxAVisitor {
            public String VisitorId { get; set; }
            public String UserId { get; set; } // at first will be anonymous
            public String Agent { get; set; } // User agent or application name that user use to access the applicatiom (browser agent, "Desktop Client", "Console Client")
            public String Language { get; set; } // Agent language used by user or Http Accept Language
            public String RemoteAddress { get; set; } // Client IP if possible
            public DateTime CreatedAt { get; set; } // is it necessary? I keep this just in case
            public DateTime UpdatedAt { get; set; }
        }

        // Just for the identified user
        public class AppAnxUserProfile {
            public String UserId { get; set; }
            public String FirstName { get; set; }
            public String LastName { get; set; }
            public String Email { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            // Optional
            public String Data { get; set; } // for additional data such as phone no, company, job
        }

        public class AppAnxTracking {
            public String TrackingId { get; set; }
            public String VisitorId { get; set; }
            public String UserId { get; set; }
            public String Activity { get; set; }
            public String Data { get; set; }
            public DateTime TrackedAt { get; set; }
        }
    }
}