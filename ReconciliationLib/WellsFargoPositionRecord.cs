using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReconciliationLib
{
    public class WellsFargoPositionRecord : PositionRecord
    {
        private string recId;
        private string firmId;
        private string officeNumber;
        private string accountNumber;
        private string accountType;
        private string subAccount;
        private string tradeDate;
        private string buySellCode;
        private string quantity;
        private string exchangeCode;
        private string tradedExchange;
        private string futuresCode;
        private string securityDescription;
        private string instrumentCode;
        private string contractYear;
        private string secTypeCode;
        private string optionIndicator;
        private string putCall;
        private string strikePrice;
        private string tradePrice;
        private string settlementPrice;
        private string dealId;
        private string multiplicationFactor;
        private string marketValue;
        private string lastTradeDate;
        private string firstNoticeDate;
        private string expirationDate;
        private string settlementDate;
        private string cardNumber;
        private string dailyUniqueId;
        private string currencyCode;
        private string valueDate;

        public WellsFargoPositionRecord(string recId,
            string firmId,
            string officeNumber,
            string accountNumber,
            string accountType,
            string subAccount,
            string tradeDate,
            string buySellCode,
            string quantity,
            string exchangeCode,
            string tradedExchange,
            string futuresCode,
            string securityDescription,
            string instrumentCode,
            string contractYear,
            string secTypeCode,
            string optionIndicator,
            string putCall,
            string strikePrice,
            string tradePrice,
            string settlementPrice,
            string dealId,
            string multiplicationFactor,
            string marketValue,
            string lastTradeDate,
            string firstNoticeDate,
            string expirationDate,
            string settlementDate,
            string cardNumber,
            string dailyUniqueId,
            string currencyCode,
            string valueDate)
        {
            this.recId = recId;
            this.firmId = firmId;
            this.officeNumber = officeNumber;
            this.accountNumber = accountNumber;
            this.accountType = accountType;
            this.subAccount = subAccount;
            this.tradeDate = tradeDate;
            this.buySellCode = buySellCode;
            this.quantity = quantity;
            this.exchangeCode = exchangeCode;
            this.tradedExchange = tradedExchange;
            this.futuresCode = futuresCode;
            this.securityDescription = securityDescription;
            this.instrumentCode = instrumentCode;
            this.contractYear = contractYear;
            this.secTypeCode = secTypeCode;
            this.optionIndicator = optionIndicator;
            this.putCall = putCall;
            this.strikePrice = strikePrice;
            this.tradePrice = tradePrice;
            this.settlementPrice = settlementPrice;
            this.dealId = dealId;
            this.multiplicationFactor = multiplicationFactor;
            this.marketValue = marketValue;
            this.lastTradeDate = lastTradeDate;
            this.firstNoticeDate = firstNoticeDate;
            this.expirationDate = expirationDate;
            this.settlementDate = settlementDate;
            this.cardNumber = cardNumber;
            this.dailyUniqueId = dailyUniqueId;
            this.currencyCode = currencyCode;
            this.valueDate = valueDate;
        }
        public string RecId { get { return recId; } }
        public string FirmId { get { return firmId; } }
        public string OfficeNumber { get { return officeNumber; } }
        public string AccountNumber { get { return accountNumber; } }
        public string AccountType { get { return accountType; } }
        public string SubAccount { get { return subAccount; } }
        public DateTime? TradeDate { get { return ReconciliationConvert.ToDateTime(tradeDate, "tradeDate", "yyyyMMdd"); } }
        public string BuySellCode { get { return buySellCode; } }
        public decimal Quantity { get { return ReconciliationConvert.ToDecimal(quantity, "quantity"); } }
        public string ExchangeCode { get { return exchangeCode; } }
        public string TradedExchange { get { return tradedExchange; } }
        public string FuturesCode { get { return futuresCode; } }
        public string SecurityDescription { get { return securityDescription; } }
        public string InstrumentCode { get { return instrumentCode; } }
        public string ContractYear { get { return contractYear; } }
        public string SecTypeCode { get { return secTypeCode; } }
        public string OptionIndicator { get { return optionIndicator; } }
        public string PutCall { get { return putCall; } }
        public decimal StrikePrice { get { return ReconciliationConvert.ToDecimal(strikePrice, "strikePrice"); } }
        public decimal TradePrice { get { return ReconciliationConvert.ToDecimal(tradePrice, "tradePrice"); } }
        public decimal SettlementPrice { get { return ReconciliationConvert.ToDecimal(settlementPrice, "settlementPrice"); } }
        public string DealId { get { return dealId; } }
        public decimal MultiplicationFactor { get { return ReconciliationConvert.ToDecimal(multiplicationFactor, "multiplicationFactor"); } }
        public decimal MarketValue { get { return ReconciliationConvert.ToDecimal(marketValue, "marketValue"); } }
        public DateTime? LastTradeDate { get { return ReconciliationConvert.ToDateTime(lastTradeDate, "lastTradeDate", "yyyyMMdd"); } }
        public DateTime? FirstNoticeDate { get { return ReconciliationConvert.ToDateTime(firstNoticeDate, "firstNoticeDate", "yyyyMMdd"); } }
        public DateTime? ExpirationDate { get { return ReconciliationConvert.ToDateTime(expirationDate, "expirationDate", "yyyyMMdd"); } }
        public DateTime? SettlementDate { get { return ReconciliationConvert.ToDateTime(settlementDate, "settlementDate", "yyyyMMdd"); } }
        public string CardNumber { get { return cardNumber; } }
        public string DailyUniqueId { get { return dailyUniqueId; } }
        public string CurrencyCode { get { return currencyCode; } }
        public DateTime? ValueDate { get { return ReconciliationConvert.ToDateTime(valueDate, "valueDate", "yyyyMMdd"); } }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "RecId={0}|FirmId={1}|OfficeNumber={2}|AccountNumber={3}|AccountType={4}|" +
                "SubAccount={5}|TradeDate={6}|BuySellCode={7}|Quantity={8}|" +
                "ExchangeCode={9}|TradedExchange={10}|FuturesCode={11}|SecurityDescription={12}|InstrumentCode={13}|ContractYear={14}|SecTypeCode={15}|" +
                "OptionIndicator={16}|PutCall={17}|StrikePrice={18}|TradePrice={19}|SettlementPrice={20}|" +
                "DealId={21}|MultiplicationFactor={22}|" +
                "MarketValue={23}LastTradeDate={24}|FirstNoticeDate={25}|ExpirationDate={26}|" +
                "SettlementDate={27}|CardNumber={28}|DailyUniqueId={29}|CurrencyCode={30}|ValueDate={31}",
                recId,
                firmId,
                officeNumber,
                accountNumber,
                accountType,
                subAccount,
                tradeDate,
                buySellCode,
                quantity,
                exchangeCode,
                tradedExchange,
                futuresCode,
                securityDescription,
                instrumentCode,
                contractYear,
                secTypeCode,
                optionIndicator,
                putCall,
                strikePrice,
                tradePrice,
                settlementPrice,
                dealId,
                multiplicationFactor,
                marketValue,
                lastTradeDate,
                firstNoticeDate,
                expirationDate,
                settlementDate,
                cardNumber,
                dailyUniqueId,
                currencyCode,
                valueDate);
        }

    }
}
