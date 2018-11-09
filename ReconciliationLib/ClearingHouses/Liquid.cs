using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class Liquid : ClearingHouseBase, IClearingHouse
    {
        public Liquid() : base(ClearingHouse.Liquid) { }

       #region IClearingHouse Members
        public override IPositionTableAdapter PositionTableAdapter
        {
            get
            {
                 return new HugoDataSetTableAdapters.t_LiquidPositionsTableAdapter(Utilities.Connection);
            }
        }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
             get
            {
                return new HugoDataSetTableAdapters.t_LiquidConfirmationsTableAdapter(Utilities.Connection);
            }
     }
        public override string PositionFileTemplate
        {
            get { return "*.*"; }
        }

        public override string ConfirmationFileTemplate
        {
            get { return "*.*"; }
        }
        public override string TaxlotFileTemplate
        {
            get { return "*.*"; }
        }

       #endregion
    }
}
