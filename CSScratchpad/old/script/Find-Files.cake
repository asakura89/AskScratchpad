#l "Common.cake"

using System;
using System.Linq;
using BCLDir = System.IO.Directory;

void Script() {
    const String DirPath = @"D:\Data\Music";
    const String SearchPattern = "*.*";

    IEnumerable<String> files = BCLDir
        .GetFiles(DirPath, SearchPattern, SearchOption.AllDirectories)
        .Where(file => file.EndsWith(".mp3") || file.EndsWith(".flac") || file.EndsWith(".wav") || file.EndsWith(".alac") || file.EndsWith(".ape"));

    foreach (String fileName in files)
        Console.WriteLine(fileName);
}
