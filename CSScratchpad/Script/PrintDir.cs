using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintDir : Common, IRunnable {
        public void Run() {
            String input = GetDataPath("file-1.txt");
            Dbg("GetPathInfo on File", GetPathInfo(input));

            input = DataDirPath;
            Dbg("GetPathInfo on Directory", GetPathInfo(input));

            Dbg("PrintDirContents", String.Empty);
            PrintDirContents(input);

            Dbg("GetPathInfoNode without recursion", GetPathInfoNode(input));
        }

        #region :: PathInfo ::

        PathInfo GetPathInfo(String path) {
            var info1 = new {
                Path = path,
                IsDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory)
            };

            var info2 = new {
                info1.Path,
                Name = info1.IsDirectory ? new DirectoryInfo(info1.Path).Name : new FileInfo(info1.Path).Name,
                info1.IsDirectory,
                Children = info1.IsDirectory ?
                    new DirectoryInfo(info1.Path)
                        .GetDirectories()
                        .Select(dir => dir.FullName)
                        .Concat(new DirectoryInfo(info1.Path)
                            .GetFiles()
                            .Select(file => file.FullName)) :
                    null
            };

            return new PathInfo {
                Path = info2.Path,
                Name = info2.Name,
                IsDirectory = info2.IsDirectory,
                HasChildren = info2.Children != null && info2.Children.Any(),
                Children = info2.Children
            };
        }

        class PathInfo {
            public String Path { get; set; }
            public String Name { get; set; }
            public Boolean IsDirectory { get; set; }
            public Boolean HasChildren { get; set; }
            public IEnumerable<String> Children { get; set; }
        }

        #endregion

        void PrintDirContents(String path) {
            if (String.IsNullOrEmpty(path))
                return;

            PathInfo current = null;
            var remainings = new Stack<PathInfo>(new[] { GetPathInfo(path) });
            while (remainings.Any()) {
                current = remainings.Pop();

                Console.WriteLine(current.Name);
                Console.WriteLine();

                if (current.HasChildren)
                    foreach (String child in current.Children)
                        remainings.Push(GetPathInfo(child));
            }
        }

        #region :: N-Ary Node ::

        void TraverseNAryTree(NAryNode node, Action<NAryNode> action) {
            if (node == null)
                return;

            NAryNode current = null;
            var remainings = new Stack<NAryNode>(new[] { node });
            while (remainings.Any()) {
                current = remainings.Pop();

                action.Invoke(current);

                if (current.HasChildren)
                    foreach (NAryNode child in current.Children)
                        remainings.Push(child);
            }
        }

        class NAryNode {
            public Object Content { get; set; }
            public Int32 Level { get; set; }
            public Boolean HasChildren { get; set; }
            public IList<NAryNode> Children { get; set; }
        }

        #endregion

        #region :: N-Ary Node T ::

        void TraverseNAryTree<T>(NAryNode<T> node, Action<NAryNode<T>> action) {
            if (node == null)
                return;

            NAryNode<T> current = null;
            var remainings = new Stack<NAryNode<T>>(new[] { node });
            while (remainings.Any()) {
                current = remainings.Pop();

                action.Invoke(current);

                if (current.HasChildren)
                    foreach (NAryNode<T> child in current.Children)
                        remainings.Push(child);
            }
        }

        class NAryNode<T> {
            public T Content { get; set; }
            public Int32 Level { get; set; }
            public Boolean HasChildren { get; set; }
            public IList<NAryNode<T>> Children { get; set; }
        }

        #endregion

        NAryNode ConvertPathToNAryNode(String path) {
            var content = new PathDir { Path = path, IsDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory) };
            var node = new NAryNode { Content = content };

            return node;
        }

        IEnumerable<String> GetChildren(String path) {
            Boolean isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            return isDirectory ?
                new DirectoryInfo(path)
                    .GetDirectories()
                    .Select(dir => dir.FullName)
                    .Concat(new DirectoryInfo(path)
                        .GetFiles()
                        .Select(file => file.FullName)) :
                null;
        }

        NAryNode GetPathInfoNode(String path) {
            if(String.IsNullOrEmpty(path))
                return null;

            NAryNode root = ConvertPathToNAryNode(path);
            var remainings = new Stack<NAryNode>(new[] { root });
            while (remainings.Any()) {
                NAryNode current = remainings.Pop();
                if (current.Children == null) {
                    PathDir content = current.Content as PathDir; ;
                    IEnumerable<String> children = GetChildren(content.Path);
                    current.HasChildren = children != null && children.Any();
                    if (!current.HasChildren)
                        current.Children = null;
                    else {
                        current.Children = new List<NAryNode>();
                        foreach (String child in children) {
                            NAryNode childNode = ConvertPathToNAryNode(child);
                            remainings.Push(childNode);
                            current.Children.Add(childNode);
                        }
                    }
                }
            }

            return root;
        }

        class PathDir {
            public String Path { get; set; }
            public Boolean IsDirectory { get; set; }
        }


        

        

        

        
    }
}
