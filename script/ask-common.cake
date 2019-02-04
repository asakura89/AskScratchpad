#tool nuget:?package=Newtonsoft.Json&version=10.0.3
#r ../tools/newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

using Newtonsoft.Json;

DirectoryPath DataDir = Context
        .Environment
        .WorkingDirectory
        .Combine("..")
        .Combine("data");

String DataDirPath => DataDir.FullPath;

String GetDataPath(String configfilename) =>
        DataDir
        .Combine(configfilename)
        .FullPath;

void Dbg(Object obj) => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
void DbgCake(Object obj) => Information(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
