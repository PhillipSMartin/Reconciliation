using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class RefreshBindingSourceEventArgs : EventArgs
    {
        private IUnderlying underlying;
        private IOption option;
        private DataTable dataTable;
        private bool groupByPrice;

        public RefreshBindingSourceEventArgs(IUnderlying underlying, IOption option)
            : base()
        {
            this.underlying = underlying;
            this.option = option;
        }

        public RefreshBindingSourceEventArgs(IUnderlying underlying, bool groupByPrice)
            : this(underlying, null)
        {
            this.groupByPrice = groupByPrice;
        }

        public RefreshBindingSourceEventArgs()
            : this(null, null)
        {
        }

        public IUnderlying IUnderlying { get { return underlying; } }
        public IOption IOption { get { return option; } }
        public bool GroupByPrice { get { return groupByPrice; } }
        public DataTable DataTable { get { return dataTable; } set { dataTable = value; } }
    }
}
