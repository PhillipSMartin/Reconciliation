using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Gargoyle.Utils.DataGridViewAutoFilter;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForOptionTrades : DataGridStateManagerForTrades
    {
       #region Declarations
        private ToolStripStatusLabel showOptionLabel;
        private ToolStripStatusLabel showUnderlyingLabel;
       #endregion

        #region Constructor
        public DataGridStateManagerForOptionTrades(DataGridView dataGridView,
            EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
            ToolStripStatusLabel filterStatusLabel,
            ToolStripStatusLabel showOptionLabel,
            ToolStripStatusLabel showUnderlyingLabel,
            ToolStripStatusLabel showAllLabel,
            ToolStripStatusLabel tradeSumLabel) :
            base(dataGridView, refreshBindingSource, filterStatusLabel, showAllLabel, tradeSumLabel)
        {
            this.showOptionLabel = showOptionLabel;
            this.showUnderlyingLabel = showUnderlyingLabel;
        }
        #endregion

        #region Public methods
        public void SwitchToOptionView()
        {
            RemoveFilter();
            if (CanSwitchToOptionView)
            {
                showState = ShowState.ShowOption;
                bindingSource.DataSource = Rebind();
            }
            else
            {
                // this will be done automatically if we set the bindingSource's DataSource
                RefreshStatusStrip();
            }
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

        #region Private properties
        private bool CanSwitchToOptionView
        {
            get { return (showState != ShowState.ShowOption) && (Form1.SelectedOption != null); }
        }
        private bool CanSwitchToUnderlyingView
        {
            get { return (showState != ShowState.ShowStock) && ((Form1.SelectedOption != null) || (Form1.SelectedUnderlying != null)); }
        }
        #endregion

        #region Private Methods
        //protected override bool UsingTotalCost
        //{
        //    get
        //    {
        //        return Utilities.UsingTaxLots && (Utilities.OptionPriceDiscrepancyCount == 0);
        //    }
        //}
        protected override DataTable  Rebind()
        {
            RefreshBindingSourceEventArgs e;

            switch (showState)
            {
                case ShowState.ShowOption:
                    e = new RefreshBindingSourceEventArgs(Form1.SelectedUnderlying, Form1.SelectedOption);
                    break;
                case ShowState.ShowStock:
                    e = new RefreshBindingSourceEventArgs(Form1.SelectedUnderlying, null);
                    break;
                case ShowState.ShowAll:
                default:
                    e = new RefreshBindingSourceEventArgs(null, null);
                    break;
            }

            return Rebind(e);
        }
        protected override void ShowLabelsWhenNotFiltered()
        {
            showOptionLabel.Visible = CanSwitchToOptionView;
            showUnderlyingLabel.Visible = CanSwitchToUnderlyingView;
            showAllLabel.Visible = (showState != ShowState.ShowAll);
            filterStatusLabel.Visible = false;
        }

        protected override void ShowLabelsWhenFiltered()
        {
            showOptionLabel.Visible = (Form1.SelectedOption != null);
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
                showOptionLabel.Text = String.Format("Show all ({0})", Form1.SelectedOption.ToString());
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
