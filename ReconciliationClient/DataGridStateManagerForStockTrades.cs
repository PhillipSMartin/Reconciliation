using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForStockTrades : DataGridStateManagerForTrades
    {
        private ToolStripStatusLabel showUnderlyingLabel;
        private ToolStripStatusLabel groupByPriceLabel;
        private bool groupByPrice;
        private bool[] readOnlyFlags;

        public DataGridStateManagerForStockTrades(DataGridView dataGridView,
             EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
             ToolStripStatusLabel filterStatusLabel,
             ToolStripStatusLabel showUnderlyingLabel,
             ToolStripStatusLabel showAllLabel,
             ToolStripStatusLabel tradeSumLabel,
                ToolStripStatusLabel groupByPriceLabel)
            : base(dataGridView, 
            refreshBindingSource, 
            filterStatusLabel,
            showAllLabel,
            tradeSumLabel) 
        {
            this.showUnderlyingLabel = showUnderlyingLabel;
            this.groupByPriceLabel = groupByPriceLabel;

            SaveReadOnlyColumns();
        }

        #region Public Properties
        public override bool GroupByPrice
        {
            get { return groupByPrice; }
        }
        #endregion

        #region Public Methods
        public void ToggleGroupByPrice()
        {
            groupByPrice = !groupByPrice;
            if (groupByPrice)
            {
                dataGridView.ReadOnly = true;
                if (groupByPriceLabel != null)
                {
                    groupByPriceLabel.Text = "Show individual trades";
                }
            }
            else
            {
                dataGridView.ReadOnly = false;
                RestoreReadOnlyColumns();
                if (groupByPriceLabel != null)
                {
                    groupByPriceLabel.Text = "Group by price";
                }
            }
            bindingSource.DataSource = Rebind();
        }

        public void SwitchToUnderlyingView()
        {
            RemoveFilter();
            if (CanSwitchToUnderlyingView)
            {
                showState = ShowState.ShowStock;
                bindingSource.DataSource = Rebind();
            }
            else
            {
                // this will be done automatically if we set the bindingSource's DataSource
                RefreshStatusStrip();
            }
        }

        #endregion

        #region Private Properties  and Methods
        //protected override bool UsingTotalCost
        //{
        //    get
        //    {
        //        return Utilities.UsingTaxLots && (Utilities.StockPriceDiscrepancyCount == 0);
        //    }
        //}
        private bool CanSwitchToUnderlyingView
        {
            get { return (showState == ShowState.ShowAll) && (Form1.SelectedUnderlying != null); }
        }

        private void SaveReadOnlyColumns()
        {
            readOnlyFlags = new bool[dataGridView.Columns.Count];
            int n = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                readOnlyFlags[n++] = column.ReadOnly;
            }
        }
        private void RestoreReadOnlyColumns()
        {
            int n = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.ReadOnly = readOnlyFlags[n++];
            }
        }
        #endregion

        #region Overrides
        protected override System.Data.DataTable Rebind()
        {
            RefreshBindingSourceEventArgs e;

            switch (showState)
            {
                case ShowState.ShowStock:
                case ShowState.ShowOption:
                    e = new RefreshBindingSourceEventArgs(Form1.SelectedUnderlying, groupByPrice);
                    break;
                case ShowState.ShowAll:
                default:
                    e = new RefreshBindingSourceEventArgs(null, groupByPrice);
                    break;
            }

            return Rebind(e);
        }

        protected override void ShowLabelsWhenNotFiltered()
        {
            showUnderlyingLabel.Visible = CanSwitchToUnderlyingView;
            showAllLabel.Visible = (showState != ShowState.ShowAll);
            filterStatusLabel.Visible = false;
        }

        protected override void ShowLabelsWhenFiltered()
        {
            showUnderlyingLabel.Visible = (Form1.SelectedUnderlying != null);
            showAllLabel.Visible = true;
            filterStatusLabel.Visible = true;
        }
        #endregion

        #region Event handlers
        protected override void OnSelectedUnderlyingChanged(object sender, EventArgs e)
        {
            if (Form1.SelectedUnderlying != null)
            {
                showUnderlyingLabel.Text = String.Format("Show all ({0})", Form1.SelectedUnderlying.UnderlyingSymbol);
                showState = ShowState.ShowStock;
            }
            else
            {
                showState = ShowState.ShowAll;
            }

            NeedToRefresh = true;
            RemoveFilter();
            Refresh();
        }

        protected override void OnSelectedOptionChanged(object sender, EventArgs e)
        {
            if (Form1.SelectedOption != null)
            {
                showUnderlyingLabel.Text = String.Format("Show all ({0})", Form1.SelectedUnderlying.UnderlyingSymbol);
                showState = ShowState.ShowOption;
            }
            else
            {
                showState = ShowState.ShowAll;
            }

            NeedToRefresh = true;
            RemoveFilter();
            Refresh();
        }
        #endregion
    }
}
