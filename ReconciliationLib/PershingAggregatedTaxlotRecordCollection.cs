using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    class PershingAggregatedTaxlotRecordCollection : ABRecordCollection<PershingAggregatedTaxlotRecord>, ITaxlotRecordCollection
    {

        public PershingAggregatedTaxlotRecordCollection(string fileName)
            : base(fileName,
                    new FixedLengthField<string>("TransactionCode", 0, 3),
                    new FixedLengthField<string>("RecordIndicator", 2, 1),
                    new FixedLengthField<int>("SequenceNumber", 3, 8),
                    new FixedLengthCustomDateField("HeaderDate", 46, 10, "MM/dd/yyyy"))
        {
        }

        #region ITaxlotRecordCollection Members

        public new TaxlotRecord NextRecord
        {
            get { return base.NextRecord; }
        }

        #endregion
    }
}
