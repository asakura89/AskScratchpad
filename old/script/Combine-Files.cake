#l "Common.cake";

using System;
using System.IO;
using System.Text;

void Script() {
    String Target = GetOutputPath("combined.txt");
    String[] Source = new[] {
        GetDataPath("file-3.txt"),
        GetDataPath("file-4.txt"),
        GetDataPath("file-2.txt"),
        GetDataPath("file-1.txt")
    };

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
