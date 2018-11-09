using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ReconciliationLib;

namespace ReconciliationClient
{
    public partial class FormSubAccounts : Form
    {
        private bool bNeedToRefresh;
        private bool bChangesMade;

        // form returns DialogResult.OK if any changes were made to the mappings table
        public FormSubAccounts()
        {
            InitializeComponent();
        }

        public void OnClearingHouseChanged()
        {
            merrillAcctNumberDataGridViewTextBoxColumn.HeaderText = String.Format("{0} Account", Form1.ClearingHouseText);
            merrillAcctNumberDataGridViewTextBoxColumn.ToolTipText = String.Format("{0}\'s account number", Form1.ClearingHouseText);
        }

        public bool NeedToRefresh
        {
            set { bNeedToRefresh = value; }
            get { return bNeedToRefresh; }
        }

        private void ReBind()
        {
            dataGridView1.DataSource = Utilities.SubaccountNames;
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool bCanClose = true;
            DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();

            if (changes != null)
            {
                bCanClose = false;
                if (DialogResult.OK == MessageBox.Show("Save all changes?", "Subaccounts", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    bCanClose = Utilities.AcceptSubaccountNameChanges(changes as HugoDataSet.SubaccountNamesDataTable);
                    bChangesMade = true;
                    ReBind();
                }
            }

            if (bCanClose)
            {
                SetDialogResultAndClose();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            bool bCanClose = true;
            DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
            if (changes != null)
            {
                if (DialogResult.OK == MessageBox.Show("Cancel all changes?", "Subaccounts", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    ((DataTable)dataGridView1.DataSource).RejectChanges();
                }
                else
                {
                    bCanClose = false;
                }
            }

            if (bCanClose)
            {
                SetDialogResultAndClose();
            }

        }

        private void SetDialogResultAndClose()
        {
            DialogResult = bChangesMade ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void FormSubAccounts_Load(object sender, EventArgs e)
        {
            if (bNeedToRefresh)
            {
                ReBind();
            }
        }
    }
}