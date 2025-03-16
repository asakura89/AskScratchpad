<script lang="ts">
    interface Nvy {
        name: string,
        value: number
    }

    let email: string = "";
    let repeat: number = 0;
    let repeatOptions: Nvy[] = [
        { name: "Don't repeat", value: 0 },
        { name: "Daily", value: 1 },
        { name: "Weekly", value: 2 },
        { name: "Monthly", value: 3 },
        { name: "Yearly", value: 4 }
    ];
    let now = new Date();
    let startsOn: string = formatDateTime(now);
    let every: string = "1";
    let everyPeriod: number = 0;
    let everyPeriodOptions: Nvy[] = [
        { name: "", value: 0 },
        { name: "day(s)", value: 1 },
        { name: "week(s)", value: 2 },
        { name: "month(s)", value: 3 },
        { name: "year(s)", value: 4 }
    ];
    //let ends: number = 0;
    let neverEnds: boolean = true;
    let endsOn: boolean = !neverEnds;
    let endsOptions: Nvy[] = [
        { name: "Never", value: 0 },
        { name: "On", value: 1 }
    ];
    let endsOnDate: string = formatDateTime(new Date(now.getFullYear(), now.getMonth() +1, now.getDate() +1));
    let endsOnDateVisibility: string = endsOn ? "inline-block" : "none";
    let message: string = "";

    function endsOnChanged(e: Event) {
        console.table({
            eTargetChecked: e.target.checked,
            neverEnds: neverEnds,
            endsOn: endsOn,
            visibility: endsOnDateVisibility
        });
    }

    function formatDateTime(date: Date): string {
        return String(date.getDate()) +
            "/" +
            [
                "Jan", "Feb", "Mar", "Apr", "May",
                "Jun", "Jul", "Aug", "Sep", "Oct",
                "Nov", "Dec"
            ][date.getMonth() +1] +
            "/" +
            String(date.getFullYear() %100) +
            " " +
            (() => {let h = date.getHours(); return h >12 ? h %12 : h})()
                .toString()
                .padStart(2, "0") +
            ":" +
            date.getMinutes() +
            " " +
            (() => {let h = date.getHours(); return h >12 ? "pm" : "am"})();
    }

    /* function getStringPadding(width: number, pad?: string): string {
        if (!pad)
            pad = " ";

        let padded = "";
        for(let idx = 0; idx < width; idx++)
            padded += pad;

        return padded;
    }

    function padLeft(str: string, width: number, pad?: string): string {
        if (width <= 0)
            return str;

        var padWidth = width - str.length;
        if (padWidth <= 0)
            return str;

        var padded = getStringPadding(padWidth, pad);
        var resulting = padded + str;

        return (resulting).substring(resulting.length - width, resulting.length);
    } */

    function getEveryPeriod(selectedRepeat: number): string {
        let filtered: Nvy[] = everyPeriodOptions.filter(opt => opt.value === repeat);
        if (filtered && filtered.length > 0) {
            return filtered[0].name;
        }

        return everyPeriodOptions
            .filter(opt => opt.value === 0)[0]
            .name;
    }
</script>

<div class="container">
    <h2>Create a reminder</h2>
    <div class="section">
        <label for="email">To*</label>
        <input bind:value={email} type="email" id="email" required />
    </div>

    <div class="section">
        <label for="repeat">Repeat</label>
        <select bind:value={repeat} id="repeat" required>
            {#each repeatOptions as { name, value }, i}
                <!-- {#if value === 0}
                    <option value="{value}" selected>{name}</option>
                {:else}
                    <option value="{value}">{name}</option>
                {/if} -->
                <option value="{value}">{name}</option>
            {/each}
        </select>
    </div>

    <div class="section">
        <label for="starts-on">Starts On*</label>
        <input bind:value={startsOn} type="text" id="starts-on" required />
    </div>

    <div class="section">
        <label for="every">Every*</label>
        <input bind:value={every} type="text" id="every" required />
        {#key repeat}
            <span>{getEveryPeriod(repeat)}</span>
        {/key}
    </div>

    <div class="section">
        <label for="never-ends">Ends</label>
        <input bind:checked={neverEnds} type="checkbox" id="never-ends" on:change={endsOnChanged} />
        <input bind:checked={endsOn} type="checkbox" id="ends-on" on:change={endsOnChanged} />
        {#key endsOn}
            <span>
                <input bind:value={endsOnDate} type="text" id="ends-on-date" style="display: {endsOnDateVisibility};" />
            </span>
        {/key}
    </div>

    <button on:click={() => console.log("Add")}>Add</button>
    <button on:click={() => console.log("Close")}>Close</button>
</div>

