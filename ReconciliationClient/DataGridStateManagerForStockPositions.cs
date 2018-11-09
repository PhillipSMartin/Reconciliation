using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForStockPositions : DataGridStateManagerForPositions
    {
        private string selectedHugoSymbol;
        private string selectedMerrillSymbol;

        public DataGridStateManagerForStockPositions(DataGridView dataGridView,
             EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
             ToolStripStatusLabel filterStatusLabel,
             ToolStripStatusLabel showAllLabel,
             ToolStripStatusLabel showNonZeroDiscrepanciesLabel,
            ToolStripStatusLabel showDetailsLabel,
             DataGridViewColumn[] detailColumns,
             DataGridViewColumn[] totalColumns)
            : base(dataGridView,
            refreshBindingSource,
            filterStatusLabel,
            showAllLabel,
            showNonZeroDiscrepanciesLabel,
            showDetailsLabel,
            detailColumns,
            totalColumns)
        {
        }

        public string SelectedHugoSymbol { get { return selectedHugoSymbol; } }
        public string SelectedMerrillSymbol { get { return selectedMerrillSymbol; } }

        public void SetSelectedSymbols(out bool symbolsSet, out bool sameRow)
        {
            selectedHugoSymbol = null;
            selectedMerrillSymbol = null;
            int hugoRowIndex = -1;
            int merrillRowIndex = -1;
            int nCellCount = 0;

            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                if ("StockSymbol" == dataGridView.Columns[cell.ColumnIndex].DataPropertyName)
                {
                    nCellCount++;
                    hugoRowIndex = cell.RowIndex;
                    selectedHugoSymbol = Convert.ToString(cell.Value);
                }
                else if ("MerrillSymbol" == dataGridView.Columns[cell.ColumnIndex].DataPropertyName)
                {
                    nCellCount++;
                    merrillRowIndex = cell.RowIndex;
                    selectedMerrillSymbol = Convert.ToString(cell.Value);
                }
            }

            sameRow = (hugoRowIndex == merrillRowIndex);
            if (String.IsNullOrEmpty(selectedHugoSymbol) || String.IsNullOrEmpty(selectedMerrillSymbol))
            {
                // neither symbol can be null
                symbolsSet = false;
            }
            else
            {
                // we must have selected each symbol only once and they must be different
                symbolsSet = (selectedHugoSymbol != selectedMerrillSymbol) && (nCellCount == 2);
            }

        }
    }
}
