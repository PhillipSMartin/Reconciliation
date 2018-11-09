using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ReconciliationLib;
using log4net;
using LoggingUtilitiesLib;

namespace ReconciliationFileViewer
{
    public partial class Form1 : Form
    {
        private IClearingHouse m_clearingHouseInfo;
        private DataSet m_dataSet;
        private BindingSource m_bindingSource;
        private ILog m_logger;
        private bool m_settingsChanged;
        private bool m_needToBindSource;

        public Form1()
        {
            // initialize log4net via app.config
            log4net.Config.XmlConfigurator.Configure();
            m_logger = LogManager.GetLogger(typeof(Form1));

            InitializeComponent();
         }

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
 
                // wire events
                ReconciliationLib.Utilities.Init();
                ReconciliationLib.Utilities.OnError += new EventHandler<LoggingUtilitiesLib.ErrorEventArgs>(Utilities_OnError);
                ReconciliationLib.Utilities.OnInfo += new EventHandler<LoggingUtilitiesLib.InfoEventArgs>(Utilities_OnInfo);
                //ReconciliationLib.Utilities.OnTablesUpdated += new EventHandler<TablesUpdatedEventArgs>(Utilities_OnTablesUpdated);
                Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);

                if (Properties.Settings.Default.UpgradeSettings)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.UpgradeSettings = false;
                }

                // set up Hugo connection
                Gargoyle.Utils.DBAccess.DBAccess dbAccess = Gargoyle.Utils.DBAccess.DBAccess.GetDBAccessOfTheCurrentUser(Properties.Settings.Default.ApplicationName);
                ReconciliationLib.Utilities.Connection = dbAccess.GetConnection(Properties.Settings.Default.Database);

                // set up Liquid connection
                Utilities.LiquidConnectionString = Properties.Settings.Default.LiquidConnectionString;

                Disposed += new EventHandler(Form1_Disposed);

                // update title bar
                Text = String.Format("{0} {1} - {2}",
                     System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                    ReconciliationLib.Utilities.Connection.DataSource.ToUpper());
            }
            catch (Exception ex)
            {
                HandleError("Error loading form", ex);
                Close();
            }
        }
        private void Form1_Disposed(object sender, EventArgs e)
        {
           if ( m_dataSet != null) m_dataSet.Dispose();
           if ( m_bindingSource != null) m_bindingSource.Dispose();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            // see if we have any setting changes
            if (m_settingsChanged && (e.Cancel == false))
            {
                DialogResult result = MessageBox.Show("Do you want to save your settings?", "Reconciliation File Viewer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        Properties.Settings.Default.Save();
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            if (e.Cancel == false)
            {
                Properties.Settings.Default.PropertyChanged -= new PropertyChangedEventHandler(Default_PropertyChanged);
                ReconciliationLib.Utilities.Dispose();

                if (m_dataSet != null)
                    m_dataSet.Dispose();
            }

        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            m_settingsChanged = true;
            Utilities.LiquidConnectionString = Properties.Settings.Default.LiquidConnectionString;
        }

        private void Utilities_OnError(object sender, LoggingUtilitiesLib.ErrorEventArgs e)
        {
            HandleError(e.Message, e.Exception);
        }
        private void Utilities_OnInfo(object sender, LoggingUtilitiesLib.InfoEventArgs e)
        {
            m_logger.Info(e.Message);
        }

        private void radioButtonFile_CheckedChanged(object sender, EventArgs e)
        {
            m_needToBindSource = true;
            EnableControls();
            BindSource();
        }
        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            m_needToBindSource = true;
            ReconciliationLib.Utilities.ImportDate = dateTimePicker1.Value;
            FillComboBoxFileNames();
            BindSource();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.InitialDirectory = Properties.Settings.Default.Database;
                    dialog.Filter = String.Format("{0} ({1})|{1}|All files (*.*)|*.*",
                      SelectedFileType,
                      FileTemplate);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        SelectedFileName = dialog.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error selecting file", ex);
            }
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            m_needToBindSource = true;
            BindSource();
        }

        private void comboBoxClearingHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_needToBindSource = true;
                if (comboBoxClearingHouse.SelectedIndex <= 0)
                {
                    m_clearingHouseInfo = null;
                }
                else
                {
                    m_clearingHouseInfo = ClearingHouseFactory.GetClearingHouse((ClearingHouse)comboBoxClearingHouse.SelectedItem);
                }

                FillComboBoxFileTypes();
            }
            catch (Exception ex)
            {
                HandleError("Error changing clearing house selection", ex);
            }
            finally
            {
                EnableControls();
                BindSource();
            }
        }

        private void comboBoxFileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_needToBindSource = true;
                FillComboBoxFileNames();
            }
            catch (Exception ex)
            {
                HandleError("Error changing file type selection", ex);
            }
            finally
            {
                EnableControls();
                BindSource();
            }

        }

        private void comboBoxFileNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_needToBindSource = true;
                FillComboBoxRows();
            }
            catch (Exception ex)
            {
                HandleError("Error changing file name selection", ex);
            }
            finally
            {
                EnableControls();
                BindSource();
            }
        }

        private void comboBoxRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_needToBindSource = true;
            EnableControls();
            BindSource();
         }

         private void buttonFixIds_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Fix option ids?", "Reconciliation File Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int rc = ReconciliationLib.Utilities.FixOptionIds(SelectedClearingHouse.ToString());
                MessageBox.Show(String.Format("Return code {0} from FixOptionIds", rc), "Reconciliation File Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
         private void buttonUpdateHugo_Click(object sender, EventArgs e)
         {
             try
             {
                 DataTable changes = null;
                 int rowsToChange = 0;
                 if (dataGridView1.DataSource != null)
                 {
                     changes = ((DataTable)dataGridView1.DataSource).GetChanges();
                     if (changes != null)
                         rowsToChange = changes.Rows.Count;
                 }
                 if (rowsToChange <= 0)
                 {
                     MessageBox.Show("No updates to perform", "Update Hugo", MessageBoxButtons.OK);
                 }
                 else if (DialogResult.Yes == MessageBox.Show(String.Format("Update {0} records?", rowsToChange), "Update Hugo", MessageBoxButtons.YesNo))
                 {
                     buttonUpdateHugo.Enabled = false;
                     switch (SelectedFileType)
                     {
                         case FileType.Positions:
                             Utilities.UpdatePositionRecords(SelectedClearingHouse, changes);
                              break;
                         case FileType.Confirmations:
                             Utilities.UpdateConfirmationRecords(SelectedClearingHouse, changes);
                             break;
                          default:
                             MessageBox.Show(String.Format("Updates not supported for file type {0}", SelectedFileType.ToString()),
                                 "Update Hugo", MessageBoxButtons.OK);
                            break;
                     }
                 }
             }
             catch (Exception ex)
             {
                 HandleError("Error updating Hugo", ex);
             }
             finally
             {
                 m_needToBindSource = true;
                 EnableControls();
                 BindSource();
             }

         }

        private void buttonInsertTrades_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Enumerators
        private static IEnumerable<T> GetConfirmationRecords<T>(IConfirmationRecordCollection confirmationRecords) where T : ConfirmationRecord
        {
            ConfirmationRecord confirmationRecord = confirmationRecords.NextRecord;
            while (confirmationRecord != null)
            {
                yield return (T)confirmationRecord;
                confirmationRecord = confirmationRecords.NextRecord;
            }
        }
        private static IEnumerable<T> GetPositionRecords<T>(IPositionRecordCollection positionRecords) where T : PositionRecord
        {
            PositionRecord positionRecord = positionRecords.NextRecord;
            while (positionRecord != null)
            {
                yield return (T)positionRecord;
                positionRecord = positionRecords.NextRecord;
            }
        }
        private static IEnumerable<T> GetTaxlotRecords<T>(ITaxlotRecordCollection taxlotRecords) where T : TaxlotRecord
        {
            TaxlotRecord taxlotRecord = taxlotRecords.NextRecord;
            while (taxlotRecord != null)
            {
                yield return (T)taxlotRecord;
                taxlotRecord = taxlotRecords.NextRecord;
            }
        }
        #endregion

        #region Helper Functions
        private void InitializeControls()
        {
            try
            {
                // set up clearing house combobox
                FillComboBoxClearingHouses();
            }
            catch (Exception ex)
            {
                HandleError("Error initializing controls", ex);
            }
        }

        private void FillComboBoxClearingHouses()
        {
            try
            {
                comboBoxClearingHouse.Enabled = false;
                comboBoxClearingHouse.Items.Clear();

                comboBoxClearingHouse.Items.AddRange(
                    new object[]
                    {
                        "<Select>",
                        ClearingHouse.BONY,
                        ClearingHouse.IB,
                        ClearingHouse.MorganStanley,
                        ClearingHouse.Pershing,
                        ClearingHouse.TCW,
                        ClearingHouse.WellsFargo
                    }
                );

                comboBoxClearingHouse.SelectedIndex = 0;
                m_clearingHouseInfo = null;
            }
            catch (Exception ex)
            {
                HandleError("Error filling ClearingHouse combo box", ex);
            }
        }


        private void FillComboBoxFileTypes()
        {
            try
            {
                comboBoxFileTypes.Enabled = false;
                comboBoxFileTypes.Items.Clear();
                comboBoxFileTypes.Text = String.Empty;

                if (m_clearingHouseInfo != null)
                {
                    if (m_clearingHouseInfo.HasConfirmations)
                    {
                        comboBoxFileTypes.Items.Add(FileType.Confirmations);
                    }
                    if (m_clearingHouseInfo.HasPositions)
                    {
                        comboBoxFileTypes.Items.Add(FileType.Positions);
                    }
                    if (m_clearingHouseInfo.HasTaxlots)
                    {
                        comboBoxFileTypes.Items.Add(FileType.Taxlots);
                    }
                    if (m_clearingHouseInfo.HasDividends)
                    {
                        comboBoxFileTypes.Items.Add(FileType.Dividends);
                    }
                    if (m_clearingHouseInfo.HasBookkeeping)
                    {
                        comboBoxFileTypes.Items.Add(FileType.Bookkeeping);
                    }
                    if (comboBoxFileTypes.Items.Count > 1)
                    {
                        comboBoxFileTypes.Items.Insert(0, ("<Select>"));
                    }
                }

                if (comboBoxFileTypes.Items.Count > 0)
                {
                    comboBoxFileTypes.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error filling FileType combo box", ex);
            }
        }
        private void FillComboBoxFileNames()
        {
            try
            {
                comboBoxFileNames.Enabled = false;
                comboBoxFileNames.Items.Clear();
                comboBoxFileNames.Text = String.Empty;

                if (SelectedFileType != FileType.None)
                {
                    DataRow[] dataRows = GetFileNames();
                    foreach (DataRow dataRow in dataRows)
                    {
                        comboBoxFileNames.Items.Add(BuildFileName(dataRow));
                    }
                    if (comboBoxFileNames.Items.Count > 0)
                    {
                        if (!comboBoxFileNames.Items.Contains("MySQL"))
                        {
                            comboBoxFileNames.Items.Insert(0, "<Select or Browse>");
                        }
                    }
                    else
                    {
                        comboBoxFileNames.Items.Insert(0, "<Browse>");
                    }
                    comboBoxFileNames.SelectedIndex = 0;
                }
             }
            catch (Exception ex)
            {
                HandleError("Error filling FileName combo box", ex);
            }
        }

        private string BuildFileName(DataRow dataRow)
        {
            return String.Format("{0}{1}{2}{3}{4}",
               dataRow["Directory"],
               String.IsNullOrEmpty(dataRow["FileName"].ToString()) ? "" : "\\",
               String.Format(dataRow["FileName"].ToString(), ImportDate),
               String.IsNullOrEmpty(dataRow["Extension"].ToString()) ? "" : ".",
               dataRow["Extension"]);
        }

        private void FillComboBoxRows()
        {
            try
            {
                comboBoxRows.Enabled = false;
                comboBoxRows.Items.Clear();
                comboBoxRows.Text = String.Empty;

                if (SelectedFileIsXml)
                {
                    comboBoxRows.Items.AddRange(XmlHelper.GetRowNames(SelectedFileName));
                    if (comboBoxRows.Items.Count > 1)
                    {
                        comboBoxRows.Items.Insert(0, ("<Select>"));
                    }
                }

                if (comboBoxRows.Items.Count > 0)
                {
                    comboBoxRows.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error filling row combo box", ex);
            }
        }
        private void EnableControls()
        {
            try
            {
                comboBoxClearingHouse.Enabled = comboBoxClearingHouse.Items.Count > 0;
                comboBoxFileTypes.Enabled = comboBoxFileTypes.Items.Count > 0;
                comboBoxFileNames.Enabled = comboBoxFileNames.Items.Count > 0;
                comboBoxRows.Enabled = comboBoxRows.Items.Count > 0;

                buttonBrowse.Enabled = comboBoxFileNames.Enabled;
                buttonRefresh.Enabled = CanBindSource;
                buttonFixIds.Enabled = (m_clearingHouseInfo != null);
                buttonUpdateHugo.Enabled = radioButtonDatabase.Checked && (SelectedFileType != FileType.None);
                buttonInsertTrades.Enabled = false;
            }
            catch (Exception ex)
            {
                HandleError("Error enabling controls", ex);
            }
        }

        private void HandleError(string msg, Exception e)
        {
            if (m_logger != null)
            {
                m_logger.Error(msg, e);
            }
            MessageBox.Show(String.Format("{0}: {1}", msg, e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DataRow[] GetFileNames()
        {
                DataRow[] dataRows = new DataRow[] { };

             try
            {
               if ((SelectedClearingHouse != ClearingHouse.None) && (SelectedFileType != FileType.None))
                {
                    string expression = String.Format("ClearingHouse = '{0}' AND FileType = '{1}'", SelectedClearingHouse, SelectedFileType);
                    string sortOrder = "SequenceNumber ASC";
                    dataRows = ReconciliationLib.Utilities.ClearingHouseFileNames.Select(expression, sortOrder, DataViewRowState.CurrentRows);
                }
            }
             catch (Exception ex)
             {
                 HandleError("Error getting filenames", ex);
             }
 
            return dataRows;
        }

        //private int GetLiquidConfirmations(StringWriter stream)
        //{
        //    int count = 0;
        //    using (XmlWriter xmlWriter = XmlWriter.Create(stream))
        //    {
        //        try
        //        {
        //            //count = ReconciliationLib.Utilities.GetLiquidConfirmationsFromMySql(ReconciliationLib.Utilities.CalculatePreviousBusinessDay(ImportDate), xmlWriter);
        //        }
        //        catch (Exception e)
        //        {
        //            HandleError("Error importing Liquid confirmations from MySql", e);
        //            count = 0;
        //        }
        //    }
        //    return count;
        //}
        //private int GetLiquidPositions(StringWriter stream)
        //{
        //    int count = 0;
        //    using (XmlWriter xmlWriter = XmlWriter.Create(stream))
        //    {
        //        try
        //        {
        //            //count = ReconciliationLib.Utilities.GetLiquidPositionsFromMySql(ReconciliationLib.Utilities.CalculatePreviousBusinessDay(ImportDate), xmlWriter);
        //        }
        //        catch (Exception e)
        //        {
        //            HandleError("Error importing Liquid positions from MySql", e);
        //            count = 0;
        //        }
        //    }
        //    return count;
        //}
        #endregion

        #region Properties
        private DateTime? LastImportDate
        {
            set
            {
                if (value.HasValue)
                {
                    labelLastImportDate.Text = value.Value.ToShortDateString();
                    labelLastImportDate.Visible = true;
                }
                else
                {
                    labelLastImportDate.Visible = false;
                }
            }
        }
        private DateTime ImportDate
        {
            get { return ReconciliationLib.Utilities.ImportDate.Value; }
        }
        private SourceType SelectedSourceType
        {
            get { return radioButtonFile.Checked ? SourceType.File : SourceType.Database; } 
        }
        private bool CanBindSource
        {
            get
            {
                switch (SelectedSourceType)
                {
                    case SourceType.File:
                        if (string.IsNullOrEmpty(SelectedFileName))
                            return false;
                        if ((comboBoxRows.Items.Count > 0) && String.IsNullOrEmpty(SelectedRowName))
                            return false;
                        return true;

                    case SourceType.Database:
                        return SelectedFileType != FileType.None;
                }

                return false;
            }
        }

        private string SelectedFileName
        {
            get 
            {
                if (comboBoxFileNames.Text.StartsWith("<"))
                    return null;
                else
                    return comboBoxFileNames.Text; 
            }
            set
            {
                comboBoxFileNames.SelectedIndex = -1;
                if (comboBoxFileNames.Items.Count > 0)
                    comboBoxFileNames.Items.RemoveAt(0);
                comboBoxFileNames.Items.Insert(0, value);
                comboBoxFileNames.SelectedIndex = 0;
             }
        }
        private string SelectedRowName
        {
            get 
            {
                if (comboBoxRows.Text.StartsWith("<"))
                    return null;
                else
                    return comboBoxRows.Text; 
            }
       }

        private ClearingHouse SelectedClearingHouse
        {
             get
            {
                if (comboBoxClearingHouse.SelectedIndex <= 0)
                    return ClearingHouse.None;
                else
                    return (ClearingHouse)comboBoxClearingHouse.SelectedItem;
            }
        }

        private FileType SelectedFileType
        {
            get
            {
                if (comboBoxFileTypes.Text.StartsWith("<"))
                    return FileType.None;
                else if (comboBoxFileTypes.SelectedIndex < 0)
                    return FileType.None;
                else
                    return (FileType)comboBoxFileTypes.SelectedItem;
            }
        }

        private bool SelectedFileIsXml
        {
            get
            {
                if (!String.IsNullOrEmpty(SelectedFileName))
                {
                    if (SelectedFileName.EndsWith(".xml"))
                        return true;
                }
                return false;
            }
        }

        private string FileTemplate
        {
            get
            {
                if (m_clearingHouseInfo != null)
                {
                    switch (SelectedFileType)
                    {
                        case FileType.Positions:
                            return m_clearingHouseInfo.PositionFileTemplate;
                        case FileType.Confirmations:
                            return m_clearingHouseInfo.ConfirmationFileTemplate;
                        case FileType.Taxlots:
                            return m_clearingHouseInfo.TaxlotFileTemplate;
                        case FileType.Dividends:
                            return m_clearingHouseInfo.DividendFileTemplate;
                    }
                }
                return String.Empty;
            }
        }
        #endregion

        #region Bindings
        private void BindSource()
        {
            try
            {
                if (m_needToBindSource)
                {
                    if (CanBindSource)
                    {
                        buttonRefresh.Enabled = false;
                        LastImportDate = null;
                        switch (SelectedFileType)
                        {
                            case FileType.Positions:
                                BindPositionRecords();
                                break;
                            case FileType.Confirmations:
                                BindConfirmationRecords();
                                break;
                            case FileType.Taxlots:
                                BindTaxlotRecords();
                                break;
                            case FileType.Assignments:
                                BindAssignmentRecords();
                                break;
                            case FileType.Bookkeeping:
                                BindBookkeepingRecords();
                                break;
                            case FileType.Dividends:
                                BindDividendRecords();
                                break;
                            default:
                                dataGridView1.DataSource = null;
                                break;
                        }
                        buttonRefresh.Enabled = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                    m_needToBindSource = false;
                }
             }
            catch (Exception ex)
            {
                HandleError("Error binding datasource to grid", ex);
                m_needToBindSource = false;
            }
        }
        private void BindXmlFile(string fileName, string dataMember)
        {
            if (m_dataSet != null)
                m_dataSet.Dispose();
            m_dataSet = new DataSet();
            m_dataSet.ReadXml(fileName);

            dataGridView1.DataSource = m_dataSet;
            dataGridView1.DataMember = dataMember;
        }
  
        private void BindXmlString(string xml, string dataMember)
        {
            if (m_dataSet != null)
                m_dataSet.Dispose();
            m_dataSet = new DataSet();
            byte[] buffer = Encoding.Unicode.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                XmlReader reader = XmlReader.Create(stream);
                m_dataSet.ReadXml(reader);
            }

            dataGridView1.DataSource = m_dataSet;
            dataGridView1.DataMember = dataMember;
        }


        private void BindConfirmationRecords()
        {
            switch (SelectedSourceType)
            {
                case SourceType.File:
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindConfirmationRecords<PershingConfirmationRecord>();
                            break;
                        case ClearingHouse.WellsFargo:
                              BindXmlFile(SelectedFileName, SelectedRowName);
                            break;
                        case ClearingHouse.MorganStanley:
                        case ClearingHouse.BONY:
                        case ClearingHouse.ICBC:
                        case ClearingHouse.IB:
                            string xml = String.Empty;
                            CSVtoXMLConverter.Convert(SelectedFileName, "Confirmations", out xml);
                            BindXmlString(xml, "Row");
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    break;
                case SourceType.Database:
                    DateTime? lastImportDate = null;
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindDataset(ReconciliationLib.Utilities.GetPershingConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.WellsFargo:
                            BindDataset(ReconciliationLib.Utilities.GetWellsFargoConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.MorganStanley:
                            BindDataset(ReconciliationLib.Utilities.GetMorganStanleyConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.Liquid:
                            BindDataset(ReconciliationLib.Utilities.GetLiquidConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.BONY:
                            BindDataset(ReconciliationLib.Utilities.GetBONYConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.ICBC:
                            BindDataset(ReconciliationLib.Utilities.GetICBCConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.IB:
                            BindDataset(ReconciliationLib.Utilities.GetIBConfirmations(ImportDate, null, ref lastImportDate));
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    LastImportDate = lastImportDate;
                    break;
            }

        }

        private void BindDataset(DataTable dataTable)
        {
            dataGridView1.DataSource = dataTable;
        }

        private void BindConfirmationRecords<T>() where T : ConfirmationRecord
        {
            List<T> confirmationRecords = new List<T>(
                 GetConfirmationRecords<T>(m_clearingHouseInfo.GetConfirmationRecordCollection(SelectedFileName)));

            if (m_bindingSource != null)
                m_bindingSource.Dispose();
            m_bindingSource = new BindingSource();
            m_bindingSource.DataSource = confirmationRecords;
            dataGridView1.DataSource = m_bindingSource;
        }

        private void BindPositionRecords()
        {
            switch (SelectedSourceType)
            {
                case SourceType.File:
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindPositionRecords<PershingPositionRecord>();
                            break;
                        case ClearingHouse.WellsFargo:
                            BindXmlFile(SelectedFileName, SelectedRowName);
                            break;
                        case ClearingHouse.MorganStanley:
                        case ClearingHouse.BONY:
                        case ClearingHouse.ICBC:
                        case ClearingHouse.IB:
                            string xml = String.Empty;
                            CSVtoXMLConverter.Convert(SelectedFileName, "Positions", out xml);
                            BindXmlString(xml, "Row");
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    break;
                 case SourceType.Database:
                    DateTime? lastImportDate = null;
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindDataset(ReconciliationLib.Utilities.GetPershingPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.WellsFargo:
                            BindDataset(ReconciliationLib.Utilities.GetWellsFargoPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.MorganStanley:
                            BindDataset(ReconciliationLib.Utilities.GetMorganStanleyPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.Liquid:
                            BindDataset(ReconciliationLib.Utilities.GetLiquidPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.BONY:
                            BindDataset(ReconciliationLib.Utilities.GetBONYPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.ICBC:
                            BindDataset(ReconciliationLib.Utilities.GetICBCPositions(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.IB:
                            BindDataset(ReconciliationLib.Utilities.GetIBPositions(ImportDate, null, ref lastImportDate));
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    LastImportDate = lastImportDate;
                    break;
            }
        }
        private void BindPositionRecords<T>() where T : PositionRecord
        {
            List<T> positionRecords = new List<T>(
                 GetPositionRecords<T>(m_clearingHouseInfo.GetPositionRecordCollection(SelectedFileName)));

            if (m_bindingSource != null)
                m_bindingSource.Dispose();
            m_bindingSource = new BindingSource();
            m_bindingSource.DataSource = positionRecords;
            dataGridView1.DataSource = m_bindingSource;
        }

        private void BindTaxlotRecords()
        {
            switch (SelectedSourceType)
            {
                case SourceType.File:
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindTaxlotRecords<PershingAggregatedTaxlotRecord>();
                            break;
                        case ClearingHouse.TCW:
                            string xml = String.Empty;
                            CSVtoXMLConverter.Convert(SelectedFileName, "Taxlots", out xml);
                            BindXmlString(xml, "Row");
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    break;
                case SourceType.Database:
                    DateTime? lastImportDate = null;
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.Pershing:
                            BindDataset(ReconciliationLib.Utilities.GetPershingTaxlots(ImportDate, null, ref lastImportDate));
                            break;
                        case ClearingHouse.TCW:
                            BindDataset(ReconciliationLib.Utilities.GetTCWTaxlots(ImportDate, null, ref lastImportDate));
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    LastImportDate = lastImportDate;
                    break;
            }
        }
        private void BindTaxlotRecords<T>() where T : TaxlotRecord
        {
            List<T> taxlotRecords = new List<T>(
                 GetTaxlotRecords<T>(m_clearingHouseInfo.GetTaxlotRecordCollection(SelectedFileName)));

            if (m_bindingSource != null)
                m_bindingSource.Dispose();
            m_bindingSource = new BindingSource();
            m_bindingSource.DataSource = taxlotRecords;
            dataGridView1.DataSource = m_bindingSource;
        }

        private void BindAssignmentRecords()
        {
        }

        private void BindBookkeepingRecords()
        {
            switch (SelectedSourceType)
            {
                case SourceType.File:
                    switch (SelectedClearingHouse)
                    {
                         case ClearingHouse.IB:
                            string xml = String.Empty;
                            CSVtoXMLConverter.Convert(SelectedFileName, "Bookkeeping", out xml);
                            BindXmlString(xml, "Row");
                            break;
                         default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    break;
                case SourceType.Database:
                    DateTime? lastImportDate = null;
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.IB:
                            BindDataset(ReconciliationLib.Utilities.GetIBTransfers(ImportDate, null, ref lastImportDate));
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    LastImportDate = lastImportDate;
                    break;
            }
        }
        private void BindDividendRecords()
        {
            switch (SelectedSourceType)
            {
                case SourceType.File:
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.WellsFargo:
                            BindXmlFile(SelectedFileName, SelectedRowName);
                            break;
                        case ClearingHouse.IB:
                            string xml = String.Empty;
                            CSVtoXMLConverter.Convert(SelectedFileName, "Dividends", out xml);
                            BindXmlString(xml, "Row");
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    break;
                case SourceType.Database:
                    DateTime? lastImportDate = null;
                    switch (SelectedClearingHouse)
                    {
                        case ClearingHouse.IB:
                            BindDataset(ReconciliationLib.Utilities.GetIBCorporateActions(ImportDate, null, ref lastImportDate));
                            break;
                        default:
                            dataGridView1.DataSource = null;
                            break;
                    }
                    LastImportDate = lastImportDate;
                    break;
            }
        }
        #endregion

 
     }
}

