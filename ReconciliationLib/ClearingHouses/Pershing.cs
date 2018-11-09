using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class Pershing : ClearingHouseBase, IClearingHouse
    {
        public Pershing() : base(ClearingHouse.Pershing) { }

        #region IClearingHouse Members
        public override IPositionTableAdapter PositionTableAdapter
        {
             get
            {
                return new HugoDataSetTableAdapters.t_PershingPositionsTableAdapter(Utilities.Connection);
            }
      }

        public override IConfirmationTableAdapter ConfirmationTableAdapter
        {
             get
            {
                return new HugoDataSetTableAdapters.t_PershingConfirmationsTableAdapter(Utilities.Connection);
            }
      }
        public override ITaxlotTableAdapter TaxlotTableAdapter
        {
            get
            {
                return new HugoDataSetTableAdapters.t_PershingAggregatedTaxlotsTableAdapter(Utilities.Connection);
              }
        }

        public override IPositionRecordCollection GetPositionRecordCollection(string fileName)
        {
            return new PershingPositionRecordCollection(fileName);
        }

        public override IConfirmationRecordCollection GetConfirmationRecordCollection(string fileName)
        {
            return new PershingConfirmationRecordCollection(fileName);
        }
        public override ITaxlotRecordCollection GetTaxlotRecordCollection(string fileName)
        {
            return new PershingAggregatedTaxlotRecordCollection(fileName);
        }

        public override string PositionFileTemplate
        {
            get { return "*.GCUS"; }
        }

        public override string ConfirmationFileTemplate
        {
            get { return "*.GACT"; }
        }
        public override string TaxlotFileTemplate
        {
            get { return "*.POTL"; }
        }
        #endregion
    }
}
