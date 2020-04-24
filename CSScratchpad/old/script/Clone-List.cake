#l "Common.cake"

using System;
using System.Collections;
using System.Collections.Generic;

void Script() {
    try {
        IList<String> all = new[] {
            "08803012",
            "00111342",
            "09506564",
            "09003194",
            "09205132",
            "09709662",
            "09003122",
            "09709503",
            "09507544",
            "00010563"
        }.ToList();

        Console.WriteLine("All:");
        Dbg(all);

        IList<String> clone = all.Where(item => true).ToList();
        all.Clear();

        Console.WriteLine("All Cleared:");
        Dbg(all);

        Console.WriteLine("Clone:");
        Dbg(clone);

        all = new[] {
            "08803012",
            "00111342",
            "09506564",
            "09003194",
            "09205132",
            "09709662",
            "09003122",
            "09709503",
            "09507544",
            "00010563"
        }.ToList();

        IList<String> filter = all.Where(item => item.Substring(0, 2) == "00").ToList();
        all.Clear();

        Console.WriteLine("All Cleared:");
        Dbg(all);

        Console.WriteLine("Filter:");
        Dbg(filter);
    }
    catch (Exception ex) {
        Dbg(ex);
    }
}
