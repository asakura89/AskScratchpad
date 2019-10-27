using System;

namespace CSScratchpad.Script {
    internal class CombineUrl : Common, IRunnable {
        public void Run() {
            Dbg(Combine("/Asset/Script/", "/MainSite/"));
            Dbg(Combine("/Asset/Script/", String.Empty));
        }

        String Combine(String url1, String url2) {
            if (String.IsNullOrEmpty(url1))
                return url2;
            if (String.IsNullOrEmpty(url2))
                return url1;

            return $"{url1.TrimEnd('/', '\\')}/{url2.TrimStart('/', '\\')}";
        }
    }
}
