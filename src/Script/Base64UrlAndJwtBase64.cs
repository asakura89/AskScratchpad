using System;
using System.Text;

namespace CSScratchpad.Script {
    class Base64UrlAndJwtBase64 : Common, IRunnable {
        public void Run() {
            const String base64Url = "eyJ1bmlxdWVfbmFtZSI6ImNzc2NyYXRjaHBhZCIsIkNTU2NyYXRjaHBhZC5TY3JpcHQuQ29yZS5XZWJBcGkuQ2xhaW1zLkNvbXBhbnlTaWQiOiJjY2ZkYjQyMjEwMTY0MjNkOGVjZTY5N2E3NTYzYThkNCIsIkNTU2NyYXRjaHBhZC5TY3JpcHQuQ29yZS5XZWJBcGkuQ2xhaW1zLkRldmljZVNpZCI6IjM5OTg3YmQyMTgyN2RhMjkiLCJuYmYiOjE1MjM1MjUzNzEsImV4cCI6MTUyNjExNzM3MSwiaWF0IjoxNTIzNTI1MzcxfQ";
            const String base64Url2 = "eyJ1bmlxdWVfbmFtZSI6ImNzc2NyYXRjaHBhZCIsIkNTU2NyYXRjaHBhZC5TY3JpcHQuQ29yZS5XZWJBcGkuQ2xhaW1zLkNvbXBhbnlTaWQiOiIyYzk4YzE1NDYyYzU0OTkyYmRkZGUzYWZhYjczYTBmOSIsIkNTU2NyYXRjaHBhZC5TY3JpcHQuQ29yZS5XZWJBcGkuQ2xhaW1zLkRldmljZVNpZCI6IjE1MDE2YjExYWFkYTQ0YmE4NGRiMDQ4ZDRhZTA0ZjRkIiwibmJmIjogMTUyMzI2MjcwNCwiZXhwIjogMTUyOTI2MjcwNCwiaWF0IjogMTUyMzI2MjcwNH0";

            Dbg("A. original", base64Url);
            Dbg("B. decoded", Decode(base64Url));

            Dbg("C. original", base64Url2);
            Dbg("D. decoded", Decode(base64Url2));

            const String json = "{\"unique_name\":\"csscratchpad\",\"CSScratchpad.Script.Core.WebApi.Claims.CompanySid\":\"ccfdb4221016423d8ece697a7563a8d4\",\"CSScratchpad.Script.Core.WebApi.Claims.DeviceSid\":\"39987bd21827da29\",\"nbf\":1523525371,\"exp\":1526117371,\"iat\":1523525371}";
            const String json2 = "{\"unique_name\":\"csscratchpad\",\"CSScratchpad.Script.Core.WebApi.Claims.CompanySid\":\"2c98c15462c54992bddde3afab73a0f9\",\"CSScratchpad.Script.Core.WebApi.Claims.DeviceSid\":\"15016b11aada44ba84db048d4ae04f4d\",\"nbf\": 1523262704,\"exp\": 1529262704,\"iat\": 1523262704}";

            Dbg("E. original", json);
            Dbg("F. encoded", Encode(json));

            Dbg("G. original", json2);
            Dbg("H. encoded", Encode(json2));

            Dbg("A == F", base64Url.Equals(Encode(json)));
            Dbg("C == H", base64Url2.Equals(Encode(json2)));
        }

        String Encode(String json) {
            return Convert
                .ToBase64String(Encoding.UTF8.GetBytes(json))
                .TrimEnd('=')
                .Replace("+", "-")
                .Replace("/", "_");
        }

        String Decode(String base64Url) {
            String base64 = base64Url
                .Replace("-", "+")
                .Replace("_", "/");

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    base64.Length % 4 == 2 ?
                        base64 + "==" :
                        base64.Length % 4 == 3 ?
                            base64 + "=" : base64
                )
            );
        }

        /*
        String Encode(String json) {
            String base64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(json.Dump("original"))
            );

            base64 = base64.Dump("formatted")
                .TrimEnd('=')
                .Replace("+", "-")
                .Replace("/", "_");

            return base64.Dump("encoded");
        }

        String Decode(String base64Url) {
            String base64 = base64Url.Dump("original")
                .Replace("-", "+")
                .Replace("_", "/");

            base64 = base64.Length % 4 == 2 ?
                base64 + "==" :
                base64.Length % 4 == 3 ?
                    base64 + "=" : base64;

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(base64.Dump("formatted"))
            ).Dump("decoded");
        }
        */
    }
}
