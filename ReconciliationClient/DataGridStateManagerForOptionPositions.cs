using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ReconciliationLib;

namespace ReconciliationClient
{
    internal class DataGridStateManagerForOptionPositions : DataGridStateManagerForPositions
    {
        private bool rowsToProcessCanBeMatched;
        private bool rowsToProcessCanBeUnmatched;
        private DateTime? importDate;
        private int recordId;
        private int hugoOptionId;
        private short hugoUnderlyingId;

        public DataGridStateManagerForOptionPositions(DataGridView dataGridView,
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
            showAllLabel,
            showNonZeroDiscrepanciesLabel,
            showDetailsLabel,
            detailColumns,
            totalColumns)
        {
        }

        public bool RowsToProcessCanBeUnmatched
        {
            get { return rowsToProcessCanBeUnmatched; }
        }
        public bool RowsToProcessCanBeMatched
        {
            get { return rowsToProcessCanBeMatched; }
        }
        public DateTime? ImportDate
        {
            get { return importDate; }
        }
        public int RecordId
        {
            get { return recordId; }
        }
        public int HugoOptionId
        {
            get { return hugoOptionId; }
        }
        public short HugoUnderlyingId
        {
            get { return hugoUnderlyingId; }
        }

        public override void SaveRowsToProcess(int rowIndex)
        {
            base.SaveRowsToProcess(rowIndex);

            rowsToProcessCanBeMatched = false;
            rowsToProcessCanBeUnmatched = false;
            importDate = null;
            recordId = -1;
            hugoOptionId = -1;
            hugoUnderlyingId = -1;

            if (RowsToProcess.Length == 2)
            {
                foreach (DataGridViewRow rowView in RowsToProcess)
                {
                    DataRowView row = rowView.DataBoundItem as DataRowView;
                    if (GetOptionId(row) < 0)
                    {
                        importDate = GetImportDate(row);
                        recordId = GetRecordId(row);
                    }
                    else
                    {
                        hugoOptionId = GetOptionId(row);
                        hugoUnderlyingId = GetUnderlyingId(row);
                    }
                }
                rowsToProcessCanBeMatched = (importDate.HasValue) && (recordId >= 0) && (hugoOptionId >= 0);
            }

            else if (RowsToProcess.Length == 1)
            {
                DataRowView row = RowsToProcess[0].DataBoundItem as DataRowView;
                if (GetOptionId(row) >= 0)
                {
                    importDate = GetImportDate(row);
                    recordId = GetRecordId(row);
                    hugoOptionId = GetOptionId(row);
                    hugoUnderlyingId = GetUnderlyingId(row);
                }
                rowsToProcessCanBeUnmatched = (importDate.HasValue) && (recordId >= 0) && (hugoOptionId >= 0);
            }
        }
        protected static int GetOptionId(DataRowView row)
        {
            return Convert.IsDBNull(row["OptionId"]) ? -1 : Convert.ToInt32(row["OptionId"]);
        }
        protected static short GetUnderlyingId(DataRowView row)
        {
            return (short)(Convert.IsDBNull(row["UnderlyingId"]) ? -1 : Convert.ToInt32(row["UnderlyingId"]));
        }
        protected static DateTime? GetImportDate(DataRowView row)
        {
            return Convert.IsDBNull(row["ImportDate"]) ? null : (DateTime?)Convert.ToDateTime(row["ImportDate"]);
        }
        protected static int GetRecordId(DataRowView row)
        {
            return Convert.IsDBNull(row["RecordId"]) ? -1 : Convert.ToInt32(row["RecordId"]);
        }
    }
}
