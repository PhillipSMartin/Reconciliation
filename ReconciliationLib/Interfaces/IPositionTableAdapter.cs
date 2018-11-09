using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ReconciliationLib
{
    public interface IPositionTableAdapter : IDisposable
    {
        int InsertRecord(PositionRecord positionRecord);
        int DeleteRecords(DateTime? ImportDate, string AcctGroupName);
        void SetAllCommandTimeouts(int timeOut);
        int Update(DataRow[] rows);
    }
}
