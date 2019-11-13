using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintPatchedJson : Common, IRunnable {
        public void Run() {
            var sampleData = JToken.Parse(File.ReadAllText(GetDataPath("sample-data.json")));
            Dbg("Root children count", sampleData.Children().Count());
            Dbg("Root child with key: config.var", sampleData["config.var"]);
            Dbg("Root child with key: myvar", sampleData["myvar"]);

            IDictionary<String, String> vars = sampleData["config.var"]
                .ToDictionary(
                    token => token["name"].Value<String>(),
                    token => token["value"].Value<String>());

            Dbg("Dictionary", vars);

            String patchedJson = File.ReadAllText(GetDataPath("sample-config.json")).ReplaceWith(vars);
            var sampleConfig = JToken.Parse(patchedJson);
            Dbg(sampleConfig["WebApp"]);
        }       
    }
}
