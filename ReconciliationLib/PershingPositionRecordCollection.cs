using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ReconciliationLib
{
    [CLSCompliant(false)]
    public class PershingPositionRecordCollection : ABRecordCollection<PershingPositionRecord>, IPositionRecordCollection
    {

        public PershingPositionRecordCollection(string fileName)
            : base(fileName,
                    new FixedLengthField<string>("TransactionCode", 0, 3),
                    new FixedLengthField<string>("RecordIndicator", 2, 1),
                    new FixedLengthField<int>("SequenceNumber", 3, 8),
                    new FixedLengthCustomDateField("HeaderDate", 46, 10, "MM/dd/yyyy"))
        {
        }

        #region IPositionRecordCollection Members

        public new PositionRecord NextRecord
        {
            get { return base.NextRecord; }
        }

        #endregion
    }
}


