using System;
using Scratch;

namespace CSScratchpad.Script {
    class TestTryParse : Common, IRunnable {
        public void Run() {
            Boolean valid = Int32.TryParse("23", out Int32 intResult);
            Dbg(new { Valid = valid, Result = intResult });

            valid = Int32.TryParse("ax", out intResult);
            Dbg(new { Valid = valid, Result = intResult });

            valid = Int32.TryParse("54.6", out intResult);
            Dbg(new { Valid = valid, Result = intResult });

            /** ──────────────────────────────────────────────────────────────────────────────── */

            valid = Boolean.TryParse("true", out Boolean boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("True", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("trUE", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("1", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("false", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("False", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("FaLsE", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });

            valid = Boolean.TryParse("0", out boolResult);
            Dbg(new { Valid = valid, Result = boolResult });
        }
    }
}
