using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSScratchpad.Script {
    public class FindFiles : Common, IRunnable {
        public void Run() {
            const String DirPath = @"D:\Data\Music";
            const String SearchPattern = "*.*";

            IEnumerable<String> files = Directory
                .GetFiles(DirPath, SearchPattern, SearchOption.AllDirectories)
                .Where(file => file.EndsWith(".mp3") || file.EndsWith(".flac") || file.EndsWith(".wav") || file.EndsWith(".alac") || file.EndsWith(".ape"));

            foreach (String fileName in files)
                Console.WriteLine(fileName);
        }
    }
}
