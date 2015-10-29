using System;

namespace StringMagician
{
    public static class Magician
    {
        private static readonly String[] splittedAlphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
        private static readonly Int32 alphabetCount = splittedAlphabet.Length;

        private static String A(Int32 no)
        {
            if (no < 1)
                throw new ArgumentOutOfRangeException("no");

            String postfix = String.Empty;
            if (no > alphabetCount)
            {
                postfix = A(no % alphabetCount == 0 ? alphabetCount : no % alphabetCount);
                no = no % alphabetCount == 0 ? (no / alphabetCount) - 1 : no / alphabetCount;
                if (no > alphabetCount)
                    return A(no) + " " + postfix;
            }

            return postfix == String.Empty ? no.ToString() : no + " " + postfix;
        }

        public static String NumberToAlphabetByOrder(Int32 no)
        {
            String idxString = A(no);
            String[] splittedIdxString = idxString.Split(' ');
            String finalString = String.Empty;
            foreach (String idx in splittedIdxString)
            {
                Int16 idxNo = Convert.ToInt16(idx);
                finalString += splittedAlphabet[idxNo - 1];
            }

            return finalString;
        }

        private static String B(String alphabetString)
        {
            if (String.IsNullOrEmpty(alphabetString))
                throw new ArgumentNullException("alphabetString");

            String idxString = String.Empty;
            foreach (Char alpha in alphabetString)
                for (int idx = 0; idx < alphabetCount; idx++)
                    if (alpha.ToString() == splittedAlphabet[idx])
                        idxString += idx + 1 + " ";

            return idxString.TrimEnd();
        }

        public static Int32 AlphabetToNumberByOrder(String alphabetString)
        {
            String idxString = B(alphabetString);
            Int32 no = 0;
            String[] splittedIdxString = idxString.Split(' ');
            for (int idx = 0; idx < splittedIdxString.Length; idx++)
            {
                Int32 idxNo = Convert.ToInt32(splittedIdxString[idx]);
                no += idx == splittedIdxString.Length - 1 ? idxNo : idxNo * Convert.ToInt32(Math.Pow(alphabetCount, splittedIdxString.Length - (idx + 1)));
            }

            return no;
        }
    }
}
