using System;
using Scratch;

namespace CSScratchpad.Script {
    public class ManipulateString : Common, IRunnable {
        public void Run() {
            var test = new StringMagicianTest();
            test.NumberToAlphabetTest();
            test.AlphabetToNumberTest();
        }

        public class StringMagicianTest {
            public void NumberToAlphabetTest() {
                Console.WriteLine(Magician.NumberToAlphabetByOrder(1) == "A" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(5) == "E" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(7) == "G" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(27) == "AA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(28) == "AB" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(51) == "AY" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(52) == "AZ" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(53) == "BA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(54) == "BB" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(341) == "MC" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(702) == "ZZ" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(703) == "AAA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(704) == "AAB" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(728) == "AAZ" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(983) == "AKU" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(1378) == "AZZ" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(1380) == "BAB" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(2369) == "CMC" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(18279) == "AAAA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(17603) == "ZAA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(457003) == "YZAA" ? "true" : "false");
                Console.WriteLine(Magician.NumberToAlphabetByOrder(457679) == "ZAAA" ? "true" : "false");
            }

            public void AlphabetToNumberTest() {
                Console.WriteLine(Magician.AlphabetToNumberByOrder("A") == 1 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("E") == 5 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("G") == 7 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AA") == 27 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AB") == 28 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AY") == 51 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AZ") == 52 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("BA") == 53 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("BB") == 54 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("MC") == 341 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("ZZ") == 702 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AAA") == 703 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AAB") == 704 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AAZ") == 728 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AKU") == 983 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AZZ") == 1378 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("BAB") == 1380 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("CMC") == 2369 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("AAAA") == 18279 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("ZAA") == 17603 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("YZAA") == 457003 ? "true" : "false");
                Console.WriteLine(Magician.AlphabetToNumberByOrder("ZAAA") == 457679 ? "true" : "false");
            }
        }

        public static class Magician {
            static readonly String[] splittedAlphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
            static readonly Int32 alphabetCount = splittedAlphabet.Length;

            static String A(Int32 no) {
                if (no < 1)
                    throw new ArgumentOutOfRangeException("no");

                String postfix = String.Empty;
                if (no > alphabetCount) {
                    postfix = A(no % alphabetCount == 0 ? alphabetCount : no % alphabetCount);
                    no = no % alphabetCount == 0 ? (no / alphabetCount) - 1 : no / alphabetCount;
                    if (no > alphabetCount)
                        return A(no) + " " + postfix;
                }

                return postfix == String.Empty ? no.ToString() : no + " " + postfix;
            }

            public static String NumberToAlphabetByOrder(Int32 no) {
                String idxString = A(no);
                String[] splittedIdxString = idxString.Split(' ');
                String finalString = String.Empty;
                foreach (String idx in splittedIdxString) {
                    Int16 idxNo = Convert.ToInt16(idx);
                    finalString += splittedAlphabet[idxNo - 1];
                }

                return finalString;
            }

            static String B(String alphabetString) {
                if (String.IsNullOrEmpty(alphabetString))
                    throw new ArgumentNullException("alphabetString");

                String idxString = String.Empty;
                foreach (Char alpha in alphabetString)
                    for (Int32 idx = 0; idx < alphabetCount; idx++)
                        if (alpha.ToString() == splittedAlphabet[idx])
                            idxString += idx + 1 + " ";

                return idxString.TrimEnd();
            }

            public static Int32 AlphabetToNumberByOrder(String alphabetString) {
                String idxString = B(alphabetString);
                Int32 no = 0;
                String[] splittedIdxString = idxString.Split(' ');
                for (Int32 idx = 0; idx < splittedIdxString.Length; idx++) {
                    Int32 idxNo = Convert.ToInt32(splittedIdxString[idx]);
                    no += idx == splittedIdxString.Length - 1 ? idxNo : idxNo * Convert.ToInt32(Math.Pow(alphabetCount, splittedIdxString.Length - (idx + 1)));
                }

                return no;
            }
        }
    }
}
