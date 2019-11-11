using System;
using Scratch;

namespace CSScratchpad.Script {
    public class GenerateId : Common, IRunnable {
        public void Run() {
            var guid = Guid.NewGuid();

            Console.WriteLine("Generated Id: {0}", guid.ToString().Replace("-", String.Empty));
            Console.WriteLine("Generated Id: {0}", guid.ToString("D"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("d"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("N"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("n"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("P"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("p"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("B"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("b"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("X"));
            Console.WriteLine("Generated Id: {0}", guid.ToString("x"));
            Console.WriteLine("Generated Id: {0}", guid.ToString().ToUpperInvariant());
        }
    }
}
