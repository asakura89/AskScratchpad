using System;

Task("Main")
    .Does(() => {
        // NOTE: to display unicode arrow ;)
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        Console.WriteLine($"Cache-Control: max-age → {(31_536_000 / 60 / 60 / 24 / 7 / 4 / 12).ToString()} year");
        Console.WriteLine($"→ length is {"→".Length.ToString()}");
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.Message));
RunTarget("Main");
