using System;
using System.Linq;
using System.Text.RegularExpressions;
using Scratch;

namespace CSScratchpad.Script {
    internal class CompareString : Common, IRunnable {
        public void Run() {
            Dbg("Start", "-");
            Dbg("Compare",
                new[] {
                    "https://app-stg.localweb.net",
                    "https://beta-app.devsvr.net",
                    "https://beta-app-admin.devsvr.net",
                    "https://web.shadowmonarch.io",
                    "https://web-admin.shadowmonarch.io",
                    "https://web-dev.shadowmonarch.io",
                    "https://admin-dev.shadowmonarch.io",
                    "https://api-stg.localweb.net",
                    "https://web.api.io",
                    "https://api.devsvr.net",
                    "https://beta-api.devsvr.net",
                    "https://hub.devsvr.net",
                    "https://hub-admin.devsvr.net",
                    "https://beta-hub.devsvr.net"
                }
                    .Select(host => new {
                        Host = host,
                        Match = Regex.Match(host, ".+(shadow|monarch|devsvr).+")
                    })
                    .Select(match => new {
                        match.Host,
                        match.Match.Success,
                        match.Match.Value
                    })
                    .ToList()
            );
            Dbg("Done", "-");
        }
    }
}
