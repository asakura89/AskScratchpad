
#tool nuget:?package=Newtonsoft.Json&version=10.0.3
#r tools/newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

using System;
using Newtonsoft.Json;

public class Dummy { }

public class Program {
    static Dummy st;
    public Dummy Singleton {
        get {
            return st ?? (st = new Dummy());
        }
    }

    public Dummy Transient {
        get {
            return new Dummy();
        }
    }
}

void Script() {
    try {
        var p = new Program();

        var a = p.Singleton;
        var b = p.Transient;
        var c = p.Singleton;
        var d = p.Transient;
        var e = p.Singleton;

        Console.WriteLine($"a == b >> {a == b}");
        Console.WriteLine($"a == c >> {a == c}");
        Console.WriteLine($"a == d >> {a == d}");
        Console.WriteLine($"a == e >> {a == e}");

        Console.WriteLine();
        Console.WriteLine($"b == c >> {b == c}");
        Console.WriteLine($"b == d >> {b == d}");
        Console.WriteLine($"b == e >> {b == e}");
    }
    catch(Exception ex) {
        Dbg(ex);
    }
}

#region : Main :

String GetDataPath(String configfilename) =>
    Context
        .Environment
        .WorkingDirectory
        .Combine("data")
        .Combine(configfilename)
        .FullPath;

void Dbg(Object obj) => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
void DbgCake(Object obj) => Information(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));

Task("Main")
    .Does(() => {
        //Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        Script();
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.Message));
RunTarget("Main");

#endregion