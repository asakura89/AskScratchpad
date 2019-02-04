#l "ask-common.cake";

using System;
using System.IO;
using System.Text;

readonly String Target = GetDataPath("combined.txt");
readonly String[] Source = new[] {
    GetDataPath("file-1.txt"),
    GetDataPath("file-2.txt"),
    GetDataPath("file-3.txt"),
    GetDataPath("file-4.txt")
};

void Script() {
    String text = String.Empty;
    using (var sw = new StreamWriter(new FileStream(Target, FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8)) {
        for (Int32 idx = 0; idx < Source.Length-1; idx++) {
            using (var sr = new StreamReader(Source[idx], Encoding.UTF8)) {
                text = sr.ReadToEnd();
                sw.WriteLine(text);
            }
        }
    }
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
