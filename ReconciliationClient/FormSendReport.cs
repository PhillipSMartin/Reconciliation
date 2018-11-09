using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ReconciliationLib;
using log4net;

namespace ReconciliationClient
{
	/// <summary>
    /// Summary description for FormSendReport.
	/// </summary>
	public class FormSendReport : System.Windows.Forms.Form
    {
        #region Declarations
        ILog logger;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox textBoxTo;
		private System.Windows.Forms.Label labelTo;
		private System.Windows.Forms.Label labelInfo;
		private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.ToolTip toolTipMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxSubject;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
        private TextBox textBoxFrom;
		private System.ComponentModel.IContainer components;
        #endregion

        #region Contructors and Destructors
        /// <summary>
		/// Creates a form uses a <c>ReportSender</c> to create and send the reports to our clearing house.
		/// These reports tell them which trades we think need to be added or deleted from our account.
		/// </summary>
		/// <param name="reportsDirectory">If null, uses the apps directory</param>
		/// <param name="from">Who the email should come from</param>
		/// <param name="to">Who the email should go to. May be a comma delimited list</param>
		/// <param name="host">SMTP server name</param>
        public FormSendReport(ILog logger)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.logger = logger;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxSubject = new System.Windows.Forms.TextBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(507, 232);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send";
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxTo
            // 
            this.textBoxTo.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "MailTo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxTo.Location = new System.Drawing.Point(64, 48);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(507, 20);
            this.textBoxTo.TabIndex = 1;
            this.textBoxTo.Text = global::ReconciliationClient.Properties.Settings.Default.MailTo;
            this.toolTipMain.SetToolTip(this.textBoxTo, "Add additional email addresses separated by commas");
            // 
            // labelTo
            // 
            this.labelTo.Location = new System.Drawing.Point(8, 48);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(48, 16);
            this.labelTo.TabIndex = 2;
            this.labelTo.Text = "To :";
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInfo.Location = new System.Drawing.Point(24, 136);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(547, 88);
            this.labelInfo.TabIndex = 3;
            // 
            // textBoxSubject
            // 
            this.textBoxSubject.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "MailSubject", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSubject.Location = new System.Drawing.Point(64, 88);
            this.textBoxSubject.Name = "textBoxSubject";
            this.textBoxSubject.Size = new System.Drawing.Size(507, 20);
            this.textBoxSubject.TabIndex = 7;
            this.textBoxSubject.Text = global::ReconciliationClient.Properties.Settings.Default.MailSubject;
            this.toolTipMain.SetToolTip(this.textBoxSubject, "Add additional email addresses separated by commas");
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(8, 16);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(48, 16);
            this.labelFrom.TabIndex = 5;
            this.labelFrom.Text = "From :";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Subject :";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(507, 232);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.Visible = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(427, 232);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReconciliationClient.Properties.Settings.Default, "MailFrom", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxFrom.Location = new System.Drawing.Point(65, 14);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(506, 20);
            this.textBoxFrom.TabIndex = 11;
            this.textBoxFrom.Text = global::ReconciliationClient.Properties.Settings.Default.MailFrom;
            // 
            // FormSendReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(595, 269);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSubject);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonOK);
            this.Name = "FormSendReport";
            this.Text = "Send Reports";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void buttonSend_Click(object sender, System.EventArgs e)
		{
            try
            {
                Enabled = false;
                ReportSender reportSender = new ReportSender(Utilities.ConfirmationCorrectionsToday);
                reportSender.Info += new EventHandler<LoggingUtilitiesLib.InfoEventArgs>(m_reportSender_Info);
                reportSender.Error += new EventHandler<LoggingUtilitiesLib.ErrorEventArgs>(m_reportSender_Error);

                // On success make the OK button visible
                if (reportSender.SendReports())
                {
                    buttonCancel.Visible = false;
                    buttonSend.Visible = false;
                    buttonOK.Visible = true;
                }
                reportSender.Info -= new EventHandler<LoggingUtilitiesLib.InfoEventArgs>(m_reportSender_Info);
                reportSender.Error -= new EventHandler<LoggingUtilitiesLib.ErrorEventArgs>(m_reportSender_Error);
            }
            finally
            {
                Enabled = true;
            }
        }

        private void m_reportSender_Info(object sender, LoggingUtilitiesLib.InfoEventArgs e)
        {
            labelInfo.Text = e.Message;
            if (logger != null)
                logger.Info(e.Message);
        }

        private void m_reportSender_Error(object sender, LoggingUtilitiesLib.ErrorEventArgs e)
        {
            labelInfo.Text = String.Format("{0}: {1}", e.Message, e.Exception.Message);
            if (logger != null)
                logger.Error(e.Message, e.Exception);
        }
    }
}
