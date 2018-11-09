namespace ReconciliationFileViewer
{
    partial class Form1
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
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxClearingHouse = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFileTypes = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxRows = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelLastImportDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.radioButtonDatabase = new System.Windows.Forms.RadioButton();
            this.radioButtonFile = new System.Windows.Forms.RadioButton();
            this.comboBoxFileNames = new System.Windows.Forms.ComboBox();
            this.buttonUpdateHugo = new System.Windows.Forms.Button();
            this.buttonFixIds = new System.Windows.Forms.Button();
            this.buttonInsertTrades = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(636, 74);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Clearing House:";
            // 
            // comboBoxClearingHouse
            // 
            this.comboBoxClearingHouse.FormattingEnabled = true;
            this.comboBoxClearingHouse.Location = new System.Drawing.Point(107, 21);
            this.comboBoxClearingHouse.Name = "comboBoxClearingHouse";
            this.comboBoxClearingHouse.Size = new System.Drawing.Size(122, 21);
            this.comboBoxClearingHouse.TabIndex = 4;
            this.comboBoxClearingHouse.SelectedIndexChanged += new System.EventHandler(this.comboBoxClearingHouse_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File Type:";
            // 
            // comboBoxFileTypes
            // 
            this.comboBoxFileTypes.FormattingEnabled = true;
            this.comboBoxFileTypes.Location = new System.Drawing.Point(107, 49);
            this.comboBoxFileTypes.Name = "comboBoxFileTypes";
            this.comboBoxFileTypes.Size = new System.Drawing.Size(122, 21);
            this.comboBoxFileTypes.TabIndex = 6;
            this.comboBoxFileTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileTypes_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(40, 177);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1177, 526);
            this.dataGridView1.TabIndex = 7;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(1054, 135);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(163, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Row:";
            // 
            // comboBoxRows
            // 
            this.comboBoxRows.FormattingEnabled = true;
            this.comboBoxRows.Location = new System.Drawing.Point(108, 102);
            this.comboBoxRows.Name = "comboBoxRows";
            this.comboBoxRows.Size = new System.Drawing.Size(178, 21);
            this.comboBoxRows.TabIndex = 11;
            this.comboBoxRows.SelectedIndexChanged += new System.EventHandler(this.comboBoxRows_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelLastImportDate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.radioButtonDatabase);
            this.groupBox1.Controls.Add(this.radioButtonFile);
            this.groupBox1.Location = new System.Drawing.Point(728, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 148);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // labelLastImportDate
            // 
            this.labelLastImportDate.AutoSize = true;
            this.labelLastImportDate.Location = new System.Drawing.Point(118, 107);
            this.labelLastImportDate.Name = "labelLastImportDate";
            this.labelLastImportDate.Size = new System.Drawing.Size(162, 13);
            this.labelLastImportDate.TabIndex = 5;
            this.labelLastImportDate.Text = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.labelLastImportDate.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Last Import Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Import Date:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(92, 76);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.CloseUp += new System.EventHandler(this.dateTimePicker1_CloseUp);
            // 
            // radioButtonDatabase
            // 
            this.radioButtonDatabase.AutoSize = true;
            this.radioButtonDatabase.Location = new System.Drawing.Point(21, 53);
            this.radioButtonDatabase.Name = "radioButtonDatabase";
            this.radioButtonDatabase.Size = new System.Drawing.Size(51, 17);
            this.radioButtonDatabase.TabIndex = 1;
            this.radioButtonDatabase.TabStop = true;
            this.radioButtonDatabase.Text = "Hugo";
            this.radioButtonDatabase.UseVisualStyleBackColor = true;
            // 
            // radioButtonFile
            // 
            this.radioButtonFile.AutoSize = true;
            this.radioButtonFile.Checked = true;
            this.radioButtonFile.Location = new System.Drawing.Point(21, 26);
            this.radioButtonFile.Name = "radioButtonFile";
            this.radioButtonFile.Size = new System.Drawing.Size(78, 17);
            this.radioButtonFile.TabIndex = 0;
            this.radioButtonFile.TabStop = true;
            this.radioButtonFile.Text = "Source File";
            this.radioButtonFile.UseVisualStyleBackColor = true;
            this.radioButtonFile.CheckedChanged += new System.EventHandler(this.radioButtonFile_CheckedChanged);
            // 
            // comboBoxFileNames
            // 
            this.comboBoxFileNames.FormattingEnabled = true;
            this.comboBoxFileNames.Location = new System.Drawing.Point(107, 76);
            this.comboBoxFileNames.Name = "comboBoxFileNames";
            this.comboBoxFileNames.Size = new System.Drawing.Size(523, 21);
            this.comboBoxFileNames.TabIndex = 13;
            this.comboBoxFileNames.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileNames_SelectedIndexChanged);
            // 
            // buttonUpdateHugo
            // 
            this.buttonUpdateHugo.Location = new System.Drawing.Point(1054, 21);
            this.buttonUpdateHugo.Name = "buttonUpdateHugo";
            this.buttonUpdateHugo.Size = new System.Drawing.Size(163, 23);
            this.buttonUpdateHugo.TabIndex = 14;
            this.buttonUpdateHugo.Text = "Update Hugo";
            this.buttonUpdateHugo.UseVisualStyleBackColor = true;
            this.buttonUpdateHugo.Click += new System.EventHandler(this.buttonUpdateHugo_Click);
            // 
            // buttonFixIds
            // 
            this.buttonFixIds.Location = new System.Drawing.Point(1054, 59);
            this.buttonFixIds.Name = "buttonFixIds";
            this.buttonFixIds.Size = new System.Drawing.Size(163, 23);
            this.buttonFixIds.TabIndex = 15;
            this.buttonFixIds.Text = "Fix Option Ids";
            this.buttonFixIds.UseVisualStyleBackColor = true;
            this.buttonFixIds.Click += new System.EventHandler(this.buttonFixIds_Click);
            // 
            // buttonInsertTrades
            // 
            this.buttonInsertTrades.Location = new System.Drawing.Point(1054, 97);
            this.buttonInsertTrades.Name = "buttonInsertTrades";
            this.buttonInsertTrades.Size = new System.Drawing.Size(163, 23);
            this.buttonInsertTrades.TabIndex = 16;
            this.buttonInsertTrades.Text = "Insert Trades";
            this.buttonInsertTrades.UseVisualStyleBackColor = true;
            this.buttonInsertTrades.Click += new System.EventHandler(this.buttonInsertTrades_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 734);
            this.Controls.Add(this.buttonInsertTrades);
            this.Controls.Add(this.buttonFixIds);
            this.Controls.Add(this.buttonUpdateHugo);
            this.Controls.Add(this.comboBoxFileNames);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxRows);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxFileTypes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxClearingHouse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxClearingHouse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFileTypes;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxRows;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDatabase;
        private System.Windows.Forms.RadioButton radioButtonFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label labelLastImportDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxFileNames;
        private System.Windows.Forms.Button buttonUpdateHugo;
        private System.Windows.Forms.Button buttonFixIds;
        private System.Windows.Forms.Button buttonInsertTrades;
    }
}

