using System;
using System.IO;
using System.Threading;
using Scratch;

namespace CSScratchpad.Script
{
    public class WriteBinary : Common, IRunnable {
        public void Run() {
            using (var writer = new BinaryWriter(File.Open(GetOutputPath($"Binary_{DateTime.Now:ddMMyyyyhhmmss}.txt"), FileMode.Create)))
                writer.Write("Aku seorang kapiten");

            Thread.Sleep(3000);

            using (var writer = new BinaryWriter(File.Open(GetOutputPath($"Binary_{DateTime.Now:ddMMyyyyhhmmss}.txt"), FileMode.Create))) {
                writer.Write(65);
                writer.Write("Aku seorang kapiten");
            }
        }
    }
}
