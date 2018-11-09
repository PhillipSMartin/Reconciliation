using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using smw.smtp;
using System.Net.Mail;
using System.Windows.Forms;

using Gargoyle.Messaging;
using com.latencybusters.lbm;
using ReconciliationLib;
using Gargoyle.Middleware;

namespace ReconciliationClient
{

    /// <summary>
    /// Summary description for ReportSender.
    /// </summary>
    /// 
    public class ReportSender
    {
        private string m_reportsDir;
        private HugoDataSet.ConfirmationCorrectionsDataTable m_dataTable;
        private static StringBuilder s_reportBody;
        private const string s_header = "RecordType,ImportDate,TradeDate,TradeReferenceNumber,AccountNumber,UnderlyingSymbol,OptionSymbol,ExpirationDate,StrikePrice,OptionType,TradeType,TradeVolume,TradePrice,ExecutingBroker,Exchange,TradeDate,Correction";
        private const string _hugoOptionHeader = "RcdType,Underlying,Option,ExpirationDate,Strike,C/P,B/S,Quantity,Price,SubAcct,Medium,Correction,TradeDate,UpdateTime";
        private const string _hugoStockHeader = "RcdType,Stock,B/S,Quantity,Price,SubAcct,Medium,Correction,TradeDate,UpdateTime";

        public event EventHandler<LoggingUtilitiesLib.InfoEventArgs> Info;
        public event EventHandler<LoggingUtilitiesLib.ErrorEventArgs> Error;

        private static Dictionary<int, StringBuilder> _tradersMail = new Dictionary<int, StringBuilder>();
        private static Dictionary<int, StringBuilder> _hugoSC = new Dictionary<int, StringBuilder>();
        private static Dictionary<int, StringBuilder> _hugoOC = new Dictionary<int,StringBuilder>(); 

        /// <summary>
        /// Creates a report sender which creates the reports which we send to Pax.
        /// These reports tell Pax which trades we think need to be added or deleted from our accounts.
        /// </summary>
        public ReportSender(HugoDataSet.ConfirmationCorrectionsDataTable dataTable)
        {
            m_dataTable = dataTable;
            if (String.IsNullOrEmpty(Properties.Settings.Default.ReportsDirectory))
            {
                m_reportsDir = Directory.GetCurrentDirectory();
            }
            else
            {
                m_reportsDir = Properties.Settings.Default.ReportsDirectory;
            }
        }

        /// <summary>
        /// Sends trade correction reports to Merrill
        /// </summary>
        /// <returns>True on success</returns>
        public bool SendReports()
        {
            bool success = false;
            string[] reportFilenames;
            try
            {
                reportFilenames = CreateReports( );
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                FireError(String.Format("Could not find directory {0}.", Properties.Settings.Default.ReportsDirectory),
                    new ReconciliationException("Choose 'Edit\\Settings' from the menu and change 'ReportsDirectory' to an existing directory"));
                return success;
            }
            catch (Exception ex)
            {
                FireError("Could not create reports", ex);
                return success;
            }
            try
            {
                EmailReports(reportFilenames);
                success = true;

                if (Properties.Settings.Default.SendReportToTraders)
                {
                    success = EmailTraderReports(_tradersMail, Properties.Settings.Default.MailSubject, "Corrections_");
                    success = EmailTraderReports(_hugoOC, "Hugo Option Corrections", "HugoOptionCorrections_");
                    success = EmailTraderReports(_hugoSC, "Hugo Stock Corrections", "HugoStockCorrections_");
                }
            }
            catch (Exception ex)
            {
                FireError("Could not send reports", ex);
                return success;
            }
            FireInfo("Sending reports completed");
            return success;
        }

        /// <summary>
        /// Creates the reports which should be sent to Pax
        /// </summary>
        /// <returns>An array containing the names of all of the report files</returns>
        private string[] CreateReports()
        {
            List<string> exportList = new List<string>();

            string correctionsFilename = GetFullFilename("Corrections.csv");
            // Make sure the file does not already exist
            if (System.IO.File.Exists(correctionsFilename))
            {
                System.IO.File.Delete(correctionsFilename);
            }

            Export(correctionsFilename, m_dataTable);
            ExportHugoCorrections();
            exportList.Add(correctionsFilename);

            return exportList.ToArray();
        }

