class EmitterEventArgs {
    eventName: string;
    data: Dict = new Dict();

    constructor(eventName: string, data: Dict) {
        if (!eventName)
            throw new EmitterError("Event name must be specified.");

        this.eventName = eventName;

        if (!data)
            throw new EmitterError("Data must be specified.");

        if (data.count() === 0)
            throw new EmitterError("Data must not be empty.");

        let dataKeys = data.keys();
        for (let key of dataKeys) {
            let value = data.get(key);
            this.data.add(key, value);
        }
    }
}