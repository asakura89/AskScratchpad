using System;
using System.Linq;
using System.Text;

namespace CSScratchpad.Script {
    public class GenerateTenGuid : Common, IRunnable {
        public void Run() {
            var builder = new StringBuilder();
            for (Int32 idx = 0; idx < 10; idx++)
                builder.Append(Guid.NewGuid().ToString()).AppendLine();

            Dbg("Ten Guid",
                builder
                    .ToString()
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(item => {
                        String guid = item.ToLower().Replace("-", "");
                        String base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(guid));
                        return new {
                            OriGuid = item,
                            OriGuidLen = item.Length,
                            Guid = guid,
                            GuidLen = guid.Length,
                            GuidBase64 = base64,
                            GuidBase64Len = base64.Length
                        };
                    })
            );

            var builder2 = new StringBuilder();
            var newG = Guid.NewGuid();
            Dbg("New Guid",
                builder2
                    .Append(newG).AppendLine()
                    .Append(newG.ToString("N"))
                    .ToString()
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(item => item)
            );
        }
    }
}
