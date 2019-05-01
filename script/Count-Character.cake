#l "Common.cake"

using System;

void Script() {
    String Source = GetDataPath("file-1.txt");

    String text;
    using (StreamReader reader = new StreamReader(Source))
        text = reader.ReadToEnd();

    Console.WriteLine(text.Count(char.IsLetter));
}
