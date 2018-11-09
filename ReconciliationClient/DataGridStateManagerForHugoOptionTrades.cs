using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Gargoyle.Utils.DataGridViewAutoFilter;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForHugoOptionTrades : DataGridStateManagerForOptionTrades
    {
        #region Declarations
        private bool canDeconsolidate;

        private List<string> columnsWeCantEditForPackagedTrades = new List<string>(new string[]
        {
            "Trader",
            "Quantity",
            "Price",
            "TradeDate",
            "Reason",
            "Broker",
            "StkPrice",
            "SpecRate",
            "Note"
        });
        #endregion

        #region Constructor
        public DataGridStateManagerForHugoOptionTrades(DataGridView dataGridView,
            EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
            ToolStripStatusLabel filterStatusLabel,
            ToolStripStatusLabel showOptionLabel,
            ToolStripStatusLabel showUnderlyingLabel,
            ToolStripStatusLabel showAllLabel,
            ToolStripStatusLabel tradeSumLabel) :
            base(dataGridView, refreshBindingSource, filterStatusLabel, showOptionLabel, showUnderlyingLabel, showAllLabel, tradeSumLabel)
        {
        }
        #endregion

        #region Public methods
        public void SwitchToMissingStockPricesView()
        {
            RemoveFilter();
            if (CanSwitchToMissingStockPricesView)
            {
                showState = ShowState.ShowMissingStockPrices;
                bindingSource.DataSource = Rebind();
            }
            else
            {
                // this will be done automatically if we set the bindingSource's DataSource
                RefreshStatusStrip();
            }
        }
        public bool CanEditColumn(int rowIndex, int columnIndex)
        {
            if (Convert.ToInt32(dataGridView.Rows[rowIndex].Cells["ConsolidationPackageId"].Value) >= 0)
            {
                string headerText = dataGridView.Columns[columnIndex].HeaderText;
                if (columnsWeCantEditForPackagedTrades.Contains(headerText))
                {
                    dataGridView.Rows[rowIndex].ErrorText = String.Format("Can't edit {0} for a consolidated trade", headerText);
                    return false;
                }
            }

            return true;
        }
        public override void SaveRowsToProcess(int rowIndex)
        {
            base.SaveRowsToProcess(rowIndex);

            canDeconsolidate = false;
            foreach (DataGridViewRow rowView in RowsToProcess)
            {
                DataRowView dataRow = (DataRowView)rowView.DataBoundItem;
                if (Convert.ToInt32(dataRow["ConsolidationPackageId"]) >= 0)
                {
                    canDeconsolidate = true;
                    break;
                }
            }
        }
        #endregion

        #region Public Properties
        public bool CanDeconsolidate
        {
            get { return canDeconsolidate; }
        }
        #endregion

        private bool CanSwitchToMissingStockPricesView
        {
            get { return (showState != ShowState.ShowMissingStockPrices); }
        }

        protected override DataTable Rebind()
        {
            switch (showState)
            {
                case ShowState.ShowMissingStockPrices:
                    return Utilities.OptionTradesWithMissingStockPrices;

                default:
                    return base.Rebind();
            }

        }
    }
}
