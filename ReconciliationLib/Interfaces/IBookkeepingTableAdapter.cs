using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface IBookkeepingTableAdapter : IDisposable
    {
        int InsertRecord(BookkeepingRecord BookkeepingRecord);
        int DeleteRecords(DateTime? ImportDate, string AcctGroupName);
        void SetAllCommandTimeouts(int timeOut);
    }
}
