using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public delegate int ImportTradesDelegate(string xml, ref global::System.Nullable<int> numberTradesInserted, ref global::System.Nullable<int> numberTradesRejected, global::System.Nullable<global::System.DateTime> tradeDate, global::System.Nullable<bool> debug);
    public delegate int ImportConfirmationsDelegate(string xml, global::System.Nullable<global::System.DateTime> importDate, ref global::System.Nullable<int> numberTradesInserted, global::System.Nullable<bool> restart, global::System.Nullable<bool> debug);
    public delegate int ImportPositionsDelegate(string xml, global::System.Nullable<global::System.DateTime> importDate, ref global::System.Nullable<int> numberTradesInserted, global::System.Nullable<bool> restart, global::System.Nullable<bool> debug);
    public delegate int ImportDividendsDelegate(string xml, global::System.Nullable<global::System.DateTime> importDate, ref global::System.Nullable<int> numberTradesInserted, global::System.Nullable<bool> restart, global::System.Nullable<bool> debug);
    public delegate int ImportTaxlotsDelegate(string xml, global::System.Nullable<global::System.DateTime> importDate, global::System.Nullable<bool> debug);
    public delegate int ImportBookkeepingDelegate(string xml, global::System.Nullable<global::System.DateTime> importDate, ref global::System.Nullable<int> numberTradesInserted, global::System.Nullable<bool> restart, global::System.Nullable<bool> debug);
}
