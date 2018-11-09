using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class Stock : IUnderlying, IEquatable<IUnderlying>
    {
        private string underlyingSymbol;

        internal Stock(DataRowView rowView)
        {
            if (rowView.DataView.Table.Columns.Contains("StockSymbol"))
            {
                underlyingSymbol = Convert.ToString(rowView["StockSymbol"]);
            }
            else
            {
                underlyingSymbol = Convert.ToString(rowView["UnderlyingSymbol"]);
            }
        }

        public string UnderlyingSymbol { get { return underlyingSymbol; } }

        public override string ToString()
        {
            return underlyingSymbol;
        }

        #region IEquatable<IUnderlying> Members

        public bool Equals(IUnderlying other)
        {
            if (other == null)
                return false;
            else
                return underlyingSymbol == other.UnderlyingSymbol;
        }

        #endregion
    }
}
