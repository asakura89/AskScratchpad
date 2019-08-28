using System;

namespace CSScratchpad.Script {
    public class PrintUri : Common, IRunnable {
        public void Run() {
            String address = "http://localhost:8090/Services/Employee?dtFromString=20170801&dtToString=20170830";
            var uri = new Uri(address);

            Dbg("All", uri);
            Dbg("partial authority", uri.GetLeftPart(UriPartial.Authority));
            Dbg("partial path", uri.GetLeftPart(UriPartial.Path));
            Dbg("partial query", uri.GetLeftPart(UriPartial.Query));
            Dbg("partial scheme", uri.GetLeftPart(UriPartial.Scheme));
            Dbg("for HttpClient", uri.GetLeftPart(UriPartial.Authority));
            Dbg("for GetAsync", uri.AbsolutePath);
        }
    }
}
