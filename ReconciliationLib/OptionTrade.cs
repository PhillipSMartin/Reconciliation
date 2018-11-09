using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ReconciliationLib
{
    public class OptionTrade : IOptionTrade, IEquatable<OptionTrade>
    {
        #region Declarations
        int? tradeId;
        string underlyingSymbol;
        string tradeType;
        double tradeVolume;
        string optionSymbol;
        DateTime expirationDate;
        decimal strikePrice;
        string  optionType;
        double tradePrice;
        string  subAcctName;
        string  brokerName;
        string  exchangeName;
        string tradeMedium;
        string  tradeNote;
        DateTime tradeDate;
        string  tradeReason;
        string traderName;
        bool? spreadFlag;
        bool? tradeArchiveFlag;
        bool? optionArchiveFlag;
        double commission;
        double secFee;
        double orfFee;
        double totalCost;
        int? consolidationPackageId;
        int numberOfTrades = 1;
        #endregion

        #region Constructors
        public OptionTrade(HugoDataSet.HugoOptionTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = row.TradeTypeName;
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            commission = Convert.ToDouble(row.Commission);
            secFee = Convert.ToDouble(row.SECFee);
            orfFee = Convert.ToDouble(row.ORFFee);
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;
            optionSymbol = row.OptionSymbol;
            expirationDate = row.ExpirationDate;
            strikePrice = row.StrikePrice;
            optionType = row.OptionType;

            brokerName = ConvertExt.ToStringOrNull(row.BrokerName);
            exchangeName = ConvertExt.ToStringOrNull(row.ExchangeName);
            tradeMedium = ConvertTradeMedium(ConvertExt.ToStringOrNull(row.TradeMediumName));
            tradeReason = ConvertExt.ToStringOrNull(row.TradeReasonDescr);
            tradeDate = row.TradeDateTime;
            tradeNote = ConvertExt.ToStringOrNull(row.TradeNote);
            traderName = ConvertExt.ToStringOrNull(row.TraderName);

            spreadFlag = ConvertExt.ToBooleanOrNull(row["SpreadFlag"]);
            tradeArchiveFlag = ConvertExt.ToBooleanOrNull(row["TradeArchiveFlag"]);
  
            consolidationPackageId = row.ConsolidationPackageId;
            numberOfTrades = row.NumberOfTrades;
        }

        public OptionTrade(HugoDataSet.HugoOptionCorrectionsRow row)
        {
            tradeType = row.TradeTypeName;
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;
            optionSymbol = row.OptionSymbol;
            expirationDate = row.ExpirationDate;
            strikePrice = row.StrikePrice;
            optionType = row.OptionType;

            tradeMedium = ConvertTradeMedium(ConvertExt.ToStringOrNull(row.TradeMediumName));
            tradeDate = row.TradeDateTime;

            if (row.RecordType == "Update")
                tradeNote = "Changed to: " + row.DifferenceMsg;
            else
                tradeNote = "(" + row.RecordType + ")";
        }

        public OptionTrade(HugoDataSet.OptionPositionsRow row)
        {
            tradeId = -1;
            tradeType = (row.Discrepancy > 0) ? "Sell" : "Buy";
            tradeVolume = Convert.ToDouble(Math.Abs(row.Discrepancy));
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;
            optionSymbol = row.OptionSymbol;
            expirationDate = row.ExpirationDate;
            strikePrice = row.StrikePrice;
            optionType = row.OptionType;

            tradeMedium = "Correction";
            tradeReason = "Journal";
            tradeDate = ConvertExt.ToValidDateTime(null);
            tradeNote = "Added by reconciliation client to match clearing house position";
            traderName = Utilities.TraderName;
        }

        public OptionTrade(HugoDataSet.MerrillOptionTradesRow row)
        {
            tradeId = row.TradeId;
            tradeType = ConvertTradeTypeName(row.TradeTypeName, row.CancelCode);
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            commission = row.Commission;
            secFee = row.SECFee;
            orfFee = row.ORFFee;
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;
            optionSymbol = row.OptionSymbol;
            expirationDate = row.ExpirationDate;
            strikePrice = row.StrikePrice;
            optionType = row.OptionType;

            exchangeName = row.ExchangeName.StartsWith("?") ? null : row.ExchangeName;
            tradeMedium = ConvertTradeMedium(ConvertExt.ToStringOrNull(row.TradeMediumName));
            tradeNote = "Added by reconciliation client";
            tradeDate = ConvertExt.ToValidDateTime(row.TradeDate);
            traderName = Utilities.TraderName;
            optionArchiveFlag = row.OptionArchiveFlag;
        }

        public OptionTrade(HugoDataSet.ConfirmationCorrectionsRow row)
        {
            tradeType = ConvertTradeTypeName(row.TradeTypeName, "");
            tradeVolume = Convert.ToDouble(row.TradeVolume);
            tradePrice = Convert.ToDouble(row.TradePrice);
            totalCost = Convert.ToDouble(row.TotalCost);
            subAcctName = row.SubAcctName;

            underlyingSymbol = row.UnderlyingSymbol;
            optionSymbol = row.OptionSymbol;
            expirationDate = row.ExpirationDate;
            strikePrice = row.StrikePrice;
            optionType = row.OptionType;

            exchangeName = row.ExchangeName.StartsWith("?") ? null : row.ExchangeName;
            tradeDate = ConvertExt.ToValidDateTime(row.TradeDate);

            if (row.RecordType == "Correction")
                tradeNote = "Changed to: " + row.GetDifferenceMsg(false /* use Hugo account and underlying symbols */);
            else
                tradeNote = "(" + row.RecordType + ")";
        }

         #endregion

        #region Public methods
        public double CopyTradeInfoFromPackage(IOptionTrade package, int tradeNumber, double remainderTotalCost)
        {
            tradeType = package.TradeType;
            subAcctName = package.SubAcctName;

            optionSymbol = package.OptionSymbol;
            expirationDate = package.ExpirationDate;
            strikePrice = package.StrikePrice;
            optionType = package.OptionType;

            exchangeName = package.ExchangeName;
            tradeMedium = package.TradeMedium;

            // for last trade, use remainder of TotalCost
            if (tradeNumber == package.NumberOfTrades)
            {
                totalCost = remainderTotalCost;
                remainderTotalCost = 0;
            }

            // for earlier trades, calculate it
            else
            {
                totalCost = Math.Round(package.TotalCost * (((package.UnitCost * package.TradeVolume) == 0) ? 1 :
                    UnitCost * TradeVolume / (package.UnitCost * package.TradeVolume)), 2);
                remainderTotalCost -= totalCost;
            }

            return remainderTotalCost;
        }
        #endregion

        #region Private methods
        private static string ConvertTradeTypeName(string tradeTypeName, string cancelCode)
        {
            // "Buy"? 
            bool bBuy = (tradeTypeName == "Buy");

            // if a cancellation, reverse tradetype
            if (cancelCode == "Cancel")
                bBuy = !bBuy;

            // return tradetype
            return bBuy ? "Buy" : "Sell";
        }

        private static string ConvertTradeMedium(string tradeMedium)
        {
            if (Utilities.IsExerciseAssignment(tradeMedium))
                return Utilities.ExerciseAssignmentName;
            else
                return tradeMedium;
        }
        #endregion

        #region IOptionTrade Members
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

        public decimal TradePrice
        {
            get { return Convert.ToDecimal(tradePrice); }
            set { tradePrice = Convert.ToDouble(value); }
        }

        public DateTime TradeDate
        {
            get { return tradeDate; }
            set { tradeDate = value; }
        }

        public string SubAcctName
        {
            get { return subAcctName; }
        }

        public string TraderName
        {
            get { return traderName; }
            set { traderName = value; }
        }

        public short? TraderId
        {
            get { return null; }
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

        public short? MediumId
        {
            get { return null; }
        }

        public string TradeReason
        {
            get { return tradeReason; }
        }

        public string TradeNote
        {
            get { return tradeNote; }
            set { tradeNote = value; }
        }

   
        public bool? SpreadFlag
        {
            get { return spreadFlag; }
        }

        public bool? TradeArchiveFlag
        {
            get { return tradeArchiveFlag; }
        }
        public bool? OptionArchiveFlag
        {
            get { return optionArchiveFlag; }
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
            get { return orfFee; }
            set { orfFee = value; }
        }

        #endregion

        #region IOption Members

        public string OptionSymbol
        {
            get { return optionSymbol; }
        }

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
        }

        public decimal StrikePrice
        {
            get { return strikePrice; }
        }

        public string OptionType
        {
            get { return optionType; }
        }

        public double FractionalRemainder 
        { 
            get { return TradeVolume - tradeVolume; } 
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
            get { return null; } 
        }

        public int? ConsolidationPackageId
        {
            get { return consolidationPackageId; }
        }
        public int NumberOfTrades
        {
            get { return numberOfTrades; }
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
                return String.Format("({8}) {0} {1} {2} {3:d} {4} {5}s at {6} for {7}, Cost=${9:f2}",
                     TradeType, TradeVolume, OptionSymbol, ExpirationDate, StrikePrice, OptionType, TradePrice, SubAcctName, TradeId, TotalCost);
            }
            else
            {
                return String.Format("{0} {1} {2} {3:d} {4} {5}s at {6} for {7}, Cost=${8:f2}",
                    TradeType, TradeVolume, OptionSymbol, ExpirationDate, StrikePrice, OptionType, TradePrice, SubAcctName, TotalCost);
            }
        }
        #endregion

        #region IEquatable<OptionTrade> Members

        public bool Equals(OptionTrade other)
        {
            if (other == null)
                return false;
            else
                return (OptionSymbol == other.OptionSymbol) &&
                    (ExpirationDate == other.ExpirationDate) &&
                    (StrikePrice == other.StrikePrice) &&
                    (OptionType == other.OptionType) &&
                    (TradeType == other.TradeType) &&
                    (TradeVolume == other.TradeVolume) &&
                    (Math.Abs(Commission - other.Commission) < .005) &&
                    (Math.Abs(SECFee - other.SECFee) < .005) &&
                    (Math.Abs(ORFFee - other.ORFFee) < .005) &&
                    (TradePrice == other.TradePrice) &&
                    (SubAcctName == other.SubAcctName) &&
                    (Math.Abs(TotalCost - other.TotalCost) < .005);
        }

        #endregion
    }
}
