using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class MorganStanley : ClearingHouseBase, IClearingHouse
    {
        public MorganStanley() : base(ClearingHouse.MorganStanley) { }

        #region Table adapters
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_MorganStanleyPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_MorganStanleyConfirmationsTableAdapter(Utilities.Connection);
            }
        }
        #endregion

        #region IClearingHouse Members
        public override ImportConfirmationsDelegate ImportConfirmationsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertMorganStanleyConfirmations;
            }
        }
        public override ImportPositionsDelegate ImportPositionsMethod
        {
            get
            {
                return Utilities.QueriesAdapter.InsertMorganStanleyPositions;
            }
        }

        public override void LogImportPositionsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_MorganStanleyPositions");
        }
        public override void LogImportConfirmationsMethod()
        {
            Utilities.QueriesAdapter.LogCommand("Reconciliation.p_insert_MorganStanleyConfirmations");
        }

         public override string PositionFileTemplate
        {
            get { return "*.csv"; }
        }

        public override string ConfirmationFileTemplate
        {
            get { return "*.csv"; }
        }
          #endregion
    }
}
