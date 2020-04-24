#l "Common.cake"

using System;

// NOTE: Exy already added in Common.cake

void FakeDataGetter() {
    throw new IOException("Network is closed.");
}

void FakeProcessingMethod() {
    try {
        FakeDataGetter();
    }
    catch (Exception ex) {
        throw new InvalidOperationException("Failed get data because of I/O problem.", ex);
    }
}

void Script() {
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

