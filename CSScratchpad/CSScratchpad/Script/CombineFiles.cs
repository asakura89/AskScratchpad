using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class CombineFiles : Common, IRunnable {
        public void Run() {
            const String DirPath = @"D:\Data\Downloads\20210415_CD_logs\20210415_CD1\20210415_CD1\appdata_logs";
            const String SearchPattern = @"log.*.*.*";
            //String Target = Path.Combine(DirPath, "combined.txt");

            String[] Source = Directory.GetFiles(DirPath, SearchPattern, SearchOption.AllDirectories);
            IList<IGrouping<String, String>> Groupped = Source
                .GroupBy(file => file
                    .Replace(DirPath, String.Empty)
                    .Trim('\\')
                    .Substring(0, 19))
                .ToList();
            Boolean PrintFileDelimiter = false;

            Console.WriteLine("Start.");

            const Int32 MaxCharacterColumn = 80;
            foreach (var grp in Groupped) {
                String Target = Path.Combine(DirPath, grp.Key + ".txt");
                using (var sw = new StreamWriter(new FileStream(Target, FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8)) {
                    //for (Int32 idx = 0; idx < Source.Length; idx++) {
                    foreach(String filepath in grp) {
                        //String filepath = Source[idx];
                        //Console.WriteLine(filepath);
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
            }

            Console.WriteLine("Done.");
        }
    }
}
