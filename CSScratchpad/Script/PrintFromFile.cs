using System;
using System.IO;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintFromFile : Common, IRunnable {
        public void Run() {
            Console.OutputEncoding = Encoding.UTF8;

            String filepath = GetDataPath("file-2.txt");
            Int32 counter = 0;
            using (StreamReader reader = File.OpenText(filepath)) {
                while (!reader.EndOfStream && counter < 30) {
                    String line = reader.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        continue;

                    Console.WriteLine(line.ToLower().Trim());
                    counter++;
                }
            }
        }
    }
}
