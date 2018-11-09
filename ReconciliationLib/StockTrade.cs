using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class StockTrade : IStockTrade, IEquatable<StockTrade>
    {
        #region Declarations
        int? tradeId;
        string underlyingSymbol;
        string tradeType;
        double tradeVolume;
        double tradePrice;
        string subAcctName;
        string brokerName;
        string exchangeName;
        string tradeMedium;
        string tradeNote;
        DateTime tradeDate;
        string tradeReason;
        bool shortFlag;
        string traderName;
        double totalCost;
        string taxLotId;
        double commission;
        double secFee;

        #endregion

        #region Constructors
        public StockTrade(HugoDataSet.HugoStockTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "", ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.StockSymbol;

            brokerName = ConvertExt.ToStringOrNull(row.BrokerName);
            exchangeName = ConvertExt.ToStringOrNull(row.ExchangeName);
            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeReason = ConvertExt.ToStringOrNull(row.TradeReasonDescr);
            tradeDate = row.TradeDateTime;
            tradeNote = ConvertExt.ToStringOrNull(row.TradeNote);
            traderName = ConvertExt.ToStringOrNull(row.TraderName);
            totalCost = Convert.ToDouble(row.TotalCost);
            taxLotId = ConvertExt.ToStringOrNull(row.TaxLotId);
            commission = Convert.ToDouble(row.Commission);
            secFee = Convert.ToDouble(row.SEC_Fee);
        }

        public StockTrade(HugoDataSet.StockPositionsRow row)
        {
            tradeId = -1;
            tradeType = (row.Discrepancy > 0) ? "Sell" : "Buy";
            tradeVolume = Convert.ToDouble(Math.Abs(row.Discrepancy));
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.StockSymbol;

            tradeMedium = "Correction";
            tradeReason = "Journal";
            tradeDate = ConvertExt.ToValidDateTime(null);
            tradeNote = "Added by reconciliation client to match clearing house position";
            traderName = Utilities.TraderName;
        }

        public StockTrade(HugoDataSet.HugoStockCorrectionsRow row)
        {
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "", ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.StockSymbol;

            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeDate = row.TradeDateTime;
            totalCost = Convert.ToDouble(row.TotalCost);
            taxLotId = ConvertExt.ToStringOrNull(row.TaxLotId);

            if (row.RecordType == "Update")
                tradeNote = "Changed to: " + row.DifferenceMsg;
            else
                tradeNote = "(" + row.RecordType + ")";
        }

        public StockTrade(HugoDataSet.MerrillStockTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = ConvertTradeTypeName(row.TradeTypeName, row.CancelCode, ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            commission = row.Commission;
            secFee = row.SECFee;
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.StockSymbol;

            exchangeName = row.ExchangeName.StartsWith("?") ? null : row.ExchangeName;
            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeNote = "Added by reconciliation client";
            tradeDate = ConvertExt.ToValidDateTime(row.TradeDate);
            traderName = Utilities.TraderName;
         }

        public StockTrade(HugoDataSet.ConfirmationCorrectionsRow row)
        {
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "", ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;

            exchangeName = row.ExchangeName.StartsWith("?") ? null : row.ExchangeName;
            tradeDate = ConvertExt.ToValidDateTime(row.TradeDate);

            if (row.RecordType == "Correction")
                tradeNote = "Changed to: " + row.GetDifferenceMsg(false /* use Hugo account and underlying symbols */);
            else
                tradeNote = "(" + row.RecordType + ")";
        }

         #endregion

        #region Private methods
        private static string ConvertTradeTypeName(string tradeTypeName, string cancelCode, ref bool shortFlag)
        {
            // "Buy"? - alternatives are "Sell" and "Sell short"
            bool bBuy = (tradeTypeName == "Buy");

            // if a cancellation, reverse tradetype
            if (cancelCode == "Cancel")
                bBuy = !bBuy;

            // set short flag
            shortFlag = (tradeTypeName == "Sell short") && !bBuy;

            // return tradetype
            return bBuy ? "Buy" : "Sell";
        }

       #endregion

        #region IStockTrade Members

        public int? TradeId
        {
            get { return tradeId; }
        }
        public string TradeType
        {
            get { return tradeType; }
        }

        public int TradeVolume
        {
            get { return Convert.ToInt32(Math.Truncate(tradeVolume)); }
            set { tradeVolume = Convert.ToDouble(value); }
        }

        public double FullVolume
        {
            get { return tradeVolume; }
            set { tradeVolume = value; }
        }

        public decimal TradePrice
        {
            get { return Convert.ToDecimal(tradePrice); }
            set { tradePrice = Convert.ToDouble(value); }
        }

        public DateTime TradeDate
        {
            get { return tradeDate; }
        }

        public string SubAcctName
        {
            get { return subAcctName; }
        }

        public string TraderName
        {
            get { return traderName; }
        }

        public short TraderId
        {
            get { return -1; }
        }

        public string BrokerName
        {
            get { return brokerName; }
        }

        public string ExchangeName
        {
            get { return exchangeName; }
        }

        public string TradeMedium
        {
            get { return tradeMedium; }
        }

        public string TradeReason
        {
            get { return tradeReason; }
        }

        public string TradeNote
        {
            get { return tradeNote; }
        }

        public string BillingTypeDescr
        {
            get { return null; }
        }

        public decimal SpecialRate
        {
            get { return 0; }
        }

        public long? OptionTradeId
        {
            get { return null; }
        }

        public bool TradeArchiveFlag
        {
            get { return false; }
        }

        public bool ShortFlag
        {
            get { return shortFlag; }
        }

        public double FractionalRemainder
        {
            get { return tradeVolume - TradeVolume; }
        }
        public double UnitCost
        {
            get { return tradePrice; }
        }
        public double TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }
        public string TaxLotId
        {
            get { return taxLotId; }
        }
        public int? ConsolidationPackageId
        {
            get { return null; }
        }
        public int NumberOfTrades
        {
            get { return 1; }
        }
        public double Commission
        {
            get { return commission; }
            set { commission = value; }
        }
        public double SECFee
        {
            get { return secFee; }
            set { secFee = value; }
        }
        public double ORFFee
        {
            get { return 0; }
        }
        #endregion

        #region IUnderlying Members

        public string UnderlyingSymbol
        {
            get { return underlyingSymbol; }
        }

        #endregion

        #region Overrides
        public override string ToString()
        {
            if (TradeId.HasValue)
            {
                return String.Format("({6}) {0}{1} {2} {3} at {4} for {5}, cost=${7:f2}",
                    TradeType, ShortFlag ? " short" : "", FullVolume, UnderlyingSymbol, TradePrice, SubAcctName, TradeId, TotalCost);
            }
            else
            {
                return String.Format("{0}{1} {2} {3} at {4} for {5}, cost=${6:f2}",
                   TradeType, ShortFlag ? " short" : "", FullVolume, UnderlyingSymbol, TradePrice, SubAcctName, TotalCost);
            }
        }
        #endregion

        #region IEquatable<StockTrade> Members

        public bool Equals(StockTrade other)
        {
            if (other == null)
                return false;
            else
                return (UnderlyingSymbol == other.UnderlyingSymbol) &&
                    (TradeType == other.TradeType) &&
                    (TradeVolume == other.TradeVolume) &&
                    (Math.Abs(Commission - other.Commission) < .005) &&
                    (Math.Abs(SECFee - other.SECFee) < .005) &&
                    (TradePrice == other.TradePrice) &&
                    (SubAcctName == other.SubAcctName) &&
                    (Math.Abs(TotalCost - other.TotalCost) < .005);
        }

        #endregion
    }
}
