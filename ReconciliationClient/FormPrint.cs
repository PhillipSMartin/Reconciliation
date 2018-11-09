using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Gargoyle.Utils.DataGridViewPrinterLib;

namespace ReconciliationClient
{
    public partial class FormPrint : Form
    {
        private DataGridViewPrinter dataGridViewPrinter = new DataGridViewPrinter();
        private CheckBox[] checkBoxes;

        public FormPrint(DataGridView stockPositionsDataGridView,
                DataGridView optionPositionsDataGridView,
                DataGridView futuresPositionsDataGridView,
                DataGridView hugoStockTradesDataGridView,
                DataGridView hugoOptionTradesDataGridView,
                DataGridView hugoFuturesTradesDataGridView,
                DataGridView merrillStockTradesDataGridView,
                DataGridView merrillOptionTradesDataGridView,
                DataGridView merrillFuturesTradesDataGridView,
                DataGridView todaysMerrillCorrectionsDataGridView,
                DataGridView yesterdaysMerrillCorrectionsDataGridView,
                DataGridView hugoStockCorrectionsDataGridView,
                DataGridView hugoOptionCorrectionsDataGridView,
                DataGridView hugoFuturesCorrectionsDataGridView)
        {
            InitializeComponent();

            checkBoxStockPositions.Tag = buttonStockPositions.Tag = stockPositionsDataGridView;
            checkBoxOptionPositions.Tag = buttonOptionPositions.Tag = optionPositionsDataGridView;
            checkBoxFuturesPositions.Tag = buttonFuturesPositions.Tag = futuresPositionsDataGridView;
            checkBoxHugoStockTrades.Tag = buttonHugoStockTrades.Tag = hugoStockTradesDataGridView;
            checkBoxHugoOptionTrades.Tag = buttonHugoOptionTrades.Tag = hugoOptionTradesDataGridView;
            checkBoxHugoFuturesTrades.Tag = buttonHugoFuturesTrades.Tag = hugoFuturesTradesDataGridView;
            checkBoxMerrillStockTrades.Tag = buttonMerrillStockTrades.Tag = merrillStockTradesDataGridView;
            checkBoxMerrillOptionTrades.Tag = buttonMerrillOptionTrades.Tag = merrillOptionTradesDataGridView;
            checkBoxMerrillFuturesTrades.Tag = buttonMerrillFuturesTrades.Tag = merrillFuturesTradesDataGridView;
            checkBoxTodaysMerrillCorrections.Tag = buttonTodaysMerrillCorrections.Tag = todaysMerrillCorrectionsDataGridView;
            checkBoxYesterdaysMerrillCorrections.Tag = buttonYesterdaysMerrillCorrections.Tag = yesterdaysMerrillCorrectionsDataGridView;
            checkBoxHugoStockCorrections.Tag = buttonHugoStockCorrections.Tag = hugoStockCorrectionsDataGridView;
            checkBoxHugoOptionCorrections.Tag = buttonHugoOptionCorrections.Tag = hugoOptionCorrectionsDataGridView;
            checkBoxHugoFuturesCorrections.Tag = buttonHugoFuturesCorrections.Tag = hugoFuturesCorrectionsDataGridView;

            checkBoxes = new CheckBox[] {
                checkBoxStockPositions,
                checkBoxOptionPositions,
                checkBoxFuturesPositions,
                checkBoxHugoStockTrades,
                checkBoxHugoOptionTrades,
                checkBoxHugoFuturesTrades,
                checkBoxMerrillStockTrades,
                checkBoxMerrillOptionTrades,
                checkBoxMerrillFuturesTrades,
                checkBoxTodaysMerrillCorrections,
                checkBoxYesterdaysMerrillCorrections,
                checkBoxHugoStockCorrections,
                checkBoxHugoOptionCorrections,
                checkBoxHugoFuturesCorrections
            };

            checkBoxMerrillStockTrades.Text = String.Format("{0} stock trades", Form1.ClearingHouseText);
            checkBoxMerrillOptionTrades.Text = String.Format("{0} option trades", Form1.ClearingHouseText);
            checkBoxMerrillFuturesTrades.Text = String.Format("{0} futures trades", Form1.ClearingHouseText);
            checkBoxTodaysMerrillCorrections.Text = String.Format("Today\'s {0} corrections", Form1.ClearingHouseText);
            checkBoxYesterdaysMerrillCorrections.Text = String.Format("Yesterday\'s {0} corrections", Form1.ClearingHouseText);

            Disposed += new EventHandler(FormPrint_Disposed);
        }

        private void FormPrint_Disposed(object sender, EventArgs e)
        {
            dataGridViewPrinter.Dispose();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            List<DataGridView> dataGridViews = new List<DataGridView>();
            foreach (CheckBox checkBox in checkBoxes)
            {
                if (checkBox.Checked)
                {
                    dataGridViews.Add(checkBox.Tag as DataGridView);
                }
            }

            if (dataGridViews.Count > 0)
            {
                dataGridViewPrinter.Print(dataGridViews.ToArray());
            }

        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            dataGridViewPrinter.PrintPreview(((Control)sender).Tag as DataGridView);
        }
    }
}