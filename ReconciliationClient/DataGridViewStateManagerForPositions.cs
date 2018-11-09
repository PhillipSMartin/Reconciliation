using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForPositions : DataGridStateManager
    {
        private const string nonZeroDiscrepancyFilter = "CONVERT([Discrepancy],'System.Int32')<>0";
        private ToolStripStatusLabel showNonZeroDiscrepanciesLabel;
        private ToolStripStatusLabel showDetailsLabel;
        private bool showingDetails;
        private DataGridViewColumn[] detailColumns;
        private DataGridViewColumn[] totalColumns;
        private bool rowsToProcessContainNonzeroDiscrepancies;
        private bool rowsToProcessContainAcceptedDiscrepancies;

        public DataGridStateManagerForPositions(DataGridView dataGridView,
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
            showAllLabel) 
        {
            this.showNonZeroDiscrepanciesLabel = showNonZeroDiscrepanciesLabel;
            this.showDetailsLabel = showDetailsLabel;
            this.detailColumns = detailColumns;
            this.totalColumns = totalColumns;
            bindingSource.Filter = nonZeroDiscrepancyFilter;
        }

        public void SwitchToNonZeroDiscrepancyView()
        {
            RemoveFilter();
            bindingSource.Filter = nonZeroDiscrepancyFilter;
        }
        public void SwitchDetailsView()
        {
            if (showingDetails)
            {
                SetColumnsVisible(detailColumns, false);
                SetColumnsVisible(totalColumns, true);
                showDetailsLabel.Text = "Show " + Form1.ClearingHouseText + " &details";
                showingDetails = false;
            }
            else
            {
                SetColumnsVisible(detailColumns, true);
                SetColumnsVisible(totalColumns, false);
                showDetailsLabel.Text = "Show " + Form1.ClearingHouseText + " &totals";
                showingDetails = true;
            }
        }

        private static void SetColumnsVisible(DataGridViewColumn[] columns, bool visibleValue)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.Visible = visibleValue;
            }
        }
        protected override void ShowLabelsWhenFiltered()
        {
            showNonZeroDiscrepanciesLabel.Visible = (bindingSource.Filter != nonZeroDiscrepancyFilter);
            showAllLabel.Visible = (ReconciliationLib.Utilities.AccountGroupName != null);
            filterStatusLabel.Visible = (ReconciliationLib.Utilities.AccountGroupName != null);
            showDetailsLabel.Visible = (ReconciliationLib.Utilities.AccountGroupName != null);
        }
        protected override void ShowLabelsWhenNotFiltered()
        {
            showNonZeroDiscrepanciesLabel.Visible = (ReconciliationLib.Utilities.AccountGroupName != null);
            showAllLabel.Visible = false;
            filterStatusLabel.Visible = false;
            showDetailsLabel.Visible = (ReconciliationLib.Utilities.AccountGroupName != null);
        }
        public override void SaveRowsToProcess(int rowIndex)
        {
            base.SaveRowsToProcess(rowIndex);

            rowsToProcessContainNonzeroDiscrepancies = false;
            rowsToProcessContainAcceptedDiscrepancies = false;
            foreach (DataGridViewRow rowView in RowsToProcess)
            {
                rowsToProcessContainNonzeroDiscrepancies |= (0f != Discrepancy(rowView));
                rowsToProcessContainAcceptedDiscrepancies |= (0f != Accepted(rowView));
            }
        }

        protected static double Discrepancy(DataGridViewRow rowView)
        {
            DataRowView row = rowView.DataBoundItem as DataRowView;
            return Convert.ToDouble(row["Discrepancy"]);
        }

        protected static double Accepted(DataGridViewRow rowView)
        {
            DataRowView row = rowView.DataBoundItem as DataRowView;
            return Convert.IsDBNull(row["Accepted"]) ? 0f : Convert.ToDouble(row["Accepted"]);
        }

        public bool RowsToProcessContainNonzeroDiscrepancies
        {
            get { return rowsToProcessContainNonzeroDiscrepancies; }
        }

        public bool RowsToProcessContainAcceptedDiscrepancies
        {
            get { return rowsToProcessContainAcceptedDiscrepancies; }
        }
   }
}
