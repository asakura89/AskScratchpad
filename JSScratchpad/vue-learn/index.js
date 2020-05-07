new Vue({
    el: "#app",
    data: {
        name: "Vue",
        job: "Framework",
        website: "https://github.com/asakura89",
        jobBadge: "<img src='https://img.shields.io/badge/Framework-%C2%A0%E2%9C%94-brightgreen' />",
        highlightPos: undefined,
        nameHighlighted: "",
        jobHighlighted: "",
        websiteHighlighted: "",
        input: "",
        input2: ""
    },
    methods: {
        Greet: function(time) {
            return `Yo, ${this.name}! Good ${time}`;
        },
        Highlight: function (next) {
            var pos = ["name", "job", "website"];
            if (this.highlightPos === undefined) {
                this.highlightPos = "name";
            }
            else if (next) {
                var idx = pos.indexOf(this.highlightPos) +1;
                var adjusted = idx > (pos.length -1) ? (pos.length -1) : idx;
                this.highlightPos = pos[adjusted];
            }
            else {
                var idx = pos.indexOf(this.highlightPos) -1;
                var adjusted = idx < 0 ? 0 : idx;
                this.highlightPos = pos[adjusted];
            }
            
            this.nameHighlighted = this.highlightPos === "name" ? "bold" : "";
            this.jobHighlighted = this.highlightPos === "job" ? "bold" : "";
            this.websiteHighlighted = this.highlightPos === "website" ? "bold" : "";
        },
        Jump: function (next) {
            var pos = ["name", "job", "website"];
            if (this.highlightPos === undefined) {
                this.highlightPos = "name";
            }
            else if (next) {
                var idx = pos.indexOf(this.highlightPos) +2;
                var adjusted = idx > (pos.length -1) ? (pos.length -1) : idx;
                this.highlightPos = pos[adjusted];
            }
            else {
                var idx = pos.indexOf(this.highlightPos) -2;
                var adjusted = idx < 0 ? 0 : idx;
                this.highlightPos = pos[adjusted];
            }
            
            this.nameHighlighted = this.highlightPos === "name" ? "bold" : "";
            this.jobHighlighted = this.highlightPos === "job" ? "bold" : "";
            this.websiteHighlighted = this.highlightPos === "website" ? "bold" : "";
        },
        UpdateInput2(event) {
            console.log(event);
            var app = this.$el;
            this.input2 = app.querySelector("#input-2").value;
        },
        Warning(message, event) {
            if (event) {
                event.preventDefault();
            }
            console.log(event);
            alert(message);
        }
    }
});


new Vue({
    el: "#app-2",
    data: {
        resolutions: [
            "stay positive",
            "meet new people",
            "don't get stress",
            "don't complaining / squawking / sighing"
        ],
        nextyresolutions: [
            "always smiling",
            "keep the good mood",
            "wake up early",
            "express feelings with hobbies"
        ],
        ress: [
            {
                No: 1,
                Title: "stay positive"
            },
            {
                No: 2,
                Title: "meet new people"
            },
            {
                No: 3,
                Title: "don't get stress"
            },
            {
                No: 4,
                Title: "don't complaining / squawking / sighing"
            }
        ],
        selected: ""
    },
    filters: {
        TitleCasing: function (value) {
            if (!value) {
                return "";
            }
            value = value.toString();
            return value.charAt(0).toUpperCase() + value.slice(1);
        },
        CleansSelected: function (selected, data) {
            if (selected === "") {
                return {
                    No: -1,
                    Title: ""
                };
            }

            var idx = Number(selected) -1;
            return data[idx];
        },
        Expand: function (obj) {
            var propNames = Object.keys(obj);
            var expanded = "";
            var actual = [];
            for (var idx = 0; idx < propNames.length; idx++) {
                var name = propNames[idx];
                var current = obj[name];
                if (current === undefined) {
                    actual.push(`${name}: undefined`);
                }
                else if (current === null) {
                    actual.push(`${name}: null`);
                }
                else {
                    actual.push(`${name}: ${current.toString()}`);
                }

                expanded = Array.prototype.join.call(actual, ", ");
            }
            return expanded;
        }
    }
});