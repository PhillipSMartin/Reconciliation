using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReconciliationClient
{
    public partial class FormAcceptDiscrepancy : Form
    {
        public FormAcceptDiscrepancy()
        {
            InitializeComponent();
        }

        public string Note
        {
            get { return String.IsNullOrEmpty(textBoxNote.Text) ? null : textBoxNote.Text; }
        }
    }
}