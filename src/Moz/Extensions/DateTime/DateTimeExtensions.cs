
// ReSharper disable once CheckNamespace
namespace System
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime BeginOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToJavaScriptTicks(this DateTime dateTime)
        {
            DateTimeOffset utcDateTime = dateTime.ToUniversalTime();
            long javaScriptTicks = (utcDateTime.Ticks - BeginOfEpoch.Ticks) / 10000;
            return javaScriptTicks;
        }

        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            DateTime dtFrom = date;
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
            return dtFrom;
        }

        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            DateTime dtTo = date;
            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;
        }

        public static DateTime ToEndOfTheDay(this DateTime dt)
        {
            if (dt != null)
                return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return dt;
        }

        public static DateTime? ToEndOfTheDay(this DateTime? dt)
        {
            return (dt.HasValue ? dt.Value.ToEndOfTheDay() : dt);
        }

        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - BeginOfEpoch).TotalSeconds);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            return BeginOfEpoch.AddSeconds(unixTime);
        }

        public static string GetFriendlyTimespan(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            if (Math.Floor(ts.TotalDays) > 365)
            {
                return (int)(Math.Floor(ts.TotalDays) / 365) + "年前";
            }
            else if (Math.Floor(ts.TotalDays) >= 30)
            {
                return (int)(Math.Floor(ts.TotalDays) / 30) + "月前";
            }
            else if (Math.Floor(ts.TotalDays) >= 1)
            {
                return (int)ts.TotalDays + "天前";
            }
            else if (Math.Floor(ts.TotalHours) >= 1)
            {
                return (int)ts.TotalHours + "小时前";
            }
            else if (Math.Floor(ts.TotalMinutes) >= 1)
            {
                return (int)ts.TotalMinutes + "分钟前";
            }
            else
            {
                return "刚刚";
            }
        } 
    }
}