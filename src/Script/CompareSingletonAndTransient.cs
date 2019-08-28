using System;
using System.Text;

namespace CSScratchpad.Script {
    class CompareSingletonAndTransient : Common, IRunnable {
        public void Run() {
            try {
                var caller = new SingletonTransientCaller();

                var a = caller.Singleton;
                var b = caller.Transient;
                var c = caller.Singleton;
                var d = caller.Transient;
                var e = caller.Singleton;

                Console.WriteLine(
                    new StringBuilder()
                        .AppendLine("A Singleton")
                        .AppendLine("B Transient")
                        .AppendLine("C Singleton")
                        .AppendLine("D Transient")
                        .ToString()
                );

                Console.WriteLine($"a == b >> {a == b}");
                Console.WriteLine($"a == c >> {a == c}");
                Console.WriteLine($"a == d >> {a == d}");
                Console.WriteLine($"a == e >> {a == e}");

                Console.WriteLine();
                Console.WriteLine($"b == c >> {b == c}");
                Console.WriteLine($"b == d >> {b == d}");
                Console.WriteLine($"b == e >> {b == e}");
            }
            catch (Exception ex) {
                Dbg(ex);
            }
        }
    }

    public class SingletonTransient { }

    public class SingletonTransientCaller {
        static SingletonTransient st;
        public SingletonTransient Singleton => st ?? (st = new SingletonTransient());
        public SingletonTransient Transient => new SingletonTransient();
    }
}
