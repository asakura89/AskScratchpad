using System;
using Scratch;

namespace CSScratchpad.Script {
    class TestBitwise : Common, IRunnable {
        public void Run() {
            Int32 ax = 4;
            Int32 bx = 6;
            Int32 cx = 234;

            Int32 hx = bx | ax | cx;

            Console.WriteLine(hx);
        }
    }
}
