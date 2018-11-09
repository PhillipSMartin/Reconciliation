using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReconciliationLib
{
    public class WellsFargoConfirmationRecord : ConfirmationRecord
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
      private string buySideQuantity;
      private string sellSideQuantity;
      private string exchangeCode;
      private string tradedExchange;
      private string futuresCode;
      private string secDescLine1;
      private string instrumentCode;
      private string contractYear;
      private string secTypeCode;
      private string putCall;
      private string strikePrice;
      private string tradePrice;
      private string settlementPrice;
      private string executingBroker;
      private string giveInBroker;
      private string commissionAmount;
      private string commissionCurrency;
      private string clearingFee;
      private string clearingFeeCurrency;
      private string exchangeFee;
      private string exchangeFeeCurrency;
      private string nfaFee;
      private string nfaFeeCurrency;
      private string pitBrokerage;
      private string pitBrokerageCurrency;
      private string executionCharge;
      private string executionChargeCurrency;
      private string dealId;
      private string spreadCode;
      private string commentCode1;
      private string orderOriginator;
      private string multiplicationFactor;
      private string marketValue;
      private string prevMarketValue;
      private string lastTradeDate;
      private string firstNoticeDate;
      private string expirationDate;
      private string settlementDate;
      private string cardNumber;
      private string currencyCode;
      private string valueDate;

    public WellsFargoConfirmationRecord(string recId,
        string firmId,
        string officeNumber,
        string accountNumber,
        string accountType,
        string subAccount,
        string tradeDate,
        string buySellCode,
        string quantity,
        string buySideQuantity,
        string sellSideQuantity,
        string exchangeCode,
        string tradedExchange,
        string futuresCode,
        string secDescLine1,
        string instrumentCode,
        string contractYear,
        string secTypeCode,
        string putCall,
        string strikePrice,
        string tradePrice,
        string settlementPrice,
        string executingBroker,
        string giveInBroker,
        string commissionAmount,
        string commissionCurrency,
        string clearingFee,
        string clearingFeeCurrency,
        string exchangeFee,
        string exchangeFeeCurrency,
        string nfaFee,
        string nfaFeeCurrency,
        string pitBrokerage,
        string pitBrokerageCurrency,
        string executionCharge,
        string executionChargeCurrency,
        string dealId,
        string spreadCode,
        string commentCode1,
        string orderOriginator,
        string multiplicationFactor,
        string marketValue,
        string prevMarketValue,
        string lastTradeDate,
        string firstNoticeDate,
        string expirationDate,
        string settlementDate,
        string cardNumber,
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
            this.buySideQuantity = buySideQuantity;
            this.sellSideQuantity = sellSideQuantity;
            this.exchangeCode = exchangeCode;
            this.tradedExchange = tradedExchange;
            this.futuresCode = futuresCode;
            this.secDescLine1 = secDescLine1;
            this.instrumentCode = instrumentCode;
            this.contractYear = contractYear;
            this.secTypeCode = secTypeCode;
            this.putCall = putCall;
            this.strikePrice = strikePrice;
            this.tradePrice = tradePrice;
            this.settlementPrice = settlementPrice;
            this.executingBroker = executingBroker;
            this.giveInBroker = giveInBroker;
            this.commissionAmount = commissionAmount;
            this.commissionCurrency = commissionCurrency;
            this.clearingFee = clearingFee;
            this.clearingFeeCurrency = clearingFeeCurrency;
            this.exchangeFee = exchangeFee;
            this.exchangeFeeCurrency = exchangeFeeCurrency;
            this.nfaFee = nfaFee;
            this.nfaFeeCurrency = nfaFeeCurrency;
            this.pitBrokerage = pitBrokerage;
            this.pitBrokerageCurrency = pitBrokerageCurrency;
            this.executionCharge = executionCharge;
            this.executionChargeCurrency = executionChargeCurrency;
            this.dealId = dealId;
            this.spreadCode = spreadCode;
            this.commentCode1 = commentCode1;
            this.orderOriginator = orderOriginator;
            this.multiplicationFactor = multiplicationFactor;
            this.marketValue = marketValue;
            this.prevMarketValue = prevMarketValue;
            this.lastTradeDate = lastTradeDate;
            this.firstNoticeDate = firstNoticeDate;
            this.expirationDate = expirationDate;
            this.settlementDate = settlementDate;
            this.cardNumber = cardNumber;
            this.currencyCode = currencyCode;
            this.valueDate = valueDate;
        }
    public string RecId { get { return recId; } }
    public string FirmId { get { return firmId; } }
    public string OfficeNumber { get { return officeNumber; } }
    public string AccountNumber { get { return accountNumber; } }
    public string AccountType { get { return accountType; } }
    public string SubAccount { get { return subAccount; } }
    public DateTime? TradeDate { get { return  ReconciliationConvert.ToDateTime(tradeDate, "tradeDate", "yyyyMMdd"); } }
    public string BuySellCode { get { return buySellCode; } }
    public decimal Quantity { get { return ReconciliationConvert.ToDecimal(quantity, "quantity"); } }
    public decimal BuySideQuantity { get { return ReconciliationConvert.ToDecimal(buySideQuantity, "buySideQuantity"); } }
    public decimal SellSideQuantity { get { return ReconciliationConvert.ToDecimal(sellSideQuantity, "sellSideQuantity"); } }
    public string ExchangeCode { get { return exchangeCode; } }
    public string TradedExchange { get { return tradedExchange; } }
    public string FuturesCode { get { return futuresCode; } }
    public string SecDescLine1 { get { return secDescLine1; } }
    public string InstrumentCode { get { return instrumentCode; } }
    public string ContractYear { get { return contractYear; } }
    public string SecTypeCode { get { return secTypeCode; } }
    public string PutCall { get { return putCall; } }
    public decimal StrikePrice { get { return ReconciliationConvert.ToDecimal(strikePrice, "strikePrice"); } }
    public decimal TradePrice { get { return ReconciliationConvert.ToDecimal(tradePrice, "tradePrice"); } }
    public decimal SettlementPrice { get { return ReconciliationConvert.ToDecimal(settlementPrice, "settlementPrice"); } }
    public string ExecutingBroker { get { return executingBroker; } }
    public string GiveInBroker { get { return giveInBroker; } }
    public decimal CommissionAmount { get { return ReconciliationConvert.ToDecimal(commissionAmount, "commissionAmount"); } }
    public string CommissionCurrency { get { return commissionCurrency; } }
    public decimal ClearingFee { get { return ReconciliationConvert.ToDecimal(clearingFee, "clearingFee"); } }
    public string ClearingFeeCurrency { get { return clearingFeeCurrency; } }
    public decimal ExchangeFee { get { return ReconciliationConvert.ToDecimal(exchangeFee, "exchangeFee"); } }
    public string ExchangeFeeCurrency { get { return exchangeFeeCurrency; } }
    public decimal NFAFee { get { return ReconciliationConvert.ToDecimal(nfaFee, "nfaFee"); } }
    public string NFAFeeCurrency { get { return nfaFeeCurrency; } }
    public decimal PITBrokerage { get { return ReconciliationConvert.ToDecimal(pitBrokerage, "pitBrokerage"); } }
    public string PITBrokerageCurrency { get { return pitBrokerageCurrency; } }
    public decimal ExecutionCharge { get { return ReconciliationConvert.ToDecimal(executionCharge, "executionCharge"); } }
    public string ExecutionChargeCurrency { get { return executionChargeCurrency; } }
    public string DealId { get { return dealId; } }
    public string SpreadCode { get { return spreadCode; } }
    public string CommentCode1 { get { return commentCode1; } }
    public string OrderOriginator { get { return orderOriginator; } }
    public decimal MultiplicationFactor { get { return ReconciliationConvert.ToDecimal(multiplicationFactor, "multiplicationFactor"); } }
    public decimal MarketValue { get { return ReconciliationConvert.ToDecimal(marketValue, "marketValue"); } }
    public decimal PrevMarketValue { get { return ReconciliationConvert.ToDecimal(prevMarketValue, "prevMarketValue"); } }
    public DateTime? LastTradeDate { get { return ReconciliationConvert.ToDateTime(lastTradeDate, "lastTradeDate", "yyyyMMdd"); } }
    public DateTime? FirstNoticeDate { get { return ReconciliationConvert.ToDateTime(firstNoticeDate, "firstNoticeDate", "yyyyMMdd"); } }
    public DateTime? ExpirationDate { get { return ReconciliationConvert.ToDateTime(expirationDate, "expirationDate", "yyyyMMdd"); } }
    public DateTime? SettlementDate { get { return ReconciliationConvert.ToDateTime(settlementDate, "settlementDate", "yyyyMMdd"); } }
    public string CardNumber { get { return cardNumber; } }
    public string CurrencyCode { get { return currencyCode; } }
    public DateTime? ValueDate { get { return ReconciliationConvert.ToDateTime(valueDate, "valueDate", "yyyyMMdd"); } }

         public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "RecId={0}|FirmId={1}|OfficeNumber={2}|AccountNumber={3}|AccountType={4}|" +
                "SubAccount={5}|TradeDate={6}|BuySellCode={7}|Quantity={8}|" +
                "BuySideQuantity={9}|SellSideQuantity={10}|ExchangeCode={11}|TradedExchange={12}|FuturesCode={13}|SecDescLine1={14}|InstrumentCode={15}|ContractYear={16}|SecTypeCode={17}|" +
                "PutCall={18}|StrikePrice={19}|TradePrice={20}|SettlementPrice={21}|ExecutingBroker={22}|GiveInBroker={23}|CommissionAmount={24}|" +
                "CommissionCurrency={25}|ClearingFee={26}|ClearingFeeCurrency={27}|ExchangeFee={28}|ExchangeFeeCurrency={29}|NFAFee={30}|" +
                "NFAFeeCurrency={31}|PITBrokerage={32}|PITBrokerageCurrency={33}|ExecutionCharge={34}|ExecutionChargeCurrency={35}|" +
                "DealId={36}|SpreadCode={37}|CommentCode1={38}|OrderOriginator={39}|MultiplicationFactor={40}|" +
                "MarketValue={41}PrevMarketValue={42}|LastTradeDate={43}|FirstNoticeDate={44}|ExpirationDate={45}|" +
                "SettlementDate={46}|CardNumber={47}|CurrencyCode={48}|ValueDate={49}",
                recId,
                firmId,
                officeNumber,
                accountNumber,
                accountType,
                subAccount,
                tradeDate,
                buySellCode,
                quantity,
                buySideQuantity,
                sellSideQuantity,
                exchangeCode,
                tradedExchange,
                futuresCode,
                secDescLine1,
                instrumentCode,
                contractYear,
                secTypeCode,
                putCall,
                strikePrice,
                tradePrice,
                settlementPrice,
                executingBroker,
                giveInBroker,
                commissionAmount,
                commissionCurrency,
                clearingFee,
                clearingFeeCurrency,
                exchangeFee,
                exchangeFeeCurrency,
                nfaFee,
                nfaFeeCurrency,
                pitBrokerage,
                pitBrokerageCurrency,
                executionCharge,
                executionChargeCurrency,
                dealId,
                spreadCode,
                commentCode1,
                orderOriginator,
                multiplicationFactor,
                marketValue,
                prevMarketValue,
                lastTradeDate,
                firstNoticeDate,
                expirationDate,
                settlementDate,
                cardNumber,
                currencyCode,
                valueDate);
        }

    }
}
