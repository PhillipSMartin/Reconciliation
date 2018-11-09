using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public abstract class TaxlotRecord
    {
        public virtual bool IsValid { get { return true; } }
    }
}
