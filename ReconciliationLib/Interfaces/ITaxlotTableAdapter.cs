using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface ITaxlotTableAdapter : IDisposable 
    {
        int InsertRecord(TaxlotRecord positionRecord);
        int DeleteRecords(DateTime? ImportDate, string AcctGroupName);
        void SetAllCommandTimeouts(int timeOut);
    }
}
