const dateISO = new Date().toISOString();
const readable = dateISO
    .replace(/[\.Z]/gm, "")
    .replace(/\-/gm, ".")
    .replace(/T/gm, ".")
    .slice(0, 19);
const idFromDate = dateISO.replace(/[\.\-:TZ]/gm, "").slice(0, 14);

console.table({
    dateISO,
    readable,
    idFromDate
});