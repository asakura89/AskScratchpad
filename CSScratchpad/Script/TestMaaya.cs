using System;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    class TestMaaya : Common, IRunnable {
        public void Run() {
            Console.WriteLine("Start.");

            String[] urls = new[] {
                "",
                null,
                " ",
                "/",
                "/Home/Index",
                "/Dashboard",
                "/Non",
                "~",
                "~/",
                "~/Home/Index",
                "~/Dashboard",
                "~/Non",
                "/~/",
                "../",
                "../Home.aspx",
                "Home/Index.aspx",
                "http:app.localweb.net",
                "http:/app.localweb.net",
                "https:app.localweb.net",
                "https:/app.localweb.net",
                "http://app.localweb.net/Home/Index",
                "https://app.localweb.net/Home/Index",
                "https://google.com"
            };

            Dbg("IsLocalUrl:",
                urls.Select(url => new {
                    Url = url,
                    Local = url.IsLocalUrl(new Uri("http://app.localweb.net"))
                })
            );

            Dbg("IsWellFormedUriString:",
                urls.Select(url => new {
                    Url = url,
                    Absolute = Uri.IsWellFormedUriString(url, UriKind.Absolute),
                    Relative = Uri.IsWellFormedUriString(url, UriKind.Relative)
                })
            );

            Func<String, Boolean> isAbsolute = url => {
                try {
                    new Uri(url);
                }
                catch (ArgumentNullException ane) {
                    return false;
                }
                catch (UriFormatException ufe) {
                    return false;
                }

                return true;
            };

            Dbg("Not Absolute:",
                urls
                    .Select(url => new {
                        Url = url,
                        Absolute = isAbsolute(url)
                    })
            );

            Dbg("Host:",
                urls
                    .Skip(urls.Length -3)
                    .Select(url => new {
                        Url = url,
                        Host = new Uri(url).Host,
                        DnsSafeHost = new Uri(url).DnsSafeHost
                    })
            );

            Console.WriteLine("Done.");
        }
    }

    static class UrlExt {
        public static Boolean IsLocalUrl(this String url, Uri hostUri) {
            if (String.IsNullOrEmpty(url))
                return false;

            if (String.IsNullOrWhiteSpace(url))
                return false;

            if (url[0] == '/') {
                if (url.Length == 1) // "/" valid
                    return true;

                if (url.Length > 1 && (url[1] == '/' || url[1] == '\\')) // "//" or "/\" invalid
                    return false;

                return true;
            }

            if (url[0] == '~') {
                if (url.Length == 1) // "~" invalid
                    return false;

                if (url.Length == 2 && url[1] == '/') // "~/" valid
                    return true;

                if (url.Length > 2 && (url[2] == '/' || url[2] == '\\')) // "~//" or "~/\" invalid
                    return false;

                return true;
            }

            Boolean httpUrl = url.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase);
            Boolean httpsUrl = url.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase);
            if (httpUrl || httpsUrl) {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    return false;

                var absolute = new Uri(url);
                if (absolute.Host.Equals(hostUri.Host, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
