using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class WellsFargo : ClearingHouseBase, IClearingHouse
    {
        public WellsFargo() : base(ClearingHouse.WellsFargo) { }

        #region IClearingHouse Members

        #region Table adapters
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_WellsFargoPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get 
            {
                return new HugoDataSetTableAdapters.t_WellsFargoConfirmationsTableAdapter(Utilities.Connection);
            }
        }
        #endregion

        #region Import methods
        public override ImportConfirmationsDelegate ImportConfirmationsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertWellsFargoConfirmations;
            }
        }
        public override ImportPositionsDelegate ImportPositionsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertWellsFargoPositions;
            }
        }
        public override ImportDividendsDelegate ImportDividendsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.ImportWellsFargoDividends;
            }
        }
        public override ImportTradesDelegate ImportTradesMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertWellsFargoTradesFromEODBlotter;
            }
        }
        #endregion

        #region Logging methods
        public override void LogImportPositionsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_WellsFargoXmlPositions");
        }
        public override void LogImportConfirmationsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_WellsFargoXmlConfirmations");
        }
        public override void LogImportDividendsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insertWellsFargoDividends");
        }

        public override void LogImportTradesMethod() 
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insertWellsFargoTrades_from_EODBlotter");
        }
        #endregion

        #region Filename templates
        public override string PositionFileTemplate
        {
            get { return "*.xml"; }
        }

        public override string ConfirmationFileTemplate
        {
            get { return "*.xml"; }
        }
        public override string DividendFileTemplate
        {
            get { return "*.xml"; }
        }
        public override string TradeFileTemplate
        {
            get { return "*.csv"; }
        }
        #endregion

        #endregion
    }
}
