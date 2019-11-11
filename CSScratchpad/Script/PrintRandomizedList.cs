using System;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintRandomizedList : Common, IRunnable {
        public void Run() {
            Int32[] ints = Enumerable
                .Range(1, 15)
                .ToArray();

            Dbg("Original", String.Join(", ", ints));

            var rand = new Random();
            Int32 counter = 0;
            while (counter < ints.Length) {
                Int32 maxVal = counter +1;
                Int32 randVal = rand.Next(maxVal);
                Dbg("Random", new {
                    Counter = counter,
                    MaxValue = maxVal,
                    RandomValue = randVal
                });

                Int32 temp = ints[randVal];
                ints[randVal] = ints[counter];
                ints[counter] = temp;

                counter++;
            }

            Dbg("Shuffled", String.Join(", ", ints));
        }
    }
}
