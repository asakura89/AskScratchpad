using System;
using System.IO;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class CombineFiles : Common, IRunnable {
        public void Run() {
            String Target = GetOutputPath("combined.txt");
            String[] Source = new[] {
                GetDataPath("file-3.txt"),
                GetDataPath("file-4.txt"),
                GetDataPath("file-2.txt"),
                GetDataPath("file-1.txt")
            };
            Boolean PrintFileDelimiter = true;

            Console.WriteLine("Start.");

            const Int32 MaxCharacterColumn = 80;
            using (var sw = new StreamWriter(new FileStream(Target, FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8)) {
                for (Int32 idx = 0; idx < Source.Length; idx++) {
                    String filepath = Source[idx];
                    using (var sr = new StreamReader(filepath, Encoding.UTF8)) {
                        if (PrintFileDelimiter)
                            sw.WriteLine(
                                new StringBuilder()
                                    .AppendLine()
                                    .AppendLine(
                                        String.Join(
                                            String.Empty,
                                            Enumerable.Repeat("=-", MaxCharacterColumn / 2)
                                        )
                                    )
                                    .AppendLine(filepath)
                                    .AppendLine(
                                        String.Join(
                                            String.Empty,
                                            Enumerable.Repeat("-=", MaxCharacterColumn / 2)
                                        )
                                    )
                                    .AppendLine()
                            );

                        sw.WriteLine(sr.ReadToEnd());
                    }
                }
            }

            Console.WriteLine("Done.");
        }
    }
}
