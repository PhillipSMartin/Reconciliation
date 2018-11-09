using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface ITaxlotRecordCollection : IDisposable
    {
        TaxlotRecord NextRecord { get; }
    }
}
