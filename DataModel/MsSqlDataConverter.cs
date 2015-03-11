using System;

namespace DataModel {
    internal class MsSqlDataConverter {
        private const string ValueNULL = "NULL";

        #region ISqlDataConverter Members

        public string ConvertString(string value) {
            if (string.IsNullOrEmpty(value))
                return GetNullValue();

            return string.Format("N'{0}'", value.SDAsSqlString());
        }

        public string ConvertDouble(double value) {
            if (double.IsNaN(value))
                return GetNullValue();

            return value.SDAsEngString();
        }

        public string ConvertInt(int value) {
            return value.SDAsEngString();
        }

        public string ConvertBool(bool value) {
            return value.SDAsEngString();
        }

        public string ConvertDateTime(DateTime value) {
            if (DateTime.MinValue.Equals(value))
                return GetNullValue();
            if (DateTime.MaxValue.Equals(value))
                value = DateTime.Now.ToUniversalTime();
            return string.Format("convert(datetime, {0}, 101)", DateToStr(value));
        }

        public string ConvertGuid(Guid value) {
            if (Guid.Empty.Equals(value))
                return GetNullValue();

            return WrapWithQuotes(value.ToString("D"));
        }

        public static string WrapWithQuotes<T>(T value) {
            const string format = "'{0}'";
            return string.Format(format, value);
        }

        public string ConvertDateTimeAlt(DateTime value) {
            if (DateTime.MinValue.Equals(value))
                return GetNullValue();
            if (DateTime.MaxValue.Equals(value))
                value = DateTime.Now.ToUniversalTime();
            return DateToStr(value);
        }

        public string GetNullValue() {
            return ValueNULL;
        }

        private string DateToStr(DateTime value) {
            return string.Format("'{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}'", value.Month, value.Day, value.Year, value.Hour, value.Minute, value.Second);
        }

        #endregion
    }
}