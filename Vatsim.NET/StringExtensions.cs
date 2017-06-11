using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Vatsim.NET
{
    internal static class StringExtensions
    {
        public static int ToInt(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0;
            }
            return Convert.ToInt32(s);
        }

        public static decimal ToDecimal(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0M;
            }
            return Convert.ToDecimal(s);
        }

        public static DateTime? ToDateTime(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            return DateTime.ParseExact(s, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
        }
    }
}
