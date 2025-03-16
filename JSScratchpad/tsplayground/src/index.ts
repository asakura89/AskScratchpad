/* class StringHelper {
    constructor() { }

    getStringPadding(width: number, pad?: string): string {
        if (pad === undefined || pad === null)
            pad = " ";

        var padded = "";
        for(var idx = 0; idx < width; idx++)
            padded += pad;

        return padded;
    }

    padLeft(str: string, width: number): string;
    padLeft(str: string, width: number, pad: string): string;
    padLeft(str: string, width: number, pad?: string): string {
        if (width <= 0)
            return str;

        var padWidth = width - str.length;
        if (padWidth <= 0)
            return str;

        var padded = this.getStringPadding(padWidth, pad);
        var resulting = padded + str;

        return (resulting).substring(resulting.length - width, resulting.length);
    }

    padRight(str: string, width: number): string;
    padRight(str: string, width: number, pad: string): string;
    padRight(str: string, width: number, pad?: string): string {
        if (width <= 0)
            return str;

        var padWidth = width - str.length;
        if (padWidth <= 0)
            return str;

        var padded = this.getStringPadding(padWidth, pad);

        return (str + padded).substring(0, width);
    }
}
 */