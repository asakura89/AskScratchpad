#l "ask-common.cake"

using System;

readonly String Source = GetDataPath("file-1.txt");

void Script() {
    String text;
    using (StreamReader reader = new StreamReader(Source))
        text = reader.ReadToEnd();

    Console.WriteLine(text.Count(char.IsLetter));
}

#region : Main :

Task("Main")
    .Does(() => {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        Script();
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.Message));
RunTarget("Main");

#endregion
