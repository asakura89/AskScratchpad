using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class FindDuplicates : Common, IRunnable {
        public void Run() {
            try {
                var dataList = new List<String> {
                    "08803012",
                    "00111342",
                    "09506564",
                    "09003194",
                    "09205132",
                    "09709662",
                    "09003122",
                    "09709503",
                    "09507544",
                    "00010563",
                    "09104840",
                    "00517054",
                    "09708543",
                    "09507674",
                    "09405675",
                    "08700935",
                    "09507796",
                    "00123456",
                    "00112041",
                    "09507798",
                    "09507862",
                    "09506852",
                    "09608446",
                    "09507313",
                    "09507537",
                    "09405933",
                    "00112047",
                    "09205060",
                    "09205094",
                    "09709493",
                    "00111845",
                    "09406395",
                    "09405687",
                    "09709696",
                    "09003307",
                    "00414860",
                    "00112443",
                    "09205028",
                    "09608232",
                    "09506678",
                    "09607998",
                    "08903043",
                    "00415165",

                    "00010563",
                    "00111342",
                    "00111845",
                    "00112041",
                    "00112047",
                    "00112443",
                    "00414860",
                    "00415165",
                    "00517054",
                    "08700935",
                    "08803012",
                    "08903043",
                    "09003122",
                    "09003194",
                    "09003307",
                    "09104840",
                    "09205028",
                    "09205060",
                    "09205094",
                    "09205132",
                    "09405675",
                    "09405687",
                    "09405933",
                    "09406395",
                    "09506564",
                    "09506678",
                    "09506852",
                    "09507313",
                    "09507537",
                    "09507544",
                    "09507674",
                    "09507796",
                    "09507798",
                    "09507862",
                    "09607998",
                    "09608232",
                    "09608446",
                    "09708543",
                    "09709493",
                    "09709503",
                    "09709662",
                    "09709696"
                };

                Dbg($"ContainsDuplicate: {dataList.ContainsDuplicate()}");
                Dbg("Here are the duplicates");
                Dbg(dataList.GetDuplicate());
                dataList.CheckDuplicate();
            }
            catch (Exception ex) {
                Dbg(ex);
            }

            try {
                var dl1 = new List<String> {
                    "08803012",
                    "00111342",
                    "09506564",
                    "09003194",
                    "09205132",
                    "09709662",
                    "09003122",
                    "09709503",
                    "09507544",
                    "00010563",
                    "09104840",
                    "00517054",
                    "09708543",
                    "09507674",
                    "09405675",
                    "08700935",
                    "09507796",
                    "00123456",
                    "00112041",
                    "09507798",
                    "09507862",
                    "09506852",
                    "09608446",
                    "09507313",
                    "09507537",
                    "09405933",
                    "00112047",
                    "09205060",
                    "09205094",
                    "09709493",
                    "00111845",
                    "09406395",
                    "09405687",
                    "09709696",
                    "09003307",
                    "00414860",
                    "00112443",
                    "09205028",
                    "09608232",
                    "09506678",
                    "09607998",
                    "08903043",
                    "00415165"
                };

                var dl2 = new List<String> {
                    "00010563",
                    "00111342",
                    "00111845",
                    "00112041",
                    "00112047",
                    "00112443",
                    "00414860",
                    "00415165",
                    "00517054",
                    "08700935",
                    "08803012",
                    "08903043",
                    "09003122",
                    "09003194",
                    "09003307",
                    "09104840",
                    "09205028",
                    "09205060",
                    "09205094",
                    "09205132",
                    "09405675",
                    "09405687",
                    "09405933",
                    "09406395",
                    "09506564",
                    "09506678",
                    "09506852",
                    "09507313",
                    "09507537",
                    "09507544",
                    "09507674",
                    "09507796",
                    "09507798",
                    "09507862",
                    "09607998",
                    "09608232",
                    "09608446",
                    "09708543",
                    "09709493",
                    "09709503",
                    "09709662",
                    "09709696"
                };

                Dbg(
                    dl1.Select(dl => $"Data: {dl}, IsDuplicate: {dl2.Contains(dl)}")
                );
            }
            catch (Exception ex) {
                Dbg(ex);
            }
        }
    }

    public static class DupExt {
        public static void CheckDuplicate(this IList suspectedList) {
            if (suspectedList.ContainsDuplicate())
                throw new Exception("Duplicate!!!");
        }

        public static Boolean ContainsDuplicate(this IList suspectedList) => suspectedList.GetDuplicate().Count() > 0;

        public static IEnumerable GetDuplicate(this IList suspectedList) {
            Int32 suspectedListCount = suspectedList.Count();

            for (Int32 i = 0; i < suspectedListCount - 1; i++)
                for (Int32 j = i + 1; j < suspectedListCount; j++)
                    if (suspectedList[i].ToString() == suspectedList[j].ToString())
                        yield return suspectedList[i].ToString();
        }

        public static Int32 Count(this IEnumerable source) {
            Int32 res = 0;
            foreach (Object item in source)
                res++;

            return res;
        }
    }
}
