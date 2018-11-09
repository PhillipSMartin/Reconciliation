using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Gargoyle.Utils.DataGridViewAutoFilter;

namespace ReconciliationClient
{
    internal class DataGridStateManager
    {
        #region Declarations
        protected DataGridView dataGridView;
        protected BindingSource bindingSource;
        // private because it should always be called via Rebind method
        private EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource;
        protected string key;

        private bool needToRefresh;     // the datasource has changed and the grid needs to be refreshed
        private bool isDirty;           // changes were made to the grid that have not yet been committed to the datasource

        private DataGridViewRow[] rowsToProcess;
        private DataGridViewCell[] cellsToProcess;

        protected ToolStripStatusLabel filterStatusLabel;
        protected ToolStripStatusLabel showAllLabel;

        #endregion

        #region Constructor
        public DataGridStateManager(DataGridView dataGridView,
            EventHandler<RefreshBindingSourceEventArgs> refreshBindingSource,
            ToolStripStatusLabel filterStatusLabel,
            ToolStripStatusLabel showAllLabel)
        {
            if (dataGridView == null)
            {
                throw new ArgumentNullException("dataGridView");
            }
            if (refreshBindingSource == null)
            {
                throw new ArgumentNullException("refreshBindingSource");
            }
            if (filterStatusLabel == null)
            {
                throw new ArgumentNullException("filterStatusLabel");
            }

            this.dataGridView = dataGridView;
            this.bindingSource = dataGridView.DataSource as BindingSource;
            this.refreshBindingSource = refreshBindingSource;
            this.filterStatusLabel = filterStatusLabel;
            this.showAllLabel = showAllLabel;

            // allow access to this object from dataGridView
            dataGridView.Tag = this;

            // allow access to this object from toolstrip
            filterStatusLabel.Owner.Tag = this;
        }
        #endregion

        #region Public properties
        public bool NeedToRefresh
        {
            get { return needToRefresh; }
            set { needToRefresh = value; }
        }
        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }
 
        // when user selects an item from the context menu of a row header, this array is filled with either
        //  the rows from dataGridView.SelectedRows or, if this is empty, the single
        //  row that the user right-clicked on to bring up the context menu
        public DataGridViewRow[] RowsToProcess
        {
            get { return rowsToProcess; }
        }
        // when user selects an item from the context menu of a cell, this array is filled with either
        //  the cells from dataGridView.SelectedCells within the clicked-on column or, if this is empty, the single
        //  cell that the user right-clicked on to bring up the context menu
        public DataGridViewCell[] CellsToProcess
        {
            get { return cellsToProcess; }
        }

        public virtual bool GroupByPrice
        {
            get { return false; }
        }
        #endregion

        #region Public methods
        public void Refresh()
        {
            if (NeedToRefresh)
            {
                // save hidden keys
                List<object> hiddenKeys = GetHiddenKeys();

                // re-bind
                bindingSource.DataSource = Rebind();

                // re-hide keys
                HideKeys(hiddenKeys);
                RefreshStatusStrip();

                NeedToRefresh = false;
                IsDirty = false;
            }
        }

        public virtual void RefreshStatusStrip()
        {
            ShowFilterStatus();
        }

        public virtual void SaveRowsToProcess(int rowIndex)
        {
            rowsToProcess = null;

            // make sure we have at least one selection
            dataGridView.Rows[rowIndex].Selected = true;

            List<DataGridViewRow> rowList = new List<DataGridViewRow>(VisibleSelectedRows);
            rowsToProcess = rowList.ToArray();
        }

        protected IEnumerable<DataGridViewRow> VisibleSelectedRows
        {
            get
            {
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (row.Visible)
                    {
                        yield return row;
                    }
                }
            }
        }
        public void SaveCellsToProcess(int columnIndex, int rowIndex)
        {
            cellsToProcess = null;

            // make sure we have at least one selection
            dataGridView[columnIndex, rowIndex].Selected = true;

            // save list of all selected cells
            List<DataGridViewCell> cells = new List<DataGridViewCell>();
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                if (cell.ColumnIndex == columnIndex)
                    cells.Add(cell);
            }
            cellsToProcess = cells.ToArray();
        }
        public void RemoveFilter()
        {
            // Confirm that the data source is a BindingSource that supports filtering.
            if ((bindingSource.DataSource != null) && bindingSource.SupportsFiltering)
            {
                DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dataGridView);
            }
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Visible = true;
            }
            RefreshStatusStrip();
        }
        public void ReverseFilter()
        {
            // Confirm that the data source is a BindingSource that supports filtering.
            if ((bindingSource.DataSource != null) && bindingSource.SupportsFiltering)
            {
                if (!String.IsNullOrEmpty(bindingSource.Filter))
                {
                    StringBuilder newFilter = new StringBuilder();
                    for(int n = 0; n < bindingSource.Filter.Length; n++)
                    {
                        if (bindingSource.Filter[n] == '=')
                        {
                            newFilter.Append("<>");
                        }
                        else if ((bindingSource.Filter[n] == '<') && (bindingSource.Filter[n + 1] == '>'))
                        {
                            newFilter.Append("=");
                            n++;
                        }
                        else
                        {
                            newFilter.Append(bindingSource.Filter[n]);
                        }
                    }
                    bindingSource.Filter = newFilter.ToString();

                    RefreshStatusStrip();
                }
            }
        }

        public void EndEdit()
        {
            bindingSource.EndEdit();
        }
        #endregion

        #region Private properties
        protected bool AllRowsAreVisible
        {
            get
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.Visible)
                        return false;
                }
                return true;
            }
        }
        #endregion

        #region Private methods
        protected virtual DataTable Rebind()
        {
            return Rebind(new RefreshBindingSourceEventArgs());
        }

        protected DataTable Rebind(RefreshBindingSourceEventArgs e)
        {
            refreshBindingSource(this, e);
            if (dataGridView.Controls.Count > 0)
                dataGridView.Controls[0].Enabled = true;
            if (dataGridView.Controls.Count > 1)
                dataGridView.Controls[1].Enabled = true;
            return e.DataTable;
        }
        protected List<object> GetHiddenKeys()
        {
            List<object> hiddenKeys = new List<object>();
            if (key != null)
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.Visible)
                        hiddenKeys.Add(((DataRowView)row.DataBoundItem)[key]);
                }
            }
            return hiddenKeys;
        }
        protected void HideKeys(List<object> hiddenKeys)
        {
            if (key != null)
            {
                dataGridView.CurrentCell = null;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (hiddenKeys.Contains(((DataRowView)row.DataBoundItem)[key]))
                        row.Visible = false;
                }
            }
        }
        protected void ShowFilterStatus()
        {
            string filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView);

            // set labels when not filtered
            if (string.IsNullOrEmpty(filterStatus) && AllRowsAreVisible)
            {
                ShowLabelsWhenNotFiltered();
            }

            // set labels when filtered
            else
            {
                ShowLabelsWhenFiltered();
                filterStatusLabel.Text = filterStatus;
            }
        }

        protected virtual void ShowLabelsWhenFiltered()
        {
            showAllLabel.Visible = true;
            filterStatusLabel.Visible = true;
        }

        protected virtual void ShowLabelsWhenNotFiltered()
        {
            showAllLabel.Visible = false;
            filterStatusLabel.Visible = false;
        }
        protected bool ColumnExists(string dataPropertyName)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.DataPropertyName.CompareTo(dataPropertyName) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
   }
}
