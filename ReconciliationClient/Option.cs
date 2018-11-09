using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class Option : IOption, IEquatable<IOption>
    {
        private string underlyingSymbol;
        private string optionSymbol;
        private DateTime expirationDate;
        private decimal strikePrice;
        private string optionType;

        internal Option(DataRowView rowView)
        {
            underlyingSymbol = Convert.ToString(rowView["UnderlyingSymbol"]);
            optionSymbol = Convert.ToString(rowView["OptionSymbol"]);
            expirationDate = Convert.ToDateTime(rowView["ExpirationDate"]);
            strikePrice = Convert.ToDecimal(rowView["StrikePrice"]);
            optionType = Convert.ToString(rowView["OptionType"]);
        }

        public string UnderlyingSymbol { get { return underlyingSymbol; } }
        public string OptionSymbol { get { return optionSymbol; } }
        public DateTime ExpirationDate { get { return expirationDate; } }
        public decimal StrikePrice { get { return strikePrice; } }
        public string OptionType { get { return optionType; } }

        public override string ToString()
        {
            return String.Format("{0} {1:d} {2} {3}", optionSymbol, expirationDate, strikePrice, optionType);
        }

        #region IEquatable<IOption> Members

        public bool Equals(IOption other)
        {
            if (other == null)
                return false;
            else
                return optionSymbol == other.OptionSymbol && expirationDate == other.ExpirationDate &&
                    strikePrice == other.StrikePrice && optionType == other.OptionType;
        }

        #endregion
    }
}
