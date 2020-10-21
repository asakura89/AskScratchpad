using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Scratch;

namespace CSScratchpad.Script {
    public class TestDatetimeUtil : Common, IRunnable {
        public void Run() {
            // NOTE: DateTime Utils

            Dbg(new {
                DateTimeUtil = new {
                    TodayDayName = DateTimeUtil.GetTodayDayName(),
                    TodayDate = DateTimeUtil.GetTodayDate()
                }
            });

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: TryParse with AM and PM

            const String DateTimeFormatWithAMPM = "dd.MM.yyyy_hh:mm_tt";
            String datetimeString = DateTime.Now.ToString("dd.MM.yyyy") + "_10:30_AM";
            DateTime.TryParseExact(datetimeString, DateTimeFormatWithAMPM, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime inTime);
            DateTime added = inTime.AddHours(9);

            Dbg(
                new {
                    DateTimeTryParse = new {
                        OriginalString = datetimeString,
                        Parsed = inTime.ToString(DateTimeFormatWithAMPM),
                        WorktimeIsAdded9Hours = added.ToString(DateTimeFormatWithAMPM)
                    }
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Time span

            Dbg(
                new {
                    TimeSpan1 = new TimeSpan(0, 0, 0, 200),
                    TimeSpan2 = new TimeSpan(0, 0, 63, 59),
                    TimeSpan3 = new TimeSpan(0, 25, 45, 0),
                    TimeSpan4 = new TimeSpan(22, 25, 63, 200)
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Overtime Calculation

            const String SimpleDateTimeFormat = "dd.MM.yyyy hh:mm tt";
            var dt = Convert.ToDateTime("04 Sep 2017 07:30");
            Double overtimeBef = Convert.ToDouble("2") * -1;
            DateTime dt2 = dt.AddHours(overtimeBef);

            var befIn = Convert.ToDateTime("04 Sep 2017 07:30");
            var befOut = Convert.ToDateTime("04 Sep 2017 08:00");
            TimeSpan overtime = befOut - befIn;

            Dbg(
                new {
                    OvertimeCalcuation = new {
                        OriginalDateTime = dt.ToString(SimpleDateTimeFormat),
                        OvertimeBeafore = dt2.ToString(SimpleDateTimeFormat),
                        BeforeIn = befIn.ToString(SimpleDateTimeFormat),
                        BeforeOut = befOut.ToString(SimpleDateTimeFormat),
                        OvertimeTimeSpan = overtime.ToString(),
                        OvertimeMinutesDividedBy60 = ((Double) overtime.Minutes / 60).ToString(),
                        OvertimeHours = overtime.Hours.ToString()
                    }
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Time compare

            DateTime currentTime = DateTime.Now.AddDays(-3);
            String timeFrom = String.Format("{0}:{1}", currentTime.Hour, currentTime.Minute);
            var convertedCurrentTime = Convert.ToDateTime(timeFrom);
            Int32 span = convertedCurrentTime.CompareTo(DateTime.Now);

            Dbg(
                new {
                    TimeCompare = new {
                        Note1 = "<0 --> instance less than value",
                        Note2 = "0 --> instance equals to value",
                        Note3 = ">0 --> instance later than value",
                        TimeFrom = timeFrom,
                        ConvertedCurrentTime = convertedCurrentTime.ToString(SimpleDateTimeFormat),
                        CompareToDateTimeNow = span.ToString()
                    }
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Time Durations

            DateTime now = DateTime.Now;
            TimeSpan substract = new DateTime(now.Year, now.Month, now.Day, 17, 30, 0)
                .Subtract(new DateTime(now.Year, now.Month, now.Day, 8, 30, 0));

            var end = new DateTime(2018, 10, 26, 14, 35, 0, 0);
            var start = new DateTime(end.Year, end.Month, end.Day, 1, 25, 45, 5); // ms == ms * 10_000

            TimeSpan diff = end - start;

            Double totalSecs = Math.Floor(diff.TotalMilliseconds / 1_000);
            var timespan = new {
                Milliseconds = Math.Floor(diff.TotalMilliseconds) % totalSecs,
                Seconds = totalSecs % 15,
                Minutes = Math.Floor(diff.TotalMilliseconds / 1_000 / 60) % 60,
                Hours = Math.Floor(diff.TotalMilliseconds / 1_000 / 60 / 60) % 60,
                Days = Math.Floor(diff.TotalMilliseconds / 1_000 / 60 / 60 / 24) % 24,
                Months = Math.Floor(diff.TotalMilliseconds / 1_000 / 60 / 60 / 24 / 30) % 30,
                Years = Math.Floor(diff.TotalMilliseconds / 1_000 / 60 / 60 / 24 / 30 / 12) % 12
            };

            Dbg(
                new {
                    TimeDurations = new {
                        now,
                        substract,
                        start,
                        end,
                        Subtraction1 = 10_000_000 - 50_000,
                        Subtraction2 = (10_000_000 - 50_000) / 10_000,
                        diff,
                        DiffMilliseconds = diff.Milliseconds,
                        DiffSeconds = diff.Seconds,
                        DiffMinutes = diff.Minutes,
                        DiffHours = diff.Hours,
                        DiffDays = diff.Days,
                        totalSecs,
                        timespan
                    }
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Date compare to pure string

            Dbg(
                new {
                    DateCompareToPureString = new {
                        DataList = new List<SamplePOCO> {
                            new SamplePOCO {
                                Type = "Type1",
                                EffectiveDate = new DateTime(2014, 1, 1),
                                EndDate = new DateTime(1, 1, 1)
                            },
                            new SamplePOCO {
                                Type = "Type2",
                                EffectiveDate = new DateTime(2014, 1, 1),
                                EndDate = new DateTime(2014, 2, 28)
                            }
                        }

                        // F! I can't think a simpler way.
                        .Select(item =>
                            (item.Type.Equals("type1", StringComparison.InvariantCultureIgnoreCase) &&
                            item.EndDate.ToString("ddMMyyyy") != "01010001" &&
                            item.EndDate < item.EffectiveDate) ||

                            (item.Type.Equals("type2", StringComparison.InvariantCultureIgnoreCase) &&
                            item.EndDate.ToString("ddMMyyyy") != "01010001" &&
                            item.EndDate < item.EffectiveDate) ||

                            (item.Type.Equals("type2", StringComparison.InvariantCultureIgnoreCase) &&
                            item.EndDate.ToString("ddMMyyyy") == "01010001") ?
                                new ActionResponseViewModel {
                                    ResponseType = ActionResponseViewModel.Error,
                                    Message = $"End Date can't be earlier than Effective Date. Type: {item.Type}."
                                } :
                                new ActionResponseViewModel {
                                    ResponseType = ActionResponseViewModel.Success,
                                    Message = $"End Date and Effective Date validation success. Type: {item.Type}."
                                }
                        )
                    }
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Random Date Range

            var seedRand = new SeededRandom();
            var jan = new DateTime(2018, 1, 1);
            var feb = new DateTime(2018, 2, 1);

            Dbg(
                new {
                    RandomDateRange =
                        Enumerable
                            .Range(0, feb.Subtract(jan).Days /* +1 */)
                            .Select(range => jan.AddDays(range))
                            .Select(date => {
                                Int32 hour = seedRand.GetRandomNumber(7, 21);
                                Int32 mint = seedRand.GetRandomNumber(0, 59);
                                return $"{date:dd/MM/yyyy} {(hour < 10 ? $"0{hour}" : $"{hour}")}:{(mint < 10 ? $"0{mint}" : $"{mint}")}";
                            })
                            .Concat(
                                Enumerable
                                .Range(0, feb.Subtract(jan).Days /* +1 */)
                                .Select(range => jan.AddDays(range))
                                .Select(date => {
                                    Int32 hour = seedRand.GetRandomNumber(7, 21);
                                    Int32 mint = seedRand.GetRandomNumber(0, 59);
                                    return $"{date:dd/MM/yyyy} {(hour < 10 ? $"0{hour}" : $"{hour}")}:{(mint < 10 ? $"0{mint}" : $"{mint}")}";
                                })
                            )
                            .OrderBy(date => date)
                            .Select(date => DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None))
                            .Select(date => date.ToString(SimpleDateTimeFormat))
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: DateTime Now Formatted

            // Already exist above this
            // DateTime now = DateTime.Now;

            Dbg(
                new {
                    FormattedDateTimeNow = new {
                        NowWithoutCultureOptions = now.ToString(),
                        NowWithoutCultureOptionsFormatD = now.ToString("D"),
                        NowWithoutCultureOptionsFormatG = now.ToString("G"),
                        NowWithoutCultureOptionsFormatg = now.ToString("g"),
                        NowWithoutCultureOptionsFormatf = now.ToString("f"),
                        NowWithoutCultureOptionsFormatF = now.ToString("F"),
                        NowInvariantCulture = now.ToString(CultureInfo.InvariantCulture),
                        NowInvariantCultureFormatD = now.ToString("D", CultureInfo.InvariantCulture),
                        NowInvariantCultureFormatG = now.ToString("G", CultureInfo.InvariantCulture),
                        NowInvariantCultureFormatg = now.ToString("g", CultureInfo.InvariantCulture),
                        NowInvariantCultureFormatf = now.ToString("f", CultureInfo.InvariantCulture),
                        NowInvariantCultureFormatF = now.ToString("F", CultureInfo.InvariantCulture)
                    }
                }
            );

            var datetime1 = new DateTime(2017, 12, 19, 15, 30, 0);
            var datetime2 = new DateTime(2017, 12, 19, 17, 26, 0);
            const String DateFormat = "HHmmsstt";

            Int32 diffUtcNowAndNow = DateTime.UtcNow.Subtract(DateTime.Now).Hours;

            String sqlSimpleDate1 = datetime1.ToString("dd-yyyy").Replace("-", " " + datetime1.ToString("MMM").ToUpperInvariant().Substring(0, 3) + " ");
            String sqlSimpleDate2 = datetime2.ToString("dd-yyyy").Replace("-", " " + datetime2.ToString("MMM").ToUpperInvariant().Substring(0, 3) + " ");

            Dbg(
                new {
                    AnotherDateTimeFormat = new {
                        DateTime1 = datetime1,
                        DateTime2 = datetime2,
                        IsDate1AndDate2SameDate = datetime1.Date == datetime2.Date,
                        SqlSimpleDate1 = sqlSimpleDate1,
                        SqlSimpleDate2 = sqlSimpleDate2,
                        IsSqlSimpleDateSameDate = sqlSimpleDate1 == sqlSimpleDate2,
                        NowToString1 = $"{DateTime.Now:hhmmss}",
                        NowToString2 = $"{DateTime.Now:HHmmss}",
                        NowToString3 = $"{DateTime.Now:DateFormat}",
                        DiffUtcNowAndNow = diffUtcNowAndNow,
                        NowToString4 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        UtcNowToString = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CustomDateToString1 = new DateTime(2018, 9, 3, 10, 50, 31).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CustomDateToString2 = new DateTime(2018, 9, 3, 10, 50, 31).AddHours(diffUtcNowAndNow * -1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CustomDateToString3 = new DateTime(2018, 9, 3, 10, 50, 31).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CustomDateToString4 = new DateTime(2018, 9, 3, 10, 50, 31).AddHours(diffUtcNowAndNow).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        DateTimeInUtc = new DateTime(2018, 9, 3, 10, 50, 31)
                            .AddHours(DateTime.UtcNow.Subtract(DateTime.Now).Hours)
                            .ToString("yyyy-MM-ddTHH:mm:ssZ")
                    }
                }
            );

            Boolean t = true;
            Boolean f = false;

            Dbg(
                new {
                    DayOfWeekAndItsCustom = new {
                        DateTime.Now.Day,
                        DateTime.Now.DayOfWeek,
                        DayOfWeekNo = (Int32) DateTime.Now.DayOfWeek,
                        DateTime.Now.DayOfYear,
                        DateTime.Now.Date,
                        CustomDayOfWeekNo = (Int32) DayOfWeeeeeeek.Friday,
                        True = t ? 1 : 0,
                        False = f ? 1 : 0
                    }
                }
            );

            var dtStart = new DateTime(2018, 1, 7);
            var dtEnd = new DateTime(2018, 1, 13);
            var dtRangeList = new List<DateTime>();

            for (DateTime currentDt = dtStart; currentDt <= dtEnd; currentDt = currentDt.AddDays(1))
                dtRangeList.Add(currentDt);

            Dbg(
                new {
                    DateRange =
                        dtRangeList.Select(d => new {
                            DayOfWeek = Convert.ToInt32(d.DayOfWeek),
                            DayName = Convert.ToString(d.DayOfWeek),
                            Date = d.ToString("yyyy-MM-dd")
                        })
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Datetime leap  year

            Dbg(
                // Actual Date is 01.01.2014
                // Excel Bug
                new DateTime(1900, 1, 1)
                    .AddDays(41640)
                    .ToString("dd.MM.yyyy")
            );

            Dbg(
                // Actual Date is 28.02.2014
                // Excel Bug
                new DateTime(1900, 1, 1)
                    .AddDays(41698)
                    .ToString("dd.MM.yyyy")
            );

            // still incorrect
            Dbg(CalculateLeapYearCount(2018, 1900));

            Dbg(
                new DateTime(1900, 1, 1)
                    .AddDays(41640 - 2)
                    .ToString("dd.MM.yyyy")
            );

            Dbg(
                new DateTime(1900, 1, 1)
                    .AddDays(41698 - 2)
                    .ToString("dd.MM.yyyy")
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Ago time

            String format = "MM/dd/yyyy h:mm:ss tt";
            Dbg(
                new {
                    DateTime1 = DateTime.ParseExact("05/10/2012 8:25:07 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime2 = DateTime.ParseExact("05/10/2012 7:12:08 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime3 = DateTime.ParseExact("05/10/2012 5:55:40 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime4 = DateTime.ParseExact("05/10/2012 5:55:40 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime5 = DateTime.ParseExact("12/25/2011 8:25:07 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime6 = DateTime.ParseExact("12/22/2010 8:25:07 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),
                    DateTime7 = DateTime.ParseExact("12/22/2011 8:25:07 PM", format, CultureInfo.CurrentCulture).AsTimeAgo(),

                    DateTime8 = DateTime.ParseExact("05/10/2012 8:25:07 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime9 = DateTime.ParseExact("05/10/2012 7:12:08 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime10 = DateTime.ParseExact("05/10/2012 5:55:40 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime11 = DateTime.ParseExact("05/10/2012 5:55:40 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime12 = DateTime.ParseExact("12/25/2011 8:25:07 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime13 = DateTime.ParseExact("12/22/2010 8:25:07 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo(),
                    DateTime14 = DateTime.ParseExact("12/22/2011 8:25:07 PM", format, CultureInfo.CurrentCulture).AsAnotherTimeAgo()
                }
            );

            /** ──────────────────────────────────────────────────────────────────────────────── */

            // NOTE: Time with Z

            TimeZone tz = TimeZone.CurrentTimeZone;
            TimeSpan offset = tz.GetUtcOffset(DateTime.Now);
            Console.WriteLine(offset);

            Func<String, DateTime> ParseDateTime1 = input => DateTime.ParseExact(input, "yyyyMMdd\\THHmmss\\Z", CultureInfo.InvariantCulture, DateTimeStyles.None);
            Func<String, DateTime> ParseDateTime2 = input => DateTime.ParseExact(input, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None);

            Func<DateTime, String> StringifyDateTime = input => $"{input} => Kind: {input.Kind}, IsDaylightSavingTime: {input.IsDaylightSavingTime()}, UTC: {input.ToUniversalTime()}, Local: {input.ToLocalTime()}";

            var datetime = new {
                Date1 = ParseDateTime1("20191228T170845Z"),
                Date2 = ParseDateTime2("20191228T170845Z"),
                Date3 = ParseDateTime1("20200127T144936Z"),
                Date4 = ParseDateTime2("20200127T144936Z")
            };

            Dbg(
                new {
                    Original = datetime,
                    Stringified = new {
                        String1 = StringifyDateTime(datetime.Date1),
                        String2 = StringifyDateTime(datetime.Date2),
                        String3 = StringifyDateTime(datetime.Date3),
                        String4 = StringifyDateTime(datetime.Date4)
                    }
                }
            );
        }

        Int32 CalculateLeapYearCount(Int32 year, Int32 startingYear) {
            Int32 min = Math.Min(year, startingYear);
            Int32 max = Math.Max(year, startingYear);
            Int32 counter = 0;
            for (Int32 i = min; i < max; i++)
                if ((i % 4 == 0 && i % 100 != 0) || i % 400 == 0)
                    counter = counter + 1;

            return counter;
        }
    }

    public static class DateTimeUtil {
        public static String GetTodayDayName() => DateTime.Now.DayOfWeek.ToString();
        public static String GetTodayDate() => DateTime.Now.Date.ToString("dd MMMM yyyy");

        // http://www.dotnetperls.com/pretty-date
        public static String AsTimeAgo(this DateTime date) {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(date);

            // 2.
            // Get total number of days elapsed.
            Int32 dayDiff = (Int32) s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            Int32 secDiff = (Int32) s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0)
                return date.ToString();

            // 5.
            // Handle same-day times.
            if (dayDiff == 0) {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                    return "just now";

                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                    return "1 minute ago";

                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                    return $"{Math.Floor((Double) secDiff / 60)} minutes ago";

                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                    return "1 hour ago";

                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                    return $"{Math.Floor((Double) secDiff / 3600)} hours ago";
            }

            // 6.
            // Handle previous days.
            if (dayDiff == 1)
                return "yesterday";

            if (dayDiff < 7)
                return $"{dayDiff} days ago";

            if (dayDiff < 31)
                return $"{Math.Ceiling((Double) dayDiff / 7)} weeks ago";

            if (dayDiff < 365)
                return $"{Math.Floor((Double) dayDiff / 30)} months ago";

            if (dayDiff < 730)
                return "1 year ago";

            return $"{Math.Ceiling((Double) dayDiff / 365)} years ago";
        }

        // NOTE: find this source
        public static String AsAnotherTimeAgo(this DateTime datetime) {
            TimeSpan span = DateTime.Now.Subtract(datetime);
            Double TotalMinutes = span.TotalMinutes;
            String Suffix = " ago";

            if (TotalMinutes < 0.0) {
                TotalMinutes = Math.Abs(TotalMinutes);
                Suffix = " from now";
            }

            var agoList = new SortedList<Double, Func<String>> {
                { 0.75, () => "less than a minute" },
                { 1.5, () => "about a minute" },
                { 45, () => $"{Math.Round(TotalMinutes)} minutes" },
                { 90, () => "about an hour" },
                { 1440, () => $"about {Math.Round(Math.Abs(span.TotalHours))} hours" }, // 60 * 24
                { 2880, () => "a day" }, // 60 * 48
                { 43200, () => $"{Math.Floor(Math.Abs(span.TotalDays))} days" }, // 60 * 24 * 30
                { 86400, () => "about a month" }, // 60 * 24 * 60
                { 525600, () => $"{Math.Floor(Math.Abs(span.TotalDays / 30))} months" }, // 60 * 24 * 365 
                { 1051200, () => "about a year" }, // 60 * 24 * 365 * 2
                { Double.MaxValue, () => $"{Math.Floor(Math.Abs(span.TotalDays / 365))} years" }
            };

            return agoList.First(n => TotalMinutes < n.Key).Value.Invoke() + Suffix;
        }
    }

    public sealed class SamplePOCO {
        public String Type;
        public DateTime EffectiveDate;
        public DateTime EndDate;
    }

    public class SeededRandom {
        public Int32 GetRandomNumber() {
            Int32 seed = Guid.NewGuid().GetHashCode() % InternalHelper.Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(0, 256);
        }

        public Int32 GetRandomNumber(Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % InternalHelper.Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(lowerBound, upperBound);
        }

        public static Int32 GetRandomNumber2(Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % InternalHelper.Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(lowerBound, upperBound);
        }
    }

    public enum DayOfWeeeeeeek {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }
}
