using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using Gargoyle.Utils.DataGridViewAutoFilter;
using ReconciliationLib;
using LoggingUtilitiesLib;

namespace ReconciliationClient
{
    internal delegate string VerifyValue(object text);

    public partial class Form1 : Form
    {
        #region Declarations

        #region Constants
        private const string priceDiscrepancyFilter = "CONVERT([PriceDiscrepancy],'System.String')='$'";
        private const string totalCostDiscrepancyFilter = "CONVERT([TotalCostDiscrepancy],'System.String')='$'";
        private const string tradeIdFilter = "CONVERT([TradeId],'System.Int32')={0}";
        private const string optionTradeIdFilter = "CONVERT([OptionTradeId],'System.Int32')={0}";
        private string[] hugoStockTradeTypes = new string[] { "Buy", "Sell", "Sell short", "RecDiv", "PayDiv", "RecInt", "PayInt" };
        private string[] hugoOptionTradeTypes = new string[] { "Buy", "Sell" };
        private string[] optionTypes = new string[] { "Call", "Put" };
        #endregion

        private static string clearingHouseText = "Clearing House";
        private bool settingsChanged;   // set to true if user has changed application settings
        private static ILog logger;     // log4net logger
        private int numPositions, numConfirmations, numTaxlots, numBookkeepingEntries, numDividends;
        private ImportStatusInfo importStatusInfo = new ImportStatusInfo();
        private bool importStatusShown;
        private List<string> accountGroupsForWhichImportWasRunManually = new List<string>();

        private FormPrint formPrint;
        private FormSymbolMappings formSymbolMappings;
        private FormSubAccounts formSubAccounts;
        private ContextMenuStrip menu;

        private static IOption selectedOption;
        private static IUnderlying selectedUnderlying;
        private static IUnderlying selectedFuture;
        private static EventHandler selectedOptionChanged;
        private static EventHandler selectedUnderlyingChanged;
        private static EventHandler selectedFutureChanged;

        private DataGridStateManagerForStockPositions stockPositionsDataGridStateManager;
        private DataGridStateManagerForOptionPositions optionPositionsDataGridStateManager;
        private DataGridStateManagerForFuturesPositions futuresPositionsDataGridStateManager;
        private DataGridStateManagerForStockTrades merrillStockTradesDataGridStateManager;
        private DataGridStateManagerForStockTrades hugoStockTradesDataGridStateManager;
        private DataGridStateManagerForOptionTrades merrillOptionTradesDataGridStateManager;
        private DataGridStateManagerForHugoOptionTrades hugoOptionTradesDataGridStateManager;
        private DataGridStateManagerForFuturesTrades merrillFuturesTradesDataGridStateManager;
        private DataGridStateManagerForFuturesTrades hugoFuturesTradesDataGridStateManager;
        private DataGridStateManager todaysMerrillCorrectionsDataGridStateManager;
        private DataGridStateManager yesterdaysMerrillCorrectionsDataGridStateManager;
        private DataGridStateManager hugoStockCorrectionsDataGridStateManager;
        private DataGridStateManager hugoOptionCorrectionsDataGridStateManager;
        private DataGridStateManager hugoFuturesCorrectionsDataGridStateManager;
 
        // Clipboards

        // DataGridView responsible for a currently executing context-menu comman
        private DataGridView currentDataGridView;
        // Option Key being copied
        private IOption clipboardOption;
        private StockTrade clipboardStockTrade;
        private FuturesTrade clipboardFuturesTrade;
        private OptionTrade clipboardOptionTrade;

        #region Context menu commands
        ToolStripMenuItem acceptFuturesDiscrepancyCommand;
        ToolStripMenuItem acceptOptionDiscrepancyCommand;
        ToolStripMenuItem acceptStockDiscrepancyCommand;
        ToolStripMenuItem addFuturesAdjustmentCommand;
        ToolStripMenuItem addFuturesTradesToHugoCommand;
        ToolStripMenuItem addFuturesTradesToMerrillCommand;
        ToolStripMenuItem addOptionAdjustmentCommand;
        ToolStripMenuItem addOptionTradesToHugoCommand;
        ToolStripMenuItem addOptionTradesToMerrillCommand;
        ToolStripMenuItem addStockAdjustmentCommand;
        ToolStripMenuItem addStockTradesToHugoCommand;
        ToolStripMenuItem addStockTradesToMerrillCommand;
        ToolStripMenuItem addSymbolMappingCommand;
        ToolStripMenuItem changeFuturesSelectionCommand;
        ToolStripMenuItem changeOptionSelectionCommand;
        ToolStripMenuItem changeStockSelectionCommand;
        ToolStripMenuItem clearAcceptedFuturesDiscrepancyCommand;
        ToolStripMenuItem clearAcceptedOptionDiscrepancyCommand;
        ToolStripMenuItem clearAcceptedStockDiscrepancyCommand;
        ToolStripMenuItem consolidateTradesCommand;
        ToolStripMenuItem copyConfirmationCorrectionCommand;
        ToolStripMenuItem copyHugoFuturesCorrectionCommand;
        ToolStripMenuItem copyHugoFuturesTradeCommand;
        ToolStripMenuItem copyHugoOptionCorrectionCommand;
        ToolStripMenuItem copyHugoOptionTradeCommand;
        ToolStripMenuItem copyHugoStockCorrectionCommand;
        ToolStripMenuItem copyHugoStockTradeCommand;
        ToolStripMenuItem copyMerrillFuturesTradeCommand;
        ToolStripMenuItem copyMerrillOptionTradeCommand;
        ToolStripMenuItem copyMerrillStockTradeCommand;
        ToolStripMenuItem copyOptionCommand;
        ToolStripMenuItem deconsolidateTradesCommand;
        ToolStripMenuItem deleteConfirmationCommand;
        ToolStripMenuItem deleteMerrillCorrectionCommand;
        ToolStripMenuItem deleteSymbolMappingCommand;
        ToolStripMenuItem deleteTradeCommand;
        ToolStripMenuItem distributeOptionTradeCommand;
        ToolStripMenuItem distributeStockTradeCommand;
        ToolStripMenuItem editDateTimeCommand;
        ToolStripMenuItem editNoteCommand;
        ToolStripMenuItem editTextCommand;
        ToolStripMenuItem hideCommand;
        ToolStripMenuItem hideDoneCorrectionsCommand;
        ToolStripMenuItem matchFuturesTotalCostCommand;
        ToolStripMenuItem matchFuturesTradesCommand;
        ToolStripMenuItem matchOptionPositionsCommand;
        ToolStripMenuItem matchOptionTotalCostCommand;
        ToolStripMenuItem matchOptionTradesCommand;
        ToolStripMenuItem matchStockTotalCostCommand;
        ToolStripMenuItem matchStockTradesCommand;
        ToolStripMenuItem pasteFuturesTradeCommand;
        ToolStripMenuItem pasteOptionCommand;
        ToolStripMenuItem pasteOptionTradeCommand;
        ToolStripMenuItem pasteStockTradeCommand;
        ToolStripMenuItem redoMerrillCorrectionCommand;
        ToolStripMenuItem redoMerrillCorrectionToDateCommand;
        ToolStripMenuItem restoreConfirmationCommand;
        ToolStripMenuItem reverseFilterCommand;
        ToolStripMenuItem selectAllCommand;
        ToolStripMenuItem showAssociatedOptionTradesCommand;
        ToolStripMenuItem showAssociatedStockTradesCommand;
        ToolStripMenuItem undoHugoFuturesCorrectionCommand;
        ToolStripMenuItem undoHugoOptionCorrectionCommand;
        ToolStripMenuItem undoHugoStockCorrectionCommand;
        ToolStripMenuItem unmatchOptionPositionsCommand;
        #endregion
        #endregion

        #region Constructor
        public Form1()
        {
            Type classType = this.GetType();
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger(classType);

            try
            {
                Icon = Properties.Resources.App;
                InitializeComponent();

                formSymbolMappings = new FormSymbolMappings();
                formSubAccounts = new FormSubAccounts();
                menu = new ContextMenuStrip();
                BuildDataGridStateManagers();
                BuildContextMenuCommands();
                OnClearingHouseChanged();

                EnableControls();
            }
            catch (Exception ex)
            {
                logger.Error("Error instantiating form", ex);
                throw;
            }
        }
        #endregion

        #region Public properties
        public static ILog Logger { get { return logger; } }
        #endregion

        #region Events
        public static event EventHandler SelectedUnderlyingChanged
        {
            add { selectedUnderlyingChanged += value; }
            remove { selectedUnderlyingChanged -= value; }
        }
        public static event EventHandler SelectedOptionChanged
        {
            add { selectedOptionChanged += value; }
            remove { selectedOptionChanged -= value; }
        }
        public static event EventHandler SelectedFutureChanged
        {
            add { selectedFutureChanged += value; }
            remove { selectedFutureChanged -= value; }
        }
        #endregion

        #region Public methods and properties
        public static IUnderlying SelectedUnderlying
        {
            get { return selectedUnderlying; }
            set
            {
                selectedUnderlying = value;
                selectedOption = null;

                if (selectedUnderlyingChanged != null)
                {
                    selectedUnderlyingChanged(null, new EventArgs());
                }
            }
        }

        public static IOption SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                selectedUnderlying = value;

