using System;
using Scratch;

namespace CSScratchpad.Script {
    public class TestPOCOBuilder : Common, IRunnable {
        public void Run() {
            Dbg("Full",
                new Pocho<GetSegmentViewModel>()
                    .With(p => p.App = "ConsoleApp")
                    .With(p => p.Segment = 1)
                    .With(p => p.SegmentLevel = 1)
                    .Create()
            );

            Dbg("App Only",
                new Pocho<GetSegmentViewModel>()
                    .With(p => p.App = "ConsoleApp")
                    .Create()
            );
        }

        public sealed class GetSegmentViewModel {
            public String App { get; set; }
            public Int32 Segment { get; set; }
            public Int32 SegmentLevel { get; set; }
        }

        public class Pocho<T> where T : class, new() {
            private readonly T poco;

            public Pocho() {
                poco = new T();
            }

            public Pocho<T> With(Action<T> action) {
                action(poco);
                return this;
            }

            public T Create() {
                return poco;
            }
        }
    }
}
