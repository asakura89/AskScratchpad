using System;
using System.Linq;

namespace CSScratchpad.Script {
    class SimpleStickyNotesDatetimeFromFloat : Common, IRunnable {
        public void Run() {
            Dbg($"{43077.05898:00.000000}");
            Dbg($"{43077.05898:00.000000}");
            Dbg(43077.05898.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty));

            Dbg(new DateTime(Convert.ToInt64(43077.05898.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty))));
            Dbg(new DateTime(Convert.ToInt64(42409.9604.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty))));
            Dbg(new DateTime(Convert.ToInt64(43210.4714543981.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty))));

            Dbg(new DateTime(Convert.ToInt64(42409.9604.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty)), DateTimeKind.Unspecified));
            Dbg(new DateTime(Convert.ToInt64(42409.9604.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty)), DateTimeKind.Local));
            Dbg(new DateTime(Convert.ToInt64(42409.9604.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", String.Empty)), DateTimeKind.Utc));

            Dbg("new DateTime(DateTime.MaxValue.Ticks)", new DateTime(DateTime.MaxValue.Ticks));
            Dbg("DateTime.MinValue.Ticks", DateTime.MinValue.Ticks);
            Dbg("DateTime.MaxValue.Ticks", DateTime.MaxValue.Ticks);
            Dbg("Int64.MaxValue", Int64.MaxValue);


            var a = new[] {
                43077.05898,
                42409.9604,
                43210.4714543981
            }
            .Select(item => Int64.MaxValue / item);

            Dbg(a);

            var b = a.Select(item => new DateTime(Convert.ToInt64(item)));

            Dbg(b);

            Dbg(new DateTime(2016, 2, 9).Ticks);
            Dbg((new DateTime(2016, 2, 9).Ticks / 42409.9604));
            Dbg((Int64.MaxValue / new DateTime(2016, 2, 9).Ticks));


            Dbg((new DateTime(2016, 2, 25).Ticks / 43210.4714543981));

            var a2 = new[] {
                43077.05898,
                42409.9604,
                43210.4714543981
            }
            .Select(item => item * 14994254227128);

            Dbg(a2);

            var b2 = a2.Select(item => new DateTime(Convert.ToInt64(item)));

            Dbg(b2);

            Dbg("635905728000000000".Length);
            Dbg("6,45908373703107".Length);
            Dbg($"{6.45908373703107 * 100000000000000000}");
        }
    }
}
