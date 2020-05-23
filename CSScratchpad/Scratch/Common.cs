using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Scratch {
    public abstract class Common {
        protected readonly String DataDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        protected String GetDataPath(String filename) {
            if (!Directory.Exists(DataDirPath))
                Directory.CreateDirectory(DataDirPath);

            return Path.Combine(DataDirPath, filename);
        }

        protected readonly String OutputDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

        protected String GetOutputPath(String filename) {
            if (!Directory.Exists(OutputDirPath))
                Directory.CreateDirectory(OutputDirPath);

            return Path.Combine(OutputDirPath, filename);
        }

        protected void Dbg(Object obj) => Dbg(String.Empty, obj);

        protected void Dbg(String title, Object obj) {
            if (!String.IsNullOrEmpty(title)) {
                Console.WriteLine(title);
                Console.WriteLine(
                    String.Join(
                        String.Empty,
                        Enumerable.Repeat("-", title.Length)
                    ));
            }

            if (!(obj is String && String.IsNullOrEmpty(obj.ToString()))) {
                Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                Console.WriteLine();
            }
        }
    }

    public static class ExcpExt {
        public static String GetExceptionMessage(this Exception ex) {
            var errorList = new StringBuilder();
            Exception current = ex;
            while (current != null) {
                errorList
                    .AppendLine($"Exception: {current.GetType().FullName}")
                    .AppendLine($"Message: {current.Message}")
                    .AppendLine($"Source: {current.Source}")
                    .AppendLine(current.StackTrace)
                    .AppendLine();

                current = current.InnerException;
            }

            return errorList.ToString();
        }
    }
}
