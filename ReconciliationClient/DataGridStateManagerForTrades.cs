using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForTrades : DataGridStateManager
    {
        #region Declarations
        private double visibleTradeSum;
        private double selectedTradeSum;
        private double visibleDollarSum;
        private double selectedDollarSum;
        private double visibleCommissionSum;
        private double selectedCommissionSum;
        private double visibleSECFeeSum;
        private double selectedSECFeeSum;
        private double visibleORFFeeSum;
        private double selectedORFFeeSum;
        private bool selectionMade;
        private double rowsToProcessTradeVolume;
        private bool canDistributeTrade;

        protected enum ShowState
        {
            ShowOption,
            ShowStock,
            ShowAll,
            ShowMissingStockPrices,
            ShowFuture
        }
        protected ShowState showState = ShowState.ShowAll;

        protected ToolStripStatusLabel tradeSumLabel;
        #endregion

        #region Constructors
        public DataGridStateManagerForTrades(DataGridView dataGridView,
             EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
             ToolStripStatusLabel filterStatusLabel,
             ToolStripStatusLabel showAllLabel,
             ToolStripStatusLabel tradeSumLabel)
            : base(dataGridView,
            refreshBindingSource,
            filterStatusLabel,
            showAllLabel) 
        {
            this.tradeSumLabel = tradeSumLabel;
            key = "TradeId";

            Form1.SelectedUnderlyingChanged += new EventHandler(OnSelectedUnderlyingChanged);
            Form1.SelectedOptionChanged += new EventHandler(OnSelectedOptionChanged);
            Form1.SelectedFutureChanged += new EventHandler(OnSelectedFutureChanged);
        }
        #endregion

        #region Public methods
        public void ShowAll()
        {
            RemoveFilter();
            showState = ShowState.ShowAll;
            bindingSource.DataSource = Rebind();
        }

        public override void RefreshStatusStrip()
        {
            base.RefreshStatusStrip();
            ShowVisibleSum();
        }
        public void CalculateVisibleTradeSum()
        {
            double currTradeSum;
            double currDollarSum;
            double commissionSum;
            double secFeeSum;
            double orfFeeSum;

            visibleTradeSum = 0f;
            visibleDollarSum = 0f;
            visibleCommissionSum = 0f;
            visibleSECFeeSum = 0f;
            visibleORFFeeSum = 0f;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Visible)
                {
                    GetTradeVolumeAndPrice(row, out currTradeSum, out currDollarSum, out commissionSum, out secFeeSum, out orfFeeSum);

                    visibleTradeSum += currTradeSum;
                    visibleDollarSum += currDollarSum;
                    visibleCommissionSum += commissionSum;
                    visibleSECFeeSum += secFeeSum;
                    visibleORFFeeSum += orfFeeSum;
                }
            }
        }

        public void CalculateSelectedTradeSum()
        {
            double currTradeSum;
            double currDollarSum;
            double commissionSum;
            double secFeeSum;
            double orfFeeSum;

            selectedTradeSum = 0;
            selectedDollarSum = 0f;
            selectedCommissionSum = 0f;
            selectedSECFeeSum = 0f;
            selectedORFFeeSum = 0f;
            selectionMade = false;

            foreach (DataGridViewRow row in VisibleSelectedRows)
            {
                selectionMade = true;
                GetTradeVolumeAndPrice(row, out currTradeSum, out currDollarSum, out commissionSum, out secFeeSum, out orfFeeSum);

                selectedTradeSum += currTradeSum;
                selectedDollarSum += currDollarSum;
                selectedCommissionSum += commissionSum;
                selectedSECFeeSum += secFeeSum;
                selectedORFFeeSum += orfFeeSum;
            }
        }
        // used for status strip
        public string TradeSumMessage
        {
            get
            {
                if (selectionMade)
                    return GetVisibleSumMessage() + GetSelectedSumMessage();
                else
                    return GetVisibleSumMessage();
            }
        }

        private string GetVisibleSumMessage()
        {
            string msg = String.Format("Total trade volume={0}", Math.Round(visibleTradeSum));
            if (visibleCommissionSum != 0)
                msg += String.Format(" comm={0:F2}", visibleCommissionSum);
            if (visibleSECFeeSum != 0)
                msg += String.Format(" SEC={0:F2}", visibleSECFeeSum);
            if (visibleORFFeeSum != 0)
                msg += String.Format(" ORF={0:F2}", visibleORFFeeSum);
            msg += String.Format(" ${0:F2}", visibleDollarSum);
            return msg;
        }
        private string GetSelectedSumMessage()
        {
            string msg = String.Format(", selected volume = {0}", Math.Round(selectedTradeSum));
            if (selectedCommissionSum != 0)
                msg += String.Format(" comm={0:F2}", selectedCommissionSum);
            if (selectedSECFeeSum != 0)
                msg += String.Format(" SEC={0:F2}", selectedSECFeeSum);
            if (selectedORFFeeSum != 0)
                msg += String.Format(" ORF={0:F2}", selectedORFFeeSum);
            msg += String.Format(" ${0:F2}", selectedDollarSum);
            return msg;
     }
        public override void SaveRowsToProcess(int rowIndex)
        {
            base.SaveRowsToProcess(rowIndex);

            canDistributeTrade = true;
            rowsToProcessTradeVolume = 0;
            double transactionSign = 0;

            foreach (DataGridViewRow rowView in RowsToProcess)
            {
                DataRowView dataRow = (DataRowView)rowView.DataBoundItem;

                // save info from first row
                if (transactionSign == 0)
                {
                    transactionSign = Convert.ToDouble(dataRow["TransactionSign"]);
                }
                else
                {
                    // if info doesn't match, can't distribute trade
                    if (transactionSign != Convert.ToDouble(dataRow["TransactionSign"]))
                    {
                        canDistributeTrade = false;
                        return;
                    }
                }

                // keep track of trade volume
                rowsToProcessTradeVolume += transactionSign * Convert.ToDouble(dataRow["TradeVolume"]);
            }
        }

        public bool CanDistributeTrade { get { return canDistributeTrade; } }
        public double RowsToProcessTradeVolume { get { return rowsToProcessTradeVolume; } }
        #endregion

        #region Private methods
        private void ShowVisibleSum()
        {
            if (tradeSumLabel != null)
            {
                if (dataGridView.Rows.Count > 0)
                {
                    CalculateVisibleTradeSum();
                    tradeSumLabel.Visible = true;
                    tradeSumLabel.Text = TradeSumMessage;
                }
                else
                {
                    tradeSumLabel.Visible = false;
                }
            }
        }

        private void GetTradeVolumeAndPrice(DataGridViewRow row, out double tradeSum, out double dollarSum, out double commissionSum, out double secFeeSum, out double orfFeeSum)
        {
            DataRowView rowView = (DataRowView)row.DataBoundItem;
 
            tradeSum = Convert.ToDouble(rowView["TransactionSign"]) * Convert.ToDouble(rowView["TradeVolume"]);
            dollarSum = -Convert.ToDouble(rowView["TransactionSign"]) * Convert.ToDouble(rowView["TotalCost"]);

            commissionSum = 0;
            secFeeSum = 0;
            orfFeeSum = 0;
            if (Convert.ToDouble(rowView["TransactionSign"]) != 0)
            {
                if (ColumnExists("Commission"))
                    commissionSum = Convert.ToDouble(rowView["Commission"]);
 
                if (ColumnExists("SECFee"))
                    secFeeSum = Convert.ToDouble(rowView["SECFee"]);
                else if (ColumnExists("SEC Fee"))
                    secFeeSum = Convert.ToDouble(rowView["SEC Fee"]);
 
                if (ColumnExists("ORFFee"))
                    orfFeeSum = Convert.ToDouble(rowView["ORFFee"]);
             }
        }

        protected virtual bool UsingTotalCost
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Event handlers
        protected virtual void OnSelectedOptionChanged(object sender, EventArgs e)
        {
        }

        protected virtual void OnSelectedUnderlyingChanged(object sender, EventArgs e)
        {
        }
        protected virtual void OnSelectedFutureChanged(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