        private string GetFullFilename(string filename)
        {
            return m_reportsDir + Path.DirectorySeparatorChar + filename;
        }

        private void ExportHugoCorrections()
        {
            FireInfo("Building Hugo Option Correction Reports...");

            _hugoSC.Clear();
            _hugoOC.Clear();
            // build the hugoStockCorrections and hugoOPtionsCorections
            foreach (HugoDataSet.HugoOptionCorrectionsRow optionRow in Utilities.HugoOptionCorrections)
            {
                int account = Utilities.GetAccountIdFromSubaccountName(optionRow.SubAcctName);

                string outputLine = String.Format("\"{0}\",{1},{2},\"{3:d}\",\"{4}\",\"{5}\",\"{6}\",{7},{8}, {9},\"{10}\",\"{11}\",{12:G},{13:G}",
                    optionRow.RecordType ?? String.Empty,
                    optionRow.UnderlyingSymbol,
                    optionRow.OptionSymbol,
                    optionRow.ExpirationDate,
                    optionRow.StrikePrice,
                    optionRow.OptionType ?? String.Empty,
                    optionRow.TradeTypeName ?? String.Empty,
                    optionRow.TradeVolume,
                    optionRow.TradePrice,
                    optionRow.SubAcctName,
                    optionRow.TradeMediumName,
                    optionRow.DifferenceMsg ?? String.Empty,
                    optionRow.TradeDateTime,
                    optionRow.UpdateTime);

                StringBuilder sOMail = null;
                if (!_hugoOC.TryGetValue(account, out sOMail))
                {
                    sOMail = new StringBuilder();
                    sOMail.AppendLine(_hugoOptionHeader);
                    _hugoOC.Add(account, sOMail);
                }
                sOMail.AppendLine(outputLine);
            }
            FireInfo("Building Hugo Stock Correction Reports...");

            // build the hugoStockCorrections and hugoStockCorections
            foreach (HugoDataSet.HugoStockCorrectionsRow stockRow in Utilities.HugoStockCorrections)
            {
                int account = Utilities.GetAccountIdFromSubaccountName(stockRow.SubAcctName);

                string outputLine = String.Format("\"{0}\",\"{1}\",{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",{8:G},{9:G}",
                    stockRow.RecordType ?? String.Empty,
                    stockRow.StockSymbol,
                    stockRow.TradeTypeName ?? String.Empty,
                    stockRow.TradeVolume,
                    stockRow.TradePrice,
                    stockRow.SubAcctName,
                    stockRow.TradeMediumName,
                    stockRow.DifferenceMsg ?? String.Empty,
                    stockRow.TradeDateTime,
                    stockRow.UpdateTime);

                StringBuilder sMail = null;
                if (!_hugoSC.TryGetValue(account, out sMail))
                {
                    sMail = new StringBuilder();
                    sMail.AppendLine(_hugoStockHeader);
                    _hugoSC.Add(account, sMail);
                }
                sMail.AppendLine(outputLine);
            }
        }

        private static void Export(string filename, HugoDataSet.ConfirmationCorrectionsDataTable table)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            StreamWriter output = new StreamWriter(filename, false, System.Text.Encoding.ASCII);
            string outputLine;

            // start clean, in case you call export multiple times
            _tradersMail.Clear();

