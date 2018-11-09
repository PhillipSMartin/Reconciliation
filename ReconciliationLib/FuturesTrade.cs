using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class FuturesTrade : IFuturesTrade, IEquatable<FuturesTrade>
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
        double nfaFee;
        double clearingFee;
        double exchangeFee;
        double commission;

        #endregion

        #region Constructors
        public FuturesTrade(HugoDataSet.HugoFuturesTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "", ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.FuturesSymbol;

            brokerName = ConvertExt.ToStringOrNull(row.BrokerName);
            exchangeName = ConvertExt.ToStringOrNull(row.ExchangeName);
            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeReason = ConvertExt.ToStringOrNull(row.TradeReasonDescr);
            tradeDate = row.TradeDateTime;
            tradeNote = ConvertExt.ToStringOrNull(row.TradeNote);
            traderName = ConvertExt.ToStringOrNull(row.TraderName);
            totalCost = Convert.ToDouble(row.TotalCost);

            commission = Convert.ToDouble(row.Commission);
            nfaFee = Convert.ToDouble(row.NFAFee);
            exchangeFee = Convert.ToDouble(row.ExchangeFee);
            clearingFee = Convert.ToDouble(row.ClearingFee);
        }

        public FuturesTrade(HugoDataSet.FuturesPositionsRow row)
        {
            tradeId = -1;
            tradeType = (row.Discrepancy > 0) ? "Sell" : "Buy";
            tradeVolume = Convert.ToDouble(Math.Abs(row.Discrepancy));
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.FuturesSymbol;

            tradeMedium = "Correction";
            tradeReason = "Journal";
            tradeDate = ConvertExt.ToValidDateTime(null);
            tradeNote = "Added by reconciliation client to match clearing house position";
            traderName = Utilities.TraderName;
        }

        public FuturesTrade(HugoDataSet.HugoFuturesCorrectionsRow row)
        {
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "", ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.FuturesSymbol;

            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeDate = row.TradeDateTime;
            totalCost = Convert.ToDouble(row.TotalCost);

            if (row.RecordType == "Update")
                tradeNote = "Changed to: " + row.DifferenceMsg;
            else
                tradeNote = "(" + row.RecordType + ")";
        }

        public FuturesTrade(HugoDataSet.MerrillFuturesTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = ConvertTradeTypeName(row.TradeTypeName, row.CancelCode, ref shortFlag);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.FuturesSymbol;

            exchangeName = row.ExchangeName.StartsWith("?") ? null : row.ExchangeName;
            tradeMedium = ConvertExt.ToStringOrNull(row.TradeMediumName);
            tradeNote = "Added by reconciliation client";
            tradeDate = ConvertExt.ToValidDateTime(row.TradeDate);
            traderName = Utilities.TraderName;

            commission = Convert.ToDouble(row.Commission);
            nfaFee = Convert.ToDouble(row.NFAFee);
            exchangeFee = Convert.ToDouble(row.ExchangeFee);
            clearingFee = Convert.ToDouble(row.ClearingFee);
        }

        public FuturesTrade(HugoDataSet.ConfirmationCorrectionsRow row)
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
            // if a cancellation, reverse tradetype
            if (cancelCode == "Cancel")
            {
                if (tradeTypeName == "Buy")
                    tradeTypeName = "Sell";
                else
                    tradeTypeName = "Buy";
            }
 
            // set short flag
            if (tradeTypeName == "Sell short")
            {
                shortFlag = true;
                tradeTypeName = "Sell";
            }
            else
            {
                shortFlag = false;
            }

            // return tradetype
            return tradeTypeName;
        }

        #endregion

        #region IFuturesTrade Members

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

         public double UnitCost
        {
            get { return tradePrice; }
        }
        public double TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }
         public int? ConsolidationPackageId
        {
            get { return null; }
        }
        public int NumberOfTrades
        {
            get { return 1; }
        }
        public string TaxLotId
        {
            get { return null; }
        }
        public double FractionalRemainder
        {
            get { return TradeVolume - tradeVolume; }
        }
        public double Commission
        {
            get { return commission; }
            set { commission = value; }
         }
        public double SECFee
        {
            get { return 0; }
          }
        public double ORFFee
        {
            get { return 0; }
        }
        public double NFAFee
        {
            get { return nfaFee; }
            set { nfaFee = value; }
        }
        public double ExchangeFee
        {
            get { return exchangeFee; }
            set { exchangeFee = value; }
        }
        public double ClearingFee
        {
            get { return clearingFee; }
            set { clearingFee = value; }
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

        #region IEquatable<FuturesTrade> Members

        public bool Equals(FuturesTrade other)
        {
            if (other == null)
                return false;
            else
                return (UnderlyingSymbol == other.UnderlyingSymbol) &&
                    (TradeType == other.TradeType) &&
                    (TradeVolume == other.TradeVolume) &&
                    (TradePrice == other.TradePrice) &&
                    (SubAcctName == other.SubAcctName);
        }

        #endregion
    }
}
