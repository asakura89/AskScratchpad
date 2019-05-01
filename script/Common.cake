#tool nuget:?package=Newtonsoft.Json&version=10.0.3
#r ../tools/newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

using Newtonsoft.Json;

DirectoryPath DataDir = Context
    .Environment
    .WorkingDirectory
    .Combine("..")
    .Combine("data");

String DataDirPath => DataDir.FullPath;

String GetDataPath(String filename) {
    if (!System.IO.Directory.Exists(DataDirPath))
        System.IO.Directory.CreateDirectory(DataDirPath);

    return DataDir
        .Combine(filename)
        .FullPath;
}

DirectoryPath OutputDir = Context
    .Environment
    .WorkingDirectory
    .Combine("..")
    .Combine("tmp");

String OutputDirPath => OutputDir.FullPath;

String GetOutputPath(String filename) {
    if (!System.IO.Directory.Exists(OutputDirPath))
        System.IO.Directory.CreateDirectory(OutputDirPath);

    return OutputDir
        .Combine(filename)
        .FullPath;
}

void Dbg(Object obj) => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
void DbgCake(Object obj) => Information(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));

public static String GetExceptionMessage(this Exception ex) {
    var errorList = new StringBuilder();
    if (ex.InnerException != null)
        errorList.AppendLine(GetExceptionMessage(ex.InnerException));

    return errorList
        .AppendLine(ex.Message)
        .ToString();
}

#region : Main :

Task("Main")
    .Does(() => {
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        Script();
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.GetExceptionMessage()));
RunTarget("Main");

#endregion
