using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public class TablesUpdatedEventArgs : EventArgs
    {
        private bool stockPositionsTableFilled;
        private bool optionPositionsTableFilled;
        private bool futuresPositionsTableFilled;
        private bool accountGroupsTableFilled;
        private bool hugoStockTradesTableFilled;
        private bool merrillStockTradesTableFilled;
        private bool hugoOptionTradesTableFilled;
        private bool merrillOptionTradesTableFilled;
        private bool hugoFuturesTradesTableFilled;
        private bool merrillFuturesTradesTableFilled;
        private bool subAccountNamesTableFilled;
        private bool tradersTableFilled;
        private bool tradeMediaTableFilled;
        private bool tradeTypesTableFilled;
        private bool exchangesTableFilled;
        private bool brokersTableFilled;
        private bool stockTradeReasonsTableFilled;
        private bool optionTradeReasonsTableFilled;
        private bool confirmationCorrectionsTodayTableFilled;
        private bool confirmationCorrectionsYesterdayTableFilled;
        private bool symbolMappingsTableFilled;
        private bool hugoStockCorrectionsTableFilled;
        private bool hugoOptionCorrectionsTableFilled;
        private bool hugoFuturesCorrectionsTableFilled;
        private bool clearingHouseFileNamesTableFilled;

        public TablesUpdatedEventArgs()
        {
            stockPositionsTableFilled = Utilities.StockPositionsTableFilled;
            optionPositionsTableFilled = Utilities.OptionPositionsTableFilled;
            futuresPositionsTableFilled = Utilities.FuturesPositionsTableFilled;
            accountGroupsTableFilled = Utilities.AccountGroupsTableFilled;
            hugoStockTradesTableFilled = Utilities.HugoStockTradesTableFilled;
            merrillStockTradesTableFilled = Utilities.MerrillStockTradesTableFilled;
            hugoOptionTradesTableFilled = Utilities.HugoOptionTradesTableFilled;
            merrillOptionTradesTableFilled = Utilities.MerrillOptionTradesTableFilled;
            hugoFuturesTradesTableFilled = Utilities.HugoFuturesTradesTableFilled;
            merrillFuturesTradesTableFilled = Utilities.MerrillFuturesTradesTableFilled;
            subAccountNamesTableFilled = Utilities.SubAccountNamesTableFilled;
            tradersTableFilled = Utilities.TradersTableFilled;
            tradeMediaTableFilled = Utilities.TradeMediaTableFilled;
            tradeTypesTableFilled = Utilities.TradeTypesTableFilled;
            exchangesTableFilled = Utilities.ExchangesTableFilled;
            brokersTableFilled = Utilities.BrokersTableFilled;
            stockTradeReasonsTableFilled = Utilities.StockTradeReasonsTableFilled;
            optionTradeReasonsTableFilled = Utilities.OptionTradeReasonsTableFilled;
            confirmationCorrectionsTodayTableFilled = Utilities.ConfirmationCorrectionsTodayTableFilled;
            confirmationCorrectionsYesterdayTableFilled = Utilities.ConfirmationCorrectionsYesterdayTableFilled;
            symbolMappingsTableFilled = Utilities.SymbolMappingsTableFilled;
            hugoStockCorrectionsTableFilled = Utilities.HugoStockCorrectionsTableFilled;
            hugoOptionCorrectionsTableFilled = Utilities.HugoOptionCorrectionsTableFilled;
            hugoFuturesCorrectionsTableFilled = Utilities.HugoFuturesCorrectionsTableFilled;
            clearingHouseFileNamesTableFilled = Utilities.ClearingHouseFileNamesTableFilled;
        }

        public bool StockPositionsTableUpdated { get { return !stockPositionsTableFilled; } }
        public bool OptionPositionsTableUpdated { get { return !optionPositionsTableFilled; } }
        public bool FuturesPositionsTableUpdated { get { return !futuresPositionsTableFilled; } }
        public bool AccountGroupsTableUpdated { get { return !accountGroupsTableFilled; } }
        public bool HugoStockTradesTableUpdated { get { return !hugoStockTradesTableFilled; } }
        public bool MerrillStockTradesTableUpdated { get { return !merrillStockTradesTableFilled; } }
        public bool HugoOptionTradesTableUpdated { get { return !hugoOptionTradesTableFilled; } }
        public bool MerrillOptionTradesTableUpdated { get { return !merrillOptionTradesTableFilled; } }
        public bool HugoFuturesTradesTableUpdated { get { return !hugoFuturesTradesTableFilled; } }
        public bool MerrillFuturesTradesTableUpdated { get { return !merrillFuturesTradesTableFilled; } }
        public bool SubAccountNamesTableUpdated { get { return !subAccountNamesTableFilled; } }
        public bool TradersTableUpdated { get { return !tradersTableFilled; } }
        public bool TradeMediaTableUpdated { get { return !tradeMediaTableFilled; } }
        public bool TradeTypesTableUpdated { get { return !tradeTypesTableFilled; } }
        public bool ExchangesTableUpdated { get { return !exchangesTableFilled; } }
        public bool BrokersTableUpdated { get { return !brokersTableFilled; } }
        public bool StockTradeReasonsTableUpdated { get { return !stockTradeReasonsTableFilled; } }
        public bool OptionTradeReasonsTableUpdated { get { return !optionTradeReasonsTableFilled; } }
        public bool ConfirmationCorrectionsTodayTableUpdated { get { return !confirmationCorrectionsTodayTableFilled; } }
        public bool ConfirmationCorrectionsYesterdayTableUpdated { get { return !confirmationCorrectionsYesterdayTableFilled; } }
         public bool SymbolMappingsTableUpdated { get { return !symbolMappingsTableFilled; } }
        public bool HugoStockCorrectionsTableUpdated { get { return !hugoStockCorrectionsTableFilled; } }
        public bool HugoOptionCorrectionsTableUpdated { get { return !hugoOptionCorrectionsTableFilled; } }
        public bool HugoFuturesCorrectionsTableUpdated { get { return !hugoFuturesCorrectionsTableFilled; } }
        public bool ClearingHouseFileNamesTableUpdated { get { return !clearingHouseFileNamesTableFilled; } }

        public override string ToString()
        {
            string str = "Tables updated:";
            if (StockPositionsTableUpdated)
                str += " StockPositions";
            if (OptionPositionsTableUpdated)
                str += " OptionPositions";
            if (FuturesPositionsTableUpdated)
                str += " FuturesPositions";
            if (AccountGroupsTableUpdated)
                str += " AccountGroups";
            if (HugoStockTradesTableUpdated)
                str += " HugoStockTrades";
            if (MerrillStockTradesTableUpdated)
                str += " MerrillStockTrades";
            if (HugoOptionTradesTableUpdated)
                str += " HugoOptionTrades";
            if (MerrillOptionTradesTableUpdated)
                str += " MerrillOptionTrades";
            if (HugoFuturesTradesTableUpdated)
                str += " HugoFuturesTrades";
            if (MerrillFuturesTradesTableUpdated)
                str += " MerrillFuturesTrades";
            if (SubAccountNamesTableUpdated)
                str += " SubAccountNames";
            if (TradersTableUpdated)
                str += " Traders";
            if (TradeMediaTableUpdated)
                str += " TradeMedia";
            if (TradeTypesTableUpdated)
                str += " TradeTypes";
            if (ExchangesTableUpdated)
                str += " Exchanges";
            if (BrokersTableUpdated)
                str += " Brokers";
            if (StockTradeReasonsTableUpdated)
                str += " StockTradeReasons";
            if (OptionTradeReasonsTableUpdated)
                str += " OptionTradeReasons";
            if (ConfirmationCorrectionsTodayTableUpdated)
                str += " ConfirmationCorrectionsToday";
             if (SymbolMappingsTableUpdated)
                str += " SymbolMappingsTableUpdated";
            if (HugoStockCorrectionsTableUpdated)
                str += " HugoStockCorrectionsTableUpdated";
            if (HugoOptionCorrectionsTableUpdated)
                str += " HugoOptionCorrectionsTableUpdated";
            if (HugoFuturesCorrectionsTableUpdated)
                str += " HugoFuturesCorrectionsTableUpdated";
            if (ClearingHouseFileNamesTableUpdated)
                str += " ClearingHouseFileNamesTableUpdated";
            return str;
        }
    }
}
