using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scratch;

namespace CSScratchpad.Script {
    class FormatJson3 : Common, IRunnable {
        public void Run() => Console.WriteLine(JToken.Parse("{\"blah\":\"v\", \"blah2\":\"v2\"}").ToString(Formatting.Indented));
        
    }
}
