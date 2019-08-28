using System;
using System.IO;

namespace CSScratchpad.Script {
    class TestExy : Common, IRunnable {

        // NOTE: Exy already added in Common.cs
        public void Run() {
            try {
                FakeDataGetter();
            }
            catch (Exception ex) {
                Dbg(ex.GetExceptionMessage());
            }

            try {
                FakeProcessingMethod();
            }
            catch (Exception ex) {
                Dbg(ex.GetExceptionMessage());
            }
        }

        void FakeDataGetter() => throw new IOException("Network is closed.");

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
