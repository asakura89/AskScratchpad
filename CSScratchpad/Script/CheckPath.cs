using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class CheckPath : Common, IRunnable {
        public void Run() {
            String path = @"C:\inetpub\wwwroot\iisstart.htm";

            Dbg("Path", path);
            Dbg("FileInfo", new FileInfo(path));
            Dbg("FileInfo.Name", new FileInfo(path).Name);
            Dbg("FileInfo.Directory", new FileInfo(path).Directory);
            Dbg("FileInfo.DirectoryName", new FileInfo(path).DirectoryName);
            Dbg("DirectoryInfo", new DirectoryInfo(path));
            Dbg("DirectoryInfo.Name", new DirectoryInfo(path).Name);
            Dbg("DirectoryInfo.Root", new DirectoryInfo(path).Root);

            Dbg("Path.GetDirectoryName", Path.GetDirectoryName(path));
            Dbg("Path.GetDirectoryName", Path.GetDirectoryName("C:\\inetpub\\wwwroot"));

            // files = new FileInfo(path)
            var pathInfos = new FileInfo(path)
                .Directory
                .GetFiles()
                .Select(file => (Root: file.DirectoryName, Path: file))
                .GroupBy(dirInfo => dirInfo.Root)
                .Select(dirfInfo => (Root: dirfInfo.First().Root, Files: dirfInfo.Select(igrp => igrp.Path)))
                //.Select(file => file.Path);
                ;

            Dbg("GroupBy root dir", pathInfos);

            Dbg("SubDirs", new DirectoryInfo(pathInfos.First().Root).GetDirectories());

            path = @"C:\Users\User\Downloads";
            RecurseCreateDirectory(new DirectoryInfo(@"D:\"), path);
        }

        void RecurseCreateDirectory(DirectoryInfo dirInfo, String pathToBeCreated) {
            String[] splitted = pathToBeCreated.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            while (splitted.Length >= 1) {
                String currentPath = Path.Combine(dirInfo.FullName, splitted[0]);
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);

                dirInfo = new DirectoryInfo(currentPath);
                splitted = splitted.Skip(1).ToArray();
                String newP = String.Join("\\", splitted);
                if (String.IsNullOrEmpty(newP))
                    return;
            }
        }
    }
}
