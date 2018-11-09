using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ReconciliationLib
{
    public class PershingPositionRecord : PositionRecord, IABRecord
    {
        #region Field definitions
        private FixedLengthField<int> m_SequenceNumberA = new FixedLengthField<int>("SequenceNumberA", 3, 8);
        private FixedLengthField<string> m_AccountNumberA = new FixedLengthField<string>("AccountNumberA", 11, 10);
        private FixedLengthField<string> m_CusipA = new FixedLengthField<string>("CusipA", 21, 9);
        private FixedLengthField<string> m_UnderlyingCusipA = new FixedLengthField<string>("UnderlyingCusipA", 34, 9);
        private FixedLengthField<string> m_InvestmentProfessionalA = new FixedLengthField<string>("InvestmentProfessionalA", 47, 3);
        private FixedLengthField<string> m_IntroductingBrokerA = new FixedLengthField<string>("IntroductingBrokerA", 50, 3);
        private FixedLengthField<string> m_CurrencySecurityIndicator = new FixedLengthField<string>("CurrencySecurityIndicator", 53, 1);
        private FixedLengthField<string> m_IssueCurrency = new FixedLengthField<string>("IssueCurrency", 54, 3);
        private FixedLengthCustomDateField m_TDQuantityUpdateDate = new FixedLengthCustomDateField("TDQuantityUpdateDate", 57, 8, "yyyyMMdd");
        private FixedLengthCustomDateField m_SDQuantityUpdateDate = new FixedLengthCustomDateField("SDQuantityUpdateDate", 65, 8, "yyyyMMdd");
        private FixedLengthSignedDoubleField m_TDQuantity = new FixedLengthSignedDoubleField("TDQuantity", 73, 18, 5, 91);
        private FixedLengthSignedDoubleField m_SDQuantity = new FixedLengthSignedDoubleField("SDQuantity", 92, 18, 5, 110);
        private FixedLengthSignedDoubleField m_SEGQuantity = new FixedLengthSignedDoubleField("SEGQuantity", 111, 18, 5, 129);
        private FixedLengthSignedDoubleField m_SafekeepingQuantity = new FixedLengthSignedDoubleField("SafekeepingQuantity", 130, 18, 5, 148);
        private FixedLengthSignedDoubleField m_TransferQuantity = new FixedLengthSignedDoubleField("TransferQuantity", 149, 18, 5, 167);
        private FixedLengthSignedDoubleField m_PendingTransferQuantity = new FixedLengthSignedDoubleField("PendingTransferQuantity", 168, 18, 5, 186);
        private FixedLengthSignedDoubleField m_LegalTransferQuantity = new FixedLengthSignedDoubleField("LegalTransferQuantity", 187, 18, 5, 205);
        private FixedLengthSignedDoubleField m_TenderedQuantity = new FixedLengthSignedDoubleField("TenderedQuantity", 206, 18, 5, 224);
        private FixedLengthSignedDoubleField m_PendingPapersQuantity = new FixedLengthSignedDoubleField("PendingPapersQuantity", 225, 18, 5, 243);
        private FixedLengthSignedDoubleField m_ShortAgainstBoxQuantity = new FixedLengthSignedDoubleField("ShortAgainstBoxQuantity", 244, 18, 5, 262);
        private FixedLengthSignedDoubleField m_NetworkedQuantity = new FixedLengthSignedDoubleField("NetworkedQuantity", 263, 18, 5, 281);
        private FixedLengthSignedDoubleField m_PendingSplitQuantity = new FixedLengthSignedDoubleField("PendingSplitQuantity", 282, 18, 5, 300);
        private FixedLengthSignedDoubleField m_CoveredQuantity = new FixedLengthSignedDoubleField("CoveredQuantity", 301, 18, 5, 319);
        private FixedLengthSignedDoubleField m_TDQuantityBought = new FixedLengthSignedDoubleField("TDQuantityBought", 320, 18, 5, 338);
        private FixedLengthSignedDoubleField m_TDQuantitySold = new FixedLengthSignedDoubleField("TDQuantitySold", 339, 18, 5, 357);
        private FixedLengthSignedDoubleField m_RegTRequirement = new FixedLengthSignedDoubleField("RegTRequirement", 358, 18, 2, 376);
        private FixedLengthSignedDoubleField m_HouseMarginRequirement = new FixedLengthSignedDoubleField("HouseMarginRequirement", 377, 18, 2, 395);
        private FixedLengthSignedDoubleField m_ExchangeRequirement = new FixedLengthSignedDoubleField("ExchangeRequirement", 396, 18, 2, 414);
        private FixedLengthSignedDoubleField m_EquityRequirement = new FixedLengthSignedDoubleField("EquityRequirement", 415, 18, 2, 433);
        private FixedLengthField<string> m_SecuritySymbol = new FixedLengthField<string>("SecuritySymbol", 434, 9);
        private FixedLengthField<string> m_SecurityType = new FixedLengthField<string>("SecurityType", 443, 1);
        private FixedLengthField<string> m_SecurityMod = new FixedLengthField<string>("SecurityMod", 444, 1);
        private FixedLengthField<string> m_SecurityCalc = new FixedLengthField<string>("SecurityCalc", 445, 1);
        private FixedLengthField<string> m_MinorProductCode = new FixedLengthField<string>("MinorProductCode", 446, 3);
        private FixedLengthField<string> m_NetworkEligibilityIndicator = new FixedLengthField<string>("NetworkEligibilityIndicator", 449, 1);
        private FixedLengthSignedDoubleField m_StrikePriceA = new FixedLengthSignedDoubleField("StrikePriceA", 450, 18, 9, 468);
        private FixedLengthCustomDateField m_ExpirationDateA = new FixedLengthCustomDateField("ExpirationDateA", 469, 8, "yyyyMMdd");
        private FixedLengthDoubleField m_ContractSize = new FixedLengthDoubleField("ContractSize", 477, 18, 5);
        private FixedLengthDoubleField m_ConversionRatio = new FixedLengthDoubleField("ConversionRatio", 495, 18, 9);
        private FixedLengthField<string> m_AccountShortName = new FixedLengthField<string>("AccountShortName", 513, 10);
        private FixedLengthField<string> m_StateCode = new FixedLengthField<string>("StateCode", 523, 3);
        private FixedLengthField<string> m_CountryCode = new FixedLengthField<string>("CountryCode", 526, 3);
        private FixedLengthField<int> m_NumberofDescriptionLines = new FixedLengthField<int>("NumberofDescriptionLines", 533, 4);
        private FixedLengthField<string> m_DescriptionLine1 = new FixedLengthField<string>("DescriptionLine1", 537, 20);
        private FixedLengthField<string> m_DescriptionLine2 = new FixedLengthField<string>("DescriptionLine2", 557, 20);
        private FixedLengthField<string> m_DescriptionLine3 = new FixedLengthField<string>("DescriptionLine3", 577, 20);
        private FixedLengthField<string> m_DescriptionLine4 = new FixedLengthField<string>("DescriptionLine4", 597, 20);
        private FixedLengthField<string> m_DescriptionLine5 = new FixedLengthField<string>("DescriptionLine5", 617, 20);
        private FixedLengthField<string> m_DescriptionLine6 = new FixedLengthField<string>("DescriptionLine6", 637, 20);
        private FixedLengthField<string> m_DividendOption = new FixedLengthField<string>("DividendOption", 657, 1);
        private FixedLengthField<string> m_LTCapialGainsOption = new FixedLengthField<string>("LTCapialGainsOption", 658, 1);
        private FixedLengthField<string> m_STCapialGainsOption = new FixedLengthField<string>("STCapialGainsOption", 659, 1);
        private FixedLengthField<string> m_FirmTradingIndicator = new FixedLengthField<string>("FirmTradingIndicator", 660, 1);
        private FixedLengthField<string> m_PositionCurrency = new FixedLengthField<string>("PositionCurrency", 661, 3);
        private FixedLengthSignedDoubleField m_TDLiquidatingValue = new FixedLengthSignedDoubleField("TDLiquidatingValue", 664, 18, 3, 682);
        private FixedLengthSignedDoubleField m_PoolFactor = new FixedLengthSignedDoubleField("PoolFactor", 683, 10, 8, 693);
        private FixedLengthSignedDoubleField m_ExchangeRate = new FixedLengthSignedDoubleField("ExchangeRate", 694, 18, 10, 712);
        private FixedLengthSignedDoubleField m_SDLiquidatingValue = new FixedLengthSignedDoubleField("SDLiquidatingValue", 713, 18, 3, 731);
        private FixedLengthField<string> m_AlternateSecurityIdType = new FixedLengthField<string>("AlternateSecurityIdType", 735, 1);
        private FixedLengthField<string> m_AlternateSecurityId = new FixedLengthField<string>("AlternateSecurityId", 736, 12);
        private FixedLengthField<string> m_StructureProductIndicator = new FixedLengthField<string>("StructureProductIndicator", 748, 1);

        private FixedLengthField<int> m_SequenceNumberB = new FixedLengthField<int>("SequenceNumberB", 3, 8);
        private FixedLengthField<string> m_AccountNumberB = new FixedLengthField<string>("AccountNumberB", 11, 10);
        private FixedLengthField<string> m_CusipB = new FixedLengthField<string>("CusipB", 21, 9);
        private FixedLengthField<string> m_UnderlyingCusipB = new FixedLengthField<string>("UnderlyingCusipB", 34, 9);
        private FixedLengthField<string> m_InvestmentProfessionalB = new FixedLengthField<string>("InvestmentProfessionalB", 47, 3);
        private FixedLengthField<string> m_IntroductingBrokerB = new FixedLengthField<string>("IntroductingBrokerB", 50, 3);
        private FixedLengthSignedDoubleField m_FullyPaidLendingQuantity = new FixedLengthSignedDoubleField("FullyPaidLendingQuantity", 53, 18, 5, 71);
        private FixedLengthSignedDoubleField m_FullyPaidLendingQuantityCollateralAmount = new FixedLengthSignedDoubleField("FullyPaidLendingQuantityCollateralAmount", 72, 18, 3, 90);
        private FixedLengthField<string> m_OptionRoot = new FixedLengthField<string>("OptionRoot", 91, 6);
        private FixedLengthCustomDateField m_ExpirationDateB = new FixedLengthCustomDateField("ExpirationDateB", 97, 6, "yyMMdd");
        private FixedLengthField<string> m_CallPutIndicator = new FixedLengthField<string>("CallPutIndicator", 103, 1);
        private FixedLengthDoubleField m_StrikePriceB = new FixedLengthDoubleField("StrikePriceB", 104, 8, 3);
        private FixedLengthSignedDoubleField m_TDRepoQuantity = new FixedLengthSignedDoubleField("TDRepoQuantity", 112, 18, 5, 130);
        private FixedLengthSignedDoubleField m_SDRepoQuantity = new FixedLengthSignedDoubleField("SDRepoQuantity", 131, 18, 5, 149);
        private FixedLengthSignedDoubleField m_TDReverseRepoQuantity = new FixedLengthSignedDoubleField("TDReverseRepoQuantity", 150, 18, 5, 168);
        private FixedLengthSignedDoubleField m_SDReverseRepoQuantity = new FixedLengthSignedDoubleField("SDReverseRepoQuantity", 169, 18, 5, 187);
        private FixedLengthSignedDoubleField m_CollateralPledgeQuantity = new FixedLengthSignedDoubleField("CollateralPledgeQuantity", 188, 18, 5, 206);
        private FixedLengthSignedDoubleField m_CorporateExecutiveServicesCollateralPledgeQuantity = new FixedLengthSignedDoubleField("CorporateExecutiveServicesCollateralPledgeQuantity", 207, 18, 5, 225);
        private FixedLengthSignedDoubleField m_TDRepoLiquidatingValue = new FixedLengthSignedDoubleField("TDRepoLiquidatingValue", 226, 18, 3, 244);
        private FixedLengthSignedDoubleField m_SDRepoLiquidatingValue = new FixedLengthSignedDoubleField("SDRepoLiquidatingValue", 245, 18, 3, 263);
        private FixedLengthSignedDoubleField m_TDReverseRepoLiquidatingValue = new FixedLengthSignedDoubleField("TDReverseRepoLiquidatingValue", 264, 18, 3, 282);
        private FixedLengthSignedDoubleField m_SDReverseRepoLiquidatingValue = new FixedLengthSignedDoubleField("SDReverseRepoLiquidatingValue", 283, 18, 3, 301);
        private FixedLengthSignedDoubleField m_CollateralPledgeLiquidatingValue = new FixedLengthSignedDoubleField("CollateralPledgeLiquidatingValue", 302, 18, 3, 320);
        private FixedLengthSignedDoubleField m_CorporateExecutiveServicesCollateralPledgeLiquidatingValue = new FixedLengthSignedDoubleField("CorporateExecutiveServicesCollateralPledgeLiquidatingValue", 321, 18, 3, 339);
        private FixedLengthSignedDoubleField m_TDRepoLoanAmount = new FixedLengthSignedDoubleField("TDRepoLoanAmount", 340, 18, 3, 358);
        private FixedLengthSignedDoubleField m_SDRepoLoanAmount = new FixedLengthSignedDoubleField("SDRepoLoanAmount", 359, 18, 3, 377);
        private FixedLengthSignedDoubleField m_TDReverseRepoLoanAmount = new FixedLengthSignedDoubleField("TDReverseRepoLoanAmount", 378, 18, 3, 396);
        private FixedLengthSignedDoubleField m_SDReverseRepoLoanAmount = new FixedLengthSignedDoubleField("SDReverseRepoLoanAmount", 397, 18, 3, 415);
        private FixedLengthSignedDoubleField m_AccruedInterestValue = new FixedLengthSignedDoubleField("AccruedInterestValue", 416, 18, 3, 434);
        private FixedLengthDoubleField m_DividendorCouponRate = new FixedLengthDoubleField("DividendorCouponRate", 435, 18, 9);
        #endregion

        public PershingPositionRecord() { }

        public void ReadRecordA(string record)
        {
            m_SequenceNumberA.ExtractValueFromRecord(record);
            m_AccountNumberA.ExtractValueFromRecord(record);
            m_CusipA.ExtractValueFromRecord(record);
            m_UnderlyingCusipA.ExtractValueFromRecord(record);
            m_InvestmentProfessionalA.ExtractValueFromRecord(record);
            m_IntroductingBrokerA.ExtractValueFromRecord(record);
            m_CurrencySecurityIndicator.ExtractValueFromRecord(record);
            m_IssueCurrency.ExtractValueFromRecord(record);
            m_TDQuantityUpdateDate.ExtractValueFromRecord(record);
            m_SDQuantityUpdateDate.ExtractValueFromRecord(record);
            m_TDQuantity.ExtractValueFromRecord(record);
            m_SDQuantity.ExtractValueFromRecord(record);
            m_SEGQuantity.ExtractValueFromRecord(record);
            m_SafekeepingQuantity.ExtractValueFromRecord(record); ;
            m_TransferQuantity.ExtractValueFromRecord(record);
            m_PendingTransferQuantity.ExtractValueFromRecord(record);
            m_LegalTransferQuantity.ExtractValueFromRecord(record);
            m_TenderedQuantity.ExtractValueFromRecord(record);
            m_PendingPapersQuantity.ExtractValueFromRecord(record);
            m_ShortAgainstBoxQuantity.ExtractValueFromRecord(record);
            m_NetworkedQuantity.ExtractValueFromRecord(record);
            m_PendingSplitQuantity.ExtractValueFromRecord(record);
            m_CoveredQuantity.ExtractValueFromRecord(record);
            m_TDQuantityBought.ExtractValueFromRecord(record);
            m_TDQuantitySold.ExtractValueFromRecord(record);
            m_RegTRequirement.ExtractValueFromRecord(record);
            m_HouseMarginRequirement.ExtractValueFromRecord(record);
            m_ExchangeRequirement.ExtractValueFromRecord(record);
            m_EquityRequirement.ExtractValueFromRecord(record);
            m_SecuritySymbol.ExtractValueFromRecord(record);
            m_SecurityType.ExtractValueFromRecord(record);
            m_SecurityMod.ExtractValueFromRecord(record);
            m_SecurityCalc.ExtractValueFromRecord(record);
            m_MinorProductCode.ExtractValueFromRecord(record);
            m_NetworkEligibilityIndicator.ExtractValueFromRecord(record);
            if (IsOption)
            {
                m_StrikePriceA.ExtractValueFromRecord(record);
                m_ExpirationDateA.ExtractValueFromRecord(record);
            }
            m_ContractSize.ExtractValueFromRecord(record);
            m_ConversionRatio.ExtractValueFromRecord(record);
            m_AccountShortName.ExtractValueFromRecord(record);
            m_StateCode.ExtractValueFromRecord(record);
            m_CountryCode.ExtractValueFromRecord(record);
            m_NumberofDescriptionLines.ExtractValueFromRecord(record);
            m_DescriptionLine1.ExtractValueFromRecord(record);
            m_DescriptionLine2.ExtractValueFromRecord(record);
            m_DescriptionLine3.ExtractValueFromRecord(record);
            m_DescriptionLine4.ExtractValueFromRecord(record);
            m_DescriptionLine5.ExtractValueFromRecord(record);
            m_DescriptionLine6.ExtractValueFromRecord(record);
            m_DividendOption.ExtractValueFromRecord(record);
            m_LTCapialGainsOption.ExtractValueFromRecord(record);
            m_STCapialGainsOption.ExtractValueFromRecord(record);
            m_FirmTradingIndicator.ExtractValueFromRecord(record);
            m_PositionCurrency.ExtractValueFromRecord(record);
            m_TDLiquidatingValue.ExtractValueFromRecord(record);
            m_PoolFactor.ExtractValueFromRecord(record);
            m_ExchangeRate.ExtractValueFromRecord(record);
            m_SDLiquidatingValue.ExtractValueFromRecord(record);
            m_AlternateSecurityIdType.ExtractValueFromRecord(record);
            m_AlternateSecurityId.ExtractValueFromRecord(record); ;
            m_StructureProductIndicator.ExtractValueFromRecord(record);
        }

        public void ReadRecordB(string record)
        {
            m_SequenceNumberB.ExtractValueFromRecord(record);
            m_AccountNumberB.ExtractValueFromRecord(record);
            m_CusipB.ExtractValueFromRecord(record);
            m_UnderlyingCusipB.ExtractValueFromRecord(record);
            m_InvestmentProfessionalB.ExtractValueFromRecord(record);
            m_IntroductingBrokerB.ExtractValueFromRecord(record);
            m_FullyPaidLendingQuantity.ExtractValueFromRecord(record);
            m_FullyPaidLendingQuantityCollateralAmount.ExtractValueFromRecord(record);
            m_OptionRoot.ExtractValueFromRecord(record);
            if (IsOption)
            {
                m_ExpirationDateB.ExtractValueFromRecord(record);
                m_CallPutIndicator.ExtractValueFromRecord(record);
                m_StrikePriceB.ExtractValueFromRecord(record);
            }
            m_TDRepoQuantity.ExtractValueFromRecord(record);
            m_SDRepoQuantity.ExtractValueFromRecord(record);
            m_TDReverseRepoQuantity.ExtractValueFromRecord(record);
            m_SDReverseRepoQuantity.ExtractValueFromRecord(record);
            m_CollateralPledgeQuantity.ExtractValueFromRecord(record);
            m_CorporateExecutiveServicesCollateralPledgeQuantity.ExtractValueFromRecord(record);
            m_TDRepoLiquidatingValue.ExtractValueFromRecord(record);
            m_SDRepoLiquidatingValue.ExtractValueFromRecord(record);
            m_TDReverseRepoLiquidatingValue.ExtractValueFromRecord(record);
            m_SDReverseRepoLiquidatingValue.ExtractValueFromRecord(record);
            m_CollateralPledgeLiquidatingValue.ExtractValueFromRecord(record);
            m_CorporateExecutiveServicesCollateralPledgeLiquidatingValue.ExtractValueFromRecord(record);
            m_SDRepoLoanAmount.ExtractValueFromRecord(record);
            m_TDReverseRepoLoanAmount.ExtractValueFromRecord(record);
            m_SDReverseRepoLoanAmount.ExtractValueFromRecord(record);
            m_AccruedInterestValue.ExtractValueFromRecord(record);
            m_DividendorCouponRate.ExtractValueFromRecord(record);
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
        public string Cusip { get { return m_CusipB.IsNull ? m_CusipA.FieldValue : m_CusipB.FieldValue; } }
        public string UnderlyingCusip { get { return m_UnderlyingCusipB.IsNull ? m_UnderlyingCusipA.FieldValue : m_UnderlyingCusipB.FieldValue; } }
        public string InvestmentProfessional { get { return m_InvestmentProfessionalB.IsNull ? m_InvestmentProfessionalA.FieldValue : m_InvestmentProfessionalB.FieldValue; } }
        public string IntroductingBroker { get { return m_IntroductingBrokerB.IsNull ? m_IntroductingBrokerA.FieldValue : m_IntroductingBrokerB.FieldValue; } }
        public string CurrencySecurityIndicator { get { return m_CurrencySecurityIndicator.FieldValue; } }
        public string IssueCurrency { get { return m_IssueCurrency.FieldValue; } }
        public DateTime TDQuantityUpdateDate { get { return m_TDQuantityUpdateDate.FieldValue; } }
        public DateTime SDQuantityUpdateDate { get { return m_SDQuantityUpdateDate.FieldValue; } }
        public double TDQuantity { get { return m_TDQuantity.FieldValue; } }
        public double SDQuantity { get { return m_SDQuantity.FieldValue; } }
        public double SEGQuantity { get { return m_SEGQuantity.FieldValue; } }
        public double SafekeepingQuantity { get { return m_SafekeepingQuantity.FieldValue; } }
        public double TransferQuantity { get { return m_TransferQuantity.FieldValue; } }
        public double PendingTransferQuantity { get { return m_PendingTransferQuantity.FieldValue; } }
        public double LegalTransferQuantity { get { return m_LegalTransferQuantity.FieldValue; } }
        public double TenderedQuantity { get { return m_TenderedQuantity.FieldValue; } }
        public double PendingPapersQuantity { get { return m_PendingPapersQuantity.FieldValue; } }
        public double ShortAgainstBoxQuantity { get { return m_ShortAgainstBoxQuantity.FieldValue; } }
        public double NetworkedQuantity { get { return m_NetworkedQuantity.FieldValue; } }
        public double PendingSplitQuantity { get { return m_PendingSplitQuantity.FieldValue; } }
        public double CoveredQuantity { get { return m_CoveredQuantity.FieldValue; } }
        public double TDQuantityBought { get { return m_TDQuantityBought.FieldValue; } }
        public double TDQuantitySold { get { return m_TDQuantitySold.FieldValue; } }
        public double RegTRequirement { get { return m_RegTRequirement.FieldValue; } }
        public double HouseMarginRequirement { get { return m_HouseMarginRequirement.FieldValue; } }
        public double ExchangeRequirement { get { return m_ExchangeRequirement.FieldValue; } }
        public double EquityRequirement { get { return m_EquityRequirement.FieldValue; } }
        public string SecuritySymbol { get { return m_SecuritySymbol.FieldValue; } }
        public string SecurityType { get { return m_SecurityType.FieldValue; } }
        public string SecurityMod { get { return m_SecurityMod.FieldValue; } }
        public string SecurityCalc { get { return m_SecurityCalc.FieldValue; } }
        public string MinorProductCode { get { return m_MinorProductCode.FieldValue; } }
        public string NetworkEligibilityIndicator { get { return m_NetworkEligibilityIndicator.FieldValue; } }
        public double ContractSize { get { return m_ContractSize.FieldValue; } }
        public double ConversionRatio { get { return m_ConversionRatio.FieldValue; } }
        public string AccountShortName { get { return m_AccountShortName.FieldValue; } }
        public string StateCode { get { return m_StateCode.FieldValue; } }
        public string CountryCode { get { return m_CountryCode.FieldValue; } }
        public int NumberofDescriptionLines { get { return m_NumberofDescriptionLines.FieldValue; } }
        public string DescriptionLine1 { get { return m_DescriptionLine1.FieldValue; } }
        public string DescriptionLine2 { get { return m_DescriptionLine2.FieldValue; } }
        public string DescriptionLine3 { get { return m_DescriptionLine3.FieldValue; } }
        public string DescriptionLine4 { get { return m_DescriptionLine4.FieldValue; } }
        public string DescriptionLine5 { get { return m_DescriptionLine5.FieldValue; } }
        public string DescriptionLine6 { get { return m_DescriptionLine6.FieldValue; } }
        public string DividendOption { get { return m_DividendOption.FieldValue; } }
        public string LTCapialGainsOption { get { return m_LTCapialGainsOption.FieldValue; } }
        public string STCapialGainsOption { get { return m_STCapialGainsOption.FieldValue; } }
        public string FirmTradingIndicator { get { return m_FirmTradingIndicator.FieldValue; } }
        public string PositionCurrency { get { return m_PositionCurrency.FieldValue; } }
        public double TDLiquidatingValue { get { return m_TDLiquidatingValue.FieldValue; } }
        public double PoolFactor { get { return m_PoolFactor.FieldValue; } }
        public double ExchangeRate { get { return m_ExchangeRate.FieldValue; } }
        public double SDLiquidatingValue { get { return m_SDLiquidatingValue.FieldValue; } }
        public string AlternateSecurityIdType { get { return m_AlternateSecurityIdType.FieldValue; } }
        public string AlternateSecurityId { get { return m_AlternateSecurityId.FieldValue; } }
        public string StructureProductIndicator { get { return m_StructureProductIndicator.FieldValue; } }
        public double FullyPaidLendingQuantity { get { return m_FullyPaidLendingQuantity.FieldValue; } }
        public double FullyPaidLendingQuantityCollateralAmount { get { return m_FullyPaidLendingQuantityCollateralAmount.FieldValue; } }
        public string OptionRoot { get { return m_OptionRoot.FieldValue; } }
        public DateTime? ExpirationDate 
        { 
            get 
            {
                if (!m_ExpirationDateB.IsNull)
                    return m_ExpirationDateB.FieldValue;
                if (!m_ExpirationDateA.IsNull)
                    return m_ExpirationDateA.FieldValue;
                return null;
            } 
        }
        public string CallPutIndicator { get { return m_CallPutIndicator.FieldValue; } }
        public double StrikePrice { get { return m_StrikePriceB.IsNull ? m_StrikePriceA.FieldValue : m_StrikePriceB.FieldValue; } }
        public double TDRepoQuantity { get { return m_TDRepoQuantity.FieldValue; } }
        public double SDRepoQuantity { get { return m_SDRepoQuantity.FieldValue; } }
        public double TDReverseRepoQuantity { get { return m_TDReverseRepoQuantity.FieldValue; } }
        public double SDReverseRepoQuantity { get { return m_SDReverseRepoQuantity.FieldValue; } }
        public double CollateralPledgeQuantity { get { return m_CollateralPledgeQuantity.FieldValue; } }
        public double CorporateExecutiveServicesCollateralPledgeQuantity { get { return m_CorporateExecutiveServicesCollateralPledgeQuantity.FieldValue; } }
        public double TDRepoLiquidatingValue { get { return m_TDRepoLiquidatingValue.FieldValue; } }
        public double SDRepoLiquidatingValue { get { return m_SDRepoLiquidatingValue.FieldValue; } }
        public double TDReverseRepoLiquidatingValue { get { return m_TDReverseRepoLiquidatingValue.FieldValue; } }
        public double SDReverseRepoLiquidatingValue { get { return m_SDReverseRepoLiquidatingValue.FieldValue; } }
        public double CollateralPledgeLiquidatingValue { get { return m_CollateralPledgeLiquidatingValue.FieldValue; } }
        public double CorporateExecutiveServicesCollateralPledgeLiquidatingValue { get { return m_CorporateExecutiveServicesCollateralPledgeLiquidatingValue.FieldValue; } }
        public double TDRepoLoanAmount { get { return m_TDRepoLoanAmount.FieldValue; } }
        public double SDRepoLoanAmount { get { return m_SDRepoLoanAmount.FieldValue; } }
        public double TDReverseRepoLoanAmount { get { return m_TDReverseRepoLoanAmount.FieldValue; } }
        public double SDReverseRepoLoanAmount { get { return m_SDReverseRepoLoanAmount.FieldValue; } }
        public double AccruedInterestValue { get { return m_AccruedInterestValue.FieldValue; } }
        public double DividendorCouponRate { get { return m_DividendorCouponRate.FieldValue; } }
        #endregion

        public override string ToString()
        {
            try{
            return String.Format(CultureInfo.CurrentCulture, String.Format(
                "SequenceNumber={0}|" + 
                "AccountNumber={1}|" + 
                "Cusip={2}|" + 
                "UnderlyingCusip={3}" + 
                "InvestmentProfessional={4}|" + 
                "IntroductingBroker={5}|" + 
                "CurrencySecurityIndicator={8}|" + 
                "IssueCurrency={9}|" + 
                "TDQuantityUpdateDate={10}|" + 
                "SDQuantityUpdateDate={11}|" + 
                "TDQuantity={12}|" + 
                "SDQuantity={13}|" + 
                "SEGQuantity={14}|" + 
                "SafekeepingQuantity={15}|" + 
                "TransferQuantity={16}|" + 
                "PendingTransferQuantity={17}|" + 
                "LegalTransferQuantity={18}|" + 
                "TenderedQuantity={19}|" + 
                "PendingPapersQuantity={20}|" + 
                "ShortAgainstBoxQuantity={21}|" + 
                "NetworkedQuantity={22}|" + 
                "PendingSplitQuantity={23}|" + 
                "CoveredQuantity={24}|" + 
                "TDQuantityBought={25}|" + 
                "TDQuantitySold={26}|" + 
                "RegTRequirement={27}|" + 
                "HouseMarginRequirement={28}|" + 
                "ExchangeRequirement={29}|" + 
                "EquityRequirement={30}|" + 
                "SecuritySymbol={31}|" + 
                "SecurityType={32}|" + 
                "SecurityMod={33}|" + 
                "SecurityCalc={34}|" + 
                "MinorProductCode={35}|" + 
                "NetworkEligibilityIndicator={36}|" + 
                "ContractSize={37}|" + 
                "ConversionRatio={38}|" + 
                "AccountShortName={39}|" + 
                "StateCode={40}|" + 
                "CountryCode={41}|" + 
                "NumberofDescriptionLines={42}|" + 
                "DescriptionLine1={43}|" + 
                "DescriptionLine2={44}|" + 
                "SescriptionLine3={45}|" + 
                "DescriptionLine4={46}|" + 
                "DescriptionLine5={47}|" + 
                "DescriptionLine6={48}|" + 
                "DividendOption={49}|" + 
                "LTCapialGainsOption={50}|" + 
                "STCapialGainsOption={51}|" + 
                "FirmTradingIndicator={52}|" + 
                "PositionCurrency={53}|" + 
                "TDLiquidatingValue={54}|" + 
                "PoolFactor={55}|" + 
                "ExchangeRate={56}|" + 
                "SDLiquidatingValue={57}|" + 
                "AlternateSecurityIdType={58}|" + 
                "AlternateSecurityId={59}|" + 
                "StructureProductIndicator={60}|" + 
                "FullyPaidLendingQuantity={61}|" + 
                "FullyPaidLendingQuantityCollateralAmount={62}|" + 
                "OptionRoot={63}|" +
                "ExpirationDate={7}|" +
                "CallPutIndicator={64}|" +
                "StrikePrice={6}|" +
                "TDRepoQuantity={65}|" + 
                "SDRepoQuantity={66}|" + 
                "TDReverseRepoQuantity={67}|" + 
                "SDReverseRepoQuantity={68}|" + 
                "CollateralPledgeQuantity={69}|" + 
                "CorporateExecutiveServicesCollateralPledgeQuantity={70}|" + 
                "TDRepoLiquidatingValue={71}|" + 
                "SDRepoLiquidatingValue={72}|" + 
                "TDReverseRepoLiquidatingValue={73}|" + 
                "SDReverseRepoLiquidatingValue={74}|" + 
                "CollateralPledgeLiquidatingValue={75}|" + 
                "CorporateExecutiveServicesCollateralPledgeLiquidatingValue={76}|" + 
                "TDRepoLoanAmount={77}|" + 
                "SDRepoLoanAmount={78}|" + 
                "TDReverseRepoLoanAmount={79}|" + 
                "SDReverseRepoLoanAmount={80}|" + 
                "AccruedInterestValue={81}|" + 
                "DividendorCouponRate={82}", 
                SequenceNumber, 
                AccountNumber, 
                Cusip, 
                UnderlyingCusip, 
                InvestmentProfessional, 
                IntroductingBroker, 
                StrikePrice, 
                ExpirationDate, 
                CurrencySecurityIndicator, 
                IssueCurrency, 
                TDQuantityUpdateDate, 
                SDQuantityUpdateDate, 
                TDQuantity, 
                SDQuantity, 
                SEGQuantity, 
                SafekeepingQuantity, 
                TransferQuantity, 
                PendingTransferQuantity, 
                LegalTransferQuantity, 
                TenderedQuantity, 
                PendingPapersQuantity, 
                ShortAgainstBoxQuantity, 
                NetworkedQuantity, 
                PendingSplitQuantity, 
                CoveredQuantity, 
                TDQuantityBought, 
                TDQuantitySold, 
                RegTRequirement, 
                HouseMarginRequirement, 
                ExchangeRequirement, 
                EquityRequirement, 
                SecuritySymbol, 
                SecurityType, 
                SecurityMod, 
                SecurityCalc, 
                MinorProductCode, 
                NetworkEligibilityIndicator, 
                ContractSize, 
                ConversionRatio, 
                AccountShortName, 
                StateCode, 
                CountryCode, 
                NumberofDescriptionLines, 
                DescriptionLine1, 
                DescriptionLine2, 
                DescriptionLine3, 
                DescriptionLine4, 
                DescriptionLine5, 
                DescriptionLine6, 
                DividendOption, 
                LTCapialGainsOption, 
                STCapialGainsOption, 
                FirmTradingIndicator, 
                PositionCurrency, 
                TDLiquidatingValue, 
                PoolFactor, 
                ExchangeRate, 
                SDLiquidatingValue, 
                AlternateSecurityIdType, 
                AlternateSecurityId, 
                StructureProductIndicator, 
                FullyPaidLendingQuantity, 
                FullyPaidLendingQuantityCollateralAmount, 
                OptionRoot, 
                CallPutIndicator, 
                TDRepoQuantity, 
                SDRepoQuantity, 
                TDReverseRepoQuantity, 
                SDReverseRepoQuantity, 
                CollateralPledgeQuantity, 
                CorporateExecutiveServicesCollateralPledgeQuantity, 
                TDRepoLiquidatingValue, 
                SDRepoLiquidatingValue, 
                TDReverseRepoLiquidatingValue, 
                SDReverseRepoLiquidatingValue, 
                CollateralPledgeLiquidatingValue, 
                CorporateExecutiveServicesCollateralPledgeLiquidatingValue, 
                TDRepoLoanAmount, 
                SDRepoLoanAmount, 
                TDReverseRepoLoanAmount, 
                SDReverseRepoLoanAmount, 
                AccruedInterestValue, 
                DividendorCouponRate 
));
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
    }
}
