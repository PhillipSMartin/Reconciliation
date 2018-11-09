using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class Future : IUnderlying, IEquatable<IUnderlying>
    {
        private string underlyingSymbol;

        internal Future(DataRowView rowView)
        {
                underlyingSymbol = Convert.ToString(rowView["FuturesSymbol"]);
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
