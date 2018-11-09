using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ReconciliationLib
{
    public class PershingConfirmationRecord : ConfirmationRecord, IABRecord
    {
        #region Field definitions
        private FixedLengthField<int>           m_SequenceNumberA                       = new FixedLengthField<int>("SequenceNumberA", 3, 8);
		private FixedLengthField<string>        m_AccountNumberA                        = new FixedLengthField<string>("AccountNumberA",11,10);
		private FixedLengthField<string>        m_Cusip                                 = new FixedLengthField<string>("Cusip",21,9);
		private FixedLengthField<string>        m_UnderlyingCusip                       = new FixedLengthField<string>("UnderlyingCusip",34,9);
		private FixedLengthField<string>        m_SecuritySymbol                        = new FixedLengthField<string>("SecuritySymbol",47,16);
		private FixedLengthField<string>        m_InvestmentProfessional                = new FixedLengthField<string>("InvestmentProfessional",63,3);
		private FixedLengthField<string>        m_ExecutingInvestmentProfessional       = new FixedLengthField<string>("ExecutingInvestmentProfessional",66,3);
		private FixedLengthField<string>        m_TransactionType                       = new FixedLengthField<string>("TransactionType",69,1);
		private FixedLengthField<string>        m_BuySellCode                           = new FixedLengthField<string>("BuySellCode",70,1);
		private FixedLengthField<string>        m_OpenCloseIndicator                    = new FixedLengthField<string>("OpenCloseIndicator",71,1);
		private FixedLengthField<string>        m_ParKeyCode                            = new FixedLengthField<string>("ParKeyCode",72,2);
		private FixedLengthField<string>        m_SourceCode                            = new FixedLengthField<string>("SourceCode",74,3);
		private FixedLengthField<int>           m_MaxxKeyCode                           = new FixedLengthField<int>("MaxxKeyCode",77,4);
		private FixedLengthCustomDateField      m_ProcessDate                           = new FixedLengthCustomDateField("ProcessDate",81,8,"yyyyMMdd");
		private FixedLengthCustomDateField      m_TradeDate                             = new FixedLengthCustomDateField("TradeDate",89,8,"yyyyMMdd");
		private FixedLengthCustomDateField      m_SettlementEntryDate                   = new FixedLengthCustomDateField("SettlementEntryDate",97,8,"yyyyMMdd");
		private FixedLengthField<string>        m_SourceofInput                         = new FixedLengthField<string>("SourceofInput",112,2);
		private FixedLengthField<string>        m_ReferenceNumber                       = new FixedLengthField<string>("ReferenceNumber",114,6);
		private FixedLengthField<string>        m_BatchCode                             = new FixedLengthField<string>("BatchCode",120,5);
		private FixedLengthField<string>        m_SameDaySettlement                     = new FixedLengthField<string>("SameDaySettlement",125,1);
		private FixedLengthField<string>        m_ContraAccount                         = new FixedLengthField<string>("ContraAccount",126,10);
		private FixedLengthField<string>        m_MarketCodeA                           = new FixedLengthField<string>("MarketCodeA",136,1);
		private FixedLengthField<string>        m_BlotterCode                           = new FixedLengthField<string>("BlotterCode",137,1);
		private FixedLengthField<string>        m_CancelCode                            = new FixedLengthField<string>("CancelCode",138,1);
		private FixedLengthField<string>        m_CorrectionCode                        = new FixedLengthField<string>("CorrectionCode",139,1);
		private FixedLengthField<string>        m_MarketLimitOrderIndicator             = new FixedLengthField<string>("MarketLimitOrderIndicator",140,1);
		private FixedLengthField<string>        m_LegendCode1                           = new FixedLengthField<string>("LegendCode1",141,1);
		private FixedLengthField<string>        m_LegendCode2                           = new FixedLengthField<string>("LegendCode2",142,1);
		private FixedLengthField<string>        m_SLSActivityIndicator                  = new FixedLengthField<string>("SLSActivityIndicator",144,1);
		private FixedLengthSignedDoubleField    m_Quantity                              = new FixedLengthSignedDoubleField("Quantity",145,18,5,163);
		private FixedLengthSignedDoubleField    m_PriceinTradeCurrency                  = new FixedLengthSignedDoubleField("PriceinTradeCurrency",164,18,9,187);
		private FixedLengthField<string>        m_CurrencyIndicatorforPrice             = new FixedLengthField<string>("CurrencyIndicatorforPrice",188,3);
		private FixedLengthSignedDoubleField    m_NetAmountofTransaction                = new FixedLengthSignedDoubleField("NetAmountofTransaction",191,18,3,209);
		private FixedLengthSignedDoubleField    m_Principal                             = new FixedLengthSignedDoubleField("Principal",210,18,3,228);
		private FixedLengthSignedDoubleField    m_Interest                              = new FixedLengthSignedDoubleField("Interest",229,18,2,247);
		private FixedLengthSignedDoubleField    m_Commission                            = new FixedLengthSignedDoubleField("Commission",248,18,2,266);
		private FixedLengthSignedDoubleField    m_Tax                                   = new FixedLengthSignedDoubleField("Tax",267,18,2,285);
		private FixedLengthSignedDoubleField    m_TransactionFee                        = new FixedLengthSignedDoubleField("TransactionFee",286,18,2,304);
		private FixedLengthSignedDoubleField    m_MiscFee                               = new FixedLengthSignedDoubleField("MiscFee",305,18,2,323);
		private FixedLengthSignedDoubleField    m_OtherFee                              = new FixedLengthSignedDoubleField("OtherFee",324,18,2,342);
		private FixedLengthSignedDoubleField    m_TefraWithholdingAmount                = new FixedLengthSignedDoubleField("TefraWithholdingAmount",343,18,2,361);
		private FixedLengthSignedDoubleField    m_PershingCharge                        = new FixedLengthSignedDoubleField("PershingCharge",362,18,2,380);
		private FixedLengthSignedDoubleField    m_BrokerageCharge                       = new FixedLengthSignedDoubleField("BrokerageCharge",381,18,2,399);
		private FixedLengthSignedDoubleField    m_SalesCredit                           = new FixedLengthSignedDoubleField("SalesCredit",400,18,2,418);
		private FixedLengthSignedDoubleField    m_SettlementFee                         = new FixedLengthSignedDoubleField("SettlementFee",419,18,2,437);
		private FixedLengthSignedDoubleField    m_ServiceCharge                         = new FixedLengthSignedDoubleField("ServiceCharge",438,18,2,456);
		private FixedLengthSignedDoubleField    m_MarkupMarkdownAmount                  = new FixedLengthSignedDoubleField("MarkupMarkdownAmount",457,18,2,475);
		private FixedLengthCustomDateField      m_DividendPayableDate                   = new FixedLengthCustomDateField("DividendPayableDate",477,8,"yyyyMMdd");
		private FixedLengthCustomDateField      m_DividendRecordDate                    = new FixedLengthCustomDateField("DividendRecordDate",486,8,"yyyyMMdd");
		private FixedLengthField<int>           m_DividendType                          = new FixedLengthField<int>("DividendType",494,1);
		private FixedLengthField<string>        m_DividendReinvestmentIndicator         = new FixedLengthField<string>("DividendReinvestmentIndicator",495,1);
		private FixedLengthDoubleField          m_SharesorRecordQuantityforDividends    = new FixedLengthDoubleField("SharesorRecordQuantityforDividends",496,18,5);
		private FixedLengthDoubleField          m_OrderSizeQuantity                     = new FixedLengthDoubleField("OrderSizeQuantity",514,18,5);
		private FixedLengthDoubleField          m_PoolFactor                            = new FixedLengthDoubleField("PoolFactor",533,18,9);
		private FixedLengthField<string>        m_AssociatedCustomerAccountNumber       = new FixedLengthField<string>("AssociatedCustomerAccountNumber",551,10);
		private FixedLengthField<string>        m_IntroducingBrokerDealer               = new FixedLengthField<string>("IntroducingBrokerDealer",561,3);
		private FixedLengthField<string>        m_SecurityType                          = new FixedLengthField<string>("SecurityType",564,1);
		private FixedLengthField<string>        m_SecurityMod                           = new FixedLengthField<string>("SecurityMod",565,1);
		private FixedLengthField<string>        m_SecurityCalc                          = new FixedLengthField<string>("SecurityCalc",566,1);
		private FixedLengthField<string>        m_MinorProductCode                      = new FixedLengthField<string>("MinorProductCode",567,3);
		private FixedLengthField<string>        m_ForeignProductIndicator               = new FixedLengthField<string>("ForeignProductIndicator",570,1);
		private FixedLengthField<string>        m_WithDueBillIndicator                  = new FixedLengthField<string>("WithDueBillIndicator",571,1);
		private FixedLengthField<string>        m_TaxableMunicipalBondIndicator         = new FixedLengthField<string>("TaxableMunicipalBondIndicator",572,1);
		private FixedLengthField<string>        m_OmnibusIndicator                      = new FixedLengthField<string>("OmnibusIndicator",573,1);
		private FixedLengthField<string>        m_ExternalOrderId                       = new FixedLengthField<string>("ExternalOrderId",574,20);
		private FixedLengthDoubleField          m_MarketValueofTransaction              = new FixedLengthDoubleField("MarketValueofTransaction",597,18,2);
		private FixedLengthField<string>        m_IPNumber                              = new FixedLengthField<string>("IPNumber",615,3);
		private FixedLengthSignedDoubleField    m_ReportedPrice                         = new FixedLengthSignedDoubleField("ReportedPrice",618,18,9,636);
		private FixedLengthDoubleField          m_PreviousDayMarketValueofTransaction   = new FixedLengthDoubleField("PreviousDayMarketValueofTransaction",637,18,2);
		private FixedLengthDoubleField          m_PriceinUSDE                           = new FixedLengthDoubleField("PriceinUSDE",655,18,9);
		private FixedLengthField<string>        m_OptionRoot                            = new FixedLengthField<string>("OptionRoot",673,6);
		private FixedLengthCustomDateField      m_ExpirationDate                        = new FixedLengthCustomDateField("ExpirationDate",679,6,"yyMMdd");
		private FixedLengthField<string>        m_CallPutIndicator                      = new FixedLengthField<string>("CallPutIndicator",685,1);
		private FixedLengthDoubleField          m_StrikePrice                           = new FixedLengthDoubleField("StrikePrice",686,8,3);
		private FixedLengthField<string>        m_RepoIndicator                         = new FixedLengthField<string>("RepoIndicator",694,1);

 		private FixedLengthField<int>           m_SequenceNumberB                       = new FixedLengthField<int>("SequenceNumberB",3,8);
		private FixedLengthField<string>        m_AccountNumberB                        = new FixedLengthField<string>("AccountNumberB",11,10);
		private FixedLengthField<string>        m_SecurityCurrency                      = new FixedLengthField<string>("SecurityCurrency",21,3);
		private FixedLengthField<string>        m_TradeCurrency                         = new FixedLengthField<string>("TradeCurrency",24,3);
		private FixedLengthField<string>        m_SettlementCurrencyCode                = new FixedLengthField<string>("SettlementCurrencyCode",27,3);
		private FixedLengthDoubleField          m_SettlementUSDCurrencyFXRate           = new FixedLengthDoubleField("SettlementUSDCurrencyFXRate",30,18,9);
		private FixedLengthField<string>        m_SettlementUSDMultiplyDivideCode       = new FixedLengthField<string>("SettlementUSDMultiplyDivideCode",48,1);
		private FixedLengthDoubleField          m_CrossCurrencyFXRate                   = new FixedLengthDoubleField("CrossCurrencyFXRate",49,18,9);
		private FixedLengthField<string>        m_CurrencyMultiplyDivideCode            = new FixedLengthField<string>("CurrencyMultiplyDivideCode",67,1);
		private FixedLengthDoubleField          m_AccruedInterestinSettlementCurrency   = new FixedLengthDoubleField("AccruedInterestinSettlementCurrency",68,18,2);
		private FixedLengthField<string>        m_MarketCodeB                           = new FixedLengthField<string>("MarketCodeB",87,12);
		private FixedLengthField<string>        m_InternalReferenceForGloss             = new FixedLengthField<string>("InternalReferenceForGloss",99,20);
        private FixedLengthField<string>        m_IntroducingBrokerDealerVersion        = new FixedLengthField<string>("IntroducingBrokerDealerVersion",119,2);
		private FixedLengthSignedDoubleField    m_NetAmountinSettlementCurrency         = new FixedLengthSignedDoubleField("NetAmountinSettlementCurrency",121,18,2,139);
		private FixedLengthSignedDoubleField    m_PrincipalAmountinSettlementCurrency   = new FixedLengthSignedDoubleField("PrincipalAmountinSettlementCurrency",140,18,2,158);
		private FixedLengthSignedDoubleField    m_InterestinSettlementCurrency          = new FixedLengthSignedDoubleField("InterestinSettlementCurrency",159,18,2,177);
		private FixedLengthSignedDoubleField    m_CommissioninSettlementCurrency        = new FixedLengthSignedDoubleField("CommissioninSettlementCurrency",178,18,2,196);
		private FixedLengthSignedDoubleField    m_TaxinSettlementCurrency               = new FixedLengthSignedDoubleField("TaxinSettlementCurrency",197,18,2,215);
		private FixedLengthSignedDoubleField    m_TransactionFeeinSettlementCurrency    = new FixedLengthSignedDoubleField("TransactionFeeinSettlementCurrency",216,18,2,234);
		private FixedLengthSignedDoubleField    m_MiscFeeinSettlementCurrency           = new FixedLengthSignedDoubleField("MiscFeeinSettlementCurrency",235,18,2,253);
		private FixedLengthSignedDoubleField    m_OtherFeeinSettlementCurrency          = new FixedLengthSignedDoubleField("OtherFeeinSettlementCurrency",254,18,2,272);
		private FixedLengthSignedDoubleField    m_SalesCreditinSettlementCurrency       = new FixedLengthSignedDoubleField("SalesCreditinSettlementCurrency",273,18,2,291);
		private FixedLengthSignedDoubleField    m_SettlementFeeinSettlementCurrency     = new FixedLengthSignedDoubleField("SettlementFeeinSettlementCurrency",292,18,2,310);
		private FixedLengthSignedDoubleField    m_ServiceChargeinSettlementCurrency     = new FixedLengthSignedDoubleField("ServiceChargeinSettlementCurrency",311,18,2,329);
		private FixedLengthSignedDoubleField    m_MarkupMarkdowninSettlementCurrency    = new FixedLengthSignedDoubleField("MarkupMarkdowninSettlementCurrency",330,18,2,348);
		private FixedLengthField<string>        m_GlobalExchange                        = new FixedLengthField<string>("GlobalExchange",349,4);
		private FixedLengthField<int>           m_NumberofDescriptionLines              = new FixedLengthField<int>("NumberofDescriptionLines",353,2);
		private FixedLengthField<int>           m_LastDescriptionLine                   = new FixedLengthField<int>("LastDescriptionLine",355,2);
		private FixedLengthField<string>        m_DescriptionLine1                      = new FixedLengthField<string>("DescriptionLine1",357,20);
		private FixedLengthField<string>        m_DescriptionLine2                      = new FixedLengthField<string>("DescriptionLine2",377,20);
		private FixedLengthField<string>        m_DescriptionLine3                      = new FixedLengthField<string>("DescriptionLine3",397,20);
		private FixedLengthField<string>        m_DescriptionLine4                      = new FixedLengthField<string>("DescriptionLine4",417,20);
		private FixedLengthField<string>        m_DescriptionLine5                      = new FixedLengthField<string>("DescriptionLine5",437,20);
		private FixedLengthField<string>        m_DescriptionLine6                      = new FixedLengthField<string>("DescriptionLine6",457,20);
		private FixedLengthField<string>        m_DescriptionLine7                      = new FixedLengthField<string>("DescriptionLine7",477,20);
		private FixedLengthField<string>        m_DescriptionLine8                      = new FixedLengthField<string>("DescriptionLine8",497,20);
		private FixedLengthField<string>        m_DescriptionLine9                      = new FixedLengthField<string>("DescriptionLine9",517,20);
		private FixedLengthField<string>        m_DescriptionLine10                     = new FixedLengthField<string>("DescriptionLine10",537,20);
		private FixedLengthField<string>        m_DescriptionLine11                     = new FixedLengthField<string>("DescriptionLine11",557,20);
		private FixedLengthField<string>        m_DescriptionLine12                     = new FixedLengthField<string>("DescriptionLine12",577,20);
		private FixedLengthField<string>        m_SecurityCurrencyIndicator             = new FixedLengthField<string>("SecurityCurrencyIndicator",597,1);
		private FixedLengthField<string>        m_MarketMneumonicCode                   = new FixedLengthField<string>("MarketMneumonicCode",598,4);
		private FixedLengthDoubleField          m_CurrencyofIssuance                    = new FixedLengthDoubleField("CurrencyofIssuance",602,18,9);
		private FixedLengthField<string>        m_CurrencyofIssuanceMuliplyDivideCode   = new FixedLengthField<string>("CurrencyofIssuanceMuliplyDivideCode",320,1);
		private FixedLengthField<string>        m_AlternateSecurityIdType               = new FixedLengthField<string>("AlternateSecurityIdType",621,1);
		private FixedLengthField<string>        m_AlternateSecurityId                   = new FixedLengthField<string>("AlternateSecurityId",622,12);
        #endregion

        public PershingConfirmationRecord() { }

        public void ReadRecordA(string record)
        {
            m_SequenceNumberA.ExtractValueFromRecord(record);
            m_AccountNumberA.ExtractValueFromRecord(record);
            m_Cusip.ExtractValueFromRecord(record);
            m_UnderlyingCusip.ExtractValueFromRecord(record);
            m_SecuritySymbol.ExtractValueFromRecord(record);
            m_InvestmentProfessional.ExtractValueFromRecord(record);
            m_ExecutingInvestmentProfessional.ExtractValueFromRecord(record);
            m_TransactionType.ExtractValueFromRecord(record);
            m_BuySellCode.ExtractValueFromRecord(record);
            m_OpenCloseIndicator.ExtractValueFromRecord(record);
            m_ParKeyCode.ExtractValueFromRecord(record);
            m_SourceCode.ExtractValueFromRecord(record);
            m_MaxxKeyCode.ExtractValueFromRecord(record);
            m_ProcessDate.ExtractValueFromRecord(record);
            m_TradeDate.ExtractValueFromRecord(record);
            m_SettlementEntryDate.ExtractValueFromRecord(record);
            m_SourceofInput.ExtractValueFromRecord(record);
            m_ReferenceNumber.ExtractValueFromRecord(record);
            m_BatchCode.ExtractValueFromRecord(record);
            m_SameDaySettlement.ExtractValueFromRecord(record);
            m_ContraAccount.ExtractValueFromRecord(record);
            m_MarketCodeA.ExtractValueFromRecord(record);
            m_BlotterCode.ExtractValueFromRecord(record);
            m_CancelCode.ExtractValueFromRecord(record);
            m_CorrectionCode.ExtractValueFromRecord(record);
            m_MarketLimitOrderIndicator.ExtractValueFromRecord(record);
            m_LegendCode1.ExtractValueFromRecord(record);
            m_LegendCode2.ExtractValueFromRecord(record);
            m_SLSActivityIndicator.ExtractValueFromRecord(record);
            m_Quantity.ExtractValueFromRecord(record);
            m_PriceinTradeCurrency.ExtractValueFromRecord(record);
            m_CurrencyIndicatorforPrice.ExtractValueFromRecord(record);
            m_NetAmountofTransaction.ExtractValueFromRecord(record);
            m_Principal.ExtractValueFromRecord(record);
            m_Interest.ExtractValueFromRecord(record);
            m_Commission.ExtractValueFromRecord(record);
            m_Tax.ExtractValueFromRecord(record);
            m_TransactionFee.ExtractValueFromRecord(record);
            m_MiscFee.ExtractValueFromRecord(record);
            m_OtherFee.ExtractValueFromRecord(record);
            m_TefraWithholdingAmount.ExtractValueFromRecord(record);
            m_PershingCharge.ExtractValueFromRecord(record);
            m_BrokerageCharge.ExtractValueFromRecord(record);
            m_SalesCredit.ExtractValueFromRecord(record);
            m_SettlementFee.ExtractValueFromRecord(record);
            m_ServiceCharge.ExtractValueFromRecord(record);
            m_MarkupMarkdownAmount.ExtractValueFromRecord(record);
            m_DividendPayableDate.ExtractValueFromRecord(record);
            m_DividendRecordDate.ExtractValueFromRecord(record);
            m_DividendType.ExtractValueFromRecord(record);
            m_DividendReinvestmentIndicator.ExtractValueFromRecord(record);
            m_SharesorRecordQuantityforDividends.ExtractValueFromRecord(record);
            m_OrderSizeQuantity.ExtractValueFromRecord(record);
            m_PoolFactor.ExtractValueFromRecord(record);
            m_AssociatedCustomerAccountNumber.ExtractValueFromRecord(record);
            m_IntroducingBrokerDealer.ExtractValueFromRecord(record);
            m_SecurityType.ExtractValueFromRecord(record);
            m_SecurityMod.ExtractValueFromRecord(record);
            m_SecurityCalc.ExtractValueFromRecord(record);
            m_MinorProductCode.ExtractValueFromRecord(record);
            m_ForeignProductIndicator.ExtractValueFromRecord(record);
            m_WithDueBillIndicator.ExtractValueFromRecord(record);
            m_TaxableMunicipalBondIndicator.ExtractValueFromRecord(record);
            m_OmnibusIndicator.ExtractValueFromRecord(record);
            m_ExternalOrderId.ExtractValueFromRecord(record);
            m_MarketValueofTransaction.ExtractValueFromRecord(record);
            m_IPNumber.ExtractValueFromRecord(record);
            m_ReportedPrice.ExtractValueFromRecord(record);
            m_PreviousDayMarketValueofTransaction.ExtractValueFromRecord(record);
            m_PriceinUSDE.ExtractValueFromRecord(record);
            m_OptionRoot.ExtractValueFromRecord(record);
            if (IsOption)
            {
                m_ExpirationDate.ExtractValueFromRecord(record);
                m_StrikePrice.ExtractValueFromRecord(record);
            }
            m_CallPutIndicator.ExtractValueFromRecord(record);
            m_RepoIndicator.ExtractValueFromRecord(record);
        }

        public void ReadRecordB(string record)
        {
            m_SequenceNumberB.ExtractValueFromRecord(record);
            m_AccountNumberB.ExtractValueFromRecord(record);
            m_SecurityCurrency.ExtractValueFromRecord(record);
            m_TradeCurrency.ExtractValueFromRecord(record);
            m_SettlementCurrencyCode.ExtractValueFromRecord(record);
            m_SettlementUSDCurrencyFXRate.ExtractValueFromRecord(record);
            m_SettlementUSDMultiplyDivideCode.ExtractValueFromRecord(record);
            m_CrossCurrencyFXRate.ExtractValueFromRecord(record);
            m_CurrencyMultiplyDivideCode.ExtractValueFromRecord(record);
            m_AccruedInterestinSettlementCurrency.ExtractValueFromRecord(record);
            m_MarketCodeB.ExtractValueFromRecord(record);
            m_InternalReferenceForGloss.ExtractValueFromRecord(record);
            m_IntroducingBrokerDealerVersion.ExtractValueFromRecord(record);
            m_NetAmountinSettlementCurrency.ExtractValueFromRecord(record);
            m_PrincipalAmountinSettlementCurrency.ExtractValueFromRecord(record);
            m_InterestinSettlementCurrency.ExtractValueFromRecord(record);
            m_CommissioninSettlementCurrency.ExtractValueFromRecord(record);
            m_TaxinSettlementCurrency.ExtractValueFromRecord(record);
            m_TransactionFeeinSettlementCurrency.ExtractValueFromRecord(record);
            m_MiscFeeinSettlementCurrency.ExtractValueFromRecord(record);
            m_OtherFeeinSettlementCurrency.ExtractValueFromRecord(record);
            m_SalesCreditinSettlementCurrency.ExtractValueFromRecord(record);
            m_SettlementFeeinSettlementCurrency.ExtractValueFromRecord(record);
            m_ServiceChargeinSettlementCurrency.ExtractValueFromRecord(record);
            m_MarkupMarkdowninSettlementCurrency.ExtractValueFromRecord(record);
            m_GlobalExchange.ExtractValueFromRecord(record);
            m_NumberofDescriptionLines.ExtractValueFromRecord(record);
            m_LastDescriptionLine.ExtractValueFromRecord(record);
            m_DescriptionLine1.ExtractValueFromRecord(record);
            m_DescriptionLine2.ExtractValueFromRecord(record);
            m_DescriptionLine3.ExtractValueFromRecord(record);
            m_DescriptionLine4.ExtractValueFromRecord(record);
            m_DescriptionLine5.ExtractValueFromRecord(record);
            m_DescriptionLine6.ExtractValueFromRecord(record);
            m_DescriptionLine7.ExtractValueFromRecord(record);
            m_DescriptionLine8.ExtractValueFromRecord(record);
            m_DescriptionLine9.ExtractValueFromRecord(record);
            m_DescriptionLine10.ExtractValueFromRecord(record);
            m_DescriptionLine11.ExtractValueFromRecord(record);
            m_DescriptionLine12.ExtractValueFromRecord(record);
            m_SecurityCurrencyIndicator.ExtractValueFromRecord(record);
            m_MarketMneumonicCode.ExtractValueFromRecord(record);
            m_CurrencyofIssuance.ExtractValueFromRecord(record);
            m_CurrencyofIssuanceMuliplyDivideCode.ExtractValueFromRecord(record);
            m_AlternateSecurityIdType.ExtractValueFromRecord(record);
            m_AlternateSecurityId.ExtractValueFromRecord(record);
        }
        #region Public Properties
        public int SequenceNumber
        {
            get
            {
                try
                {
                    return m_SequenceNumberB.IsNull ? m_SequenceNumberA.FieldValue : m_SequenceNumberB.FieldValue;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public string AccountNumber { get { return m_AccountNumberB.IsNull ? m_AccountNumberA.FieldValue : m_AccountNumberB.FieldValue; } }
        public string      Cusip                                { get { return m_Cusip.FieldValue; } }
        public string      UnderlyingCusip                      { get { return m_UnderlyingCusip.FieldValue; } }
        public string      SecuritySymbol                       { get { return m_SecuritySymbol.FieldValue; } }
        public string      InvestmentProfessional               { get { return m_InvestmentProfessional.FieldValue; } }
        public string      ExecutingInvestmentProfessional      { get { return m_ExecutingInvestmentProfessional.FieldValue; } }
        public string      TransactionType                      { get { return m_TransactionType.FieldValue; } }
        public string      BuySellCode                          { get { return m_BuySellCode.FieldValue; } }
        public string      OpenCloseIndicator                   { get { return m_OpenCloseIndicator.FieldValue; } }
        public string      ParKeyCode                           { get { return m_ParKeyCode.FieldValue; } }
        public string      SourceCode                           { get { return m_SourceCode.FieldValue; } }
        public int         MaxxKeyCode                          { get { return m_MaxxKeyCode.FieldValue; } }
        public  DateTime    ProcessDate                         { get { return m_ProcessDate.FieldValue; } }
        public  DateTime    TradeDate                           { get { return m_TradeDate.FieldValue; } }
        public  DateTime    SettlementEntryDate                 { get { return m_SettlementEntryDate.FieldValue; } }
        public string      SourceofInput                        { get { return m_SourceofInput.FieldValue; } }
        public string      ReferenceNumber                      { get { return m_ReferenceNumber.FieldValue; } }
        public string      BatchCode                            { get { return m_BatchCode.FieldValue; } }
        public string      SameDaySettlement                    { get { return m_SameDaySettlement.FieldValue; } }
        public string      ContraAccount                        { get { return m_ContraAccount.FieldValue; } }
        public string      MarketCodeA                          { get { return m_MarketCodeA.FieldValue; } }
        public string      BlotterCode                          { get { return m_BlotterCode.FieldValue; } }
        public string      CancelCode                           { get { return m_CancelCode.FieldValue; } }
        public string      CorrectionCode                       { get { return m_CorrectionCode.FieldValue; } }
        public string      MarketLimitOrderIndicator            { get { return m_MarketLimitOrderIndicator.FieldValue; } }
        public string      LegendCode1                          { get { return m_LegendCode1.FieldValue; } }
        public string      LegendCode2                          { get { return m_LegendCode2.FieldValue; } }
        public string      SLSActivityIndicator                 { get { return m_SLSActivityIndicator.FieldValue; } }
        public  double  Quantity                                { get { return m_Quantity.FieldValue; } }
        public  double  PriceinTradeCurrency                    { get { return m_PriceinTradeCurrency.FieldValue; } }
        public string      CurrencyIndicatorforPrice            { get { return m_CurrencyIndicatorforPrice.FieldValue; } }
        public  double  NetAmountofTransaction                  { get { return m_NetAmountofTransaction.FieldValue; } }
        public  double  Principal                               { get { return m_Principal.FieldValue; } }
        public  double  Interest                                { get { return m_Interest.FieldValue; } }
        public  double  Commission                              { get { return m_Commission.FieldValue; } }
        public  double  Tax                                     { get { return m_Tax.FieldValue; } }
        public  double  TransactionFee                          { get { return m_TransactionFee.FieldValue; } }
        public  double  MiscFee                                 { get { return m_MiscFee.FieldValue; } }
        public  double  OtherFee                                { get { return m_OtherFee.FieldValue; } }
        public  double  TefraWithholdingAmount                  { get { return m_TefraWithholdingAmount.FieldValue; } }
        public  double  PershingCharge                          { get { return m_PershingCharge.FieldValue; } }
        public  double  BrokerageCharge                         { get { return m_BrokerageCharge.FieldValue; } }
        public  double  SalesCredit                             { get { return m_SalesCredit.FieldValue; } }
        public  double  SettlementFee                           { get { return m_SettlementFee.FieldValue; } }
        public  double  ServiceCharge                           { get { return m_ServiceCharge.FieldValue; } }
        public  double  MarkupMarkdownAmount                    { get { return m_MarkupMarkdownAmount.FieldValue; } }
        public  DateTime    DividendPayableDate                 { get { return m_DividendPayableDate.FieldValue; } }
        public  DateTime    DividendRecordDate                  { get { return m_DividendRecordDate.FieldValue; } }
        public int         DividendType                         { get { return m_DividendType.FieldValue; } }
        public string      DividendReinvestmentIndicator        { get { return m_DividendReinvestmentIndicator.FieldValue; } }
        public  double     SharesorRecordQuantityforDividends   { get { return m_SharesorRecordQuantityforDividends.FieldValue; } }
        public  double        OrderSizeQuantity                 { get { return m_OrderSizeQuantity.FieldValue; } }
        public  double        PoolFactor                        { get { return m_PoolFactor.FieldValue; } }
        public string      AssociatedCustomerAccountNumber      { get { return m_AssociatedCustomerAccountNumber.FieldValue; } }
        public string      IntroducingBrokerDealer              { get { return m_IntroducingBrokerDealer.FieldValue; } }
        public string      SecurityType                         { get { return m_SecurityType.FieldValue; } }
        public string      SecurityMod                          { get { return m_SecurityMod.FieldValue; } }
        public string      SecurityCalc                         { get { return m_SecurityCalc.FieldValue; } }
        public string      MinorProductCode                     { get { return m_MinorProductCode.FieldValue; } }
        public string      ForeignProductIndicator              { get { return m_ForeignProductIndicator.FieldValue; } }
        public string      WithDueBillIndicator                 { get { return m_WithDueBillIndicator.FieldValue; } }
        public string      TaxableMunicipalBondIndicator        { get { return m_TaxableMunicipalBondIndicator.FieldValue; } }
        public string      OmnibusIndicator                     { get { return m_OmnibusIndicator.FieldValue; } }
        public string      ExternalOrderId                      { get { return m_ExternalOrderId.FieldValue; } }
        public  double        MarketValueofTransaction          { get { return m_MarketValueofTransaction.FieldValue; } }
        public string      IPNumber                             { get { return m_IPNumber.FieldValue; } }
        public  double  ReportedPrice                           { get { return m_ReportedPrice.FieldValue; } }
        public  double   PreviousDayMarketValueofTransaction    { get { return m_PreviousDayMarketValueofTransaction.FieldValue; } }
        public  double        PriceinUSDE                       { get { return m_PriceinUSDE.FieldValue; } }
        public string      OptionRoot                           { get { return m_OptionRoot.FieldValue; } }
        public  DateTime?    ExpirationDate                     
        { 
            get 
            { 
                if (m_ExpirationDate.IsNull)
                    return null;
                else
                    return m_ExpirationDate.FieldValue; 
            } 
        }
        public string      CallPutIndicator                     { get { return m_CallPutIndicator.FieldValue; } }
        public  double        StrikePrice                       { get { return m_StrikePrice.FieldValue; } }
        public string      RepoIndicator                        { get { return m_RepoIndicator.FieldValue; } }

        public string      SecurityCurrency                     { get { return m_SecurityCurrency.FieldValue; } }
        public string      TradeCurrency                        { get { return m_TradeCurrency.FieldValue; } }
        public string      SettlementCurrencyCode               { get { return m_SettlementCurrencyCode.FieldValue; } }
        public  double        SettlementUSDCurrencyFXRate       { get { return m_SettlementUSDCurrencyFXRate.FieldValue; } }
        public string      SettlementUSDMultiplyDivideCode      { get { return m_SettlementUSDMultiplyDivideCode.FieldValue; } }
        public  double        CrossCurrencyFXRate               { get { return m_CrossCurrencyFXRate.FieldValue; } }
        public string      CurrencyMultiplyDivideCode           { get { return m_CurrencyMultiplyDivideCode.FieldValue; } }
        public  double     AccruedInterestinSettlementCurrency   { get { return m_AccruedInterestinSettlementCurrency.FieldValue; } }
        public string      MarketCodeB                          { get { return m_MarketCodeB.FieldValue; } }
        public string      InternalReferenceForGloss            { get { return m_InternalReferenceForGloss.FieldValue; } }
        public string      IntroducingBrokerDealerVersion       { get { return m_IntroducingBrokerDealerVersion.FieldValue; } }
        public  double  NetAmountinSettlementCurrency           { get { return m_NetAmountinSettlementCurrency.FieldValue; } }
        public  double  PrincipalAmountinSettlementCurrency     { get { return m_PrincipalAmountinSettlementCurrency.FieldValue; } }
        public  double  InterestinSettlementCurrency            { get { return m_InterestinSettlementCurrency.FieldValue; } }
        public  double  CommissioninSettlementCurrency          { get { return m_CommissioninSettlementCurrency.FieldValue; } }
        public  double  TaxinSettlementCurrency                 { get { return m_TaxinSettlementCurrency.FieldValue; } }
        public  double  TransactionFeeinSettlementCurrency      { get { return m_TransactionFeeinSettlementCurrency.FieldValue; } }
        public  double  MiscFeeinSettlementCurrency             { get { return m_MiscFeeinSettlementCurrency.FieldValue; } }
        public  double  OtherFeeinSettlementCurrency            { get { return m_OtherFeeinSettlementCurrency.FieldValue; } }
        public  double  SalesCreditinSettlementCurrency         { get { return m_SalesCreditinSettlementCurrency.FieldValue; } }
        public  double  SettlementFeeinSettlementCurrency       { get { return m_SettlementFeeinSettlementCurrency.FieldValue; } }
        public  double  ServiceChargeinSettlementCurrency       { get { return m_ServiceChargeinSettlementCurrency.FieldValue; } }
        public  double  MarkupMarkdowninSettlementCurrency      { get { return m_MarkupMarkdowninSettlementCurrency.FieldValue; } }
        public string      GlobalExchange                       { get { return m_GlobalExchange.FieldValue; } }
        public int         NumberofDescriptionLines             { get { return m_NumberofDescriptionLines.FieldValue; } }
        public int         LastDescriptionLine                  { get { return m_LastDescriptionLine.FieldValue; } }
        public string      DescriptionLine1                     { get { return m_DescriptionLine1.FieldValue; } }
        public string      DescriptionLine2                     { get { return m_DescriptionLine2.FieldValue; } }
        public string      DescriptionLine3                     { get { return m_DescriptionLine3.FieldValue; } }
        public string      DescriptionLine4                     { get { return m_DescriptionLine4.FieldValue; } }
        public string      DescriptionLine5                     { get { return m_DescriptionLine5.FieldValue; } }
        public string      DescriptionLine6                     { get { return m_DescriptionLine6.FieldValue; } }
        public string      DescriptionLine7                     { get { return m_DescriptionLine7.FieldValue; } }
        public string      DescriptionLine8                     { get { return m_DescriptionLine8.FieldValue; } }
        public string      DescriptionLine9                     { get { return m_DescriptionLine9.FieldValue; } }
        public string      DescriptionLine10                    { get { return m_DescriptionLine10.FieldValue; } }
        public string      DescriptionLine11                    { get { return m_DescriptionLine11.FieldValue; } }
        public string      DescriptionLine12                    { get { return m_DescriptionLine12.FieldValue; } }
        public string      SecurityCurrencyIndicator            { get { return m_SecurityCurrencyIndicator.FieldValue; } }
        public string      MarketMneumonicCode                  { get { return m_MarketMneumonicCode.FieldValue; } }
        public  double        CurrencyofIssuance                { get { return m_CurrencyofIssuance.FieldValue; } }
        public string      CurrencyofIssuanceMuliplyDivideCode  { get { return m_CurrencyofIssuanceMuliplyDivideCode.FieldValue; } }
        public string      AlternateSecurityIdType              { get { return m_AlternateSecurityIdType.FieldValue; } }
        public string      AlternateSecurityId                  { get { return m_AlternateSecurityId.FieldValue; } }
        #endregion

        public override string ToString()
        {
            try
            {
                return String.Format(CultureInfo.CurrentCulture, String.Format(
                    "SequenceNumber={0}|" +
                    "AccountNumber={1}|" +
                    "Cusip={2}|" +
                    "UnderlyingCusip={3}|" +
                    "SecuritySymbol={4}|" +
                    "InvestmentProfessional={5}|" +
                    "ExecutingInvestmentProfessional={6}|" +
                    "TransactionType={7}|" +
                    "BuySellCode={8}|" +
                    "OpenCloseIndicator={9}|" +
                    "ParKeyCode={10}|" +
                    "SourceCode={11}|" +
                    "MaxxKeyCode={12}|" +
                    "ProcessDate={13}|" +
                    "TradeDate={14}|" +
                    "SettlementEntryDate={15}|" +
                    "SourceofInput={16}|" +
                    "ReferenceNumber={17}|" +
                    "BatchCode={18}|" +
                    "SameDaySettlement={19}|" +
                    "ContraAccount={20}|" +
                    "MarketCodeA={21}|" +
                    "BlotterCode={22}|" +
                    "CancelCode={23}|" +
                    "CorrectionCode={24}|" +
                    "MarketLimitOrderIndicator={25}|" +
                    "LegendCode1={26}|" +
                    "LegendCode2={27}|" +
                    "SLSActivityIndicator={28}|" +
                    "Quantity={29}|" +
                    "PriceinTradeCurrency={30}|" +
                    "CurrencyIndicatorforPrice={31}|" +
                    "NetAmountofTransaction={32}|" +
                    "Principal={33}|" +
                    "Interest={34}|" +
                    "Commission={35}|" +
                    "Tax={36}|" +
                    "TransactionFee={37}|" +
                    "MiscFee={38}|" +
                    "OtherFee={39}|" +
                    "TefraWithholdingAmount={40}|" +
                    "PershingCharge={41}|" +
                    "BrokerageCharge={42}|" +
                    "SalesCredit={43}|" +
                    "SettlementFee={44}|" +
                    "ServiceCharge={45}|" +
                    "MarkupMarkdownAmount={46}|" +
                    "DividendPayableDate={47}|" +
                    "DividendRecordDate={48}|" +
                    "DividendType={49}|" +
                    "DividendReinvestmentIndicator={50}|" +
                    "SharesorRecordQuantityforDividends={51}|" +
                    "OrderSizeQuantity={52}|" +
                    "PoolFactor={53}|" +
                    "AssociatedCustomerAccountNumber={54}|" +
                    "IntroducingBrokerDealer={55}|" +
                    "SecurityType={56}|" +
                    "SecurityMod={57}|" +
                    "SecurityCalc={58}|" +
                    "MinorProductCode={59}|" +
                    "ForeignProductIndicator={60}|" +
                    "WithDueBillIndicator={61}|" +
                    "TaxableMunicipalBondIndicator={62}|" +
                    "OmnibusIndicator={63}|" +
                    "ExternalOrderId={64}|" +
                    "MarketValueofTransaction={65}|" +
                    "IPNumber={66}|" +
                    "ReportedPrice={67}|" +
                    "PreviousDayMarketValueofTransaction={68}|" +
                    "PriceinUSDE={69}|" +
                    "OptionRoot={70}|" +
                    "ExpirationDate={71}|" +
                    "CallPutIndicator={72}|" +
                    "StrikePrice={73}|" +
                    "RepoIndicator={74}" +
                    "SecurityCurrency={75}|" +
                    "TradeCurrency={76}|" +
                    "SettlementCurrencyCode={77}|" +
                    "SettlementUSDCurrencyFXRate={78}|" +
                    "SettlementUSDMultiplyDivideCode={79}|" +
                    "CrossCurrencyFXRate={80}|" +
                    "CurrencyMultiplyDivideCode={81}|" +
                    "AccruedInterestinSettlementCurrency={82}|" +
                    "MarketCodeB={83}|" +
                    "InternalReferenceForGloss={84}|" +
                    "IntroducingBrokerDealerVersion={85}|" +
                    "NetAmountinSettlementCurrency={86}|" +
                    "PrincipalAmountinSettlementCurrency={87}|" +
                    "InterestinSettlementCurrency={88}|" +
                    "CommissioninSettlementCurrency={89}|" +
                    "TaxinSettlementCurrency={90}|" +
                    "TransactionFeeinSettlementCurrency={91}|" +
                    "MiscFeeinSettlementCurrency={92}|" +
                    "OtherFeeinSettlementCurrency={93}|" +
                    "SalesCreditinSettlementCurrency={94}|" +
                    "SettlementFeeinSettlementCurrency={95}|" +
                    "ServiceChargeinSettlementCurrency={96}|" +
                    "MarkupMarkdowninSettlementCurrency={97}|" +
                    "GlobalExchange={98}|" +
                    "NumberofDescriptionLines={99}|" +
                    "LastDescriptionLine={100}|" +
                    "DescriptionLine1={101}|" +
                    "DescriptionLine2={102}|" +
                    "DescriptionLine3={103}|" +
                    "DescriptionLine4={104}|" +
                    "DescriptionLine5={105}|" +
                    "DescriptionLine6={106}|" +
                    "DescriptionLine7={107}|" +
                    "DescriptionLine8={108}|" +
                    "DescriptionLine9={109}|" +
                    "DescriptionLine10={110}|" +
                    "DescriptionLine11={111}|" +
                    "DescriptionLine12={112}|" +
                    "SecurityCurrencyIndicator={113}|" +
                    "MarketMneumonicCode={114}|" +
                    "CurrencyofIssuance={115}|" +
                    "CurrencyofIssuanceMuliplyDivideCode={116}|" +
                    "AlternateSecurityIdType={117}|" +
                    "AlternateSecurityId={118}",
                    SequenceNumber,
                    AccountNumber,
                    Cusip,
                    UnderlyingCusip,
                    SecuritySymbol,
                    InvestmentProfessional,
                    ExecutingInvestmentProfessional,
                    TransactionType,
                    BuySellCode,
                    OpenCloseIndicator,
                    ParKeyCode,
                    SourceCode,
                    MaxxKeyCode,
                    ProcessDate,
                    TradeDate,
                    SettlementEntryDate,
                    SourceofInput,
                    ReferenceNumber,
                    BatchCode,
                    SameDaySettlement,
                    ContraAccount,
                    MarketCodeA,
                    BlotterCode,
                    CancelCode,
                    CorrectionCode,
                    MarketLimitOrderIndicator,
                    LegendCode1,
                    LegendCode2,
                    SLSActivityIndicator,
                    Quantity,
                    PriceinTradeCurrency,
                    CurrencyIndicatorforPrice,
                    NetAmountofTransaction,
                    Principal,
                    Interest,
                    Commission,
                    Tax,
                    TransactionFee,
                    MiscFee,
                    OtherFee,
                    TefraWithholdingAmount,
                    PershingCharge,
                    BrokerageCharge,
                    SalesCredit,
                    SettlementFee,
                    ServiceCharge,
                    MarkupMarkdownAmount,
                    DividendPayableDate,
                    DividendRecordDate,
                    DividendType,
                    DividendReinvestmentIndicator,
                    SharesorRecordQuantityforDividends,
                    OrderSizeQuantity,
                    PoolFactor,
                    AssociatedCustomerAccountNumber,
                    IntroducingBrokerDealer,
                    SecurityType,
                    SecurityMod,
                    SecurityCalc,
                    MinorProductCode,
                    ForeignProductIndicator,
                    WithDueBillIndicator,
                    TaxableMunicipalBondIndicator,
                    OmnibusIndicator,
                    ExternalOrderId,
                    MarketValueofTransaction,
                    IPNumber,
                    ReportedPrice,
                    PreviousDayMarketValueofTransaction,
                    PriceinUSDE,
                    OptionRoot,
                    ExpirationDate,
                    CallPutIndicator,
                    StrikePrice,
                    RepoIndicator,
                    SecurityCurrency,
                    TradeCurrency,
                    SettlementCurrencyCode,
                    SettlementUSDCurrencyFXRate,
                    SettlementUSDMultiplyDivideCode,
                    CrossCurrencyFXRate,
                    CurrencyMultiplyDivideCode,
                    AccruedInterestinSettlementCurrency,
                    MarketCodeB,
                    InternalReferenceForGloss,
                    IntroducingBrokerDealerVersion,
                    NetAmountinSettlementCurrency,
                    PrincipalAmountinSettlementCurrency,
                    InterestinSettlementCurrency,
                    CommissioninSettlementCurrency,
                    TaxinSettlementCurrency,
                    TransactionFeeinSettlementCurrency,
                    MiscFeeinSettlementCurrency,
                    OtherFeeinSettlementCurrency,
                    SalesCreditinSettlementCurrency,
                    SettlementFeeinSettlementCurrency,
                    ServiceChargeinSettlementCurrency,
                    MarkupMarkdowninSettlementCurrency,
                    GlobalExchange,
                    NumberofDescriptionLines,
                    LastDescriptionLine,
                    DescriptionLine1,
                    DescriptionLine2,
                    DescriptionLine3,
                    DescriptionLine4,
                    DescriptionLine5,
                    DescriptionLine6,
                    DescriptionLine7,
                    DescriptionLine8,
                    DescriptionLine9,
                    DescriptionLine10,
                    DescriptionLine11,
                    DescriptionLine12,
                    SecurityCurrencyIndicator,
                    MarketMneumonicCode,
                    CurrencyofIssuance,
                    CurrencyofIssuanceMuliplyDivideCode,
                    AlternateSecurityIdType,
                    AlternateSecurityId));
            }
            catch
            {
                return String.Format(CultureInfo.CurrentCulture, "PershingConfirmationRecord {0}", SequenceNumber);
            }
        }
        private bool IsOption
        {
            get
            {
                switch (SecurityType)
                {
                    case "8":
                        return true;
                    default:
                        return false;
                }
            }
        }
        public override bool IsValid { get { return ProcessDate >= Utilities.PreviousDate; } }
    }
}




                   
