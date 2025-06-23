using System.Globalization;

namespace SaltStackers.Common.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertFromUtc(this DateTime dateTime, string culture = "Pacific Standard Time")
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(culture);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }

        public static DateTime ConvertFromUtc(this DateTime dateTime)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }

        public static string ConvertFromUtcString(this DateTime dateTime, string format = "")
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "yyyy/MM/dd HH:mm:ss";
            }

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone).ToString(format);
        }

        public static int DaysUntil(this DayOfWeek dayOfWeek, DateTime calculateDate)
        {
            return ((int)dayOfWeek - (int)calculateDate.DayOfWeek + 7) % 7;
        }

        public static bool InBetweenDaysInclusive(this DateTime date, DayOfWeek start, DayOfWeek end)
        {
            var currentDate = date.DayOfWeek;
            return start <= end
                ? start <= currentDate && currentDate <= end
                : start <= currentDate || currentDate <= end;
        }

        public static string GetShortDayName(this DayOfWeek day)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            string[] names = culture.DateTimeFormat.AbbreviatedDayNames;
            return names[(int)day];
        }

        public static DateTime AddDayOfWeekDate(this DayOfWeek day, int hours, DateTime indexDate)
        {
            var date = indexDate;
            for (int i = 0; i < 7; i++)
            {
                if (date.DayOfWeek == day)
                {
                    return date.AddHours(hours);
                }
                date = date.AddDays(1);
            }
            return date;
        }

        public static string AddDayOfWeek(this DayOfWeek day, int hours)
        {
            var today = DateTime.Now.Date;
            for (int i = 0; i < 7; i++)
            {
                if (today.DayOfWeek == day)
                {
                    return today.AddHours(hours).ToString("ddd. hh:mm tt");
                }
                today = today.AddDays(1);
            }
            return today.DayOfWeek.ToString();
        }
    }
}
