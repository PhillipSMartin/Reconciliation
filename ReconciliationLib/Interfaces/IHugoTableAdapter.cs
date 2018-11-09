using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace ReconciliationLib
{
    public interface IHugoTableAdapter : IDisposable
    {
        int DeleteRecords(DateTime? ImportDate, string AcctGroupName);
        void SetAllCommandTimeouts(int timeOut);
        void LogCommand(string commnad);
        DataTable GetDataByImportDateAcctGroupName(Nullable<DateTime> importDate, string acctGroupName);
        void SetConnection(SqlConnection connection);
    }
}
