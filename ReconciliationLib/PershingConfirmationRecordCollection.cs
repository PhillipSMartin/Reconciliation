using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ReconciliationLib
{
    [CLSCompliant(false)]
    public class PershingConfirmationRecordCollection : ABRecordCollection<PershingConfirmationRecord>, IConfirmationRecordCollection
    {
        public PershingConfirmationRecordCollection(string fileName)
            : base(fileName,
                    new FixedLengthField<string>("TransactionCode", 0, 3),
                    new FixedLengthField<string>("RecordIndicator", 2, 1),
                    new FixedLengthField<int>("SequenceNumber", 3, 8),
                    new FixedLengthCustomDateField("HeaderDate", 46, 10, "MM/dd/yyyy"))
        {
        }


        #region IConfirmationRecordCollection Members

        public new ConfirmationRecord NextRecord
        {
            get { return base.NextRecord; }
        }

        #endregion
    }
}   
