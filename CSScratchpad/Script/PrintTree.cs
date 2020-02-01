using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintTree : Common, IRunnable {
        public void Run() {
            Console.WriteLine("========================= Tree Experimentation: Start =========================");
            Console.WriteLine();

            IList<TreeItem> treeItems = Ref.InitTreeItems();

            Dbg("Tree Items", treeItems);
            Dbg("GetByPath", new {
                Data3Json = GetByPath(treeItems, "ProtectedInnerData/AdminLevel1/data-3.json"),
                Data4Json = GetByPath(treeItems, "ProtectedInnerData/AdminLevel1/data-4.json"),
                Data1Json = GetByPath(treeItems, "InnerData/data-1.json"),
                Data2Json = GetByPath(treeItems, "InnerData/data-2.json"),
                Data7Json = GetByPath(treeItems, "InnerData/VeryInnerData/data-7.json"),
                DyanaMasterZip = GetByPath(treeItems, "Dyana-master.zip"),
                File1Txt = GetByPath(treeItems, "file-1.txt"),
                File15Txt = GetByPath(treeItems, "file-15.txt")
            });
            Console.WriteLine();

            Dbg("Should be:", "");
            Console.WriteLine(Ref.GetListItemsString());
            Console.WriteLine();

            Dbg("Actual:", "");
            Console.WriteLine(Ref.ListToString(ToListItems(treeItems)));
            Console.WriteLine();

            IList<ListItem> listItems = Ref.InitListItems();
            IList<TreeItem> convertedTreeItems = ToTreeItems(listItems);

            Dbg("List Items", listItems);
            Dbg("Converted Tree Items from List Items", convertedTreeItems);
            Console.WriteLine();

            Dbg("Should be:", "");
            Console.WriteLine(Ref.GetListItemsString2());
            Console.WriteLine();

            Dbg("Actual:", "");
            Console.WriteLine(GetMap(treeItems));
            Console.WriteLine();


            Console.WriteLine("======================== Tree Experimentation: Finish =========================");
        }

        #region :: Logic ::

        static TreeItem GetByPath(IList<TreeItem> treeItems, String path) {
            String[] splitted = path.Split('/');
            foreach (String p in splitted)
            {
                TreeItem tree = treeItems.FirstOrDefault(t => t.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase));
                if (tree == null)
                    return null;

                String newP = String.Join("/", splitted.Where(s => s != p));
                if (String.IsNullOrEmpty(newP))
                    return tree;

                if (tree.Children != null || tree.Children.Count != 0)
                    return GetByPath(tree.Children, newP);
            }
            
            return null;
        }

        static IList<ListItem> ToListItems(IList<TreeItem> treeItems) {
            Func<IList<TreeItem>, Int32?, IList<Parent>> children = null;
            children = (items, pId) =>
                items
                    .Select(item => new Parent {ParentId = pId, Node = item})
                    .Concat(items
                        .SelectMany(item => children(item.Children ?? new List<TreeItem>(), item.Id)))
                    .ToList();

            return children(treeItems, null)
                .Select(item => new ListItem {Id = item.Node.Id, Name = item.Node.Name, ParentId = item.ParentId})
                .OrderBy(item => item.Id.ToString())
                .ToList();
        }

        static String GetMap(IList<TreeItem> treeItems) {
            Func<IList<TreeItem>, Int32, IList<Depthness>> children = null;
            children = (items, depth) =>
                items
                    .Select(item => new Depthness {Depth = depth, Node = item})
                    .Concat(items
                        .SelectMany(item => children(item.Children ?? new List<TreeItem>(), depth+1)))
                    .ToList();

            return String.Join("\r\n", children(treeItems, 0)
                .OrderBy(item => item.Node.Id.ToString())
                .Select(item =>
                    (item.Depth > 0 ?
                        String.Join(String.Empty, Enumerable.Repeat("--", item.Depth)) :
                            String.Empty) + "Id: " + item.Node.Id.ToString() + ", Name: " + item.Node.Name));
        }

        static IList<TreeItem> ToTreeItems(IList<ListItem> listItems) {
            Func<IList<ListItem>, Int32?, IList<TreeItem>> recur = null;
            recur = (items, pId) =>
                items
                    .Where(item => item.ParentId == pId)
                    .Select(item => new TreeItem {Id = item.Id, Name = item.Name, Children = recur(items, item.Id)})
                    .ToList();

            return recur(listItems, null);
        }

        class Parent {
            public Int32? ParentId;
            public TreeItem Node;
        }

        class Depthness {
            public Int32 Depth;
            public TreeItem Node;
        }

        #endregion

        #region :: POCO ::

        class TreeItem {
            public IList<TreeItem> Children;
            public Int32 Id;
            public String Name;
        }

        class ListItem {
            public Int32 Id;
            public String Name;
            public Int32? ParentId;

            public override String ToString() => $"Id: {Id}, Name: {Name}, ParentId: {(ParentId == null ? "null" : ParentId.ToString())}";
        }

        #endregion

        #region :: Validator ::

        static class Ref {
            public static IList<TreeItem> InitTreeItems() =>
                new List<TreeItem> {
                    new TreeItem {
                        Id = 1,
                        Name = "ProtectedInnerData",
                        Children = new List<TreeItem> {
                            new TreeItem {
                                Id = 11,
                                Name = "AdminLevel1",
                                Children = new List<TreeItem> {
                                    new TreeItem {Id = 111, Name = "data-3.json", Children = null}
                                }
                            },
                            new TreeItem {
                                Id = 12,
                                Name = "AdminLevel2",
                                Children = new List<TreeItem> {
                                    new TreeItem {Id = 121, Name = "data-4.json", Children = null},
                                    new TreeItem {Id = 122, Name = "data-5.json", Children = null}
                                }
                            }
                        }
                    },
                    new TreeItem {
                        Id = 2,
                        Name = "InnerData",
                        Children = new List<TreeItem> {
                            new TreeItem {Id = 21, Name = "data-1.json", Children = null},
                            new TreeItem {Id = 22, Name = "data-2.json", Children = null}
                        }
                    },
                    new TreeItem {Id = 3, Name = "Dyana-master.zip", Children = null},
                    new TreeItem {Id = 4, Name = "file-1.txt", Children = null},
                    new TreeItem {Id = 5, Name = "file-2.txt", Children = null},
                    new TreeItem {Id = 6, Name = "file-3.txt", Children = null},
                    new TreeItem {Id = 7, Name = "file-4.txt", Children = null},
                    new TreeItem {Id = 8, Name = "sample-config.json", Children = null},
                    new TreeItem {Id = 9, Name = "sample-data.json", Children = null},
                    new TreeItem {Id = 10, Name = "sample-list.txt", Children = null}
                };

            public static List<ListItem> InitListItems() => new List<ListItem> {
                new ListItem {Id = 1, Name = "ProtectedInnerData", ParentId = null},
                new ListItem {Id = 11, Name = "AdminLevel1", ParentId = 1},
                new ListItem {Id = 111, Name = "data-3.json", ParentId = 11},
                new ListItem {Id = 12, Name = "AdminLevel2", ParentId = 1},
                new ListItem {Id = 121, Name = "data-4.json", ParentId = 12},
                new ListItem {Id = 122, Name = "data-5.json", ParentId = 12},

                new ListItem {Id = 2, Name = "InnerData", ParentId = null},
                new ListItem {Id = 21, Name = "data-1.json", ParentId = 2},
                new ListItem {Id = 22, Name = "data-2.json", ParentId = 2},

                new ListItem {Id = 3, Name = "Dyana-master.zip", ParentId = null},
                new ListItem {Id = 4, Name = "file-1.txt", ParentId = null},
                new ListItem {Id = 5, Name = "file-2.txt", ParentId = null},
                new ListItem {Id = 6, Name = "file-4.txt", ParentId = null},
                new ListItem {Id = 7, Name = "file-4.txt", ParentId = null},
                new ListItem {Id = 8, Name = "sample-config.json", ParentId = null},
                new ListItem {Id = 9, Name = "sample-data.json", ParentId = null},
                new ListItem {Id = 10, Name = "sample-list.txt", ParentId = null}
            };

            public static String GetListItemsString() =>
                String.Join(
                    Environment.NewLine,
                    InitListItems().Select(item => item.ToString()))
                .Trim();

            public static String GetListItemsString2() =>
                new StringBuilder()
                    .AppendLine("Id: 1, Name: ProtectedInnerData")
                    .AppendLine("--Id: 11, Name: AdminLevel1")
                    .AppendLine("----Id: 111, Name: data-3.json")
                    .AppendLine("--Id: 12, Name: AdminLevel2")
                    .AppendLine("----Id: 121, Name: data-4.json")
                    .AppendLine("----Id: 122, Name: data-5.json")
                    .AppendLine("Id: 2, Name: InnerData")
                    .AppendLine("--Id: 21, Name: data-1.json")
                    .AppendLine("--Id: 22, Name: data-2.json")
                    .AppendLine("Id: 3, Name: Dyana-master.zip")
                    .AppendLine("Id: 4, Name: file-1.txt")
                    .AppendLine("Id: 5, Name: file-2.txt")
                    .AppendLine("Id: 6, Name: file-4.txt")
                    .AppendLine("Id: 7, Name: file-4.txt")
                    .AppendLine("Id: 8, Name: sample-config.json")
                    .AppendLine("Id: 9, Name: sample-data.json")
                    .AppendLine("Id: 10, Name: sample-list.txt")
                    .ToString()
                    .Trim();

            public static String ListToString(IList<ListItem> listItems) {
                var sb = new StringBuilder();
                foreach (ListItem item in listItems)
                    sb.AppendLine($"Id: {item.Id}, Name: {item.Name}, ParentId: {(item.ParentId == null ? "null" : item.ParentId.ToString())}");

                return sb
                    .ToString()
                    .Trim();
            }
        }

        #endregion
    }
}
 