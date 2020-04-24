using System;
using System.Security.Cryptography;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class GenerateMD5Base64 : Common, IRunnable {
        public void Run() {
            String stringToHash = "StringToMD5Base64";
            using (var algo = MD5.Create()) {
                Byte[] hashedBytes = algo.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                String output = Convert.ToBase64String(hashedBytes);

                Console.WriteLine(output);
            }
        }
    }
}
