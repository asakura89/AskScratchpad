using System;
using System.Collections.Generic;
using System.Dynamic;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintDynamic : Common, IRunnable {
        public void Run() {
            dynamic data = new ExpandoObject();
            data.Name = "Data";
            data.Length = 10;

            Dbg("Dynamic 1", data);

            var data2 = data as IDictionary<String, Object>;
            data2["Enabled"] = true;
            data2["Id"] = Guid.NewGuid().ToString("N");

            Dbg("Dynamic 2", data);
        }
    }
}
