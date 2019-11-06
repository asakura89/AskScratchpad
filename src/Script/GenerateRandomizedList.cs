using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSScratchpad.Script {
    public class GenerateRandomizedList : Common, IRunnable {
        public void Run() {
            String filepath = GetDataPath("sample-list.txt");
            String outputpath = GetOutputPath("sample-generated.txt");
            Int32 maxItemCount = 5;

            IList<String> contents = File
                .ReadAllLines(filepath)
                .ToList();

            var lookup = (Lookup<String, String>) contents
                .ToLookup(str => str[0].ToString().ToUpperInvariant(), str => str);

            var randomizedList = new List<String>();
            foreach (IGrouping<String, String> lookupGroup in lookup) {
                Int32 lookupContentCounter = 0;
                var lookupContent = lookupGroup.ToList();
                while (lookupContentCounter < lookupContent.Count) {
                    Int32 randIdx = GetRandomNumber(lookupContentCounter +1);
                    String temp = lookupContent[randIdx];
                    lookupContent[randIdx] = lookupContent[lookupContentCounter];
                    lookupContent[lookupContentCounter] = temp;
                    lookupContentCounter++;
                }

                randomizedList.AddRange(lookupContent.Take(maxItemCount));
            }

            File.WriteAllLines(outputpath, randomizedList);
        }

        Int32 GetRandomNumber(Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % 50001;
            var rnd = new Random(seed);
            return rnd.Next(0, upperBound);
        }
    }
}
