interface KeyValue {
    [key: string]: any | undefined;
}

interface IDict {
    get(key: string): any | undefined;
    add(key: string, value: any): void;
    remove(key: string): void;
    containsKey(key: string): boolean;
    keys(): string[];
    values(): any[];
    count(): number;
}

class Dict implements IDict {
    internal: KeyValue[] = [];

    constructor(initial?: { key: string; value: any; }[]) {
        if (initial)
            for (let idx = 0; idx < initial.length; idx++)
                this.add(initial[idx].key, initial[idx].value);
    }

    get(key: string): any | undefined {
        let idx: number = this.internal.indexOf((kv: KeyValue) => kv.key === key);
        if (idx !== -1)
            return this.internal[idx].value;
    }

    add(key: string, value: any): void {
        let idx: number = this.internal.indexOf((kv: KeyValue) => kv.key === key);
        if (idx !== -1) {
            this.internal[idx].value = value;
            return;
        }

        this.internal.push({
            key: key,
            value: value
        });
    }

    remove(key: string): void {
        let idx = this.internal.indexOf((kv: KeyValue) => kv.key === key);
        if (idx !== -1)
            this.internal.splice(idx, 1);

        delete this.internal[idx];
    }

    containsKey(key: string): boolean {
        let idx = this.internal.indexOf((kv: KeyValue) => kv.key === key);
        return idx !== -1;
    }

    keys(): string[] {
        return this.internal.map(kv => kv.key);
    }

    values(): any[] {
        return this.internal.map(kv => kv.value);
    }

    count(): number {
        return this.internal.length;
    }
}
