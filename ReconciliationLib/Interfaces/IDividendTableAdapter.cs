using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface IDividendTableAdapter : IDisposable
    {
        int InsertRecord(DividendRecord dividendRecord);
        int DeleteRecords(DateTime? ImportDate, string AcctGroupName);
        void SetAllCommandTimeouts(int timeOut);
    }
}
