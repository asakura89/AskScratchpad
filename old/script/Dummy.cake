#tool nuget:?package=HtmlAgilityPack&version=1.4.0
#r ../tools/htmlagilitypack.1.4.0/HtmlAgilityPack/lib/HtmlAgilityPack.dll

#tool nuget:?package=Newtonsoft.Json&version=10.0.3
#r ../tools/newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

#r "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"

using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Reflection;

public String CleanURL(String Url, String Domain) {
    String strReturn = String.Empty;

    Boolean bHasDomain = GetDomainFromUrl(Url).Equals("") ? false : true;
    if (bHasDomain)
        strReturn = Url;
    else {
        String strImgLink = "/" + Url;
        String strPattern2Find = "^//?.*";
        String strPattern2Replace = "//?";
        String strReplacer = "/";
        strReturn = Domain + FindAndReplace(strImgLink, strPattern2Find, strPattern2Replace, strReplacer);
    }

    return strReturn;
}

public String GetDomainFromUrl(String Url) {
    String strReturn = String.Empty;
    
    String strPattern2Find = "^https?://.*/";
    Match mat = Regex.Match(Url, strPattern2Find, RegexOptions.Singleline);
    strReturn = mat.Groups[0].Value;
    
    return strReturn;
}

public String FindAndReplace(String source, String pattern2Find, String pattern2Replace, String replacer) {
    String replaced = String.Empty;
    
    Match mat = Regex.Match(source, pattern2Find, RegexOptions.Singleline);
    replaced = Regex.Replace(mat.Groups[0].Value, pattern2Replace, replacer);
    
    return replaced;
}

Task("Main")
    .Does(() => {
        String domain = "http://www.mcdonalds.co.id";
        String[] urls = {
            "http://www.mcdonalds.co.id/wp-content/gallery/menu-andalan/chicken_pahadada-edit.jpg",
            "/menu-andalan/chicken_pahadada-edit.jpg"
        };
        
        foreach (String url in urls)
            Console.WriteLine(CleanURL(url, domain));

        Console.WriteLine(JsonConvert.SerializeObject(
            AppDomain.CurrentDomain.GetAssemblies()
                .Select(asm => asm.FullName)
                .OrderBy(name => name),
            Formatting.Indented)
        );
        
    })
    .ReportError(ex => Error(ex.Message));

RunTarget("Main");
