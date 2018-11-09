using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public interface IStockTrade : IUnderlying, ITrade
    {
        short TraderId { get; }     // no nulls - default should be -1

        bool TradeArchiveFlag { get; }   // no nulls - default should be false
        bool ShortFlag { get; }     // no nulls - default should be false

        long? OptionTradeId { get; }

        double FullVolume { get; set; }
   }
}
