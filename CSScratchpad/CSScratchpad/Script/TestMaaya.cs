using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
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
                "http:web.development.net",
                "http:/web.development.net",
                "https:web.development.net",
                "https:/web.development.net",
                "http://web.development.net/Home/Index",
                "https://web.development.net/Home/Index",
                "https://google.com",
                "http://web.development.net/Home/Index?q=hello",
                "http://web.development.net/Home-Index",
                "http://web.development.net/Home/Index?q=<script>alert('yeehaw');</script>",
                "http://web.development.net/Home/Index<en-us>/[page].htm?v={value1}#x=[amount]",
                "http://web.development.net/Home/Index<en-us>/[page].htm?v={value1}#x=[amount]&q=<script>alert('yeehaw');</script>", // NOTE: fragment issue
                "http://web.development.net/Home/Index<en-us>/[page].htm?q=<script>alert('yeehaw');</script>&x=a+b&v={value1}#x=[amount]"
            };

            Dbg("IsLocalUrl:",
                urls.Select(url => new {
                    Url = url,
                    Local = url.IsLocalUrl(new Uri("http://web.development.net"))
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
                catch (ArgumentNullException) {
                    return false;
                }
                catch (UriFormatException) {
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
                    .Skip(urls.Length -9)
                    .Select(url => new {
                        Url = url,
                        new Uri(url).Host,
                        new Uri(url).DnsSafeHost
                    })
            );

            IEnumerable<String> toBeCleaneds = urls
                .Take(3)
                .Concat(urls
                    .Skip(urls.Length -9));

            Dbg("CleanedLink:",
                toBeCleaneds
                    .Select(url => new {
                        UriBuilder = new UriBuilder(url),
                        HtmlEnc = AntiXssEncoder.HtmlEncode(url, true),
                        UrlEnc = AntiXssEncoder.UrlEncode(url),
                        CustomEnc = url.AsCleanedLink()
                    })
                    .Select(url => new {
                        Uri = new {
                            url.UriBuilder.Uri.Host,
                            url.UriBuilder.Uri.AbsolutePath,
                            url.UriBuilder.Uri.AbsoluteUri,
                            url.UriBuilder.Uri.Authority,
                            url.UriBuilder.Uri.DnsSafeHost,
                            url.UriBuilder.Uri.Port,
                            url.UriBuilder.Uri.Fragment,
                            url.UriBuilder.Uri.HostNameType,
                            url.UriBuilder.Uri.IdnHost,
                            url.UriBuilder.Uri.IsAbsoluteUri,
                            url.UriBuilder.Uri.IsDefaultPort,
                            url.UriBuilder.Uri.IsFile,
                            url.UriBuilder.Uri.IsLoopback,
                            url.UriBuilder.Uri.IsUnc,
                            url.UriBuilder.Uri.LocalPath,
                            url.UriBuilder.Uri.OriginalString,
                            url.UriBuilder.Uri.PathAndQuery,
                            url.UriBuilder.Uri.Query,
                            QueryStrings = HttpUtility.ParseQueryString(url.UriBuilder.Uri.Query),
                            url.UriBuilder.Uri.Scheme,
                            url.UriBuilder.Uri.Segments,
                            url.UriBuilder.Uri.UserEscaped,
                            url.UriBuilder.Uri.UserInfo
                        },
                        url.UriBuilder,
                        url.HtmlEnc,
                        url.UrlEnc,
                        url.CustomEnc
                    })
            );

            Dbg("CleanedLink:",
                toBeCleaneds
                    .Select(url => url.AsCleanedLink())
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

        // NOTE: Return value determine if need to processed further or not
        static Boolean ParseObject(Object source, out String result) {
            String cleaned;
            if (source == null) {
                result = null;
                return false;
            }

            cleaned = source as String;
            if (cleaned == null) {
                result = null;
                return false;
            }

            cleaned = cleaned.Trim();
            if (String.IsNullOrEmpty(cleaned)) {
                result = cleaned;
                return false;
            }

            result = cleaned;
            return true;
        }

        public static String AsCleanedString(this Object source) {
            Boolean okForFurtherProcessing = ParseObject(source, out String cleaned);
            return !okForFurtherProcessing ? cleaned : AntiXssEncoder.HtmlEncode(cleaned, true);
        }

        public static String AsCleanedLink(this Object source) {
            Boolean okForFurtherProcessing = ParseObject(source, out String cleaned);
            if (!okForFurtherProcessing)
                return cleaned;

            var uriBuilder = new UriBuilder(cleaned);
            IList<String> cleanedSegments = new List<String>();
            foreach (String segment in uriBuilder.Uri.Segments) {
                String cleanedSegment = segment.Replace("/", String.Empty);
                if (cleanedSegment == String.Empty)
                    continue;

                cleanedSegments.Add(AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(cleanedSegment)));
                // NOTE: we're back and forth because we can't turn off the UriBuilder's default url encoder
                // and the UriBuilder's default url encoder did not escape singiequote and normal brackets
            }
            uriBuilder.Path = String.Join("/", cleanedSegments);

            NameValueCollection queryStrings = HttpUtility.ParseQueryString(uriBuilder.Query);
            NameValueCollection shadowQueryStrings = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (String key in shadowQueryStrings)
                queryStrings[key] = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(shadowQueryStrings[key]));
            uriBuilder.Query = HttpUtility.UrlDecode(queryStrings.ToString());
            uriBuilder.Fragment = AntiXssEncoder.UrlEncode(HttpUtility.UrlDecode(uriBuilder.Fragment).Replace("#", String.Empty));

            // NOTE: hack to remove port
            if (uriBuilder.Uri.IsDefaultPort)
                uriBuilder.Port = -1;

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
