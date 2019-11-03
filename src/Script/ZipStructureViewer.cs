
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace CSScratchpad.Script {
    class ZipStructureViewer : Common, IRunnable {
        public void Run() {
            //String zipPath = @"D:\gtt-migration-20191029.0-1.zip";
            //using (ZipArchive archive = ZipFile.OpenRead(zipPath)) {
            //    Dbg(
            //        archive
            //            .Entries
            //            .Select(entry => entry.FullName)
            //    );

            //    Func<List<ZipArchiveEntry>, Int32, List<Level>> children = null;
            //    children = (entries, lv) =>
            //        entries
            //            .Select(entry => new Level { LevelCount = lv, Entry = entry })
            //            .Concat(entries
            //                .SelectMany(entry => children(entry.Open() ?? new List<ZipArchiveEntry>(), lv+1)))
            //            .ToList();

            //    return String.Join("\r\n", children(archive.Entries.ToList(), 0)
            //        .OrderBy(lv => lv.Entry.Id.ToString())
            //        .Select(lv =>
            //            (lv.LevelCount > 0 ?
            //                String.Join(String.Empty, Enumerable.Repeat("--", lv.LevelCount)) :
            //                String.Empty) + "Id: " + lv.Tree.Id.ToString() + ", Name: " + lv.Tree.Name));
            //}
        }

        public class Level {
            public Int32 LevelCount;
            public ZipArchiveEntry Entry;
        }
    }
}
