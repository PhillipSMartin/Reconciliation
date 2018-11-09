using System;
using System.Data.SqlClient;
using LoggingUtilitiesLib;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ReconciliationLib.HugoDataSetTableAdapters
{
    partial class t_IBPositionsTableAdapter : IPositionTableAdapter
    {
        public t_IBPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IPositionTableAdapter Members

        public int InsertRecord(PositionRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? ImportDate, string AcctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(ImportDate, AcctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_IBPositions");
            }
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }
    partial class t_IBConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_IBConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IConfirmationTableAdapter Members

        public int InsertRecord(ConfirmationRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? ImportDate, string AcctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(ImportDate, AcctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting confirmation records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_IBConfirmations");
            }
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }
    partial class t_IBTransfersTableAdapter : IBookkeepingTableAdapter
    {
        public t_IBTransfersTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IBookkeepingTableAdapter Members

        public int InsertRecord(BookkeepingRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? ImportDate, string AcctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(ImportDate, AcctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting bookkeeping records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_IBTransfers");
            }
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }
    partial class t_IBCorporateActionsTableAdapter : IDividendTableAdapter
    {
        public t_IBCorporateActionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IDividendTableAdapter Members

        public int InsertRecord(DividendRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? ImportDate, string AcctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(ImportDate, AcctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting dividend records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_IBCorporateActions");
            }
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }
    partial class t_ICBCPositionsTableAdapter : IPositionTableAdapter
    {
        public t_ICBCPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IPositionTableAdapter Members

        public int InsertRecord(PositionRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? ImportDate, string AcctGroupName)
        {
            throw new NotImplementedException();
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        public int Update(System.Data.DataRow[] rows)
        {
            throw new NotImplementedException();
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }
    partial class t_ICBCConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_ICBCConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        #region IConfirmationTableAdapter Members

        public int InsertRecord(ConfirmationRecord positionRecord)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            throw new NotImplementedException();
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        public int Update(System.Data.DataRow[] rows)
        {
            throw new NotImplementedException();
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        #endregion
    }

    public partial class TaskNamesTableAdapter
    {
        public TaskNamesTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(int index)
        {
            Utilities.LogSqlCommand(CommandCollection, index);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_TCWTaxlotsTableAdapter : ITaxlotTableAdapter
    {
        public t_TCWTaxlotsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        public int InsertRecord(TaxlotRecord positionRecord)
        {
                 return 0;
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
                return 0;
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_WellsFargoPositionsTableAdapter : IPositionTableAdapter
    {
        public t_WellsFargoPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        public int InsertRecord(PositionRecord positionRecord)
        {
            try
            {
                WellsFargoPositionRecord WellsFargoPositionRecord = positionRecord as WellsFargoPositionRecord;
                int recordsInserted = InsertRecord(ReconciliationLib.Utilities.ImportDate,
                    ReconciliationLib.Utilities.AccountGroupName,
                    WellsFargoPositionRecord.RecId,
                    WellsFargoPositionRecord.FirmId,
                    WellsFargoPositionRecord.OfficeNumber,
                    WellsFargoPositionRecord.AccountNumber,
                    WellsFargoPositionRecord.AccountType,
                    WellsFargoPositionRecord.SubAccount,
                    WellsFargoPositionRecord.TradeDate,
                    WellsFargoPositionRecord.BuySellCode,
                    WellsFargoPositionRecord.Quantity,
                    WellsFargoPositionRecord.ExchangeCode,
                    WellsFargoPositionRecord.TradedExchange,
                    WellsFargoPositionRecord.FuturesCode,
                    WellsFargoPositionRecord.SecurityDescription,
                    WellsFargoPositionRecord.InstrumentCode,
                    WellsFargoPositionRecord.ContractYear,
                    WellsFargoPositionRecord.SecTypeCode,
                    WellsFargoPositionRecord.OptionIndicator,
                    WellsFargoPositionRecord.PutCall,
                    WellsFargoPositionRecord.StrikePrice,
                    WellsFargoPositionRecord.TradePrice,
                    WellsFargoPositionRecord.SettlementPrice,
                    WellsFargoPositionRecord.DealId,
                    WellsFargoPositionRecord.MultiplicationFactor,
                    WellsFargoPositionRecord.MarketValue,
                    WellsFargoPositionRecord.LastTradeDate,
                    WellsFargoPositionRecord.FirstNoticeDate,
                    WellsFargoPositionRecord.ExpirationDate,
                    WellsFargoPositionRecord.SettlementDate,
                    WellsFargoPositionRecord.CardNumber,
                    WellsFargoPositionRecord.DailyUniqueId,
                    WellsFargoPositionRecord.CurrencyCode,
                    WellsFargoPositionRecord.ValueDate,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

                return (recordsInserted > 0) ? recordsInserted : 0;
            }
            catch (Exception e)
            {
                Utilities.Error("Error inserting position record: " + positionRecord.ToString(), e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_insert_WellsFargoPosition");
            }
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_WellsFargoPositions");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_WellsFargoConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_WellsFargoConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(ConfirmationRecord confirmationRecord)
        {
            try
            {
                WellsFargoConfirmationRecord WellsFargoConfirmationRecord = confirmationRecord as WellsFargoConfirmationRecord;
                int recordsInserted = InsertRecord(ReconciliationLib.Utilities.ImportDate,
                    Utilities.AccountGroupName,
                    "O",    // original record
                    WellsFargoConfirmationRecord.RecId,
                    WellsFargoConfirmationRecord.FirmId,
                    WellsFargoConfirmationRecord.OfficeNumber,
                    WellsFargoConfirmationRecord.AccountNumber,
                    WellsFargoConfirmationRecord.AccountType,
                    WellsFargoConfirmationRecord.SubAccount,
                    WellsFargoConfirmationRecord.TradeDate,
                    WellsFargoConfirmationRecord.BuySellCode,
                    WellsFargoConfirmationRecord.Quantity,
                    WellsFargoConfirmationRecord.BuySideQuantity,
                    WellsFargoConfirmationRecord.SellSideQuantity,
                    WellsFargoConfirmationRecord.ExchangeCode,
                    WellsFargoConfirmationRecord.TradedExchange,
                    WellsFargoConfirmationRecord.FuturesCode,
                    WellsFargoConfirmationRecord.SecDescLine1,
                    WellsFargoConfirmationRecord.InstrumentCode,
                    WellsFargoConfirmationRecord.ContractYear,
                    WellsFargoConfirmationRecord.SecTypeCode,
                    WellsFargoConfirmationRecord.PutCall,
                    WellsFargoConfirmationRecord.StrikePrice,
                    WellsFargoConfirmationRecord.TradePrice,
                    WellsFargoConfirmationRecord.SettlementPrice,
                    WellsFargoConfirmationRecord.ExecutingBroker,
                    WellsFargoConfirmationRecord. GiveInBroker,
                    WellsFargoConfirmationRecord. CommissionAmount,
                    WellsFargoConfirmationRecord.CommissionCurrency,
                    WellsFargoConfirmationRecord.ClearingFee,
                    WellsFargoConfirmationRecord.ClearingFeeCurrency,
                    WellsFargoConfirmationRecord.ExchangeFee,
                    WellsFargoConfirmationRecord.ExchangeFeeCurrency,
                    WellsFargoConfirmationRecord.NFAFee,
                    WellsFargoConfirmationRecord.NFAFeeCurrency,
                    WellsFargoConfirmationRecord. PITBrokerage,
                    WellsFargoConfirmationRecord.PITBrokerageCurrency,
                    WellsFargoConfirmationRecord.ExecutionCharge,
                    WellsFargoConfirmationRecord.ExecutionChargeCurrency,
                    WellsFargoConfirmationRecord.DealId,
                    WellsFargoConfirmationRecord.SpreadCode,
                    WellsFargoConfirmationRecord.CommentCode1,
                    WellsFargoConfirmationRecord. OrderOriginator,
                    WellsFargoConfirmationRecord.MultiplicationFactor,
                    WellsFargoConfirmationRecord.MarketValue,
                    WellsFargoConfirmationRecord.PrevMarketValue,
                    WellsFargoConfirmationRecord.LastTradeDate,
                    WellsFargoConfirmationRecord.FirstNoticeDate,
                    WellsFargoConfirmationRecord.ExpirationDate,
                    WellsFargoConfirmationRecord.SettlementDate,
                    WellsFargoConfirmationRecord.CardNumber,
                    WellsFargoConfirmationRecord.CurrencyCode,
                    WellsFargoConfirmationRecord.ValueDate,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return (recordsInserted > 0) ? recordsInserted : 0;
            }
            catch (Exception e)
            {
                Utilities.Error("Error inserting confirmation record: " + confirmationRecord.ToString(), e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_insert_WellsFargoConfirmation");
            }
        }
        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting confirmation records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_WellsFargoConfirmations");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
    public partial class t_MorganStanleyPositionsTableAdapter : IPositionTableAdapter
    {
        public t_MorganStanleyPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public int InsertRecord(PositionRecord positionRecord)
        {
            // not used - use query adapter instead to insert all position records at once via xml
            return 0;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

 
        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_MorganStanleyPositions");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
    public partial class t_MorganStanleyConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_MorganStanleyConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(ConfirmationRecord confirmationRecord)
        {
            // not used - use query adapter instead to insert all confirmation records at once via xml
            return 0;
        }
        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting confirmation records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_MorganStanleyConfirmations");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
    public partial class t_BONYPositionsTableAdapter : IPositionTableAdapter
    {
        public t_BONYPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public int InsertRecord(PositionRecord positionRecord)
        {
            // not used - use query adapter instead to insert all position records at once via xml
            return 0;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }


        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_BONYPositions");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
    public partial class t_BONYConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_BONYConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(ConfirmationRecord confirmationRecord)
        {
            // not used - use query adapter instead to insert all confirmation records at once via xml
            return 0;
        }
        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting confirmation records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_BONYConfirmations");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_PershingAggregatedTaxlotsTableAdapter : ITaxlotTableAdapter
    {
        public t_PershingAggregatedTaxlotsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(TaxlotRecord aggregatedTaxlotRecord)
        {
            try
            {
                PershingAggregatedTaxlotRecord pershingPositionRecord = aggregatedTaxlotRecord as PershingAggregatedTaxlotRecord;
                int recordsInserted = InsertRecord(ReconciliationLib.Utilities.ImportDate,
                    Utilities.AccountGroupName,
                    pershingPositionRecord.SequenceNumber,
                    pershingPositionRecord.AccountNumber,
                    pershingPositionRecord.PortfolioAccountType,
                    pershingPositionRecord.Cusip,
                    pershingPositionRecord.IntroductingBroker,
                    pershingPositionRecord.InvestmentProfessional,
                    pershingPositionRecord.ProcessDate,
                    pershingPositionRecord.ReconciliationBreakIndicator,
                    pershingPositionRecord.Quantity,
                    pershingPositionRecord.CostBasis,
                    pershingPositionRecord.SecuritySymbol,
                    pershingPositionRecord.OptionRoot,
                    pershingPositionRecord.ExpirationDate,
                    pershingPositionRecord.CallPutIndicator,
                    pershingPositionRecord.StrikePrice,
                    pershingPositionRecord.DateOfData,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return (recordsInserted > 0) ? recordsInserted : 0;
            }
            catch (Exception e)
            {
                Utilities.Error("Error inserting AggregatedTaxlot record: " + aggregatedTaxlotRecord.ToString(), e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_insert_PershingAggregatedTaxlot");
            }
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting aggregatedTaxlot records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_PershingAggregatedTaxlots");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
    public partial class t_PershingPositionsTableAdapter : IPositionTableAdapter
    {
        public t_PershingPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(PositionRecord positionRecord)
        {
            try
            {
                PershingPositionRecord pershingPositionRecord = positionRecord as PershingPositionRecord;
                int recordsInserted = InsertRecord(ReconciliationLib.Utilities.ImportDate,
                    Utilities.AccountGroupName,
                    pershingPositionRecord.SequenceNumber,
                    pershingPositionRecord.AccountNumber,
                    pershingPositionRecord.Cusip,
                    pershingPositionRecord.UnderlyingCusip,
                    pershingPositionRecord.InvestmentProfessional,
                    pershingPositionRecord.IntroductingBroker,
                    pershingPositionRecord.CurrencySecurityIndicator,
                    pershingPositionRecord.IssueCurrency,
                    pershingPositionRecord.TDQuantityUpdateDate,
                    pershingPositionRecord.SDQuantityUpdateDate,
                    pershingPositionRecord.TDQuantity,
                    pershingPositionRecord.SDQuantity,
                    pershingPositionRecord.SEGQuantity,
                    pershingPositionRecord.SafekeepingQuantity,
                    pershingPositionRecord.TransferQuantity,
                    pershingPositionRecord.PendingTransferQuantity,
                    pershingPositionRecord.LegalTransferQuantity,
                    pershingPositionRecord.TenderedQuantity,
                    pershingPositionRecord.PendingPapersQuantity,
                    pershingPositionRecord.ShortAgainstBoxQuantity,
                    pershingPositionRecord.NetworkedQuantity,
                    pershingPositionRecord.PendingSplitQuantity,
                    pershingPositionRecord.CoveredQuantity,
                    pershingPositionRecord.TDQuantityBought,
                    pershingPositionRecord.TDQuantitySold,
                    pershingPositionRecord.RegTRequirement,
                    pershingPositionRecord.HouseMarginRequirement,
                    pershingPositionRecord.ExchangeRequirement,
                    pershingPositionRecord.EquityRequirement,
                    pershingPositionRecord.SecuritySymbol,
                    pershingPositionRecord.SecurityType,
                    pershingPositionRecord.SecurityMod,
                    pershingPositionRecord.SecurityCalc,
                    pershingPositionRecord.MinorProductCode,
                    pershingPositionRecord.NetworkEligibilityIndicator,
                    pershingPositionRecord.ContractSize,
                    pershingPositionRecord.ConversionRatio,
                    pershingPositionRecord.AccountShortName,
                    pershingPositionRecord.StateCode,
                    pershingPositionRecord.CountryCode,
                    pershingPositionRecord.NumberofDescriptionLines,
                    pershingPositionRecord.DescriptionLine1,
                    pershingPositionRecord.DescriptionLine2,
                    pershingPositionRecord.DescriptionLine3,
                    pershingPositionRecord.DescriptionLine4,
                    pershingPositionRecord.DescriptionLine5,
                    pershingPositionRecord.DescriptionLine6,
                    pershingPositionRecord.DividendOption,
                    pershingPositionRecord.LTCapialGainsOption,
                    pershingPositionRecord.STCapialGainsOption,
                    pershingPositionRecord.FirmTradingIndicator,
                    pershingPositionRecord.PositionCurrency,
                    pershingPositionRecord.TDLiquidatingValue,
                    pershingPositionRecord.PoolFactor,
                    pershingPositionRecord.ExchangeRate,
                    pershingPositionRecord.SDLiquidatingValue,
                    pershingPositionRecord.AlternateSecurityIdType,
                    pershingPositionRecord.AlternateSecurityId,
                    pershingPositionRecord.StructureProductIndicator,
                    pershingPositionRecord.FullyPaidLendingQuantity,
                    pershingPositionRecord.FullyPaidLendingQuantityCollateralAmount,
                    pershingPositionRecord.OptionRoot,
                    pershingPositionRecord.ExpirationDate,
                    pershingPositionRecord.CallPutIndicator,
                    pershingPositionRecord.StrikePrice,
                    pershingPositionRecord.TDRepoQuantity,
                    pershingPositionRecord.SDRepoQuantity,
                    pershingPositionRecord.TDReverseRepoQuantity,
                    pershingPositionRecord.SDReverseRepoQuantity,
                    pershingPositionRecord.CollateralPledgeQuantity,
                    pershingPositionRecord.CorporateExecutiveServicesCollateralPledgeQuantity,
                    pershingPositionRecord.TDRepoLiquidatingValue,
                    pershingPositionRecord.SDRepoLiquidatingValue,
                    pershingPositionRecord.TDReverseRepoLiquidatingValue,
                    pershingPositionRecord.SDReverseRepoLiquidatingValue,
                    pershingPositionRecord.CollateralPledgeLiquidatingValue,
                    pershingPositionRecord.CorporateExecutiveServicesCollateralPledgeLiquidatingValue,
                    pershingPositionRecord.TDRepoLoanAmount,
                    pershingPositionRecord.SDRepoLoanAmount,
                    pershingPositionRecord.TDReverseRepoLoanAmount,
                    pershingPositionRecord.SDReverseRepoLoanAmount,
                    pershingPositionRecord.AccruedInterestValue,
                    pershingPositionRecord.DividendorCouponRate,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return (recordsInserted > 0) ? recordsInserted : 0;
            }
            catch (Exception e)
            {
                Utilities.Error("Error inserting position record: " + positionRecord.ToString(), e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_insert_PershingPosition");
            }
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_PershingPositions");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_PershingConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_PershingConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(ConfirmationRecord positionRecord)
        {
            try
            {
                PershingConfirmationRecord pershingConfirmationRecord = positionRecord as PershingConfirmationRecord;
                int recordsInserted = InsertRecord(ReconciliationLib.Utilities.ImportDate,
                    Utilities.AccountGroupName,
                    "O",    // original record
                    pershingConfirmationRecord.SequenceNumber,
                    pershingConfirmationRecord.AccountNumber,
                    pershingConfirmationRecord.Cusip,
                    pershingConfirmationRecord.UnderlyingCusip,
                    pershingConfirmationRecord.SecuritySymbol,
                    pershingConfirmationRecord.InvestmentProfessional,
                    pershingConfirmationRecord.ExecutingInvestmentProfessional,
                    pershingConfirmationRecord.TransactionType,
                    pershingConfirmationRecord.BuySellCode,
                    pershingConfirmationRecord.OpenCloseIndicator,
                    pershingConfirmationRecord.ParKeyCode,
                    pershingConfirmationRecord.SourceCode,
                    pershingConfirmationRecord.MaxxKeyCode,
                    pershingConfirmationRecord.ProcessDate,
                    pershingConfirmationRecord.TradeDate,
                    pershingConfirmationRecord.SettlementEntryDate,
                    pershingConfirmationRecord.SourceofInput,
                    pershingConfirmationRecord.ReferenceNumber,
                    pershingConfirmationRecord.BatchCode,
                    pershingConfirmationRecord.SameDaySettlement,
                    pershingConfirmationRecord.ContraAccount,
                    pershingConfirmationRecord.MarketCodeA,
                    pershingConfirmationRecord.BlotterCode,
                    pershingConfirmationRecord.CancelCode,
                    pershingConfirmationRecord.CorrectionCode,
                    pershingConfirmationRecord.MarketLimitOrderIndicator,
                    pershingConfirmationRecord.LegendCode1,
                    pershingConfirmationRecord.LegendCode2,
                    pershingConfirmationRecord.SLSActivityIndicator,
                    pershingConfirmationRecord.Quantity,
                    pershingConfirmationRecord.PriceinTradeCurrency,
                    pershingConfirmationRecord.CurrencyIndicatorforPrice,
                    pershingConfirmationRecord.NetAmountofTransaction,
                    pershingConfirmationRecord.Principal,
                    pershingConfirmationRecord.Interest,
                    pershingConfirmationRecord.Commission,
                    pershingConfirmationRecord.Tax,
                    pershingConfirmationRecord.TransactionFee,
                    pershingConfirmationRecord.MiscFee,
                    pershingConfirmationRecord.OtherFee,
                    pershingConfirmationRecord.TefraWithholdingAmount,
                    pershingConfirmationRecord.PershingCharge,
                    pershingConfirmationRecord.BrokerageCharge,
                    pershingConfirmationRecord.SalesCredit,
                    pershingConfirmationRecord.SettlementFee,
                    pershingConfirmationRecord.ServiceCharge,
                    pershingConfirmationRecord.MarkupMarkdownAmount,
                    pershingConfirmationRecord.DividendPayableDate,
                    pershingConfirmationRecord.DividendRecordDate,
                    pershingConfirmationRecord.DividendType,
                    pershingConfirmationRecord.DividendReinvestmentIndicator,
                    pershingConfirmationRecord.SharesorRecordQuantityforDividends,
                    pershingConfirmationRecord.OrderSizeQuantity,
                    pershingConfirmationRecord.PoolFactor,
                    pershingConfirmationRecord.AssociatedCustomerAccountNumber,
                    pershingConfirmationRecord.IntroducingBrokerDealer,
                    pershingConfirmationRecord.SecurityType,
                    pershingConfirmationRecord.SecurityMod,
                    pershingConfirmationRecord.SecurityCalc,
                    pershingConfirmationRecord.MinorProductCode,
                    pershingConfirmationRecord.ForeignProductIndicator,
                    pershingConfirmationRecord.WithDueBillIndicator,
                    pershingConfirmationRecord.TaxableMunicipalBondIndicator,
                    pershingConfirmationRecord.OmnibusIndicator,
                    pershingConfirmationRecord.ExternalOrderId,
                    pershingConfirmationRecord.MarketValueofTransaction,
                    pershingConfirmationRecord.IPNumber,
                    pershingConfirmationRecord.ReportedPrice,
                    pershingConfirmationRecord.PreviousDayMarketValueofTransaction,
                    pershingConfirmationRecord.PriceinUSDE,
                    pershingConfirmationRecord.OptionRoot,
                    pershingConfirmationRecord.ExpirationDate,
                    pershingConfirmationRecord.CallPutIndicator,
                    pershingConfirmationRecord.StrikePrice,
                    pershingConfirmationRecord.RepoIndicator,
                    pershingConfirmationRecord.SecurityCurrency,
                    pershingConfirmationRecord.TradeCurrency,
                    pershingConfirmationRecord.SettlementCurrencyCode,
                    pershingConfirmationRecord.SettlementUSDCurrencyFXRate,
                    pershingConfirmationRecord.SettlementUSDMultiplyDivideCode,
                    pershingConfirmationRecord.CrossCurrencyFXRate,
                    pershingConfirmationRecord.CurrencyMultiplyDivideCode,
                    pershingConfirmationRecord.AccruedInterestinSettlementCurrency,
                    pershingConfirmationRecord.MarketCodeB,
                    pershingConfirmationRecord.InternalReferenceForGloss,
                    pershingConfirmationRecord.IntroducingBrokerDealerVersion,
                    pershingConfirmationRecord.NetAmountinSettlementCurrency,
                    pershingConfirmationRecord.PrincipalAmountinSettlementCurrency,
                    pershingConfirmationRecord.InterestinSettlementCurrency,
                    pershingConfirmationRecord.CommissioninSettlementCurrency,
                    pershingConfirmationRecord.TaxinSettlementCurrency,
                    pershingConfirmationRecord.TransactionFeeinSettlementCurrency,
                    pershingConfirmationRecord.MiscFeeinSettlementCurrency,
                    pershingConfirmationRecord.OtherFeeinSettlementCurrency,
                    pershingConfirmationRecord.SalesCreditinSettlementCurrency,
                    pershingConfirmationRecord.SettlementFeeinSettlementCurrency,
                    pershingConfirmationRecord.ServiceChargeinSettlementCurrency,
                    pershingConfirmationRecord.MarkupMarkdowninSettlementCurrency,
                    pershingConfirmationRecord.GlobalExchange,
                    pershingConfirmationRecord.NumberofDescriptionLines,
                    pershingConfirmationRecord.LastDescriptionLine,
                    pershingConfirmationRecord.DescriptionLine1,
                    pershingConfirmationRecord.DescriptionLine2,
                    pershingConfirmationRecord.DescriptionLine3,
                    pershingConfirmationRecord.DescriptionLine4,
                    pershingConfirmationRecord.DescriptionLine5,
                    pershingConfirmationRecord.DescriptionLine6,
                    pershingConfirmationRecord.DescriptionLine7,
                    pershingConfirmationRecord.DescriptionLine8,
                    pershingConfirmationRecord.DescriptionLine9,
                    pershingConfirmationRecord.DescriptionLine10,
                    pershingConfirmationRecord.DescriptionLine11,
                    pershingConfirmationRecord.DescriptionLine12,
                    pershingConfirmationRecord.SecurityCurrencyIndicator,
                    pershingConfirmationRecord.MarketMneumonicCode,
                    pershingConfirmationRecord.CurrencyofIssuance,
                    pershingConfirmationRecord.CurrencyofIssuanceMuliplyDivideCode,
                    pershingConfirmationRecord.AlternateSecurityIdType,
                    pershingConfirmationRecord.AlternateSecurityId,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return (recordsInserted > 0) ? recordsInserted : 0;
            }
            catch (Exception e)
            {
                Utilities.Error("Error inserting position record: " + positionRecord.ToString(), e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_insert_PershingConfirmation");
            }
        }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_PershingConfirmations");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

     public partial class t_LiquidPositionsTableAdapter : IPositionTableAdapter
    {
        public t_LiquidPositionsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

        public int InsertRecord(PositionRecord positionRecord)
        {
            return 0;
         }

        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting position records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_LiquidPositions");
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    public partial class t_LiquidConfirmationsTableAdapter : IConfirmationTableAdapter
    {
        public t_LiquidConfirmationsTableAdapter(SqlConnection sqlConnection)
            : this()
        {
            Connection = sqlConnection;
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        public void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public int InsertRecord(ConfirmationRecord confirmationRecord)
        {
            return 0;
         }
        public int DeleteRecords(DateTime? importDate, string acctGroupName)
        {
            try
            {
                return this.DeleteByImportDateAcctGroupName(importDate, acctGroupName);
            }
            catch (Exception e)
            {
                Utilities.Error("Error deleting confirmation records", e);
                return 0;
            }
            finally
            {
                Utilities.LogSqlCommand(CommandCollection, "Reconciliation.p_delete_LiquidConfirmations");
            }
        }
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }

    partial class QueriesTableAdapter
    {
        private global::System.Data.SqlClient.SqlConnection _connection;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal global::System.Data.SqlClient.SqlConnection Connection
        {
            get
            {
                if ((this._connection == null))
                {
                    this._connection = Utilities.Connection;
                }
                return this._connection;
            }
            set
            {
                this._connection = value;
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1))
                {
                    if ((this.CommandCollection[i] != null))
                    {
                        ((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[i])).Connection = value;
                    }
                }
            }
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }

        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }

    }

    partial class SymbolMappingsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class SubaccountNamesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class MerrillOptionTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }

    partial class HugoOptionTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }

        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand( CommandCollection, commandText);
        }
    }

    partial class StockPositionsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class OptionPositionsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class FuturesPositionsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class AccountGroupsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class TraderAccountsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }

     partial class TradersTableAdapter
     {
             internal void SetAllCommandTimeouts(int timeOut)
            {
                foreach (System.Data.IDbCommand cmd in CommandCollection)
                {
                    cmd.CommandTimeout = timeOut;
                }
            }
            public void LogCommand()
            {
                Utilities.LogSqlCommand(CommandCollection, 0);
            }
    }
     partial class TradeMediaTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class TradeTypesTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class ExchangesTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class BrokersTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class StockTradeReasonsTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class ClearingHouseFileNamesTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }
     partial class OptionTradeReasonsTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand()
         {
             Utilities.LogSqlCommand(CommandCollection, 0);
         }
     }

     partial class HugoStockCorrectionsTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand(string commandText)
         {
             Utilities.LogSqlCommand(CommandCollection, commandText);
         }
     }
     partial class HugoFuturesCorrectionsTableAdapter
     {
         internal void SetAllCommandTimeouts(int timeOut)
         {
             foreach (System.Data.IDbCommand cmd in CommandCollection)
             {
                 cmd.CommandTimeout = timeOut;
             }
         }
         public void LogCommand(string commandText)
         {
             Utilities.LogSqlCommand(CommandCollection, commandText);
         }
     }
     partial class HugoOptionCorrectionsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
    }
    partial class MerrillStockTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand( CommandCollection, commandText);
        }
    }
    partial class MerrillFuturesTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand(CommandCollection, commandText);
        }
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }
    }
    partial class ConfirmationCorrectionsTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand( CommandCollection, commandText);
        }
    }

    partial class HugoStockTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }

        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand( CommandCollection, commandText);
        }
    }
    partial class HugoFuturesTradesTableAdapter
    {
        internal void SetAllCommandTimeouts(int timeOut)
        {
            foreach (System.Data.IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeOut;
            }
        }
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }

        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }

        public void LogCommand(string commandText)
        {
            Utilities.LogSqlCommand( CommandCollection, commandText);
        }
    }


    partial class ConfirmationCorrectionsTableAdapter
    {
         internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }
    }
    partial class SymbolMappingsTableAdapter
    {
         internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }

        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }
    }
    partial class HugoStockCorrectionsTableAdapter
    {
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }
    }
    partial class HugoFuturesCorrectionsTableAdapter
    {
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }
    }
    partial class HugoOptionCorrectionsTableAdapter
    {
        internal int GetReturnValue(string commandText)
        {
            return LoggingUtilities.GetReturnCode(CommandCollection, commandText);
        }
    }
    partial class OptionPositionsTableAdapter
    {
        internal void SetCommandTimeout(string commandText, int timeOut)
        {
            System.Data.IDbCommand cmd = LoggingUtilities.FindSqlCommand(CommandCollection, commandText);
            cmd.CommandTimeout = timeOut;
        }
    }
}


namespace ReconciliationLib
{


    partial class HugoDataSet
    {
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        partial class t_BONYPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_BONYConfirmationsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class TaskNamesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class AccountGroupsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class BrokersDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class ClearingHouseFileNamesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class ConfirmationCorrectionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class ExchangesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class FuturesPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class HugoFuturesCorrectionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class HugoFuturesTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
    
        partial class HugoOptionCorrectionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class HugoStockCorrectionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class HugoOptionTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class HugoStockTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class MerrillOptionTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class MerrillStockTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class MerrillFuturesTradesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class OptionPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class OptionTradeReasonsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class StockPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class StockTradeReasonsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class SubaccountNamesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class SymbolMappingsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_LiquidConfirmationsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_LiquidPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_PershingAggregatedTaxlotsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_PershingPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_PershingConfirmationsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_TCWTaxlotsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_WellsFargoConfirmationsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_WellsFargoPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_MorganStanleyConfirmationsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class t_MorganStanleyPositionsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class TradeMediaDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class TraderAccountsDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class TradersDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
        partial class TradeTypesDataTable
        {
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }

        partial class ConfirmationCorrectionsDataTable
        {
        }

        public partial class AccountGroupsRow
        {
            public ClearingHouse ClearingHouseEnum
            {
                get
                {
                        return (ClearingHouse)Enum.Parse(typeof(ClearingHouse), this.ClearingHouse);
                }
            }
        }
        private HugoOptionTradesDataTable consolidatedOptionTrades = null;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public HugoOptionTradesDataTable ConsolidatedOptionTrades
        {
            get
            {
                if (consolidatedOptionTrades == null)
                {
                    consolidatedOptionTrades = new HugoOptionTradesDataTable();
                    Tables.Add(consolidatedOptionTrades);
                }
                return consolidatedOptionTrades;
            }
        }

        private HugoOptionTradesDataTable unconsolidatedOptionTrades = null;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public HugoOptionTradesDataTable UnconsolidatedOptionTrades
        {
            get
            {
                if (unconsolidatedOptionTrades == null)
                {
                    unconsolidatedOptionTrades = new HugoOptionTradesDataTable();
                    Tables.Add(unconsolidatedOptionTrades);
                }
                return unconsolidatedOptionTrades;
            }
        }

        private HugoOptionTradesDataTable optionTradesWithMissingStockPrices = null;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public HugoOptionTradesDataTable OptionTradesWithMissingStockPrices
        {
            get
            {
                if (optionTradesWithMissingStockPrices == null)
                {
                    optionTradesWithMissingStockPrices = new HugoOptionTradesDataTable();
                    Tables.Add(optionTradesWithMissingStockPrices);
                }
                return optionTradesWithMissingStockPrices;
            }
        }

        private ConfirmationCorrectionsDataTable confirmationCorrectionsYesterday = null;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public ConfirmationCorrectionsDataTable ConfirmationCorrectionsYesterday
        {
            get
            {
                if (confirmationCorrectionsYesterday == null)
                {
                    confirmationCorrectionsYesterday = new ConfirmationCorrectionsDataTable();
                    Tables.Add(confirmationCorrectionsYesterday);
                }
                return confirmationCorrectionsYesterday;
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public ConfirmationCorrectionsDataTable ConfirmationCorrectionsToday
        {
            get
            {
                return ConfirmationCorrections;
            }
        }

        partial class ConfirmationCorrectionsRow
        {
            public string GetDifferenceMsg(bool useMerrillValues)
            {
                string msg = String.Empty;
                if (((NewUnderlyingSymbol != null) || (NewOptionSymbol != null)) && (RecordType == "Correction"))
                {
                    if (NewOptionSymbol != null)
                    {
                        Utilities.BuildDifferenceMsgToken(NewOptionSymbol, OptionSymbol, "Sym", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewExpirationDate, ExpirationDate, "Exp", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewStrikePrice, StrikePrice, "Strike", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewOptionType, OptionType, "C/P", ref msg);
                    }
                    else
                    {
                        Utilities.BuildDifferenceMsgToken(NewUnderlyingSymbol, UnderlyingSymbol, "Sym", ref msg);
                    }
                    Utilities.BuildDifferenceMsgToken(NewTradeTypeName, TradeTypeName, "B/S", ref msg);
                    Utilities.BuildDifferenceMsgToken(NewTradeVolume, TradeVolume, "Vlme", ref msg);
                    Utilities.BuildDifferenceMsgToken(NewTradePrice, TradePrice, "Price", ref msg);
                    if (Utilities.UsingTaxLots)
                    {
                        Utilities.BuildDifferenceMsgToken(NewTotalCost, TotalCost, "TotalCost", ref msg);
                    }
                    if (useMerrillValues)
                    {
                        Utilities.BuildDifferenceMsgToken(NewAccountNumber, AccountNumber, "Acct", ref msg);
                    }
                    else
                    {
                        Utilities.BuildDifferenceMsgToken(NewSubAcctName, SubAcctName, "SubAcct", ref msg);
                    }
                }

                // include note if we have one
                if (!String.IsNullOrEmpty(Note))
                {
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = Note;
                    }
                    else
                    {
                        msg += " - " + Note;
                    }
                }

                return msg;
            }
        }

        partial class HugoStockCorrectionsRow
        {
            public string DifferenceMsg
            {
                get
                {
                    string msg = String.Empty;
                    if (RecordType == "Update")
                    {
                        Utilities.BuildDifferenceMsgToken(NewStockSymbol, StockSymbol, "Sym", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeTypeName, TradeTypeName, "B/S", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeVolume, TradeVolume, "Vlme", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradePrice, TradePrice, "Price", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewSubAcctName, SubAcctName, "Acct", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeDateTime, TradeDateTime, "Date", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewCommission, Commission, "Comm", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewSECFee, SECFee, "SEC", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTotalCost, TotalCost, "Total", ref msg);

                        if (String.IsNullOrEmpty(msg))
                        {
                            msg = "(Change did not affect position)";
                        }
                    }
                    return msg;
                }
            }

            public string FullMsg
            {
                get
                {
                    string msg;
                    string totalCostMsg = String.Format(" ({0})", TotalCost);
                    string newTotalCostMsg = String.Format(" ({0})", NewTotalCost);
                    if (RecordType == "Update")
                    {
                        msg = String.Format("Old: {0} {1} {2} at {3} for {4}{7}, TradeMedium={5} {6:f}\n",
                              TradeTypeName, TradeVolume, StockSymbol, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, totalCostMsg);
                        msg += String.Format("New: {0} {1} {2} at {3} for {4}{7}, TradeMedium={5} {6:f}",
                            NewTradeTypeName, NewTradeVolume, NewStockSymbol, NewTradePrice, NewSubAcctName, NewTradeMediumName, NewTradeDateTime, newTotalCostMsg);
                    }
                    else
                    {
                        msg = String.Format("{7}: {0} {1} {2} at {3} for {4}{8}, TradeMedium={5} {6:f}",
                          TradeTypeName, TradeVolume, StockSymbol, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, RecordType, totalCostMsg);
                    }
                    return msg;
                }
            }
        }
        partial class HugoFuturesCorrectionsRow
        {
            public string DifferenceMsg
            {
                get
                {
                    string msg = String.Empty;
                    if (RecordType == "Update")
                    {
                        Utilities.BuildDifferenceMsgToken(NewFuturesSymbol, FuturesSymbol, "Sym", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeTypeName, TradeTypeName, "B/S", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeVolume, TradeVolume, "Vlme", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradePrice, TradePrice, "Price", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewSubAcctName, SubAcctName, "Acct", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeDateTime, TradeDateTime, "Date", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewCommission, Commission, "Comm", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewNFAFee, NFAFee, "NFA", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewClearingFee, ClearingFee, "Clearing", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewExchangeFee, ExchangeFee, "Exch", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTotalCost, TotalCost, "TotalCost", ref msg);

                        if (String.IsNullOrEmpty(msg))
                        {
                            msg = "(Change did not affect position)";
                        }
                    }
                    return msg;
                }
            }

            public string FullMsg
            {
                get
                {
                    string msg;
                    string totalCostMsg =  String.Format(" ({0})", TotalCost);
                    string newTotalCostMsg =  String.Format(" ({0})", NewTotalCost);
                    if (RecordType == "Update")
                    {
                        msg = String.Format("Old: {0} {1} {2} at {3} for {4}{7}, TradeMedium={5} {6:f}\n",
                              TradeTypeName, TradeVolume, FuturesSymbol, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, totalCostMsg);
                        msg += String.Format("New: {0} {1} {2} at {3} for {4}{7}, TradeMedium={5} {6:f}",
                            NewTradeTypeName, NewTradeVolume, NewFuturesSymbol, NewTradePrice, NewSubAcctName, NewTradeMediumName, NewTradeDateTime, newTotalCostMsg);
                    }
                    else
                    {
                        msg = String.Format("{7}: {0} {1} {2} at {3} for {4}{8}, TradeMedium={5} {6:f}",
                          TradeTypeName, TradeVolume, FuturesSymbol, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, RecordType, totalCostMsg);
                    }
                    return msg;
                }
            }
        }
        partial class HugoOptionCorrectionsRow
        {
            public string DifferenceMsg
            {
                get
                {
                    string msg = String.Empty;
                    if (RecordType == "Update")
                    {
                        Utilities.BuildDifferenceMsgToken(NewOptionSymbol, OptionSymbol, "Sym", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewExpirationDate, ExpirationDate, "Exp", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewStrikePrice, StrikePrice, "Strike", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewOptionType, OptionType, "C/P", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeTypeName, TradeTypeName, "B/S", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeVolume, TradeVolume, "Vlme", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradePrice, TradePrice, "Price", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewSubAcctName, SubAcctName, "Acct", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTradeDateTime, TradeDateTime, "Date", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewCommission, Commission, "Comm", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewSECFee, SECFee, "SEC", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewORFFee, ORFFee, "ORF", ref msg);
                        Utilities.BuildDifferenceMsgToken(NewTotalCost, TotalCost, "Total", ref msg);

                        if (String.IsNullOrEmpty(msg))
                        {
                            msg = "(Change did not affect position)";
                        }
                    }
                    return msg;
                }
            }

            public string FullMsg
            {
                get
                {
                    string msg;
                    string totalCostMsg = String.Format(" ({0})", TotalCost);
                    string newTotalCostMsg = String.Format(" ({0})", NewTotalCost);
                    if (RecordType == "Update")
                    {
                        msg = String.Format("Old: {0} {1} {2} {3:d} {4} {5}s at {6} for {7}{10}, TradeMedium={8} {9:f}\n",
                              TradeTypeName, TradeVolume, OptionSymbol, ExpirationDate, StrikePrice, OptionType, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, totalCostMsg);
                        msg += String.Format("New: {0} {1} {2} {3:d} {4} {5}s at {6} for {7}{10}, TradeMedium={8} {9:f}",
                            NewTradeTypeName, NewTradeVolume, NewOptionSymbol, NewExpirationDate, NewStrikePrice, NewOptionType, NewTradePrice, NewSubAcctName, NewTradeMediumName, NewTradeDateTime, newTotalCostMsg);
                    }
                    else
                    {
                        msg = String.Format("{10}: {0} {1} {2} {3:d} {4} {5}s at {6} for {7}{11}, TradeMedium={8} {9:f}",
                          TradeTypeName, TradeVolume, OptionSymbol, ExpirationDate, StrikePrice, OptionType, TradePrice, SubAcctName, TradeMediumName, TradeDateTime, RecordType, totalCostMsg);
                    }
                    return msg;
                }
            }
        }
        public partial class StockPositionsRow
        {
            public double AcceptedOrZeroIfNull
            {
                get
                {
                    return Convert.IsDBNull(this[this.tableStockPositions.AcceptedColumn]) ? 0f : Accepted;
                }
            }
            public double PrevAcceptedOrZeroIfNull
            {
                get
                {
                    return Convert.IsDBNull(this[this.tableStockPositions.PrevAcceptedColumn]) ? 0f : Accepted;
                }
            }
            public int AcceptanceAdjustment
            {
                get
                {
                    return Convert.ToInt32(AcceptedOrZeroIfNull - Discrepancy);
                }
            }
        }

        public partial class OptionPositionsRow
        {
            public double AcceptedOrZeroIfNull
            {
                get
                {
                    return Convert.IsDBNull(this[this.tableOptionPositions.AcceptedColumn]) ? 0f : Accepted;
                }
            }
            public double PrevAcceptedOrZeroIfNull
            {
                get
                {
                    return Convert.IsDBNull(this[this.tableOptionPositions.PrevAcceptedColumn]) ? 0f : Accepted;
                }
            }
            public int AcceptanceAdjustment
            {
                get
                {
                    return Convert.ToInt32(AcceptedOrZeroIfNull - Discrepancy);
                }
            }
        }
    }
}
