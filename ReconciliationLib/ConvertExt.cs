using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class ConvertExt 
    {
        protected ConvertExt() { }

        public static decimal? ToDecimalOrNull(object value)
        {
            return Convert.IsDBNull(value) ? (decimal?)null : Convert.ToDecimal(value);
        }
        public static bool? ToBooleanOrNull(object value)
        {
            return Convert.IsDBNull(value) ? (bool?)null : Convert.ToBoolean(value);
        }
        public static string ToStringOrNull(object value)
        {
            return ToStringOrNull(Convert.ToString(value));
        }
        public static string ToStringOrNull(string value)
        {
            return String.IsNullOrEmpty(value) ? null : value;
        }

        // convert to a date time guaranteed to be between PreviousDate and ImportDate
        //  and with time of 5 pm
        public static DateTime ToValidDateTime(object value)
        {
            return ToValidDateTime(Convert.IsDBNull(value) ? DateTime.Today : Convert.ToDateTime(value));
        }
        public static DateTime ToValidDateTime(DateTime tempDate)
        {
            // make sure it is within bounds
            if ((tempDate.Ticks < Utilities.PreviousDate.Value.Ticks) ||
                (tempDate.Ticks >= Utilities.ImportDate.Value.Ticks))
            {
                tempDate = Utilities.PreviousDate.Value;
            }

            // make time 5 pm
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 17, 0, 0);
        }

        public static DateTime FromOADate(int oaDate)
        {
            if (oaDate > 59) oaDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(oaDate);
        }

    }
}
