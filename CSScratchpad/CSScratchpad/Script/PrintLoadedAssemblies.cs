using System;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintLoadedAssemblies : Common, IRunnable {
        public void Run() =>
            Dbg(
                AppDomain.CurrentDomain.GetAssemblies()
                    .Select(asm => asm.FullName)
                    .OrderBy(name => name)
            );
    }
}
