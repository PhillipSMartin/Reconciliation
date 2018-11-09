using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class BONY : ClearingHouseBase, IClearingHouse
    {
        public BONY() : base(ClearingHouse.BONY) { }

        #region IClearingHouse Members
        #region Table adapters
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_BONYPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_BONYConfirmationsTableAdapter(Utilities.Connection);
            }
        }
        #endregion

        #region Import methods
        public override ImportConfirmationsDelegate ImportConfirmationsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertBONYConfirmations;
            }
        }
        public override ImportPositionsDelegate ImportPositionsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertBONYPositions;
            }
        }
         #endregion

        #region Logging methods
        public override void LogImportPositionsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_BONYPositions");
        }
        public override void LogImportConfirmationsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_BONYConfirmations");
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
