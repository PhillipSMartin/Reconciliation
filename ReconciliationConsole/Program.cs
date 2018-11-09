using System;
using System.Collections.Generic;
using System.Text;
using Gargoyle.Utilities.CommandLine;
using log4net;
using ReconciliationLib;
using LoggingUtilitiesLib;
using GargoyleTaskLib;

namespace ReconciliationConsole
{
    class Program
    {
//      private const string creditSuisseContra = "0355";
        private const string applicationName = "ReconciliationConsole";
        private const string databaseName = "Hugo";
        private static ILog s_logger;
        private static string s_completionMessage;
        private static string s_lastErrorMessage;
        private static bool s_softFailOnFileNotFound;
        private static TaskUtilities s_taskUtilities;

        static void Main(string[] args)
        {
            CommandLineParameters parms = new CommandLineParameters();

            try
            {
                // initialize log4net via app.config
                log4net.Config.XmlConfigurator.Configure();
                s_logger = LogManager.GetLogger(typeof(Program));

                if (Utility.ParseCommandLineArguments(args, parms))
                {

                    if (Run(parms))
                    {
                        System.Diagnostics.EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName.Split('.')[0], "Gargoyle Reconciliation Console completed", System.Diagnostics.EventLogEntryType.Information);
                    }
                    else
                    {
                        System.Diagnostics.EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName.Split('.')[0], "Gargoyle Reconciliation Console failed - see error log", System.Diagnostics.EventLogEntryType.Error);
                    }
                }
                else
                {
                    // display usage message
                    string errorMessage = Utility.CommandLineArgumentsUsage(typeof(CommandLineParameters));

                    Console.WriteLine(errorMessage);
                    System.Diagnostics.EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName.Split('.')[0], errorMessage, System.Diagnostics.EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Diagnostics.EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName.Split('.')[0], ex.Source + ": " + ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                ReconciliationLib.Utilities.Dispose();
            }
        }

        private static void Utilities_OnError(object sender, ErrorEventArgs e)
        {
            bool bSoftFail = false;  // allow soft fail on certain errors
            s_lastErrorMessage = e.Exception.Message;


            if (e.Exception.GetType() == typeof(ReconciliationTradeException))
            {
                if (((ReconciliationTradeException)e.Exception).ReturnCode == ReconciliationTradeException.CannotAddOption)
                {
                    bSoftFail = true;
                }
            }
            else if (s_softFailOnFileNotFound)
            {
                if (e.Exception.GetType() == typeof(System.IO.FileNotFoundException))
                {
                    bSoftFail = true;
                }
            }

            if (bSoftFail)
            {
                if (s_logger != null)
                {
                    s_logger.Info(e.Exception.Message);
                }
            }
            else
            {
                throw e.Exception;
            }
        }
        private static void Utilities_OnInfo(object sender, InfoEventArgs e)
        {
            if (s_logger != null)
            {
                s_logger.Info(e.Message);
            }
        }

