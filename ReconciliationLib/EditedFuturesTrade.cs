using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class EditedFuturesTrade
    {
        private int tradeid;

        // if these value are NULL, they remain unchanged from the original trade
        private string symbol;

        private string tradetype;
        private int? volume;
        private decimal? price;
        private string subacctname;
        private DateTime? tradedate;
        private string tradenote;

        private string trader;
        private string broker;
        private string exchange;
        private string medium;
        private string reason;

        // if these parms are NULL, they must be recalculated
        private double? commission;
        private double? nfaFee;
        private double? exchangeFee;
        private double? clearingFee;
        private double? totalcost;

        public EditedFuturesTrade(HugoDataSet.HugoFuturesTradesDataTable table, HugoDataSet.HugoFuturesTradesRow row)
        {
            tradeid = row.TradeId;

            if (IsColumnChanged(row, table.FuturesSymbolColumn))
                symbol = row.FuturesSymbol;

            if (IsColumnChanged(row, table.TradeTypeNameColumn))
                tradetype = row.TradeTypeName;
            if (tradetype == "Sell short") tradetype = "Sell";

            if (IsColumnChanged(row, table.TradeVolumeColumn))
                volume = row.TradeVolume;
            if (IsColumnChanged(row, table.TradePriceColumn))
                price = row.TradePrice;
            if (IsColumnChanged(row, table.SubAcctNameColumn))
                subacctname = row.SubAcctName;
            if (IsColumnChanged(row, table.TradeDateTimeColumn))
                tradedate = row.TradeDateTime;
            if (IsColumnChanged(row, table.TradeNoteColumn))
                tradenote = row.TradeNote;

            if (IsColumnChanged(row, table.TraderNameColumn))
                trader = row.TraderName;
            if (IsColumnChanged(row, table.BrokerNameColumn))
                broker = row.BrokerName;
            if (IsColumnChanged(row, table.ExchangeNameColumn))
                exchange = row.ExchangeName;
            if (IsColumnChanged(row, table.TradeMediumNameColumn))
                medium = row.TradeMediumName;
            if (IsColumnChanged(row, table.TradeReasonDescrColumn))
                reason = row.TradeReasonDescr;
            if (IsColumnChanged(row, table.TotalCostColumn))
                totalcost = row.TotalCost;
            if (IsColumnChanged(row, table.CommissionColumn))
                commission = row.Commission;
            if (IsColumnChanged(row, table.NFAFeeColumn))
                nfaFee = row.NFAFee;
            if (IsColumnChanged(row, table.ExchangeFeeColumn))
                exchangeFee = row.ExchangeFee;
            if (IsColumnChanged(row, table.ClearingFeeColumn))
                clearingFee = row.ClearingFee;
        }

        private bool IsColumnChanged(DataRow row, DataColumn column)
        {
            return row[column, System.Data.DataRowVersion.Current].ToString() != row[column, System.Data.DataRowVersion.Original].ToString();
        }

        public int TradeId { get { return tradeid; } }
        public string Symbol { get { return symbol; } }

        public string TradeType { get { return tradetype; } }
        public int? Volume { get { return volume; } }
        public decimal? Price { get { return price; } }
        public string SubAcctName { get { return subacctname; } }
        public DateTime? TradeDate { get { return tradedate; } }
        public string TradeNote { get { return tradenote; } }

        public string Trader { get { return trader; } }
        public string Broker { get { return broker; } }
        public string Exchange { get { return exchange; } }
        public string Medium { get { return medium; } }
        public string Reason { get { return reason; } }

        public double? Commission { get { return commission; } }
        public double? NFAFee { get { return nfaFee; } }
        public double? ExchangeFee { get { return exchangeFee; } }
        public double? ClearingFee { get { return clearingFee; } }
        public double? TotalCost { get { return totalcost; } }
 
    }
}