                if (selectedOptionChanged != null)
                {
                    selectedOptionChanged(null, new EventArgs());
                }
            }
        }

        public static IUnderlying SelectedFuture
        {
            get { return selectedFuture; }
            set
            {
                selectedFuture = value;

                if (selectedFutureChanged != null)
                {
                    selectedFutureChanged(null, new EventArgs());
                }
            }
        }

        public static string ClearingHouseText
        {
            get { return clearingHouseText; }
        }
        #endregion

        #region Event handlers

        #region Form event handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
     
                ReconciliationLib.Utilities.Init();
                ReconciliationLib.Utilities.TraderName = Properties.Settings.Default.TraderName;

                 // wire events
                ReconciliationLib.Utilities.OnError += new EventHandler<ErrorEventArgs>(Utilities_OnError);
                ReconciliationLib.Utilities.OnInfo += new EventHandler<InfoEventArgs>(Utilities_OnInfo);
                ReconciliationLib.Utilities.OnTablesUpdated += new EventHandler<TablesUpdatedEventArgs>(Utilities_OnTablesUpdated);
                GargoyleTaskLib.TaskUtilities.OnError += new EventHandler<ErrorEventArgs>(Utilities_OnError);
                GargoyleTaskLib.TaskUtilities.OnInfo += new EventHandler<InfoEventArgs>(Utilities_OnInfo);
                Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);

                if (Properties.Settings.Default.UpgradeSettings)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.UpgradeSettings = false;
                }

                // get Hugo connection
                Gargoyle.Utils.DBAccess.DBAccess dbAccess = Gargoyle.Utils.DBAccess.DBAccess.GetDBAccessOfTheCurrentUser(Properties.Settings.Default.ApplicationName);
                ReconciliationLib.Utilities.Connection = dbAccess.GetConnection(Properties.Settings.Default.Database);

 
                // update title bar
                Text = String.Format("{0} {1} - {2}",
                     System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                    ReconciliationLib.Utilities.Connection.DataSource.ToUpper());

 
                // populate controls
                FillDataGridComboBoxes();
                dateTimePickerPrevious.Value = ReconciliationLib.Utilities.CalculatePreviousBusinessDay(dateTimePickerCurrent.Value);

                // instantiate print dialog
                formPrint = new FormPrint(dataGridViewStockPositions,
                    dataGridViewOptionPositions,
                    dataGridViewFuturesPositions,
                    dataGridViewHugoStockTrades,
                    dataGridViewHugoOptionTrades,
                    dataGridViewHugoFuturesTrades,
                    dataGridViewMerrillStockTrades,
                    dataGridViewMerrillOptionTrades,
                    dataGridViewMerrillFuturesTrades,
                    dataGridViewMerrillCorrections1,
                    dataGridViewMerrillCorrections2,
                    dataGridViewHugoCorrections1,
                    dataGridViewHugoCorrections2,
                    dataGridViewHugoFuturesCorrections);

                WireVerificationMethods();

                // not sure why this is necessary, but Visual Studio sets these to what it wants
                //  and won't allow me to change them in the designer
                statusStrip1.Visible = true;
                statusStrip2.Visible = true;
                statusStrip3.Visible = true;
                statusStrip4.Visible = true;
                statusStrip5.Visible = true;
                statusStrip6.Visible = true;
                statusStrip7.Visible = true;
                statusStrip8.Visible = true;
                statusStrip9.Visible = true;
                statusStrip10.Visible = true;
                statusStrip11.Visible = true;
                statusStrip12.Visible = true;
                statusStrip13.Visible = true;
                statusStrip14.Visible = true;
                totalCostDataGridViewTextBoxColumn1.ReadOnly = false;
 
                Disposed +=new EventHandler(Form1_Disposed);
            }
            catch (Exception ex)
            {
                HandleError("Error loading form", ex);
                Close();
            }
        }

        private void Form1_Disposed(object sender, EventArgs e)
        {
            if (formPrint != null) formPrint.Dispose();
            if (formSymbolMappings != null) formSymbolMappings.Dispose();
            if (formSubAccounts != null) formSubAccounts.Dispose();
            if (menu != null) menu.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // see if we have any dataset changes that haven't been committed
            if (!CanChangeSelection(null))
            {
                e.Cancel = true;
            }

            // see if we have any setting changes
            if (settingsChanged && (e.Cancel == false))
            {
                DialogResult result = MessageBox.Show("Do you want to save your settings?", "Reconciliation Client", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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

            // see if we need to finalize
            if (AccountGroupSelected)
            {
                if (!GetAccountGroupInfo().IsFinalized)
                {
                    DialogResult result = MessageBox.Show("Be sure to finalize this account group at some point today", "Reconciliation Client", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }
                }
            }

            if (e.Cancel == false)
            {
                Properties.Settings.Default.PropertyChanged -= new PropertyChangedEventHandler(Default_PropertyChanged);
                Utilities.Dispose();
                components = null;  // prevents calling dispose on components, which takes too long
            }
        }

        #endregion

        #region Status strip event handlers
        private void showNonZeroDiscrepanciesLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForPositions stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForPositions;
                stateManager.SwitchToNonZeroDiscrepancyView();
            }
            catch (Exception ex)
            {
                HandleError("Error showing non-zero discrepancies", ex);
            }
        }
 
        private void showMerrillDetailsLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForPositions stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForPositions;
                stateManager.SwitchDetailsView();
            }
            catch (Exception ex)
            {
                HandleError("Error showing" + clearingHouseText + "details", ex);
            }
        }

        private void showStockLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForStockTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForStockTrades;
                if (CanChangeSelection(stateManager))
                {
                    stateManager.SwitchToUnderlyingView();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error selecting stock", ex);
            }
        }
        private void showFutureLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForFuturesTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForFuturesTrades;
                if (CanChangeSelection(stateManager))
                {
                    stateManager.SwitchToUnderlyingView();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error selecting stock", ex);
            }
        }

        private void groupByPriceLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForStockTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForStockTrades;
                stateManager.ToggleGroupByPrice();
            }
            catch (Exception ex)
            {
                HandleError("Error grouping by price", ex);
            }
        }

        private void showOptionLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForOptionTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForOptionTrades;
                if (CanChangeSelection(stateManager))
                {
                    stateManager.SwitchToOptionView();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error selecting option", ex);
            }
        }

        private void showUnderlyingLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForOptionTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForOptionTrades;
                if (CanChangeSelection(stateManager))
                {
                    stateManager.SwitchToUnderlyingView();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing options for underlying", ex);
            }
        }

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromToolStripItem(sender);
                stateManager.RemoveFilter();
            }
              catch (Exception ex)
            {
                HandleError("Error removing grid filter", ex);
            }
      }

        private void showAllLabelAfterVerification_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManagerForTrades stateManager = GetStateManagerFromToolStripItem(sender) as DataGridStateManagerForTrades;
                if (CanChangeSelection(stateManager))
                {
                    stateManager.ShowAll();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error processing 'Show All' request", ex);
            }
        }

        private void finalizationStatusLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.StockPriceDiscrepancyCount > 0)
                {
                    stockPositionsDataTableBindingSource.Filter = priceDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing stock price discrepancies", ex);
            }
        }

        private void finalizationStatusLabel2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.OptionPriceDiscrepancyCount > 0)
                {
                    optionPositionsDataTableBindingSource.Filter = priceDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing option price discrepancies", ex);
            }
        }

        private void finalizationStatusLabel9_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.FuturesPriceDiscrepancyCount > 0)
                {
                    futuresPositionsDataTableBindingSource.Filter = priceDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing futures price discrepancies", ex);
            }

        }

        private void totalCostLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.StockTotalCostDiscrepancyCount > 0)
                {
                    stockPositionsDataTableBindingSource.Filter = totalCostDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing stock total cost discrepancies", ex);
            }
        }

        private void totalCostLabel2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.OptionTotalCostDiscrepancyCount > 0)
                {
                    optionPositionsDataTableBindingSource.Filter = totalCostDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing option total cost discrepancies", ex);
            }
        }
        private void totalCostLabel9_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.FuturesTotalCostDiscrepancyCount > 0)
                {
                    futuresPositionsDataTableBindingSource.Filter = totalCostDiscrepancyFilter;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing futures total cost discrepancies", ex);
            }
        }


        private void missingStockPricesLabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.MissingConcurrentStockPriceCount > 0)
                {
                    if (CanChangeSelection(hugoOptionTradesDataGridStateManager))
                    {
                        hugoOptionTradesDataGridStateManager.SwitchToMissingStockPricesView();
                        tabControl1.SelectTab("tabOptionTrades");
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing option trades with missing stock prices", ex);
            }
        }
        #endregion

        #region Menu event handlers
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (Form formSettings = new FormSettings())
                {
                    formSettings.ShowDialog();

                    OnClearingHouseChanged(); // clearing house may have changed
                    ReconciliationLib.Utilities.TraderName = Properties.Settings.Default.TraderName;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing settings", ex);
            }
        }
   
        private void symbolMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK == formSymbolMappings.ShowDialog())
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing symbol mappings", ex);
            }
        }

        private void accountMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK == formSubAccounts.ShowDialog())
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing account mappings", ex);
            }
        }
 
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                formPrint.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError("Error printing report", ex);
            }
        }

        private void consolidateoptionTradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConsolidateOptions = consolidateoptionTradesToolStripMenuItem.Checked;
            hugoOptionTradesDataTableBindingSource.DataSource = Properties.Settings.Default.ConsolidateOptions
                ? Utilities.ConsolidatedHugoOptionTrades : Utilities.UnconsolidatedHugoOptionTrades;
        }
 
        #endregion

        #region ComboBox event handlers
        private void comboBoxAccountGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                // set new account group
                ReconciliationLib.Utilities.AccountGroupName = AccountGroupName;
                GetAccountGroupInfo();
                GetImportStatusInfo();

                OnClearingHouseChanged();
                FillPositions();
            }
            catch (Exception ex)
            {
                HandleError("Error changing account group", ex);
            }
        }

        private void OnClearingHouseChanged()
        {
            clearingHouseText = Utilities.ClearingHouse.ToString();
            if (Utilities.ClearingHouse == ClearingHouse.None)
                clearingHouseText = "Clearing House";

            //TotalCostDiscrepancy.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //TotalCost.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //NetAmount.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewTotalCostAutoFilterTextBoxColumn.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn3.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn7.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn8.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn10.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn12.Visible = ReconciliationLib.Utilities.UsingTaxLots;
            //dataGridViewAutoFilterTextBoxColumn15.Visible = ReconciliationLib.Utilities.UsingTaxLots;

            ShowImportButton();
            showMerrillDetailsLabel1.Text = String.Format("Show {0} &details", clearingHouseText);
            showMerrillDetailsLabel1.ToolTipText = String.Format("Toggle between {0} totals and original and adjusted subtotals", clearingHouseText);
            MerrillSymbol.ToolTipText = String.Format("Stock symbol according to {0}", clearingHouseText);
            discrepancyDataGridViewTextBoxColumn.ToolTipText = String.Format("Difference between Hugo\'s current position and {0}\'s current position", clearingHouseText);
            merrillAdjustmentDataGridViewTextBoxColumn.ToolTipText = String.Format("Adjustments we made to {0}\'s position on current date", clearingHouseText);
            previousMerrillOriginalDataGridViewTextBoxColumn.ToolTipText = String.Format("Imported {0} position on previous date", clearingHouseText);
            merrillTotalDataGridViewTextBoxColumn.ToolTipText = String.Format("Sum of imported {0} position and our adjustments for current date", clearingHouseText);
            previousMerrillAdjustmentDataGridViewTextBoxColumn.ToolTipText = String.Format("Adjustments we made to {0}\'s position on previous date", clearingHouseText);
            previousMerrillTotalDataGridViewTextBoxColumn.ToolTipText = String.Format("Sum of imported {0} position and our adjustments for previous date", clearingHouseText);
            showMerrillDetailsLabel2.Text = String.Format("Show {0} &details", clearingHouseText);
            showMerrillDetailsLabel2.ToolTipText = String.Format("Toggle between {0} totals and original and adjusted subtotals", clearingHouseText);
            merrillPositionDataGridViewTextBoxColumn1.ToolTipText = String.Format("Imported {0} position on current date", clearingHouseText);
            merrillAdjustmentDataGridViewTextBoxColumn1.ToolTipText = String.Format("Adjustments we made to {0}\'s position for current date", clearingHouseText);
            merrillTotalDataGridViewTextBoxColumn1.ToolTipText = String.Format("Sum of imported {0} position and our adjustments for current date", clearingHouseText);
            previousMerrillPositionDataGridViewTextBoxColumn1.ToolTipText = String.Format("Imported {0}\'s position on previous date", clearingHouseText);
            previousMerrillAdjustmentDataGridViewTextBoxColumn1.ToolTipText = String.Format("Adjustments we made to {0}\'s position for previous date", clearingHouseText);
            previousMerrillTotalDataGridViewTextBoxColumn1.ToolTipText = String.Format("Sum of imported {0} position and our adjustments for previous date", clearingHouseText);
            toolStripStatusLabel4.Text = String.Format("{0} Stock Trades", clearingHouseText);
            dataGridViewMerrillStockTrades.AccessibleName = String.Format("{0} Stock Trades", clearingHouseText);
            dataGridViewMerrillOptionTrades.AccessibleName = String.Format("{0} Option Trades", clearingHouseText);
            dataGridViewMerrillFuturesTrades.AccessibleName = String.Format("{0} Futures Trades", clearingHouseText);
            recordTypeDataGridViewTextBoxColumn.ToolTipText = String.Format("Add: a trade we added (i.e., not imported from {0}); DK: a trade we imported " +
                 "but are disputing", clearingHouseText);
            cancelCodeDataGridViewTextBoxColumn.ToolTipText = String.Format("Flagged by {0} as a cancelation or correction", clearingHouseText);
            importDateDataGridViewTextBoxColumn.ToolTipText = String.Format("Date we downloaded the trade from {0}", clearingHouseText);
            tradeReferenceNumberDataGridViewTextBoxColumn.ToolTipText = String.Format("A unique number assigned by {0} to each trade pending settlement", clearingHouseText);
            toolStripStatusLabel6.Text = String.Format("{0} Option Trades", clearingHouseText);
            recordTypeDataGridViewTextBoxColumn1.ToolTipText = String.Format("Add: a trade we added (i.e., not imported from {0}); DK: a trade we imported " +
                "but are disputing", clearingHouseText);
            cancelCodeDataGridViewTextBoxColumn1.ToolTipText = String.Format("Flagged by {0} as a cancelation or correction", clearingHouseText);
            importDateDataGridViewTextBoxColumn1.ToolTipText = String.Format("Date we downloaded the trade from {0}", clearingHouseText);
            tradeReferenceNumberDataGridViewTextBoxColumn1.ToolTipText = String.Format("A unique number assigned by {0} to each trade pending settlement", clearingHouseText);
            tabMerrillCorrections.Text = String.Format("{0} Corrections", clearingHouseText);
            toolStripStatusLabel8.Text = String.Format("{0} Corrections for Previous Date", clearingHouseText);
            toolStripStatusLabel7.Text = String.Format("{0} Corrections for Current Date", clearingHouseText);
            dataGridViewMerrillCorrections2.AccessibleName = String.Format("Yesterday\'s {0} Corrections", clearingHouseText);
            recordTypeDataGridViewTextBoxColumn3.ToolTipText = String.Format("Add: a trade we added (i.e., not imported from {0}); DK: a trade we imported " +
               " trade", clearingHouseText);
            importDateDataGridViewTextBoxColumn3.ToolTipText = String.Format("Date we downloaded the trade from {0}", clearingHouseText);
            tradeReferenceNumberDataGridViewTextBoxColumn3.ToolTipText = String.Format("A unique number assigned by {0} to each trade pending settlement", clearingHouseText);
            dataGridViewMerrillCorrections1.AccessibleName = String.Format("Today\'s {0} Corrections", clearingHouseText);
            AccountNumber.ToolTipText = String.Format("Number of {0} account the trade belongs to", clearingHouseText);
            recordTypeDataGridViewTextBoxColumn2.ToolTipText = String.Format("Add: a trade we added (i.e., not imported from {0}); DK: a trade we imported " +
                 "but are disputing; Correction: a change to an imported record", clearingHouseText);
            importDateDataGridViewTextBoxColumn2.ToolTipText = String.Format("Date we downloaded the trade from {0}", clearingHouseText);
            tradeReferenceNumberDataGridViewTextBoxColumn2.ToolTipText = String.Format("A unique number assigned by {0} to each trade pending settlement", clearingHouseText);
            buttonImport.Text = String.Format("Import {0}\'s files", clearingHouseText);

            showMerrillDetailsLabel9.Text = String.Format("Show {0} &details", clearingHouseText);
            showMerrillDetailsLabel9.ToolTipText = String.Format("Toggle between {0} totals and original and adjusted positions", clearingHouseText);
            toolStripStatusLabel13.Text = String.Format("{0} Futures Trades", clearingHouseText);
            importDateDataGridViewTextBoxColumn4.ToolTipText = String.Format("Date we downloaded the trade from {0}", clearingHouseText);

            if (addStockTradesToMerrillCommand != null) // do only if we have created the context menus
            {
                addStockTradesToMerrillCommand.Text = String.Format("&Add trade(s) to {0}", clearingHouseText);
                addOptionTradesToMerrillCommand.Text = String.Format("&Add trade(s) to {0}", clearingHouseText);
                addSymbolMappingCommand.Text = String.Format("Match Hugo and {0} symbols", clearingHouseText);
                deleteSymbolMappingCommand.Text = String.Format("Un-match Hugo and {0} symbols", clearingHouseText);
                matchOptionPositionsCommand.Text = String.Format("Match Hugo and {0} option positions", clearingHouseText);
                unmatchOptionPositionsCommand.Text = String.Format("Unmatch Hugo and {0} option positions", clearingHouseText);
            }

            if (formSymbolMappings != null)
            {
                formSymbolMappings.OnClearingHouseChanged();
                formSubAccounts.OnClearingHouseChanged();
            }

        }
        #endregion

        #region DataGrid event handlers

        #region DataBindingComplete events
        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                if (stateManager != null)
                {
                    stateManager.RefreshStatusStrip();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error binding data grid", ex);
            }
        }
        #endregion


  
  
        #region RowHeaderMouseClick events
        private void dataGridViewStockPositions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                object obj = dataGridViewStockPositions.Rows[e.RowIndex].DataBoundItem;
                if (obj != null)
                {
                    if (CanChangeSelection(null))
                    {
                        SelectedUnderlying = new Stock((DataRowView)obj);
                        EnableControls();
                    }
                    else
                    {
                        dataGridViewStockPositions.Rows[e.RowIndex].Selected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error loading stock trades", ex);
            }
        }

        private void dataGridViewOptionPositions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                object obj = dataGridViewOptionPositions.Rows[e.RowIndex].DataBoundItem;
                if (obj != null)
                {
                    if (CanChangeSelection(null))
                    {
                        SelectedOption = new Option((DataRowView)obj);
                        EnableControls();
                    }
                    else
                    {
                        dataGridViewOptionPositions.Rows[e.RowIndex].Selected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error loading option trades", ex);
            }
        }
        private void dataGridViewFuturesPositions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                object obj = dataGridViewFuturesPositions.Rows[e.RowIndex].DataBoundItem;
                if (obj != null)
                {
                    if (CanChangeSelection(null))
                    {
                        SelectedFuture = new Future((DataRowView)obj);
                        EnableControls();
                    }
                    else
                    {
                        dataGridViewFuturesPositions.Rows[e.RowIndex].Selected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error loading futures trades", ex);
            }

        }

        #endregion

        #region CellContextMenuStripNeeded events
        private void dataGridViewHugoFuturesCorrections_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                currentDataGridView = sender as DataGridView;
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex < 0)
                    {
                        stateManager.SaveRowsToProcess(e.RowIndex);
                        menu.Items.Add(undoHugoFuturesCorrectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyHugoFuturesCorrectionCommand);
                        menu.Items.Add(changeFuturesSelectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableFuturesRelatedCommands(stateManager, currentDataGridView);
                    }

                    if (menu.Items.Count > 0)
                    {
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }

        }

 
 
        
        private void dataGridViewHugoCorrections1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                currentDataGridView = sender as DataGridView;
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex < 0)
                    {
                        stateManager.SaveRowsToProcess(e.RowIndex);
                        menu.Items.Add(undoHugoStockCorrectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyHugoStockCorrectionCommand);
                        menu.Items.Add(changeStockSelectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableStockRelatedCommands(stateManager, currentDataGridView);
                    }

                    if (menu.Items.Count > 0)
                    {
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewHugoCorrections2_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                currentDataGridView = sender as DataGridView;
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex < 0)
                    {
                        stateManager.SaveRowsToProcess(e.RowIndex);
                        menu.Items.Add(undoHugoOptionCorrectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyHugoOptionCorrectionCommand);
                        menu.Items.Add(changeOptionSelectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableOptionRelatedCommands(stateManager, currentDataGridView);
                    }

                    if (menu.Items.Count > 0)
                    {
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewOptionPositions_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex < 0)
                    {
                        optionPositionsDataGridStateManager.SaveRowsToProcess(e.RowIndex);
                        if (CurrentDatesImportDone())
                        {
                            if (optionPositionsDataGridStateManager.RowsToProcessContainAcceptedDiscrepancies)
                            {
                                menu.Items.Add(clearAcceptedOptionDiscrepancyCommand);
                            }
                            if (optionPositionsDataGridStateManager.RowsToProcessContainNonzeroDiscrepancies)
                            {
                                menu.Items.Add(acceptOptionDiscrepancyCommand);
                                if (Properties.Settings.Default.DebugMode)
                                {
                                    menu.Items.Add(addOptionAdjustmentCommand);
                                }
                            }
                            if (optionPositionsDataGridStateManager.RowsToProcessCanBeMatched)
                            {
                                menu.Items.Add(matchOptionPositionsCommand);
                            }
                            if (optionPositionsDataGridStateManager.RowsToProcessCanBeUnmatched)
                            {
                                menu.Items.Add(unmatchOptionPositionsCommand);
                            }
                            if (menu.Items.Count > 0)
                            {
                                menu.Items.Add(new ToolStripSeparator());
                            }
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);
                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewOptionPositions;
                        e.ContextMenuStrip = menu;
                    }
                }

            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }
        
        private void dataGridViewStockPositions_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    bool symbolsSet, sameRow;
                    stockPositionsDataGridStateManager.SetSelectedSymbols(out symbolsSet, out sameRow);
                    if (symbolsSet)
                    {
                        if (sameRow)
                        {
                            menu.Items.Add(deleteSymbolMappingCommand);
                        }
                        else
                        {
                            menu.Items.Add(addSymbolMappingCommand);
                        }
                    }

                    // additional commands for header column only
                    if (e.ColumnIndex < 0)
                    {
                        stockPositionsDataGridStateManager.SaveRowsToProcess(e.RowIndex);
                        if (CurrentDatesImportDone())
                        {
                            if (stockPositionsDataGridStateManager.RowsToProcessContainAcceptedDiscrepancies)
                            {
                                menu.Items.Add(clearAcceptedStockDiscrepancyCommand);
                            }
                            if (stockPositionsDataGridStateManager.RowsToProcessContainNonzeroDiscrepancies)
                            {
                                menu.Items.Add(acceptStockDiscrepancyCommand);
                                if (Properties.Settings.Default.DebugMode)
                                {
                                    menu.Items.Add(addStockAdjustmentCommand);
                                }
                            }
                            if (menu.Items.Count > 0)
                            {
                                menu.Items.Add(new ToolStripSeparator());
                            }
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);
                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewStockPositions;
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewFuturesPositions_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    bool symbolsSet, sameRow;
                    futuresPositionsDataGridStateManager.SetSelectedSymbols(out symbolsSet, out sameRow);
                    if (symbolsSet)
                    {
                        if (sameRow)
                        {
                            menu.Items.Add(deleteSymbolMappingCommand);
                        }
                        else
                        {
                            menu.Items.Add(addSymbolMappingCommand);
                        }
                    }

                    // additional commands for header column only
                    if (e.ColumnIndex < 0)
                    {
                        futuresPositionsDataGridStateManager.SaveRowsToProcess(e.RowIndex);
                        if (CurrentDatesImportDone())
                        {
                            if (futuresPositionsDataGridStateManager.RowsToProcessContainAcceptedDiscrepancies)
                            {
                                menu.Items.Add(clearAcceptedFuturesDiscrepancyCommand);
                            }
                            if (futuresPositionsDataGridStateManager.RowsToProcessContainNonzeroDiscrepancies)
                            {
                                menu.Items.Add(acceptFuturesDiscrepancyCommand);
                                if (Properties.Settings.Default.DebugMode)
                                {
                                    menu.Items.Add(addFuturesAdjustmentCommand);
                                }
                            }
                            if (menu.Items.Count > 0)
                            {
                                menu.Items.Add(new ToolStripSeparator());
                            }
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);
                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewFuturesPositions;
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
  
        }
        
        private void dataGridViewHugoStockTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex >= 0)
                    {
                        if (!dataGridViewHugoStockTrades.Columns[e.ColumnIndex].ReadOnly)
                        {
                            hugoStockTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                            FillDataGridViewContextMenuStrip(menu, dataGridViewHugoStockTrades.Columns[e.ColumnIndex]);
                        }
                    }
                    else
                    {
                        hugoStockTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                        if (CurrentDatesImportDone())
                        {
                            menu.Items.Add(addStockTradesToMerrillCommand);
                        }
                        menu.Items.Add(deleteTradeCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyHugoStockTradeCommand);
                        menu.Items.Add(pasteStockTradeCommand);
                        menu.Items.Add(distributeStockTradeCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(changeStockSelectionCommand);
                        menu.Items.Add(showAssociatedOptionTradesCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(hideCommand);
                        if (Utilities.UsingTaxLots && (Utilities.StockPriceDiscrepancyCount == 0))
                        {
                            menu.Items.Add(matchStockTotalCostCommand);
                        }
                        else
                        {
                            menu.Items.Add(matchStockTradesCommand);
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableStockRelatedCommands(hugoStockTradesDataGridStateManager, dataGridViewHugoStockTrades);
                        EnableHugoRelatedCommands(hugoStockTradesDataGridStateManager);

                        if (clipboardStockTrade == null)
                        {
                            distributeStockTradeCommand.Enabled = false;
                        }
                        else
                        {
                            distributeStockTradeCommand.Enabled = (hugoStockTradesDataGridStateManager.RowsToProcess.Length > 1)
                                && hugoStockTradesDataGridStateManager.CanDistributeTrade
                                && (hugoStockTradesDataGridStateManager.RowsToProcessTradeVolume == clipboardStockTrade.TradeVolume *
                                ((clipboardStockTrade.TradeType[0] == 'S') ? -1 : 1));
                        }

                        if (hugoStockTradesDataGridStateManager.RowsToProcess.Length == 1)
                        {
                            HugoDataSet.HugoStockTradesRow row = ConvertExt.ToRow<HugoDataSet.HugoStockTradesRow>(hugoStockTradesDataGridStateManager.RowsToProcess[0]);
                            showAssociatedOptionTradesCommand.Enabled = !row.IsOptionTradeIdNull();
                        }
                        else
                        {
                            showAssociatedOptionTradesCommand.Enabled = false;
                        }
                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewHugoStockTrades;
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewHugoFuturesTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex >= 0)
                    {
                        if (!dataGridViewHugoFuturesTrades.Columns[e.ColumnIndex].ReadOnly)
                        {
                            hugoFuturesTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                            FillDataGridViewContextMenuStrip(menu, dataGridViewHugoFuturesTrades.Columns[e.ColumnIndex]);
                        }
                    }
                    else
                    {
                        hugoFuturesTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                        if (CurrentDatesImportDone())
                        {
                            menu.Items.Add(addFuturesTradesToMerrillCommand);
                        }
                        menu.Items.Add(deleteTradeCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyHugoFuturesTradeCommand);
                        menu.Items.Add(pasteFuturesTradeCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(changeFuturesSelectionCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(hideCommand);
                        if (Utilities.UsingTaxLots && (Utilities.FuturesPriceDiscrepancyCount == 0))
                        {
                            menu.Items.Add(matchFuturesTotalCostCommand);
                        }
                        else
                        {
                            menu.Items.Add(matchFuturesTradesCommand);
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableFuturesRelatedCommands(hugoFuturesTradesDataGridStateManager, dataGridViewHugoFuturesTrades);
                        EnableHugoRelatedCommands(hugoFuturesTradesDataGridStateManager);

                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewHugoFuturesTrades;
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }

        }

        
        private void dataGridViewMerrillOptionTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if (CurrentDatesImportDone())
                {
                    menu.Items.Clear();
                    {
                        menu.ShowImageMargin = false;

                        if (e.ColumnIndex >= 0)
                        {
                            if (!dataGridViewMerrillOptionTrades.Columns[e.ColumnIndex].ReadOnly)
                            {
                                merrillOptionTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                                FillDataGridViewContextMenuStrip(menu, dataGridViewMerrillOptionTrades.Columns[e.ColumnIndex]);
                            }
                        }
                        else
                        {
                            merrillOptionTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                            menu.Items.Add(addOptionTradesToHugoCommand);
                            menu.Items.Add(deleteConfirmationCommand);
                            menu.Items.Add(restoreConfirmationCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(copyOptionCommand);
                            menu.Items.Add(copyMerrillOptionTradeCommand);
                            menu.Items.Add(pasteOptionCommand);
                            menu.Items.Add(pasteOptionTradeCommand);
                            menu.Items.Add(changeOptionSelectionCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(hideCommand);
                            if (Utilities.UsingTaxLots && (Utilities.OptionPriceDiscrepancyCount == 0))
                            {
                                menu.Items.Add(matchOptionTotalCostCommand);
                            }
                            else
                            {
                                menu.Items.Add(matchOptionTradesCommand);
                            }
                            menu.Items.Add(reverseFilterCommand);
                            menu.Items.Add(selectAllCommand);

                            // enable or disable commands as appropriate
                            EnableOptionRelatedCommands(merrillOptionTradesDataGridStateManager, dataGridViewMerrillOptionTrades);
                            EnableMerrillRelatedCommands(merrillOptionTradesDataGridStateManager);
                        }

                        if (menu.Items.Count > 0)
                        {
                            currentDataGridView = dataGridViewMerrillOptionTrades;
                            e.ContextMenuStrip = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewMerrillStockTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if (CurrentDatesImportDone())
                {
                    menu.Items.Clear();
                    {
                        menu.ShowImageMargin = false;

                        if (e.ColumnIndex >= 0)
                        {
                            if (!dataGridViewMerrillStockTrades.Columns[e.ColumnIndex].ReadOnly)
                            {
                                merrillStockTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                                FillDataGridViewContextMenuStrip(menu, dataGridViewMerrillStockTrades.Columns[e.ColumnIndex]);
                            }
                        }
                        else
                        {
                            merrillStockTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                            menu.Items.Add(addStockTradesToHugoCommand);
                            menu.Items.Add(deleteConfirmationCommand);
                            menu.Items.Add(restoreConfirmationCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(copyMerrillStockTradeCommand);
                            menu.Items.Add(pasteStockTradeCommand);
                            menu.Items.Add(changeStockSelectionCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(hideCommand);
                            if (Utilities.UsingTaxLots && (Utilities.StockPriceDiscrepancyCount == 0))
                            {
                                menu.Items.Add(matchStockTotalCostCommand);
                            }
                            else
                            {
                                menu.Items.Add(matchStockTradesCommand);
                            }
                            menu.Items.Add(reverseFilterCommand);
                            menu.Items.Add(selectAllCommand);

                            // enable or disable commands as appropriate
                            EnableStockRelatedCommands(merrillStockTradesDataGridStateManager, dataGridViewMerrillStockTrades);
                            EnableMerrillRelatedCommands(merrillStockTradesDataGridStateManager);
                        }

                        if (menu.Items.Count > 0)
                        {
                            currentDataGridView = dataGridViewMerrillStockTrades;
                            e.ContextMenuStrip = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewMerrillFuturesTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if (CurrentDatesImportDone())
                {
                    menu.Items.Clear();
                    {
                        menu.ShowImageMargin = false;

                        if (e.ColumnIndex >= 0)
                        {
                            if (!dataGridViewMerrillFuturesTrades.Columns[e.ColumnIndex].ReadOnly)
                            {
                                merrillFuturesTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                                FillDataGridViewContextMenuStrip(menu, dataGridViewMerrillFuturesTrades.Columns[e.ColumnIndex]);
                            }
                        }
                        else
                        {
                            merrillFuturesTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                            menu.Items.Add(addFuturesTradesToHugoCommand);
                            menu.Items.Add(deleteConfirmationCommand);
                            menu.Items.Add(restoreConfirmationCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(copyMerrillFuturesTradeCommand);
                            menu.Items.Add(pasteFuturesTradeCommand);
                            menu.Items.Add(changeFuturesSelectionCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(hideCommand);
                            if (Utilities.UsingTaxLots && (Utilities.FuturesPriceDiscrepancyCount == 0))
                            {
                                menu.Items.Add(matchFuturesTotalCostCommand);
                            }
                            else
                            {
                                menu.Items.Add(matchFuturesTradesCommand);
                            }
                            menu.Items.Add(reverseFilterCommand);
                            menu.Items.Add(selectAllCommand);

                            // enable or disable commands as appropriate
                            EnableFuturesRelatedCommands(merrillFuturesTradesDataGridStateManager, dataGridViewMerrillFuturesTrades);
                            EnableMerrillRelatedCommands(merrillFuturesTradesDataGridStateManager);
                        }

                        if (menu.Items.Count > 0)
                        {
                            currentDataGridView = dataGridViewMerrillFuturesTrades;
                            e.ContextMenuStrip = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewHugoOptionTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                menu.Items.Clear();
                {
                    menu.ShowImageMargin = false;

                    if (e.ColumnIndex >= 0)
                    {
                        if (!dataGridViewHugoOptionTrades.Columns[e.ColumnIndex].ReadOnly)
                        {
                            if (hugoOptionTradesDataGridStateManager.CanEditColumn(e.RowIndex, e.ColumnIndex))
                            {
                                hugoOptionTradesDataGridStateManager.SaveCellsToProcess(e.ColumnIndex, e.RowIndex);
                                FillDataGridViewContextMenuStrip(menu, dataGridViewHugoOptionTrades.Columns[e.ColumnIndex]);
                            }
                        }
                    }
                    else
                    {
                        hugoOptionTradesDataGridStateManager.SaveRowsToProcess(e.RowIndex);

                        if (CurrentDatesImportDone())
                        {
                            menu.Items.Add(addOptionTradesToMerrillCommand);
                        }
                        menu.Items.Add(deleteTradeCommand);
                        if (hugoOptionTradesDataGridStateManager.RowsToProcess.Length > 1)
                        {
                            menu.Items.Add(consolidateTradesCommand);
                        }
                        if (hugoOptionTradesDataGridStateManager.CanDeconsolidate && Properties.Settings.Default.ConsolidateOptions)
                        {
                            menu.Items.Add(deconsolidateTradesCommand);
                        }
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(copyOptionCommand);
                        menu.Items.Add(copyHugoOptionTradeCommand);
                        menu.Items.Add(pasteOptionCommand);
                        menu.Items.Add(pasteOptionTradeCommand);
                        menu.Items.Add(distributeOptionTradeCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(changeOptionSelectionCommand);
                        menu.Items.Add(showAssociatedStockTradesCommand);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(hideCommand);
                        if (Utilities.UsingTaxLots && (Utilities.OptionPriceDiscrepancyCount == 0))
                        {
                            menu.Items.Add(matchOptionTotalCostCommand);
                        }
                        else
                        {
                            menu.Items.Add(matchOptionTradesCommand);
                        }
                        menu.Items.Add(reverseFilterCommand);
                        menu.Items.Add(selectAllCommand);

                        // enable or disable commands as appropriate
                        EnableOptionRelatedCommands(hugoOptionTradesDataGridStateManager, dataGridViewHugoOptionTrades);

                        if (clipboardOptionTrade == null)
                        {
                            distributeOptionTradeCommand.Enabled = false;
                        }
                        else
                        {
                            distributeOptionTradeCommand.Enabled = (hugoOptionTradesDataGridStateManager.RowsToProcess.Length > 1)
                              && hugoOptionTradesDataGridStateManager.CanDistributeTrade
                              && (hugoOptionTradesDataGridStateManager.RowsToProcessTradeVolume == clipboardOptionTrade.TradeVolume *
                              ((clipboardOptionTrade.TradeType[0] == 'S') ? -1 : 1));
                        }

                        if (hugoOptionTradesDataGridStateManager.RowsToProcess.Length == 1)
                        {
                            OptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(hugoOptionTradesDataGridStateManager.RowsToProcess[0], dataGridViewHugoOptionTrades);
                            showAssociatedStockTradesCommand.Enabled = Utilities.IsExerciseAssignment(optionTrade.TradeMedium);
                        }
                        else
                        {
                            showAssociatedStockTradesCommand.Enabled = false;
                        }
                    }

                    if (menu.Items.Count > 0)
                    {
                        currentDataGridView = dataGridViewHugoOptionTrades;
                        e.ContextMenuStrip = menu;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }

        
        private void dataGridViewMerrillCorrections1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if (CurrentDatesImportDone())
                {
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                    menu.Items.Clear();
                    {
                        menu.ShowImageMargin = false;

                        if (e.ColumnIndex < 0)
                        {
                            stateManager.SaveRowsToProcess(e.RowIndex);

                            menu.Items.Add(deleteMerrillCorrectionCommand);
                            menu.Items.Add(editNoteCommand);
                            if (CurrentDate < DateTime.Today)
                            {
                                menu.Items.Add(redoMerrillCorrectionToDateCommand);
                            }
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(copyConfirmationCorrectionCommand);
                            menu.Items.Add(changeStockSelectionCommand);
                            menu.Items.Add(changeOptionSelectionCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(reverseFilterCommand);
                            menu.Items.Add(selectAllCommand);

                            if (stateManager.RowsToProcess.Length == 1)
                            {
                                copyConfirmationCorrectionCommand.Enabled = true;
                                changeStockSelectionCommand.Enabled = true;
                                HugoDataSet.ConfirmationCorrectionsRow row = ConvertExt.ToRow<HugoDataSet.ConfirmationCorrectionsRow>(stateManager.RowsToProcess[0]);
                                changeOptionSelectionCommand.Enabled = !row.IsOptionSymbolNull();
                            }
                            else
                            {
                                copyConfirmationCorrectionCommand.Enabled = false;
                                changeStockSelectionCommand.Enabled = false;
                                changeOptionSelectionCommand.Enabled = false;
                            }
                        }

                        if (menu.Items.Count > 0)
                        {
                            currentDataGridView = sender as DataGridView;
                            e.ContextMenuStrip = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }
        
        private void dataGridViewMerrillCorrections2_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if (CurrentDatesImportDone())
                {
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
                    menu.Items.Clear();
                    {
                        menu.ShowImageMargin = false;

                        if (e.ColumnIndex < 0)
                        {
                            stateManager.SaveRowsToProcess(e.RowIndex);
                            menu.Items.Add(redoMerrillCorrectionCommand);
                            if (CurrentDate < DateTime.Today)
                            {
                                menu.Items.Add(redoMerrillCorrectionToDateCommand);
                            }
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(copyConfirmationCorrectionCommand);
                            menu.Items.Add(changeStockSelectionCommand);
                            menu.Items.Add(changeOptionSelectionCommand);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(hideDoneCorrectionsCommand);
                            menu.Items.Add(reverseFilterCommand);
                            menu.Items.Add(selectAllCommand);


                            if (stateManager.RowsToProcess.Length == 1)
                            {
                                copyConfirmationCorrectionCommand.Enabled = true;
                                changeStockSelectionCommand.Enabled = true;
                                HugoDataSet.ConfirmationCorrectionsRow row = ConvertExt.ToRow<HugoDataSet.ConfirmationCorrectionsRow>(stateManager.RowsToProcess[0]);
                                changeOptionSelectionCommand.Enabled = !row.IsOptionSymbolNull();
                            }
                            else
                            {
                                copyConfirmationCorrectionCommand.Enabled = false;
                                changeStockSelectionCommand.Enabled = false;
                                changeOptionSelectionCommand.Enabled = false;
                            }
                        }

                        if (menu.Items.Count > 0)
                        {
                            currentDataGridView = sender as DataGridView;
                            e.ContextMenuStrip = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error showing context menu", ex);
            }
        }
        #endregion

        #region SelectionChanged events
        private void dataGridViewMerrillOptionTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                merrillOptionTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel6.Text = merrillOptionTradesDataGridStateManager.TradeSumMessage;
            }
            catch (Exception ex)
            {
                HandleError("Error selecting" + clearingHouseText + "option trade", ex);
            }
        }
        private void dataGridViewHugoOptionTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                hugoOptionTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel5.Text = hugoOptionTradesDataGridStateManager.TradeSumMessage;
            }
            catch (Exception ex)
            {
                HandleError("Error selecting Hugo option trade", ex);
            }
        }
        private void dataGridViewMerrillStockTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                merrillStockTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel4.Text = merrillStockTradesDataGridStateManager.TradeSumMessage;
            }
            catch (Exception ex)
            {
                HandleError("Error selecting" + clearingHouseText + "stock trade", ex);
            }
        }
        private void dataGridViewMerrillFuturesTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                merrillFuturesTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel13.Text = merrillFuturesTradesDataGridStateManager.TradeSumMessage;
            }
            catch (Exception ex)
            {
                HandleError("Error selecting" + clearingHouseText + "futures trade", ex);
            }
        }
        private void dataGridViewHugoStockTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                hugoStockTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel3.Text = hugoStockTradesDataGridStateManager.TradeSumMessage;
            }
             catch (Exception ex)
            {
                HandleError("Error selecting Hugo stock trade", ex);
            }
        }
        private void dataGridViewHugoFuturesTrades_SelectionChanged(object sender, EventArgs e)
        {
            try
           {
                hugoFuturesTradesDataGridStateManager.CalculateSelectedTradeSum();
                tradeSumLabel10.Text = hugoFuturesTradesDataGridStateManager.TradeSumMessage;
            }
             catch (Exception ex)
            {
                HandleError("Error selecting Hugo futures trade", ex);
            }
      }
        #endregion

        #region Cell editing events
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context == DataGridViewDataErrorContexts.Commit)
            {
                e.Cancel = true;
                if (e.Exception != null)
                {
                    ((DataGridView)sender).Rows[e.RowIndex].ErrorText = e.Exception.Message;
                }
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (dataGridView.Columns[e.ColumnIndex].Tag != null)
            {
                string errorMessage = ((VerifyValue)dataGridView.Columns[e.ColumnIndex].Tag)(e.FormattedValue);
                if (errorMessage != null)
                {
                    e.Cancel = true;
                    dataGridView.Rows[e.RowIndex].ErrorText = errorMessage;
                }
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridStateManager stateManager = GetStateManagerFromDataGridView(sender);
            if (e.RowIndex >= 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                DataGridViewCell cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (Convert.IsDBNull(cell.Value) && ((cell.ValueType.Name == "Double") || (cell.ValueType.Name == "Decimal")))
                {
                    cell.Value = 0.0;
                }
                stateManager.IsDirty = true;
                EnableControls();
            }
        }

        private void dataGridViewMerrillCorrections_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView.Columns[e.ColumnIndex].Name.StartsWith("Correction"))
                {
                    DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow;
                    e.Value = row.GetDifferenceMsg(false);
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                e.Value = ex.Message;
                e.FormattingApplied = true;
            }
        }

        private void dataGridViewHugoFuturesCorrections_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView.Columns[e.ColumnIndex].Name.StartsWith("Correction"))
                {
                    DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow;
                    e.Value = row.DifferenceMsg;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                e.Value = ex.Message;
                e.FormattingApplied = true;
            }

        }

        private void dataGridViewHugoCorrections1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView.Columns[e.ColumnIndex].Name.StartsWith("Correction"))
                {
                    DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow;
                    e.Value = row.DifferenceMsg;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                e.Value = ex.Message;
                e.FormattingApplied = true;
            }
        }

        private void dataGridViewHugoCorrections2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = sender as DataGridView;
                if (dataGridView.Columns[e.ColumnIndex].Name.StartsWith("Correction"))
                {
                    DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow;
                    e.Value = row.DifferenceMsg;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                e.Value = ex.Message;
                e.FormattingApplied = true;
            }

        }

        private void dataGridViewHugoOptionTrades_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                DataGridStateManagerForHugoOptionTrades stateManager = GetStateManagerFromDataGridView(dataGridView) as DataGridStateManagerForHugoOptionTrades;
                if (!stateManager.CanEditColumn(e.RowIndex, e.ColumnIndex))
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region CellToolTipTextNeeded events
        private void dataGridViewHugoCorrections2_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridViewRow gridRowView = ((DataGridView)sender).Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow;
                    e.ToolTipText = row.FullMsg;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }
        }

        private void dataGridViewHugoCorrections1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridViewRow gridRowView = ((DataGridView)sender).Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow;
                    e.ToolTipText = row.FullMsg;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }

        }
        private void dataGridViewHugoFuturesCorrections_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridViewRow gridRowView = ((DataGridView)sender).Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow;
                    e.ToolTipText = row.FullMsg;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }
        }

        private void dataGridViewStockPositions_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "PriceDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.StockPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.StockPositionsRow;
                        e.ToolTipText = Convert.ToString(row.PriceDiscrepancyAmount);
                    }
                    else if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "TotalCostDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.StockPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.StockPositionsRow;
                        e.ToolTipText = Convert.ToString(row.TotalCostDiscrepancyAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }

        }

        private void dataGridViewOptionPositions_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "PriceDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.OptionPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.OptionPositionsRow;
                        e.ToolTipText = Convert.ToString(row.PriceDiscrepancyAmount);
                    }
                    else if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "TotalCostDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.OptionPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.OptionPositionsRow;
                        e.ToolTipText = Convert.ToString(row.TotalCostDiscrepancyAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }
        }
        private void dataGridViewFuturesPositions_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "PriceDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.FuturesPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.FuturesPositionsRow;
                        e.ToolTipText = Convert.ToString(row.PriceDiscrepancyAmount);
                    }
                    else if (dataGridView.Columns[e.ColumnIndex].DataPropertyName == "TotalCostDiscrepancy")
                    {
                        DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                        DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                        ReconciliationLib.HugoDataSet.FuturesPositionsRow row =
                            rowView.Row as ReconciliationLib.HugoDataSet.FuturesPositionsRow;
                        e.ToolTipText = Convert.ToString(row.TotalCostDiscrepancyAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }

        }

        private void dataGridViewHugoOptionTrades_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            try
            {
                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && (e.ColumnIndex <= 4))
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    DataGridViewRow gridRowView = dataGridView.Rows[e.RowIndex];
                    DataRowView rowView = gridRowView.DataBoundItem as DataRowView;
                    ReconciliationLib.HugoDataSet.HugoOptionTradesRow row =
                        rowView.Row as ReconciliationLib.HugoDataSet.HugoOptionTradesRow;

                    string toolTipText = "";
                    if (row.ConsolidationPackageId < 0)
                    {
                        toolTipText = "Single trade";
                    }
                    else
                    {
                        foreach (IOptionTrade optionTrade in
                            ReconciliationLib.Utilities.GetOptionTradesForPackageId(row.ConsolidationPackageId))
                        {
                            if (toolTipText.Length == 0)
                                toolTipText = optionTrade.ToString();
                            else
                                toolTipText += "\n" + optionTrade.ToString();
                        }
                    }
                    e.ToolTipText = toolTipText;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error displaying tooltip", ex);
            }

        }
        #endregion

        #endregion

        #region DateTimePicker event handlers
        private void dateTimePickerCurrent_CloseUp(object sender, EventArgs e)
        {
            try
            {
                ReconciliationLib.Utilities.ImportDate = CurrentDate;
                DateTime previousDate = ReconciliationLib.Utilities.CalculatePreviousBusinessDay(CurrentDate);
                if (previousDate != PreviousDate)
                {
                    PreviousDate = previousDate;
                }

                if (AccountGroupName != null)
                {
                    FillPositions();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error changing date", ex);
            }
        }

        private void dateTimePickerPrevious_ValueChanged(object sender, EventArgs e)
        {
            ReconciliationLib.Utilities.PreviousDate = PreviousDate;
        }

        private void dateTimePickerPrevious_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (AccountGroupName != null)
                {
                    FillPositions();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error changing previous date", ex);
            }
        }
        #endregion

        #region Button event handlers

        private void buttonSyncTotalCosts_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                ReconciliationLib.Utilities.SyncTotalCosts();
                RefreshDataGrids();
            }
            catch (Exception ex)
            {
                HandleError("Error syncing total costs", ex);
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }

        }
 
        private void buttonFinalize_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                ReconciliationLib.Utilities.FinalizeImport();
                ShowFinalizationState();
            }
            catch (Exception ex)
            {
                HandleError("Error during finalization", ex);
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }
        }
        private void buttonAddAssignments_Click(object sender, EventArgs e)
        {
         }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                using (FormImport form = new FormImport())
                {
                    form.ShowDialog();

                    if (form.RefreshNeeded)
                    {
                        if (!accountGroupsForWhichImportWasRunManually.Contains(Utilities.AccountGroupName))
                        {
                            accountGroupsForWhichImportWasRunManually.Add(Utilities.AccountGroupName);
                        }
                        FillPositions();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error during import", ex);
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                ReconciliationLib.Utilities.Refresh();

                // refresh data that does not depend on account group
                RefreshGenericData();

                // refresh data that does depend on account group
                if (AccountGroupSelected)
                {
                    GetImportStatusInfo();
                    subaccountNamesDataTableBindingSource.DataSource = Utilities.SubaccountNames;
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error refreshing data", ex);
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }
        }

        private void buttonCancelChanges_Click(object sender, EventArgs e)
        {
              Enabled = false;
              try
              {
                  hugoStockTradesDataGridStateManager.NeedToRefresh = true;
                  hugoOptionTradesDataGridStateManager.NeedToRefresh = true;
                  hugoFuturesTradesDataGridStateManager.NeedToRefresh = true;
                  merrillStockTradesDataGridStateManager.NeedToRefresh = true;
                  merrillOptionTradesDataGridStateManager.NeedToRefresh = true;
                  merrillFuturesTradesDataGridStateManager.NeedToRefresh = true;
                  RefreshDataGrids();
              }
              catch (Exception ex)
              {
                  HandleError("Error refreshing data", ex);
              }
              finally
              {
                  Enabled = true;
                  EnableControls();
              }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                HugoDataSet.HugoStockTradesDataTable hugoStockTradesToEdit = (HugoDataSet.HugoStockTradesDataTable)((DataTable)hugoStockTradesDataTableBindingSource.DataSource).GetChanges();
                HugoDataSet.HugoOptionTradesDataTable hugoOptionTradesToEdit = (HugoDataSet.HugoOptionTradesDataTable)((DataTable)hugoOptionTradesDataTableBindingSource.DataSource).GetChanges();
                HugoDataSet.HugoFuturesTradesDataTable hugoFuturesTradesToEdit = (HugoDataSet.HugoFuturesTradesDataTable)((DataTable)hugoFuturesTradesDataTableBindingSource.DataSource).GetChanges();
                HugoDataSet.MerrillStockTradesDataTable merrillStockTradesToEdit = (HugoDataSet.MerrillStockTradesDataTable)((DataTable)merrillStockTradesDataTableBindingSource.DataSource).GetChanges();
                HugoDataSet.MerrillOptionTradesDataTable merrillOptionTradesToEdit = (HugoDataSet.MerrillOptionTradesDataTable)((DataTable)merrillOptionTradesDataTableBindingSource.DataSource).GetChanges();
                HugoDataSet.MerrillFuturesTradesDataTable merrillFuturesTradesToEdit = (HugoDataSet.MerrillFuturesTradesDataTable)((DataTable)merrillFuturesTradesDataTableBindingSource.DataSource).GetChanges();

                int hugoStockEdits = (hugoStockTradesToEdit == null) ? 0 : hugoStockTradesToEdit.Rows.Count;
                int hugoOptionEdits = (hugoOptionTradesToEdit == null) ? 0 : hugoOptionTradesToEdit.Rows.Count;
                int hugoFuturesEdits = (hugoFuturesTradesToEdit == null) ? 0 : hugoFuturesTradesToEdit.Rows.Count;
                int merrillStockEdits = (merrillStockTradesToEdit == null) ? 0 : merrillStockTradesToEdit.Rows.Count;
                int merrillOptionEdits = (merrillOptionTradesToEdit == null) ? 0 : merrillOptionTradesToEdit.Rows.Count;
                int merrillFuturesEdits = (merrillFuturesTradesToEdit == null) ? 0 : merrillFuturesTradesToEdit.Rows.Count;

                if (ShouldProceedWithUpdates(hugoStockEdits, hugoOptionEdits, hugoFuturesEdits, merrillStockEdits, merrillOptionEdits, merrillFuturesEdits))
                {
                    int editsDone = 0;
                    Enabled = false;

                    try
                    {
                        editsDone = ProceedWithUpdates(hugoStockTradesToEdit,
                            hugoOptionTradesToEdit,
                            hugoFuturesTradesToEdit,
                            merrillStockTradesToEdit,
                            merrillOptionTradesToEdit,
                            merrillFuturesTradesToEdit,
                            hugoStockEdits,
                            hugoOptionEdits,
                            hugoFuturesEdits,
                            merrillStockEdits,
                            merrillOptionEdits,
                            merrillFuturesEdits);
                    }
                    finally
                    {
                        Enabled = true;
                    }

                    if (editsDone > 0)
                    {
                        MessageBox.Show(String.Format("Committed {0} trade(s)", editsDone), "Update",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // need to refresh even if edit failed, so we will restore previous values
                    hugoStockTradesDataGridStateManager.NeedToRefresh = (hugoStockEdits + hugoOptionEdits) > 0;
                    hugoOptionTradesDataGridStateManager.NeedToRefresh = hugoOptionEdits > 0;
                    hugoFuturesTradesDataGridStateManager.NeedToRefresh = hugoFuturesEdits > 0;
                    merrillStockTradesDataGridStateManager.NeedToRefresh = merrillStockEdits > 0;
                    merrillOptionTradesDataGridStateManager.NeedToRefresh = merrillOptionEdits > 0;
                    merrillFuturesTradesDataGridStateManager.NeedToRefresh = merrillFuturesEdits > 0;
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error committing changes", ex);
                buttonCancelChanges_Click(sender, e);
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }
        }

        #endregion

        #region Settings event handlers
        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            settingsChanged = true;
            Utilities.LiquidConnectionString = Properties.Settings.Default.LiquidConnectionString;
        }
        #endregion

        #region Utilities event handlers
        private void Utilities_OnError(object sender, ErrorEventArgs e)
        {
            HandleError(e.Message, e.Exception);
        }
        private void Utilities_OnInfo(object sender, InfoEventArgs e)
        {
            logger.Info(e.Message);
        }
        private void Utilities_OnTablesUpdated(object sender, TablesUpdatedEventArgs e)
        {
            stockPositionsDataGridStateManager.NeedToRefresh |= e.StockPositionsTableUpdated;
            optionPositionsDataGridStateManager.NeedToRefresh |= e.OptionPositionsTableUpdated;
            futuresPositionsDataGridStateManager.NeedToRefresh |= e.FuturesPositionsTableUpdated;
            hugoStockTradesDataGridStateManager.NeedToRefresh |= e.HugoStockTradesTableUpdated;
            hugoOptionTradesDataGridStateManager.NeedToRefresh |= e.HugoOptionTradesTableUpdated;
            hugoFuturesTradesDataGridStateManager.NeedToRefresh |= e.HugoFuturesTradesTableUpdated;
            merrillStockTradesDataGridStateManager.NeedToRefresh |= e.MerrillStockTradesTableUpdated;
            merrillOptionTradesDataGridStateManager.NeedToRefresh |= e.MerrillOptionTradesTableUpdated;
            merrillFuturesTradesDataGridStateManager.NeedToRefresh |= e.MerrillFuturesTradesTableUpdated;
            todaysMerrillCorrectionsDataGridStateManager.NeedToRefresh |= e.ConfirmationCorrectionsTodayTableUpdated;
            yesterdaysMerrillCorrectionsDataGridStateManager.NeedToRefresh |= e.ConfirmationCorrectionsYesterdayTableUpdated;
            hugoStockCorrectionsDataGridStateManager.NeedToRefresh |= e.HugoStockCorrectionsTableUpdated;
            hugoFuturesCorrectionsDataGridStateManager.NeedToRefresh |= e.HugoFuturesCorrectionsTableUpdated;
            hugoOptionCorrectionsDataGridStateManager.NeedToRefresh |= e.HugoOptionCorrectionsTableUpdated;
 
            if (e.SubAccountNamesTableUpdated && (AccountGroupName != null))
            {
                subaccountNamesDataTableBindingSource.DataSource = Utilities.SubaccountNames;
                formSubAccounts.NeedToRefresh = true;
            }
            if (e.AccountGroupsTableUpdated)
            {
                FillAccountGroupComboBox();
            }
            if (e.BrokersTableUpdated)
            {
                brokersDataTableBindingSource.DataSource = Utilities.Brokers;
            }
            if (e.ExchangesTableUpdated)
            {
                exchangesDataTableBindingSource.DataSource = Utilities.Exchanges;
            }
            if (e.OptionTradeReasonsTableUpdated)
            {
                optionTradeReasonsDataTableBindingSource.DataSource = Utilities.OptionTradeReasons;
            }
            if (e.StockTradeReasonsTableUpdated)
            {
                stockTradeReasonsDataTableBindingSource.DataSource = Utilities.StockTradeReasons;
            }
            if (e.TradeMediaTableUpdated)
            {
                tradeMediaDataTableBindingSource.DataSource = Utilities.TradeMedia;
            }
            if (e.TradersTableUpdated && (AccountGroupName != null))
            {
                tradersDataTableBindingSource.DataSource = Utilities.Traders;
            }
            if ((e.SymbolMappingsTableUpdated) && (formSymbolMappings != null))
            {
                formSymbolMappings.NeedToRefresh = true;
            }
        }
        #endregion

        #region Context menu event handlers
        private void OnConsolidateTrades(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Consolidate selected trade(s)?", "Condolidate Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int nAlteredTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    List<int> tradeIds = new List<int>();
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        IOptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(rowView, currentDataGridView);
                        if (optionTrade.TradeId.HasValue)
                        {
                            tradeIds.Add(optionTrade.TradeId.Value);
                            nAlteredTrades++;
                        }
                    }

                    if (nAlteredTrades > 1)
                    {
                        if (0 == Utilities.ConsolidateTrades(true, false, tradeIds.ToArray()))
                        {
                            MessageBox.Show(String.Format("Consolidated {0} trade(s)", nAlteredTrades), "Condolidate Trades",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGrids();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error consolidating trades", ex);
            }
        }
        private void OnDeconsolidateTrades(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Deconsolidate selected trade(s)?", "Decondolidate Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int nAlteredTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        IOptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(rowView, currentDataGridView);
                        if (optionTrade.ConsolidationPackageId.HasValue)
                        {
                            if (optionTrade.ConsolidationPackageId.Value >= 0)
                            {
                                if (0 == Utilities.DeconsolidateTrades(optionTrade.ConsolidationPackageId.Value))
                                {
                                    nAlteredTrades++;
                                }
                            }
                        }
                    }

                    if (nAlteredTrades > 0)
                    {
                        MessageBox.Show(String.Format("Deconsolidated {0} trade(s)", nAlteredTrades), "Decondolidate Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error deconsolidating trades", ex);
            }
        }
        private void OnAddHugoOptionTradesToMerrill(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Add selected trade(s) to " + clearingHouseText + "?", "Add Trades", out note))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        if (0 == Utilities.AddOptionTradeConfirmation(GetOptionTradeFromDataGridViewRow(rowView, currentDataGridView), note))
                        {
                            nAddedTrades++;
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to " + clearingHouseText, nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding Hugo option trades to " + clearingHouseText, ex);
            }
        }

        private void OnAddHugoStockTradesToMerrill(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Add selected trade(s) to " + clearingHouseText + "?", "Add Trades", out note))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        if (0 == Utilities.AddStockTradeConfirmation(GetStockTradeFromDataGridViewRow(rowView, currentDataGridView), note))
                        {
                            nAddedTrades++;
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to " + clearingHouseText + "", nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding Hugo stock trades to " + clearingHouseText, ex);
            }
        }
        private void OnAddHugoFuturesTradesToMerrill(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Add selected trade(s) to " + clearingHouseText + "?", "Add Trades", out note))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        if (0 == Utilities.AddFuturesTradeConfirmation(GetFuturesTradeFromDataGridViewRow(rowView, currentDataGridView), note))
                        {
                            nAddedTrades++;
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to " + clearingHouseText + "", nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding Hugo stock trades to " + clearingHouseText, ex);
            }
        }

        private void OnDeleteMerrillCorrection(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Delete selected correction(s)?", "Delete Correction", out note))
                {
                    int deletedCorrections = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow row =
                            (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow;
                        if (0 == Utilities.DeleteConfirmation(row.RecordId, note))
                        {
                            deletedCorrections++;
                        }
                    }

                    if (deletedCorrections > 0)
                    {
                        MessageBox.Show(String.Format("Deleted {0} corrections(s)", deletedCorrections), "Delete Corrections",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error deleting " + clearingHouseText + " correction", ex);
            }
        }
        private void OnRedoMerrillCorrectionToDate(object sender, EventArgs e)
        {
            try
              {
                  // display a form to enter the data
                  using (FormEditDateTime formEdit = new FormEditDateTime("Date of correction:"))
                  {
                      formEdit.NewValue = Convert.ToDateTime(CurrentDate.AddDays(1));
                      formEdit.SetMinDate(CurrentDate.AddDays(1));
                      formEdit.SetMaxDate(DateTime.Today);

                      if (DialogResult.OK == formEdit.ShowDialog())
                      {
                          int addedCorrections = 0;

                          // get state manager for the datagridview generating this call
                          DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                          foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                          {
                              ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow row =
                                  (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow;
                              if (0 == Utilities.RedoCorrectionToDate(row.RecordId, formEdit.NewValue))
                              {
                                  addedCorrections++;
                              }
                          }

                          if (addedCorrections > 0)
                          {
                              MessageBox.Show(String.Format("Re-did {0} corrections(s)", addedCorrections), "Re-do Corrections",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                              RefreshDataGrids();
                          }
                      }
                  }
            }
            catch (Exception ex)
            {
                HandleError("Error redoing " + clearingHouseText +" correction for specific date", ex);
            }
      }
        private void OnRedoMerrillCorrection(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Re-do correction(s)?", "Re-do Corrections",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int addedCorrections = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow row =
                            (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow;
                        if (0 == Utilities.RedoCorrection(row.RecordId))
                        {
                            addedCorrections++;
                        }
                    }

                    if (addedCorrections > 0)
                    {
                        MessageBox.Show(String.Format("Re-did {0} corrections(s)", addedCorrections), "Re-do Corrections",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error redoing " + clearingHouseText +" correction", ex);
            }
        }

        private void OnUndoHugoStockCorrection(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Undo selected correction(s)?", "Undo Correction",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int deletedCorrections = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow row =
                            (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.HugoStockCorrectionsRow;
                        if (0 == Utilities.UndoHugoStockCorrection(row.RecordId))
                        {
                            deletedCorrections++;
                        }
                    }

                    if (deletedCorrections > 0)
                    {
                        MessageBox.Show(String.Format("Undid {0} corrections(s)", deletedCorrections), "Undo Corrections",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error undoing Hugo stock correction", ex);
            }
        }

        private void OnUndoHugoFuturesCorrection(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Undo selected correction(s)?", "Undo Correction",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int deletedCorrections = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow row =
                            (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.HugoFuturesCorrectionsRow;
                        if (0 == Utilities.UndoHugoFuturesCorrection(row.RecordId))
                        {
                            deletedCorrections++;
                        }
                    }

                    if (deletedCorrections > 0)
                    {
                        MessageBox.Show(String.Format("Undid {0} corrections(s)", deletedCorrections), "Undo Corrections",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error undoing Hugo futures correction", ex);
            }
        }
        private void OnEditCorrectionNote(object sender, EventArgs e)
        {
            try
            {
                using (FormEditText formEdit = new FormEditText(null))
                {
                    if (DialogResult.OK == formEdit.ShowDialog())
                    {
                        // get state manager for the datagridview generating this call
                        DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                        foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                        {
                            ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow row =
                              (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.ConfirmationCorrectionsRow;
                            Utilities.EditCorrectionNote(row.RecordId, formEdit.NewValue);
                        }

                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing correction note", ex);
            }
        }

        private void OnUndoHugoOptionCorrection(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Undo selected correction(s)?", "Undo Correction",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int deletedCorrections = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow row =
                            (rowView.DataBoundItem as DataRowView).Row as ReconciliationLib.HugoDataSet.HugoOptionCorrectionsRow;
                        if (0 == Utilities.UndoHugoOptionCorrection(row.RecordId))
                        {
                            deletedCorrections++;
                        }
                    }

                    if (deletedCorrections > 0)
                    {
                        MessageBox.Show(String.Format("Undid {0} corrections(s)", deletedCorrections), "Undo Corrections",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error undoing Hugo option correction", ex);
            }
        }

        private void OnClearAcceptedStockDiscrepancy(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                bool refresh = false;
                foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                {
                    HugoDataSet.StockPositionsRow row = ((DataRowView)rowView.DataBoundItem).Row as HugoDataSet.StockPositionsRow;
                    if (row.AcceptedOrZeroIfNull != 0)
                    {
                        refresh |= (0 == Utilities.AcceptStockDiscrepancy(row.StockSymbol, row.SubAcctName,
                            0, null));
                    }
                }

                if (refresh)
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error clearing accepted stock discrepancy", ex);
            }
        }
        private void OnClearAcceptedFuturesDiscrepancy(object sender, EventArgs e)
        {
            // TODO - implement
        }
        private void OnAcceptStockDiscrepancy(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                using (FormAcceptDiscrepancy form = new FormAcceptDiscrepancy())
                {
                    if (DialogResult.OK == form.ShowDialog())
                    {
                        bool refresh = false;
                        foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                        {
                            HugoDataSet.StockPositionsRow row = ((DataRowView)rowView.DataBoundItem).Row as HugoDataSet.StockPositionsRow;
                            if (row.Discrepancy != 0)
                            {
                                refresh |= (0 == Utilities.AcceptStockDiscrepancy(row.StockSymbol, row.SubAcctName,
                                    row.AcceptanceAdjustment, form.Note));
                            }
                        }

                        if (refresh)
                        {
                            RefreshDataGrids();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error accepting stock discrepancy", ex);
            }
        }
        private void OnAcceptFuturesDiscrepancy(object sender, EventArgs e)
        {
            // TODO - implement 
        }
        private void OnClearAcceptedOptionDiscrepancy(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                bool refresh = false;
                foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                {
                    HugoDataSet.OptionPositionsRow row = ((DataRowView)rowView.DataBoundItem).Row as HugoDataSet.OptionPositionsRow;
                    if (row.AcceptedOrZeroIfNull != 0)
                    {
                        refresh |= (0 == Utilities.AcceptOptionDiscrepancy(row.OptionSymbol, row.ExpirationDate, row.StrikePrice, row.OptionType, row.SubAcctName,
                            0, null));
                    }
                }

                if (refresh)
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error clearing accepted option discrepancy", ex);
            }
        }
        private void OnAcceptOptionDiscrepancy(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                using (FormAcceptDiscrepancy form = new FormAcceptDiscrepancy())
                {
                    if (DialogResult.OK == form.ShowDialog())
                    {
                        bool refresh = false;
                        foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                        {
                            HugoDataSet.OptionPositionsRow row = ((DataRowView)rowView.DataBoundItem).Row as HugoDataSet.OptionPositionsRow;
                            if (row.Discrepancy != 0)
                            {
                                refresh |= (0 == Utilities.AcceptOptionDiscrepancy(row.OptionSymbol, row.ExpirationDate, row.StrikePrice, row.OptionType, row.SubAcctName,
                                    row.AcceptanceAdjustment, form.Note));
                            }
                        }

                        if (refresh)
                        {
                            RefreshDataGrids();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error accepting option discrepancy", ex);
            }
        }

        private void OnMatchOptionPositions(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManagerForOptionPositions stateManager = GetStateManagerFromDataGridView(currentDataGridView) as DataGridStateManagerForOptionPositions;
                if (0 == Utilities.MatchOptionPositions(stateManager.ImportDate.Value, stateManager.RecordId, stateManager.HugoOptionId, stateManager.HugoUnderlyingId))
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error matching option positions", ex);
            }
        }
        private void OnUnmatchOptionPositions(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManagerForOptionPositions stateManager = GetStateManagerFromDataGridView(currentDataGridView) as DataGridStateManagerForOptionPositions;
                if (0 == Utilities.MatchOptionPositions(stateManager.ImportDate.Value, stateManager.RecordId, null, null))
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error unmatching option positions", ex);
            }
        }
        private void OnAddSymbolMapping(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManagerForStockPositions stateManager = GetStateManagerFromDataGridView(currentDataGridView) as DataGridStateManagerForStockPositions;
                if (0 == Utilities.AddSymbolMapping(stateManager.SelectedMerrillSymbol, stateManager.SelectedHugoSymbol))
                {
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding symbol mapping", ex);
            }
        }
        private void OnDeleteSymbolMapping(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManagerForStockPositions stateManager = GetStateManagerFromDataGridView(currentDataGridView) as DataGridStateManagerForStockPositions;
                HugoDataSet.SymbolMappingsRow row = Utilities.SymbolMappings.FindByMerrillSymbol(stateManager.SelectedMerrillSymbol);
                if (row == null)
                {
                    MessageBox.Show(String.Format("Mapping for " + clearingHouseText +" symbol {0} not found", stateManager.SelectedMerrillSymbol),
                        "Delete Symbol Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (0 == Utilities.DeleteSymbolMapping(row.MappingId))
                    {
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error deleting symbol mapping", ex);
            }
        }

        private void OnAddMerrillOptionTradesToHugo(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Add selected trade(s) to Hugo?", "Add Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        OptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(rowView, currentDataGridView);
                        if (VerifyOptionTrade(optionTrade))
                        {
                            if (null != Utilities.AddOptionTrade(optionTrade))
                            {
                                nAddedTrades++;
                            }
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to Hugo", nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding " + clearingHouseText +" option trade to Hugo", ex);
            }
        }

        private void OnAddMerrillStockTradesToHugo(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Add selected trade(s) to Hugo?", "Add Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        StockTrade stockTrade = GetStockTradeFromDataGridViewRow(rowView, currentDataGridView);
                        if (VerifyStockTrade(stockTrade, null))
                        {
                            if (null != Utilities.AddStockTrade(stockTrade))
                            {
                                nAddedTrades++;
                            }
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to Hugo", nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding " + clearingHouseText +" stock trade to Hugo", ex);
            }
        }
        private void OnAddMerrillFuturesTradesToHugo(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Add selected trade(s) to Hugo?", "Add Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int nAddedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        FuturesTrade stockTrade = GetFuturesTradeFromDataGridViewRow(rowView, currentDataGridView);
                        if (VerifyFuturesTrade(stockTrade, null))
                        {
                            if (null != Utilities.AddFuturesTrade(stockTrade))
                            {
                                nAddedTrades++;
                            }
                        }
                    }

                    if (nAddedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} trade(s) to Hugo", nAddedTrades), "Add Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding " + clearingHouseText + " stock trade to Hugo", ex);
            }
        }
        private void OnDeleteTrade(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Delete selected trade(s)?", "Delete Trades",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    int deletedTrades = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        if (stateManager is DataGridStateManagerForOptionTrades)
                        {
                            foreach(IOptionTrade optionTrade in ExpandOptionTrades(GetOptionTradeFromDataGridViewRow(rowView, currentDataGridView)))
                            {
                                deletedTrades += DeleteOptionTrade(optionTrade) ? 1 : 0;
                            }
                        }
                        else if (stateManager is DataGridStateManagerForStockTrades)
                        {
                            StockTrade stockTrade = GetStockTradeFromDataGridViewRow(rowView, currentDataGridView);
                            if (VerifyStockTrade(stockTrade, null))
                            {
                                if (0 == Utilities.DeleteStockTrade(stockTrade))
                                {
                                    deletedTrades++;
                                }
                            }
                        }
                        else if (stateManager is DataGridStateManagerForFuturesTrades)
                        {
                            FuturesTrade futuresTrade = GetFuturesTradeFromDataGridViewRow(rowView, currentDataGridView);
                            if (VerifyFuturesTrade(futuresTrade, null))
                            {
                                if (0 == Utilities.DeleteFuturesTrade(futuresTrade))
                                {
                                    deletedTrades++;
                                }
                            }
                        }
                    }

                    if (deletedTrades > 0)
                    {
                        MessageBox.Show(String.Format("Deleted {0} trade(s)", deletedTrades), "Delete Trades",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error deleting trade", ex);
            }
        }
 
        private void OnDeleteConfirmation(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Delete selected trade confirmation(s)?", "Delete Trade Confirmations", out note))
                {
                    int deletedConfirmations = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        // don't delete an already deleted record
                        DataRow row = ((DataRowView)rowView.DataBoundItem).Row;
                        if (Convert.ToString(row["RecordType"]) != "DK")
                        {
                            if (0 == Utilities.DeleteConfirmation(Convert.ToInt32(row["TradeId"]), note))
                            {
                                deletedConfirmations++;
                            }
                        }
                    }

                    if (deletedConfirmations > 0)
                    {
                        MessageBox.Show(String.Format("Deleted {0} trade confirmation(s)", deletedConfirmations), "Delete Trade Confirmations",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error deleting confirmation", ex);
            }
        }

        private void OnRestoreConfirmation(object sender, EventArgs e)
        {
            try
            {
                string note;
                if (DialogResult.Yes == MessageBoxWithNote.Show("Restore selected trade confirmation(s)?", "Restore Trade Confirmations", out note))
                {
                    int restoredConfirmations = 0;

                    // get state manager for the datagridview generating this call
                    DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        // can't restore unless this confirmation has been deleted
                        DataRow row = ((DataRowView)rowView.DataBoundItem).Row;
                        if (Convert.ToString(row["RecordType"]) == "DK")
                        {
                            if (0 == Utilities.DeleteConfirmation(Convert.ToInt32(row["TradeId"]), note))
                            {
                                restoredConfirmations++;
                            }
                        }
                    }

                    if (restoredConfirmations > 0)
                    {
                        MessageBox.Show(String.Format("Restored {0} trade confirmation(s)", restoredConfirmations), "Restore Trade Confirmations",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error restoring confirmation", ex);
            }
        }

        private void OnCopyOption(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // exactly one row should be selected
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("OnCopyOption should not execute unless exactly one row is selected");

                clipboardOption = new Option((DataRowView)stateManager.RowsToProcess[0].DataBoundItem);
                Clipboard.SetText(clipboardOption.ToString());
            }
            catch (Exception ex)
            {
                HandleError("Error copying option", ex);
            }
        }

        private void OnPasteOption(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be an option in the clipboard
                if (clipboardOption == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[FindColumnIndexByDataPropertyName(currentDataGridView, "UnderlyingSymbol")].Value = clipboardOption.UnderlyingSymbol;
                    row.Cells[FindColumnIndexByDataPropertyName(currentDataGridView, "OptionSymbol")].Value = clipboardOption.OptionSymbol;
                    row.Cells[FindColumnIndexByDataPropertyName(currentDataGridView, "ExpirationDate")].Value = clipboardOption.ExpirationDate;
                    row.Cells[FindColumnIndexByDataPropertyName(currentDataGridView, "StrikePrice")].Value = clipboardOption.StrikePrice;
                    row.Cells[FindColumnIndexByDataPropertyName(currentDataGridView, "OptionType")].Value = clipboardOption.OptionType;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error pasting option", ex);
            }
        }

        private void OnPasteStockTrade(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be a stock trade in the clipboard
                if (clipboardStockTrade == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                int tradeTypeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeTypeName");
                int tradeVolumeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeVolume");
                int tradePriceIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradePrice");
                int totalCostIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TotalCost");
                int commissionIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "Commission");
                int SECFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "SECFee");
                if (SECFeeIndex < 0)
                    SECFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "SEC Fee");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[tradeTypeIndex].Value = clipboardStockTrade.TradeType
                        + (clipboardStockTrade.ShortFlag ? " short" : "");
                    row.Cells[tradeVolumeIndex].Value = clipboardStockTrade.FullVolume;
                    row.Cells[tradePriceIndex].Value = clipboardStockTrade.TradePrice;
                    row.Cells[commissionIndex].Value = clipboardStockTrade.Commission;
                    row.Cells[SECFeeIndex].Value = clipboardStockTrade.SECFee;
                    row.Cells[totalCostIndex].Value = clipboardStockTrade.TotalCost;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error pasting stock trade", ex);
            }
        }
        private void OnPasteFuturesTrade(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be a futures trade in the clipboard
                if (clipboardFuturesTrade == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                int tradeTypeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeTypeName");
                int tradeVolumeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeVolume");
                int tradePriceIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradePrice");
                int totalCostIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TotalCost");
                int commissionIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "Commission");
                int nfaFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "NFAFee");
                int clearingFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "ClearingFee");
                int exchangeFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "ExchangeFee");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[tradeTypeIndex].Value = clipboardFuturesTrade.TradeType
                        + (clipboardFuturesTrade.ShortFlag ? " short" : "");
                    row.Cells[tradeVolumeIndex].Value = clipboardFuturesTrade.FullVolume;
                    row.Cells[tradePriceIndex].Value = clipboardFuturesTrade.TradePrice;
                    row.Cells[totalCostIndex].Value = clipboardFuturesTrade.TotalCost;
                    row.Cells[commissionIndex].Value = clipboardFuturesTrade.Commission;
                    row.Cells[nfaFeeIndex].Value = clipboardFuturesTrade.NFAFee;
                    row.Cells[clearingFeeIndex].Value = clipboardFuturesTrade.ClearingFee;
                    row.Cells[exchangeFeeIndex].Value = clipboardFuturesTrade.ExchangeFee;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error pasting stock trade", ex);
            }
        }
        private void OnDistributeOptionTrade(object sender, EventArgs e)
        {
            try
            {
                  // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be an option trade in the clipboard
                if (clipboardOptionTrade == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                double fullVolume = Math.Abs(clipboardOptionTrade.TradeVolume);
                if (fullVolume <= 0)
                    throw new ReconciliationException("Cannot distribute a trade with zero volume");

                int tradeTypeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeTypeName");
                int tradeVolumeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeVolume");
                int tradePriceIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradePrice");
                int totalCostIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TotalCost");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[tradeTypeIndex].Value = clipboardOptionTrade.TradeType;
                    row.Cells[tradePriceIndex].Value = clipboardOptionTrade.TradePrice;
                    row.Cells[totalCostIndex].Value = clipboardOptionTrade.TotalCost *
                        Math.Abs(Convert.ToDouble(row.Cells[tradeVolumeIndex].Value)) / fullVolume;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error distributing option trade", ex);
            }
      }
        private void OnDistributeStockTrade(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be a stock trade in the clipboard
                if (clipboardStockTrade == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                double fullVolume = Math.Abs(clipboardStockTrade.FullVolume);
                if (fullVolume <= 0)
                    throw new ReconciliationException("Cannot distribute a trade with zero volume");

                int tradeTypeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeTypeName");
                int tradeVolumeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeVolume");
                int tradePriceIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradePrice");
                int totalCostIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TotalCost");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[tradeTypeIndex].Value = clipboardStockTrade.TradeType
                        + (clipboardStockTrade.ShortFlag ? " short" : "");
                    row.Cells[tradePriceIndex].Value = clipboardStockTrade.TradePrice;
                    row.Cells[totalCostIndex].Value = clipboardStockTrade.TotalCost *
                        Math.Abs(Convert.ToDouble(row.Cells[tradeVolumeIndex].Value)) / fullVolume;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error distributing stock trade", ex);
            }
        }

        private void OnPasteOptionTrade(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // should be an option trade in the clipboard
                if (clipboardOptionTrade == null)
                    throw new ReconciliationException("Nothing on clipboard to paste");

                int tradeTypeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeTypeName");
                int tradeVolumeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradeVolume");
                int tradePriceIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TradePrice");
                int totalCostIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "TotalCost");
                int commissionIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "Commission");
                int SECFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "SECFee");
                int ORFFeeIndex = FindColumnIndexByDataPropertyName(currentDataGridView, "ORFFee");

                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Cells[tradeTypeIndex].Value = clipboardOptionTrade.TradeType;
                    row.Cells[tradeVolumeIndex].Value = clipboardOptionTrade.TradeVolume;
                    row.Cells[tradePriceIndex].Value = clipboardOptionTrade.TradePrice;
                    row.Cells[totalCostIndex].Value = clipboardOptionTrade.TotalCost;
                    row.Cells[commissionIndex].Value = clipboardOptionTrade.Commission;
                    row.Cells[SECFeeIndex].Value = clipboardOptionTrade.SECFee;
                    row.Cells[ORFFeeIndex].Value = clipboardOptionTrade.ORFFee;
                }

                stateManager.EndEdit();
                EnableControls();
            }
            catch (Exception ex)
            {
                HandleError("Error pasting option trade", ex);
            }
        }

         private void OnCopyMerrillStockTrade(object sender, EventArgs e)
        {
            try
            {
                CopyStockTrade();
                Clipboard.SetText(clearingHouseText +" knows: " + clipboardStockTrade.ToString());
            }
            catch (Exception ex)
            {
                HandleError("Error copying " + clearingHouseText +" stock trade", ex);
            }
        }
         private void OnCopyMerrillFuturesTrade(object sender, EventArgs e)
         {
             try
             {
                 CopyFuturesTrade();
                 Clipboard.SetText(clearingHouseText + " knows: " + clipboardFuturesTrade.ToString());
             }
             catch (Exception ex)
             {
                 HandleError("Error copying " + clearingHouseText + " stock trade", ex);
             }
         }
         private void OnCopyHugoStockTrade(object sender, EventArgs e)
         {
             try
             {
                 CopyStockTrade();
                 Clipboard.SetText("Hugo knows: " + clipboardStockTrade.ToString());
             }
             catch (Exception ex)
             {
                 HandleError("Error copying Hugo stock trade", ex);
             }
         }
         private void OnCopyHugoFuturesTrade(object sender, EventArgs e)
         {
             try
             {
                 CopyFuturesTrade();
                 Clipboard.SetText("Hugo knows: " + clipboardFuturesTrade.ToString());
             }
             catch (Exception ex)
             {
                 HandleError("Error copying Hugo stock trade", ex);
             }
         }

 
        private void OnCopyMerrillOptionTrade(object sender, EventArgs e)
        {
            try
            {
                CopyOptionTrade();
                Clipboard.SetText(clearingHouseText +" knows: " + clipboardOptionTrade.ToString());
            }
            catch (Exception ex)
            {
                HandleError("Error copying " + clearingHouseText +" option trade", ex);
            }
        }
        private void OnCopyHugoOptionTrade(object sender, EventArgs e)
        {
            try
            {
                CopyOptionTrade();
                Clipboard.SetText("Hugo knows: " + clipboardOptionTrade.ToString());
            }
            catch (Exception ex)
            {
                HandleError("Error copying Hugo option trade", ex);
            }
        }
         private void OnCopyHugoStockCorrection(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // exactly one row should be selected
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("CopyHugoStockCorrection should not execute unless exactly one row is selected");

                StockTrade stockTrade = GetStockTradeFromDataGridViewRow(stateManager.RowsToProcess[0], currentDataGridView);
                Clipboard.SetText(stockTrade.ToString() + " " + stockTrade.TradeNote);
            }
            catch (Exception ex)
            {
                HandleError("Error copying Hugo stock correction", ex);
            }
        }
         private void OnCopyHugoFuturesCorrection(object sender, EventArgs e)
         {
             try
             {
                 // get state manager for the datagridview generating this call
                 DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                 // exactly one row should be selected
                 if (stateManager.RowsToProcess.Length != 1)
                     throw new ReconciliationException("CopyHugoFuturesCorrection should not execute unless exactly one row is selected");

                 FuturesTrade futuresTrade = GetFuturesTradeFromDataGridViewRow(stateManager.RowsToProcess[0], currentDataGridView);
                 Clipboard.SetText(futuresTrade.ToString() + " " + futuresTrade.TradeNote);
             }
             catch (Exception ex)
             {
                 HandleError("Error copying Hugo futures correction", ex);
             }
         }
         private void OnCopyHugoOptionCorrection(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // exactly one row should be selected
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("CopyHugoOptionCorrection should not execute unless exactly one row is selected");

                OptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(stateManager.RowsToProcess[0], currentDataGridView);
                Clipboard.SetText(optionTrade.ToString() + " " + optionTrade.TradeNote);
            }
            catch (Exception ex)
            {
                HandleError("Error copying Hugo option correction", ex);
            }
        }
        private void OnCopyConfirmationCorrection(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // exactly one row should be selected
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("CopyConfirmationCorrection should not execute unless exactly one row is selected");

                HugoDataSet.ConfirmationCorrectionsRow row = ConvertExt.ToRow<HugoDataSet.ConfirmationCorrectionsRow>(stateManager.RowsToProcess[0]);
                if (row.IsOptionSymbolNull())
                {
                    StockTrade stockTrade = new StockTrade(row);
                    Clipboard.SetText(stockTrade.ToString() + " " + stockTrade.TradeNote);
                }
                else
                {
                    OptionTrade optionTrade = new OptionTrade(row);
                    Clipboard.SetText(optionTrade.ToString() + " " + optionTrade.TradeNote);
                }
            }
            catch (Exception ex)
            {
                HandleError("Error copying confirmation correction", ex);
            }
        }
        private void OnShowAssociatedOptionTrades(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // these exceptions indicate logic errors in this program
                // the command executing this method should be disabled if these conditions exist
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("OnShowAssociatedOptionTrades should not execute unless exactly one row is selected");

                HugoDataSet.HugoStockTradesRow row = ConvertExt.ToRow<HugoDataSet.HugoStockTradesRow>(stateManager.RowsToProcess[0]);
                if (row.IsOptionTradeIdNull())
                    throw new ReconciliationException("OnShowAssociatedOptionTrades should not execute when OptionTradeId is null");

                hugoOptionTradesDataGridStateManager.ShowAll();
                hugoOptionTradesDataTableBindingSource.Filter = String.Format(tradeIdFilter, row.OptionTradeId);
            }
            catch (Exception ex)
            {
                HandleError("Error showing associated option trade", ex);
            }
        }
        private void OnShowAssociatedStockTrades(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // these exceptions indicate logic errors in this program
                // the command executing this method should be disabled if these conditions exist
                if (stateManager.RowsToProcess.Length != 1)
                    throw new ReconciliationException("OnShowAssociatedStockTrades should not execute unless exactly one row is selected");

                HugoDataSet.HugoOptionTradesRow row = ConvertExt.ToRow<HugoDataSet.HugoOptionTradesRow>(stateManager.RowsToProcess[0]);

                hugoStockTradesDataGridStateManager.ShowAll();
                hugoStockTradesDataTableBindingSource.Filter = String.Format(optionTradeIdFilter, row.TradeId);
            }
            catch (Exception ex)
            {
                HandleError("Error showing associated stock trade", ex);
            }
        }
        private void OnChangeOptionSelection(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                SelectedOption = new Option((DataRowView)stateManager.RowsToProcess[0].DataBoundItem);
            }
            catch (Exception ex)
            {
                HandleError("Error changing option selection", ex);
            }
        }
        private void OnChangeStockSelection(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                SelectedUnderlying = new Stock((DataRowView)stateManager.RowsToProcess[0].DataBoundItem);
            }
            catch (Exception ex)
            {
                HandleError("Error changing stock selection", ex);
            }
        }
        private void OnChangeFuturesSelection(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                SelectedFuture = new Future((DataRowView)stateManager.RowsToProcess[0].DataBoundItem);
            }
            catch (Exception ex)
            {
                HandleError("Error changing future selection", ex);
            }
        }

        private void OnHide(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                currentDataGridView.CurrentCell = null;
                foreach (DataGridViewRow row in stateManager.RowsToProcess)
                {
                    row.Selected = row.Visible = false;
                }

                stateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error hiding trades", ex);
            }
        }

        private void OnReverseFilter(object sender, EventArgs e)
        {
            try
            {
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);
                stateManager.ReverseFilter();
            }
            catch (Exception ex)
            {
                HandleError("Error reversing filter", ex);
            }
        }

        private void OnAddStockAdjustment(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                if (DialogResult.Yes == MessageBox.Show("Add a Hugo trade to make Hugo's position match " + clearingHouseText +"'s?", "Add Stock Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    bool refresh = false;
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        HugoDataSet.StockPositionsRow row = ConvertExt.ToRow<HugoDataSet.StockPositionsRow>(rowView);
                        if (row.Discrepancy != 0)
                        {
                            refresh |= (null != Utilities.AddStockTrade(new StockTrade(row)));
                        }
                    }

                    if (refresh)
                    {
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding stock adjustment", ex);
            }
        }

        private void OnAddOptionAdjustment(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                if (DialogResult.Yes == MessageBox.Show("Add a Hugo trade to make Hugo's position match " + clearingHouseText + "'s?", "Add Stock Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    bool refresh = false;
                    foreach (DataGridViewRow rowView in stateManager.RowsToProcess)
                    {
                        HugoDataSet.OptionPositionsRow row = ConvertExt.ToRow<HugoDataSet.OptionPositionsRow>(rowView);
                        if (row.Discrepancy != 0)
                        {
                            refresh |= (null != Utilities.AddOptionTrade(new OptionTrade(row)));
                        }
                    }

                    if (refresh)
                    {
                        RefreshDataGrids();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error adding option adjustment", ex);
            }
        }
        private void OnAddFuturesAdjustment(object sender, EventArgs e)
        {
            // TODO - implement
        }

        private void OnSelectAll(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in currentDataGridView.Rows)
                {
                    row.Selected = row.Visible;
                }
            }
            catch (Exception ex)
            {
                HandleError("Error sellecting all rows", ex);
            }
        }

        private void OnGenericCommand(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                // set the selected cells to the commandname (e.g., "Buy" or "Sell")
                foreach (DataGridViewCell cell in stateManager.CellsToProcess)
                {
                    cell.Value = sender.ToString();
                }
                stateManager.EndEdit();
            }
            catch (Exception ex)
            {
                HandleError("Error executing command", ex);
            }
        }

        private void OnEditText(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                DataGridViewCell[] cellsToProcess = stateManager.CellsToProcess;
                if (cellsToProcess.Length > 0)
                {
                    DataGridViewColumn column = currentDataGridView.Columns[cellsToProcess[0].ColumnIndex];

                    // display a form to enter the data
                    // column's Tag contains either null or a VerifyValue delegate, which we execute
                    //   to verify the data
                    using (FormEditText formEdit = new FormEditText(column.Tag))
                    {
                        if (DialogResult.OK == formEdit.ShowDialog())
                        {
                            // update the value of each selected cell
                            foreach (DataGridViewCell cell in cellsToProcess)
                            {
                                cell.Value = formEdit.NewValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing text", ex);
            }
        }

        private void OnEditDateTime(object sender, EventArgs e)
        {
            try
            {
                // get state manager for the datagridview generating this call
                DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

                DataGridViewCell[] cellsToProcess = stateManager.CellsToProcess;
                if (cellsToProcess.Length > 0)
                {
                    // display a form to enter the data
                    using (FormEditDateTime formEdit = new FormEditDateTime())
                    {
                        formEdit.NewValue = Convert.ToDateTime(cellsToProcess[0].Value);
                        if (DialogResult.OK == formEdit.ShowDialog())
                        {
                            // update the value of each selected cell
                            foreach (DataGridViewCell cell in cellsToProcess)
                            {
                                cell.Value = formEdit.NewValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Error editing date/time", ex);
            }
        }

        private void OnMatchOptionTrades(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoOptionTrades.CurrentCell = null;
                dataGridViewMerrillOptionTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoOptionTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillOptionTradeRow(GetOptionTradeFromDataGridViewRow(hugoRow, dataGridViewHugoOptionTrades), /*match totalcost =*/ false);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoOptionTradesDataGridStateManager.RefreshStatusStrip();
                merrillOptionTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching option trades", ex);
            }
        }

        private void OnHideDoneCorrections(object sender, EventArgs e)
        {
            //Enabled = false;
            //try
            //{
            //    // get all trades
            //    merrillOptionTradesDataGridStateManager.ShowAll();
            //    merrillStockTradesDataGridStateManager.ShowAll();
            //    yesterdaysMerrillCorrectionsDataGridStateManager.RemoveFilter();

            //    dataGridViewMerrillOptionTrades.CurrentCell = null;
            //    dataGridViewMerrillStockTrades.CurrentCell = null;
            //    dataGridViewMerrillCorrections2.CurrentCell = null;

            //    foreach (HugoDataSet.ConfirmationCorrectionsRow correctionRow in dataGridViewMerrillCorrections2.Rows)
            //    {
            //        if (correctionRow.Visible)
            //        {
            //            // stock correction
            //            if (String.IsNullOrEmpty(correctionRow.OptionType))
            //            {
            //                if (correctionRow.RecordType == "Add")
            //                {

            //                }
            //                else if (correctionRow.RecordType == "DK")
            //                {
            //                }

            //                else if (correctionRow.RecordType == "Correction")
            //                {
            //                }
            //            }

            //            // option correction
            //            else
            //            {
            //            }
            //            //DataGridViewRow merrillRow = FindMatchingMerrillStockTradeRow(GetStockTradeFromDataGridViewRow(hugoRow, dataGridViewHugoStockTrades), /*match totalcost =*/ false);
            //            //if (merrillRow != null)
            //            //{
            //            //    hugoRow.Selected = hugoRow.Visible = false;
            //            //    merrillRow.Selected = merrillRow.Visible = false;
            //            //}
            //        }
            //    }
            //    hugoStockTradesDataGridStateManager.RefreshStatusStrip();
            //    merrillStockTradesDataGridStateManager.RefreshStatusStrip();
            //}
            //catch (Exception ex)
            //{
            //    HandleError("Error hiding done corrections", ex);
            //}
            //finally
            //{
            //    Enabled = true;
            //}
        }

        private void OnMatchStockTrades(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoStockTrades.CurrentCell = null;
                dataGridViewMerrillStockTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoStockTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillStockTradeRow(GetStockTradeFromDataGridViewRow(hugoRow, dataGridViewHugoStockTrades), /*match totalcost =*/ false);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoStockTradesDataGridStateManager.RefreshStatusStrip();
                merrillStockTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching stock trades", ex);
            }
        }

        private void OnMatchFuturesTrades(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoFuturesTrades.CurrentCell = null;
                dataGridViewMerrillFuturesTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoFuturesTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillFuturesTradeRow(GetFuturesTradeFromDataGridViewRow(hugoRow, dataGridViewHugoFuturesTrades), /*match totalcost =*/ false);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoFuturesTradesDataGridStateManager.RefreshStatusStrip();
                merrillFuturesTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching stock trades", ex);
            }
        }
        private void OnMatchOptionTotalCosts(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoOptionTrades.CurrentCell = null;
                dataGridViewMerrillOptionTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoOptionTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillOptionTradeRow(GetOptionTradeFromDataGridViewRow(hugoRow, dataGridViewHugoOptionTrades), /*match totalcost =*/ true);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoOptionTradesDataGridStateManager.RefreshStatusStrip();
                merrillOptionTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching option total costs", ex);
            }
        }

        private void OnMatchStockTotalCosts(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoStockTrades.CurrentCell = null;
                dataGridViewMerrillStockTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoStockTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillStockTradeRow(GetStockTradeFromDataGridViewRow(hugoRow, dataGridViewHugoStockTrades), /*match totalcost =*/ true);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoStockTradesDataGridStateManager.RefreshStatusStrip();
                merrillStockTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching stock total costs", ex);
            }
        }
        private void OnMatchFuturesTotalCosts(object sender, EventArgs e)
        {
            try
            {
                dataGridViewHugoFuturesTrades.CurrentCell = null;
                dataGridViewMerrillFuturesTrades.CurrentCell = null;

                foreach (DataGridViewRow hugoRow in dataGridViewHugoFuturesTrades.Rows)
                {
                    if (hugoRow.Visible)
                    {
                        DataGridViewRow merrillRow = FindMatchingMerrillFuturesTradeRow(GetFuturesTradeFromDataGridViewRow(hugoRow, dataGridViewHugoFuturesTrades), /*match totalcost =*/ true);
                        if (merrillRow != null)
                        {
                            hugoRow.Selected = hugoRow.Visible = false;
                            merrillRow.Selected = merrillRow.Visible = false;
                        }
                    }
                }
                hugoFuturesTradesDataGridStateManager.RefreshStatusStrip();
                merrillFuturesTradesDataGridStateManager.RefreshStatusStrip();
            }
            catch (Exception ex)
            {
                HandleError("Error matching stock total costs", ex);
            }
        }

        #endregion

        #endregion

        #region Private properties
        private bool AccountGroupSelected
        {
            get { return comboBoxAccountGroup.SelectedIndex >= 0; }
        }
        private string AccountGroupName
        {
            get { return AccountGroupSelected ? (string)comboBoxAccountGroup.SelectedItem : null; }
        }
        private AccountGroupInfo GetAccountGroupInfo()
        {
            AccountGroupInfo info = Utilities.AccountGroupInfo;
            if (info == null)
            {
                comboBoxAccountGroup.SelectedIndex = -1;
                EnableControls();
                return new AccountGroupInfo();  // to avoid a null reference exception
            }
            return info;
        }
        private void GetImportStatusInfo()
        {
            TaskStatus oldStatus = importStatusInfo.LastStatus;
            importStatusInfo = Utilities.GetImportStatus();
            if (importStatusInfo == null)
            {
                importStatusInfo = new ImportStatusInfo();  // to avoid a null reference exception
            }

            // if import has not completed, warn user (unless status is unchanged)
            if ((!TodaysImportDone()) && ((oldStatus != importStatusInfo.LastStatus) || !importStatusShown))
            {
                importStatusShown = true;
                MessageBox.Show(ImportStatusMsg, "Import Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ImportStatusMsg
        {
            get
            {
                if (!TodaysImportDone())
                    switch (importStatusInfo.LastStatus)
                    {
                        case TaskStatus.Started:
                            return "Position import is still running";

                        case TaskStatus.Failed:
                            return "Position import failed";

                        case TaskStatus.Completed:
                            return "Position import has not yet run";

                        default:
                            return "Position import info is unavailable";
                    }
                else
                {
                    return String.Empty;
                }
            }
        }
        private DateTime CurrentDate
        {
            get { return dateTimePickerCurrent.Value; }
        }
        private DateTime PreviousDate
        {
            get { return dateTimePickerPrevious.Value; }
            set { dateTimePickerPrevious.Value = value; }
        }
         #endregion

        #region Private methods

        #region Cast row methods
        private StockTrade GetStockTradeFromDataGridViewRow(DataGridViewRow rowView, DataGridView dataGridView)
        {
            if (dataGridView == dataGridViewMerrillStockTrades)
            {
                return new StockTrade(ConvertExt.ToRow<HugoDataSet.MerrillStockTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoStockTrades)
            {
                return new StockTrade(ConvertExt.ToRow<HugoDataSet.HugoStockTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoCorrections1)
            {
                return new StockTrade(ConvertExt.ToRow<HugoDataSet.HugoStockCorrectionsRow>(rowView));
            }
            return null;
        }
        private FuturesTrade GetFuturesTradeFromDataGridViewRow(DataGridViewRow rowView, DataGridView dataGridView)
        {
            if (dataGridView == dataGridViewMerrillFuturesTrades)
            {
                return new FuturesTrade(ConvertExt.ToRow<HugoDataSet.MerrillFuturesTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoFuturesTrades)
            {
                return new FuturesTrade(ConvertExt.ToRow<HugoDataSet.HugoFuturesTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoFuturesCorrections)
            {
                return new FuturesTrade(ConvertExt.ToRow<HugoDataSet.HugoFuturesCorrectionsRow>(rowView));
            }
            return null;
        }
        private OptionTrade GetOptionTradeFromDataGridViewRow(DataGridViewRow rowView, DataGridView dataGridView)
        {
            if (dataGridView == dataGridViewMerrillOptionTrades)
            {
                return new OptionTrade(ConvertExt.ToRow<HugoDataSet.MerrillOptionTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoOptionTrades)
            {
                return new OptionTrade(ConvertExt.ToRow<HugoDataSet.HugoOptionTradesRow>(rowView));
            }
            else if (dataGridView == dataGridViewHugoCorrections2)
            {
                return new OptionTrade(ConvertExt.ToRow<HugoDataSet.HugoOptionCorrectionsRow>(rowView));
            }
             return null;
        }
        #endregion

        #region Data validation methods
        private static string VerifyTradeVolume(object text)
        {
            try
            {
                if (Convert.ToInt32(text) > 0)
                    return null;
                else
                    return "Value must be greater than zero";
            }
            catch (Exception)
            {
                return "Value must be an integer";
            }
        }
        private static string VerifyStockTradeVolume(object text)
        {
            try
            {
                if (Convert.ToDouble(text) > 0)
                    return null;
                else
                    return "Value must be greater than zero";
            }
            catch (Exception)
            {
                return "Value must be a number";
            }
        }
        private static string VerifyTradePrice(object text)
        {
            try
            {
                if (Convert.ToDouble(text) >= 0)
                    return null;
                else
                    return "Value must be greater than or equal to zero";
            }
            catch (Exception)
            {
                return "Value must be numeric";
            }
        }
        private static string VerifyStrikePrice(object text)
        {
            try
            {
                if (Convert.ToDouble(text) > 0)
                    return null;
                else
                    return "Value must be greater than zero";
            }
            catch (Exception)
            {
                return "Value must be numeric";
            }
        }
        #endregion

        #region Initialization

        private void WireVerificationMethods()
        {
            tradeVolumeDataGridViewTextBoxColumn.Tag = new VerifyValue(VerifyStockTradeVolume);
            tradeVolumeDataGridViewTextBoxColumn1.Tag = new VerifyValue(VerifyTradeVolume);
            tradeVolumeDataGridViewTextBoxColumn2.Tag = new VerifyValue(VerifyTradeVolume);
            tradeVolumeDataGridViewTextBoxColumn3.Tag = new VerifyValue(VerifyTradeVolume);

            tradePriceDataGridViewTextBoxColumn.Tag = new VerifyValue(VerifyTradePrice);
            tradePriceDataGridViewTextBoxColumn1.Tag = new VerifyValue(VerifyTradePrice);
            tradePriceDataGridViewTextBoxColumn2.Tag = new VerifyValue(VerifyTradePrice);
            tradePriceDataGridViewTextBoxColumn3.Tag = new VerifyValue(VerifyTradePrice);
            //concurrentStockPriceDataGridViewTextBoxColumn.Tag = new VerifyValue(VerifyTradePrice);
            //specRateDataGridViewTextBoxColumn.Tag = new VerifyValue(VerifyTradePrice);

            strikePriceDataGridViewTextBoxColumn1.Tag = new VerifyValue(VerifyStrikePrice);
            strikePriceDataGridViewTextBoxColumn2.Tag = new VerifyValue(VerifyStrikePrice);
        }

        private void BuildDataGridStateManagers()
        {
            stockPositionsDataGridStateManager = new DataGridStateManagerForStockPositions(dataGridViewStockPositions,
                GetStockPositions, filterStatusLabel1, showAllLabel1, showNonZeroDiscrepanciesLabel1, showMerrillDetailsLabel1,
                new DataGridViewColumn[] { merrillPositionDataGridViewTextBoxColumn, merrillAdjustmentDataGridViewTextBoxColumn,
                previousMerrillOriginalDataGridViewTextBoxColumn, previousMerrillAdjustmentDataGridViewTextBoxColumn},
                new DataGridViewColumn[] { merrillTotalDataGridViewTextBoxColumn, previousMerrillTotalDataGridViewTextBoxColumn });
            optionPositionsDataGridStateManager = new DataGridStateManagerForOptionPositions(dataGridViewOptionPositions,
                GetOptionPositions, filterStatusLabel2, showAllLabel2, showNonZeroDiscrepanciesLabel2, showMerrillDetailsLabel2,
                  new DataGridViewColumn[] { merrillPositionDataGridViewTextBoxColumn1, merrillAdjustmentDataGridViewTextBoxColumn1,
                previousMerrillPositionDataGridViewTextBoxColumn1, previousMerrillAdjustmentDataGridViewTextBoxColumn1},
                new DataGridViewColumn[] { merrillTotalDataGridViewTextBoxColumn1, previousMerrillTotalDataGridViewTextBoxColumn1 });
            futuresPositionsDataGridStateManager = new DataGridStateManagerForFuturesPositions(dataGridViewFuturesPositions,
                GetFuturesPositions, filterStatusLabel9, showAllLabel9, showNonZeroDiscrepanciesLabel9, showMerrillDetailsLabel9,
               new DataGridViewColumn[] { merrillPositionDataGridViewTextBoxColumn2, merrillAdjustmentDataGridViewTextBoxColumn2,
                previousMerrillPositionDataGridViewTextBoxColumn, previousMerrillAdjustmentDataGridViewTextBoxColumn2},
             new DataGridViewColumn[] { merrillTotalDataGridViewTextBoxColumn2, previousMerrillTotalDataGridViewTextBoxColumn2 });
            hugoStockTradesDataGridStateManager = new DataGridStateManagerForStockTrades(dataGridViewHugoStockTrades,
               GetHugoStockTrades, filterStatusLabel3, showStockLabel3, showAllLabel3, tradeSumLabel3, groupByPriceLabel2);
            merrillStockTradesDataGridStateManager = new DataGridStateManagerForStockTrades(dataGridViewMerrillStockTrades,
                 GetMerrillStockTrades, filterStatusLabel4, showStockLabel4, showAllLabel4, tradeSumLabel4, groupByPriceLabel1);
            hugoOptionTradesDataGridStateManager = new DataGridStateManagerForHugoOptionTrades(dataGridViewHugoOptionTrades,
               GetHugoOptionTrades, filterStatusLabel5, showOptionLabel5, showUnderlyingLabel5, showAllLabel5, tradeSumLabel5);
            merrillOptionTradesDataGridStateManager = new DataGridStateManagerForOptionTrades(dataGridViewMerrillOptionTrades,
               GetMerrillOptionTrades, filterStatusLabel6, showOptionLabel6, showUnderlyingLabel6, showAllLabel6, tradeSumLabel6);
            hugoFuturesTradesDataGridStateManager = new DataGridStateManagerForFuturesTrades(dataGridViewHugoFuturesTrades,
             GetHugoFuturesTrades, filterStatusLabel10, showFutureLabel10, showAllLabel10, tradeSumLabel10, groupByPriceLabel10);
            merrillFuturesTradesDataGridStateManager = new DataGridStateManagerForFuturesTrades(dataGridViewMerrillFuturesTrades,
                 GetMerrillFuturesTrades, filterStatusLabel13, showFutureLabel13, showAllLabel13, tradeSumLabel13, groupByPriceLabel13);
            todaysMerrillCorrectionsDataGridStateManager = new DataGridStateManager(dataGridViewMerrillCorrections1,
               GetTodaysMerrillCorrections, filterStatusLabel7, showAllLabel7);
            yesterdaysMerrillCorrectionsDataGridStateManager = new DataGridStateManager(dataGridViewMerrillCorrections2,
               GetYesterdaysMerrillCorrections, filterStatusLabel8, showAllLabel8);
            hugoStockCorrectionsDataGridStateManager = new DataGridStateManager(dataGridViewHugoCorrections1,
               GetHugoStockCorrections, filterStatusLabel11, showAllLabel11);
            hugoFuturesCorrectionsDataGridStateManager = new DataGridStateManager(dataGridViewHugoFuturesCorrections,
              GetHugoFuturesCorrections, filterStatusLabel14, showAllLabel14);
            hugoOptionCorrectionsDataGridStateManager = new DataGridStateManager(dataGridViewHugoCorrections2,
              GetHugoOptionCorrections, filterStatusLabel12, showAllLabel12);
         }

        // To add a new combobox column to a datagrid
        //  1. Make sure there is a datatable in Utilities that contains the values you want (or add a string array
        //      to the 'Constants' region).  If using a datatable in Utilities, you may want to ensure that
        //      it has an entry with a NULL value if that is an acceptable choice.
        //  2. Right-click on the DataGrid and choose 'Edit columns...'
        //  3. Select the column you wish to make a combobox column.
        //  4. Set ColumnType to 'DataGridViewComboBoxColumn' or 'DataGridViewAutoFilterComboBoxColumn'
        //  5. Set DisplayStyle to 'Nothing'.  Otherwise a dropdown arrow will appear in the cell even if you
        //      are not in edit mode.
        //  6. Make sure 'ReadOnly' is false.
        //
        //  If the datasource is a DataTable:
        //  1. Edit the DataSource property.  If the binding source is in the list, select it.  Otherwise, click
        //      'Add Project Data Source...' and select 'ReconciliationLib.ReconciliationLib.HugoDataSet.<whatever>DataTable'.
        //  2. Set the DisplayMember and ValueMember to the appropriate column name in the DataTable.
        //  3. Set the BindingSource data source below
        //
        //  If the datasource is a string array, call Items.AddRange on the column below.
        //

        private void FillDataGridComboBoxes()
        {
            tradeTypeNameDataGridViewTextBoxColumn.Items.AddRange(hugoStockTradeTypes);
            tradeTypeNameDataGridViewTextBoxColumn1.Items.AddRange(hugoStockTradeTypes);
            tradeTypeNameDataGridViewTextBoxColumn2.Items.AddRange(hugoOptionTradeTypes);
            tradeTypeNameDataGridViewTextBoxColumn3.Items.AddRange(hugoOptionTradeTypes);
            optionTypeDataGridViewTextBoxColumn.Items.AddRange(optionTypes);
            optionTypeNameDataGridViewTextBoxColumn1.Items.AddRange(optionTypes);
        }


        private void FillAccountGroupComboBox()
        {
            string saveSelectedAccountGroupName = AccountGroupName;

            comboBoxAccountGroup.BeginUpdate();
            try
            {
                comboBoxAccountGroup.Items.Clear();
                foreach (ReconciliationLib.HugoDataSet.AccountGroupsRow row in ReconciliationLib.Utilities.AccountGroups.Rows)
                {
                    comboBoxAccountGroup.Items.Add(row.AcctGroupName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error loading account groups", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                comboBoxAccountGroup.EndUpdate();

                if (saveSelectedAccountGroupName != null)
                {
                    int item = comboBoxAccountGroup.FindStringExact(saveSelectedAccountGroupName);
                    comboBoxAccountGroup.SelectedIndex = item;
                }
                else
                {
                    comboBoxAccountGroup.SelectedIndex = -1;
                }
            }

        }

        private void BuildContextMenuCommands()
        {
            acceptFuturesDiscrepancyCommand = new ToolStripMenuItem("Accept discrepancy", null, new System.EventHandler(OnAcceptFuturesDiscrepancy));
            acceptOptionDiscrepancyCommand = new ToolStripMenuItem("Accept discrepancy", null, new System.EventHandler(OnAcceptOptionDiscrepancy));
            acceptStockDiscrepancyCommand = new ToolStripMenuItem("Accept discrepancy", null, new System.EventHandler(OnAcceptStockDiscrepancy));
            addFuturesAdjustmentCommand = new ToolStripMenuItem("Add adjustment trade to Hugo", null, new System.EventHandler(OnAddFuturesAdjustment));
            addFuturesTradesToHugoCommand = new ToolStripMenuItem("&Add trade(s) to Hugo", null, new System.EventHandler(OnAddMerrillFuturesTradesToHugo));
            addFuturesTradesToMerrillCommand = new ToolStripMenuItem("&Add trade(s) to Clearing House", null, new System.EventHandler(OnAddHugoFuturesTradesToMerrill));
            addOptionAdjustmentCommand = new ToolStripMenuItem("Add adjustment trade to Hugo", null, new System.EventHandler(OnAddOptionAdjustment));
            addOptionTradesToHugoCommand = new ToolStripMenuItem("&Add trade(s) to Hugo", null, new System.EventHandler(OnAddMerrillOptionTradesToHugo));
            addOptionTradesToMerrillCommand = new ToolStripMenuItem("&Add trade(s) to Clearing House", null, new System.EventHandler(OnAddHugoOptionTradesToMerrill));
            addStockAdjustmentCommand = new ToolStripMenuItem("Add adjustment trade to Hugo", null, new System.EventHandler(OnAddStockAdjustment));
            addStockTradesToHugoCommand = new ToolStripMenuItem("&Add trade(s) to Hugo", null, new System.EventHandler(OnAddMerrillStockTradesToHugo));
            addStockTradesToMerrillCommand = new ToolStripMenuItem("&Add trade(s) to Clearing House", null, new System.EventHandler(OnAddHugoStockTradesToMerrill));
            addSymbolMappingCommand = new ToolStripMenuItem("Match Hugo and Clearing House symbols", null, new System.EventHandler(OnAddSymbolMapping));
            changeFuturesSelectionCommand = new ToolStripMenuItem("&Filter by this stock", null, new System.EventHandler(OnChangeFuturesSelection));
            changeOptionSelectionCommand = new ToolStripMenuItem("&Filter by this option", null, new System.EventHandler(OnChangeOptionSelection));
            changeStockSelectionCommand = new ToolStripMenuItem("&Filter by this stock", null, new System.EventHandler(OnChangeStockSelection));
            clearAcceptedFuturesDiscrepancyCommand = new ToolStripMenuItem("Clear accepted discrepancy", null, new System.EventHandler(OnClearAcceptedFuturesDiscrepancy));
            clearAcceptedOptionDiscrepancyCommand = new ToolStripMenuItem("Clear accepted discrepancy", null, new System.EventHandler(OnClearAcceptedOptionDiscrepancy));
            clearAcceptedStockDiscrepancyCommand = new ToolStripMenuItem("Clear accepted discrepancy", null, new System.EventHandler(OnClearAcceptedStockDiscrepancy));
            consolidateTradesCommand = new ToolStripMenuItem("Consolidate trades", null, new System.EventHandler(OnConsolidateTrades));
            copyConfirmationCorrectionCommand = new ToolStripMenuItem("Copy message", null, new System.EventHandler(OnCopyConfirmationCorrection));
            copyHugoFuturesCorrectionCommand = new ToolStripMenuItem("Copy message", null, new System.EventHandler(OnCopyHugoFuturesCorrection));
            copyHugoFuturesTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyHugoFuturesTrade));
            copyHugoOptionCorrectionCommand = new ToolStripMenuItem("Copy message", null, new System.EventHandler(OnCopyHugoOptionCorrection));
            copyHugoOptionTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyHugoOptionTrade));
            copyHugoStockCorrectionCommand = new ToolStripMenuItem("Copy message", null, new System.EventHandler(OnCopyHugoStockCorrection));
            copyHugoStockTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyHugoStockTrade));
            copyMerrillFuturesTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyMerrillFuturesTrade));
            copyMerrillOptionTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyMerrillOptionTrade));
            copyMerrillStockTradeCommand = new ToolStripMenuItem("Copy &trade", null, new System.EventHandler(OnCopyMerrillStockTrade));
            copyOptionCommand = new ToolStripMenuItem("&Copy option", null, new System.EventHandler(OnCopyOption));
            deconsolidateTradesCommand = new ToolStripMenuItem("Deconsolidate trades", null, new System.EventHandler(OnDeconsolidateTrades));
            deleteConfirmationCommand = new ToolStripMenuItem("&Delete", null, new System.EventHandler(OnDeleteConfirmation));
            deleteMerrillCorrectionCommand = new ToolStripMenuItem("&Delete", null, new System.EventHandler(OnDeleteMerrillCorrection));
            deleteSymbolMappingCommand = new ToolStripMenuItem("Un-match Hugo and Clearing House symbols", null, new System.EventHandler(OnDeleteSymbolMapping));
            deleteTradeCommand = new ToolStripMenuItem("&Delete", null, new System.EventHandler(OnDeleteTrade));
            distributeOptionTradeCommand = new ToolStripMenuItem("Distribute t&rade", null, new System.EventHandler(OnDistributeOptionTrade));
            distributeStockTradeCommand = new ToolStripMenuItem("Distribute t&rade", null, new System.EventHandler(OnDistributeStockTrade));
            editDateTimeCommand = new ToolStripMenuItem("&Edit", null, new System.EventHandler(OnEditDateTime));
            editNoteCommand = new ToolStripMenuItem("Edit note", null, new System.EventHandler(OnEditCorrectionNote));
            editTextCommand = new ToolStripMenuItem("&Edit", null, new System.EventHandler(OnEditText));
            hideCommand = new ToolStripMenuItem("&Hide", null, new System.EventHandler(OnHide));
            hideDoneCorrectionsCommand = new ToolStripMenuItem("Show only corrections not yet done", null, new System.EventHandler(OnHideDoneCorrections));
            matchFuturesTotalCostCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchFuturesTotalCosts));
            matchFuturesTradesCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchFuturesTrades));
            matchOptionPositionsCommand = new ToolStripMenuItem("Match Hugo and Clearing House option positions", null, new System.EventHandler(OnMatchOptionPositions));
            matchOptionTotalCostCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchOptionTotalCosts));
            matchOptionTradesCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchOptionTrades));
            matchStockTotalCostCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchStockTotalCosts));
            matchStockTradesCommand = new ToolStripMenuItem("Hide &matches", null, new System.EventHandler(OnMatchStockTrades));
            pasteFuturesTradeCommand = new ToolStripMenuItem("Paste &trade", null, new System.EventHandler(OnPasteFuturesTrade));
            pasteOptionCommand = new ToolStripMenuItem("&Paste option", null, new System.EventHandler(OnPasteOption));
            pasteOptionTradeCommand = new ToolStripMenuItem("Paste &trade", null, new System.EventHandler(OnPasteOptionTrade));
            pasteStockTradeCommand = new ToolStripMenuItem("Paste &trade", null, new System.EventHandler(OnPasteStockTrade));
            redoMerrillCorrectionCommand = new ToolStripMenuItem("&Re-do", null, new System.EventHandler(OnRedoMerrillCorrection));
            redoMerrillCorrectionToDateCommand = new ToolStripMenuItem("&Re-do for specific date...", null, new System.EventHandler(OnRedoMerrillCorrectionToDate));
            restoreConfirmationCommand = new ToolStripMenuItem("&Restore", null, new System.EventHandler(OnRestoreConfirmation));
            reverseFilterCommand = new ToolStripMenuItem("Reverse filter", null, new System.EventHandler(OnReverseFilter));
            selectAllCommand = new ToolStripMenuItem("&Select All", null, new System.EventHandler(OnSelectAll));
            showAssociatedOptionTradesCommand = new ToolStripMenuItem("Show associated option trades", null, new System.EventHandler(OnShowAssociatedOptionTrades));
            showAssociatedStockTradesCommand = new ToolStripMenuItem("Show associated stock trades", null, new System.EventHandler(OnShowAssociatedStockTrades));
            undoHugoFuturesCorrectionCommand = new ToolStripMenuItem("&Undo", null, new System.EventHandler(OnUndoHugoFuturesCorrection));
            undoHugoOptionCorrectionCommand = new ToolStripMenuItem("&Undo", null, new System.EventHandler(OnUndoHugoOptionCorrection));
            undoHugoStockCorrectionCommand = new ToolStripMenuItem("&Undo", null, new System.EventHandler(OnUndoHugoStockCorrection));
            unmatchOptionPositionsCommand = new ToolStripMenuItem("Unmatch Hugo and Clearing House option positions", null, new System.EventHandler(OnUnmatchOptionPositions));
        }
        #endregion

        #region RefreshBindingSource events
        private static void GetStockPositions(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.StockPositions;
        }
        private static void GetOptionPositions(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.OptionPositions;
        }
        private static void GetFuturesPositions(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.FuturesPositions;
        }
        private static void GetHugoStockTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetHugoStockTrades(e.IUnderlying, e.GroupByPrice);
        }
        private static void GetMerrillStockTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetMerrillStockTrades(e.IUnderlying, e.GroupByPrice);
        }
        private static void GetHugoFuturesTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetHugoFuturesTrades(e.IUnderlying, e.GroupByPrice);
        }
        private static void GetMerrillFuturesTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetMerrillFuturesTrades(e.IUnderlying, e.GroupByPrice);
        }
        private static void GetHugoOptionTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetHugoOptionTrades(e.IUnderlying, e.IOption, Properties.Settings.Default.ConsolidateOptions);
        }
        private static void GetMerrillOptionTrades(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.GetMerrillOptionTrades(e.IUnderlying, e.IOption);
        }
        private static void GetTodaysMerrillCorrections(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.ConfirmationCorrectionsToday;
        }
        private static void GetYesterdaysMerrillCorrections(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.ConfirmationCorrectionsYesterday;
        }
        private static void GetHugoStockCorrections(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.HugoStockCorrections;
        }
        private static void GetHugoFuturesCorrections(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.HugoFuturesCorrections;
        }
        private static void GetHugoOptionCorrections(object sender, RefreshBindingSourceEventArgs e)
        {
            e.DataTable = Utilities.HugoOptionCorrections;
        }
        #endregion

        private static IEnumerable<OptionTrade> ExpandOptionTrades(OptionTrade optionTrade)
        {
            if (optionTrade.NumberOfTrades <= 1)
            {
                yield return optionTrade;
            }
            else
            {
                List<OptionTrade> tradesForPackage = new List<OptionTrade>(Utilities.GetOptionTradesForPackageId(optionTrade.ConsolidationPackageId.Value));
                foreach (OptionTrade subTrade in tradesForPackage)
                {
                    yield return subTrade;
                }
            }
        }

        private static bool DeleteOptionTrade(IOptionTrade optionTrade)
        {
            if (VerifyOptionTrade(optionTrade))
            {
                if (0 == Utilities.DeleteOptionTrade(optionTrade))
                {
                    return true;
                }
            }
            return false;
        }

        private static void HandleError(string msg, Exception e)
        {
            logger.Error(msg, e);
            MessageBox.Show(String.Format("{0}: {1}", msg, e.Message), "Reconciliation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void FillDataGridViewContextMenuStrip(ContextMenuStrip menu, DataGridViewColumn column)
        {
            // if it is a combobox column, display a command for each item in the combobox
            if (column is DataGridViewAutoFilterComboBoxColumn)
            {
                FillComboBoxColumnContextMenuStrip(menu, (DataGridViewComboBoxColumn)column);
            }
            // if it is datetime column, wire a datetimepicker edit form
            else if ((column is DataGridViewDateColumn) || (column is DataGridViewDateTimeColumn))
            {
                menu.Items.Add(editDateTimeCommand);
            }
            // otherwise, wire a text edit form
            else
            {
                menu.Items.Add(editTextCommand);
            }
        }

        private void FillComboBoxColumnContextMenuStrip(ContextMenuStrip menu, DataGridViewComboBoxColumn column)
        {
            try
            {
                if (column.Items.Count > 0)    // make sure combobox is populated
                {
                    // see if it contains strings or DataRowViews
                    if (column.Items[0] is string)
                    {
                        foreach (string str in column.Items)
                        {
                            menu.Items.Add(new ToolStripMenuItem(str, null,
                                  new System.EventHandler(OnGenericCommand)));
                        }
                    }
                    else
                    {
                        foreach (DataRowView row in column.Items)
                        {
                            menu.Items.Add(new ToolStripMenuItem(row[column.DisplayMember].ToString(), null,
                                  new System.EventHandler(OnGenericCommand)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Unable to construct context menu: {0}", e.Message), "Context menu error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // refresh data that does not depend on account group
        private void RefreshGenericData()
        {
            FillAccountGroupComboBox();
            brokersDataTableBindingSource.DataSource = Utilities.Brokers;
            exchangesDataTableBindingSource.DataSource = Utilities.Exchanges;
            optionTradeReasonsDataTableBindingSource.DataSource = Utilities.OptionTradeReasons;
            stockTradeReasonsDataTableBindingSource.DataSource = Utilities.StockTradeReasons;
            tradeMediaDataTableBindingSource.DataSource = Utilities.TradeMedia;
            tradersDataTableBindingSource.DataSource = Utilities.Traders;
            formSymbolMappings.NeedToRefresh = true;
        }

        private void RefreshDataGrids()
        {
            Enabled = false;
            try
            {
                // cannot refresh if we don't have an account group
                if (AccountGroupSelected)
                {
                    // these refreshes will do nothing unless the NeedToRefresh property has been set
                    stockPositionsDataGridStateManager.Refresh();
                    optionPositionsDataGridStateManager.Refresh();
                    futuresPositionsDataGridStateManager.Refresh();
                    hugoOptionTradesDataGridStateManager.Refresh();
                    hugoStockTradesDataGridStateManager.Refresh();
                    hugoFuturesTradesDataGridStateManager.Refresh();
                    merrillOptionTradesDataGridStateManager.Refresh();
                    merrillStockTradesDataGridStateManager.Refresh();
                    merrillFuturesTradesDataGridStateManager.Refresh();
                    todaysMerrillCorrectionsDataGridStateManager.Refresh();
                    yesterdaysMerrillCorrectionsDataGridStateManager.Refresh();
                    hugoStockCorrectionsDataGridStateManager.Refresh();
                    hugoFuturesCorrectionsDataGridStateManager.Refresh();
                    hugoOptionCorrectionsDataGridStateManager.Refresh();
                 }

                ShowFinalizationState();
            }
            finally
            {
                Enabled = true;
                EnableControls();
            }
        }

        private void EnableControls()
        {
            try
            {
                if (!AccountGroupSelected)
                {
                    buttonImport.Visible = buttonImport.Enabled = false;
                    buttonFinalize.Visible = buttonFinalize.Enabled = false;
                    buttonSyncTotalCosts.Visible = buttonSyncTotalCosts.Enabled = false;
                }
                else
                {
                    buttonImport.Visible = buttonImport.Enabled =
                      Properties.Settings.Default.DebugMode &&
                      (importStatusInfo.LastStatus != TaskStatus.Started);
                    buttonFinalize.Visible = buttonFinalize.Enabled = CanFinalize();
                    buttonSyncTotalCosts.Visible = buttonSyncTotalCosts.Enabled = true;
                }

                buttonUpdate.Enabled = CurrentDatesImportDone() &&
                    (hugoOptionTradesDataGridStateManager.IsDirty ||
                        hugoStockTradesDataGridStateManager.IsDirty ||
                        hugoFuturesTradesDataGridStateManager.IsDirty ||
                        merrillOptionTradesDataGridStateManager.IsDirty ||
                        merrillStockTradesDataGridStateManager.IsDirty ||
                        merrillFuturesTradesDataGridStateManager.IsDirty
                        );
                buttonCancelChanges.Enabled = (hugoOptionTradesDataGridStateManager.IsDirty ||
                        hugoStockTradesDataGridStateManager.IsDirty ||
                        hugoFuturesTradesDataGridStateManager.IsDirty ||
                        merrillOptionTradesDataGridStateManager.IsDirty ||
                        merrillStockTradesDataGridStateManager.IsDirty ||
                        merrillFuturesTradesDataGridStateManager.IsDirty
                        );
                comboBoxAccountGroup.Enabled = !buttonCancelChanges.Enabled;
                accountMappingsToolStripMenuItem.Enabled = AccountGroupSelected;
                symbolMappingsToolStripMenuItem.Enabled = AccountGroupSelected;
            }
            catch (Exception e)
            {
                HandleError("Error enabling controls", e);
            }
        }

        private bool TodaysImportDone()
        {
            if ((importStatusInfo.LastStatus == TaskStatus.Completed)
                && (importStatusInfo.LastEndTime >= DateTime.Today))
            {
                return true;
            }
            else
            {
                return (importStatusInfo.LastStatus != TaskStatus.Started)
                    && accountGroupsForWhichImportWasRunManually.Contains(Utilities.AccountGroupName);
            }
        }

        private bool CurrentDatesImportDone()
        {
            if (DateTime.Today <= CurrentDate.Date)
                return TodaysImportDone();
            else
                return (importStatusInfo.LastEndTime.Date >= CurrentDate.Date) &&
                    (importStatusInfo.LastStatus != TaskStatus.None);
        }

        private bool CanFinalize()
        {
            return (!GetAccountGroupInfo().IsFinalized)
                                    && TodaysImportDone()
                                    && (CurrentDate.Date > GetAccountGroupInfo().PrevImportDate)

                                    // no position discrepancies allowed
                                    && (Utilities.StockDiscrepancyCount == 0)
                                    && (Utilities.OptionDiscrepancyCount == 0)
                                    && (Utilities.FuturesDiscrepancyCount == 0);
/*
                                                 // no missing concurrent stock prices allowed
                                                && (Utilities.MissingConcurrentStockPriceCount == 0)

                                                // no totalcost discrepancies allowed for accounts using taxlots
                                                && ((!Utilities.UsingTaxLots) ||
                                                ((Utilities.StockTotalCostDiscrepancyCount == 0)
                                                && (Utilities.OptionTotalCostDiscrepancyCount == 0)));
              */
        }
        private void FillPositions()
        {
            // see if we have already done any imports today
            ReconciliationLib.Utilities.CheckImportState(out numPositions, out numConfirmations, out numTaxlots, out numBookkeepingEntries, out numDividends);

            ShowImportButton();

            // don't use SelectedOption property, since we don't want to fire selection-changed event twice
            //      (which would cause us to get all the Hugo stock and option trades twice)
            // SelectedUnderlying property will notify the datagrids that the selection has changed, and
            //      one such notification is sufficient
            selectedOption = null;  
            SelectedUnderlying = null;

            RefreshDataGrids();
        }

        private void ShowImportButton()
        {
            if (numPositions > 0)
            {
                buttonImport.Text = String.Format("Re-import {0}'s files", clearingHouseText);
            }
            else
            {
                buttonImport.Text = String.Format("Import {0}'s files", clearingHouseText);
            }
        }

        private bool CanChangeSelection(DataGridStateManager stateManager /*if null, will check all grids*/)
        {
            if ((stateManager == null) ? buttonUpdate.Enabled : stateManager.IsDirty)
            {
                switch (MessageBox.Show("Commit changes?", "Changing selection", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        buttonUpdate_Click(this, null);
                        return true;

                    case DialogResult.No:
                        buttonCancelChanges_Click(this, null);
                        return true;

                    default:
                        return false;
                }
            }

            return true;
        }

        private bool ShouldProceedWithUpdates(int hugoStockEdits, int hugoOptionEdits, int hugoFuturesEdits, int merrillStockEdits, int merrillOptionEdits, int merrillFuturesEdits)
        {
            string msg = "Commit ";

            // count types of changes
            int typesOfChanges = 0;
            if (hugoStockEdits > 0)
                typesOfChanges++;
            if (hugoOptionEdits > 0)
                typesOfChanges++;
            if (hugoFuturesEdits > 0)
                typesOfChanges++;
            if (merrillStockEdits > 0)
                typesOfChanges++;
            if (merrillOptionEdits > 0)
                typesOfChanges++;
            if (merrillFuturesEdits > 0)
                typesOfChanges++;

            if (typesOfChanges == 0)
            {
                MessageBox.Show("No changes to commit", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // fix dirty flags
                hugoStockTradesDataGridStateManager.IsDirty = false;
                hugoOptionTradesDataGridStateManager.IsDirty = false;
                hugoFuturesTradesDataGridStateManager.IsDirty = false;
                merrillStockTradesDataGridStateManager.IsDirty = false;
                merrillOptionTradesDataGridStateManager.IsDirty = false;
                merrillFuturesTradesDataGridStateManager.IsDirty = false;

                return false;
            }
            else
            {
                string[] connectors = new string[] { "?", " and ", ", ", ", " };

                if (hugoStockEdits > 0)
                {
                    msg += String.Format("{0} stock trade(s) to Hugo{1}", hugoStockEdits, connectors[--typesOfChanges]);
                }
                if (hugoOptionEdits > 0)
                {
                    msg += String.Format("{0} option trade(s) to Hugo{1}", hugoOptionEdits, connectors[--typesOfChanges]);
                }
                if (hugoFuturesEdits > 0)
                {
                    msg += String.Format("{0} futures trade(s) to Hugo{1}", hugoFuturesEdits, connectors[--typesOfChanges]);
                }
                if (merrillStockEdits > 0)
                {
                    msg += String.Format("{0} stock trade(s) to {2}{1}", merrillStockEdits, connectors[--typesOfChanges], clearingHouseText);
                }
                if (merrillOptionEdits > 0)
                {
                    msg += String.Format("{0} option trade(s) to {2}{1}", merrillOptionEdits, connectors[--typesOfChanges], clearingHouseText);
                }
                if (merrillFuturesEdits > 0)
                {
                    msg += String.Format("{0} futures trade(s) to {2}{1}", merrillFuturesEdits, connectors[--typesOfChanges], clearingHouseText);
                }
                return DialogResult.Yes == MessageBox.Show(msg, "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
        }

        private static int ProceedWithUpdates(HugoDataSet.HugoStockTradesDataTable hugoStockTradesToEdit,
            HugoDataSet.HugoOptionTradesDataTable hugoOptionTradesToEdit,
            HugoDataSet.HugoFuturesTradesDataTable hugoFuturesTradesToEdit,
            HugoDataSet.MerrillStockTradesDataTable merrillStockTradesToEdit,
            HugoDataSet.MerrillOptionTradesDataTable merrillOptionTradesToEdit,
            HugoDataSet.MerrillFuturesTradesDataTable merrillFuturesTradesToEdit,
            int hugoStockEdits,
            int hugoOptionEdits,
            int hugoFuturesEdits,
            int merrillStockEdits,
            int merrillOptionEdits,
            int merrillFuturesEdits)
       {
            int editsDone = 0;
            bool triedToEditaDK = false;
            if (hugoStockEdits > 0)
            {
                editsDone = UpdateHugoStockTrades(hugoStockTradesToEdit, editsDone);
            }
            if (hugoOptionEdits > 0)
            {
                editsDone = UpdateHugoOptionTrades(hugoOptionTradesToEdit, editsDone);
            }
            if (hugoFuturesEdits > 0)
            {
                editsDone = UpdateHugoFuturesTrades(hugoFuturesTradesToEdit, editsDone);
            }
            if (merrillStockEdits > 0)
            {
                UpdateMerrillStockTrades(merrillStockTradesToEdit, ref editsDone, ref triedToEditaDK);
            }
            if (merrillOptionEdits > 0)
            {
                UpdateMerrillOptionTrades(merrillOptionTradesToEdit, ref editsDone, ref triedToEditaDK);
            }
            if (merrillFuturesEdits > 0)
            {
                UpdateMerrillFuturesTrades(merrillFuturesTradesToEdit, ref editsDone, ref triedToEditaDK);
            }
            if (triedToEditaDK)
            {
                MessageBox.Show("Cannot edit a DK'd trade", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return editsDone;
        }

        private static void UpdateMerrillOptionTrades(HugoDataSet.MerrillOptionTradesDataTable merrillOptionTradesToEdit, ref int editsDone, ref bool triedToEditaDK)
        {
            foreach (HugoDataSet.MerrillOptionTradesRow row in merrillOptionTradesToEdit.Rows)
            {
                if (row.RecordType == "DK")
                {
                    triedToEditaDK = true;
                }
                else
                {
                    OptionTrade optionTrade = new OptionTrade(row);
                    optionTrade.TotalCost = GetAdjustedTotalCost(row, /*isOption = */ true);

                    if (0 == Utilities.EditOptionTradeConfirmation(optionTrade, null))
                    {
                        editsDone++;
                    }
                }
            }
        }

        private static void UpdateMerrillStockTrades(HugoDataSet.MerrillStockTradesDataTable merrillStockTradesToEdit, ref int editsDone, ref bool triedToEditaDK)
        {
            foreach (HugoDataSet.MerrillStockTradesRow row in merrillStockTradesToEdit.Rows)
            {
                if (row.RecordType == "DK")
                {
                    triedToEditaDK = true;
                }
                else
                {
                    StockTrade stockTrade = new StockTrade(row);
                    stockTrade.TotalCost = GetAdjustedTotalCost(row, /*isOption = */ false);

                    if (0 == Utilities.EditStockTradeConfirmation(stockTrade, null))
                    {
                        editsDone++;
                    }
                }
            }
        }
        private static void UpdateMerrillFuturesTrades(HugoDataSet.MerrillFuturesTradesDataTable merrillFuturesTradesToEdit, ref int editsDone, ref bool triedToEditaDK)
        {
            foreach (HugoDataSet.MerrillFuturesTradesRow row in merrillFuturesTradesToEdit.Rows)
            {
                if (row.RecordType == "DK")
                {
                    triedToEditaDK = true;
                }
                else
                {
                    FuturesTrade futuresTrade = new FuturesTrade(row);
                     if (0 == Utilities.EditFuturesTradeConfirmation(futuresTrade, null))
                    {
                        editsDone++;
                    }
                }
            }
        }

        private static int UpdateHugoOptionTrades(HugoDataSet.HugoOptionTradesDataTable hugoOptionTradesToEdit, int editsDone)
        {
            foreach (HugoDataSet.HugoOptionTradesRow row in hugoOptionTradesToEdit.Rows)
            {
                OptionTrade optionTrade = new OptionTrade(row);
                EditedOptionTrade editedOptionTrade = new EditedOptionTrade(hugoOptionTradesToEdit, row);
 
                if (VerifyOptionTrade(optionTrade))
                {
                    // processing for single trades
                    if (optionTrade.NumberOfTrades <= 1)
                    {
                          if (0 == Utilities.EditOptionTrade(editedOptionTrade))
                        {
                            editsDone++;
                        }
                    }

                      // processing for packages
                    else
                    {
                        double remainderTotalCost = editedOptionTrade.TotalCost ?? 0;
                        double remainderCommission = editedOptionTrade.TotalCost ?? 0;
                        double remainderSECFee = editedOptionTrade.SECFee ?? 0;
                        double remainderORFFee = editedOptionTrade.ORFFee ?? 0;
                        int tradeNumber = 0;
                        foreach (OptionTrade subTrade in ExpandOptionTrades(optionTrade))
                        {
                            EditedOptionTrade subEditedTrade = new EditedOptionTrade(editedOptionTrade, optionTrade, subTrade,
                                ref remainderTotalCost, ref remainderCommission, ref remainderSECFee, ref remainderORFFee, ref tradeNumber);
                            if (0 == Utilities.EditOptionTrade(subEditedTrade))
                            {
                                editsDone++;
                            }
                        }
                    }
                }
            }
            return editsDone;
        }

        private static int UpdateHugoStockTrades(HugoDataSet.HugoStockTradesDataTable hugoStockTradesToEdit, int editsDone)
        {
            foreach (HugoDataSet.HugoStockTradesRow row in hugoStockTradesToEdit.Rows)
            {
                StockTrade stockTrade = new StockTrade(row);
                EditedStockTrade editedStockTrade = new EditedStockTrade(hugoStockTradesToEdit, row);
  
                if (VerifyStockTrade(stockTrade, row))
                {
                    if (0 == Utilities.EditStockTrade(editedStockTrade))
                    {
                        editsDone++;
                    }
                }
            }
            return editsDone;
        }
        private static int UpdateHugoFuturesTrades(HugoDataSet.HugoFuturesTradesDataTable hugoFuturesTradesToEdit, int editsDone)
        {
            foreach (HugoDataSet.HugoFuturesTradesRow row in hugoFuturesTradesToEdit.Rows)
            {
                FuturesTrade futuresTrade = new FuturesTrade(row);
                EditedFuturesTrade editedFuturesTrade = new EditedFuturesTrade(hugoFuturesTradesToEdit, row);

                if (VerifyFuturesTrade(futuresTrade, row))
                {
                    if (0 == Utilities.EditFuturesTrade(editedFuturesTrade))
                    {
                        editsDone++;
                    }
                }
            }
            return editsDone;
        }


        private static double GetAdjustedTotalCost(DataRow row, bool isOption)
        {
            // if user has not changed total cost himself, see if we need to change it for him
            double adjustedTotalCost = Convert.ToDouble(row["TotalCost", DataRowVersion.Current]);
            if (adjustedTotalCost == Convert.ToDouble(row["TotalCost", DataRowVersion.Original]))
            {
                double currentCost = Convert.ToDouble(row["TradeVolume", DataRowVersion.Current]) * Convert.ToDouble(row["TradePrice", DataRowVersion.Current]);
                double originalCost = Convert.ToDouble(row["TradeVolume", DataRowVersion.Original]) * Convert.ToDouble(row["TradePrice", DataRowVersion.Original]);

                // this column exists only for option trades
                double multiplier = isOption ?
                    (row["SharesPerContract"] == null) ? 100.0 : Convert.ToDouble(row["SharesPerContract", DataRowVersion.Original])
                    : 1.0;
                adjustedTotalCost += multiplier * (currentCost - originalCost);
            }
            return adjustedTotalCost;
        }

        private void AddOptionAssignmentsToHugo(ref int addedAssignments, ref int notAddedAssignments, ref int errorsAddingAssignments)
        {
            HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades = Utilities.GetMerrillOptionTrades(null, null);
            foreach (HugoDataSet.MerrillOptionTradesRow row in merrillOptionTrades.Rows)
            {
                if (Utilities.IsAssignment(row.TradeMediumName) && !Utilities.IsCancelOrCorrection(row.CancelCode)
                    && (row.TradeDate >= PreviousDate))
                {
                    switch (Utilities.AddOptionAssignment(new OptionTrade(row)))
                    {
                        case 0:
                            addedAssignments++;
                            break;
                        case 1:
                        case 2:
                            notAddedAssignments++;
                            break;
                        default:
                            errorsAddingAssignments++;
                            break;
                    }
                }
            }
        }

        private void AddOptionExpirationsToHugo(ref int addedExpirations, ref int notAddedExpirations, ref int errorsAddingExpirations)
        {
            HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades = Utilities.GetMerrillOptionTrades(null, null);
            foreach (HugoDataSet.MerrillOptionTradesRow row in merrillOptionTrades.Rows)
            {
                if (Utilities.IsExpiration(row.TradeMediumName) && !Utilities.IsCancelOrCorrection(row.CancelCode)
                    && row.TradeDate >= PreviousDate)
                {
                    switch (Utilities.AddOptionExpiration(new OptionTrade(row)))
                    {
                        case 0:
                            addedExpirations++;
                            break;
                        case 1:
                        case 2:
                            notAddedExpirations++;
                            break;
                        default:
                            errorsAddingExpirations++;
                            break;
                    }
                }
            }
        }

        DataGridViewRow FindMatchingMerrillOptionTradeRow(OptionTrade hugoOptionTrade, bool matchTotalCost)
        {
            foreach (DataGridViewRow merrillRow in dataGridViewMerrillOptionTrades.Rows)
            {
                if (merrillRow.Visible)
                {
                    OptionTrade merrillOptionTrade = GetOptionTradeFromDataGridViewRow(merrillRow, dataGridViewMerrillOptionTrades);
                    if (merrillOptionTrade.Equals(hugoOptionTrade))
                    {
                             return merrillRow;
                     }
                }
            }
            return null;
        }

        DataGridViewRow FindMatchingMerrillStockTradeRow(StockTrade hugoStockTrade, bool matchTotalCost)
        {
            foreach (DataGridViewRow merrillRow in dataGridViewMerrillStockTrades.Rows)
            {
                if (merrillRow.Visible)
                {
                    StockTrade merrillStockTrade = GetStockTradeFromDataGridViewRow(merrillRow, dataGridViewMerrillStockTrades);
                    if (merrillStockTrade.Equals(hugoStockTrade))
                    {
                            return merrillRow;
                      }
                }
            }
            return null;
        }
        DataGridViewRow FindMatchingMerrillFuturesTradeRow(FuturesTrade hugoFuturesTrade, bool matchTotalCost)
        {
            foreach (DataGridViewRow merrillRow in dataGridViewMerrillFuturesTrades.Rows)
            {
                if (merrillRow.Visible)
                {
                    FuturesTrade merrillFuturesTrade = GetFuturesTradeFromDataGridViewRow(merrillRow, dataGridViewMerrillFuturesTrades);
                    if (merrillFuturesTrade.Equals(hugoFuturesTrade))
                    {
                               return merrillRow;
                     }
                }
            }
            return null;
        }

        private void EnableOptionRelatedCommands(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            copyHugoOptionCorrectionCommand.Enabled = copyOptionCommand.Enabled = (stateManager.RowsToProcess.Length == 1);
            copyHugoOptionTradeCommand.Enabled = copyMerrillOptionTradeCommand.Enabled = EnableCopyOptionTrade(stateManager, dataGridView);

            if (clipboardOptionTrade == null)
            {
                pasteOptionCommand.Enabled = false;
                pasteOptionTradeCommand.Enabled = false;
            }
            else
            {
                pasteOptionCommand.Enabled = true;
                pasteOptionTradeCommand.Enabled = true;
            }
            if (stateManager.RowsToProcess.Length == 1)
            {
                changeOptionSelectionCommand.Enabled = !(new Option((DataRowView)stateManager.RowsToProcess[0].DataBoundItem).Equals(SelectedOption));
            }
            else
            {
                changeOptionSelectionCommand.Enabled = false;
            }
        }
        private void EnableStockRelatedCommands(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            copyHugoStockCorrectionCommand.Enabled = (stateManager.RowsToProcess.Length == 1);
            copyHugoStockTradeCommand.Enabled = copyMerrillStockTradeCommand.Enabled = EnableCopyStockTrade(stateManager, dataGridView);

            if ((clipboardStockTrade == null) && (!stateManager.GroupByPrice))
            {
                pasteStockTradeCommand.Enabled = false;
            }
            else
            {
                pasteStockTradeCommand.Enabled = true;
            }
            if (stateManager.RowsToProcess.Length == 1) 
            {
                changeStockSelectionCommand.Enabled = !(new Stock((DataRowView)stateManager.RowsToProcess[0].DataBoundItem).Equals(SelectedUnderlying));
            }
            else
            {
                changeStockSelectionCommand.Enabled = false;
            }
        }
        private void EnableFuturesRelatedCommands(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            copyHugoFuturesCorrectionCommand.Enabled = (stateManager.RowsToProcess.Length == 1);
            copyHugoFuturesTradeCommand.Enabled = copyMerrillFuturesTradeCommand.Enabled = EnableCopyFuturesTrade(stateManager, dataGridView);

            if ((clipboardFuturesTrade == null) && (!stateManager.GroupByPrice))
            {
                pasteFuturesTradeCommand.Enabled = false;
            }
            else
            {
                pasteFuturesTradeCommand.Enabled = true;
            }
            if (stateManager.RowsToProcess.Length == 1)
            {
                changeFuturesSelectionCommand.Enabled = !(new Future((DataRowView)stateManager.RowsToProcess[0].DataBoundItem).Equals(SelectedUnderlying));
            }
            else
            {
                changeFuturesSelectionCommand.Enabled = false;
            }
        }

        private void EnableHugoRelatedCommands(DataGridStateManager stateManager)
        {
            addStockTradesToMerrillCommand.Enabled = !stateManager.GroupByPrice;
            deleteTradeCommand.Enabled = !stateManager.GroupByPrice;
        }

        private bool EnableCopyStockTrade(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            bool bEnabled = false;
            string stockSymbol = null;
            string subAccountName = null;
            string tradeType = null;
            bool isShort = false;

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                StockTrade stockTrade = GetStockTradeFromDataGridViewRow(row, dataGridView);
                if (stockSymbol == null)
                {
                    stockSymbol = stockTrade.UnderlyingSymbol;
                    subAccountName = stockTrade.SubAcctName;
                    tradeType = stockTrade.TradeType;
                    isShort = stockTrade.ShortFlag;
                    bEnabled = true;
                }
                else
                {
                    if ((stockSymbol != stockTrade.UnderlyingSymbol)
                        || (subAccountName != stockTrade.SubAcctName)
                        || (tradeType != stockTrade.TradeType)
                        || (isShort != stockTrade.ShortFlag))
                    {
                        bEnabled = false;
                        break;
                    }
                }
            }

            return bEnabled;
        }

        private bool EnableCopyFuturesTrade(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            bool bEnabled = false;
            string futuresSymbol = null;
            string subAccountName = null;
            string tradeType = null;
            bool isShort = false;

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                FuturesTrade futuresTrade = GetFuturesTradeFromDataGridViewRow(row, dataGridView);
                if (futuresSymbol == null)
                {
                    futuresSymbol = futuresTrade.UnderlyingSymbol;
                    subAccountName = futuresTrade.SubAcctName;
                    tradeType = futuresTrade.TradeType;
                    isShort = futuresTrade.ShortFlag;
                    bEnabled = true;
                }
                else
                {
                    if ((futuresSymbol != futuresTrade.UnderlyingSymbol)
                        || (subAccountName != futuresTrade.SubAcctName)
                        || (tradeType != futuresTrade.TradeType)
                        || (isShort != futuresTrade.ShortFlag))
                    {
                        bEnabled = false;
                        break;
                    }
                }
            }

            return bEnabled;
        }
        private bool EnableCopyOptionTrade(DataGridStateManager stateManager, DataGridView dataGridView)
        {
            bool bEnabled = false;
            Option option = null;
            string subAccountName = null;
            string tradeType = null;

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                OptionTrade optionTrade = GetOptionTradeFromDataGridViewRow(row, dataGridView);
                if (option == null)
                {
                    option = new Option(row.DataBoundItem as DataRowView);
                    subAccountName = optionTrade.SubAcctName;
                    tradeType = optionTrade.TradeType;
                    bEnabled = true;
                }
                else
                {
                    if (!option.Equals((IOption)optionTrade)
                        || (subAccountName != optionTrade.SubAcctName)
                        || (tradeType != optionTrade.TradeType))
                    {
                        bEnabled = false;
                        break;
                    }
                }
            }

            return bEnabled;
        }

        private void EnableMerrillRelatedCommands(DataGridStateManager stateManager)
        {
            bool canDelete = false;
            bool canRestore = false;
            bool canAdd = false;

            if (!stateManager.GroupByPrice)
            {
                canAdd = true;
                if ((CurrentDate >= GetAccountGroupInfo().LastImportDate) || Properties.Settings.Default.DebugMode)
                {
                    foreach (DataGridViewRow row in stateManager.RowsToProcess)
                    {
                        if (Convert.ToString(((DataRowView)row.DataBoundItem)["RecordType"]) == "DK")
                        {
                            canRestore = true;
                        }
                        else
                        {
                            canDelete = true;
                        }

                        if (canDelete && canRestore)
                            break;
                    }
                }
            }

            deleteConfirmationCommand.Enabled = canDelete;
            restoreConfirmationCommand.Enabled = canRestore;
            addStockTradesToHugoCommand.Enabled = canAdd;
        }
        private static DataGridStateManager GetStateManagerFromToolStripItem(object sender)
        {
            return (sender as ToolStripItem).Owner.Tag as DataGridStateManager;
        }
        private static DataGridStateManager GetStateManagerFromDataGridView(object sender)
        {
            return (sender as Control).Tag as DataGridStateManager;
        }

        private static bool VerifyStockTrade(IStockTrade stockTrade, DataRow row)
        {
            if (stockTrade == null)
                return false;

            if (stockTrade.FullVolume < 0)
                return false;

            if (Utilities.IsExerciseAssignment(stockTrade.TradeMedium))
            {
                return VerifyStockExerciseOrAssignment(row);
            }
            else
            {
                return true;
            }
        }

        private static bool VerifyFuturesTrade(IFuturesTrade futuresTrade, DataRow row)
        {
            if (futuresTrade == null)
                return false;

            if (futuresTrade.FullVolume < 0)
                return false;

            return true;
        }
        private static bool VerifyStockExerciseOrAssignment(DataRow row)
        {
            // if we are editing and assignment, the only thing we are allowed to change is
            //      the price and total cost
            bool canChange = false;
            if (row != null)
            {
                canChange = (Convert.ToString(row["TradeTypeName", DataRowVersion.Original])[0] == Convert.ToString(row["TradeTypeName", DataRowVersion.Current])[0]) &&
                    (Convert.ToDouble(row["TradeVolume", DataRowVersion.Original]) == Convert.ToDouble(row["TradeVolume", DataRowVersion.Current])) &&
                    (Convert.ToString(row["SubAcctName", DataRowVersion.Original]) == Convert.ToString(row["SubAcctName", DataRowVersion.Current])) &&
                    (Convert.ToString(row["StockSymbol", DataRowVersion.Original]) == Convert.ToString(row["StockSymbol", DataRowVersion.Current])) &&
                    (Convert.ToString(row["TradeMediumName", DataRowVersion.Original]) == Convert.ToString(row["TradeMediumName", DataRowVersion.Current])) &&
                    (Convert.ToDateTime(row["TradeDateTime", DataRowVersion.Original]) == Convert.ToDateTime(row["TradeDateTime", DataRowVersion.Current]));
            }

            // if this is an illegal change, allow it anyway (after query) in debug mode
            if (!canChange)
            {
                if (Properties.Settings.Default.DebugMode)
                {
                    canChange = DialogResult.Yes == MessageBox.Show("Are you sure you want to do this?  In general, you should edit, add, or delete the option side of an exercise or assignment rather than the stock side.  The stock side will then change automatically.  Unless you are sure you know what you are doing, respond 'No'.",
                          "Verifying stock trade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Cannot edit the stock trade of an exercise or assignment - edit the option trade instead",
                    "Verifying stock trade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return canChange;
        }
        private static bool VerifyOptionTrade(IOptionTrade optionTrade)
        {
            if (optionTrade == null)
                return false;

            if ((optionTrade.TradeVolume <= 0) || ((optionTrade.TradeMedium == Utilities.ReorgName) && !Properties.Settings.Default.DebugMode))
            {
                MessageBox.Show("Cannot add an option trade if the volume is negative or if the MediumName is 'Reorg'",
                    "Verifying option trade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CopyStockTrade()
        {
            double totalPrice = 0;
            double totalVolume = 0;
            double totalCost = 0;
            double totalCommission = 0;
            double totalSECFee = 0;
            clipboardStockTrade = null;

            // get state manager for the datagridview generating this call
            DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                clipboardStockTrade = GetStockTradeFromDataGridViewRow(row, currentDataGridView);
                totalVolume += clipboardStockTrade.FullVolume;
                totalPrice += clipboardStockTrade.FullVolume * Convert.ToDouble(clipboardStockTrade.TradePrice);
                totalCommission += clipboardStockTrade.Commission;
                totalSECFee += clipboardStockTrade.SECFee;
                totalCost += clipboardStockTrade.TotalCost;
            }

            if (clipboardStockTrade != null)
            {
                if (totalVolume != 0)
                {
                    clipboardStockTrade.TradePrice = Decimal.Round(Convert.ToDecimal(totalPrice / totalVolume), 6);
                }
                else
                {
                    clipboardStockTrade.TradePrice = Decimal.Round(Convert.ToDecimal(totalPrice), 6);
                }
                clipboardStockTrade.FullVolume = totalVolume;
                clipboardStockTrade.Commission = totalCommission;
                clipboardStockTrade.SECFee = totalSECFee;
                clipboardStockTrade.TotalCost = totalCost;
            }
        }
        private void CopyFuturesTrade()
        {
            double totalPrice = 0;
            double totalVolume = 0;
            double totalCost = 0;
            double totalCommission = 0;
            double totalNFAFee = 0;
            double totalClearingFee = 0;
            double totalExchangeFee = 0;
            clipboardFuturesTrade = null;

            // get state manager for the datagridview generating this call
            DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                clipboardFuturesTrade = GetFuturesTradeFromDataGridViewRow(row, currentDataGridView);
                totalVolume += clipboardFuturesTrade.FullVolume;
                totalPrice += clipboardFuturesTrade.FullVolume * Convert.ToDouble(clipboardFuturesTrade.TradePrice);
                totalCost += clipboardFuturesTrade.TotalCost;
                totalCommission += clipboardFuturesTrade.Commission;
                totalNFAFee += clipboardFuturesTrade.NFAFee;
                totalClearingFee += clipboardFuturesTrade.ClearingFee;
                totalExchangeFee += clipboardFuturesTrade.ExchangeFee;
            }

            if (clipboardFuturesTrade != null)
            {
                if (totalVolume != 0)
                {
                    clipboardFuturesTrade.TradePrice = Decimal.Round(Convert.ToDecimal(totalPrice / totalVolume), 6);
                }
                else
                {
                    clipboardFuturesTrade.TradePrice = Decimal.Round(Convert.ToDecimal(totalPrice), 6);
                }
                clipboardFuturesTrade.FullVolume = totalVolume;
                clipboardFuturesTrade.TotalCost = totalCost;
                clipboardFuturesTrade.Commission = totalCommission;
                clipboardFuturesTrade.NFAFee = totalNFAFee;
                clipboardFuturesTrade.ClearingFee = totalClearingFee;
                clipboardFuturesTrade.ExchangeFee = totalExchangeFee;
            }
        }
        private void CopyOptionTrade()
        {
            decimal totalPrice = 0;
            int totalVolume = 0;
            double totalCost = 0;
            clipboardOptionTrade = null;

            // get state manager for the datagridview generating this call
            DataGridStateManager stateManager = GetStateManagerFromDataGridView(currentDataGridView);

            foreach (DataGridViewRow row in stateManager.RowsToProcess)
            {
                clipboardOptionTrade = GetOptionTradeFromDataGridViewRow(row, currentDataGridView);
                totalVolume += clipboardOptionTrade.TradeVolume;
                totalPrice += clipboardOptionTrade.TradeVolume * clipboardOptionTrade.TradePrice;
                totalCost += clipboardOptionTrade.TotalCost;
            }

            if (clipboardOptionTrade != null)
            {
                clipboardOptionTrade.TradePrice = Decimal.Round(totalPrice / totalVolume, 6);
                clipboardOptionTrade.TradeVolume = totalVolume;
                clipboardOptionTrade.TotalCost = totalCost;
            }
        }

        private static int FindColumnIndexByDataPropertyName(DataGridView dataGridView, string dataPropertyName)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.DataPropertyName.CompareTo(dataPropertyName) == 0)
                {
                    return column.Index;
                }
            }
            return -1;
        }

        private void ShowFinalizationState()
        {
            if ((!AccountGroupSelected) || !CurrentDatesImportDone())
            {
                finalizationStatusLabel1.Text = ImportStatusMsg;
                finalizationStatusLabel2.Text = ImportStatusMsg;
                finalizationStatusLabel9.Text = ImportStatusMsg;
                finalizationStatusLabel1.IsLink = false;
                finalizationStatusLabel2.IsLink = false;
                finalizationStatusLabel9.IsLink = false;
                totalCostLabel1.Text = "";
                totalCostLabel2.Text = "";
                totalCostLabel9.Text = "";
                totalCostLabel1.IsLink = false;
                totalCostLabel2.IsLink = false;
                totalCostLabel9.IsLink = false;
                missingStockPricesLabel.Text = "";
                missingStockPricesLabel.IsLink = false;
            }
            else
            {
                finalizationStatusLabel1.Text = String.Format("{0}{1} price discrepanc{2}{3}",
                    GetAccountGroupInfo().IsFinalized ? "Finalized, " : "",
                    Utilities.StockPriceDiscrepancyCount,
                    (Utilities.StockPriceDiscrepancyCount == 1) ? "y" : "ies",
                    Utilities.UsingTaxLots ? "," : "");
                finalizationStatusLabel2.Text = String.Format("{0}{1} price discrepanc{2}{3}",
                    GetAccountGroupInfo().IsFinalized ? "Finalized, " : "",
                    Utilities.OptionPriceDiscrepancyCount,
                    (Utilities.OptionPriceDiscrepancyCount == 1) ? "y" : "ies",
                      Utilities.UsingTaxLots ? "," : "");
                finalizationStatusLabel9.Text = String.Format("{0}{1} price discrepanc{2}{3}",
                    GetAccountGroupInfo().IsFinalized ? "Finalized, " : "",
                    Utilities.FuturesPriceDiscrepancyCount,
                    (Utilities.FuturesPriceDiscrepancyCount == 1) ? "y" : "ies",
                      Utilities.UsingTaxLots ? "," : "");
                finalizationStatusLabel1.IsLink = (Utilities.StockPriceDiscrepancyCount > 0);
                finalizationStatusLabel2.IsLink = (Utilities.OptionPriceDiscrepancyCount > 0);
                finalizationStatusLabel9.IsLink = (Utilities.FuturesPriceDiscrepancyCount > 0);

                missingStockPricesLabel.Text = String.Format("{0} missing stock price{1}",
                    Utilities.MissingConcurrentStockPriceCount,
                    (Utilities.MissingConcurrentStockPriceCount == 1) ? "" : "s");
                missingStockPricesLabel.IsLink = (Utilities.MissingConcurrentStockPriceCount > 0);

                if (Utilities.UsingTaxLots)
                {
                    totalCostLabel1.Text = String.Format("{0} total cost discrepanc{1}",
                      Utilities.StockTotalCostDiscrepancyCount,
                      (Utilities.StockTotalCostDiscrepancyCount == 1) ? "y" : "ies");
                    totalCostLabel2.Text = String.Format("{0} total cost discrepanc{1}",
                        Utilities.OptionTotalCostDiscrepancyCount,
                        (Utilities.OptionTotalCostDiscrepancyCount == 1) ? "y" : "ies");
                    totalCostLabel9.Text = String.Format("{0} total cost discrepanc{1}",
                      Utilities.FuturesTotalCostDiscrepancyCount,
                      (Utilities.FuturesTotalCostDiscrepancyCount == 1) ? "y" : "ies");
                    totalCostLabel1.IsLink = (Utilities.StockTotalCostDiscrepancyCount > 0);
                    totalCostLabel2.IsLink = (Utilities.OptionTotalCostDiscrepancyCount > 0);
                    totalCostLabel9.IsLink = (Utilities.FuturesTotalCostDiscrepancyCount > 0);
                }
                else
                {
                    totalCostLabel1.Text = "";
                    totalCostLabel2.Text = "";
                    totalCostLabel9.Text = "";
                    totalCostLabel1.IsLink = false;
                    totalCostLabel2.IsLink = false;
                    totalCostLabel9.IsLink = false;
                }
            }
        }

        private static string BuildExpirationAndAssignmentMessage(int addedHugoExpirations, int notAddedHugoExpirations, int errorsAddingHugoExpirations, int addedHugoAssignments, int notAddedHugoAssignments, int errorsAddingHugoAssignments, int addedMerrillAssignments, int errorsAddingMerrillAssignments)
        {
            string addedHugoAssignmentsMsg = BuildSingularOrPluralMessage("assignment", addedHugoAssignments);
            string notAddedHugoAssignmentsMsg = BuildSingularOrPluralMessage("assignment", notAddedHugoAssignments);
            string errorsAddingHugoAssignmentsMsg = BuildSingularOrPluralMessage("assignment", errorsAddingHugoAssignments);
            string addedHugoExpirationsMsg = BuildSingularOrPluralMessage("expiration", addedHugoExpirations);
            string notAddedHugoExpirationsMsg = BuildSingularOrPluralMessage("expiration", notAddedHugoExpirations);
            string errorsAddingHugoExpirationsMsg = BuildSingularOrPluralMessage("expiration", errorsAddingHugoExpirations);
            string addedMerrillAssignmentsMsg = BuildSingularOrPluralMessage("assignment", addedMerrillAssignments);
            string errorsAddingMerrillAssignmentsMsg = BuildSingularOrPluralMessage("assignment", errorsAddingMerrillAssignments);

            string message = BuildExpirationAndAssignmentLine("Added {0}.\n", addedHugoAssignments, addedHugoExpirations, addedMerrillAssignments, addedHugoAssignmentsMsg, addedHugoExpirationsMsg, addedMerrillAssignmentsMsg);
            message += BuildExpirationAndAssignmentLine("Did not add {0} because they might be duplicates or because the options have expired.\n", notAddedHugoAssignments, notAddedHugoExpirations, 0, notAddedHugoAssignmentsMsg, notAddedHugoExpirationsMsg, String.Empty);
            message += BuildExpirationAndAssignmentLine("Did not add {0} because of errors (see Info log).", errorsAddingHugoAssignments, errorsAddingHugoExpirations, 0, errorsAddingHugoAssignmentsMsg, errorsAddingHugoExpirationsMsg, errorsAddingMerrillAssignmentsMsg);

            return String.IsNullOrEmpty(message) ? "No assignments or expirations found" : message;
        }

        private static string BuildExpirationAndAssignmentLine(string message, int hugoAssignments, int hugoExpirations, int merrillAssignments, string hugoAssignmentsMsg, string hugoExpirationsMsg, string merrillAssignmentsMsg)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (hugoAssignments > 0)
            {
                strBuilder.Append(hugoAssignmentsMsg + " to Hugo");
                if ((hugoExpirations > 0) && (merrillAssignments > 0))
                    strBuilder.Append(", ");
                else if ((hugoExpirations > 0) || (merrillAssignments > 0))
                    strBuilder.Append(" and ");
            }
            if (merrillAssignments > 0)
            {
                strBuilder.Append(merrillAssignmentsMsg + " to " + clearingHouseText);
                if ((hugoExpirations > 0) && (hugoAssignments > 0))
                    strBuilder.Append(", and ");
                else if (hugoExpirations > 0) 
                    strBuilder.Append(" and ");
            }
            if (hugoExpirations > 0)
            {
                strBuilder.Append(hugoExpirationsMsg + " to Hugo");
            }

            if (strBuilder.Length > 0)
                return String.Format(message, strBuilder.ToString());
            else
                return String.Empty;
        }

        private static string BuildSingularOrPluralMessage(string messageLabel, int messageValue)
        {
            return String.Format("{0} {1}{2}", messageValue, messageLabel, (messageValue > 1) ? "s" : String.Empty);
        }
        #endregion

 
  }
}
