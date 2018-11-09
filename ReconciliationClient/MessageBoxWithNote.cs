using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReconciliationClient
{
    public partial class MessageBoxWithNote : Form
    {
        public MessageBoxWithNote(string text, string caption)
        {
            InitializeComponent();

            label1.Text = text;
            Text = caption;
        }

        public string Note
        {
            get
            {
                return String.IsNullOrEmpty(textBox1.Text) ? null : textBox1.Text;
            }
        }

        public static DialogResult Show(string text, string caption, out string note)
        {
            note = null;
            using (MessageBoxWithNote msgBox = new MessageBoxWithNote(text, caption))
            {
                DialogResult result = msgBox.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    note = msgBox.Note;
                }

                return result;
            }
        }
    }
}