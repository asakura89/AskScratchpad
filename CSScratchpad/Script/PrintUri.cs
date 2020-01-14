using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security.AntiXss;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintUri : Common, IRunnable {
        public void Run() {
            String address = "http://localhost:8090/Services/Employee?dtFromString=20170801&dtToString=20170830&name=john hashkell&jobs=towns+mayor";
            var uri = new Uri(address);

            Dbg(new {
                AllUriProps = uri,
                PartialAuthority = uri.GetLeftPart(UriPartial.Authority),
                PartialPath = uri.GetLeftPart(UriPartial.Path),
                PartialQuery = uri.GetLeftPart(UriPartial.Query),
                PartialScheme = uri.GetLeftPart(UriPartial.Scheme),
                ForHttpClient = uri.GetLeftPart(UriPartial.Authority),
                ForGetAsync = uri.AbsolutePath,
                NotAllUriProps = new {
                    uri.Host,
                    uri.AbsolutePath,
                    uri.AbsoluteUri,
                    uri.Authority,
                    uri.DnsSafeHost,
                    uri.Port,
                    uri.Fragment,
                    uri.HostNameType,
                    uri.IdnHost,
                    uri.IsAbsoluteUri,
                    uri.IsDefaultPort,
                    uri.IsFile,
                    uri.IsLoopback,
                    uri.IsUnc,
                    uri.LocalPath,
                    uri.OriginalString,
                    uri.PathAndQuery,
                    uri.Query,
                    uri.Scheme,
                    uri.Segments,
                    uri.UserEscaped,
                    uri.UserInfo
                }
            });

            var uriB = new UriBuilder(address);
            NameValueCollection query = HttpUtility.ParseQueryString(uriB.Query);
            query["dtFromString"] = HttpUtility.UrlEncode(query["dtFromString"]);
            query["dtToString"] = HttpUtility.UrlEncode(query["dtToString"]);
            query["name"] = HttpUtility.UrlEncode(query["name"]);
            query["jobs"] = HttpUtility.UrlEncode(query["jobs"]);
            uriB.Query = query.ToString();

            var uriC = new UriBuilder(address);
            NameValueCollection query2 = HttpUtility.ParseQueryString(uriC.Query);
            query2["dtFromString"] = HttpUtility.UrlPathEncode(query2["dtFromString"]);
            query2["dtToString"] = HttpUtility.UrlPathEncode(query2["dtToString"]);
            query2["name"] = HttpUtility.UrlPathEncode(query2["name"]);
            query2["jobs"] = HttpUtility.UrlPathEncode(query2["jobs"]);
            uriC.Query = query2.ToString();

            var uriD = new UriBuilder(address);
            NameValueCollection query3 = HttpUtility.ParseQueryString(uriD.Query);
            query3["dtFromString"] = HttpUtility.UrlEncode(query3["dtFromString"]).Replace("+","%20");
            query3["dtToString"] = HttpUtility.UrlEncode(query3["dtToString"]).Replace("+", "%20");
            query3["name"] = HttpUtility.UrlEncode(query3["name"]).Replace("+", "%20");
            query3["jobs"] = HttpUtility.UrlEncode(query3["jobs"]).Replace("+", "%20");
            uriD.Query = query3.ToString();

            var uriE = new UriBuilder(address);
            NameValueCollection query4 = HttpUtility.ParseQueryString(uriE.Query);
            query4["dtFromString"] = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(query4["dtFromString"]));
            query4["dtToString"] = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(query4["dtToString"]));
            query4["name"] = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(query4["name"]));
            query4["jobs"] = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(query4["jobs"]));
            uriE.Query = HttpUtility.UrlDecode(query4.ToString());

            Dbg(new {
                Original = address,
                Modified = uriB.ToString(),
                Modified2 = uriC.ToString(),
                Modified3 = uriD.ToString(),
                Modified4 = uriE.ToString()
            });
        }
    }
}
