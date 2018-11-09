using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ReconciliationLib
{
    public interface IConfirmationTableAdapter : IDisposable
    {
        int InsertRecord(ConfirmationRecord positionRecord);
        int DeleteRecords(DateTime? importDate, string acctGroupName);
        void SetAllCommandTimeouts(int timeOut);
        int Update(DataRow[] rows);
    }
}
