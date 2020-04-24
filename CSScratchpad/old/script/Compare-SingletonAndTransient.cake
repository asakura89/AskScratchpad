#l "Common.cake"

using System;

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
