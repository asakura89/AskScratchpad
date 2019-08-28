#l "Common.cake"

using System;

public static class DateTimeUtil {
    public static String GetTodayDayName() => DateTime.Now.DayOfWeek.ToString();
    public static String GetTodayDate() => DateTime.Now.Date.ToString("dd MMMM yyyy");
}

void Script() {
    Dbg(new {
        DateTimeUtil = new {
            TodayDayName = DateTimeUtil.GetTodayDayName(),
            TodayDate = DateTimeUtil.GetTodayDate()
        }
    });
}
