#l "ask-common.cake"

using System;

void Script() {
    Information("Hello World.");
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
