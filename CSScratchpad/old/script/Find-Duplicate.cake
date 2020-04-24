#l "Common.cake"

using System;
using System.Collections;
using System.Collections.Generic;

void Script() {
    try {
        var dataList = new List<String>();
        dataList.Add("08803012");
        dataList.Add("00111342");
        dataList.Add("09506564");
        dataList.Add("09003194");
        dataList.Add("09205132");
        dataList.Add("09709662");
        dataList.Add("09003122");
        dataList.Add("09709503");
        dataList.Add("09507544");
        dataList.Add("00010563");
        dataList.Add("09104840");
        dataList.Add("00517054");
        dataList.Add("09708543");
        dataList.Add("09507674");
        dataList.Add("09405675");
        dataList.Add("08700935");
        dataList.Add("09507796");
        dataList.Add("00123456");
        dataList.Add("00112041");
        dataList.Add("09507798");
        dataList.Add("09507862");
        dataList.Add("09506852");
        dataList.Add("09608446");
        dataList.Add("09507313");
        dataList.Add("09507537");
        dataList.Add("09405933");
        dataList.Add("00112047");
        dataList.Add("09205060");
        dataList.Add("09205094");
        dataList.Add("09709493");
        dataList.Add("00111845");
        dataList.Add("09406395");
        dataList.Add("09405687");
        dataList.Add("09709696");
        dataList.Add("09003307");
        dataList.Add("00414860");
        dataList.Add("00112443");
        dataList.Add("09205028");
        dataList.Add("09608232");
        dataList.Add("09506678");
        dataList.Add("09607998");
        dataList.Add("08903043");
        dataList.Add("00415165");

        dataList.Add("00010563");
        dataList.Add("00111342");
        dataList.Add("00111845");
        dataList.Add("00112041");
        dataList.Add("00112047");
        dataList.Add("00112443");
        dataList.Add("00414860");
        dataList.Add("00415165");
        dataList.Add("00517054");
        dataList.Add("08700935");
        dataList.Add("08803012");
        dataList.Add("08903043");
        dataList.Add("09003122");
        dataList.Add("09003194");
        dataList.Add("09003307");
        dataList.Add("09104840");
        dataList.Add("09205028");
        dataList.Add("09205060");
        dataList.Add("09205094");
        dataList.Add("09205132");
        dataList.Add("09405675");
        dataList.Add("09405687");
        dataList.Add("09405933");
        dataList.Add("09406395");
        dataList.Add("09506564");
        dataList.Add("09506678");
        dataList.Add("09506852");
        dataList.Add("09507313");
        dataList.Add("09507537");
        dataList.Add("09507544");
        dataList.Add("09507674");
        dataList.Add("09507796");
        dataList.Add("09507798");
        dataList.Add("09507862");
        dataList.Add("09607998");
        dataList.Add("09608232");
        dataList.Add("09608446");
        dataList.Add("09708543");
        dataList.Add("09709493");
        dataList.Add("09709503");
        dataList.Add("09709662");
        dataList.Add("09709696");

        Dbg($"ContainsDuplicate: {dataList.ContainsDuplicate()}");
        Dbg("Here are the duplicates");
        Dbg(dataList.GetDuplicate());
        dataList.CheckDuplicate();
    }
    catch (Exception ex) {
        Dbg(ex);
    }

    try {
        var dl1 = new List<String>();
        dl1.Add("08803012");
        dl1.Add("00111342");
        dl1.Add("09506564");
        dl1.Add("09003194");
        dl1.Add("09205132");
        dl1.Add("09709662");
        dl1.Add("09003122");
        dl1.Add("09709503");
        dl1.Add("09507544");
        dl1.Add("00010563");
        dl1.Add("09104840");
        dl1.Add("00517054");
        dl1.Add("09708543");
        dl1.Add("09507674");
        dl1.Add("09405675");
        dl1.Add("08700935");
        dl1.Add("09507796");
        dl1.Add("00123456");
        dl1.Add("00112041");
        dl1.Add("09507798");
        dl1.Add("09507862");
        dl1.Add("09506852");
        dl1.Add("09608446");
        dl1.Add("09507313");
        dl1.Add("09507537");
        dl1.Add("09405933");
        dl1.Add("00112047");
        dl1.Add("09205060");
        dl1.Add("09205094");
        dl1.Add("09709493");
        dl1.Add("00111845");
        dl1.Add("09406395");
        dl1.Add("09405687");
        dl1.Add("09709696");
        dl1.Add("09003307");
        dl1.Add("00414860");
        dl1.Add("00112443");
        dl1.Add("09205028");
        dl1.Add("09608232");
        dl1.Add("09506678");
        dl1.Add("09607998");
        dl1.Add("08903043");
        dl1.Add("00415165");

        var dl2 = new List<String>();
        dl2.Add("00010563");
        dl2.Add("00111342");
        dl2.Add("00111845");
        dl2.Add("00112041");
        dl2.Add("00112047");
        dl2.Add("00112443");
        dl2.Add("00414860");
        dl2.Add("00415165");
        dl2.Add("00517054");
        dl2.Add("08700935");
        dl2.Add("08803012");
        dl2.Add("08903043");
        dl2.Add("09003122");
        dl2.Add("09003194");
        dl2.Add("09003307");
        dl2.Add("09104840");
        dl2.Add("09205028");
        dl2.Add("09205060");
        dl2.Add("09205094");
        dl2.Add("09205132");
        dl2.Add("09405675");
        dl2.Add("09405687");
        dl2.Add("09405933");
        dl2.Add("09406395");
        dl2.Add("09506564");
        dl2.Add("09506678");
        dl2.Add("09506852");
        dl2.Add("09507313");
        dl2.Add("09507537");
        dl2.Add("09507544");
        dl2.Add("09507674");
        dl2.Add("09507796");
        dl2.Add("09507798");
        dl2.Add("09507862");
        dl2.Add("09607998");
        dl2.Add("09608232");
        dl2.Add("09608446");
        dl2.Add("09708543");
        dl2.Add("09709493");
        dl2.Add("09709503");
        dl2.Add("09709662");
        dl2.Add("09709696");

        Dbg(
            dl1.Select(dl => $"Data: {dl}, IsDuplicate: {dl2.Contains(dl)}")
        );
    }
    catch (Exception ex) {
        Dbg(ex);
    }
}

// NOTE: Extension method are not avilable in cake due to this script is under static class scope
public static void CheckDuplicate(this IList suspectedList) {
    if (suspectedList.ContainsDuplicate())
        throw new Exception("Duplicate!!!");
}

public static Boolean ContainsDuplicate(this IList suspectedList) {
    return suspectedList.GetDuplicate().Count() > 0;
}

public static IEnumerable GetDuplicate(this IList suspectedList) {
    Int32 suspectedListCount = suspectedList.Count();

    for (int i = 0; i < suspectedListCount - 1; i++)
        for (int j = i + 1; j < suspectedListCount; j++)
            if (suspectedList[i].ToString() == suspectedList[j].ToString())
                yield return suspectedList[i].ToString();
}

public static int Count(this IEnumerable source) {
    int res = 0;
    foreach (var item in source)
        res++;

    return res;
}

