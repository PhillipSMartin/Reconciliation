using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface IABRecord
    {
        void ReadRecordA(string record);
        void ReadRecordB(string record);
        int SequenceNumber { get; }
    }
}
