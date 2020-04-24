using System;
using Scratch;

namespace CSScratchpad.Script {
    public class AnalyzeHex : Common, IRunnable {
        public void Run() {
            Dbg(Convert.ToString(0x1fc4, 10));
            Dbg(Convert.ToString(8132, 0x10));

            Dbg(Convert.ToString(07, 0x10));
            Dbg(Convert.ToString(220, 0x10));
            Dbg(Convert.ToString(0x07dc, 10));

            Dbg(Convert.ToString(0x10, 10));
            Dbg(Convert.ToString(0x80, 10));

            Dbg(Convert.ToString(0xff, 10));

            Dbg(Convert.ToString(8080, 16));
            Dbg(Convert.ToString(8090, 16));

            Dbg(16.ToString("x2"));
            Dbg(12.ToString("x2"));
            Dbg(31.ToString("x2"));

            Dbg(0.ToString("x2"));
            Dbg(1.ToString("x2"));

            Dbg(Convert.ToBoolean(0x01));
            Dbg(Convert.ToBoolean(0x00));

            Dbg(2050.ToString("x2"));
            Dbg(Convert.ToString(0x0802, 10));
            Dbg(Convert.ToString(0x802, 10));

            Dbg(0130.ToString("x2"));
        }
    }
}
