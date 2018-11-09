using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReconciliationClient
{
    public partial class FormEditText : Form
    {
        VerifyValue verifyValue;

        public FormEditText(object verifyValue)
        {
            if (verifyValue != null)
            {
                this.verifyValue = (VerifyValue)verifyValue;
            }
            InitializeComponent();
        }

        public string NewValue
        {
            get { return textBox1.Text; }
        }

        private void FormEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((verifyValue != null) && (DialogResult == DialogResult.OK))
            {
                string errorMessage = verifyValue(NewValue);
                if (errorMessage != null)
                {
                    MessageBox.Show(errorMessage, "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }
    }
}