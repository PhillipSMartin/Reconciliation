using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace ReconciliationClient
{
    public sealed class ConvertExt : ReconciliationLib.ConvertExt 
    {
        private ConvertExt() { }

        public static T ToRow<T>(DataGridViewRow rowView) where T : DataRow
        {
            return (rowView.DataBoundItem as DataRowView).Row as T;
        }

    }
}
