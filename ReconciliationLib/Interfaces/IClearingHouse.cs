using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public interface IClearingHouse
    {
        ClearingHouse ClearingHouse { get; }

        IPositionTableAdapter PositionTableAdapter { get; }
        IConfirmationTableAdapter ConfirmationTableAdapter { get; }
        ITaxlotTableAdapter TaxlotTableAdapter { get; }
        IDividendTableAdapter DividendTableAdapter { get; }
        IBookkeepingTableAdapter BookkeepingTableAdapter { get; }

        ImportPositionsDelegate ImportPositionsMethod { get; }
        ImportConfirmationsDelegate ImportConfirmationsMethod { get; }
        ImportTaxlotsDelegate ImportTaxlotsMethod { get; }
        ImportDividendsDelegate ImportDividendsMethod { get; }
        ImportTradesDelegate ImportTradesMethod { get; }
        ImportBookkeepingDelegate ImportBookkeepingMethod { get; }

        void LogImportPositionsMethod();
        void LogImportConfirmationsMethod();
        void LogImportTaxlotsMethod();
        void LogImportDividendsMethod();
        void LogImportTradesMethod();
        void LogImportBookkeepingMethod();

        string PositionFileTemplate { get; }
        string ConfirmationFileTemplate { get; }
        string TaxlotFileTemplate { get; }
        string DividendFileTemplate { get; }
        string TradeFileTemplate { get; }
        string BookkeepingFileTemplate { get; }

        bool HasPositions { get; }
        bool HasConfirmations { get; }
        bool HasTaxlots { get; }
        bool HasDividends { get; }
        bool HasTrades { get; }
        bool HasBookkeeping { get; }

        // legacy methodology
        IPositionRecordCollection GetPositionRecordCollection(string fileName);
        IConfirmationRecordCollection GetConfirmationRecordCollection(string fileName);
        ITaxlotRecordCollection GetTaxlotRecordCollection(string fileName);
        IDividendRecordCollection GetDividendRecordCollection(string fileName);
    }
}
