using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ReconciliationLib
{
    public sealed class ReconciliationConvert
    {
        private ReconciliationConvert() { }

        public static decimal ToDecimal(string parameterValue, string parameterName)
        {
            try
            {
                if (parameterValue == null)
                    return 0;
                if (parameterValue.Length <= 0)
                    return 0;

                return Convert.ToDecimal(parameterValue, CultureInfo.CurrentCulture);
            }
            catch (OverflowException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to decimal",
                    parameterName, parameterValue));
            }
            catch (FormatException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to decimal",
                    parameterName, parameterValue));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static double ToDouble(string parameterValue, string parameterName)
        {
            try
            {
                if (parameterValue == null)
                    return 0f;
                if (parameterValue.Length <= 0)
                    return 0f;

                return Convert.ToDouble(parameterValue, CultureInfo.CurrentCulture);
            }
            catch (OverflowException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to double",
                    parameterName, parameterValue));
            }
            catch (FormatException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to double",
                    parameterName, parameterValue));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int ToInt32(string parameterValue, string parameterName)
        {
            try
            {
                if (parameterValue == null)
                    return 0;
                if (parameterValue.Length <= 0)
                    return 0;

                return Convert.ToInt32(parameterValue, CultureInfo.CurrentCulture);
            }
            catch (OverflowException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to int32",
                    parameterName, parameterValue));
            }
            catch (FormatException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to int32",
                    parameterName, parameterValue));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static char ToChar(string parameterValue, string parameterName)
        {
            if (parameterValue == null)
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert null value for {0}, to char",
                    parameterValue));
            if (parameterValue.Length != 1)
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to char",
                     parameterName, parameterValue));

            return parameterValue[0];
        }

        public static T ToEnum<T>(string parameterValue, string parameterName, T defaultValue)
        {
            T returnValue = (T)Enum.ToObject(typeof(T), ToChar(parameterValue, parameterName));
            List<T> x = new List<T>((T[])Enum.GetValues(typeof(T)));
            if (x.Contains(returnValue))
            {
                return returnValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(string parameterValue, string parameterName)
        {
            try
            {
                if (parameterValue == null)
                    return DateTime.Today;
                if (parameterValue.Length <= 0)
                    return DateTime.Today;

                DateTime result;
                if (DateTime.TryParse(parameterValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;
                else
                    // if unable to parse, it is possible this is an OADate
                    return ConvertExt.FromOADate(Convert.ToInt32(parameterValue));
            }
            catch (ArgumentException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to DateTime",
                    parameterName, parameterValue));
            }
            catch (NotSupportedException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to DateTime",
                    parameterName, parameterValue));
            }
            catch (FormatException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to DateTime",
                    parameterName, parameterValue));
            }
            catch (OverflowException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, to DateTime",
                    parameterName, parameterValue));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DateTime ToDateTime(string parameterValue, string parameterName, string format)
        {
            try
            {
                if (parameterValue == null)
                    return DateTime.Today;
                if (parameterValue.Length <= 0)
                    return DateTime.Today;

                return DateTime.ParseExact(parameterValue, format, CultureInfo.CurrentCulture);
            }
            catch (ArgumentNullException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, format = {2} to DateTime",
                    parameterName, parameterValue, format));
            }
            catch (FormatException)
            {
                throw new ReconciliationConversionException(String.Format(CultureInfo.CurrentCulture, "Unable to convert {0}, value = {1}, format = {2} to DateTime",
                    parameterName, parameterValue, format));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ToNullableString<T>(Nullable<T> nullableObject) where T : struct
        {
            if (nullableObject.HasValue)
            {
                return nullableObject.ToString();
            }
            else
            {
                return "<NULL>";
            }
        }
    }
}
