using System;

namespace GPSInformation
{
    public static class Herramientas
    {
        public static string RelativeTime(DateTime yourDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? " hace un segundo" : ts.Seconds + " hace unos segundos";

            if (delta < 2 * MINUTE)
                return "hace un minuto";

            if (delta < 45 * MINUTE)
                return string.Format("hace {0} minutos", ts.Minutes);

            if (delta < 90 * MINUTE)
                return "hace una hora";

            if (delta < 24 * HOUR)
                return string.Format("hace {0} horas",ts.Hours) ;

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return string.Format("hace {0} dias", ts.Days);

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "hace un mes" : months + " hace unos meses";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? " hace un año" : years + " hace unos años";
            }
        }
    }
}
