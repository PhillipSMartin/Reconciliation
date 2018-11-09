using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ReconciliationLib;
using GargoyleTaskLib;

namespace ReconciliationClient
{
    public partial class FormImport : Form
    {
        bool bRefreshNeeded;
        bool bImportDateIsToday = (Utilities.ImportDate ?? DateTime.Today) == DateTime.Today;
        bool bTaskStarted;
        bool bTaskFailed;
        string errorMessage;
        TaskUtilities taskUtilities = new TaskUtilities(ReconciliationLib.Utilities.Connection);

        public FormImport()
        {
            InitializeComponent();
            FillComboBoxTaskNames();

            EnableControls();
            Text = "Import for Account Group " + Utilities.AccountGroupName;
        }

        public bool RefreshNeeded { get { return bRefreshNeeded; } }

        private void FillComboBoxTaskNames()
        {
            if (bImportDateIsToday)
            {
                comboBoxTaskNames.DataSource = Utilities.GetTaskNames();
                comboBoxTaskNames.DisplayMember = "TaskName";
                checkBoxIgnoreTaskName.Enabled = true;
                checkBoxIgnoreTaskName.Checked = false;
            }
            else
            {
                checkBoxIgnoreTaskName.Enabled = false;
                checkBoxIgnoreTaskName.Checked = true;
            }
        }

