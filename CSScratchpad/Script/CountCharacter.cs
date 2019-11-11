using System;
using System.IO;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    class CountCharacter : Common, IRunnable {
        public void Run() {
            String Source = GetDataPath("file-1.txt");

            String text;
            using (var reader = new StreamReader(Source))
                text = reader.ReadToEnd();

            Console.WriteLine(text.Count(Char.IsLetter));
        }
    }
}
