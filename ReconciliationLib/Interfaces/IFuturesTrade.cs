using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public interface IFuturesTrade : IUnderlying, ITrade
    {
        short TraderId { get; }     // no nulls - default should be -1

        string BillingTypeDescr { get; }
        decimal SpecialRate { get; } // no nulls - default should be 0

        bool TradeArchiveFlag { get; }   // no nulls - default should be false
        bool ShortFlag { get; }     // no nulls - default should be false

        long? OptionTradeId { get; }

        double FullVolume { get; set; }

        double NFAFee { get; }
        double ClearingFee { get; }
        double ExchangeFee { get; }
    }
}
