using System;

namespace ReconciliationConsole
{
    public enum ReconciliationAction
    {
        None,
        Import,
        AddAssignments,
        AddTrades,
        ConsolidateOptionTrades,
        ConsolidateStockTrades,
        ConstructTaxlotsFile
    }
}
