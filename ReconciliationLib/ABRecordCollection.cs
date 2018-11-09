using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ReconciliationLib
{
    public class ABRecordCollection<T> : IDisposable where T : IABRecord, new() 
    {
        private FixedLengthField<string> m_transactionCode;
        private FixedLengthField<string> m_recordIndicator;
        private FixedLengthField<int> m_sequenceNumber;
        private FixedLengthCustomDateField m_headerDate;

        private T[] m_records;
        private int m_nextIndex;
        private StreamReader m_reader;
        private bool disposed;

        protected ABRecordCollection(string fileName,
            FixedLengthField<string> transactionCodeField,
            FixedLengthField<string> recordIndicatorField,
            FixedLengthField<int> sequenceNumberField,
            FixedLengthCustomDateField headerDate)
        {
            m_transactionCode = transactionCodeField;
            m_recordIndicator = recordIndicatorField;
            m_sequenceNumber = sequenceNumberField;
            m_headerDate = headerDate;

            m_reader = new StreamReader(fileName);
            m_records = FillRecords(m_reader);
        }

        public T NextRecord
        {
            get
            {
                if (m_records != null)
                {
                    if (m_nextIndex < m_records.Length)
                    {
                        return m_records[m_nextIndex++];
                    }
                }
                return default(T);
            }
        }

        protected T[] FillRecords(StreamReader reader)
        {
            List<T> abRecordList = new List<T>();
            T abRecord = default(T);
            string currentLine = "";
             bool reachedEOF = false;

            try
            {
                while (currentLine != null)
                {
                    // read next line
                    currentLine = reader.ReadLine();

                    // on eof, save current record and exit loop
                    if ((currentLine == null) || reachedEOF)
                    {
                        if (abRecord != null)
                        {
                            abRecordList.Add(abRecord);
                        }
                        break; 
                    }

                    // otherwise, process this line
                    else
                    {
                        switch (m_transactionCode.ExtractValueFromRecord(currentLine))
                        {
                            case "BOF": // beginning of file
                                m_headerDate.ExtractValueFromRecord(currentLine);
                                if (m_headerDate.FieldValue < Utilities.PreviousDate) 
                                {
                                    throw new ReconciliationImportException("Header record has an invalid date: " + m_headerDate.FieldValue.ToShortDateString());
                                }
                                break;
                            case "EOF": // end of file
                                reachedEOF = true;
                                break;
                            default:
                            // if we have a current record and sequence numbers do not match, save that record
                            if (abRecord != null)
                            {
                                if (abRecord.SequenceNumber != m_sequenceNumber.ExtractValueFromRecord(currentLine))
                                {
                                    abRecordList.Add(abRecord);
                                    abRecord = new T();
                                }
                            }
                            else
                            {
                                abRecord = new T();
                            }

                            // read data from current line
                            switch (m_recordIndicator.ExtractValueFromRecord(currentLine))
                            {
                                case "A":
                                    abRecord.ReadRecordA(currentLine);
                                    break;
                                case "B":
                                    abRecord.ReadRecordB(currentLine);
                                    break;
                                default:
                                    throw new ReconciliationImportException("Record has an unidentified record indicator: " + m_recordIndicator.FieldValue);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.Error("Error reading record: " + abRecord ?? "", e);
                abRecordList.Clear();
            }
            return abRecordList.ToArray();
        }
  #region Implement IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                if(disposing)
                {
                    if (m_reader != null)
                    {
                    m_reader.Dispose();
                    }
                }
            }
            disposed = true;         
        }

        ~ABRecordCollection()      
        {
            Dispose(false);
        }
#endregion
   }
}