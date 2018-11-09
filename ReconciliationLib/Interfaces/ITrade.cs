using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public interface ITrade 
    {
        int? TradeId { get; }
        string TradeType { get; }
        int TradeVolume { get; }
        decimal TradePrice { get; }
        DateTime TradeDate { get; }

        string SubAcctName { get; }
        string TraderName { get; }

        string BrokerName { get; }
        string ExchangeName { get; }
        string TradeMedium { get; }
        string TradeReason { get; }
        string TradeNote { get; }

        double FractionalRemainder { get; } // extends TradeVolume in case we have fractional units
        double UnitCost { get; }        // price per unit before commissions (same as TradePrice, but can be more decimal places)
        double TotalCost { get; }       // UnitCost * TradeVolume, adjusted for all commissions and fees
        string TaxLotId { get; }

        int? ConsolidationPackageId { get; }
        int NumberOfTrades { get; }

        double Commission { get; }
        double SECFee { get; }
        double ORFFee { get; }
    }
}
