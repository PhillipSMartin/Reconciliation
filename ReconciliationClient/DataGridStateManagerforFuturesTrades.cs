using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForFuturesTrades : DataGridStateManagerForTrades
    {
        private ToolStripStatusLabel showUnderlyingLabel;
        private ToolStripStatusLabel groupByPriceLabel;
        private bool groupByPrice;
        private bool[] readOnlyFlags;

        public DataGridStateManagerForFuturesTrades(DataGridView dataGridView,
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
                showState = ShowState.ShowFuture;
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
        //        return Utilities.UsingTaxLots && (Utilities.FuturesPriceDiscrepancyCount == 0);
        //    }
        //}
        private bool CanSwitchToUnderlyingView
        {
            get { return (showState == ShowState.ShowAll) && (Form1.SelectedFuture != null); }
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
                case ShowState.ShowFuture:
                    e = new RefreshBindingSourceEventArgs(Form1.SelectedFuture, groupByPrice);
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
            showUnderlyingLabel.Visible = (Form1.SelectedFuture != null);
            showAllLabel.Visible = true;
            filterStatusLabel.Visible = true;
        }
        #endregion

        #region Event handlers
        protected override void OnSelectedFutureChanged(object sender, EventArgs e)
        {
            if (Form1.SelectedFuture != null)
            {
                showUnderlyingLabel.Text = String.Format("Show all ({0})", Form1.SelectedFuture.UnderlyingSymbol);
                showState = ShowState.ShowFuture;
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
