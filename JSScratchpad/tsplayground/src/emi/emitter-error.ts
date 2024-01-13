class EmitterError extends Error {
    constructor(message: string) {
        super(message);
        /* let errProto = Error.prototype;
        for (let prop in errProto) {
            let propInfo = Object.getOwnPropertyDescriptor(errProto, prop);
            if (propInfo) {
                Object.defineProperty(this, prop, propInfo);
            }
        } */

        this.name = "EmitterError";
    }
};

EmitterError.prototype = Error.prototype;

/*
class EmitterException extends Error {
    constructor(message: string) {
        super(message);
        this.name = "EmitterException";
    }
}

// if this is not set, `error instanceof EmitterException` is false
EmitterException.prototype = Error.prototype;

try {
    let e = new EmitterException("yahoo");
    throw e;
}
catch(error: any) {
    console.log(error.name === "EmitterException");
    console.log(error instanceof EmitterException);
    console.log(error instanceof Error);
    console.log(typeof error);

    console.table(error);
    console.log(error.toString());
    console.log(Object.prototype.toString.call(error).match(/^\[object\s+(.*?)\]$/));
}

*/