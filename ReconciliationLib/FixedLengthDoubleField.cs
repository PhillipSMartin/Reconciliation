using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class FixedLengthDoubleField : FixedLengthField<double>
    {
        private int m_fieldPrecision;

        public FixedLengthDoubleField(string fieldName, int fieldStart, int fieldLength, int fieldPrecision)
            : base(fieldName, fieldStart, fieldLength)
        {
            if (fieldPrecision < 0)
                throw new ReconciliationImportException("Field precision must be non-negative");

            m_fieldPrecision = fieldPrecision;
        }
        public override double ExtractValueFromRecord(string record)
        {
            return FieldValue = base.ExtractValueFromRecord(record) / Math.Pow(10, m_fieldPrecision);
        }
       protected int FieldPrecision { get { return m_fieldPrecision; } }
    }
}
