using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class ClearingHouseBase : IClearingHouse
    {
        private ClearingHouse m_clearingHouse;

        protected ClearingHouseBase(ClearingHouse clearingHouse)
        {
            m_clearingHouse = clearingHouse;
        }
        public ClearingHouseBase() : this(ClearingHouse.None) { }

        #region IClearingHouse Members

        public ClearingHouse ClearingHouse
        {
            get { return m_clearingHouse; }
        }

        #region Table adapters
        public virtual IPositionTableAdapter PositionTableAdapter
        {
            get { return null; }
        }

        public virtual IConfirmationTableAdapter ConfirmationTableAdapter
        {
            get { return null; }
        }

        public virtual ITaxlotTableAdapter TaxlotTableAdapter
        {
            get { return null; }
        }

        public virtual IDividendTableAdapter DividendTableAdapter
        {
            get { return null; }
        }

        public virtual IBookkeepingTableAdapter BookkeepingTableAdapter
        {
            get { return null; }
        }
        #endregion

        #region Import methods
        public virtual ImportPositionsDelegate ImportPositionsMethod { get { return null; } }
        public virtual ImportConfirmationsDelegate ImportConfirmationsMethod { get { return null; } }
        public virtual ImportTaxlotsDelegate ImportTaxlotsMethod { get { return null; } }
        public virtual ImportDividendsDelegate ImportDividendsMethod { get { return null; } }
        public virtual ImportTradesDelegate ImportTradesMethod { get { return null; } }
        public virtual ImportBookkeepingDelegate ImportBookkeepingMethod { get { return null; } }
        #endregion

        #region Logging methods
        public virtual void LogImportPositionsMethod() { }
        public virtual void LogImportConfirmationsMethod() { }
        public virtual void LogImportTaxlotsMethod() { }
        public virtual void LogImportDividendsMethod() { }
        public virtual void LogImportTradesMethod() { }
        public virtual void LogImportBookkeepingMethod() { }
        #endregion

        #region Filename templates
        public virtual string PositionFileTemplate
        {
            get { return null; }
        }

        public virtual string ConfirmationFileTemplate
        {
            get { return null; }
        }

         public virtual string TaxlotFileTemplate
        {
            get { return null; }
        }
        public virtual string DividendFileTemplate
        {
            get { return null; }
        }
        public virtual string TradeFileTemplate
        {
            get { return null; }
        }
        public virtual string BookkeepingFileTemplate
        {
            get { return null; }
        }
        #endregion

        #region Bools
        public bool HasPositions { get { return PositionFileTemplate != null; } }
        public bool HasConfirmations { get { return ConfirmationFileTemplate != null; } }
        public bool HasTaxlots { get { return TaxlotFileTemplate != null; } }
        public bool HasDividends { get { return DividendFileTemplate != null; } }
        public bool HasTrades { get { return TradeFileTemplate != null; } }
        public bool HasBookkeeping { get { return BookkeepingFileTemplate != null; } }
        #endregion

        #region Legacy methodology
        public virtual IPositionRecordCollection GetPositionRecordCollection(string fileName)
        {
            throw new ReconciliationException(String.Format("No position files defined for clearing house {0}", ClearingHouse));
        }

        public virtual IConfirmationRecordCollection GetConfirmationRecordCollection(string fileName)
        {
            throw new ReconciliationException(String.Format("No confirmation files defined for clearing house {0}", ClearingHouse));
        }

        public virtual ITaxlotRecordCollection GetTaxlotRecordCollection(string fileName)
        {
            throw new ReconciliationException(String.Format("No taxlot files defined for clearing house {0}", ClearingHouse));
        }
        public virtual IDividendRecordCollection GetDividendRecordCollection(string fileName)
        {
            throw new ReconciliationException(String.Format("No dividend files defined for clearing house {0}", ClearingHouse));
        }
        #endregion

        #endregion
    }
}
