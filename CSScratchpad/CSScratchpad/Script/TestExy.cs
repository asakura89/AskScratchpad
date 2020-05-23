using System;
using System.IO;
using Scratch;

namespace CSScratchpad.Script {
    class TestExy : Common, IRunnable {

        // NOTE: Exy already added in Common.cs
        public void Run() {
            try {
                NetworkReader();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.GetExceptionMessage());
            }

            try {
                FakeProcessingMethod();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.GetExceptionMessage());
            }
        }

        void NetworkReader() => throw  new IOException("Can't read from network.");

        void FakeDataGetter() => NetworkReader();

        void FakeProcessingMethod() {
            try {
                FakeDataGetter();
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Failed get data because of I/O problem.", ex);
            }
        }
    }
}
