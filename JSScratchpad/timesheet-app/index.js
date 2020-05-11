import {default as TimeTextbox} from "./comps/hourtextbox.js";
import {default as TimeSpan} from "./core/timespan.js";

new Vue({
    el: "#app",
    data: {
        Days: [{
                DayOfWeek: 0,
                Name: "Sunday"
            }, {
                DayOfWeek: 1,
                Name: "Monday"
            }, {
                DayOfWeek: 2,
                Name: "Tuesday"
            }, {
                DayOfWeek: 3,
                Name: "Wednesday"
            }, {
                DayOfWeek: 4,
                Name: "Thursday"
            }, {
                DayOfWeek: 5,
                Name: "Friday"
            }, {
                DayOfWeek: 6,
                Name: "Saturday"
            }
        ],
        Totals: [
            "00:00",
            "00:00",
            "00:00",
            "00:00",
            "00:00",
            "00:00",
            "00:00"
        ]
    },
    components: {
        "time-textbox": TimeTextbox
    },
    methods: {
        UpdateTotals: function (dayOfWeek) {
            let startHour = parseInt(this.$el.querySelector(`#starthour-${dayOfWeek}`).value);
            startHour = startHour === 0 ? 24 : startHour;
            let startMin = parseInt(this.$el.querySelector(`#startmin-${dayOfWeek}`).value);
            let endHour = parseInt(this.$el.querySelector(`#endhour-${dayOfWeek}`).value);
            endHour = endHour === 0 ? 24 : endHour;
            let endMin = parseInt(this.$el.querySelector(`#endmin-${dayOfWeek}`).value);
            let breakHour = parseInt(this.$el.querySelector(`#breakhour-${dayOfWeek}`).value);
            let breakMin = parseInt(this.$el.querySelector(`#breakmin-${dayOfWeek}`).value);
            
            let ensTs = new TimeSpan({hours: endHour, minutes: endMin});

            let hour = endHour - startHour - breakHour;
            let min = endMin - startMin - breakMin;

            hour = hour < 0 ? 0 : hour;
            if (min < 0) {
                let hourts = new TimeSpan({hours: hour});
                let finalts = new TimeSpan({minutes: hourts.TotalMinutes + min});
                hour = finalts.Hours;
                min = finalts.Minutes;
            }

            this.$set(this.Totals, dayOfWeek, `${hour.toString().padStart(2, "0")}:${min.toString().padStart(2, "0")}`);
        },
        GetTotalHours: function () {
            let hours = this.Totals
                .map(perDay => perDay.split(":")[0])
                .map(hrs => Number(hrs))
                .reduce((prev, curr) => prev + curr, 0);

            let mins = this.Totals
                .map(perDay => perDay.split(":")[1])
                .map(mns => Number(mns))
                .reduce((prev, curr) => prev + curr, 0);

            let ts = new TimeSpan({hours: hours, minutes: mins});
            return `${ts.Hours.toString().padStart(2, "0")}:${ts.Minutes.toString().padStart(2, "0")}`;
        }
    }
});