        private static bool Run(CommandLineParameters parms)
        {
            bool bTaskStarted = false;
            bool bReturn = false;

            try
            {

                InitializeUtilities(parms);

                int rc = StartTask(parms);
                if (rc == 1)
                    return true;    // task violates scheduling rules and should not be run

                if (rc == 0)
                    bTaskStarted = true;  // task was marked as started and must be marked as completed or failed on exit

                switch (parms.Action)
                {
                    case ReconciliationAction.Import:
                        bReturn = ImportFiles(parms);
                        break;

                     case ReconciliationAction.AddTrades:
                        bReturn = AddTrades(parms);
                        break;

                    case ReconciliationAction.ConsolidateOptionTrades:
                        bReturn = ConsolidateTrades(parms, true, false);
                        break;

                    case ReconciliationAction.ConsolidateStockTrades:
                        bReturn = ConsolidateTrades(parms, false, true);
                        break;

                    case ReconciliationAction.ConstructTaxlotsFile:
                        bReturn = ConstructTaxlotsFile(parms);
                        break;

                    default:
                        if (s_logger != null)
                            s_logger.Error("Invalid action: " + parms.Action.ToString());
                        bReturn = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                s_lastErrorMessage = ex.Message;
                if (s_logger != null)
                    s_logger.Error("Error: ", ex);
                bReturn = false;
            }
            finally
            {
                // record task status
                if (bTaskStarted)
                    EndTask(parms, bReturn);
            }
            return bReturn;
        }

        // returns 0 if task is successfully started
        // returns 1 if we should not start the task because it should not be run today (because this task should not run on a holiday, for example)
        // returns 4 if we cannot find the task name
        // returns some number higher than 4 on an unexpected failure
        private static int StartTask(CommandLineParameters parms)
        {
            if (String.IsNullOrEmpty(parms.TaskName))
            {
                return 4;
            }
            else
            {
                return s_taskUtilities.TaskStarted(parms.TaskName, ReconciliationLib.Utilities.ImportDate);
            }
        }

        private static void EndTask(CommandLineParameters parms, bool succeeded)
        {
            if (succeeded)
                s_taskUtilities.TaskCompleted(parms.TaskName, s_completionMessage);
            else
                s_taskUtilities.TaskFailed(parms.TaskName, s_lastErrorMessage);
        }

        private static void InitializeUtilities(CommandLineParameters parms)
        {
            ReconciliationLib.Utilities.Init();
  
            // wire events
            ReconciliationLib.Utilities.OnError += new EventHandler<ErrorEventArgs>(Utilities_OnError);
            ReconciliationLib.Utilities.OnInfo += new EventHandler<InfoEventArgs>(Utilities_OnInfo);
            TaskUtilities.OnError += new EventHandler<ErrorEventArgs>(Utilities_OnError);
            TaskUtilities.OnInfo += new EventHandler<InfoEventArgs>(Utilities_OnInfo);

            // get Hugo connection
            Gargoyle.Utils.DBAccess.DBAccess dbAccess = Gargoyle.Utils.DBAccess.DBAccess.GetDBAccessOfTheCurrentUser(applicationName);
            ReconciliationLib.Utilities.Connection = dbAccess.GetConnection(databaseName);
            s_taskUtilities = new TaskUtilities(ReconciliationLib.Utilities.Connection);

 
            if (s_logger != null)
                s_logger.Info("Connecting to " + ReconciliationLib.Utilities.Connection.ConnectionString);

            // set account group
            if (!String.IsNullOrEmpty(parms.AccountGroupName))
            {
                ReconciliationLib.Utilities.AccountGroupName = parms.AccountGroupName;

                // if we have not specified a clearing house, use the one associated with the account group
                if (parms.ClearingHouse == ClearingHouse.None)
                {
                    parms.ClearingHouse = ReconciliationLib.Utilities.ClearingHouse;
                }
            }

            // set dates
            ReconciliationLib.Utilities.ImportDate = parms.GetImportDateOverride() ?? DateTime.Today;
            ReconciliationLib.Utilities.PreviousDate = ReconciliationLib.Utilities.CalculatePreviousBusinessDay(ReconciliationLib.Utilities.ImportDate.Value);
        }

        private static bool ConsolidateTrades(CommandLineParameters parms, bool consolidateOptions, bool consolidateStocks)
        {
            bool bResult = false;

            try
            {
                if (String.IsNullOrEmpty(parms.AccountGroupName))
                {
                    s_logger.Error("Cannot consolidate trades without specifying an account group", null);
                }
                else
                {
                    if (consolidateOptions || consolidateStocks)
                    {
                        ReconciliationLib.Utilities.ConsolidateTrades(consolidateOptions, consolidateStocks, null);
                        bResult = true;

                        if (s_logger != null)
                        {
                            string consolidationOptions = consolidateOptions ?
                                (consolidateStocks ? "stock and option" : "option") : "stock";
                            s_logger.Info(String.Format("Consolidated {0} trades for {1}", consolidationOptions, parms.AccountGroupName));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                s_lastErrorMessage = ex.Message;
                if (s_logger != null)
                    s_logger.Error("Error consolidating trades", ex);
                bResult = false;
            }

            return bResult;
        }

        private static bool ConstructTaxlotsFile(CommandLineParameters parms)
        {
            bool bResult = false;

            try
            {
                if (parms.AccountId < 0)
                {
                    s_logger.Error("Cannot construct taxlots file without specifying an acctid", null);
                }
                else
                {
                    ReconciliationLib.Utilities.ConstructTaxlots((short)parms.AccountId);
                    bResult = true;

                    if (s_logger != null)
                    {
                        s_logger.Info(String.Format("Constructed taxlots file for acctid {0}", parms.AccountId));
                    }
                }

            }
            catch (Exception ex)
            {
                s_lastErrorMessage = ex.Message;
                if (s_logger != null)
                    s_logger.Error("Error constructing taxlots file", ex);
                bResult = false;
            }

            return bResult;
        }

        private static bool AddTrades(CommandLineParameters parms)
        {
            bool bResult = true;

            try
            {
                if (bResult)
                {
                    int nAddedOptionTrades = AddOptionTrades();
                    if ((s_logger != null) && (nAddedOptionTrades > 0))
                    {
                        s_logger.Info(String.Format("Added {0} option trade(s) to Hugo", nAddedOptionTrades));
                    }
                    int nAddedStockTrades = AddStockTrades();
                    if ((s_logger != null) && (nAddedStockTrades > 0))
                    {
                        s_logger.Info(String.Format("Added {0} stock trade(s) to Hugo", nAddedStockTrades));
                    }
                    if (CanFinalize())
                    {
                        ReconciliationLib.Utilities.FinalizeImport();
                    }
                }

            }
            catch (Exception ex)
            {
                s_lastErrorMessage = ex.Message;
                if (s_logger != null)
                    s_logger.Error("Error adding trades", ex);
                bResult = false;
            }

            return bResult;
        }

        private static int AddOptionTrades()
        {
            // get all trades
            ReconciliationLib.HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades = ReconciliationLib.Utilities.GetMerrillOptionTrades(null, null);
            ReconciliationLib.HugoDataSet.HugoOptionTradesDataTable hugoOptionTrades = ReconciliationLib.Utilities.GetHugoOptionTrades(null, null, false);

            // get matched Merrill trade ids
            List<int> matchedMerrillTradeIds = new List<int>();
            foreach (ReconciliationLib.HugoDataSet.HugoOptionTradesRow hugoRow in hugoOptionTrades.Rows)
            {
                int merrillTradeId = FindMerrillOptionTrade(merrillOptionTrades, matchedMerrillTradeIds, new OptionTrade(hugoRow));
                if (merrillTradeId >= 0)
                {
                    matchedMerrillTradeIds.Add(merrillTradeId);
                }
            }

            // add all unmatched trades
            int nAddedTrades = 0;
            foreach (ReconciliationLib.HugoDataSet.MerrillOptionTradesRow merrillRow in merrillOptionTrades.Rows)
            {
                OptionTrade optionTrade = new OptionTrade(merrillRow);
                if (!matchedMerrillTradeIds.Contains(optionTrade.TradeId.Value))
                {
                    if (VerifyOptionTrade(optionTrade))
                    {
                        if (null != ReconciliationLib.Utilities.AddOptionTrade(optionTrade))
                        {
                            nAddedTrades++;
                        }
                    }
                }
            }
            return nAddedTrades;
        }
        private static int AddStockTrades()
        {
            // get all trades
            ReconciliationLib.HugoDataSet.MerrillStockTradesDataTable merrillStockTrades = ReconciliationLib.Utilities.GetMerrillStockTrades(null, false);
            ReconciliationLib.HugoDataSet.HugoStockTradesDataTable hugoStockTrades = ReconciliationLib.Utilities.GetHugoStockTrades(null, false);

            // get matched Merrill trade ids
            List<int> matchedMerrillTradeIds = new List<int>();
            foreach (ReconciliationLib.HugoDataSet.HugoStockTradesRow hugoRow in hugoStockTrades.Rows)
            {
                int merrillTradeId = FindMerrillStockTrade(merrillStockTrades, matchedMerrillTradeIds, new StockTrade(hugoRow));
                if (merrillTradeId >= 0)
                {
                    matchedMerrillTradeIds.Add(merrillTradeId);
                }
            }

            // add all unmatched trades
            int nAddedTrades = 0;
            foreach (ReconciliationLib.HugoDataSet.MerrillStockTradesRow merrillRow in merrillStockTrades.Rows)
            {
                StockTrade stockTrade = new StockTrade(merrillRow);
                if (!matchedMerrillTradeIds.Contains(stockTrade.TradeId.Value))
                {
                    if (VerifyStockTrade(stockTrade))
                    {
                        if (null != ReconciliationLib.Utilities.AddStockTrade(stockTrade))
                        {
                            nAddedTrades++;
                        }
                    }
                }
            }
            return nAddedTrades;
        }

        private static bool VerifyOptionTrade(IOptionTrade optionTrade)
        {
            if (optionTrade == null)
                return false;

            if ((optionTrade.TradeVolume <= 0)  // don't add trades with no volum
                || (optionTrade.TradeMedium == ReconciliationLib.Utilities.ReorgName) // don't add reorg trades
                || (optionTrade.OptionArchiveFlag ?? false)) // don't add trades for archived options
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool VerifyStockTrade(IStockTrade stockTrade)
        {
            if (stockTrade == null)
                return false;

            if (stockTrade.FullVolume <= 0)
                return false;

            if (ReconciliationLib.Utilities.IsExerciseAssignment(stockTrade.TradeMedium))
                return false;

            // Credit Suisse aggregates trades - they won't match
 //         if (stockTrade.ContraName == creditSuisseContra)
  //              return false;

            return true;
        }
        private static int FindMerrillOptionTrade(ReconciliationLib.HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades, List<int> matchedMerrillTradeIds, OptionTrade hugoOptionTrade)
        {
            foreach (ReconciliationLib.HugoDataSet.MerrillOptionTradesRow merrillRow in merrillOptionTrades.Rows)
            {
                OptionTrade merrillOptionTrade = new OptionTrade(merrillRow);
                if (!matchedMerrillTradeIds.Contains(merrillOptionTrade.TradeId.Value))
                {
                    if (merrillOptionTrade.Equals(hugoOptionTrade))
                    {
                        return merrillRow.TradeId;
                    }
                }
            }
            return -1;
        }
        private static int FindMerrillStockTrade(ReconciliationLib.HugoDataSet.MerrillStockTradesDataTable merrillStockTrades, List<int> matchedMerrillTradeIds, StockTrade hugoStockTrade)
        {
            foreach (ReconciliationLib.HugoDataSet.MerrillStockTradesRow merrillRow in merrillStockTrades.Rows)
            {
                StockTrade merrillStockTrade = new StockTrade(merrillRow);
                if (!matchedMerrillTradeIds.Contains(merrillStockTrade.TradeId.Value))
                {
                    if (merrillStockTrade.Equals(hugoStockTrade))
                    {
                        return merrillRow.TradeId;
                    }
                }
            }
            return -1;
        }
        private static bool CanFinalize()
        {
            int stockDiscrepancies = ReconciliationLib.Utilities.StockDiscrepancyCount;
            int optionDiscrepancies = ReconciliationLib.Utilities.OptionDiscrepancyCount;

            if ((stockDiscrepancies == 0) && (optionDiscrepancies == 0))
            {
                return true;
            }
            else
            {
                if (s_logger != null)
                {
                    s_logger.Error(String.Format("Cannot finalize - we have {0}{1}{2}",
                        (stockDiscrepancies == 0) ? "" : String.Format("{0} stock discrepanc{1}", stockDiscrepancies, (stockDiscrepancies == 1) ? "y" : "ies"),
                        (stockDiscrepancies * optionDiscrepancies == 0) ? "" : " and ",
                        (optionDiscrepancies == 0) ? "" : String.Format("{0} option discrepanc{1}", optionDiscrepancies, (optionDiscrepancies == 1) ? "y" : "ies")));
                }
             return false;
            }
        }

  
        private static AccountGroupInfo GetAccountGroupInfo()
        {
            AccountGroupInfo info = ReconciliationLib.Utilities.AccountGroupInfo;
            if (info == null)
            {
                throw new ReconciliationException("Unable to get account group info");
            }
            return info;
        }

        private static string BuildExpirationAndAssignmentMessage(int addedExpirations, int notAddedExpirations, int errorsAddingExpirations, int addedAssignments, int notAddedAssignments, int errorsAddingAssignments, int addedTodaysAssignments, int errorsAddingTodaysAssignments)
        {
            string addedAssignmentsMsg = BuildSingularOrPluralMessage("assignment", addedAssignments);
            string notAddedAssignmentsMsg = BuildSingularOrPluralMessage("assignment", notAddedAssignments);
            string errorsAddingAssignmentsMsg = BuildSingularOrPluralMessage("assignment", errorsAddingAssignments);

            string addedExpirationsMsg = BuildSingularOrPluralMessage("expiration", addedExpirations);
            string notAddedExpirationsMsg = BuildSingularOrPluralMessage("expiration", notAddedExpirations);
            string errorsAddingExpirationsMsg = BuildSingularOrPluralMessage("expiration", errorsAddingExpirations);

            string addedTodaysAssignmentsMsg = BuildSingularOrPluralMessage("assignment", addedTodaysAssignments);
            string errorsAddingTodaysAssignmentsMsg = BuildSingularOrPluralMessage("assignment", errorsAddingTodaysAssignments);

            string message = BuildExpirationAndAssignmentLine("Added {0}.\n", addedAssignments, addedExpirations, addedTodaysAssignments, addedAssignmentsMsg, addedExpirationsMsg, addedTodaysAssignmentsMsg);
            message += BuildExpirationAndAssignmentLine("Did not add {0} because they might be duplicates or because the options have expired.\n", notAddedAssignments, notAddedExpirations, 0, notAddedAssignmentsMsg, notAddedExpirationsMsg, String.Empty);
            message += BuildExpirationAndAssignmentLine("Did not add {0} because of errors (see Info log).", errorsAddingAssignments, errorsAddingExpirations, 0, errorsAddingAssignmentsMsg, errorsAddingExpirationsMsg, errorsAddingTodaysAssignmentsMsg);

            return String.IsNullOrEmpty(message) ? "No assignments or expirations found" : message;
        }

        private static string BuildExpirationAndAssignmentLine(string message, int assignments, int expirations, int todaysAssignments, string assignmentsMsg, string expirationsMsg, string todaysAssignmentsMsg)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (assignments > 0)
            {
                strBuilder.Append(assignmentsMsg + " for yesterday");
                if ((expirations > 0) && (todaysAssignments > 0))
                    strBuilder.Append(", ");
                else if ((expirations > 0) || (todaysAssignments > 0))
                    strBuilder.Append(" and ");
            }
            if (todaysAssignments > 0)
            {
                strBuilder.Append(todaysAssignmentsMsg + " for today");
                if ((expirations > 0) && (assignments > 0))
                    strBuilder.Append(", and ");
                else if (expirations > 0)
                    strBuilder.Append(" and ");
            }
            if (expirations > 0)
            {
                strBuilder.Append(expirationsMsg);
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
        private static void AddOptionAssignmentsToHugo(ReconciliationLib.HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades, out int addedAssignments, out int notAddedAssignments, out int errorsAddingAssignments)
        {
            addedAssignments = 0;
            notAddedAssignments = 0;
            errorsAddingAssignments = 0;
            foreach (ReconciliationLib.HugoDataSet.MerrillOptionTradesRow row in merrillOptionTrades.Rows)
            {
                if (ReconciliationLib.Utilities.IsAssignment(row.TradeMediumName) && !ReconciliationLib.Utilities.IsCancelOrCorrection(row.CancelCode))
                {
                    switch (ReconciliationLib.Utilities.AddOptionAssignment(new OptionTrade(row)))
                    {
                        case 0:
                            addedAssignments++;
                            break;
                        case 1:
                            notAddedAssignments++;
                            break;
                        default:
                            errorsAddingAssignments++;
                            break;
                    }
                }
            }
        }

        private static void AddOptionExpirationsToHugo(ReconciliationLib.HugoDataSet.MerrillOptionTradesDataTable merrillOptionTrades, out int addedExpirations, out int notAddedExpirations, out int errorsAddingExpirations)
        {

            addedExpirations = 0;
            notAddedExpirations = 0;
            errorsAddingExpirations = 0;
            foreach (ReconciliationLib.HugoDataSet.MerrillOptionTradesRow row in merrillOptionTrades.Rows)
            {
                if (ReconciliationLib.Utilities.IsExpiration(row.TradeMediumName) && !ReconciliationLib.Utilities.IsCancelOrCorrection(row.CancelCode))
                {
                    switch (ReconciliationLib.Utilities.AddOptionExpiration(new OptionTrade(row)))
                    {
                        case 0:
                            addedExpirations++;
                            break;
                        case 1:
                            notAddedExpirations++;
                            break;
                        default:
                            errorsAddingExpirations++;
                            break;
                    }
                }
            }
        }

        private static bool ImportFiles(CommandLineParameters parms)
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
            int insertedTrades = 0;
            bool bResult = true;

            try
            {
                // see if we have already done any imports today
                int numPositions, numConfirmations, numTaxlots, numBookkeepingEntries, numDividends;
                ReconciliationLib.Utilities.CheckImportState(out numPositions, out numConfirmations, out numTaxlots, out numBookkeepingEntries, out numDividends, parms.ClearingHouse);

                // decide what tasks we need to do based on state and user input
                bool bDeletePositions = false;
                bool bInsertPositions = !String.IsNullOrEmpty(parms.PositionFileName);
                bool bDeleteConfirmations = false;
                bool bInsertConfirmations = !String.IsNullOrEmpty(parms.ConfirmationFileName);
                bool bDeleteTaxlots = false;
                bool bInsertTaxlots = !String.IsNullOrEmpty(parms.TaxlotsFileName);
                bool bDeleteDividends = false;
                bool bInsertDividends = !String.IsNullOrEmpty(parms.DividendsFileName);
                bool bInsertTrades = !String.IsNullOrEmpty(parms.TradeFileName);
                bool bDeleteBookkeeping = false;
                bool bInsertBookkeeping = !String.IsNullOrEmpty(parms.BookkeepingFileName);
 
                if (bInsertPositions && (numPositions != 0))
                {
                    HandleImportState(parms.Overwrite, ref bDeletePositions, ref bInsertPositions, "position");
                }
                if (bInsertConfirmations && (numConfirmations != 0))
                {
                    HandleImportState(parms.Overwrite, ref bDeleteConfirmations, ref bInsertConfirmations, "confirmation");
                }
                if (bInsertTaxlots && (numTaxlots != 0))
                {
                    HandleImportState(parms.Overwrite, ref bDeleteTaxlots, ref bInsertTaxlots, "taxlots");
                }
                if (bInsertBookkeeping && (numBookkeepingEntries != 0))
                {
                    HandleImportState(parms.Overwrite, ref bDeleteBookkeeping, ref bInsertBookkeeping, "bookkeeping");
                }
                if (bInsertDividends && (numDividends != 0))
                {
                    HandleImportState(parms.Overwrite, ref bDeleteDividends, ref bInsertDividends, "dividend");
                }

                // do tasks we have decided upon
                DoImports(parms, ref deletedPositions, ref insertedPositions, ref deletedConfirmations, ref insertedConfirmations,
                    ref deletedTaxlots, ref insertedTaxlots, ref deletedBookkeeping, ref insertedBookkeeping, ref deletedDividends, ref insertedDividends, ref insertedTrades,
                    bDeletePositions, bInsertPositions, bDeleteConfirmations, bInsertConfirmations,
                    bDeleteTaxlots, bInsertTaxlots, bDeleteBookkeeping, bInsertBookkeeping, bDeleteDividends, bInsertDividends, bInsertTrades);

                ReconciliationLib.Utilities.FixOptionIds(parms.ClearingHouse.ToString());
   //           ReconciliationLib.Utilities.SyncTotalCosts();

            }
            catch (Exception ex)
            {
                s_lastErrorMessage = ex.Message;
                if (s_logger != null)
                    s_logger.Error("Error during import", ex);
                bResult = false;
            }

            ShowImportResults(deletedPositions, insertedPositions, deletedConfirmations, insertedConfirmations,
                deletedTaxlots, insertedTaxlots, deletedDividends, insertedDividends, deletedBookkeeping, insertedBookkeeping, insertedTrades);
            return bResult;
        }

        private static void DoImports(CommandLineParameters parms, ref int deletedPositions, ref int insertedPositions, ref int deletedConfirmations, ref int insertedConfirmations,
            ref int deletedTaxlots, ref int insertedTaxlots, ref int deletedBookkeeping, ref int insertedBookkeeping, ref int deletedDividends, ref int insertedDividends, ref int insertedTrades,
            bool bDeletePositions, bool bInsertPositions, bool bDeleteConfirmations, bool bInsertConfirmations,
             bool bDeleteTaxlots, bool bInsertTaxlots, bool bDeleteBookkeeping, bool bInsertBookkeeping, bool bDeleteDividends, bool bInsertDividends, bool bInsertTrades)
          {
            s_completionMessage = "Imported ";
            if (bDeletePositions)
            {
                deletedPositions = ReconciliationLib.Utilities.DeleteTodaysPositions(parms.ClearingHouse);
            }
            if (bInsertPositions)
            {
                s_softFailOnFileNotFound = IsFullyPaidLendingAccount(parms.PositionFileName, parms.ClearingHouse) || IsFuturesAccount(parms.PositionFileName, parms.ClearingHouse);
                insertedPositions = ReconciliationLib.Utilities.ImportPositions(parms.Directory + "\\" + parms.PositionFileName, parms.ClearingHouse);
                s_completionMessage += String.Format("{0} positions ", insertedPositions);
            }
            if (bDeleteConfirmations)
            {
                deletedConfirmations = ReconciliationLib.Utilities.DeleteTodaysConfirmations(parms.ClearingHouse);
            }
            if (bInsertConfirmations)
            {
                s_softFailOnFileNotFound = true;
                insertedConfirmations = ReconciliationLib.Utilities.ImportConfirmations(parms.Directory + "\\" + parms.ConfirmationFileName, parms.ClearingHouse);
                s_completionMessage += String.Format("{0} confirmations ", insertedConfirmations);
            }
             if (bDeleteTaxlots)
            {
                deletedTaxlots = ReconciliationLib.Utilities.DeleteTodaysTaxlots(parms.ClearingHouse);
            }
            if (bInsertTaxlots)
            {
                s_softFailOnFileNotFound = false;
                insertedTaxlots = ReconciliationLib.Utilities.ImportTaxlots(parms.Directory + "\\" + parms.TaxlotsFileName, parms.ClearingHouse);
                s_completionMessage += String.Format("{0} taxlots ", insertedTaxlots);
            }
            if (bDeleteDividends)
            {
                deletedDividends = ReconciliationLib.Utilities.DeleteTodaysDividends(parms.ClearingHouse);
            }
            if (bInsertDividends)
            {
                s_softFailOnFileNotFound = true;
                //              s_softFailOnFileNotFound = IsFullyPaidLendingAccount(parms.DividendsFileName, parms.ClearingHouse);
                insertedDividends = ReconciliationLib.Utilities.ImportDividends(parms.Directory + "\\" + parms.DividendsFileName, parms.ClearingHouse);
                s_completionMessage += String.Format("{0} dividends ", insertedDividends);
            }
            if (bDeleteBookkeeping)
            {
                deletedBookkeeping = ReconciliationLib.Utilities.DeleteTodaysBookkeeping(parms.ClearingHouse);
            }
            if (bInsertBookkeeping)
            {
                s_softFailOnFileNotFound = false;
                insertedBookkeeping = ReconciliationLib.Utilities.ImportBookkeeping(parms.Directory + "\\" + parms.BookkeepingFileName, parms.ClearingHouse);
                s_completionMessage += String.Format("{0} bookkeeping records ", insertedBookkeeping);
            }
            if (bInsertTrades)
            {
                s_softFailOnFileNotFound = false;
                int? numberTradesInserted = 0;
                int? numberTradesRejected = 0;
                ReconciliationLib.Utilities.ImportTrades(parms.Directory + "\\" + parms.TradeFileName, parms.ClearingHouse, ref numberTradesInserted, ref numberTradesRejected);
                insertedTrades = numberTradesInserted.Value;
                s_completionMessage += String.Format("{0} trades  ", insertedTrades);
                if (numberTradesRejected.Value > 0)
                    s_completionMessage += String.Format("(rejected {0})  ", numberTradesRejected.Value);
            }
            if (s_completionMessage.Length <= 9)
            {
                s_completionMessage += "0 records ";
            }
            if (!String.IsNullOrEmpty(s_lastErrorMessage))
            {
                s_completionMessage = s_lastErrorMessage + "; " + s_completionMessage;
            }

        }

        public static bool IsFullyPaidLendingAccount(string fileName, ClearingHouse clearingHouse)
        {
            if (clearingHouse == ClearingHouse.WellsFargo)
            {
                if (fileName.ToUpper().StartsWith("GAR"))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFuturesAccount(string fileName, ClearingHouse clearingHouse)
        {
            if (clearingHouse == ClearingHouse.WellsFargo)
            {

                int sinkInt;
                string[] splitName = fileName.Split(new char[] { '_' });
                return int.TryParse(splitName[0], out sinkInt);
            }

            return false;
        }


        private static void ShowImportResults(int deletedPositions, int insertedPositions, int deletedConfirmations, int insertedConfirmations,
             int deletedTaxlots, int insertedTaxlots, int deletedDividends, int insertedDividends, int deletedBookkeeping, int insertedBookkeeping, int insertedTrades)
       {
            if (s_logger != null)
            {
                if (deletedPositions > 0)
                {
                    s_logger.Info(String.Format("Deleted {0} position records; ", deletedPositions));
                }
                s_logger.Info(String.Format("Imported {0} position records", insertedPositions));

                if (deletedConfirmations > 0)
                {
                    s_logger.Info(String.Format("Deleted {0} confirmation records; ", deletedConfirmations));
                }
                s_logger.Info(String.Format("Imported {0} confirmation records", insertedConfirmations));

                if (deletedTaxlots > 0)
                {
                    s_logger.Info(String.Format("Deleted {0} taxlots; ", deletedTaxlots));
                }
                if (deletedDividends > 0)
                {
                    s_logger.Info(String.Format("Deleted {0} dividends; ", deletedDividends));
                }
                s_logger.Info(String.Format("Imported {0} dividends", insertedDividends));
                if (deletedBookkeeping > 0)
                {
                    s_logger.Info(String.Format("Deleted {0} bookkeeping records; ", deletedBookkeeping));
                }
                s_logger.Info(String.Format("Imported {0}  bookkeeping records", insertedBookkeeping));
                s_logger.Info(String.Format("Imported {0} taxlots", insertedTaxlots));
                s_logger.Info(String.Format("Imported {0} trades", insertedTrades));
            }
        }

        private static void HandleImportState(bool overwrite, ref bool bDelete, ref bool bInsert, string recordType)
        {
            if (overwrite)
            {
                bDelete = true;
                bInsert = true;
                if (s_logger != null)
                    s_logger.Info(String.Format("Re-importing {0} file", recordType));
            }
            else
            {
                bDelete = false;
                bInsert = false;
                if (s_logger != null)
                    s_logger.Info(String.Format("Skipping {0} import because it was already done", recordType));
            }
        }
    }
}
