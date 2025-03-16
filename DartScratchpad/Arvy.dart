import 'InvalidOperationException.dart'

library Arvy;

class ActionResponseViewModel {
    static final String Info = "I";
    static final String Warning = "W";
    static final String Error = "E";
    static final String Success = "S";
    String ResponseType;
    String Message;

    toString() => toStringWithChecking(true);

    toStringWithChecking(bool alwaysReturn) {
        if (!alwaysReturn && ResponseType == Error)
            throw new InvalidOperationException(Message);

        return ResponseType + "|" + Message;
    }
}
