using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class IB : ClearingHouseBase, IClearingHouse
    {
        public IB() : base(ClearingHouse.IB) { }

        #region IClearingHouse Members
        #region Table adapters
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_IBPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_IBConfirmationsTableAdapter(Utilities.Connection);
            }
        }

        public override IBookkeepingTableAdapter BookkeepingTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_IBTransfersTableAdapter(Utilities.Connection);
            }
        }

        public override IDividendTableAdapter DividendTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_IBCorporateActionsTableAdapter(Utilities.Connection);
            }
        }
        #endregion

        #region Import methods
        public override ImportConfirmationsDelegate ImportConfirmationsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertIBConfirmations;
            }
        }
        public override ImportPositionsDelegate ImportPositionsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertIBPositions;
            }
        }
        public override ImportBookkeepingDelegate ImportBookkeepingMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertIBTransfers;
            }
        }
        public override ImportDividendsDelegate ImportDividendsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertIBCorporateActions;
            }
        }
        #endregion

        #region Logging methods
        public override void LogImportPositionsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_IBPositions");
        }
        public override void LogImportConfirmationsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_IBConfirmations");
        }
        public override void LogImportBookkeepingMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_IBTransfers");
        }
        public override void LogImportDividendsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_IBCorporateActions");
        }
        #endregion

        #region Filename templates
        public override string PositionFileTemplate
        {
            get { return "*.csv"; }
        }

        public override string ConfirmationFileTemplate
        {
            get { return "*.csv"; }
        }
        public override string DividendFileTemplate
        {
            get { return "*.csv"; }
        }
        public override string BookkeepingFileTemplate
        {
            get { return "*.csv"; }
        }
        #endregion

        #endregion
    }
}
