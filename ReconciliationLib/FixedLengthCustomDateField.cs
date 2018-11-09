using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class FixedLengthCustomDateField : FixedLengthField<DateTime>
    {
            private string m_format;

            public FixedLengthCustomDateField(string fieldName, int fieldStart, int fieldLength, string format)
                : base(fieldName, fieldStart, fieldLength)
            {
                m_format = format;
            }
            public override DateTime ExtractValueFromRecord(string record)
            {
                string field = GetField(record);
                try
                {
                    FieldValue = DateTime.ParseExact(field, m_format, System.Globalization.CultureInfo.CurrentCulture);
                    SetIsNull(false);
                }
                catch (Exception)
                {
                    // if datetime is specified as '0' or is an empty string, use 1/1/1900 (using the default DateTime will cause a sql error on insert)
                    if (field.Trim(new char[] { '0' }).Length == 0)
                        FieldValue = new DateTime(1900,1,1);
                    else
                    {
                        throw new ReconciliationConversionException(String.Format("Cannot parse field {0}. {1} is not a valid {2}",
                            FieldName, field, FieldType));
                    }
                }
                return FieldValue;
            }
    }
}
