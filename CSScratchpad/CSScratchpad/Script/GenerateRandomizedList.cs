using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    static class InternalHelper {
        internal const Int32 Feigenbaum = 46692;

        internal static Int32 GetRandomNumber(Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % Feigenbaum;
            return new Random(seed)
                .Next(lowerBound, upperBound);
        }

        internal static Int32 GetRandomNumber(Int32 upperBound) => GetRandomNumber(0, upperBound);
    }

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
                    Int32 randIdx = InternalHelper.GetRandomNumber(lookupContentCounter +1);
                    String temp = lookupContent[randIdx];
                    lookupContent[randIdx] = lookupContent[lookupContentCounter];
                    lookupContent[lookupContentCounter] = temp;
                    lookupContentCounter++;
                }

                randomizedList.AddRange(lookupContent.Take(maxItemCount));
            }

            File.WriteAllLines(outputpath, randomizedList);
        }
    }
}
