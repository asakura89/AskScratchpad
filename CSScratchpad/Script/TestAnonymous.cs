using System;
using Scratch;

namespace CSScratchpad.Script {
    class TestAnonymous : Common, IRunnable {
        public void Run() {
            MethodWithAnonParam(new {Id = "99r7", Name = "heyyhoo"});
            MethodWithAnonParam(new StrongTypeParam("99r7", "heyyhoo"));
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
