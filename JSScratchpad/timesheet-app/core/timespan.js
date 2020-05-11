const MillisecsPerSecond = 1000;
const MillisecsPerMinute = MillisecsPerSecond * 60;
const MillisecsPerHour = MillisecsPerMinute * 60;
const MillisecsPerDay = MillisecsPerHour * 24;

class TimeSpan {
    constructor ({days = 0, hours = 0, minutes = 0, seconds = 0, milliseconds = 0}) {
        let millisecsOnDays = days * MillisecsPerDay;
        let millisecsOnHours = hours * MillisecsPerHour;
        let millisecsOnMinutes = minutes * MillisecsPerMinute;
        let millisecsOnSeconds = seconds * MillisecsPerSecond;

        this.totalMillisecs = millisecsOnDays + millisecsOnHours + millisecsOnMinutes + millisecsOnSeconds + milliseconds;
    }

    get Days() {
        return Math.floor(this.totalMillisecs / MillisecsPerDay);
    }

    get Hours() {
        return Math.floor((this.totalMillisecs / MillisecsPerHour) % 24);
    }

    get Minutes() {
        return Math.floor((this.totalMillisecs / MillisecsPerMinute) % 60);
    }

    get Seconds() {
        return Math.floor((this.totalMillisecs / MillisecsPerSecond) % 60);
    }

    get Milliseconds() {
        return Math.floor(this.totalMillisecs % 1000);
    }

    get TotalDays() {
        return this.totalMillisecs / MillisecsPerDay;
    }

    get TotalHours() {
        return this.totalMillisecs / MillisecsPerHour;
    }

    get TotalMinutes() {
        return this.totalMillisecs / MillisecsPerMinute;
    }

    get TotalSeconds() {
        return this.totalMillisecs / MillisecsPerSecond;
    }

    get TotalMilliseconds() {
        return this.totalMillisecs;
    }

    Add(timespan) {
        let result = this.totalMillisecs + timespan.TotalMilliseconds;
        return new TimeSpan({milliseconds: result});
    }

    Substract(timespan) {
        let result = this.totalMillisecs - timespan.TotalMilliseconds;
        return new TimeSpan({milliseconds: result});
    }
}

export default TimeSpan;

/*
var ts = new TimeSpan({days: 3, minutes: 40});

console.log(ts.Days);
console.log(ts.Hours);
console.log(ts.Minutes);
console.log(ts.Seconds);
console.log(ts.Milliseconds);
console.log((3 * 24 * 60 * 60 * 1000) + (40 * 60 * 1000));

ts = new TimeSpan({days: 700, hours: 35});

console.log(ts.Days);
console.log(ts.Hours);
console.log(ts.Minutes);
console.log(ts.Seconds);
console.log(ts.Milliseconds);
*/
