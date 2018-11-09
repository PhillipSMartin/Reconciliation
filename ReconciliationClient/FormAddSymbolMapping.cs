using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReconciliationClient
{
    public partial class FormAddSymbolMapping : Form
    {
        public FormAddSymbolMapping()
        {
            InitializeComponent();
            this.label1.Text = Form1.ClearingHouseText + " Symbol";
        }

        public string MerrillSymbol
        {
            get { return textBoxMerrillSymbol.Text; }
        }
        public string HugoSymbol
        {
            get { return textBoxHugoSymbol.Text; }
        }
    }
}