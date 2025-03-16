/* class Emitter {
    e: { key: "", value: {} }[];

    constructor() {
        this.e = new Array();
    }

    get count(): number {
        return this.e.length;
    }

    on(name: string, callback: ): Emitter {
        if (!name)
            throw new EmitterException("Name must be specified.");

        if (!callback)
            throw new EmitterException("Invalid callback.");

        // Check if the event already exists
        if (!this.e.has(name))
            // If not, create a new Set instance to store the callbacks
            this.e.set(name, new Set());

        // Add the callback to the set
        this.e.get(name).add(callback);

        return this;
    }

    // Unregisters a callback or all callbacks for an event
    off(name, callback) {
        if (!name)
            throw new EmitterException("Name must be specified.");

        // Check if the event exists
        if (!this.e.has(name))
            return this;

        if (!callback) {
            // If no callback is specified, delete the event and all its callbacks
            this.e.delete(name);
            return this;
        }

        // Get the set of callbacks for the event
        let callbacks = this.e.get(name);
        // Delete the callback from the set
        callbacks.delete(callback);

        // Check if the set is empty
        if (callbacks.size === 0)
            // If yes, delete the event
            this.e.delete(name);

        return this;
    }

    // Registers a callback for an event that will be executed only once
    once(name, callback) {
        if (!name)
            throw new EmitterException("Name must be specified.");

        if (!callback)
            throw new EmitterException("Invalid callback.");

        let self = this;
        let wrapper = null;
        wrapper = arg => {
            // Unregister the wrapper callback
            self.off(name, wrapper);
            // Invoke the original callback
            callback(arg);
        };

        // Register the wrapper callback
        return self.on(name, wrapper);
    }

    // Emits an event with an argument
    emit(name, arg) {
        if (!name)
            throw new EmitterException("Name must be specified.");

        // Check if the event exists
        if (!this.e.has(name))
            return this;

        // Get the set of callbacks for the event
        let callbacks = this.e.get(name);
        // Invoke each callback with the argument
        for (let callback of callbacks)
            callback(arg);

        return this;
    }
}

// Class to represent the emitter exception
class EmitterException extends Error {
    // Constructor
    constructor(message) {
        // Call the super constructor with the message
        super(message);
        // Set the name of the error
        this.name = "EmitterException";
    }
}
 */