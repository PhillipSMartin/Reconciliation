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
    public partial class FormSymbolMappings : Form
    {

        private bool bNeedToRefresh;
        private bool bChangesMade;
        private DataGridViewRow[] rowsToProcess;
  
        // form returns DialogResult.OK if any changes were made to the mappings table
        //  return DialogResult.Cancel otherwise
        public FormSymbolMappings()
        {
            InitializeComponent();
        }

        public void OnClearingHouseChanged()
        {
            merrillSymbolDataGridViewTextBoxColumn.HeaderText = Form1.ClearingHouseText + "Symbol";
        }

        public bool NeedToRefresh
        {
            set { bNeedToRefresh = value; }
            get { return bNeedToRefresh; }
        }

        private void ReBind()
        {
            dataGridView1.DataSource = Utilities.SymbolMappings;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            bool bCanClose = true;
            DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
            if (changes != null)
            {
                if (DialogResult.OK == MessageBox.Show("Cancel all changes?", "Symbol Mappings", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool bCanClose = true;
            DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();

            if (changes != null)
            {
                bCanClose = false;
                if (DialogResult.OK == MessageBox.Show("Save all changes?", "Symbol Mappings", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    bCanClose = Utilities.AcceptSymbolMappingChanges(changes as HugoDataSet.SymbolMappingsDataTable);
                    bChangesMade = true;
                    ReBind();
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
         
        private void FormSymbolMappings_Load(object sender, EventArgs e)
        {
            if (bNeedToRefresh)
            {
                ReBind();
            }
        }

        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.ShowImageMargin = false;

            if (e.ColumnIndex < 0)
            {
                SaveRowsToProcess(e.RowIndex);
                menu.Items.Add(new ToolStripMenuItem("&Delete", null, new System.EventHandler(OnDeleteMapping)));
                menu.Items.Add(new ToolStripMenuItem("&New...", null, new System.EventHandler(OnAddMapping)));
            }
            e.ContextMenuStrip = menu;
        }

        private void OnDeleteMapping(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Delete selected symbol mappings(s)?", "Delete Symbol Mappings",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                int deletedMappings = 0;
                foreach (DataGridViewRow rowView in rowsToProcess)
                {
                    HugoDataSet.SymbolMappingsRow row = (rowView.DataBoundItem as DataRowView).Row as HugoDataSet.SymbolMappingsRow;
                    if (row.RowState == DataRowState.Added)
                    {
                        Utilities.SymbolMappings.RemoveSymbolMappingsRow(row);
                    }
                    else if (0 == Utilities.DeleteSymbolMapping(row.MappingId))
                    {
                        deletedMappings++;
                    }
                }

                if (deletedMappings > 0)
                {
                    bChangesMade = true;
                    MessageBox.Show(String.Format("Deleted {0} symbol mapping(s)", deletedMappings), "Delete Symbol Mappings",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReBind();
                }
            }
        }

        private void OnAddMapping(object sender, EventArgs e)
        {
            using (FormAddSymbolMapping formAdd = new FormAddSymbolMapping())
            {
                if (DialogResult.OK == formAdd.ShowDialog())
                {
                    if (String.IsNullOrEmpty(formAdd.MerrillSymbol) || String.IsNullOrEmpty(formAdd.HugoSymbol))
                    {
                        MessageBox.Show("Cannot add an empty symbol", "Add Symbol Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Utilities.SymbolMappings.AddSymbolMappingsRow(formAdd.MerrillSymbol, formAdd.HugoSymbol);
                    }
                }
            }
        }

        private void SaveRowsToProcess(int rowIndex)
        {
            rowsToProcess = null;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                rowsToProcess = new DataGridViewRow[dataGridView1.SelectedRows.Count];
                dataGridView1.SelectedRows.CopyTo(rowsToProcess, 0);
            }
            else
            {
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                if (row != null)
                {
                    rowsToProcess = new DataGridViewRow[] { row };
                }
            }
        }
    }
}