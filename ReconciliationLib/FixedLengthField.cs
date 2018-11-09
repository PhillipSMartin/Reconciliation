using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ReconciliationLib
{
    public class FixedLengthField<T> 
    {
        private string m_fieldName;
        private T m_fieldValue;
        private bool m_isNull = true;

        private int m_fieldStart;
        private int m_fieldLength;

        private static MethodInfo s_parseMethod = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });

        public FixedLengthField(string fieldName, int fieldStart, int fieldLength)
        {
            if (String.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException("fieldName");
            if ((fieldStart < 0) || (fieldLength < 0))
                throw new ReconciliationImportException("Field start and length must be non-negative");
            if ((s_parseMethod == null) && (FieldType != "System.String"))
                throw new ReconciliationImportException(String.Format("Field type {0} is not valid, since it has no Parse method", FieldType));


            m_fieldName = fieldName;
            m_fieldStart = fieldStart;
            m_fieldLength = fieldLength;
        }
        public virtual T ExtractValueFromRecord(string record)
        {
            string field = GetField(record);
            try
            {
                m_fieldValue = (T)Parse(field);
                m_isNull = false;
            }
            catch (Exception)
            {
                throw new ReconciliationConversionException(String.Format("Cannot parse field {0}. {1} is not a valid {2}",
                    FieldName, field, FieldType));
            }
            return m_fieldValue;
        }

        protected string GetField(string record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            if (record.Length < FieldStart + FieldLength)
                throw new ReconciliationImportException(String.Format("Record of length {0} is too short to hold field {1} in positions {2} thru {3}",
                    record.Length, FieldName, FieldStart, FieldStart + FieldLength));

            return record.Substring(FieldStart, FieldLength);
        }

        public string FieldName { get { return m_fieldName; }  }
        public T FieldValue { get { return m_fieldValue; } set { m_fieldValue = value; } }
        public string FieldType { get { return typeof(T).ToString(); } }
        public bool IsNull { get { return m_isNull; } }

        protected static object Parse(string field)
        {
            if (string.IsNullOrEmpty(field))
                return null;
            if (s_parseMethod == null)
                return field;

            return s_parseMethod.Invoke(null, new object[] { field });
        }

        protected int FieldStart { get { return m_fieldStart; } }
        protected int FieldLength { get { return m_fieldLength; } }
        protected void SetIsNull(bool value) { m_isNull = value; }
    }
}
