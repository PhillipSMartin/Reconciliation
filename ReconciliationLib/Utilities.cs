using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LoggingUtilitiesLib;
using System.Xml;
using System.IO;

namespace ReconciliationLib
{
    public sealed class Utilities
    {
        #region Declarations
        // Trade medium names
        private const string exerciseAssignmentName = "Exercise/Assign";
        private const string transferName = "Transfer";
        private const string exerciseName = "Exercised";
        private const string assignmentName = "Assigned";
        private const string reorgName = "Reorg";
        private const string expiredName = "Expired";
        private const string cancelName = "Cancel";
        private const string correctionName = "Correct";
        private const string OPRASymbology = "OPRA";
        private const string taskSuffix = "Reconciliation";

        private static bool initialized;
        private static HugoDataSet hugoDataSet = new HugoDataSet();
        private static SqlConnection connection;

        private static int stockDiscrepancyCount;
        private static int stockPriceDiscrepancyCount;
        private static int stockTotalCostDiscrepancyCount;
        private static int optionDiscrepancyCount;
        private static int optionPriceDiscrepancyCount;
        private static int optionTotalCostDiscrepancyCount;
        private static int futuresDiscrepancyCount;
        private static int futuresPriceDiscrepancyCount;
        private static int futuresTotalCostDiscrepancyCount;
        private static string optionSymbology;

 //       private static LiquidHistoricalDatabaseLib.LiquidHistoricalDatabase liquidUtils = null;

        #region saved variables for HugoOptionTrades table
        private static string saveUnderlyingSymbol;
        private static string saveOptionSymbol;
        private static DateTime? saveExpirationDate;
        private static decimal? saveStrikePrice;
        private static string saveOptionType;
        private static string saveRequestedOptions;
        #endregion

        # region table adapters
        // To add a new table adapter -
        //  1. Open HugoDataSet.xsd
        //  2. Right-click on form and select 'Add->', then 'TableAdapter...'
        //  3. Accept the default connection (Settings.HugoConnectionString, which is Gardevsrv05) by clicking 'Next>'
        //  4. Select either 'Use existing stored procedure' or 'Use SQL Statements' and click 'Next>'
        //  5. If using SQL Statements, enter the statements.  If this
        //      is for a combobox, you may want to add 'UNION SELECT ...., NULL as ....' to add a null option and
        //      'ORDER BY ...'
        //  6. Click 'Finish'. (You may get errors generating INSERT, UPDATE, and DELETE statements, which you
        //      can ignore).
        //  7. Add a declaration below for the new table adapter.
        //  8. Add a bool in the 'flags to indicate whether tables have been populated' section,
        //          an internal property to get the bool,
        //          and an entry in the TableUpdateEventArgs to pass the bool to the user
        //          a line in Refresh (and wherever else it is needed) to set the bool to false
        //  9. Add a line to Dispose() for the new table adapter.
        // 10. Add a line to the Connection set accessor to set the filled flag to false
        // 11. Add a line to the Connection set accessor for the new table adapter.
        // 12. Add a Fill... method in the 'Fill methods' section modeled after other fill methods.  
        //      Call CheckParameters(true) if you need to ensure that an account group has been selected; 
        //      otherwise call CheckParameters(false).
        // 13. Add a property to return the Table in the 'tables' region, modeled after other such properties.
        // 14. Add a GetReturnValue method to the adapter in HugoDataSet.cs modeled after existing GetReturnValue
        //      methods.  This will enable you to get the return code of any sp you call on that adapter.
        // 15. Update the TableUpdatedEventArgs class - add a private and public property and update the
        //      constructor and ToString() method
        // 16. See comments in ReconciliationClient.Form1 above 'FillDataGridComboBoxes' method if you want to add
        //      a combobox column to a datagrid containing entries from this table.
        //
        // To add a stored procedure
        //  1. Open HugoDataSet.xsd
        //  2. Right-click on either the association table adapter or, for a generalized query, on the
        //      QueriesTableAdapter and select 'Add Query...'
        //  3. Accept the default connection (Settings.HugoConnectionString, which is Gardevsrv05) by clicking 'Next>'
        //  4. Select 'Use existing stored procedure' and click 'Next>'
        //  5. Select 'No value' (a return code does not count as a returned value) and click 'Next>'
        //  6. Enter a name for the method and click 'Finish'.
        //  7. Add the name of the method to the CommandIndex enum above.
        //  8. Add a method to call the sp modeled after similar methods (such as AddStockTrade).
        //      Call CheckParameters(true) if you need to ensure that an account group has been selected; 
        //      otherwise call CheckParameters(false).
        //      If you wish to check the return code, call <tableAdapter>.GetReturnValue(<command text>)
        //  9. It is unnecessary to add lines to Dispose or the Connection get accessor.
        //
        private static HugoDataSetTableAdapters.StockPositionsTableAdapter stockPositionsAdapter = new ReconciliationLib.HugoDataSetTableAdapters.StockPositionsTableAdapter();
        private static HugoDataSetTableAdapters.OptionPositionsTableAdapter optionPositionsAdapter = new ReconciliationLib.HugoDataSetTableAdapters.OptionPositionsTableAdapter();
        private static HugoDataSetTableAdapters.FuturesPositionsTableAdapter futuresPositionsAdapter = new ReconciliationLib.HugoDataSetTableAdapters.FuturesPositionsTableAdapter();
        private static HugoDataSetTableAdapters.AccountGroupsTableAdapter accountGroupsAdapter = new ReconciliationLib.HugoDataSetTableAdapters.AccountGroupsTableAdapter();
        private static HugoDataSetTableAdapters.QueriesTableAdapter queriesAdapter = new HugoDataSetTableAdapters.QueriesTableAdapter();
        private static HugoDataSetTableAdapters.HugoStockTradesTableAdapter hugoStockTradesAdapter = new HugoDataSetTableAdapters.HugoStockTradesTableAdapter();
        private static HugoDataSetTableAdapters.MerrillStockTradesTableAdapter merrillStockTradesAdapter = new HugoDataSetTableAdapters.MerrillStockTradesTableAdapter();
        private static HugoDataSetTableAdapters.HugoOptionTradesTableAdapter hugoOptionTradesAdapter = new HugoDataSetTableAdapters.HugoOptionTradesTableAdapter();
        private static HugoDataSetTableAdapters.MerrillOptionTradesTableAdapter merrillOptionTradesAdapter = new HugoDataSetTableAdapters.MerrillOptionTradesTableAdapter();
        private static HugoDataSetTableAdapters.HugoFuturesTradesTableAdapter hugoFuturesTradesAdapter = new HugoDataSetTableAdapters.HugoFuturesTradesTableAdapter();
        private static HugoDataSetTableAdapters.MerrillFuturesTradesTableAdapter merrillFuturesTradesAdapter = new HugoDataSetTableAdapters.MerrillFuturesTradesTableAdapter();
        private static HugoDataSetTableAdapters.SubaccountNamesTableAdapter subaccountNamesAdapter = new HugoDataSetTableAdapters.SubaccountNamesTableAdapter();
        private static HugoDataSetTableAdapters.TradersTableAdapter tradersAdapter = new HugoDataSetTableAdapters.TradersTableAdapter();
        private static HugoDataSetTableAdapters.TradeMediaTableAdapter tradeMediaAdapter = new HugoDataSetTableAdapters.TradeMediaTableAdapter();
        private static HugoDataSetTableAdapters.TradeTypesTableAdapter tradeTypesAdapter = new HugoDataSetTableAdapters.TradeTypesTableAdapter();
        private static HugoDataSetTableAdapters.ExchangesTableAdapter exchangesAdapter = new HugoDataSetTableAdapters.ExchangesTableAdapter();
        private static HugoDataSetTableAdapters.BrokersTableAdapter brokersAdapter = new HugoDataSetTableAdapters.BrokersTableAdapter();
        private static HugoDataSetTableAdapters.StockTradeReasonsTableAdapter stockTradeReasonsAdapter = new HugoDataSetTableAdapters.StockTradeReasonsTableAdapter();
        private static HugoDataSetTableAdapters.OptionTradeReasonsTableAdapter optionTradeReasonsAdapter = new HugoDataSetTableAdapters.OptionTradeReasonsTableAdapter();
        private static HugoDataSetTableAdapters.ConfirmationCorrectionsTableAdapter confirmationCorrectionsAdapter = new HugoDataSetTableAdapters.ConfirmationCorrectionsTableAdapter();
        private static HugoDataSetTableAdapters.SymbolMappingsTableAdapter symbolMappingsAdapter = new HugoDataSetTableAdapters.SymbolMappingsTableAdapter();
        private static HugoDataSetTableAdapters.HugoStockCorrectionsTableAdapter hugoStockCorrectionsAdapter = new HugoDataSetTableAdapters.HugoStockCorrectionsTableAdapter();
        private static HugoDataSetTableAdapters.HugoOptionCorrectionsTableAdapter hugoOptionCorrectionsAdapter = new HugoDataSetTableAdapters.HugoOptionCorrectionsTableAdapter();
        private static HugoDataSetTableAdapters.HugoFuturesCorrectionsTableAdapter hugoFuturesCorrectionsAdapter = new HugoDataSetTableAdapters.HugoFuturesCorrectionsTableAdapter();
        private static HugoDataSetTableAdapters.TraderAccountsTableAdapter traderAccountsAdapter = new ReconciliationLib.HugoDataSetTableAdapters.TraderAccountsTableAdapter();
        private static HugoDataSetTableAdapters.ClearingHouseFileNamesTableAdapter clearingHouseFileNamesAdapter = new ReconciliationLib.HugoDataSetTableAdapters.ClearingHouseFileNamesTableAdapter();

        internal static HugoDataSetTableAdapters.QueriesTableAdapter QueriesAdapter { get { return queriesAdapter; } }
        #endregion

        #region flags to indicate whether tables have been populated
        //  set to false if any property changes that requires the table to be reloaded
        private static bool stockPositionsTableFilled;
        private static bool optionPositionsTableFilled;
        private static bool futuresPositionsTableFilled;
        private static bool accountGroupsTableFilled;
        private static bool hugoStockTradesTableFilled;
        private static bool merrillStockTradesTableFilled;
        private static bool hugoFuturesTradesTableFilled;
        private static bool merrillFuturesTradesTableFilled;
        private static bool unconsolidatedHugoOptionTradesTableFilled;
        private static bool consolidatedHugoOptionTradesTableFilled;
        private static bool optionTradesWithMissingStockPricesTableFilled;
        private static bool merrillOptionTradesTableFilled;
        private static bool subAccountNamesTableFilled;
        private static bool tradersTableFilled;
        private static bool tradeMediaTableFilled;
        private static bool tradeTypesTableFilled;
        private static bool exchangesTableFilled;
        private static bool brokersTableFilled;
        private static bool stockTradeReasonsTableFilled;
        private static bool optionTradeReasonsTableFilled;
        private static bool confirmationCorrectionsTodayTableFilled;
        private static bool confirmationCorrectionsYesterdayTableFilled;
        private static bool symbolMappingsTableFilled;
        private static bool hugoStockCorrectionsTableFilled;
        private static bool hugoOptionCorrectionsTableFilled;
        private static bool hugoFuturesCorrectionsTableFilled;
        private static bool traderAccountsTableFilled;
        private static bool clearingHouseFileNamesTableFilled;
        #endregion

        // the current date - (which can be changed if we need to see what our positions were at
        //  an earlier point in time)
        // if 'null', the Hugo sp's will use GetDate() to retrieve today's date
        private static System.Nullable<DateTime> importDate;
        // one business day before 'current' date 
        private static System.Nullable<DateTime> previousDate;

        // an account group (from the t_AccountGroups table in Hugo) contains all accounts that you wish
        //  to reconcile simultaneously
        private static HugoDataSet.AccountGroupsRow accountGroup;
        private static AccountGroupInfo accountGroupInfo;
        private static IClearingHouse clearingHouse = new ClearingHouseBase();
        private static string traderName;

        #region event handlers
        // event fired when tables are updated
        private static EventHandler<TablesUpdatedEventArgs> tablesUpdatedEventHandler;
        #endregion

        #endregion

        #region Constructor
        private Utilities()
        {
        }
        #endregion

