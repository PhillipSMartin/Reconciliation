using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class ICBC : ClearingHouseBase, IClearingHouse
    {
        public ICBC() : base(ClearingHouse.ICBC) { }

        #region IClearingHouse Members
        #region Table adapters
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_ICBCPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_ICBCConfirmationsTableAdapter(Utilities.Connection);
            }
        }
        #endregion

        #region Import methods
        public override ImportConfirmationsDelegate ImportConfirmationsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertICBCConfirmations;
            }
        }
        public override ImportPositionsDelegate ImportPositionsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertICBCPositions;
            }
        }
        #endregion

        #region Logging methods
        public override void LogImportPositionsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_ICBCPositions");
        }
        public override void LogImportConfirmationsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_ICBCConfirmations");
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
        #endregion

        #endregion
    }
}
