#l "ask-common.cake"

using System;
using System.Linq;
using BCLDir = System.IO.Directory;

readonly String DirPath = @"E:\";
const String SearchPattern = "*.*";

void Script() {
    IEnumerable<String> files = BCLDir
        .GetFiles(DirPath, SearchPattern, SearchOption.AllDirectories)
        .Where(file => file.EndsWith(".mp3") || file.EndsWith(".flac") || file.EndsWith(".wav") || file.EndsWith(".alac") || file.EndsWith(".ape"));
    foreach (String fileName in files)
        Console.WriteLine(fileName);
}

#region : Main :

Task("Main")
    .Does(() => {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        Script();
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.Message));
RunTarget("Main");

#endregion
