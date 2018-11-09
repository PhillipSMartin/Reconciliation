using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public class AccountGroupInfo
    {
        private string accountGroupName;
        private bool isFinalized;
        private bool assignmentsDone;
        private DateTime lastImportDate;    // date we last imported Merrill's positions (usually today)
        private DateTime prevImportDate;    // date of penultimate import of Merrill's positions

        public AccountGroupInfo()
        {
        }

        internal AccountGroupInfo(string accountGroupName, bool isFinalized, bool assignmentsDone, DateTime lastImportDate, DateTime prevImportDate)
        {
            this.accountGroupName = accountGroupName;
            this.isFinalized = isFinalized;
            this.assignmentsDone = assignmentsDone;
            this.lastImportDate = lastImportDate;
            this.prevImportDate = prevImportDate;
        }

        public string AccountGroupName
        {
            get { return accountGroupName; }
        }
        public bool AssignmentsDone
        {
            get { return assignmentsDone; }
        }
        public bool IsFinalized
        {
            get { return isFinalized; }
        }
        public DateTime LastImportDate
        {
            get { return lastImportDate; }
        }
        public DateTime PrevImportDate
        {
            get { return prevImportDate; }
        }

        public override string ToString()
        {
            return String.Format("Name={0}, LastImportDate={1:d}, PrevImportDate={2:d}, Finalized={3}, AssignmentsDone={4}",
                AccountGroupName, LastImportDate, PrevImportDate, IsFinalized ? "true" : "false", AssignmentsDone ? "true" : "false");
        }
    }
} 