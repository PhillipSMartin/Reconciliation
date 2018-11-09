using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class EditedOptionTrade
    {
	    private int tradeid;

	    // if these value are NULL, they remain unchanged from the original trade
	    private string optionsymbol;
	    private DateTime? nominalExpirationDate;
	    private decimal? strike;
	    private string optiontype;

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
	    private double? secFee;
	    private double? orfFee;
        private double? totalcost;

        public EditedOptionTrade(HugoDataSet.HugoOptionTradesDataTable table, HugoDataSet.HugoOptionTradesRow row)
        {
            tradeid = row.TradeId;

            if (IsColumnChanged(row, table.OptionSymbolColumn))
                optionsymbol = row.OptionSymbol;
            if (IsColumnChanged(row, table.ExpirationDateColumn))
                nominalExpirationDate = row.ExpirationDate;
            if (IsColumnChanged(row, table.StrikePriceColumn))
                strike = row.StrikePrice;
            if (IsColumnChanged(row, table.OptionTypeColumn))
                optiontype = row.OptionType;

            if (IsColumnChanged(row, table.TradeTypeNameColumn))
                tradetype = row.TradeTypeName;
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
            if (IsColumnChanged(row, table.SECFeeColumn))
                secFee = row.SECFee;
            if (IsColumnChanged(row, table.TotalCostColumn))
                orfFee = row.ORFFee;
        }

        public EditedOptionTrade(EditedOptionTrade editedTrade,
            IOptionTrade package,
            IOptionTrade partialTrade,
            ref double remainderTotalCost,
            ref double remainderCommission,
            ref double remainderSECFee,
            ref double remainderORFFee,
            ref int tradeNumber) 
       {
            tradeid = partialTrade.TradeId.Value;

            optionsymbol = editedTrade.OptionSymbol;
            nominalExpirationDate = editedTrade.NominalExpirationDate;
            strike = editedTrade.Strike;
            optiontype = editedTrade.OptionType;

            tradetype = editedTrade.TradeType;
            subacctname = editedTrade.SubAcctName;
            tradedate = editedTrade.TradeDate;
            tradenote = editedTrade.TradeNote;

            trader = editedTrade.Trader;
            broker = editedTrade.Broker;
            exchange = editedTrade.Exchange;
            medium = editedTrade.Medium;
            reason = editedTrade.Reason;

            // for last trade, use remainders
            if (++tradeNumber == package.NumberOfTrades)
            {
                commission = remainderCommission;
                secFee = remainderSECFee;
                orfFee = remainderORFFee;
                totalcost = remainderTotalCost;
            }

            // for earlier trades, calculate
            else
            {
                double proRataFactor = (((package.UnitCost * package.TradeVolume) == 0) ? 1 :
                    partialTrade.UnitCost * partialTrade.TradeVolume / (package.UnitCost * package.TradeVolume));

                commission = Math.Round(package.Commission * proRataFactor, 2);
                remainderCommission -= commission.Value;

                secFee = Math.Round(package.SECFee * proRataFactor, 2);
                remainderSECFee -= secFee.Value;

                orfFee = Math.Round(package.ORFFee * proRataFactor, 2);
                remainderORFFee -= orfFee.Value;

                totalcost = Math.Round(package.TotalCost * proRataFactor, 2);
                remainderTotalCost -= totalcost.Value;
            }
        }
        
        private bool IsColumnChanged(DataRow row, DataColumn column)
        {
            return row[column, System.Data.DataRowVersion.Current].ToString() != row[column, System.Data.DataRowVersion.Original].ToString();
        }

        public int TradeId { get { return tradeid; } }
        public string OptionSymbol { get { return optionsymbol; } }
        public DateTime? NominalExpirationDate { get { return nominalExpirationDate; } }
        public decimal? Strike { get { return strike; } }
        public string OptionType { get { return optiontype; } }

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
        public double? SECFee { get { return secFee; } }
        public double? ORFFee { get { return orfFee; } }
        public double? TotalCost { get { return totalcost; } }

    }
}
