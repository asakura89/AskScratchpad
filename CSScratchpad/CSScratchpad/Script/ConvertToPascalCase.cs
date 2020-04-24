using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class ConvertToPascalCase : Common, IRunnable {
        public void Run() {
            String str = "I_don't-want-this moment. To_ever-end.";
            Dbg(str);
            Dbg(str.ToPascalCase());

            String str2 = "Halo galo Halio_hasga-ncJdm:;jjakdhBCKbjd/hak\\ad ad";
            Dbg(str2);
            Dbg(str2.ToPascalCase());

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            Dbg(textInfo.ToTitleCase(str));
            Dbg(textInfo.ToTitleCase(str.ToLower()));
            Dbg(textInfo.ToTitleCase(str2));
            Dbg(textInfo.ToTitleCase(str2.ToLower()));

            // NOTE: see the difference!
        }
    }

    public static class StringExtensions {
        static String[] SplitByCommonSplitter(this String str) =>
            str
                .Replace(" ", "*")
                .Replace("-", "*")
                .Replace("_", "*")
                .Replace("\\", "*")
                .Replace("/", "*")
                .Split('*');

        static String FirstCharToUpperCase(this String str) {
            String upperCasedFisrtChar = str[0].ToString().ToUpper();
            String restOfString = str.Substring(1, str.Length - 1);

            return upperCasedFisrtChar + restOfString;
        }

        static IEnumerable<Int32> GetCommonSplitterIndex(this String str) {
            Char[] strArray = str.ToCharArray();
            for (Int32 i = 0; i < strArray.Length; i++) {
                String s = strArray[i].ToString();
                // common splitter i know are whitespace (" "),
                // underscore ("_"), dash ("-"), slash ("/"),
                // backslash ("\"), dot (".") and comma (",")
                if (s == " " || s == "_" || s == "-" || s == "/" ||
                    s == "\\" || s == "." || s == ",") {
                    yield return i;
                }
            }
        }

        public static String ToPascalCase(this String str) {
            var separatorIndex = str.GetCommonSplitterIndex().ToList();
            String[] splittedWords = str.SplitByCommonSplitter();
            String titleCasedString = String.Join(" ", splittedWords.Select(s => s.FirstCharToUpperCase()));

            var titleCasedSb = new StringBuilder(titleCasedString);
            foreach (Int32 index in separatorIndex) {
                titleCasedSb[index] = str[index];
            }

            return titleCasedSb.ToString();
        }
    }
}
