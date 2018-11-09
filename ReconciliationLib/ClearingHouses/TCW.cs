using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class TCW : ClearingHouseBase, IClearingHouse
    {
        public TCW() : base(ClearingHouse.TCW) { }
        public override ITaxlotTableAdapter TaxlotTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_TCWTaxlotsTableAdapter(Utilities.Connection);
            }
        }

        #region IClearingHouse Members
        // Import methods
        public override ImportTradesDelegate ImportTradesMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertTCWTradesFromEODBlotter;
            }
        }
        public override ImportTaxlotsDelegate ImportTaxlotsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertTCWTaxlots;
            }
        }

        // Logging methods
        public override void LogImportTradesMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insertTCWTrades_from_EODBlotter");
        }
        public override void LogImportTaxlotsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_TCWTaxlots");
        }

        // File templates
        public override string TaxlotFileTemplate
        {
            get { return "*.csv"; }
        }
        public override string TradeFileTemplate
        {
            get { return "*.xls"; }
        }
        #endregion
    }
}
