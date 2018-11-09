using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ReconciliationLib
{
    public class PershingAggregatedTaxlotRecord : TaxlotRecord, IABRecord
    {
       #region Field definitions
        private FixedLengthField<int> m_SequenceNumber = new FixedLengthField<int>("SequenceNumber", 3, 8);
        private FixedLengthField<string> m_AccountNumber = new FixedLengthField<string>("AccountNumber", 11, 9);
        private FixedLengthField<string> m_PortfolioAccountType = new FixedLengthField<string>("PortfolioAccountType", 20, 1);
        private FixedLengthField<string> m_Cusip = new FixedLengthField<string>("Cusip", 21, 9);
        private FixedLengthField<string> m_IntroductingBroker = new FixedLengthField<string>("IntroductingBroker", 34, 3);
        private FixedLengthField<string> m_InvestmentProfessional = new FixedLengthField<string>("InvestmentProfessional", 38, 3);
        private FixedLengthCustomDateField m_ProcessDate = new FixedLengthCustomDateField("ProcessDate", 42, 8, "yyyyMMdd");
        private FixedLengthField<string> m_ReconciliationBreakIndicator = new FixedLengthField<string>("ReconciliationBreakIndicator", 50, 1);
        private FixedLengthSignedDoubleField m_Quantity = new FixedLengthSignedDoubleField("Quantity", 51, 18, 5, 69);
        private FixedLengthSignedDoubleField m_CostBasis = new FixedLengthSignedDoubleField("CostBasis", 70, 18, 9, 88);
        private FixedLengthField<string> m_SecuritySymbol = new FixedLengthField<string>("SecuritySymbol", 89, 16);
        private FixedLengthField<string> m_OptionRoot = new FixedLengthField<string>("OptionRoot", 115, 6);
        private FixedLengthCustomDateField m_ExpirationDate = new FixedLengthCustomDateField("ExpirationDate", 121, 6, "yyMMdd");
        private FixedLengthField<string> m_CallPutIndicator = new FixedLengthField<string>("CallPutIndicator", 127, 1);
        private FixedLengthDoubleField m_StrikePrice = new FixedLengthDoubleField("StrikePrice", 128, 8, 3);
        private FixedLengthCustomDateField m_DateOfData = new FixedLengthCustomDateField("DateOfData", 491, 8, "yyyyMMdd");
        #endregion

        public PershingAggregatedTaxlotRecord() { }

        public void ReadRecordA(string record)
        {
            m_SequenceNumber.ExtractValueFromRecord(record);
            m_AccountNumber.ExtractValueFromRecord(record);
            m_PortfolioAccountType.ExtractValueFromRecord(record);
            m_Cusip.ExtractValueFromRecord(record);
            m_IntroductingBroker.ExtractValueFromRecord(record);
            m_InvestmentProfessional.ExtractValueFromRecord(record);
            m_ProcessDate.ExtractValueFromRecord(record);
            m_ReconciliationBreakIndicator.ExtractValueFromRecord(record);
            m_Quantity.ExtractValueFromRecord(record);
            m_CostBasis.ExtractValueFromRecord(record);
            m_SecuritySymbol.ExtractValueFromRecord(record);
            m_OptionRoot.ExtractValueFromRecord(record);
            if (IsOption)
            {
                m_ExpirationDate.ExtractValueFromRecord(record);
                m_CallPutIndicator.ExtractValueFromRecord(record);
                m_StrikePrice.ExtractValueFromRecord(record);
            }
            m_DateOfData.ExtractValueFromRecord(record);
       }

        public void ReadRecordB(string record)
        {
            //  ignore B record
        }

        #region Public Properties
        public int SequenceNumber 
        { 
            get 
            {
                try
                {
                    return m_SequenceNumber.FieldValue;
                }
                catch (Exception)
                {
                    return -1;
                }
            } 
        }
        public string AccountNumber { get { return m_AccountNumber.FieldValue; } }
        public string PortfolioAccountType { get { return m_PortfolioAccountType.FieldValue; } }
        public string Cusip { get { return m_Cusip.FieldValue; } }
        public string IntroductingBroker { get { return m_IntroductingBroker.FieldValue; } }
        public string InvestmentProfessional { get { return m_InvestmentProfessional.FieldValue; } }
        public DateTime ProcessDate { get { return m_ProcessDate.FieldValue; } }
        public string ReconciliationBreakIndicator { get { return m_ReconciliationBreakIndicator.FieldValue; } }
        public double Quantity { get { return m_Quantity.FieldValue; } }
        public double CostBasis { get { return m_CostBasis.FieldValue; } }
        public string SecuritySymbol { get { return m_SecuritySymbol.FieldValue; } }
        public string OptionRoot { get { return m_OptionRoot.FieldValue; } }
        public DateTime? ExpirationDate 
        { 
            get 
            {
                if (!m_ExpirationDate.IsNull)
                    return m_ExpirationDate.FieldValue;
                return null;
            } 
        }
        public string CallPutIndicator { get { return m_CallPutIndicator.FieldValue; } }
        public double StrikePrice { get { return m_StrikePrice.FieldValue; } }
        public DateTime DateOfData { get { return m_DateOfData.FieldValue; } }
        #endregion

        public override string ToString()
        {
            try{
            return String.Format(CultureInfo.CurrentCulture, String.Format(
                "SequenceNumber={0}|" + 
                "AccountNumber={1}|" + 
                "PortfolioAccountType={2}|" + 
                "Cusip={3}" + 
                "IntroductingBroker={4}|" + 
                "InvestmentProfessional={5}|" + 
                "ProcessDate={6}|" + 
                "ReconciliationBreakIndicator={7}|" + 
                "Quantity={8}|" + 
                "CostBasis={9}|" + 
                "SecuritySymbol={10}|" + 
                "OptionRoot={11}|" +
                "ExpirationDate={12}|" +
                "CallPutIndicator={13}|" +
                "StrikePrice={14}|" +
                "DateOfData={15}", 
                SequenceNumber, 
                AccountNumber, 
                PortfolioAccountType,
                Cusip, 
                IntroductingBroker, 
                InvestmentProfessional, 
                ProcessDate,
                ReconciliationBreakIndicator,
                Quantity, 
                CostBasis,
                SecuritySymbol, 
                OptionRoot, 
                ExpirationDate, 
                CallPutIndicator, 
                StrikePrice, 
                DateOfData
));
            }
            catch
            {
                return String.Format(CultureInfo.CurrentCulture, "PershingAggregatedTaxlotRecord {0}", SequenceNumber);
            }
        }

        private bool IsOption
        {
            get
            {
                return !String.IsNullOrEmpty(OptionRoot);
            }
        }

        public override bool IsValid { get { return ProcessDate >= Utilities.PreviousDate; } }
    }
}
