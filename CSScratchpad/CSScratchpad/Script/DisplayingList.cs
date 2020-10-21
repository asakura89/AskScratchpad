using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class DisplayingList : Common, IRunnable {
        public void Run() {
            IList<String> contents = File
                .ReadAllLines(GetDataPath("sample-list.txt"))
                .ToList();

            Display(contents);

            Console.WriteLine();
            Console.WriteLine();

            Display(contents, new List<Action<Object>> {
                DisplayAction1,
                DisplayAction2
            });
        }

        void DisplayAction1(Object item) => Console.WriteLine(item);

        void DisplayAction2(Object item) => Dbg(item);

        void Display<T>(IEnumerable<T> list) {
            Console.WriteLine();
            foreach (T item in list) {
                var isNull = item == null;
                var isDefault = EqualityComparer<T>.Default.Equals(item, default(T));
                if (isNull || isDefault)
                    Console.WriteLine("null");
                else
                    Console.WriteLine(item);
            }
        }

        void Display<T>(IEnumerable<T> list, IList<Action<Object>> actions) {
            Console.WriteLine();
            foreach (T item in list) {
                var isNull = item == null;
                var isDefault = EqualityComparer<T>.Default.Equals(item, default(T));
                foreach (var action in actions) {
                    if (isNull || isDefault) {
                        action("null");
                        continue;
                    }

                    action(item);
                }
            }
        }
    }
}