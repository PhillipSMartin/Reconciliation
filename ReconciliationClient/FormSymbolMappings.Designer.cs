namespace ReconciliationClient
{
    partial class FormSymbolMappings
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.merrillSymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gargoyleSymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolMappingsDataTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolMappingsDataTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(74, 488);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(179, 488);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.merrillSymbolDataGridViewTextBoxColumn,
            this.gargoyleSymbolDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.symbolMappingsDataTableBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(33, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(263, 432);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dataGridView1_CellContextMenuStripNeeded);
            // 
            // merrillSymbolDataGridViewTextBoxColumn
            // 
            this.merrillSymbolDataGridViewTextBoxColumn.DataPropertyName = "MerrillSymbol";
            this.merrillSymbolDataGridViewTextBoxColumn.HeaderText = "MerrillSymbol";
            this.merrillSymbolDataGridViewTextBoxColumn.Name = "merrillSymbolDataGridViewTextBoxColumn";
            this.merrillSymbolDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gargoyleSymbolDataGridViewTextBoxColumn
            // 
            this.gargoyleSymbolDataGridViewTextBoxColumn.DataPropertyName = "GargoyleSymbol";
            this.gargoyleSymbolDataGridViewTextBoxColumn.HeaderText = "HugoSymbol";
            this.gargoyleSymbolDataGridViewTextBoxColumn.Name = "gargoyleSymbolDataGridViewTextBoxColumn";
            // 
            // symbolMappingsDataTableBindingSource
            // 
            this.symbolMappingsDataTableBindingSource.DataSource = typeof(ReconciliationLib.HugoDataSet.SymbolMappingsDataTable);
            // 
            // FormSymbolMappings
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(329, 524);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "FormSymbolMappings";
            this.Text = "Symbol Mappings";
            this.Load += new System.EventHandler(this.FormSymbolMappings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolMappingsDataTableBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource symbolMappingsDataTableBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn merrillSymbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gargoyleSymbolDataGridViewTextBoxColumn;
    }
}