namespace ReconciliationClient
{
    partial class FormImport
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonPositionBrowse = new System.Windows.Forms.Button();
            this.buttonConfirmationBrowse = new System.Windows.Forms.Button();
            this.checkBoxPositions = new System.Windows.Forms.CheckBox();
            this.checkBoxConfirmations = new System.Windows.Forms.CheckBox();
            this.labelPositionResult = new System.Windows.Forms.Label();
            this.labelConfirmationResult = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxTaxlots = new System.Windows.Forms.CheckBox();
            this.buttonTaxlotsBrowse = new System.Windows.Forms.Button();
            this.labelTaxlotsResult = new System.Windows.Forms.Label();
            this.comboBoxTaskNames = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBoxIgnoreTaskName = new System.Windows.Forms.CheckBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonBookkeepingBrowse = new System.Windows.Forms.Button();
            this.buttonDividendsBrowse = new System.Windows.Forms.Button();
            this.checkBoxBookkeeping = new System.Windows.Forms.CheckBox();
            this.checkBoxDividends = new System.Windows.Forms.CheckBox();
            this.labelBookkeepingResult = new System.Windows.Forms.Label();
            this.labelDividendsResult = new System.Windows.Forms.Label();
            this.textBoxDividends = new System.Windows.Forms.TextBox();
            this.textBoxBookkeeping = new System.Windows.Forms.TextBox();
            this.textBoxTaxlots = new System.Windows.Forms.TextBox();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.textBoxConfirmations = new System.Windows.Forms.TextBox();
            this.textBoxPositions = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Positions:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Confirmations:";
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(198, 334);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 4;
            this.buttonImport.Text = "Start Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(307, 334);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Directory:";
            // 
            // buttonPositionBrowse
            // 
            this.buttonPositionBrowse.Location = new System.Drawing.Point(388, 79);
            this.buttonPositionBrowse.Name = "buttonPositionBrowse";
            this.buttonPositionBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonPositionBrowse.TabIndex = 9;
            this.buttonPositionBrowse.Text = "Browse...";
            this.buttonPositionBrowse.UseVisualStyleBackColor = true;
            this.buttonPositionBrowse.Click += new System.EventHandler(this.buttonPositionBrowse_Click);
            // 
            // buttonConfirmationBrowse
            // 
            this.buttonConfirmationBrowse.Location = new System.Drawing.Point(388, 105);
            this.buttonConfirmationBrowse.Name = "buttonConfirmationBrowse";
            this.buttonConfirmationBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirmationBrowse.TabIndex = 10;
            this.buttonConfirmationBrowse.Text = "Browse...";
            this.buttonConfirmationBrowse.UseVisualStyleBackColor = true;
            this.buttonConfirmationBrowse.Click += new System.EventHandler(this.buttonConfirmationBrowse_Click);
            // 
            // checkBoxPositions
            // 
            this.checkBoxPositions.AutoSize = true;
            this.checkBoxPositions.Location = new System.Drawing.Point(469, 82);
            this.checkBoxPositions.Name = "checkBoxPositions";
            this.checkBoxPositions.Size = new System.Drawing.Size(55, 17);
            this.checkBoxPositions.TabIndex = 11;
            this.checkBoxPositions.Text = "Import";
            this.checkBoxPositions.UseVisualStyleBackColor = true;
            this.checkBoxPositions.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // checkBoxConfirmations
            // 
            this.checkBoxConfirmations.AutoSize = true;
            this.checkBoxConfirmations.Location = new System.Drawing.Point(469, 108);
            this.checkBoxConfirmations.Name = "checkBoxConfirmations";
            this.checkBoxConfirmations.Size = new System.Drawing.Size(55, 17);
            this.checkBoxConfirmations.TabIndex = 12;
            this.checkBoxConfirmations.Text = "Import";
            this.checkBoxConfirmations.UseVisualStyleBackColor = true;
            this.checkBoxConfirmations.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // labelPositionResult
            // 
            this.labelPositionResult.Location = new System.Drawing.Point(78, 220);
            this.labelPositionResult.Name = "labelPositionResult";
            this.labelPositionResult.Size = new System.Drawing.Size(426, 17);
            this.labelPositionResult.TabIndex = 13;
            // 
            // labelConfirmationResult
            // 
            this.labelConfirmationResult.Location = new System.Drawing.Point(78, 239);
            this.labelConfirmationResult.Name = "labelConfirmationResult";
            this.labelConfirmationResult.Size = new System.Drawing.Size(426, 17);
            this.labelConfirmationResult.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Taxlots:";
            // 
            // checkBoxTaxlots
            // 
            this.checkBoxTaxlots.AutoSize = true;
            this.checkBoxTaxlots.Location = new System.Drawing.Point(469, 134);
            this.checkBoxTaxlots.Name = "checkBoxTaxlots";
            this.checkBoxTaxlots.Size = new System.Drawing.Size(55, 17);
            this.checkBoxTaxlots.TabIndex = 26;
            this.checkBoxTaxlots.Text = "Import";
            this.checkBoxTaxlots.UseVisualStyleBackColor = true;
            this.checkBoxTaxlots.CheckedChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // buttonTaxlotsBrowse
            // 
            this.buttonTaxlotsBrowse.Location = new System.Drawing.Point(388, 131);
            this.buttonTaxlotsBrowse.Name = "buttonTaxlotsBrowse";
            this.buttonTaxlotsBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonTaxlotsBrowse.TabIndex = 25;
            this.buttonTaxlotsBrowse.Text = "Browse...";
            this.buttonTaxlotsBrowse.UseVisualStyleBackColor = true;
            this.buttonTaxlotsBrowse.Click += new System.EventHandler(this.buttonTaxlotsBrowse_Click);
            // 
            // labelTaxlotsResult
            // 
            this.labelTaxlotsResult.Location = new System.Drawing.Point(78, 258);
            this.labelTaxlotsResult.Name = "labelTaxlotsResult";
            this.labelTaxlotsResult.Size = new System.Drawing.Size(426, 17);
            this.labelTaxlotsResult.TabIndex = 29;
            // 
            // comboBoxTaskNames
            // 
            this.comboBoxTaskNames.FormattingEnabled = true;
            this.comboBoxTaskNames.Location = new System.Drawing.Point(108, 27);
            this.comboBoxTaskNames.Name = "comboBoxTaskNames";
            this.comboBoxTaskNames.Size = new System.Drawing.Size(275, 21);
            this.comboBoxTaskNames.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Task Name:";
            // 
            // checkBoxIgnoreTaskName
            // 
            this.checkBoxIgnoreTaskName.AutoSize = true;
            this.checkBoxIgnoreTaskName.Location = new System.Drawing.Point(390, 30);
            this.checkBoxIgnoreTaskName.Name = "checkBoxIgnoreTaskName";
            this.checkBoxIgnoreTaskName.Size = new System.Drawing.Size(56, 17);
            this.checkBoxIgnoreTaskName.TabIndex = 32;
            this.checkBoxIgnoreTaskName.Text = "Ignore";
            this.checkBoxIgnoreTaskName.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreTaskName.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreTaskName_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Bookkeeping:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 187);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Dividends:";
            // 
            // buttonBookkeepingBrowse
            // 
            this.buttonBookkeepingBrowse.Location = new System.Drawing.Point(388, 157);
            this.buttonBookkeepingBrowse.Name = "buttonBookkeepingBrowse";
            this.buttonBookkeepingBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBookkeepingBrowse.TabIndex = 38;
            this.buttonBookkeepingBrowse.Text = "Browse...";
            this.buttonBookkeepingBrowse.UseVisualStyleBackColor = true;
            this.buttonBookkeepingBrowse.Click += new System.EventHandler(this.buttonBookkeepingBrowse_Click);
            // 
            // buttonDividendsBrowse
            // 
            this.buttonDividendsBrowse.Location = new System.Drawing.Point(388, 183);
            this.buttonDividendsBrowse.Name = "buttonDividendsBrowse";
            this.buttonDividendsBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonDividendsBrowse.TabIndex = 39;
            this.buttonDividendsBrowse.Text = "Browse...";
            this.buttonDividendsBrowse.UseVisualStyleBackColor = true;
            this.buttonDividendsBrowse.Click += new System.EventHandler(this.buttonDividendsBrowse_Click);
            // 
            // checkBoxBookkeeping
            // 
            this.checkBoxBookkeeping.AutoSize = true;
            this.checkBoxBookkeeping.Location = new System.Drawing.Point(469, 160);
            this.checkBoxBookkeeping.Name = "checkBoxBookkeeping";
            this.checkBoxBookkeeping.Size = new System.Drawing.Size(55, 17);
            this.checkBoxBookkeeping.TabIndex = 40;
            this.checkBoxBookkeeping.Text = "Import";
            this.checkBoxBookkeeping.UseVisualStyleBackColor = true;
            this.checkBoxBookkeeping.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // checkBoxDividends
            // 
            this.checkBoxDividends.AutoSize = true;
            this.checkBoxDividends.Location = new System.Drawing.Point(469, 186);
            this.checkBoxDividends.Name = "checkBoxDividends";
            this.checkBoxDividends.Size = new System.Drawing.Size(55, 17);
            this.checkBoxDividends.TabIndex = 41;
            this.checkBoxDividends.Text = "Import";
            this.checkBoxDividends.UseVisualStyleBackColor = true;
            this.checkBoxDividends.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // labelBookkeepingResult
            // 
            this.labelBookkeepingResult.Location = new System.Drawing.Point(78, 277);
            this.labelBookkeepingResult.Name = "labelBookkeepingResult";
            this.labelBookkeepingResult.Size = new System.Drawing.Size(426, 17);
            this.labelBookkeepingResult.TabIndex = 42;
            // 
            // labelDividendsResult
            // 
            this.labelDividendsResult.Location = new System.Drawing.Point(78, 297);
            this.labelDividendsResult.Name = "labelDividendsResult";
            this.labelDividendsResult.Size = new System.Drawing.Size(426, 17);
            this.labelDividendsResult.TabIndex = 43;
            // 
            // textBoxDividends
            // 
            this.textBoxDividends.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "DividendsFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDividends.Location = new System.Drawing.Point(108, 184);
            this.textBoxDividends.Name = "textBoxDividends";
            this.textBoxDividends.Size = new System.Drawing.Size(274, 20);
            this.textBoxDividends.TabIndex = 34;
            this.textBoxDividends.Text = global::ReconciliationClient.Properties.Settings.Default.DividendsFileName;
            // 
            // textBoxBookkeeping
            // 
            this.textBoxBookkeeping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "BookkeepingFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxBookkeeping.Location = new System.Drawing.Point(108, 158);
            this.textBoxBookkeeping.Name = "textBoxBookkeeping";
            this.textBoxBookkeeping.Size = new System.Drawing.Size(274, 20);
            this.textBoxBookkeeping.TabIndex = 33;
            this.textBoxBookkeeping.Text = global::ReconciliationClient.Properties.Settings.Default.BookkeepingFileName;
            // 
            // textBoxTaxlots
            // 
            this.textBoxTaxlots.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "TaxlotsFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxTaxlots.Location = new System.Drawing.Point(108, 132);
            this.textBoxTaxlots.Name = "textBoxTaxlots";
            this.textBoxTaxlots.Size = new System.Drawing.Size(274, 20);
            this.textBoxTaxlots.TabIndex = 27;
            this.textBoxTaxlots.Text = global::ReconciliationClient.Properties.Settings.Default.TaxlotsFileName;
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Location = new System.Drawing.Point(108, 54);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(274, 20);
            this.textBoxDirectory.TabIndex = 6;
            this.textBoxDirectory.Text = global::ReconciliationClient.Properties.Settings.Default.ImportDirectory;
            // 
            // textBoxConfirmations
            // 
            this.textBoxConfirmations.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "ConfirmationFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxConfirmations.Location = new System.Drawing.Point(108, 106);
            this.textBoxConfirmations.Name = "textBoxConfirmations";
            this.textBoxConfirmations.Size = new System.Drawing.Size(274, 20);
            this.textBoxConfirmations.TabIndex = 1;
            this.textBoxConfirmations.Text = global::ReconciliationClient.Properties.Settings.Default.ConfirmationFileName;
            // 
            // textBoxPositions
            // 
            this.textBoxPositions.AcceptsReturn = true;
            this.textBoxPositions.Location = new System.Drawing.Point(108, 80);
            this.textBoxPositions.Name = "textBoxPositions";
            this.textBoxPositions.Size = new System.Drawing.Size(274, 20);
            this.textBoxPositions.TabIndex = 0;
            this.textBoxPositions.Text = global::ReconciliationClient.Properties.Settings.Default.PositionFileName;
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(582, 377);
            this.Controls.Add(this.labelDividendsResult);
            this.Controls.Add(this.labelBookkeepingResult);
            this.Controls.Add(this.checkBoxDividends);
            this.Controls.Add(this.checkBoxBookkeeping);
            this.Controls.Add(this.buttonDividendsBrowse);
            this.Controls.Add(this.buttonBookkeepingBrowse);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDividends);
            this.Controls.Add(this.textBoxBookkeeping);
            this.Controls.Add(this.checkBoxIgnoreTaskName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxTaskNames);
            this.Controls.Add(this.labelTaxlotsResult);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxTaxlots);
            this.Controls.Add(this.checkBoxTaxlots);
            this.Controls.Add(this.buttonTaxlotsBrowse);
            this.Controls.Add(this.labelConfirmationResult);
            this.Controls.Add(this.labelPositionResult);
            this.Controls.Add(this.checkBoxConfirmations);
            this.Controls.Add(this.checkBoxPositions);
            this.Controls.Add(this.buttonConfirmationBrowse);
            this.Controls.Add(this.buttonPositionBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxConfirmations);
            this.Controls.Add(this.textBoxPositions);
            this.Name = "FormImport";
            this.Text = "FormImport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImport_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPositions;
        private System.Windows.Forms.TextBox textBoxConfirmations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonPositionBrowse;
        private System.Windows.Forms.Button buttonConfirmationBrowse;
        private System.Windows.Forms.CheckBox checkBoxPositions;
        private System.Windows.Forms.CheckBox checkBoxConfirmations;
        private System.Windows.Forms.Label labelPositionResult;
        private System.Windows.Forms.Label labelConfirmationResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxTaxlots;
        private System.Windows.Forms.CheckBox checkBoxTaxlots;
        private System.Windows.Forms.Button buttonTaxlotsBrowse;
        private System.Windows.Forms.Label labelTaxlotsResult;
        private System.Windows.Forms.ComboBox comboBoxTaskNames;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBoxIgnoreTaskName;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TextBox textBoxBookkeeping;
        private System.Windows.Forms.TextBox textBoxDividends;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonBookkeepingBrowse;
        private System.Windows.Forms.Button buttonDividendsBrowse;
        private System.Windows.Forms.CheckBox checkBoxBookkeeping;
        private System.Windows.Forms.CheckBox checkBoxDividends;
        private System.Windows.Forms.Label labelBookkeepingResult;
        private System.Windows.Forms.Label labelDividendsResult;
    }
}