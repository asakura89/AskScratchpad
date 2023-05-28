using System;
using Scratch;

namespace CSScratchpad.Script {
    class TestAnonymous : Common, IRunnable {
        public void Run() {
            /*
            List<int> numbers = new List<int> { 1, 2, 3, 4 };
            numbers.Append(5);

            string.Join(", ", numbers).Dump();

            string.Join(", ", numbers.Append(5)).Dump();

            List<int> newNumbers = numbers.Append(5).ToList();

            string.Join(", ", newNumbers).Dump();

            //MethodWithAnonParam(new {Id = "99r7", Name = "heyyhoo"});
            //MethodWithAnonParam(new StrongTypeParam("99r7", "heyyhoo"));
            */
        }

        private void MethodWithAnonParam(Object param) {
            Type paramType = param.GetType();
            Dbg(
                paramType.GetProperties()
            );
        }

        private class StrongTypeParam {
            public String Id { get; set; }
            public String Name { get; set; }

            public StrongTypeParam(String id, String name) {
                Id = id;
                Name = name;
            }
        }
    }
}
