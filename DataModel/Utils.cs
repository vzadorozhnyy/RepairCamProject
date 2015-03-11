using System;
using System.Globalization;

namespace DataModel {
    public static class Utils {
        public const string Zero = "0";
        public const string One = "1";
        private const string EnUsCultureName = "en-US";
        private static CultureInfo _cultureInfo;

        public static CultureInfo CultureInfo {
            get {
                if (_cultureInfo == null) {
                    _cultureInfo = new CultureInfo(EnUsCultureName);
                    InitCultureInfo(_cultureInfo);
                }
                return _cultureInfo;
            }
        }

        public static string SDAsEngString(this Guid value) {
            return Convert.ToString(value, CultureInfo);
        }

        public static string SDAsEngString(this DateTime date) {
            return date == DateTime.MinValue ? string.Empty : Convert.ToString(date, CultureInfo);
        }

        public static string SDAsEngString(this double value, int precision = -1) {
            if (precision < 0 || precision > 6 || double.IsNaN(value))
                return Convert.ToString(value, CultureInfo);
            return Convert.ToString(Math.Round(value, precision), CultureInfo);
        }

        public static string SDAsEngString(this long value) {
            return Convert.ToString(value, CultureInfo);
        }

        public static string SDAsEngString(this int value) {
            return Convert.ToString(value, CultureInfo);
        }

        public static string SDAsEngString(this bool value) {
            return value ? One : Zero;
        }

        public static string SDAsSqlString(this string value) {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value.Replace("'", "''");
        }

        private static void InitCultureInfo(CultureInfo info) {
            info.NumberFormat.NegativeSign = "-";
            info.NumberFormat.NumberDecimalDigits = 2;
            info.NumberFormat.NumberDecimalSeparator = ".";
            info.NumberFormat.NumberGroupSeparator = ",";
            info.DateTimeFormat.AMDesignator = "AM";
            info.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;
            info.DateTimeFormat.LongDatePattern = "dddd, MMMM dd, yyyy";
            info.DateTimeFormat.LongTimePattern = "h:mm:ss tt";
            info.DateTimeFormat.PMDesignator = "PM";
            info.DateTimeFormat.ShortDatePattern = "M/d/yyyy";
            info.DateTimeFormat.ShortTimePattern = "h:mm tt";
            info.DateTimeFormat.DateSeparator = "/";
            info.DateTimeFormat.TimeSeparator = ":";
        }

        public static DateTime SDAsDate(this object obj) {
            return obj.SDAsDate(DateTime.MinValue);
        }

        public static DateTime SDAsDate(this object obj, DateTime defaultValue) {
            if (obj == DBNull.Value || ReferenceEquals(obj, null))
                return defaultValue;
            if (obj is string) {
                string lV = Convert.ToString(obj);
                if (string.IsNullOrEmpty(lV) || string.IsNullOrEmpty(lV.Trim()))
                    return defaultValue;
                DateTime lResult;
                if (DateTime.TryParse(lV, CultureInfo, DateTimeStyles.None, out lResult))
                    return lResult;
                //return Convert.ToDateTime(lV, TextUtils.CultureInfo);
            }
            try {
                DateTime lDate = Convert.ToDateTime(obj);
                return new DateTime(lDate.Year, lDate.Month, lDate.Day, lDate.Hour, lDate.Minute, lDate.Second);
            } catch (Exception) {
                return defaultValue;
            }
        }

        public static long SDAsInt(this object obj, int defaultValue = 0) {
            if (ReferenceEquals(obj, null) || obj == DBNull.Value)
                return defaultValue;
            string lV = Convert.ToString(obj);
            if (string.IsNullOrEmpty(lV))
                return defaultValue;
            long lValue;
            if (long.TryParse(lV, out lValue))
                return lValue;
            try {
                return Convert.ToInt64(obj, CultureInfo);
            } catch (Exception) {
                return defaultValue;
            }
        }

        public static string SDAsStr(this object obj, string defaultValue = "") {
            if (obj == DBNull.Value || ReferenceEquals(obj, null))
                return defaultValue;
            return Convert.ToString(obj);
        }

        public static Guid SDAsGuid(this object obj) {
            if (obj == DBNull.Value || ReferenceEquals(obj, null))
                return Guid.Empty;
            try {
                string lS = Convert.ToString(obj);
                if (string.IsNullOrEmpty(lS))
                    return Guid.Empty;
                lS = lS.Trim();
                if (string.IsNullOrEmpty(lS))
                    return Guid.Empty;
                return new Guid(lS);
            } catch (Exception) {
                return Guid.Empty;
            }
        }

        public static bool SDAsBool(this object obj) {
            if (obj == DBNull.Value || ReferenceEquals(obj, null))
                return false;
            string lV = Convert.ToString(obj);

            if (string.IsNullOrEmpty(lV))
                return false;
            if (lV.StartsWith(One))
                return true;
            if (lV.StartsWith(Zero))
                return false;
            return Convert.ToBoolean(lV, CultureInfo);
        }
    }
}