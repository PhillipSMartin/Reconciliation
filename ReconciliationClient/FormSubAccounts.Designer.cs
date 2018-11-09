namespace ReconciliationClient
{
    partial class FormSubAccounts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.subaccountNamesDataTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.subAcctNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acctNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.merrillAcctNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subaccountNamesDataTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subAcctNameDataGridViewTextBoxColumn,
            this.acctNameDataGridViewTextBoxColumn,
            this.merrillAcctNumberDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.subaccountNamesDataTableBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(37, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(330, 440);
            this.dataGridView1.TabIndex = 0;
            // 
            // subaccountNamesDataTableBindingSource
            // 
            this.subaccountNamesDataTableBindingSource.DataSource = typeof(ReconciliationLib.HugoDataSet.SubaccountNamesDataTable);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(99, 500);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(230, 500);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // subAcctNameDataGridViewTextBoxColumn
            // 
            this.subAcctNameDataGridViewTextBoxColumn.DataPropertyName = "SubAcctName";
            this.subAcctNameDataGridViewTextBoxColumn.HeaderText = "SubAcct";
            this.subAcctNameDataGridViewTextBoxColumn.Name = "subAcctNameDataGridViewTextBoxColumn";
            this.subAcctNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.subAcctNameDataGridViewTextBoxColumn.ToolTipText = "Hugo\'s subaccount name";
            this.subAcctNameDataGridViewTextBoxColumn.Width = 65;
            // 
            // acctNameDataGridViewTextBoxColumn
            // 
            this.acctNameDataGridViewTextBoxColumn.DataPropertyName = "AcctName";
            this.acctNameDataGridViewTextBoxColumn.HeaderText = "Acct";
            this.acctNameDataGridViewTextBoxColumn.Name = "acctNameDataGridViewTextBoxColumn";
            this.acctNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.acctNameDataGridViewTextBoxColumn.ToolTipText = "Hugo\'s account name";
            // 
            // merrillAcctNumberDataGridViewTextBoxColumn
            // 
            this.merrillAcctNumberDataGridViewTextBoxColumn.DataPropertyName = "MerrillAcctNumber";
            this.merrillAcctNumberDataGridViewTextBoxColumn.HeaderText = "Merrill Account";
            this.merrillAcctNumberDataGridViewTextBoxColumn.Name = "merrillAcctNumberDataGridViewTextBoxColumn";
            this.merrillAcctNumberDataGridViewTextBoxColumn.ToolTipText = "Merrill\'s account number";
            // 
            // FormSubAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 548);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormSubAccounts";
            this.Text = "SubAccounts";
            this.Load += new System.EventHandler(this.FormSubAccounts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subaccountNamesDataTableBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource subaccountNamesDataTableBindingSource;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn subAcctNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn acctNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn merrillAcctNumberDataGridViewTextBoxColumn;
    }
}