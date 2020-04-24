
using System;
using System.IO;
using Ionic.Zip;
using Scratch;

namespace CSScratchpad.Script {
    class ZipStructureViewer : Common, IRunnable {
        public void Run() {
            String zipPath = GetDataPath("Dyana-master.zip");

            var stream = (Stream) File.Open(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var zip = ZipFile.Read(stream)) {
                foreach (ZipEntry entry in zip) {
                    if (Path.GetExtension(entry.FileName).Equals(".zip", StringComparison.InvariantCultureIgnoreCase)) {
                        String izipName = entry.FileName + "/";
                        Console.WriteLine(izipName);

                        var contentStream = new MemoryStream();
                        entry.Extract(contentStream);
                        contentStream.Position = 0;
                        using (var izip = ZipFile.Read(contentStream))
                            foreach (ZipEntry izipEntry in izip)
                                Console.WriteLine(izipName + izipEntry.FileName);
                    }
                    else
                        Console.WriteLine(entry.FileName);
                }
            }
        }
    }
}
