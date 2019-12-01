using System;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintCharCode : Common, IRunnable {
        public void Run() {
            Console.OutputEncoding = System.Text.Encoding.Unicode; // None encoding works

            const String String1 = "⠋ ⠙ ⠹ ⠸ ⠼ ⠴ ⠦ ⠧ ⠇ ⠏";
            const String String2 = "⣾ ⣽ ⣻ ⢿ ⡿ ⣟ ⣯ ⣷";

            Dbg("String1", String1);
            Dbg("String1.Analyzed",
                String1
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ch => new {
                        NumbericValue = Char.GetNumericValue(Convert.ToChar(ch)),
                        UnicodeCategory = Char.GetUnicodeCategory(Convert.ToChar(ch))
                    })
            );

            Dbg("String2", String2);
            Dbg("String2.Analyzed",
                String2
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ch => new {
                        NumbericValue = Char.GetNumericValue(Convert.ToChar(ch)),
                        UnicodeCategory = Char.GetUnicodeCategory(Convert.ToChar(ch))
                    })
            );
        }
    }
}