            try
            {
                // Write header row
                output.WriteLine(s_header);
                s_reportBody = new StringBuilder();
                s_reportBody.AppendLine(s_header);


                // Write each data row
                foreach (HugoDataSet.ConfirmationCorrectionsRow row in table)
                {
                    if (row.TradeMediumName != "Willy-Nilly")
                    {
                        if (String.IsNullOrEmpty(row.OptionSymbol))
                        {
                            outputLine = BuildStockConfirmationCorrectionLine(row);
                        }
                        else
                        {
                            outputLine = BuildOptionConfirmationCorrectionLine(row);
                        }

                        output.WriteLine(outputLine);
                        s_reportBody.AppendLine(outputLine);

                        // if have to send e-mails to traders
                        if (Properties.Settings.Default.SendReportToTraders && (row.AccountNumber != null))
                        {
                            SendEmailToTraders(outputLine, row);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                output.Close();
            }
        }

        private static string BuildOptionConfirmationCorrectionLine(HugoDataSet.ConfirmationCorrectionsRow row)
        {
            string outputLine;
            outputLine = String.Format("\"{0}\",{1:d},{2:d},\"{3}\",\"{4}\",\"{5}\",\"{6}\",{7:d},{8},\"{9}\",\"{10}\",{11},{12},\"{13}\",\"{14}\",{15:d},\"{16}\"",
                 row.RecordType ?? String.Empty,
                 row.ImportDate,
                 row.TradeDate,
                 row.TradeReferenceNumber ?? String.Empty,
                 row.AccountNumber ?? String.Empty,
                 row.UnderlyingSymbol ?? String.Empty,
                 row.OptionSymbol ?? String.Empty,
                 row.ExpirationDate,
                 row.StrikePrice,
                 row.OptionType ?? String.Empty,
                 row.TradeTypeName ?? String.Empty,
                 row.TradeVolume,
                 row.TradePrice,
                 row.ExecutingBroker ?? String.Empty,
                 row.ExchangeName ?? String.Empty,
                 row.TradeDate,
                 row.GetDifferenceMsg(true) ?? String.Empty);
            return outputLine;
        }

        private static string BuildStockConfirmationCorrectionLine(HugoDataSet.ConfirmationCorrectionsRow row)
        {
            string outputLine;
            outputLine = String.Format("\"{0}\",{1:d},{2:d},\"{3}\",\"{4}\",\"{5}\",,,,, \"{6}\",{7},{8},\"{9}\",\"{10}\",{11:d},\"{12}\"",
                row.RecordType ?? String.Empty,
                row.ImportDate,
                row.TradeDate,
                row.TradeReferenceNumber ?? String.Empty,
                row.AccountNumber ?? String.Empty,
                row.UnderlyingSymbol ?? String.Empty,
                row.TradeTypeName ?? String.Empty,
                row.TradeVolume,
                row.TradePrice,
                row.ExecutingBroker ?? String.Empty,
                row.ExchangeName ?? String.Empty,
                row.TradeDate,
                row.GetDifferenceMsg(true) ?? String.Empty);
            return outputLine;
        }

        private static void SendEmailToTraders(string outputLine, HugoDataSet.ConfirmationCorrectionsRow row)
        {
            // we have to send e-mails by account not subaccount - get the account first
            int account = Utilities.GetAccountIdFromSubaccountName(row.SubAcctName);
            // get the corresponding stringbuilder
            StringBuilder accountMail = null;
            if (!_tradersMail.TryGetValue(account, out accountMail))
            {
                accountMail = new StringBuilder();
                accountMail.AppendLine(s_header);
                _tradersMail.Add(account, accountMail);
            }
            accountMail.AppendLine(outputLine);

        }

        // email 
        public bool EmailTraderReports( Dictionary<int, StringBuilder> emails, string subject, string fileName)
        {
            FireInfo("Email " + subject + " Reports...");
            //load the config settings for the email client
            Gargoyle.Configuration.Configurator.LoadConfigurationFromService("Gargoyle.SMTPServerSettings", false);
            ConfigurationItem m_smtpSettings = Gargoyle.Configuration.Configurator.CurrentConfigurations["Gargoyle.SMTPServerSettings"];
            System.Net.Mail.SmtpClient smtpClient = new SmtpClient(
                m_smtpSettings["ServerAddress"].ValueAsStr,
                m_smtpSettings["Port"].ValueAsInt);

            //create credentials that will be passed to the smtp server
            smtpClient.Credentials = new System.Net.NetworkCredential
                (m_smtpSettings["UserName"].ValueAsStr,
                m_smtpSettings["Password"].ValueAsStr);

            System.Net.Mail.MailMessage mailToSend = null;

            try
            {
                foreach (KeyValuePair<int, StringBuilder> kvp in emails)
                {
                    TraderInfo[] traders = Utilities.GetTradersForAccount(kvp.Key);
                    if (traders == null)
                        continue;
                    mailToSend = new MailMessage();

                    string accountName = Utilities.GetAccountName(kvp.Key);

                    mailToSend.From = new MailAddress(Properties.Settings.Default.MailFrom);
                    mailToSend.Subject = subject + " for account: " + accountName;
                    mailToSend.Body = kvp.Value.ToString();
                    //do not use HTML
                    mailToSend.IsBodyHtml = false;

                    foreach (TraderInfo trader in traders)
                    {
                        //construct the Microsoft MailMessage
                        if( !String.IsNullOrEmpty(trader.email) )
                            mailToSend.To.Add(new MailAddress(trader.email));
                    }
                    // save the file first...
                    string filename = GetFullFilename(fileName + accountName + ".csv");
                    FireInfo("Saving report: " + filename);
                    // Make sure the file does not already exist
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    StreamWriter output = new StreamWriter(filename, false, System.Text.Encoding.ASCII);
                    try
                    {
                        // Write the content
                        output.Write(kvp.Value.ToString());
                    }
                    finally
                    {
                        output.Close();
                    }

                    mailToSend.Attachments.Add(new Attachment(filename));

                    smtpClient.Send(mailToSend);
                    mailToSend.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending e-mail: " + ex.Message);
                if (mailToSend != null)
                {
                    mailToSend.Dispose();
                    mailToSend = null;
                }
                return false;
            }
            return true;
        }


        private void EmailReports(string[] reportFilenames)
        {
            //load the config settings for the email client
            Gargoyle.Configuration.Configurator.LoadConfigurationFromService("Gargoyle.SMTPServerSettings", false);
            ConfigurationItem m_smtpSettings = Gargoyle.Configuration.Configurator.CurrentConfigurations["Gargoyle.SMTPServerSettings"];

            FireInfo("Building email");

            //construct the Microsoft MailMessage
            System.Net.Mail.MailMessage mailToSend = new MailMessage(
                Properties.Settings.Default.MailFrom,
                Properties.Settings.Default.MailTo,
                Properties.Settings.Default.MailSubject,
                s_reportBody.ToString());

            try
            {
                //do not use HTML
                mailToSend.IsBodyHtml = false;

                //construct the smtp client that will send the message
                System.Net.Mail.SmtpClient smtpClient = new SmtpClient(
                    m_smtpSettings["ServerAddress"].ValueAsStr,
                    m_smtpSettings["Port"].ValueAsInt);

                //create credentials that will be passed to the smtp server
                smtpClient.Credentials = new System.Net.NetworkCredential
                    (m_smtpSettings["UserName"].ValueAsStr,
                    m_smtpSettings["Password"].ValueAsStr);

                //for each file in  reportFileNames[],  add it to the MailMessage.Attachments collection
                foreach (string filename in reportFilenames)
                {
                    mailToSend.Attachments.Add(new Attachment(filename));
                }

                string[] toAddresses = Properties.Settings.Default.MailTo.Split(new char[] { ',' });

                //Now set the addresses that should be CC'd.
                foreach (string toAddress in toAddresses)
                {
                    mailToSend.CC.Add(toAddress);
                }

                FireInfo("Sending reports");
                smtpClient.Send(mailToSend);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mailToSend != null)
                {
                    mailToSend.Dispose();
                }
            }
        }


        private void FireInfo(string message)
        {
            if (Info != null)
            {
                Info(this, new LoggingUtilitiesLib.InfoEventArgs("ReportSender", message));
            }
        }
        private void FireError(string message, Exception e)
        {
            if (Error != null)
            {
                Error(this, new LoggingUtilitiesLib.ErrorEventArgs("ReportSender", message, e));
            }
        }
    }
}
