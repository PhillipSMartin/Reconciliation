using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ReconciliationLib
{
    public class FixedLengthSignedDoubleField : FixedLengthDoubleField
    {
        private int m_fieldSignStart;

        public FixedLengthSignedDoubleField(string fieldName, int fieldStart, int fieldLength, int fieldPrecision, int fieldSignStart)
            : base(fieldName, fieldStart, fieldLength, fieldPrecision)
        {
            if (fieldSignStart < 0)
                throw new ReconciliationImportException("Field sign startmust be non-negative");

            m_fieldSignStart = fieldSignStart;
        }
        public override double ExtractValueFromRecord(string record)
        {
            if (record == null)
                throw new ArgumentNullException("record");

            if (record.Length <= m_fieldSignStart)
                throw new ReconciliationImportException(String.Format("Record of length {0} is too short to hold sign of field {1} in position {2}",
                    record.Length, FieldName, m_fieldSignStart));
            return FieldValue = base.ExtractValueFromRecord(record) * ((record[m_fieldSignStart] == '-') ? -1 : 1);
        }
   }
}