        private static void SaveIfChanged(string settingName, string newSetting)
        {
            if (newSetting.CompareTo(Properties.Settings.Default[settingName]) != 0)
            {
                Properties.Settings.Default[settingName] = newSetting;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            ImportMerrillFiles();
            ReconciliationLib.Utilities.FixOptionIds(Utilities.ClearingHouse.ToString());
        }

        private void EnableControls()
        {
            this.comboBoxTaskNames.Enabled = bImportDateIsToday && !checkBoxIgnoreTaskName.Checked;
 
            this.buttonConfirmationBrowse.Enabled = (Utilities.ClearingHouseInfo.HasConfirmations);
            this.buttonPositionBrowse.Enabled = (Utilities.ClearingHouseInfo.HasPositions);
            this.buttonTaxlotsBrowse.Enabled = (Utilities.ClearingHouseInfo.HasTaxlots);
            this.buttonBookkeepingBrowse.Enabled = (Utilities.ClearingHouseInfo.HasBookkeeping);
            this.buttonDividendsBrowse.Enabled = (Utilities.ClearingHouseInfo.HasDividends);

            this.textBoxConfirmations.Enabled = (Utilities.ClearingHouseInfo.HasConfirmations);
            this.textBoxPositions.Enabled = (Utilities.ClearingHouseInfo.HasPositions);
            this.textBoxTaxlots.Enabled = (Utilities.ClearingHouseInfo.HasTaxlots);
            this.textBoxBookkeeping.Enabled = (Utilities.ClearingHouseInfo.HasBookkeeping);
            this.textBoxDividends.Enabled = (Utilities.ClearingHouseInfo.HasDividends);

            this.checkBoxConfirmations.Enabled = (Utilities.ClearingHouseInfo.HasConfirmations);
            this.checkBoxPositions.Enabled = (Utilities.ClearingHouseInfo.HasPositions);
            this.checkBoxTaxlots.Enabled = (Utilities.ClearingHouseInfo.HasTaxlots);
            this.checkBoxBookkeeping.Enabled = (Utilities.ClearingHouseInfo.HasBookkeeping);
            this.checkBoxDividends.Enabled = (Utilities.ClearingHouseInfo.HasDividends);

            this.buttonImport.Enabled = checkBoxConfirmations.Checked
                || checkBoxPositions.Checked
                || checkBoxTaxlots.Checked
                || checkBoxBookkeeping.Checked
                || checkBoxDividends.Checked;
        }

        private void ImportMerrillFiles()
        {
            int deletedPositions = 0;
            int insertedPositions = 0;
            int deletedConfirmations = 0;
            int insertedConfirmations = 0;
            int deletedTaxlots = 0;
            int insertedTaxlots = 0;
            int deletedBookkeeping = 0;
            int insertedBookkeeping = 0;
            int deletedDividends = 0;
            int insertedDividends = 0;

            labelPositionResult.Text = String.Empty;
            labelConfirmationResult.Text = String.Empty;
            labelTaxlotsResult.Text = String.Empty;
            labelBookkeepingResult.Text = String.Empty;
            labelDividendsResult.Text = String.Empty;

            try
            {
                this.Enabled = false;

                // if import date is not today, check with user
                if (CheckImportDate())
                {
                    // see if we have already done any imports today
                    int numPositions, numConfirmations, numTaxlots, numBookkeepingEntries, numDividends;
                    ReconciliationLib.Utilities.CheckImportState(out numPositions, out numConfirmations, out numTaxlots, out numBookkeepingEntries, out numDividends);

                    // decide what tasks we need to do based on state and user input
                    bool bDeletePositions = false;
                    bool bInsertPositions = checkBoxPositions.Checked;
                    bool bDeleteConfirmations = false;
                    bool bInsertConfirmations = checkBoxConfirmations.Checked;
                    bool bDeleteTaxlots = false;
                    bool bInsertTaxlots = checkBoxTaxlots.Checked;
                    bool bDeleteBookkeeping = false;
                    bool bInsertBookkeeping = checkBoxBookkeeping.Checked;
                    bool bDeleteDividends = false;
                    bool bInsertDividends = checkBoxDividends.Checked;

                    if (bInsertPositions && (numPositions != 0))
                    {
                        HandleImportState(numPositions, ref bDeletePositions, ref bInsertPositions, "position");
                    }
                    if (bInsertConfirmations && (numConfirmations != 0))
                    {
                        HandleImportState(numConfirmations, ref bDeleteConfirmations, ref bInsertConfirmations, "confirmation");
                    }
                    if (bInsertBookkeeping && (numBookkeepingEntries != 0))
                    {
                        HandleImportState(numBookkeepingEntries, ref bDeleteBookkeeping, ref bInsertBookkeeping, "bookkeeping");
                    }
                    if (bInsertDividends && (numDividends != 0))
                     {
                         HandleImportState(numDividends, ref bDeleteDividends, ref bInsertDividends, "dividend");
                     }

                    // do tasks we have decided upon
                     if (bDeletePositions || bInsertPositions || bDeleteConfirmations || bInsertConfirmations ||
                         bDeleteTaxlots || bInsertTaxlots ||
                        bDeleteBookkeeping || bInsertBookkeeping ||
                        bDeleteDividends || bInsertDividends)
                     {
                         bTaskStarted = false;
                         bTaskFailed = false;
                         if (!checkBoxIgnoreTaskName.Checked)
                         {
                             if (0 == taskUtilities.TaskStarted(TaskName, Utilities.ImportDate))
                                bTaskStarted = true;
                         }

                         DoImports(ref deletedPositions, ref insertedPositions, ref deletedConfirmations, ref insertedConfirmations,
                             ref deletedTaxlots, ref insertedTaxlots, ref deletedBookkeeping, ref insertedBookkeeping, ref deletedDividends, ref insertedDividends,
                             bDeletePositions, bInsertPositions, bDeleteConfirmations, bInsertConfirmations,
                             bDeleteTaxlots, bInsertTaxlots, bDeleteBookkeeping, bInsertBookkeeping, bDeleteDividends, bInsertDividends);
                     }
                }

            }
            catch (Exception ex)
            {
                bTaskFailed = true;
                errorMessage = ex.Message;
                OnError("Error during import", ex);
            }

            // populate labels to show what we did
            ShowImportResults(deletedPositions, insertedPositions,
                deletedConfirmations, insertedConfirmations,
                deletedTaxlots, insertedTaxlots,
                deletedBookkeeping, insertedBookkeeping,
                deletedDividends, insertedDividends);
            if (bTaskStarted)
            {
                if (bTaskFailed)
                {
                    taskUtilities.TaskFailed(TaskName, errorMessage);
                }
                else
                {
                    taskUtilities.TaskCompleted(TaskName, CompletionMessage);
                }
            }
            this.Enabled = true;
        }

        private string TaskName
        {
            get
            {
                if (comboBoxTaskNames.SelectedIndex < 0)
                    return null;
                else
                    return ((DataRowView)comboBoxTaskNames.SelectedItem)["TaskName"].ToString();
            }
        }

        private string CompletionMessage
        {
            get
            {

                return String.Format("{0}{1}{2}{3}{4}",
                    labelPositionResult.Text,
                    String.IsNullOrEmpty(labelPositionResult.Text) ? "" : ", ",
                    labelConfirmationResult.Text,
                    String.IsNullOrEmpty(labelConfirmationResult.Text) ? "" : ", ",
                    labelTaxlotsResult.Text);
            }
        }


        private bool CheckImportDate()
        {
            // TODO - have a configuration setting that allows or disallows this check
            if (!bImportDateIsToday)
            {
                return DialogResult.Yes == MessageBox.Show(String.Format("Any records you import will be assigned an import date of {0}.  Are you sure that's what you want?",
                    Utilities.ImportDate), "Import", MessageBoxButtons.YesNo);
            }
            else
            {
                return true;
            }
        }

        private void ShowImportResults(int deletedPositions, int insertedPositions,
            int deletedConfirmations, int insertedConfirmations,
           int deletedTaxlots, int insertedTaxlots,
           int deletedBookkeeping, int insertedBookkeeping,
           int deletedDividends, int insertedDividends)
        {
            labelPositionResult.Text = String.Empty;
            labelConfirmationResult.Text = String.Empty;
            labelTaxlotsResult.Text = String.Empty;

            if (deletedPositions > 0)
            {
                labelPositionResult.Text += String.Format("Deleted {0} position records; ", deletedPositions);
            }
            labelPositionResult.Text += String.Format("Imported {0} position records", insertedPositions);

            if (deletedConfirmations > 0)
            {
                labelConfirmationResult.Text += String.Format("Deleted {0} confirmation records; ", deletedConfirmations);
            }
            labelConfirmationResult.Text += String.Format("Imported {0} confirmation records", insertedConfirmations);

            if (deletedTaxlots > 0)
            {
                labelTaxlotsResult.Text += String.Format("Deleted {0} taxlots; ", deletedTaxlots);
            }
            labelTaxlotsResult.Text += String.Format("Imported {0} taxlots", insertedTaxlots);

            if (deletedBookkeeping > 0)
            {
                labelBookkeepingResult.Text += String.Format("Deleted {0} bookkeeping records; ", deletedBookkeeping);
            }
            labelBookkeepingResult.Text += String.Format("Imported {0} bookkeeping records", insertedBookkeeping);

            if (deletedDividends > 0)
            {
                labelDividendsResult.Text += String.Format("Deleted {0} dividends; ", deletedDividends);
            }
            labelDividendsResult.Text += String.Format("Imported {0} dividends", insertedDividends);
        }

        private void DoImports(ref int deletedPositions, ref int insertedPositions, ref int deletedConfirmations, ref int insertedConfirmations,
            ref int deletedTaxlots, ref int insertedTaxlots, ref int deletedBookkeeping, ref int insertedBookkeeping, ref int deletedDividends, ref int insertedDividends,
            bool bDeletePositions, bool bInsertPositions, bool bDeleteConfirmations, bool bInsertConfirmations,
             bool bDeleteTaxlots, bool bInsertTaxlots, bool bDeleteBookkeeping, bool bInsertBookkeeping, bool bDeleteDividends, bool bInsertDividends)
        {
            if (bDeletePositions)
            {
                deletedPositions = Utilities.DeleteTodaysPositions(Utilities.ClearingHouse);
            }
            if (bInsertPositions)
            {
                labelPositionResult.Text = "Importing positions...";
                Refresh();
                insertedPositions = Utilities.ImportPositions(textBoxDirectory.Text + "\\" + textBoxPositions.Text, Utilities.ClearingHouse);
            }
            if (bDeleteConfirmations)
            {
                deletedConfirmations = Utilities.DeleteTodaysConfirmations(Utilities.ClearingHouse);
            }
            if (bInsertConfirmations)
            {
                labelConfirmationResult.Text = "Importing confirmations...";
                Refresh();
                insertedConfirmations = Utilities.ImportConfirmations(textBoxDirectory.Text + "\\" + textBoxConfirmations.Text, Utilities.ClearingHouse);
            }
              if (bDeleteTaxlots)
            {
                deletedTaxlots = Utilities.DeleteTodaysTaxlots(Utilities.ClearingHouse);
            }
            if (bInsertTaxlots)
            {
                labelTaxlotsResult.Text = "Importing taxlots...";
                Refresh();
                insertedTaxlots = Utilities.ImportTaxlots(textBoxDirectory.Text + "\\" + textBoxTaxlots.Text, Utilities.ClearingHouse);
            }
            if (bDeleteBookkeeping)
            {
                deletedBookkeeping = Utilities.DeleteTodaysBookkeeping(Utilities.ClearingHouse);
            }
            if (bInsertBookkeeping)
            {
                labelConfirmationResult.Text = "Importing Bookkeeping...";
                Refresh();
                insertedBookkeeping = Utilities.ImportBookkeeping(textBoxDirectory.Text + "\\" + textBoxBookkeeping.Text, Utilities.ClearingHouse);
            }
            if (bDeleteDividends)
            {
                deletedDividends = Utilities.DeleteTodaysDividends(Utilities.ClearingHouse);
            }
            if (bInsertDividends)
            {
                labelConfirmationResult.Text = "Importing Dividends...";
                Refresh();
                insertedDividends = Utilities.ImportDividends(textBoxDirectory.Text + "\\" + textBoxDividends.Text, Utilities.ClearingHouse);
            }

            bRefreshNeeded |= (deletedPositions + insertedPositions + deletedConfirmations + insertedConfirmations  + deletedTaxlots + insertedTaxlots + deletedBookkeeping + insertedBookkeeping + deletedDividends + insertedDividends) > 0;
        }

        private static void HandleImportState(int numRecords, ref bool bDelete, ref bool bInsert, string recordType)
        {
            if (numRecords != 0)
            {
                switch (MessageBox.Show(String.Format("We have already imported {0} {1} records today.  Do you wish to overwrite them?",
                    numRecords, recordType), 
                    String.Format("Import {0}", recordType), 
                    MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        bDelete = true;
                        bInsert = true;
                        break;

                    case DialogResult.Cancel:
                        bDelete = false;
                        bInsert = false;
                        break;

                    case DialogResult.No:
                        switch (MessageBox.Show(String.Format("Do you wish to append the {0} records from the file you specified?",
                            recordType),
                            String.Format("Import {0}", recordType),
                            MessageBoxButtons.YesNoCancel))
                        {
                            case DialogResult.Yes:
                                bDelete = false;
                                bInsert = true;
                                break;

                            case DialogResult.Cancel:
                            case DialogResult.No:
                                bDelete = false;
                                bInsert = false;
                                break;
                        }
                        break;

                }
            }
        }

        private void buttonConfirmationBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.RestoreDirectory = true;
                    dlg.InitialDirectory = textBoxDirectory.Text;
                    dlg.Filter = String.Format("Trade files ({0})|{0}|All files (*.*)|*.*",
                       Utilities.ClearingHouseInfo.ConfirmationFileTemplate);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        textBoxDirectory.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                        textBoxConfirmations.Text = System.IO.Path.GetFileName(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError("Error browsing for confirmation file", ex);
            }

        }

        private void buttonPositionBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.RestoreDirectory = true;
                    dlg.InitialDirectory = textBoxDirectory.Text;
                    dlg.Filter = String.Format("Position files ({0})|{0}|All files (*.*)|*.*",
                       Utilities.ClearingHouseInfo.PositionFileTemplate);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        textBoxDirectory.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                        textBoxPositions.Text = System.IO.Path.GetFileName(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError("Error browsing for position file", ex);
            }

        }

