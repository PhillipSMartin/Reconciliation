using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public enum RecordType { None = 0, Header = '0', Position = '1', Confirmation = '2', Bookkeeping = '3', Trailer = '9' };
}
