 using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public class ImportStatusInfo
    {
        private DateTime? m_lastStartTime;
        private DateTime? m_lastEndTime;
        private TaskStatus m_lastStatus = TaskStatus.None;
        private string m_note;

        public ImportStatusInfo()
        {
        }

        public ImportStatusInfo(DateTime? lastStartTime, 
            DateTime? lastEndTime,
            string lastStatusName,
            string note)
        {
            m_lastStartTime = lastStartTime;
            m_lastEndTime = lastEndTime;
            m_lastStatus = ConvertStatusName(lastStatusName);
            m_note = note;
        }
        public DateTime LastStartTime { get { return m_lastStartTime.HasValue ? m_lastStartTime.Value : new DateTime(); } }
        public DateTime LastEndTime { get { return m_lastEndTime.HasValue ? m_lastEndTime.Value : new DateTime(); } }
        public TaskStatus LastStatus { get { return m_lastStatus; } }
        public string Note { get { return m_note; } }

        private static TaskStatus ConvertStatusName(string statusName)
        {
            try
            {
                return (TaskStatus)Enum.Parse(typeof(TaskStatus), statusName);
            }
            catch(System.ArgumentException)
            {
                return TaskStatus.None;
            }
        }
    }
}