        private void buttonBookkeepingBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.RestoreDirectory = true;
                    dlg.InitialDirectory = textBoxDirectory.Text;
                    dlg.Filter = String.Format("Bookkeeping files ({0})|{0}|All files (*.*)|*.*",
                       Utilities.ClearingHouseInfo.BookkeepingFileTemplate);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        textBoxDirectory.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                        textBoxBookkeeping.Text = System.IO.Path.GetFileName(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError("Error browsing for bookkeeping file", ex);
            }

        }

        private void buttonDividendsBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.RestoreDirectory = true;
                    dlg.InitialDirectory = textBoxDirectory.Text;
                    dlg.Filter = String.Format("Dividend files ({0})|{0}|All files (*.*)|*.*",
                       Utilities.ClearingHouseInfo.DividendFileTemplate);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        textBoxDirectory.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                        textBoxDividends.Text = System.IO.Path.GetFileName(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError("Error browsing for dividend file", ex);
            }

        }

       private void FormImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveIfChanged("ConfirmationFileName", textBoxConfirmations.Text);
            SaveIfChanged("PositionFileName", textBoxPositions.Text);
            SaveIfChanged("TaxlotsFileName", textBoxTaxlots.Text);
            SaveIfChanged("ImportDirectory", textBoxDirectory.Text);
        }

        private static void OnError(string message, Exception e)
        {
            Form1.Logger.Error(message, e);
            MessageBox.Show(String.Format("{0}: {1}", message, e.Message), "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void buttonTaxlotsBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.RestoreDirectory = true;
                    dlg.InitialDirectory = textBoxDirectory.Text;
                    dlg.Filter = String.Format("Taxlot files ({0})|{0}|All files (*.*)|*.*",
                       Utilities.ClearingHouseInfo.TaxlotFileTemplate);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        textBoxDirectory.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                        textBoxTaxlots.Text = System.IO.Path.GetFileName(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError("Error browsing for taxlots file", ex);
            }

        }

        private void checkBoxIgnoreTaskName_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
     }
}