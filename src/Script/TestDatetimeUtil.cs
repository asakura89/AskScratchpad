using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        }
    }

    public static class DateTimeUtil {
        public static String GetTodayDayName() => DateTime.Now.DayOfWeek.ToString();
        public static String GetTodayDate() => DateTime.Now.Date.ToString("dd MMMM yyyy");
    }

    public sealed class SamplePOCO {
        public String Type;
        public DateTime EffectiveDate;
        public DateTime EndDate;
    }

    public class SeededRandom {
        public Int32 GetRandomNumber() {
            Int32 seed = Guid.NewGuid().GetHashCode() % 50001;
            var rnd = new Random(seed);
            return rnd.Next(0, 256);
        }

        public Int32 GetRandomNumber(Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % 50001;
            var rnd = new Random(seed);
            return rnd.Next(lowerBound, upperBound);
        }

        public static Int32 GetRandomNumber2(Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % 50001;
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
