using System;
using System.Collections.Generic;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class TestAllAboutArray : Common, IRunnable {
        public void Run() {

            var root = new Item("Root") {
                Children = new[] {
                    new Item("Roof 1") {
                        Children = new[] {
                            new Item("Level 1 A"),
                            new Item("Level 1 B") {
                                Children = new[] {
                                    new Item("Level 2 X") 
                                }
                            },
                            new Item("Level 1 C")
                        }
                    },
                    new Item("Roof 2") {
                        Children = new[] {
                            new Item("Stair 1 A") {
                                Children = new[] {
                                    new Item("Stair 2 X")
                                }
                            }
                        }
                    },
                    new Item("Roof 3") {
                        Children = new[] {
                            new Item("Lift 1 A"),
                            new Item("Lift 1 B")
                        }
                    }
                }
            };

            IList<Item> items = GetAllItemsBFS(root);
            Dbg("GetAllItems Bread First result", items.Select(item => item.Name));

            items = GetAllItemsDFS(root);
            Dbg("GetAllItems Dept First result", items.Select(item => item.Name));


            /** ──────────────────────────────────────────────────────────────────────────────── */

        }

        public class Item {
            public Item(String name) {
                Name = name;
            }

            public String Name { get; }

            public Boolean HasChildren => Children != null && Children.Any();

            public IEnumerable<Item> Children { get; set; }
        }

        public IList<Item> GetAllItemsBFS(Item root) {
            var items = new List<Item>();
            var children = new List<Item> {root};
            while (children.Any()) {
                // Shift
                Item current = children.First();
                children.RemoveAt(0);

                if (current.HasChildren) {
                    IList<Item>currentChildren = current.Children.ToList();
                    children.AddRange(currentChildren);
                    items.AddRange(currentChildren);
                }
            }

            return items;
        }

        public IList<Item> GetAllItemsDFS(Item root) {
            var items = new List<Item>();
            var children = new List<Item> { root };
            while (children.Any()) {
                // Pop
                Item current = children.Last();
                children.RemoveAt(children.Count -1);

                if (current.HasChildren) {
                    IList<Item> currentChildren = current.Children.ToList();
                    children.AddRange(currentChildren);
                    items.AddRange(currentChildren);
                }
            }

            return items;
        }
    }
}