        #region Public Properties
        // an account group (from the t_AccountGroups table in Hugo) contains all accounts that you wish
        //  to reconcile simultaneously
        public static string AccountGroupName
        {
            get
            {
                if (accountGroup == null)
                    return null;
                else
                    return accountGroup.AcctGroupName;
            }
            set
            {
                if (AccountGroupName != value)
                {
                    HugoDataSet.AccountGroupsRow row = AccountGroups.FindByAcctGroupName(value);
                    if (row == null)
                    {
                        Error("Error setting account group name", new ReconciliationException(String.Format("Cannot find account group {0}", value)));
                    }
                    else
                    {
                        Info(String.Format("Account group name changed from {0} to {1}", AccountGroupName, value));
                        accountGroup = row;
                        clearingHouse = ClearingHouseFactory.GetClearingHouse(row);

                        subAccountNamesTableFilled = false;
                        stockPositionsTableFilled = false;
                        optionPositionsTableFilled = false;
                        futuresPositionsTableFilled = false;
                        HugoOptionTradesTableFilled = false;
                        hugoStockTradesTableFilled = false;
                        hugoFuturesTradesTableFilled = false;
                        merrillOptionTradesTableFilled = false;
                        merrillStockTradesTableFilled = false;
                        merrillFuturesTradesTableFilled = false;
                        confirmationCorrectionsTodayTableFilled = false;
                        confirmationCorrectionsYesterdayTableFilled = false;
                        hugoStockCorrectionsTableFilled = false;
                        hugoOptionCorrectionsTableFilled = false;
                        hugoFuturesCorrectionsTableFilled = false;
                        tradersTableFilled = false;
                        traderAccountsTableFilled = false;
                        symbolMappingsTableFilled = false;
                        accountGroupInfo = null;
                        FireOnTablesUpdated();
                    }
                }
            }
        }
        public static bool UsingTaxLots
        {
            get
            {
                if (accountGroup == null)
                    return false;
                else
                    return accountGroup.UsesTaxlots > 0;
            }
        }
        public static bool UsingOPRASymbology
        {
            get
            {
                return optionSymbology == OPRASymbology;
            }
        }
        public static ClearingHouse ClearingHouse
        {
            get
            {
                return clearingHouse.ClearingHouse;
            }
        }
        public static IClearingHouse ClearingHouseInfo
        {
            get
            {
                return clearingHouse;
            }
        }
        public static string TraderName
        {
            get { return traderName; }
            set
            {
                Info(String.Format("Trader name changed from {0} to {1}", traderName, value));
                traderName = value;
            }
        }
        // the 'current' date - (which can be changed if we need to see what our positions were at
        //  an earlier point in time)
        // if 'null', the Hugo sp's will use GetDate() to retrieve today's date
        public static System.Nullable<DateTime> ImportDate
        {
            get { return importDate; }
            set
            {
                Info(String.Format("ImportDate changed from {0} to {1}",
                    ReconciliationConvert.ToNullableString(importDate),
                    ReconciliationConvert.ToNullableString(value)));

                if (value.HasValue)
                {
                    importDate = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day); // make sure there is no time
                }
                else
                {
                    importDate = null;
                }
                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                HugoOptionTradesTableFilled = false;
                hugoStockTradesTableFilled = false;
                hugoFuturesTradesTableFilled = false;
                merrillOptionTradesTableFilled = false;
                merrillStockTradesTableFilled = false;
                merrillFuturesTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                hugoStockCorrectionsTableFilled = false;
                hugoOptionCorrectionsTableFilled = false;
                hugoFuturesCorrectionsTableFilled = false;
                FireOnTablesUpdated();
            }
        }
        // one business day before 'current' date 
        public static System.Nullable<DateTime> PreviousDate
        {
            get { return previousDate; }
            set
            {
                Info(String.Format("PreviousDate changed from {0} to {1}",
                    ReconciliationConvert.ToNullableString(previousDate),
                    ReconciliationConvert.ToNullableString(value)));

                if (value.HasValue)
                {
                    previousDate = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day); // make sure there is no time
                }
                else
                {
                    previousDate = null;
                }
                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                HugoOptionTradesTableFilled = false;
                hugoStockTradesTableFilled = false;
                hugoFuturesTradesTableFilled = false;
                confirmationCorrectionsYesterdayTableFilled = false;
                FireOnTablesUpdated();
            }
        }
        public static string LiquidConnectionString { set; get; }
        public static SqlConnection Connection
        {
            set
            {
                Info(String.Format("Connection changed from {0} to {1}",
                    (connection == null) ? "<NULL>" : connection.ConnectionString,
                    (value == null) ? "<NULL>" : value.ConnectionString));

                connection = value;

                // when the user changes the connection, it must be changed in each table adapter
                stockPositionsAdapter.Connection = value;
                optionPositionsAdapter.Connection = value;
                futuresPositionsAdapter.Connection = value;
                accountGroupsAdapter.Connection = value;
                queriesAdapter.Connection = value;
                hugoStockTradesAdapter.Connection = value;
                hugoFuturesTradesAdapter.Connection = value;
                merrillStockTradesAdapter.Connection = value;
                merrillFuturesTradesAdapter.Connection = value;
                hugoOptionTradesAdapter.Connection = value;
                merrillOptionTradesAdapter.Connection = value;
                subaccountNamesAdapter.Connection = value;
                tradersAdapter.Connection = value;
                tradeMediaAdapter.Connection = value;
                tradeTypesAdapter.Connection = value;
                exchangesAdapter.Connection = value;
                brokersAdapter.Connection = value;
                stockTradeReasonsAdapter.Connection = value;
                optionTradeReasonsAdapter.Connection = value;
                confirmationCorrectionsAdapter.Connection = value;
                 symbolMappingsAdapter.Connection = value;
                hugoStockCorrectionsAdapter.Connection = value;
                hugoOptionCorrectionsAdapter.Connection = value;
                hugoFuturesCorrectionsAdapter.Connection = value;
                traderAccountsAdapter.Connection = value;
                clearingHouseFileNamesAdapter.Connection = value;

                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                accountGroupsTableFilled = false;
                subAccountNamesTableFilled = false;
                tradersTableFilled = false;
                tradeMediaTableFilled = false;
                tradeTypesTableFilled = false;
                exchangesTableFilled = false;
                brokersTableFilled = false;
                stockTradeReasonsTableFilled = false;
                optionTradeReasonsTableFilled = false;
                HugoOptionTradesTableFilled = false;
                hugoStockTradesTableFilled = false;
                hugoFuturesTradesTableFilled = false;
                merrillOptionTradesTableFilled = false;
                merrillStockTradesTableFilled = false;
                merrillFuturesTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                confirmationCorrectionsYesterdayTableFilled = false;
                 symbolMappingsTableFilled = false;
                hugoStockCorrectionsTableFilled = false;
                hugoOptionCorrectionsTableFilled = false;
                hugoFuturesCorrectionsTableFilled = false;
                traderAccountsTableFilled = false;
                clearingHouseFileNamesTableFilled = false;

                queriesAdapter.SetAllCommandTimeouts(0);
                stockPositionsAdapter.SetAllCommandTimeouts(0);
                optionPositionsAdapter.SetAllCommandTimeouts(0);
                accountGroupsAdapter.SetAllCommandTimeouts(0);
                hugoStockTradesAdapter.SetAllCommandTimeouts(0);
                hugoFuturesTradesAdapter.SetAllCommandTimeouts(0);
                merrillStockTradesAdapter.SetAllCommandTimeouts(0);
                merrillFuturesTradesAdapter.SetAllCommandTimeouts(0);
                hugoOptionTradesAdapter.SetAllCommandTimeouts(0);
                merrillOptionTradesAdapter.SetAllCommandTimeouts(0);
                subaccountNamesAdapter.SetAllCommandTimeouts(0);
                confirmationCorrectionsAdapter.SetAllCommandTimeouts(0);
                symbolMappingsAdapter.SetAllCommandTimeouts(0);
                hugoStockCorrectionsAdapter.SetAllCommandTimeouts(0);
                hugoOptionCorrectionsAdapter.SetAllCommandTimeouts(0);
                hugoFuturesCorrectionsAdapter.SetAllCommandTimeouts(0);
                traderAccountsAdapter.SetAllCommandTimeouts(0);
                clearingHouseFileNamesAdapter.SetAllCommandTimeouts(0);

                tradersAdapter.SetAllCommandTimeouts(0);
                tradeTypesAdapter.SetAllCommandTimeouts(0);
                tradeMediaAdapter.SetAllCommandTimeouts(0);
                exchangesAdapter.SetAllCommandTimeouts(0);
                brokersAdapter.SetAllCommandTimeouts(0);
                stockTradeReasonsAdapter.SetAllCommandTimeouts(0);
                optionTradeReasonsAdapter.SetAllCommandTimeouts(0);

                optionSymbology = queriesAdapter.GetOptionSymbology();
                FireOnTablesUpdated();
            }
            get
            {
                return connection;
            }
        }

        // Number of records in StockPositions table with discrepancies
        public static int StockDiscrepancyCount
        {
            get
            {
                if (!stockPositionsTableFilled)
                    FillStockPositions();
                return stockDiscrepancyCount;
            }
        }

        // Number of records in OptionPositions table with discrepancies
        public static int OptionDiscrepancyCount
        {
            get
            {
                if (!optionPositionsTableFilled)
                    FillOptionPositions();
                return optionDiscrepancyCount;
            }
        }

        // Number of records in OptionPositions table with discrepancies
        public static int FuturesDiscrepancyCount
        {
            get
            {
                if (!futuresPositionsTableFilled)
                    FillFuturesPositions();
                return futuresDiscrepancyCount;
            }
        }

        // Number of records in StockPositions table with price discrepancies
        public static int StockPriceDiscrepancyCount
        {
            get
            {
                return stockPriceDiscrepancyCount;
            }
        }
        // Number of records in StockPositions table with total cost discrepancies
        public static int StockTotalCostDiscrepancyCount
        {
            get
            {
                return stockTotalCostDiscrepancyCount;
            }
        }

        // Number of records in OptionPositions table with price discrepancies
        public static int OptionPriceDiscrepancyCount
        {
            get
            {
                return optionPriceDiscrepancyCount;
            }
        }

        // Number of records in OptionPositions table with total cost discrepancies
        public static int OptionTotalCostDiscrepancyCount
        {
            get
            {
                return optionTotalCostDiscrepancyCount;
            }
        }
        // Number of records in FuturesPositions table with price discrepancies
        public static int FuturesPriceDiscrepancyCount
        {
            get
            {
                return futuresPriceDiscrepancyCount;
            }
        }

        // Number of records in FuturesPositions table with total cost discrepancies
        public static int FuturesTotalCostDiscrepancyCount
        {
            get
            {
                return futuresTotalCostDiscrepancyCount;
            }
        }


        // Number of option trades with missing stock prices
        public static int MissingConcurrentStockPriceCount
        {
            get
            {
                return OptionTradesWithMissingStockPrices.Rows.Count;
            }
        }
        // Return state info for account group
        public static AccountGroupInfo AccountGroupInfo
        {
            get
            {
                if ((accountGroupInfo == null) && (AccountGroupName != null))
                {
                    accountGroupInfo = GetAccountGroupInfo();
                }

                return accountGroupInfo;
            }
        }

        public static string ExerciseAssignmentName
        {
            get { return exerciseAssignmentName; }
        }
        public static string TransferName
        {
            get { return transferName; }
        }
        public static string ReorgName
        {
            get { return reorgName; }
        }

        #region tables
        public static HugoDataSet.StockPositionsDataTable StockPositions
        {
            get
            {
                if (!stockPositionsTableFilled)
                    FillStockPositions();
                return hugoDataSet.StockPositions;
            }
        }
        public static HugoDataSet.OptionPositionsDataTable OptionPositions
        {
            get
            {
                if (!optionPositionsTableFilled)
                    FillOptionPositions();
                return hugoDataSet.OptionPositions;
            }
        }
        public static HugoDataSet.FuturesPositionsDataTable FuturesPositions
        {
            get
            {
                if (!futuresPositionsTableFilled)
                    FillFuturesPositions();
                return hugoDataSet.FuturesPositions;
            }
        }
        public static HugoDataSet.AccountGroupsDataTable AccountGroups
        {
            get
            {
                if (!accountGroupsTableFilled)
                    FillAccountGroups();
                return hugoDataSet.AccountGroups;
            }
        }
        public static HugoDataSet.SubaccountNamesDataTable SubaccountNames
        {
            get
            {
                if (!subAccountNamesTableFilled)
                    FillSubaccountNames();
                return hugoDataSet.SubaccountNames;
            }
        }
        public static HugoDataSet.TradersDataTable Traders
        {
            get
            {
                if (!tradersTableFilled)
                    FillTraders();
                return hugoDataSet.Traders;
            }
        }
        public static HugoDataSet.TradeMediaDataTable TradeMedia
        {
            get
            {
                if (!tradeMediaTableFilled)
                    FillTradeMedia();
                return hugoDataSet.TradeMedia;
            }
        }
        public static HugoDataSet.TradeTypesDataTable TradeTypes
        {
            get
            {
                if (!tradeTypesTableFilled)
                    FillTradeMedia();
                return hugoDataSet.TradeTypes;
            }
        }
        public static HugoDataSet.ClearingHouseFileNamesDataTable ClearingHouseFileNames
        {
            get
            {
                if (!clearingHouseFileNamesTableFilled)
                    FillClearingHouseFileNames();
                return hugoDataSet.ClearingHouseFileNames;
            }
        }
        public static HugoDataSet.ExchangesDataTable Exchanges
        {
            get
            {
                if (!exchangesTableFilled)
                    FillExchanges();
                return hugoDataSet.Exchanges;
            }
        }
        public static HugoDataSet.BrokersDataTable Brokers
        {
            get
            {
                if (!brokersTableFilled)
                    FillBrokers();
                return hugoDataSet.Brokers;
            }
        }
        public static HugoDataSet.StockTradeReasonsDataTable StockTradeReasons
        {
            get
            {
                if (!stockTradeReasonsTableFilled)
                    FillStockTradeReasons();
                return hugoDataSet.StockTradeReasons;
            }
        }
        public static HugoDataSet.OptionTradeReasonsDataTable OptionTradeReasons
        {
            get
            {
                if (!optionTradeReasonsTableFilled)
                    FillOptionTradeReasons();
                return hugoDataSet.OptionTradeReasons;
            }
        }
        public static HugoDataSet.ConfirmationCorrectionsDataTable ConfirmationCorrectionsToday
        {
            get
            {
                if (!confirmationCorrectionsTodayTableFilled)
                    FillConfirmationCorrectionsToday();
                return hugoDataSet.ConfirmationCorrectionsToday;
            }
        }
        public static HugoDataSet.ConfirmationCorrectionsDataTable ConfirmationCorrectionsYesterday
        {
            get
            {
                if (!confirmationCorrectionsYesterdayTableFilled)
                    FillConfirmationCorrectionsYesterday();
                return hugoDataSet.ConfirmationCorrectionsYesterday;
            }
        }
        public static HugoDataSet.SymbolMappingsDataTable SymbolMappings
        {
            get
            {
                if (!symbolMappingsTableFilled)
                    FillSymbolMappings();
                return hugoDataSet.SymbolMappings;
            }
        }
        public static HugoDataSet.HugoStockCorrectionsDataTable HugoStockCorrections
        {
            get
            {
                if (!hugoStockCorrectionsTableFilled)
                    FillHugoStockCorrections();
                return hugoDataSet.HugoStockCorrections;
            }
        }
        public static HugoDataSet.HugoOptionCorrectionsDataTable HugoOptionCorrections
        {
            get
            {
                if (!hugoOptionCorrectionsTableFilled)
                    FillHugoOptionCorrections();
                return hugoDataSet.HugoOptionCorrections;
            }
        }
        public static HugoDataSet.HugoFuturesCorrectionsDataTable HugoFuturesCorrections
        {
            get
            {
                if (!hugoFuturesCorrectionsTableFilled)
                    FillHugoFuturesCorrections();
                return hugoDataSet.HugoFuturesCorrections;
            }
        }
        public static HugoDataSet.TraderAccountsDataTable TraderAccounts
        {
            get
            {
                if (!traderAccountsTableFilled)
                {
                    FillTraderAccounts();
                }
                return hugoDataSet.TraderAccounts;
            }
        }
        public static HugoDataSet.HugoOptionTradesDataTable UnconsolidatedHugoOptionTrades
        {
            get
            {
                if (!unconsolidatedHugoOptionTradesTableFilled)
                    FillUnconsolidatedHugoOptionTradesTable(saveUnderlyingSymbol, saveOptionSymbol, saveExpirationDate, saveStrikePrice, saveOptionType, saveRequestedOptions);
                return hugoDataSet.UnconsolidatedOptionTrades;
            }
        }
        public static HugoDataSet.HugoOptionTradesDataTable ConsolidatedHugoOptionTrades
        {
            get
            {
                if (!consolidatedHugoOptionTradesTableFilled)
                    FillConsolidatedHugoOptionTradesTable(saveUnderlyingSymbol, saveOptionSymbol, saveExpirationDate, saveStrikePrice, saveOptionType, saveRequestedOptions);
                return hugoDataSet.ConsolidatedOptionTrades;
            }
        }
        public static HugoDataSet.HugoOptionTradesDataTable OptionTradesWithMissingStockPrices
        {
            get
            {
                if (!optionTradesWithMissingStockPricesTableFilled)
                    FillOptionTradesWithMissingStockPricesTable();
                return hugoDataSet.OptionTradesWithMissingStockPrices;
            }
        }

        #region event handlers
        // event fired when an exception occurs
        public static event EventHandler<LoggingUtilitiesLib.ErrorEventArgs> OnError
        {
            add { LoggingUtilities.OnError += value; }
            remove { LoggingUtilities.OnError -= value; }
        }
        // event fired for logging
        public static event EventHandler<InfoEventArgs> OnInfo
        {
            add { LoggingUtilities.OnInfo += value; }
            remove { LoggingUtilities.OnInfo -= value; }
        }
        // event fired when tables are updated
        public static event EventHandler<TablesUpdatedEventArgs> OnTablesUpdated
        {
            add { tablesUpdatedEventHandler += value; }
            remove { tablesUpdatedEventHandler -= value; }
        }
        #endregion
        #endregion

        #endregion

        #region Public Methods
        public static void Init()
        {
            importDate = DateTime.Today;
            initialized = true;
        }

        public static void Dispose()
        {
            initialized = false;

            hugoDataSet.Dispose();
            stockPositionsAdapter.Dispose();
            optionPositionsAdapter.Dispose();
            futuresPositionsAdapter.Dispose();
            accountGroupsAdapter.Dispose();
            queriesAdapter.Dispose();
            hugoStockTradesAdapter.Dispose();
            merrillStockTradesAdapter.Dispose();
            hugoOptionTradesAdapter.Dispose();
            merrillOptionTradesAdapter.Dispose();
            hugoFuturesTradesAdapter.Dispose();
            merrillFuturesTradesAdapter.Dispose();
            subaccountNamesAdapter.Dispose();
            tradersAdapter.Dispose();
            tradeMediaAdapter.Dispose();
            tradeTypesAdapter.Dispose();
            exchangesAdapter.Dispose();
            brokersAdapter.Dispose();
            stockTradeReasonsAdapter.Dispose();
            optionTradeReasonsAdapter.Dispose();
            confirmationCorrectionsAdapter.Dispose();
             symbolMappingsAdapter.Dispose();
            hugoStockCorrectionsAdapter.Dispose();
            hugoOptionCorrectionsAdapter.Dispose();
            hugoFuturesCorrectionsAdapter.Dispose();
            traderAccountsAdapter.Dispose();
            clearingHouseFileNamesAdapter.Dispose();

        //    if (liquidUtils != null)
        //        liquidUtils.Dispose();
        }
        #region Import methods
        // see if import has been run already today - returns false or throws an exception if sp fails
        public static bool CheckImportState(out int numPositions,
           out int numConfirmations,
           out int numTaxlots,
           out int numBookkeepingEntries,
           out int numDividends)
        {
            CheckParameters(/*needAccountGroup =*/ true);
            return CheckImportState(out numPositions, out numConfirmations, out numTaxlots, out numBookkeepingEntries, out numDividends, null);
        }

        public static bool CheckImportState(out int numPositions,
            out int numConfirmations,
            out int numTaxlots, 
            out int numBookkeepingEntries, 
            out int numDividends,
            ClearingHouse? clearingHouse /* if null, account group must be set */)
        {
            Nullable<int> numPositionsNullable = null;
            Nullable<int> numConfirmationsNullable = null;
            Nullable<int> numBookkeepingEntriesNullable = null;
            Nullable<int> numAssignmentsNullable = null;
            Nullable<int> numTaxlotsNullable = null;
            Nullable<int> numDividendsNullable = null;
            bool bSucceeded = false;

            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                queriesAdapter.CheckImportState(ImportDate, AccountGroupName, ref numPositionsNullable, ref numConfirmationsNullable,
                    ref numBookkeepingEntriesNullable, ref numAssignmentsNullable, ref numTaxlotsNullable, ref numDividendsNullable,
                    clearingHouse.HasValue ? clearingHouse.Value.ToString() : null);
                bSucceeded = true;
            }
            catch (Exception e)
            {
                Error("Error checking import state", e);
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_check_import5");
            }

            numPositions = numPositionsNullable ?? 0;
            numConfirmations = numConfirmationsNullable ?? 0;
            numTaxlots = numTaxlotsNullable ?? 0;
            numBookkeepingEntries = numBookkeepingEntriesNullable ?? 0;
            numDividends = numDividendsNullable ?? 0;
            return bSucceeded;
        }


        #region import positions from the specified file
        public static int ImportPositions(string fileName, ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportPositionsMethod != null)
                {
                    // new methodology
                    count = ImportPositionsFromMethod(fileName, clearingHouseInfo);
                }
                else
                {
                    // legacy methodology
                    count = ImportPositionsFromRecordCollection(fileName, clearingHouseInfo);
                }
            }
            catch (Exception e)
            {
                Error("Error importing positions from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} position records from {1}", count, fileName));
            }

            stockPositionsTableFilled = false;
            optionPositionsTableFilled = false;
            futuresPositionsTableFilled = false;
            FireOnTablesUpdated();

            return count;
        }
   
        private static int ImportPositionsFromMethod(string fileName, IClearingHouse clearingHouseInfo)
        {
            try
            {
                string xml;
                int? numberTradesInserted = 0;

                // returns number of records read
                int rc = ConvertFileToXml(fileName, "Positions", out xml);
                if (rc > 0)
                {
                    rc = clearingHouseInfo.ImportPositionsMethod(xml, ImportDate, ref numberTradesInserted, false, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} from import positions method", rc));
                }
                return numberTradesInserted.Value;
            }
            catch (Exception e)
            {
                Error("Error importing positions", e);
                return 0;
            }
            finally
            {
                clearingHouseInfo.LogImportPositionsMethod();
            }
        }
    
        private static int ImportPositionsFromRecordCollection(string fileName, IClearingHouse clearingHouseInfo)
        {
            int count = 0;
            using (IPositionRecordCollection positionRecords = clearingHouseInfo.GetPositionRecordCollection(fileName))
            {
                PositionRecord positionRecord = positionRecords.NextRecord;

                using (IPositionTableAdapter positionTableAdapter = clearingHouseInfo.PositionTableAdapter)
                {
                    positionTableAdapter.SetAllCommandTimeouts(0);
                    while (positionRecord != null)
                    {
                        // insert method calls NextRecord
                        InsertPositionRecord(positionTableAdapter, ref count, positionRecords, ref positionRecord);
                    }
                }
            }
            return count;
        }
        #endregion
        #region import Bookkeeping from the specified file
        public static int ImportBookkeeping(string fileName, ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportBookkeepingMethod != null)
                {
                    count = ImportBookkeepingFromMethod(fileName, clearingHouseInfo);
                }
                else
                {
                    throw new ReconciliationImportException("Importing Bookkeeping not supported for clearing house " + clearingHouse.ToString());
                }
            }
            catch (Exception e)
            {
                Error("Error importing Bookkeeping from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} Bookkeeping from {1}", count, fileName));
            }

            return count;
        }

        private static int ImportBookkeepingFromMethod(string fileName, IClearingHouse clearingHouseInfo)
        {
            try
            {
                string xml;
                int? numberTradesInserted = 0;

                // returns number of records read
                int rc = ConvertFileToXml(fileName, "Bookkeeping", out xml);
                if (rc > 0)
                {
                    rc = clearingHouseInfo.ImportBookkeepingMethod(xml, ImportDate, ref numberTradesInserted, false, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} from import Bookkeeping method", rc));
                }
                return numberTradesInserted.Value;
            }
            catch (Exception e)
            {
                Error("Error importing Bookkeeping", e);
                return 0;
            }
            finally
            {
                clearingHouseInfo.LogImportBookkeepingMethod(); ;
            }
        }
        #endregion

        #region import dividends from the specified file
        public static int ImportDividends(string fileName, ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportDividendsMethod != null)
                {
                    count = ImportDividendsFromMethod(fileName, clearingHouseInfo);
                }
                else
                {
                    throw new ReconciliationImportException("Importing dividends not supported for clearing house " + clearingHouse.ToString());
                }
            }
            catch (Exception e)
            {
                Error("Error importing dividends from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} dividends from {1}", count, fileName));
            }

            return count;
        }

        private static int ImportDividendsFromMethod(string fileName, IClearingHouse clearingHouseInfo)
        {
            try
            {
                string xml;
                int? numberTradesInserted = 0;

                // returns number of records read
                int rc = ConvertFileToXml(fileName, "Dividends", out xml);
                if (rc > 0)
                {
                    rc = clearingHouseInfo.ImportDividendsMethod(xml, ImportDate, ref numberTradesInserted, false, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} from import dividends method", rc));
                }
                return numberTradesInserted.Value;
            }
            catch (Exception e)
            {
                Error("Error importing dividends", e);
                return 0;
            }
            finally
            {
                clearingHouseInfo.LogImportDividendsMethod(); ;
            }
        }
        #endregion

        #region import trades from specified file
        public static void ImportTrades(string fileName, ClearingHouse clearingHouse, ref int? numberTradesInserted, ref int? numberTradesRejected)
        {
            numberTradesInserted = 0;
            numberTradesRejected = 0;

            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportTradesMethod != null)
                {
                    ImportTradesFromMethod(fileName, clearingHouseInfo, ref numberTradesInserted, ref numberTradesRejected);
                }
                else
                {
                    throw new ReconciliationImportException("Importing trades not supported for clearing house " + clearingHouse.ToString());
                }
            }
            catch (Exception e)
            {
                Error("Error importing trades from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} trades from {1}, rejected {2} trades", numberTradesInserted.Value, fileName, numberTradesRejected.Value));
            }
         }
        private static void ImportTradesFromMethod(string fileName, IClearingHouse clearingHouseInfo, ref int? numberTradesInserted, ref int? numberTradesRejected)
        {
            try
            {
                string xml;
                numberTradesInserted = 0;
                numberTradesRejected = 0;

                // returns number of records read
                int rc = ConvertFileToXml(fileName, "Trades", out xml);
                if (rc > 0)
                {
                    rc = clearingHouseInfo.ImportTradesMethod(xml, ref numberTradesInserted, ref numberTradesRejected, ImportDate, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} from import trades method", rc));
                }
            }
            catch (Exception e)
            {
                Error("Error importing trades", e);
            }
            finally
            {
                clearingHouseInfo.LogImportTradesMethod(); ;
            }
        }
        #endregion

        #region import confirmations from the specified file
        public static int ImportConfirmations(string fileName, ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportConfirmationsMethod != null)
                {
                    // new methodology
                    count = ImportConfirmationsFromMethod(fileName, clearingHouseInfo);
                }
                else
                {
                    // legacy methodology
                    count = ImportConfirmationsFromRecordCollection(fileName, clearingHouseInfo);
                }
            }
            catch (Exception e)
            {
                Error("Error importing confirmations from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} confirmation records from {1}", count, fileName));
            }

            stockPositionsTableFilled = false;  // price discrepancies need refreshing
            optionPositionsTableFilled = false; // price discrepancies need refreshing
            futuresPositionsTableFilled = false; // price discrepancies need refreshing
            merrillOptionTradesTableFilled = false;
            merrillStockTradesTableFilled = false;
            FireOnTablesUpdated();

            return count;
        }

        private static int ImportConfirmationsFromMethod(string fileName, IClearingHouse clearingHouseInfo)
        {
            try
            {
                string xml;
                int? numberTradesInserted = 0;

                // returns number of records read
                int rc = ConvertFileToXml(fileName, "Confirmations", out xml);
                if (rc > 0)
                {
                    rc = clearingHouseInfo.ImportConfirmationsMethod(xml, ImportDate, ref numberTradesInserted, false, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} import confirmations method", rc));
                }
                return numberTradesInserted.Value;
            }
            catch (Exception e)
            {
                Error("Error importing confirmations", e);
                return 0;
            }
            finally
            {
                clearingHouseInfo.LogImportConfirmationsMethod();
            }
        }

        private static int ImportConfirmationsFromRecordCollection(string fileName, IClearingHouse clearingHouseInfo)
        {
            int count = 0;
            using (IConfirmationRecordCollection confirmationRecords = clearingHouseInfo.GetConfirmationRecordCollection(fileName))
            {
                ConfirmationRecord confirmationRecord = confirmationRecords.NextRecord;

                using (IConfirmationTableAdapter confirmationTableAdapter = clearingHouseInfo.ConfirmationTableAdapter)
                {
                    confirmationTableAdapter.SetAllCommandTimeouts(0);
                    while (confirmationRecord != null)
                    {
                        InsertConfirmationRecord(confirmationTableAdapter, ref count, confirmationRecords, ref confirmationRecord);
                    }
                }
            }
            return count;
        }
        #endregion

        #region import taxlots from the specified file
        public static int ImportTaxlots(string fileName, ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                if (clearingHouseInfo.ImportTaxlotsMethod != null)
                {
                    // new methodology
                    count = ImportTaxlotsFromMethod(fileName, clearingHouseInfo);
                }
                else
                {
                    // legacy methodology
                    count = ImportTaxlotsFromRecordCollection(fileName, clearingHouseInfo);
                }
            }
            catch (Exception e)
            {
                Error("Error importing taxlots from " + fileName, e);
            }

            finally
            {
                Info(String.Format("Inserted {0} taxlots from {1}", count, fileName));
            }

            stockPositionsTableFilled = false; 
            optionPositionsTableFilled = false; 
            FireOnTablesUpdated();

            return count;
        }

        private static int ImportTaxlotsFromMethod(string fileName, IClearingHouse clearingHouseInfo)
        {
            try
            {
                string xml;

                int  numberTradesInserted = ConvertFileToXml(fileName, "Taxlots", out xml);
                if (numberTradesInserted > 0)
                {
                    int rc = clearingHouseInfo.ImportTaxlotsMethod(xml, ImportDate, false);
                    if (rc > 4)
                        Info(String.Format("Error - return code {0} import taxlots method", rc));
                }
                return numberTradesInserted;
            }
            catch (Exception e)
            {
                Error("Error importing taxlots", e);
                return 0;
            }
            finally
            {
                clearingHouseInfo.LogImportTaxlotsMethod();
            }
        }

        private static int ImportTaxlotsFromRecordCollection(string fileName, IClearingHouse clearingHouseInfo)
        {
            int count = 0;
            using (ITaxlotRecordCollection taxlotRecords = clearingHouseInfo.GetTaxlotRecordCollection(fileName))
            {
                TaxlotRecord taxlotRecord = taxlotRecords.NextRecord;

                using (ITaxlotTableAdapter taxlotTableAdapter = clearingHouseInfo.TaxlotTableAdapter)
                {
                    taxlotTableAdapter.SetAllCommandTimeouts(0);
                    while (taxlotRecord != null)
                    {
                        InsertTaxlotRecord(taxlotTableAdapter, ref count, taxlotRecords, ref taxlotRecord);
                    }
                }
            }
            return count;
        }
        #endregion
        #endregion

        #region Fill methods
        // force a refresh of position, trades, and corrections tables (should be necessary only if
        //  changes are made to Hugo from an external source - this class should set
        //  tableFilled bools as necessary when it does things that invalidate the tables)
        public static void Refresh()
        {
            stockPositionsTableFilled = false;
            optionPositionsTableFilled = false;
            futuresPositionsTableFilled = false;
            confirmationCorrectionsTodayTableFilled = false;
            confirmationCorrectionsYesterdayTableFilled = false;
            HugoOptionTradesTableFilled = false;
            hugoStockTradesTableFilled = false;
            hugoFuturesTradesTableFilled = false;
            merrillOptionTradesTableFilled = false;
            merrillStockTradesTableFilled = false;
            merrillFuturesTradesTableFilled = false;
            symbolMappingsTableFilled = false;
            hugoStockCorrectionsTableFilled = false;
            hugoOptionCorrectionsTableFilled = false;
            hugoFuturesCorrectionsTableFilled = false;
            clearingHouseFileNamesTableFilled = false;
            accountGroupInfo = null;
            FireOnTablesUpdated();
        }

        public static string GetAccountName(int account)
        {
            foreach (HugoDataSet.TraderAccountsRow row in Utilities.TraderAccounts)
            {
                if (row.AccountID == account)
                    return row.AcctName;
            }
            return String.Empty;
        }

        public static int GetAccountIdFromSubaccountName(string subAcctName)
        {
            short? acctId = null;
            short? subAcctId = null;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                queriesAdapter.GetSubaccountId(subAcctName, ref acctId, ref subAcctId);
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting account id for subaccount {0}", subAcctName), e);
            }
            finally
            {
                queriesAdapter.LogCommand("dbo.get_subacctid");
            }
            return acctId.Value;
        }

        public static HugoDataSet.HugoStockTradesDataTable GetHugoStockTrades(IUnderlying underlying, bool groupByPrice)
        {
            int count = 0;
            string stockSymbol = (underlying == null) ? null : underlying.UnderlyingSymbol;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                count = hugoStockTradesAdapter.Fill(hugoDataSet.HugoStockTrades, stockSymbol,
                               AccountGroupName, PreviousDate, ImportDate, groupByPrice);
                hugoStockTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} hugo stock trades for {1} from {2} to {3}",
                    (stockSymbol == null) ? "all" : stockSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoStockTradesAdapter.LogCommand("Reconciliation.p_get_stock_trades3");
                Info(String.Format("Retrieved {0} {1} hugo stock trades for {2} from {3} to {4}",
                    count, (stockSymbol == null) ? "all" : stockSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }

            return hugoDataSet.HugoStockTrades;
        }

        public static HugoDataSet.MerrillStockTradesDataTable GetMerrillStockTrades(IUnderlying underlying, bool groupByPrice)
        {
            int count = 0;
            string stockSymbol = (underlying == null) ? null : underlying.UnderlyingSymbol;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                // search is by importdate, not trade date -- so pass ImportDate as the max and min dates
                count = merrillStockTradesAdapter.Fill(hugoDataSet.MerrillStockTrades, stockSymbol,
                               AccountGroupName, PreviousDate, ImportDate, groupByPrice);
                merrillStockTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} merrill stock trades for {1} from {2} to {3}",
                    (stockSymbol == null) ? "all" : stockSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                merrillStockTradesAdapter.LogCommand("Reconciliation.p_get_stock_trade_confirmations3");
                Info(String.Format("Retrieved {0} {1} merrill stock trades for {2} from {3} to {4}",
                    count, (stockSymbol == null) ? "all" : stockSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }

            return hugoDataSet.MerrillStockTrades;
        }
         public static HugoDataSet.HugoOptionTradesDataTable GetHugoOptionTrades(IUnderlying underlying, IOption option, bool consolidate)
        {
            // verify parameters and build string for message below
            CheckOptionParameters(underlying, option, out saveUnderlyingSymbol, out saveOptionSymbol, out saveExpirationDate,
                out saveStrikePrice, out saveOptionType, out saveRequestedOptions);
            unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;

            // return the desired table (consolidated or unconsolidated)
            return consolidate ? ConsolidatedHugoOptionTrades  : UnconsolidatedHugoOptionTrades;
        }

        public static HugoDataSet.MerrillOptionTradesDataTable GetMerrillOptionTrades(IUnderlying underlying, IOption option)
        {
            int count = 0;
            string underlyingSymbol;
            string optionSymbol;
            DateTime? expirationDate;
            decimal? strikePrice;
            string optionType;
            string requestedOptions;

            // verify parameters and build string for message below
            CheckOptionParameters(underlying, option, out underlyingSymbol, out optionSymbol, out expirationDate,
                out strikePrice, out optionType, out requestedOptions);

            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                // search is by importdate, not trade date -- so pass ImportDate as the max and min dates
                count = merrillOptionTradesAdapter.Fill(hugoDataSet.MerrillOptionTrades, underlyingSymbol,
                                optionSymbol, expirationDate, strikePrice, optionType,
                               AccountGroupName, PreviousDate, ImportDate);
                merrillOptionTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} merrill option trades for {1} from {2} to {3}", requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                merrillOptionTradesAdapter.LogCommand("Reconciliation.p_get_option_trade_confirmations3");
                Info(String.Format("Retrieved {0} {1} merrill option trades for {2} from {3} to {4}", count, requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }

            return hugoDataSet.MerrillOptionTrades;
        }
        public static HugoDataSet.HugoFuturesTradesDataTable GetHugoFuturesTrades(IUnderlying underlying, bool groupByPrice)
        {
            int count = 0;
            string FuturesSymbol = (underlying == null) ? null : underlying.UnderlyingSymbol;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                count = hugoFuturesTradesAdapter.Fill(hugoDataSet.HugoFuturesTrades, FuturesSymbol,
                               AccountGroupName, PreviousDate, ImportDate, groupByPrice);
                hugoFuturesTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} hugo futures trades for {1} from {2} to {3}",
                    (FuturesSymbol == null) ? "all" : FuturesSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoFuturesTradesAdapter.LogCommand("Reconciliation.p_get_futures_trades");
                Info(String.Format("Retrieved {0} {1} hugo futures trades for {2} from {3} to {4}",
                    count, (FuturesSymbol == null) ? "all" : FuturesSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }

            return hugoDataSet.HugoFuturesTrades;
        }
        public static HugoDataSet.MerrillFuturesTradesDataTable GetMerrillFuturesTrades(IUnderlying underlying, bool groupByPrice)
        {
            int count = 0;
            string FuturesSymbol = (underlying == null) ? null : underlying.UnderlyingSymbol;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                // search is by importdate, not trade date -- so pass ImportDate as the max and min dates
                count = merrillFuturesTradesAdapter.Fill(hugoDataSet.MerrillFuturesTrades, FuturesSymbol,
                               AccountGroupName, PreviousDate, ImportDate, groupByPrice);
                merrillFuturesTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} merrill futures trades for {1} from {2} to {3}",
                    (FuturesSymbol == null) ? "all" : FuturesSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                merrillFuturesTradesAdapter.LogCommand("Reconciliation.p_get_futures_trade_confirmations");
                Info(String.Format("Retrieved {0} {1} merrill futures trades for {2} from {3} to {4}",
                    count, (FuturesSymbol == null) ? "all" : FuturesSymbol, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }

            return hugoDataSet.MerrillFuturesTrades;
        }
        #endregion

        #region Delete methods
        public static int DeleteTodaysPositions(ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);

                using (IPositionTableAdapter positionTableAdapter = clearingHouseInfo.PositionTableAdapter)
                {
                    positionTableAdapter.SetAllCommandTimeouts(0);
                    count = positionTableAdapter.DeleteRecords(ImportDate, AccountGroupName);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting todays positions", e);
            }

            return count;
        }

        public static int DeleteTodaysConfirmations(ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);

                using (IConfirmationTableAdapter confirmationTableAdapter = clearingHouseInfo.ConfirmationTableAdapter)
                {
                    confirmationTableAdapter.SetAllCommandTimeouts(0);
                    count = confirmationTableAdapter.DeleteRecords(ImportDate, AccountGroupName);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting todays confirmations", e);
            }
            return count;
        }

        public static int DeleteTodaysTaxlots(ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);

                using (ITaxlotTableAdapter taxlotTableAdapter = clearingHouseInfo.TaxlotTableAdapter)
                {
                    taxlotTableAdapter.SetAllCommandTimeouts(0);
                    count = taxlotTableAdapter.DeleteRecords(ImportDate, AccountGroupName);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting todays taxlots", e);
            }
            return count;
        }

        public static int DeleteTodaysBookkeeping(ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);

                using (IBookkeepingTableAdapter BookkeepingTableAdapter = clearingHouseInfo.BookkeepingTableAdapter)
                {
                    BookkeepingTableAdapter.SetAllCommandTimeouts(0);
                    count = BookkeepingTableAdapter.DeleteRecords(ImportDate, AccountGroupName);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting todays bookkeeping entries", e);
            }
            return count;
        }

        public static int DeleteTodaysDividends(ClearingHouse clearingHouse)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);

                using (IDividendTableAdapter DividendTableAdapter = clearingHouseInfo.DividendTableAdapter)
                {
                    DividendTableAdapter.SetAllCommandTimeouts(0);
                    count = DividendTableAdapter.DeleteRecords(ImportDate, AccountGroupName);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting todays dividends", e);
            }
            return count;
        }

        public static int DeleteStockTrade(IStockTrade trade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoStockTradesAdapter.DeleteRecord(AccountGroupName,
                    ImportDate,
                    trade.TradeId);

                rc = hugoStockTradesAdapter.GetReturnValue("Reconciliation.p_delete_stocktrade_from_reconciliation_client");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoStockCorrectionsTableFilled = false;
                    FireOnTablesUpdated();
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting trade: " + trade.ToString(), e);
            }
            finally
            {
                hugoStockTradesAdapter.LogCommand("Reconciliation.p_delete_stocktrade_from_reconciliation_client");
            }

            return rc;
        }

        public static int DeleteFuturesTrade(IFuturesTrade trade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoFuturesTradesAdapter.DeleteRecord(AccountGroupName,
                    ImportDate,
                    trade.TradeId);

                rc = hugoFuturesTradesAdapter.GetReturnValue("Reconciliation.p_delete_futurestrade_from_reconciliation_client");
                if (rc == 0)
                {
                    futuresPositionsTableFilled = false;
                    hugoFuturesTradesTableFilled = false;
                    hugoFuturesCorrectionsTableFilled = false;
                    FireOnTablesUpdated();
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting trade: " + trade.ToString(), e);
            }
            finally
            {
                hugoFuturesTradesAdapter.LogCommand("Reconciliation.p_delete_futurestrade_from_reconciliation_client");
            }

            return rc;
        }
        public static int DeleteOptionTrade(IOptionTrade trade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoOptionTradesAdapter.DeleteRecord(AccountGroupName,
                    ImportDate,
                    trade.TradeId);

                rc = hugoOptionTradesAdapter.GetReturnValue("Reconciliation.p_delete_optiontrade_from_reconciliation_client");
                if (rc == 0)
                {
                    optionPositionsTableFilled = false;
                    HugoOptionTradesTableFilled = false;
                    hugoOptionCorrectionsTableFilled = false;
                    if (IsExerciseAssignment(trade.TradeMedium))
                    {
                        stockPositionsTableFilled = false;
                        hugoStockTradesTableFilled = false;
                    }
                    FireOnTablesUpdated();
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error deleting trade: " + trade.ToString(), e);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_delete_optiontrade_from_reconciliation_client");
            }

            return rc;
        }

        public static int DeleteConfirmation(int recordId, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                queriesAdapter.DeleteTradeConfirmation(ClearingHouse.ToString(), recordId, note);

                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                merrillOptionTradesTableFilled = false;
                merrillStockTradesTableFilled = false;
                merrillFuturesTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error deleting trade confirmation, recordId={0}: ", recordId), e);
                return 1;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_delete_trade_confirmation2");
            }
        }

        public static int DeleteSymbolMapping(int mappingId)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                symbolMappingsAdapter.SetCommandTimeout("Reconciliation.p_delete_gargoyle_merrill_symbol_mappings", 0);
                symbolMappingsAdapter.DeleteRecord(mappingId, ImportDate);

                stockPositionsTableFilled = false;
                symbolMappingsTableFilled = false;
                FireOnTablesUpdated();
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error deleting symbol mapping id {0}", mappingId), e);
                return 1;
            }
            finally
            {
                symbolMappingsAdapter.LogCommand("Reconciliation.p_delete_gargoyle_merrill_symbol_mappings");
            }
        }
        #endregion

        #region Add methods
        // returns 0 if expiration is added
        // returns 1 if it didn't add it (correctly) because an expiration trade already exists for this option
        // returns other if it didn't add it because of an error
        public static int AddOptionExpiration(IOptionTrade optionTrade)
        {
            int? tradeId = null;
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoOptionTradesAdapter.InsertExpiration(ref tradeId,
                    AccountGroupName,
                    ImportDate,
                    optionTrade.TradeType,
                    optionTrade.TradeVolume,
                    optionTrade.OptionSymbol,
                    optionTrade.ExpirationDate,
                    optionTrade.StrikePrice,
                    optionTrade.OptionType,
                    optionTrade.TradePrice,
                    optionTrade.SubAcctName,
                    optionTrade.TradeDate);

                rc = hugoOptionTradesAdapter.GetReturnValue("Reconciliation.p_add_option_expiration");
                switch (rc)
                {
                    case 0:
                        // adding an expiration may change stock position as a result of reducing
                        //  an assignment
                        stockPositionsTableFilled = false;
                        optionPositionsTableFilled = false;
                        unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;
                        hugoStockTradesTableFilled = false;
                        hugoOptionCorrectionsTableFilled = false;
                        FireOnTablesUpdated();

                        Info("Added option expiration: " + optionTrade.ToString());
                        break;
                    case 1:
                        Info("Did not add duplicate option expiration: " + optionTrade.ToString());
                        break;
                    case 2:
                        Info("Did not add option expiration for archived option: " + optionTrade.ToString());
                        break;
                    default:
                        throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Info("Error adding option expiration: " + optionTrade.ToString() + " - " + e.Message);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_add_option_expiration");
            }
            return rc;
        }
        // returns 0 if expiration is added
        // returns 1 if it didn't add it (correctly) because an exercise or assignment already exists for this option
        // returns other if it didn't add it because of an error
        public static int AddOptionAssignment(IOptionTrade optionTrade)
        {
            int? tradeId = null;
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoOptionTradesAdapter.InsertAssignment(ref tradeId,
                    AccountGroupName,
                    ImportDate,
                    optionTrade.TradeType,
                    optionTrade.TradeVolume,
                    optionTrade.OptionSymbol,
                    optionTrade.ExpirationDate,
                    optionTrade.StrikePrice,
                    optionTrade.OptionType,
                    optionTrade.TradePrice,
                    optionTrade.SubAcctName,
                    optionTrade.TradeDate,
                    optionTrade.TotalCost);

                rc = hugoOptionTradesAdapter.GetReturnValue("Reconciliation.p_add_option_assignment");
                switch (rc)
                {
                    case 0:
                        stockPositionsTableFilled = false;
                        optionPositionsTableFilled = false;
                        unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;
                        hugoStockTradesTableFilled = false;
                        hugoOptionCorrectionsTableFilled = false;
                        FireOnTablesUpdated();

                        Info("Added option assignment: " + optionTrade.ToString());
                        break;
                    case 1:
                        Info("Did not add duplicate option assignment: " + optionTrade.ToString());
                        break;
                    case 2:
                        Info("Did not option assignment for archived option: " + optionTrade.ToString());
                        break;
                    default:
                        throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Info("Error adding option assignment: " + optionTrade.ToString() + " - " + e.Message);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_add_option_assignment");
            }
            return rc;
        }
        public static int? AddOptionTrade(IOptionTrade optionTrade)
        {
            int? tradeId = null;
             try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoOptionTradesAdapter.SetCommandTimeout("Reconciliation.p_add_optiontrade_from_reconciliation_client2", 0);
                    hugoOptionTradesAdapter.InsertRecord(ref tradeId,
                    AccountGroupName,
                    ImportDate,
                    optionTrade.TradeType,
                    optionTrade.TradeVolume,
                    optionTrade.OptionSymbol,
                    optionTrade.ExpirationDate,
                    optionTrade.StrikePrice,
                    optionTrade.OptionType,
                    optionTrade.TradePrice,
                    optionTrade.SubAcctName,
                    optionTrade.TraderName,
                    optionTrade.BrokerName,
                    optionTrade.ExchangeName,
                    optionTrade.TradeNote,
                    optionTrade.TradeMedium,
                    optionTrade.TradeDate,
                    optionTrade.TradeReason,
                    optionTrade.Commission,
                    optionTrade.SECFee,
                    optionTrade.ORFFee,
                    optionTrade.TotalCost);

                int rc = hugoOptionTradesAdapter.GetReturnValue("Reconciliation.p_add_optiontrade_from_reconciliation_client2");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    optionPositionsTableFilled = false;
                    HugoOptionTradesTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoOptionCorrectionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Added trade: " + optionTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
             }
            catch (Exception e)
            {
                Error("Error adding trade: " + optionTrade.ToString(), e);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_add_optiontrade_from_reconciliation_client2");
            }
            return tradeId;
        }

        public static int? AddStockTrade(IStockTrade stockTrade)
        {
            int? tradeId = null;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoStockTradesAdapter.SetCommandTimeout("Reconciliation.p_add_stocktrade_from_reconciliation_client2", 0);
                hugoStockTradesAdapter.InsertRecord(ref tradeId,
                    AccountGroupName,
                    ImportDate,
                    stockTrade.TradeType,
                    stockTrade.TradeVolume,
                    stockTrade.FractionalRemainder,
                    stockTrade.UnderlyingSymbol,
                    stockTrade.TradePrice,
                    stockTrade.SubAcctName,
                    stockTrade.TradeMedium,
                    stockTrade.TradeReason,
                    stockTrade.BrokerName,
                    stockTrade.ExchangeName,
                    stockTrade.TradeNote,
                    stockTrade.TraderName,
                    stockTrade.TradeDate,
                    stockTrade.Commission,
                    stockTrade.SECFee,
                    stockTrade.TotalCost);

                int rc = hugoStockTradesAdapter.GetReturnValue("Reconciliation.p_add_stocktrade_from_reconciliation_client2");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoStockCorrectionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Added trade: " + stockTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error adding trade: " + stockTrade.ToString(), e);
            }
            finally
            {
                hugoStockTradesAdapter.LogCommand("Reconciliation.p_add_stocktrade_from_reconciliation_client2");
            }
            return tradeId;
        }
        public static int? AddFuturesTrade(IFuturesTrade futuresTrade)
        {
            int? tradeId = null;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoFuturesTradesAdapter.SetCommandTimeout("Reconciliation.p_add_futurestrade_from_reconciliation_client", 0);
                hugoFuturesTradesAdapter.InsertRecord(ref tradeId,
                    AccountGroupName,
                    ImportDate,
                    futuresTrade.TradeType,
                    futuresTrade.TradeVolume,
                    futuresTrade.UnderlyingSymbol,
                    futuresTrade.TradePrice,
                    futuresTrade.SubAcctName,
                    futuresTrade.TradeMedium,
                    futuresTrade.TradeReason,
                    futuresTrade.BrokerName,
                    futuresTrade.ExchangeName,
                    futuresTrade.TradeNote,
                    futuresTrade.TraderName,
                    futuresTrade.BillingTypeDescr,
                    futuresTrade.SpecialRate,
                    futuresTrade.TraderId,
                    futuresTrade.TradeDate,
                    futuresTrade.ShortFlag,
                    futuresTrade.TotalCost);

                int rc = hugoFuturesTradesAdapter.GetReturnValue("Reconciliation.p_add_futurestrade_from_reconciliation_client");
                if (rc == 0)
                {
                    futuresPositionsTableFilled = false;
                    hugoFuturesTradesTableFilled = false;
                    hugoFuturesCorrectionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Added trade: " + futuresTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error adding trade: " + futuresTrade.ToString(), e);
            }
            finally
            {
                hugoFuturesTradesAdapter.LogCommand("Reconciliation.p_add_futurestrade_from_reconciliation_client");
            }
            return tradeId;
        }

        // returns 0 if it worked
        public static int AddStockTradeConfirmation(IStockTrade stockTrade, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);


                merrillStockTradesAdapter.InsertRecord(ClearingHouse.ToString(),
                    ImportDate,
                    stockTrade.UnderlyingSymbol,
                    stockTrade.SubAcctName,
                    GetTransactionCode(stockTrade.TradeType, stockTrade.ShortFlag),
                    stockTrade.TradeVolume,
                    Convert.ToDouble(stockTrade.TradePrice),
                    stockTrade.TradeDate,
                    null,
                    stockTrade.BrokerName,
                    stockTrade.ExchangeName,
                    note,
                    stockTrade.TradeMedium,
                    stockTrade.TotalCost,
                    stockTrade.Commission,
                    stockTrade.SECFee,
                    "Trade");

                stockPositionsTableFilled = false;
                merrillStockTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info("Added trade confirmation: " + stockTrade.ToString());
                return 0;
            }
            catch (Exception e)
            {
                Error("Error adding trade confirmation: " + stockTrade.ToString(), e);
                return 1;
            }

            finally
            {
                merrillStockTradesAdapter.LogCommand("Reconciliation.p_add_stock_trade_confirmation2");
            }
        }
        public static int AddFuturesTradeConfirmation(IFuturesTrade futuresTrade, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);


                merrillFuturesTradesAdapter.InsertRecord(ClearingHouse.ToString(),
                    ImportDate,
                    futuresTrade.UnderlyingSymbol,
                    futuresTrade.SubAcctName,
                    GetTransactionCode(futuresTrade.TradeType, futuresTrade.ShortFlag),
                    futuresTrade.TradeVolume,
                    Convert.ToDouble(futuresTrade.TradePrice),
                    futuresTrade.TradeDate,
                    null,
                    futuresTrade.BrokerName,
                    futuresTrade.ExchangeName,
                    note,
                    futuresTrade.TradeMedium,
                    futuresTrade.TotalCost,
                    futuresTrade.Commission,
                    futuresTrade.NFAFee,
                    futuresTrade.ExchangeFee,
                    futuresTrade.ClearingFee);

                futuresPositionsTableFilled = false;
                merrillFuturesTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info("Added trade confirmation: " + futuresTrade.ToString());
                return merrillFuturesTradesAdapter.GetReturnValue("Reconciliation.p_add_futures_trade_confirmation");
            }
            catch (Exception e)
            {
                Error("Error adding trade confirmation: " + futuresTrade.ToString(), e);
                return 1;
            }

            finally
            {
                merrillFuturesTradesAdapter.LogCommand("Reconciliation.p_add_futures_trade_confirmation");
            }
        }

        // returns 0 if it worked
        public static int AddOptionTradeConfirmation(IOptionTrade optionTrade, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                merrillOptionTradesAdapter.InsertRecord(ClearingHouse.ToString(),
                    ImportDate,
                    optionTrade.UnderlyingSymbol,
                    optionTrade.OptionSymbol,
                    optionTrade.ExpirationDate,
                    optionTrade.StrikePrice,
                    optionTrade.OptionType,
                    optionTrade.SubAcctName,
                    GetTransactionCode(optionTrade.TradeType, false),
                    optionTrade.TradeVolume,
                    Convert.ToDouble(optionTrade.TradePrice),
                    optionTrade.TradeDate,
                    optionTrade.BrokerName,
                    optionTrade.ExchangeName,
                    note,
                    optionTrade.TradeMedium,
                    optionTrade.TotalCost,
                    optionTrade.Commission,
                    optionTrade.SECFee,
                    optionTrade.ORFFee);

                optionPositionsTableFilled = false;
                merrillOptionTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();
                return 0;
            }
            catch (Exception e)
            {
                Error("Error adding trade confirmation: " + optionTrade.ToString(), e);
                return 1;
            }
            finally
            {
                merrillOptionTradesAdapter.LogCommand("Reconciliation.p_add_option_trade_confirmation2");
            }
        }

        public static int AddFlex(IOption option, short? acctId)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                //          Don't do this - option.ExpirationDate is nominal, which is what we want
    //          DateTime lastTradingDay = option.ExpirationDate.Subtract(new TimeSpan(1 /*day*/, 0, 0, 0));
                queriesAdapter.AddFlex(option.UnderlyingSymbol,
                    option.OptionSymbol,
                    option.ExpirationDate,
                    option.StrikePrice,
                    option.OptionSymbol[0] == '2',
                    acctId);

                rc = queriesAdapter.GetReturnValue("dbo.add_flex");
                if (rc != 0)
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error adding flex: " + option.ToString(), e);
            }
            finally
            {
                queriesAdapter.LogCommand("dbo.add_flex");
            }
            return rc;
        }

        public static void SyncTotalCosts()
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                queriesAdapter.SyncTotalCosts(ImportDate, PreviousDate, ClearingHouse.ToString(), null, null);

                optionPositionsTableFilled = false;
                HugoOptionTradesTableFilled = false;
                stockPositionsTableFilled = false;
                hugoStockTradesTableFilled = false;
                hugoFuturesTradesTableFilled = false;
                futuresPositionsTableFilled = false;


                FireOnTablesUpdated();
            }
            catch (Exception e)
            {
                Error("Error syncing total costs for " + clearingHouse.ToString(), e);
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_sync_total_costs");
            }
        }
        public static void SyncIndexExercisesAndAssignments()
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                queriesAdapter.SyncPricesForIndexExercisesAndAssignments(AccountGroupName, ClearingHouse.ToString(), ImportDate, PreviousDate);
            }
            catch (Exception e)
            {
                Error("Error syncing index exercises and assignments  for " + AccountGroupName, e);
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_sync_prices_for_index_exercises_and_assignments");
            }
        }

        public static int AddSymbolMapping(string merrillSymbol, string gargoyleSymbol)
        {
            try
            {
                if (String.IsNullOrEmpty(merrillSymbol))
                    throw new ArgumentNullException("merrillSymbol");
                if (String.IsNullOrEmpty(gargoyleSymbol))
                    throw new ArgumentNullException("gargoyleSymbol");

                CheckParameters(/*needAccountGroup =*/ true);
                symbolMappingsAdapter.SetCommandTimeout("Reconciliation.p_add_gargoyle_merrill_symbol_mappings2", 0);
                symbolMappingsAdapter.InsertRecord(ClearingHouse.ToString(), merrillSymbol, gargoyleSymbol, ImportDate);

                stockPositionsTableFilled = false;
                symbolMappingsTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Added symbol mapping from {0} to {1}", merrillSymbol, gargoyleSymbol));
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error adding symbol mapping from {0} to {1}", merrillSymbol, gargoyleSymbol), e);
                return 1;
            }
            finally
            {
                symbolMappingsAdapter.LogCommand("Reconciliation.p_add_gargoyle_merrill_symbol_mappings2");
            }
        }

        public static int MatchOptionPositions(DateTime importDate, int recordId, int? hugoOptionId, short? hugoUnderlyingId)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                optionPositionsAdapter.UpdateOptionId(ClearingHouse.ToString(), importDate, recordId, hugoOptionId, hugoUnderlyingId);
                optionPositionsTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Updated optionid for record {0}", recordId));
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error updating optionid for record {0}", recordId), e);
                return 1;
            }
            finally
            {
                optionPositionsAdapter.LogCommand("Reconciliation.p_update_optionid_for_position");
            }
        }

        // returns 0 if it worked
        public static int RedoCorrection(int recordId)
        {
            int rc = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                int? addedRecordId = null;
                confirmationCorrectionsAdapter.RedoCorrection(ClearingHouse.ToString(),
                    recordId,
                    ImportDate,
                    ref addedRecordId);

                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                merrillStockTradesTableFilled = false;
                merrillOptionTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Redid trade correction recordId={0} ", recordId));
            }
            catch (Exception e)
            {
                Error(String.Format("Error redoing  correction recordId={0} ", recordId), e);
                rc = 1;
            }
            finally
            {
                confirmationCorrectionsAdapter.LogCommand("Reconciliation.p_redo_trade_confirmation2");
            }
            return rc;
        }

        // returns 0 if it worked
        public static int RedoCorrectionToDate(int recordId, DateTime toDate)
        {
            int rc = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                confirmationCorrectionsAdapter.RedoCorrectionToDate(ClearingHouse.ToString(),
                    recordId,
                    toDate);

                stockPositionsTableFilled = false;
                optionPositionsTableFilled = false;
                futuresPositionsTableFilled = false;
                merrillStockTradesTableFilled = false;
                merrillOptionTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Redid trade correction recordId={0} to date {1:d}", recordId, toDate));
            }
            catch (Exception e)
            {
                Error(String.Format("Error redoing  correction recordId={0} to date {1:d} ", recordId, toDate), e);
                rc = 1;
            }
            finally
            {
                confirmationCorrectionsAdapter.LogCommand("Reconciliation.p_redo_trade_confirmation_to_date");
            }
            return rc;
        }
        #endregion

        #region Edit methods
        public static int EditOptionTrade(EditedOptionTrade optionTrade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoOptionTradesAdapter.SetCommandTimeout("Reconciliation.p_update_optiontrade_from_reconciliation_client2", 0);
                hugoOptionTradesAdapter.UpdateRecord(optionTrade.TradeId,
                    ImportDate,
                    AccountGroupName,
                    optionTrade.OptionSymbol,
                    optionTrade.NominalExpirationDate,
                    optionTrade.Strike,
                    optionTrade.OptionType,

                    optionTrade.TradeType,
                    optionTrade.Volume,
                    optionTrade.Price,
                    optionTrade.SubAcctName,
                    optionTrade.TradeDate,
                    optionTrade.TradeNote,
                    optionTrade.Trader,
                    optionTrade.Broker,
                    optionTrade.Exchange,
                    optionTrade.Medium,
                    optionTrade.Reason,
                    optionTrade.Commission,
                    optionTrade.SECFee,
                    optionTrade.ORFFee,
                    optionTrade.TotalCost);

                rc = hugoOptionTradesAdapter.GetReturnValue("Reconciliation.p_update_optiontrade_from_reconciliation_client2");
                if (rc == 0)
                {
                    optionPositionsTableFilled = false;
                    HugoOptionTradesTableFilled = false;

                    // in case we are changing from or to 'Exercise/Assign'
                    stockPositionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoOptionCorrectionsTableFilled = false;

                    FireOnTablesUpdated();

                    Info("Edited trade: " + optionTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error editing trade: " + optionTrade.ToString(), e);
                rc = -1;
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_update_optiontrade_from_reconciliation_client2");
            }
            return rc;
        }

        public static int EditStockTrade(EditedStockTrade stockTrade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoStockTradesAdapter.SetCommandTimeout("Reconciliation.p_update_stocktrade_from_reconciliation_client3", 0);
                hugoStockTradesAdapter.UpdateRecord(stockTrade.TradeId,
                        stockTrade.TaxlotId,
                        AccountGroupName,
                        ImportDate,
                        stockTrade.Symbol,
  
                        stockTrade.TradeType,
                        stockTrade.Volume,
                        stockTrade.FractionalRemainder,
                        stockTrade.Price,
                        stockTrade.SubAcctName,
                        stockTrade.TradeDate,
                        stockTrade.TradeNote,
                        stockTrade.Trader,
                        stockTrade.Broker,
                        stockTrade.Exchange,
                        stockTrade.Medium,
                        stockTrade.Reason,
                        stockTrade.Commission,
                        stockTrade.SECFee,
                        stockTrade.TotalCost);


                rc = hugoStockTradesAdapter.GetReturnValue("Reconciliation.p_update_stocktrade_from_reconciliation_client3");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoStockCorrectionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Edited trade: " + stockTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error editing trade: " + stockTrade.ToString(), e);
            }
            finally
            {
                hugoStockTradesAdapter.LogCommand("Reconciliation.p_update_stocktrade_from_reconciliation_client3");
            }
            return rc;
        }
        public static int EditFuturesTrade(EditedFuturesTrade futuresTrade)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                hugoFuturesTradesAdapter.UpdateRecord(AccountGroupName,
                    ImportDate,
                    futuresTrade.TradeId,
                    futuresTrade.TradeType,
                    futuresTrade.Volume,
                    futuresTrade.Symbol,
                    futuresTrade.Price,
                    futuresTrade.SubAcctName,
                    futuresTrade.Medium,
                    futuresTrade.Reason,
                    futuresTrade.Broker,
                    futuresTrade.Exchange,
                    futuresTrade.TradeNote,
                    futuresTrade.Trader,
                    futuresTrade.TradeDate,
                    futuresTrade.TotalCost,
                    futuresTrade.Commission,
                    futuresTrade.NFAFee,
                    futuresTrade.ExchangeFee,
                    futuresTrade.ClearingFee);

                rc = hugoFuturesTradesAdapter.GetReturnValue("Reconciliation.p_update_futurestrade_from_reconciliation_client");
                if (rc == 0)
                {
                    futuresPositionsTableFilled = false;
                    hugoFuturesTradesTableFilled = false;
                    hugoFuturesCorrectionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Edited trade: " + futuresTrade.ToString());
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error("Error editing trade: " + futuresTrade.ToString(), e);
            }
            finally
            {
                hugoFuturesTradesAdapter.LogCommand("Reconciliation.p_update_futurestrade_from_reconciliation_client");
            }
            return rc;
        }
        public static int EditStockTradeConfirmation(IStockTrade stockTrade, string note)
        {
            int rc = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                merrillStockTradesAdapter.UpdateRecord(ClearingHouse.ToString(),
                    stockTrade.TradeId,
                    stockTrade.UnderlyingSymbol,
                    stockTrade.SubAcctName,
                    GetTransactionCode(stockTrade.TradeType, stockTrade.ShortFlag),
                    stockTrade.TradeVolume,
                    Convert.ToDouble(stockTrade.TradePrice),
                    note,
                    stockTrade.TotalCost,
                    stockTrade.TradeMedium,
                    stockTrade.Commission,
                    stockTrade.SECFee);

                stockPositionsTableFilled = false;
                merrillStockTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info("Edited trade confirmation: " + stockTrade.ToString());
            }
            catch (Exception e)
            {
                Error("Error editing trade confirmation: " + stockTrade.ToString(), e);
                rc = 1;
            }
            finally
            {
                merrillStockTradesAdapter.LogCommand("Reconciliation.p_update_stock_trade_confirmation2");
            }
            return rc;
        }
        public static int EditFuturesTradeConfirmation(IFuturesTrade futuresTrade, string note)
        {
            int rc = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                merrillFuturesTradesAdapter.UpdateRecord(ClearingHouse.ToString(),
                    futuresTrade.TradeId,
                    futuresTrade.UnderlyingSymbol,
                    futuresTrade.SubAcctName,
                    GetTransactionCode(futuresTrade.TradeType, futuresTrade.ShortFlag),
                    futuresTrade.TradeVolume,
                    Convert.ToDouble(futuresTrade.TradePrice),
                    note,
                    futuresTrade.TotalCost,
                    futuresTrade.TradeMedium,
                    futuresTrade.Commission,
                    futuresTrade.NFAFee,
                    futuresTrade.ClearingFee,
                    futuresTrade.ExchangeFee);

                futuresPositionsTableFilled = false;
                merrillFuturesTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info("Edited trade confirmation: " + futuresTrade.ToString());
            }
            catch (Exception e)
            {
                Error("Error editing trade confirmation: " + futuresTrade.ToString(), e);
                rc = 1;
            }
            finally
            {
                merrillFuturesTradesAdapter.LogCommand("Reconciliation.p_update_futures_trade_confirmation");
            }
            return rc;
        }
        public static int EditOptionTradeConfirmation(IOptionTrade optionTrade, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                merrillOptionTradesAdapter.UpdateRecord(ClearingHouse.ToString(),
                    optionTrade.TradeId,
                    optionTrade.UnderlyingSymbol,
                    optionTrade.OptionSymbol,
                    optionTrade.ExpirationDate,
                    optionTrade.StrikePrice,
                    optionTrade.OptionType,
                    optionTrade.SubAcctName,
                    GetTransactionCode(optionTrade.TradeType, false),
                    optionTrade.TradeVolume,
                    Convert.ToDouble(optionTrade.TradePrice),
                    note,
                    optionTrade.TotalCost,
                    optionTrade.Commission,
                    optionTrade.SECFee,
                    optionTrade.ORFFee);

                optionPositionsTableFilled = false;
                merrillOptionTradesTableFilled = false;
                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();
                return 0;
            }
            catch (Exception e)
            {
                Error("Error editing trade confirmation: " + optionTrade.ToString(), e);
                return 1;
            }
            finally
            {
                merrillOptionTradesAdapter.LogCommand("Reconciliation.p_update_option_trade_confirmation2");
            }
        }
        public static int EditSymbolMapping(int mappingId, string merrillSymbol, string gargoyleSymbol)
        {
            try
            {
                if (String.IsNullOrEmpty(merrillSymbol))
                    throw new ArgumentNullException("merrillSymbol");
                if (String.IsNullOrEmpty(gargoyleSymbol))
                    throw new ArgumentNullException("gargoyleSymbol");

                CheckParameters(/*needAccountGroup =*/ false);
                symbolMappingsAdapter.UpdateRecord(mappingId, merrillSymbol, gargoyleSymbol, ImportDate);
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error updating symbol mapping id {0}: from={1} to={2}", mappingId, merrillSymbol, gargoyleSymbol), e);
                return 1;
            }
            finally
            {
                symbolMappingsAdapter.LogCommand("Reconciliation.p_update_gargoyle_merrill_symbol_mappings");
            }
        }
        public static int EditAccountMapping(string subAcctName, string merrillAcctNumber)
        {
            try
            {
                if (String.IsNullOrEmpty(subAcctName))
                    throw new ArgumentNullException("subAcctName");
                if (String.IsNullOrEmpty(merrillAcctNumber))
                    throw new ArgumentNullException("merrillAcctNumber");

                CheckParameters(/*needAccountGroup =*/ false);
                subaccountNamesAdapter.UpdateRecord(subAcctName, merrillAcctNumber, ImportDate);
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error updating saccount mapping of {0}", subAcctName), e);
                return 1;
            }
            finally
            {
                subaccountNamesAdapter.LogCommand("Reconciliation.p_update_account_mapping");
            }
        }
        public static int EditCorrectionNote(int recordId, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                confirmationCorrectionsAdapter.UpdateNote(ClearingHouse.ToString(), recordId, note);

                confirmationCorrectionsTodayTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Updated note for trade confirmation, recordId={0}", recordId));
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error updating note for trade confirmation, recordId={0}: ", recordId), e);
                return 1;
            }
            finally
            {
                confirmationCorrectionsAdapter.LogCommand("Reconciliation.p_update_confirmation_note2");
            }
        }

        public static void UpdatePositionRecords(ClearingHouse clearingHouse, DataTable table)
        {
             if (table == null)
                throw new ArgumentNullException("table");
 
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                clearingHouseInfo.PositionTableAdapter.Update(table.Select());
            }
            catch (Exception e)
            {
                Error("Error updating position records", e);
            }

            finally
            {
                Info(String.Format("Updated {0} position records", table.Rows.Count));
            }
        }

        public static void UpdateConfirmationRecords(ClearingHouse clearingHouse, DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                IClearingHouse clearingHouseInfo = ClearingHouseFactory.GetClearingHouse(clearingHouse);
                clearingHouseInfo.ConfirmationTableAdapter.Update(table.Select());
            }
            catch (Exception e)
            {
                Error("Error updating confirmation records", e);
            }

            finally
            {
                Info(String.Format("Updated {0} confirmation records", table.Rows.Count));
            }
        }

        // returns true if all changes were accepted
        public static bool AcceptSymbolMappingChanges(HugoDataSet.SymbolMappingsDataTable changes)
        {
            int adds = 0;
            int edits = 0;
            int errors = 0;

            try
            {
                if (changes == null)
                    throw new ArgumentNullException("changes");

                foreach (HugoDataSet.SymbolMappingsRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case System.Data.DataRowState.Added:
                            if (0 == AddSymbolMapping(row.MerrillSymbol, row.GargoyleSymbol))
                                adds++;
                            else
                                errors++;
                            break;

                        case System.Data.DataRowState.Modified:
                            if (0 == EditSymbolMapping(row.MappingId, row.MerrillSymbol, row.GargoyleSymbol))
                                edits++;
                            else
                                errors++;
                            break;
                    }
                }

                Info(String.Format("Updated symbol mappings table: {0} adds, {1} edits, {2} errors",
                    adds, edits, errors));
            }
            catch (Exception e)
            {
                Error("Error updating symbol mappings table", e);
                return false;
            }
            finally
            {
                // set 'Filled' to false even if changes don't work so table will be refreshed
                symbolMappingsTableFilled = false;
                FireOnTablesUpdated();
            }
            return errors == 0;
        }
        // returns true if all changes were accepted
        public static bool AcceptSubaccountNameChanges(HugoDataSet.SubaccountNamesDataTable changes)
        {
            int edits = 0;
            int errors = 0;

            try
            {
                if (changes == null)
                    throw new ArgumentNullException("changes");

                foreach (HugoDataSet.SubaccountNamesRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case System.Data.DataRowState.Modified:
                            if (0 == EditAccountMapping(row.SubAcctName, row.MerrillAcctNumber))
                                edits++;
                            else
                                errors++;
                            break;
                    }
                }

                Info(String.Format("Updated account names table: {0} edits, {1} errors",
                    edits, errors));
            }
            catch (Exception e)
            {
                Error("Error updating account names table", e);
                return false;
            }
            finally
            {
                // set 'Filled' to false even if changes don't work so table will be refreshed
                subAccountNamesTableFilled = false;

                if (edits > 0)
                {
                    stockPositionsTableFilled = false;
                    optionPositionsTableFilled = false;
                    futuresPositionsTableFilled = false;
                    merrillOptionTradesTableFilled = false;
                    merrillStockTradesTableFilled = false;
                    confirmationCorrectionsTodayTableFilled = false;
                    confirmationCorrectionsYesterdayTableFilled = false;
                     accountGroupInfo = null;
                }
                FireOnTablesUpdated();
            }
            return errors == 0;
        }
        #endregion

        #region Undo methods

        public static int UndoHugoStockCorrection(int recordId)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                hugoStockCorrectionsAdapter.Undo(recordId);

                rc = hugoStockCorrectionsAdapter.GetReturnValue("Reconciliation.p_undo_Hugo_stock_correction");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    hugoStockCorrectionsTableFilled = false;
                    FireOnTablesUpdated();
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error undoing Hugo stock correction, recordId={0}: ", recordId), e);
                rc = 1;
            }
            finally
            {
                hugoStockCorrectionsAdapter.LogCommand("Reconciliation.p_undo_Hugo_stock_correction");
            }
            return rc;
        }

        public static int UndoHugoFuturesCorrection(int recordId)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                hugoFuturesCorrectionsAdapter.Undo(recordId);

                rc = hugoFuturesCorrectionsAdapter.GetReturnValue("Reconciliation.p_undo_Hugo_futures_correction");
                if (rc == 0)
                {
                    futuresPositionsTableFilled = false;
                    hugoFuturesTradesTableFilled = false;
                    hugoFuturesCorrectionsTableFilled = false;
                    FireOnTablesUpdated();
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error undoing Hugo futures correction, recordId={0}: ", recordId), e);
                rc = 1;
            }
            finally
            {
                hugoFuturesCorrectionsAdapter.LogCommand("Reconciliation.p_undo_Hugo_futures_correction");
            }
            return rc;
        }
        public static int UndoHugoOptionCorrection(int recordId)
        {
            int rc = -1;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                hugoOptionCorrectionsAdapter.Undo(recordId);

                rc = hugoOptionCorrectionsAdapter.GetReturnValue("Reconciliation.p_undo_Hugo_option_correction");
                if (rc == 0)
                {
                    optionPositionsTableFilled = false;
                    HugoOptionTradesTableFilled = false;
                    hugoOptionCorrectionsTableFilled = false;
                    hugoStockTradesTableFilled = false;
                    FireOnTablesUpdated();

                    Info(String.Format("Undid Hugo option correction, recordId={0}", recordId));
                }
                else
                {
                    throw new ReconciliationTradeException(rc);
                }
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error undoing Hugo option correction, recordId={0}: ", recordId), e);
                return 1;
            }
            finally
            {
                hugoOptionCorrectionsAdapter.LogCommand("Reconciliation.p_undo_Hugo_option_correction");
            }
        }
        #endregion

        #region Misc methods
        public static int FixOptionIds(string clearingHouse)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                queriesAdapter.FixOptionIds(clearingHouse, ImportDate, null);
                int rc = queriesAdapter.GetReturnValue("Reconciliation.p_fix_optionids");
                if (rc != 0)
                    Info(String.Format("Warning - return code {0} from p_fix_optionids", rc));
                return rc;
            }
            catch (Exception e)
            {
                Error("Error fixing optionids", e);
                return 8;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_fix_optionids");
            }
        }

        public static ImportStatusInfo GetImportStatus()
        {
            ImportStatusInfo newImportStatusInfo = null;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                DateTime? outputLastStartTime = null;
                DateTime? outputLastEndTime = null;
                int? outputLastStatusId = null;
                string outputLastStatusName = null;
                string outputNote = null;
                queriesAdapter.GetImportStatus(ref outputLastStartTime,
                      ref outputLastEndTime,
                      ref outputLastStatusId,
                      ref outputLastStatusName,
                      ref outputNote,
                      null,
                      ClearingHouse + taskSuffix,
                      null,
                      null);

                newImportStatusInfo = new ImportStatusInfo(outputLastStartTime,
                    outputLastEndTime,
                    outputLastStatusName,
                    outputNote);
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting import status for account group {0}", AccountGroupName), e);
                return null;
            }
            finally
            {
                queriesAdapter.LogCommand("dbo.p_TaskLastStatus");
            }
            return newImportStatusInfo;
        }
        public static int ConstructTaxlots(short acctId)
        {
            int rc = 4;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                queriesAdapter.ConstructTaxlots(acctId, ImportDate);
                rc = queriesAdapter.GetReturnValue("Reconciliation.p_construct_PershingAggregatedTaxlots");
                if (rc == 0)
                {
                    stockPositionsTableFilled = false;
                    optionPositionsTableFilled = false;
                    FireOnTablesUpdated();

                    Info(String.Format("Constructed taxlots for acctId {0}", acctId));
                }
                else
                {
                    throw new ReconciliationException(String.Format("Return code {0} from p_construct_PershingAggregatedTaxlots", rc));
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error constructing taxlots for acctDd {0}", acctId), e);
                rc = 8;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_construct_PershingAggregatedTaxlots");
            }
            return rc;
        }
        public static int ConsolidateTrades(bool consolidateOptions, bool consolidateStocks, int[] tradeIds)
        {
            int rc = 4;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                string xmlTradeIds = null;
                if (tradeIds != null)
                    xmlTradeIds = BuildTradeXmlString(tradeIds);

                queriesAdapter.ConsolidateTrades(AccountGroupName, PreviousDate, ImportDate,
                    consolidateOptions, consolidateStocks, xmlTradeIds);
                rc = queriesAdapter.GetReturnValue("Reconciliation.p_consolidate_trades");
                if (rc == 0)
                {
                    unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;
                    FireOnTablesUpdated();

                    Info("Consolidated trades");
                }
                else
                {
                    throw new ReconciliationException(String.Format("Return code {0} from p_consolidate_trades", rc));
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error consolidating trades for account group {0}", AccountGroupName), e);
                rc = 8;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_consolidate_trades");
            }
            return rc;
        }
        public static int DeconsolidateTrades(int packageId)
        {
            int rc = 4;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);
                queriesAdapter.DeconsolidateTrades(packageId);
                rc = queriesAdapter.GetReturnValue("Reconciliation.p_deconsolidate_trades");
                if (rc == 0)
                {
                    unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;
                    FireOnTablesUpdated();

                    Info(String.Format("Deconsolidated trade, packageid={0}", packageId));
                }
                else
                {
                    throw new ReconciliationException(String.Format("Return code {0} from p_deconsolidate_trades", rc));
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error deconsolidating trades for packageid {0}", packageId), e);
                rc = 8;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_deconsolidate_trades");
            }
            return rc;
        }
        public static void SetAssignmentsDone()
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                queriesAdapter.SetAssignmentsDone(AccountGroupName);
                accountGroupInfo = null;   // so we will refresh state the next time client asks
            }
            catch (Exception e)
            {
                Error(String.Format("Error setting assignments done for account group {0}", AccountGroupName), e);
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_set_assignments_done");
            }
        }
        public static void FinalizeImport()
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                queriesAdapter.SetCommandTimeout("Reconciliation.p_end_import_for_account_group", 0);
                queriesAdapter.EndImportForAccountGroup(AccountGroupName);
                accountGroupInfo = null;   // so we will refresh state the next time client asks
            }
            catch (Exception e)
            {
                Error(String.Format("Error finalizing import for account group {0}", AccountGroupName), e);
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_end_import_for_account_group");
            }
        }
        public static int AcceptStockDiscrepancy(string stockSymbol, string subAccount, int adjustment, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                stockPositionsAdapter.AcceptDiscrepancy(ImportDate,
                    stockSymbol,
                    subAccount,
                    adjustment,
                    note);

                stockPositionsTableFilled = false;
                FireOnTablesUpdated();

                Info(String.Format("Accepted discrepancy of {0} for {1} in subaccount {2}, reason={3}",
                    adjustment, stockSymbol, subAccount, note));
                return 0;
            }
            catch (Exception e)
            {
                Error(String.Format("Error accepting discrepancy of {0} for {1} in subaccount {2}, reason={3}: ",
                        adjustment, stockSymbol, subAccount, note), e);
                return 1;
            }
            finally
            {
                stockPositionsAdapter.LogCommand("Reconciliation.p_accept_stock_discrepancy");
            }
        }
        public static int AcceptOptionDiscrepancy(string optionSymbol, DateTime expirationDate,
            decimal strikePrice, string optionType, string subAccount, int adjustment, string note)
        {
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                optionPositionsAdapter.AcceptDiscrepancy(ImportDate,
                    optionSymbol,
                    expirationDate,
                    strikePrice,
                    optionType,
                    subAccount,
                    adjustment,
                    note);

                optionPositionsTableFilled = false;
                FireOnTablesUpdated();
                return 0;

            }
            catch (Exception e)
            {
                Error(String.Format("Error accepting discrepancy of {0} for {1} {2:d} {3} {4} in subaccount {5}, reason={6}: ",
                        adjustment, optionSymbol, expirationDate, strikePrice, optionType, subAccount, note), e);
                return 1;
            }
            finally
            {
                optionPositionsAdapter.LogCommand("Reconciliation.p_accept_option_discrepancy");
            }
        }
        public static DateTime CalculatePreviousBusinessDay(DateTime currentDate)
        {
            CheckParameters(/*needAccountGroup =*/ false);

            System.Nullable<System.DateTime> prevDate = new System.Nullable<System.DateTime>();
            try
            {
                queriesAdapter.GetPreviousBusinessDay(currentDate, ref prevDate);
            }
            catch (Exception e)
            {
                Error(String.Format("Error calculating business day before {0}", currentDate), e);
            }
            finally
            {
                queriesAdapter.LogCommand("dbo.p_get_prev_business_day");
            }
            return prevDate.Value;
        }
        public static bool IsExerciseAssignment(string tradeMediumName)
        {
            return (tradeMediumName == exerciseAssignmentName) || (tradeMediumName == exerciseName) || (tradeMediumName == assignmentName);
        }
        public static bool IsAssignment(string tradeMediumName)
        {
            return tradeMediumName == assignmentName;
        }
        public static bool IsExpiration(string tradeMediumName)
        {
            return tradeMediumName == expiredName;
        }
        public static bool IsCancelOrCorrection(string canCodeName)
        {
            return (canCodeName == cancelName) || (canCodeName == correctionName);
        }
        public static IEnumerable<OptionTrade> GetOptionTradesForPackageId(int packageId)
        {
            foreach (HugoDataSet.HugoOptionTradesRow row in UnconsolidatedHugoOptionTrades)
            {
                if (row.ConsolidationPackageId == packageId)
                    yield return new OptionTrade(row);
            }
        }
        #endregion

        #region Methods to return HugoTables
        public static DataTable GetPershingConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_PershingConfirmationsTableAdapter pershingConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_PershingConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return pershingConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting pershing confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    pershingConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_PershingConfirmations");
                }
            }
        }
        public static DataTable GetPershingTaxlots(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_PershingAggregatedTaxlotsTableAdapter pershingAggregatedTaxlotsTableAdapter = new HugoDataSetTableAdapters.t_PershingAggregatedTaxlotsTableAdapter(Connection))
            {
                try
                {
                    return pershingAggregatedTaxlotsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting pershing taxlots for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    pershingAggregatedTaxlotsTableAdapter.LogCommand("Reconciliation.p_get_PershingAggregatedTaxlots");
                }
            }
        }
        public static DataTable GetPershingPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_PershingPositionsTableAdapter pershingPositionsTableAdapter = new HugoDataSetTableAdapters.t_PershingPositionsTableAdapter(Connection))
            {
                try
                {
                    return pershingPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting pershing positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    pershingPositionsTableAdapter.LogCommand("Reconciliation.p_get_PershingPositions");
                }
            }
        }
        public static DataTable GetWellsFargoConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_WellsFargoConfirmationsTableAdapter WellsFargoConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_WellsFargoConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return WellsFargoConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting WellsFargo confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    WellsFargoConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_WellsFargoConfirmations");
                }
            }
        }
        public static DataTable GetWellsFargoPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_WellsFargoPositionsTableAdapter WellsFargoPositionsTableAdapter = new HugoDataSetTableAdapters.t_WellsFargoPositionsTableAdapter(Connection))
            {
                try
                {
                    return WellsFargoPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting WellsFargo positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    WellsFargoPositionsTableAdapter.LogCommand("Reconciliation.p_get_WellsFargoPositions");
                }
            }
        }
        public static DataTable GetMorganStanleyConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_MorganStanleyConfirmationsTableAdapter MorganStanleyConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_MorganStanleyConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return MorganStanleyConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting MorganStanley confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    MorganStanleyConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_MorganStanleyConfirmations");
                }
            }
        }
        public static DataTable GetMorganStanleyPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_MorganStanleyPositionsTableAdapter MorganStanleyPositionsTableAdapter = new HugoDataSetTableAdapters.t_MorganStanleyPositionsTableAdapter(Connection))
            {
                try
                {
                    return MorganStanleyPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting MorganStanley positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    MorganStanleyPositionsTableAdapter.LogCommand("Reconciliation.p_get_MorganStanleyPositions");
                }
            }
        }
        public static DataTable GetBONYConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_BONYConfirmationsTableAdapter BONYConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_BONYConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return BONYConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting BONY confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    BONYConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_BONYConfirmations");
                }
            }
        }
        public static DataTable GetBONYPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_BONYPositionsTableAdapter BONYPositionsTableAdapter = new HugoDataSetTableAdapters.t_BONYPositionsTableAdapter(Connection))
            {
                try
                {
                    return BONYPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting BONY positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    BONYPositionsTableAdapter.LogCommand("Reconciliation.p_get_BONYPositions");
                }
            }
        }
        public static DataTable GetICBCConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_ICBCConfirmationsTableAdapter ICBCConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_ICBCConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return ICBCConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting ICBC confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    ICBCConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_ICBCConfirmations");
                }
            }
        }
        public static DataTable GetICBCPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_ICBCPositionsTableAdapter ICBCPositionsTableAdapter = new HugoDataSetTableAdapters.t_ICBCPositionsTableAdapter(Connection))
            {
                try
                {
                    return ICBCPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting ICBC positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    ICBCPositionsTableAdapter.LogCommand("Reconciliation.p_get_ICBCPositions");
                }
            }
        }
        public static DataTable GetIBConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_IBConfirmationsTableAdapter IBConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_IBConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return IBConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting IB confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    IBConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_IBConfirmations");
                }
            }
        }
        public static DataTable GetIBPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_IBPositionsTableAdapter IBPositionsTableAdapter = new HugoDataSetTableAdapters.t_IBPositionsTableAdapter(Connection))
            {
                try
                {
                    return IBPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting IB positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    IBPositionsTableAdapter.LogCommand("Reconciliation.p_get_IBPositions");
                }
            }
        }
        public static DataTable GetIBTransfers(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_IBTransfersTableAdapter IBTransfersTableAdapter = new HugoDataSetTableAdapters.t_IBTransfersTableAdapter(Connection))
            {
                try
                {
                    return IBTransfersTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting IB Transfers for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    IBTransfersTableAdapter.LogCommand("Reconciliation.p_get_IBTransfers");
                }
            }
        }
        public static DataTable GetIBCorporateActions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_IBCorporateActionsTableAdapter IBCorporateActionsTableAdapter = new HugoDataSetTableAdapters.t_IBCorporateActionsTableAdapter(Connection))
            {
                try
                {
                    return IBCorporateActionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting IB CorporateActions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    IBCorporateActionsTableAdapter.LogCommand("Reconciliation.p_get_IBCorporateActions");
                }
            }
        }
        public static DataTable GetLiquidConfirmations(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_LiquidConfirmationsTableAdapter LiquidConfirmationsTableAdapter = new HugoDataSetTableAdapters.t_LiquidConfirmationsTableAdapter(Connection))
            {
                try
                {
                    return LiquidConfirmationsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting Liquid confirmations for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    LiquidConfirmationsTableAdapter.LogCommand("Reconciliation.p_get_LiquidConfirmationRecords");
                }
            }
        }
        public static DataTable GetLiquidPositions(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_LiquidPositionsTableAdapter LiquidPositionsTableAdapter = new HugoDataSetTableAdapters.t_LiquidPositionsTableAdapter(Connection))
            {
                try
                {
                    return LiquidPositionsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting Liquid positions for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    LiquidPositionsTableAdapter.LogCommand("Reconciliation.p_get_LiquidPositionRecords");
                }
            }
        }
        public static DataTable GetTCWTaxlots(DateTime importDate, string acctGroupName, ref DateTime? lastImportDate)
        {
            using (HugoDataSetTableAdapters.t_TCWTaxlotsTableAdapter TCWTaxlotsTableAdapter = new HugoDataSetTableAdapters.t_TCWTaxlotsTableAdapter(Connection))
            {
                try
                {
                    return TCWTaxlotsTableAdapter.GetDataByImportDateAcctGroupName(importDate, acctGroupName, ref lastImportDate); ;
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting TCW taxlots for {0}, accountgroup={1}", importDate.ToString(), acctGroupName ?? "All"), e);
                    return null;
                }
                finally
                {
                    TCWTaxlotsTableAdapter.LogCommand("Reconciliation.p_get_TCWTaxlots");
                }
            }
        }
        public static DataTable GetTaskNames()
        {
            using (HugoDataSetTableAdapters.TaskNamesTableAdapter taskNamesTableAdapter = new HugoDataSetTableAdapters.TaskNamesTableAdapter(Connection))
            {
                try
                {
                    return taskNamesTableAdapter.GetData(ClearingHouse.ToString());
                }
                catch (Exception e)
                {
                    Error(String.Format("Error getting tasknames"), e);
                    return null;
                }
                finally
                {
                    taskNamesTableAdapter.LogCommand(0);
                }
            }
        }
        #endregion
        #endregion

        #region Internal Properties
        internal static bool StockPositionsTableFilled { get { return stockPositionsTableFilled; } }
        internal static bool OptionPositionsTableFilled { get { return optionPositionsTableFilled; } }
        internal static bool FuturesPositionsTableFilled { get { return futuresPositionsTableFilled; } }
        internal static bool AccountGroupsTableFilled { get { return accountGroupsTableFilled; } }
        internal static bool HugoStockTradesTableFilled { get { return hugoStockTradesTableFilled; } }
        internal static bool HugoFuturesTradesTableFilled { get { return hugoFuturesTradesTableFilled; } }
        internal static bool MerrillStockTradesTableFilled { get { return merrillStockTradesTableFilled; } }
        internal static bool MerrillFuturesTradesTableFilled { get { return merrillFuturesTradesTableFilled; } }
        internal static bool HugoOptionTradesTableFilled 
        { 
            get { return unconsolidatedHugoOptionTradesTableFilled; }
            set 
            {
                // if setting to false, invalidate all option trade tables
                if (!value)
                {
                    optionTradesWithMissingStockPricesTableFilled = unconsolidatedHugoOptionTradesTableFilled = consolidatedHugoOptionTradesTableFilled = false;
                }
                else
                    unconsolidatedHugoOptionTradesTableFilled = true;
            } 
      }

        internal static bool MerrillOptionTradesTableFilled { get { return merrillOptionTradesTableFilled; } }
        internal static bool SubAccountNamesTableFilled { get { return subAccountNamesTableFilled; } }
        internal static bool TradersTableFilled { get { return tradersTableFilled; } }
        internal static bool TradeMediaTableFilled { get { return tradeMediaTableFilled; } }
        internal static bool TradeTypesTableFilled { get { return tradeTypesTableFilled; } }
        internal static bool ExchangesTableFilled { get { return exchangesTableFilled; } }
        internal static bool BrokersTableFilled { get { return brokersTableFilled; } }
        internal static bool ClearingHouseFileNamesTableFilled { get { return clearingHouseFileNamesTableFilled; } }
        internal static bool StockTradeReasonsTableFilled { get { return stockTradeReasonsTableFilled; } }
        internal static bool OptionTradeReasonsTableFilled { get { return optionTradeReasonsTableFilled; } }
        internal static bool ConfirmationCorrectionsTodayTableFilled { get { return confirmationCorrectionsTodayTableFilled; } }
        internal static bool ConfirmationCorrectionsYesterdayTableFilled { get { return confirmationCorrectionsYesterdayTableFilled; } }
             internal static bool SymbolMappingsTableFilled { get { return symbolMappingsTableFilled; } }
        internal static bool HugoStockCorrectionsTableFilled { get { return hugoStockCorrectionsTableFilled; } }
        internal static bool HugoOptionCorrectionsTableFilled { get { return hugoOptionCorrectionsTableFilled; } }
        internal static bool HugoFuturesCorrectionsTableFilled { get { return hugoFuturesCorrectionsTableFilled; } }
        #endregion

        #region Internal methods
        internal static void BuildDifferenceMsgToken<T>(T newValue, T oldValue, string label, ref string msg) where T : IComparable
        {
            if (newValue.CompareTo(oldValue) != 0)
            {
                msg += String.Format("{0}{1}={2}", (msg.Length > 0) ? ", " : "", label, newValue);
            }
        }
        internal static void BuildDifferenceMsgToken(DateTime newValue, DateTime oldValue, string label, ref string msg)
        {
            if (newValue.CompareTo(oldValue) != 0)
            {
                msg += String.Format("{0}{1}={2:d}", (msg.Length > 0) ? ", " : "", label, newValue);
            }
        }
        internal static void LogSqlCommand(IDbCommand[] commandCollection, string commandText)
        {
            try
            {
                LoggingUtilities.LogSqlCommand("SqlLog", commandCollection, commandText);
            }
            catch (LoggingUtilitiesCommandNotFoundException e)
            {
                Info("Logging error: " + e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal static void LogSqlCommand(IDbCommand[] commandCollection, int index)
        {
            try
            {
                LoggingUtilities.LogSqlCommand("SqlLog", commandCollection, index);
            }
            catch (LoggingUtilitiesCommandNotFoundException e)
            {
                Info("Logging error: " + e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private methods

        #region Private fill methods

        private static void FillConsolidatedHugoOptionTradesTable(string underlyingSymbol, string optionSymbol, DateTime? expirationDate, decimal? strikePrice, string optionType, string requestedOptions)
        {
            int count = 0;
            try
            {
                count = hugoOptionTradesAdapter.Fill(hugoDataSet.ConsolidatedOptionTrades, underlyingSymbol,
                                optionSymbol, expirationDate, strikePrice, optionType,
                               AccountGroupName, PreviousDate, ImportDate, true, false);
                consolidatedHugoOptionTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} consolidated option trades for {1} from {2} to {3}", requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_get_option_trades3");
                Info(String.Format("Retrieved {0} {1} consolidated option trades for {2} from {3} to {4}", count, requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }
        }

        private static void FillUnconsolidatedHugoOptionTradesTable(string underlyingSymbol, string optionSymbol, DateTime? expirationDate, decimal? strikePrice, string optionType, string requestedOptions)
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                count = hugoOptionTradesAdapter.Fill(hugoDataSet.UnconsolidatedOptionTrades, underlyingSymbol,
                                optionSymbol, expirationDate, strikePrice, optionType,
                               AccountGroupName, PreviousDate, ImportDate, false, false);
                unconsolidatedHugoOptionTradesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting {0} hugo option trades for {1} from {2} to {3}", requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_get_option_trades3");
                Info(String.Format("Retrieved {0} {1} hugo option trades for {2} from {3} to {4}", count, requestedOptions, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }
        }
        private static void FillOptionTradesWithMissingStockPricesTable()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);
                count = hugoOptionTradesAdapter.Fill(hugoDataSet.OptionTradesWithMissingStockPrices, null,
                                null, null, null, null,
                               AccountGroupName, PreviousDate, ImportDate, false, true);
                optionTradesWithMissingStockPricesTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error getting option trades with missing stock prices for {0} from {1} to {2}", AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoOptionTradesAdapter.LogCommand("Reconciliation.p_get_option_trades3");
                Info(String.Format("Retrieved {0} option trades with missing stock prices for {1} from {2} to {3}", count, AccountGroupName, ReconciliationConvert.ToNullableString(PreviousDate), ReconciliationConvert.ToNullableString(ImportDate)));
            }
        }
        private static int FillStockPositions()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = stockPositionsAdapter.Fill(hugoDataSet.StockPositions, AccountGroupName, ImportDate, PreviousDate);
                GetDiscrepancies(hugoDataSet.StockPositions,
                            ref stockDiscrepancyCount,
                            ref stockPriceDiscrepancyCount,
                            ref stockTotalCostDiscrepancyCount);
                stockPositionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling stock positions table for {0} on {1} vs {2}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)), e);
            }
            finally
            {
                stockPositionsAdapter.LogCommand("Reconciliation.p_get_compared_stock_positions2");
                Info(String.Format("Retreived {0} stock position records for {1} on {2} vs {3}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)));
            }
            return count;
        }
        private static int FillOptionPositions()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                optionPositionsAdapter.SetCommandTimeout("Reconciliation.p_get_compared_option_positions2", 0);
                count = optionPositionsAdapter.Fill(hugoDataSet.OptionPositions, AccountGroupName, ImportDate, PreviousDate);
                GetDiscrepancies(hugoDataSet.OptionPositions,
                          ref optionDiscrepancyCount,
                          ref optionPriceDiscrepancyCount,
                          ref optionTotalCostDiscrepancyCount);
                optionPositionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling option positions table for {0} on {1} vs {2}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)), e);
            }
            finally
            {
                optionPositionsAdapter.LogCommand("Reconciliation.p_get_compared_option_positions2");
                Info(String.Format("Retreived {0} option position records for {1} on {2} vs {3}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)));
            }
            return count;
        }
        private static int FillFuturesPositions()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = futuresPositionsAdapter.Fill(hugoDataSet.FuturesPositions, AccountGroupName, ImportDate, PreviousDate);
                GetDiscrepancies(hugoDataSet.FuturesPositions,
                          ref futuresDiscrepancyCount,
                          ref futuresPriceDiscrepancyCount,
                          ref futuresTotalCostDiscrepancyCount);
                futuresPositionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling futures positions table for {0} on {1} vs {2}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)), e);
            }
            finally
            {
                futuresPositionsAdapter.LogCommand("Reconciliation.p_get_compared_futures_positions");
                Info(String.Format("Retreived {0} futures position records for {1} on {2} vs {3}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate), ReconciliationConvert.ToNullableString(PreviousDate)));
            }
            return count;
        }
        private static int FillAccountGroups()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = accountGroupsAdapter.Fill(hugoDataSet.AccountGroups);
                accountGroupsTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling account groups table", e);
            }
            finally
            {
                accountGroupsAdapter.LogCommand("Reconciliation.p_get_account_groups");
                Info(String.Format("Retreived {0} account groups", count));
            }
            return count;
        }
        private static int FillTraders()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = tradersAdapter.Fill(hugoDataSet.Traders, AccountGroupName);
                tradersTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling traders table", e);
            }
            finally
            {
                tradersAdapter.LogCommand();
                Info(String.Format("Retreived {0} traders", count));
            }
            return count;
        }
        private static int FillTradeMedia()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = tradeMediaAdapter.Fill(hugoDataSet.TradeMedia);
                tradeMediaTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling trade media table", e);
            }
            finally
            {
                tradeMediaAdapter.LogCommand();
                Info(String.Format("Retreived {0} trade media", count));
            }
            return count;
        }
        private static int FillTradeTypes()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = tradeTypesAdapter.Fill(hugoDataSet.TradeTypes);
                tradeTypesTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling trade types table", e);
            }
            finally
            {
                tradeTypesAdapter.LogCommand();
                Info(String.Format("Retreived {0} trade types", count));
            }
            return count;
        }
        private static int FillExchanges()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = exchangesAdapter.Fill(hugoDataSet.Exchanges);
                exchangesTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling exchanges table", e);
            }
            finally
            {
                exchangesAdapter.LogCommand();
                Info(String.Format("Retreived {0} exchanges", count));
            }
            return count;
        }
        private static int FillBrokers()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = brokersAdapter.Fill(hugoDataSet.Brokers);
                brokersTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling brokers table", e);
            }
            finally
            {
                brokersAdapter.LogCommand();
                Info(String.Format("Retreived {0} brokers", count));
            }
            return count;
        }
        private static int FillClearingHouseFileNames()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = clearingHouseFileNamesAdapter.Fill(hugoDataSet.ClearingHouseFileNames);
                clearingHouseFileNamesTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling clearinghouse filenames table", e);
            }
            finally
            {
                clearingHouseFileNamesAdapter.LogCommand();
                Info(String.Format("Retreived {0} clearinghousefile names", count));
            }
            return count;
        }
        private static int FillStockTradeReasons()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = stockTradeReasonsAdapter.Fill(hugoDataSet.StockTradeReasons);
                stockTradeReasonsTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling stock trade reasons table", e);
            }
            finally
            {
                stockTradeReasonsAdapter.LogCommand();
                Info(String.Format("Retreived {0} stock trade reasons", count));
            }
            return count;
        }
        private static int FillOptionTradeReasons()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ false);

                count = optionTradeReasonsAdapter.Fill(hugoDataSet.OptionTradeReasons);
                optionTradeReasonsTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling option trade reasons table", e);
            }
            finally
            {
                optionTradeReasonsAdapter.LogCommand();
                Info(String.Format("Retreived {0} option trade reasons", count));
            }
            return count;
        }
        private static int FillConfirmationCorrectionsToday()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = confirmationCorrectionsAdapter.Fill(hugoDataSet.ConfirmationCorrectionsToday,
                    AccountGroupName,
                    ImportDate,
                    ImportDate);
                confirmationCorrectionsTodayTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling today's confirmation corrections table", e);
            }
            finally
            {
                confirmationCorrectionsAdapter.LogCommand("Reconciliation.p_get_confirmation_corrections2");
                Info(String.Format("Retreived {0} confirmation corrections for today", count));
            }
            return count;
        }
        private static int FillConfirmationCorrectionsYesterday()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = confirmationCorrectionsAdapter.Fill(hugoDataSet.ConfirmationCorrectionsYesterday,
                    AccountGroupName,
                    PreviousDate,
                    PreviousDate);
                confirmationCorrectionsYesterdayTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling yesterday's confirmation corrections table", e);
            }
            finally
            {
                confirmationCorrectionsAdapter.LogCommand("Reconciliation.p_get_confirmation_corrections2");
                Info(String.Format("Retreived {0} confirmation corrections for yesterday", count));
            }
            return count;
        }
        private static int FillSubaccountNames()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = subaccountNamesAdapter.Fill(hugoDataSet.SubaccountNames, AccountGroupName);
                subAccountNamesTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling subaccount names table", e);
            }
            finally
            {
                subaccountNamesAdapter.LogCommand("Reconciliation.p_get_subaccount_names");
                Info(String.Format("Retreived {0} subaccount names", count));
            }
            return count;
        }
        private static int FillSymbolMappings()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = symbolMappingsAdapter.Fill(hugoDataSet.SymbolMappings, ClearingHouse.ToString());
                symbolMappingsTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling symbol mappings table", e);
            }
            finally
            {
                symbolMappingsAdapter.LogCommand("Reconciliation.p_get_gargoyle_merrill_symbol_mappings2");
                Info(String.Format("Retreived {0} symbol mappings", count));
            }
            return count;
        }
        private static int FillHugoStockCorrections()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = hugoStockCorrectionsAdapter.Fill(hugoDataSet.HugoStockCorrections, AccountGroupName, ImportDate, ImportDate);
                hugoStockCorrectionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling hugo stock corrections table for {0} on {1}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoStockCorrectionsAdapter.LogCommand("Reconciliation.p_get_Hugo_stock_corrections2");
                Info(String.Format("Retreived {0} hugo stock corrections for {1} on {2}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)));
            }
            return count;
        }
        private static int FillHugoFuturesCorrections()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = hugoFuturesCorrectionsAdapter.Fill(hugoDataSet.HugoFuturesCorrections, AccountGroupName, ImportDate, ImportDate);
                hugoFuturesCorrectionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling hugo Futures corrections table for {0} on {1}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoFuturesCorrectionsAdapter.LogCommand("Reconciliation.p_get_Hugo_futures_corrections");
                Info(String.Format("Retreived {0} hugo futures corrections for {1} on {2}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)));
            }
            return count;
        }
        private static int FillTraderAccounts()
        {
            int count = 0;
            try
            {
                count = traderAccountsAdapter.Fill(hugoDataSet.TraderAccounts);
                traderAccountsTableFilled = true;
            }
            catch (Exception e)
            {
                Error("Error filling trader Accounts table", e);
            }
            finally
            {
                traderAccountsAdapter.LogCommand("dbo.p_get_trader_accounts");
                Info(String.Format("Retreived {0} trader Accounts", count));
            }
            return count;
        }
 
        private static int FillHugoOptionCorrections()
        {
            int count = 0;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                count = hugoOptionCorrectionsAdapter.Fill(hugoDataSet.HugoOptionCorrections, AccountGroupName, ImportDate, ImportDate);
                hugoOptionCorrectionsTableFilled = true;
            }
            catch (Exception e)
            {
                Error(String.Format("Error filling hugo option corrections table for {0} on {1}", AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)), e);
            }
            finally
            {
                hugoOptionCorrectionsAdapter.LogCommand("Reconciliation.p_get_Hugo_option_corrections2");
                Info(String.Format("Retreived {0} hugo option corrections for {1} on {2}", count, AccountGroupName, ReconciliationConvert.ToNullableString(ImportDate)));
            }
            return count;
        }
        #endregion

        #region Private insert methods
        private static void InsertPositionRecord(IPositionTableAdapter positionTableAdapter, ref int count, IPositionRecordCollection positionRecords, ref PositionRecord positionRecord)
        {
            if (positionRecord.IsValid)
            {
                count += positionTableAdapter.InsertRecord(positionRecord);
                Info("Inserting position record: " + positionRecord.ToString());
            }
            else
            {
                Info("Skipping position record: " + positionRecord.ToString());
            }
            positionRecord = positionRecords.NextRecord;
        }
        private static void InsertConfirmationRecord(IConfirmationTableAdapter confirmationTableAdapter, ref int count, IConfirmationRecordCollection confirmationRecords, ref ConfirmationRecord confirmationRecord)
        {
            if (confirmationRecord.IsValid)
            {
                count += confirmationTableAdapter.InsertRecord(confirmationRecord);
                Info("Inserting confirmation record: " + confirmationRecord.ToString());
            }
            else
            {
                Info("Skipping confirmation record: " + confirmationRecord.ToString());
            }
            confirmationRecord = confirmationRecords.NextRecord;
        }
        private static void InsertTaxlotRecord(ITaxlotTableAdapter taxlotTableAdapter, ref int count, ITaxlotRecordCollection taxlotRecords, ref TaxlotRecord taxlotRecord)
        {
            if (taxlotRecord.IsValid)
            {
                count += taxlotTableAdapter.InsertRecord(taxlotRecord);
                Info("Inserting taxlot record: " + taxlotRecord.ToString());
            }
            else
            {
                Info("Skipping taxlot record: " + taxlotRecord.ToString());
            }
            taxlotRecord = taxlotRecords.NextRecord;

        }
        #endregion

        #region Fire events
        public static void Info(string msg)
        {
            LoggingUtilities.Info("ReconciliationLib", msg);
        }
        public static void Error(string msg, Exception e)
        {
            LoggingUtilities.Error("ReconciliationLib", msg, e);
        }
        private static void FireOnTablesUpdated()
        {
            if (tablesUpdatedEventHandler != null)
            {
                try
                {
                    TablesUpdatedEventArgs eventArgs = new TablesUpdatedEventArgs();
                    tablesUpdatedEventHandler(null, eventArgs);

                    Info(eventArgs.ToString());
                }
                catch (Exception e)
                {
                    Error("Error in TablesUpdated event handler", e);
                }
            }
        }
        #endregion

        #region Verification
        private static void CheckParameters(bool needAccountInfo)
        {
            // must initialize
            if (!initialized)
                throw new ReconciliationNotInitializedException();

            // must specify a connection 
            if (Connection == null)
                throw new ReconciliationNoConnectionException();

            // must specify account info
            if ((AccountGroupName == null) && needAccountInfo)
                throw new ReconciliationNoAccountGroupNameException();

            if ((ClearingHouse == ClearingHouse.None) && needAccountInfo)
                throw new ReconciliationNoClearingHouseException();
        }

        private static void CheckOptionParameters(IUnderlying underlying, IOption option,
            out string underlyingSymbol, out string optionSymbol, out DateTime? expirationDate,
            out decimal? strikePrice, out string optionType, out string requestedOptions)
        {
            underlyingSymbol = (option == null) ? ((underlying == null) ? null : underlying.UnderlyingSymbol) : option.UnderlyingSymbol;
            optionSymbol = (option == null) ? null : option.OptionSymbol;
            expirationDate = (option == null) ? null : option.ExpirationDate as DateTime?;
            strikePrice = (option == null) ? null : option.StrikePrice as decimal?;
            optionType = (option == null) ? null : option.OptionType;

            // if we don't have an option symbol, try underlying symbol
            if (optionSymbol == null)
            {
                if (underlyingSymbol == null)
                    // TODO - disallow this in certain conditions
                    requestedOptions = "all";
                else
                    requestedOptions = underlyingSymbol;
            }

            // if we have an option symbol, the associated parameters cannot be null
            else
            {
                if (!expirationDate.HasValue)
                    throw new ArgumentNullException("expirationDate");
                if (!strikePrice.HasValue)
                    throw new ArgumentNullException("strikePrice");
                if (optionType == null)
                    throw new ArgumentNullException("optionType");
                requestedOptions = String.Format("{0} {1:d} {2} {3}", optionSymbol, expirationDate, strikePrice, optionType);
            }
        }
        #endregion

        #endregion

        #region Misc helper methods
        // returns number of records read (1 in the case of an xml file)
        private static int ConvertFileToXml(string filename, string tableName, out string xml)
        {
            if (filename.EndsWith(".xls", true, null))
            {
                return XLStoXMLConverter.Convert(filename, tableName, out xml);
            }
            else if (filename.EndsWith(".csv", true, null))
            {
                return CSVtoXMLConverter.Convert(filename, tableName, out xml);
            }
            else if (filename.EndsWith(".xml", true, null))
            {
                return ReadXmlFile(filename, out xml);
            }
            else
            {
                throw new ReconciliationImportException("Only xls, csv, and xml files are supported for ImportTrades");
            }
        }

        private static string BuildTradeXmlString(int[] tradeIds)
        {
            StringBuilder strBuilder = new StringBuilder("<TradeIds>");
            foreach (int tradeId in tradeIds)
            {
                strBuilder.Append(String.Format("<row TradeId=\"{0}\" />", tradeId));
            }
            strBuilder.Append("</TradeIds>");

            return strBuilder.ToString();
        }
        // load the dictionary 

        private static AccountGroupInfo GetAccountGroupInfo()
        {
            AccountGroupInfo newAccountGroupInfo = null;
            try
            {
                CheckParameters(/*needAccountGroup =*/ true);

                // StartImport will perform initialization the first time it is called
                //  on a given day for a given account group.
                // Thereafter, it simply returns state.
                bool? isFinalized = null;
                bool? assignmentsDone = null;
                DateTime? lastImportDate = null;    // date we last imported Merrill's positions (usually today)
                DateTime? prevImportDate = null;    // date of penultimate import of Merrill's positions
                queriesAdapter.StartImportForAccountGroup(AccountGroupName, ref isFinalized, ref lastImportDate, ref prevImportDate, ref assignmentsDone);

                if (isFinalized.HasValue)
                {
                    newAccountGroupInfo = new AccountGroupInfo(AccountGroupName, isFinalized.Value, assignmentsDone.Value, lastImportDate.Value, prevImportDate.Value);
                }
                else
                {
                    throw new ReconciliationException("p_start_import_for_account_group returned NULL");
                }
            }
            catch (Exception e)
            {
                Error(String.Format("Error starting import for account group {0}", AccountGroupName), e);
                return null;
            }
            finally
            {
                queriesAdapter.LogCommand("Reconciliation.p_start_import_for_account_group");
            }
            return newAccountGroupInfo;
        }
         private static string GetTransactionCode(string tradeType, bool shortFlag)
        {
            if (tradeType == "Buy")
                return "B";
            else if (tradeType == "Sell")
                return shortFlag ? "SS" : "S";
            else return null;
        }

        private static int AddFlexFromOptionTrade(IOptionTrade optionTrade)
        {
            // see if this is a flex symbol
            if ((optionTrade.OptionSymbol[0] == '1') || (optionTrade.OptionSymbol[0] == '2'))
            {
                // if so, make sure flex exists in Hugo
                HugoDataSet.SubaccountNamesRow row = hugoDataSet.SubaccountNames.FindBySubAcctName(optionTrade.SubAcctName);
                return AddFlex(optionTrade, (row == null) ? null : row.AcctID as short?);
            }
            else
            {
                return 0;
            }
        }

        private static void GetDiscrepancies(DataTable dataTable,
            ref int discrepancyCount,
            ref int priceDiscrepancyCount,
            ref int totalCostDiscrepancyCount)
        {
            DataView dataView = null;
            try
            {
                dataView = new DataView(dataTable);
                dataView.RowFilter = "(Discrepancy <> 0)";
                discrepancyCount = dataView.Count;

                dataView.RowFilter = "(PriceDiscrepancy = '$')";
                priceDiscrepancyCount = dataView.Count;

                dataView.RowFilter = "(TotalCostDiscrepancy = '$')";
                totalCostDiscrepancyCount = dataView.Count;

                Info(String.Format("Discrepancy count = {0}", discrepancyCount));
                Info(String.Format("Price discrepancy count = {0}", priceDiscrepancyCount));
                Info(String.Format("Total cost discrepancy count = {0}", totalCostDiscrepancyCount));
            }
            catch (Exception e)
            {
                Error("Error getting discrepancy counts", e);
            }
            finally
            {
                if (dataView != null)
                {
                    dataView.Dispose();
                }
            }
        }

        public static int ReadXmlFile(string filename, out string xml)
        {
            xml = File.ReadAllText(filename);
            return 1;
        }
        #endregion

    }
}
