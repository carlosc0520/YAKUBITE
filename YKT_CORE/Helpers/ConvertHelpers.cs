using System.Globalization;
using System.Runtime.InteropServices;

namespace YKT.CORE.Helpers
{
    public class ConvertHelpers
    {
        #region DATEPICKER CONVERTERS
        public static DateTime? DatepickerToDatetime(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                string formato1 = "dd/MM/yyyy h:mm:ss tt";
                string formato2 = "dd/MM/yyyy h:mm:ss";
                string formato3 = "dd/MM/yyyy HH:mm:ss";
                string formato8 = "dd/MM/yyyy";
                string formato4 = "yyyyMMdd";

                string[] Formatos = { formato1, formato2, formato3, formato4, formato8, };

                return DateTime.ParseExact(date, Formatos, new CultureInfo("es-PE"), DateTimeStyles.None);

            }
            return null;
        }

        public static DateTime? DatepickerToDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                string formato1 = "dd/MM/yyyy h:mm:ss tt";
                string formato2 = "dd/MM/yyyy h:mm:ss";
                string formato3 = "dd/MM/yyyy HH:mm:ss";
                string formato8 = "dd/MM/yyyy";
                string formato4 = "yyyyMMdd";

                string[] Formatos = { formato1, formato2, formato3, formato4, formato8 };

                return DateTime.ParseExact(date, Formatos, new CultureInfo("es-PE"), DateTimeStyles.None);

            }
            return null;
        }

        public static object DatepickerFormatDate(string date) //MMW
        {
            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    return Convert.ToDateTime(date).ToShortDateString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            return null;
        }

        public static DateTime DatepickerToUtcDateTime(string date)
        {
            var dt = DateTime.ParseExact(date, ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.CurrentCulture).ToUniversalTime();
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.TIMEZONEINFO.LINUX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.OSX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.WINDOWS_TIMEZONE_ID));
        }

        #endregion

        #region TIMEPICKER CONVERTERS

        public static DateTime TimepickerToDateTime(string time)
        {
            var dt = DateTime.ParseExact(time, ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.CurrentCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.TIMEZONEINFO.LINUX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.OSX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.WINDOWS_TIMEZONE_ID), ConvertHelpers.FindOperatingSystemTimeZoneById(ConstantHelpers.TIMEZONEINFO.LINUX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.OSX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.WINDOWS_TIMEZONE_ID));
        }

        public static DateTime TimepickerToUtcDateTime(string time)
        {
            var dt = DateTime.ParseExact(time, ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.CurrentCulture);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, FindOperatingSystemTimeZoneById(ConstantHelpers.TIMEZONEINFO.LINUX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.OSX_TIMEZONE_ID, ConstantHelpers.TIMEZONEINFO.WINDOWS_TIMEZONE_ID));
        }

        public static TimeSpan TimepickerToTimeSpan(string time)
        {
            return TimepickerToDateTime(time).TimeOfDay;
        }
        public static TimeSpan TimepickerToUtcTimeSpan(string time)
        {
            return TimepickerToUtcDateTime(time).TimeOfDay;
        }

        #endregion

        #region TIMEZONEINFO CONVERTERS

        public static TimeZoneInfo FindOperatingSystemTimeZoneById(string linuxId, string osxId, string windowsId)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(windowsId);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(osxId);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(linuxId);
            }

            return null;
        }

        #endregion
    }
}
