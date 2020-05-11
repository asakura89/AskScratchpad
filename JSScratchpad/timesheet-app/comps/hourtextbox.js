const compTemplate = `
    <input
        :id="id"
        type="text"
        maxlength="2"
        size="2"
        v-model="value"
        @focus="OnFocus"
        @blur="OnBlur"
        @change="$emit('changed-callback', dayOfWeek)" />
`;

export default {
    name: "TimeTextbox",
    template: compTemplate,
    props: {
        id: { type: String },
        mode: { type: String },
        dayOfWeek: { type: Number }
    },
    data: function () {
        return {
            value: "00",
            innerValue: 0
        };
    },
    methods: {
        OnFocus: function () {
            this.value = "";
        },
        OnBlur: function () {
            this.innerValue =
                this.value === "00" || this.value === "0" || this.value === "" ?
                    0 :
                    Number(this.value);

            if (this.mode == "h")
                this.ValidateHour();
            else if (this.mode == "m")
                this.ValidateMinute();
            
            this.UpdateValue(this.innerValue);
        },
        UpdateValue: function (innerValue) {
            this.value = innerValue.toString().padStart(2, "0");
        },
        ValidateHour: function () {
            if (this.innerValue < 0)
                this.innerValue = 0;
            else if (this.innerValue > 24)
                this.innerValue = 24;
        },
        ValidateMinute: function () {
            if (this.innerValue < 0)
                this.innerValue = 0;
            else if (this.innerValue > 59)
                this.innerValue = 59;
        }
    }
};
