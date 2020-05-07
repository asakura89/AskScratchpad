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
