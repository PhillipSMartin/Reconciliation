using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReconciliationClient
{
    public partial class FormEditDateTime : Form
    {
        public FormEditDateTime()
        {
            InitializeComponent();
        }
        public FormEditDateTime(string label) : this()
        {
            label1.Text = label;
        }
        public DateTime NewValue
        {
            get { return dateTimePicker1.Value; }
            set { dateTimePicker1.Value = value; }
        }
        public void SetMinDate(DateTime dateTime)
        {
            dateTimePicker1.MinDate = dateTime;
        }
        public void SetMaxDate(DateTime dateTime)
        {
            dateTimePicker1.MaxDate = dateTime;
        }
    }
}