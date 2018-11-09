using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ReconciliationLib
{
    [Serializable]
    public class ReconciliationException : Exception
    {
        public ReconciliationException() : base() { }
        public ReconciliationException(string message) : base(message) { }
        public ReconciliationException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationTradeException : Exception
    {
        private int m_rc;

        public ReconciliationTradeException() : this(0) { }
        public ReconciliationTradeException(int rc) : base(TranslateReturnCode(rc)) { m_rc = rc; }
        public ReconciliationTradeException(string message) : base(message) { }
        public ReconciliationTradeException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationTradeException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }

        public int ReturnCode { get { return m_rc; } }
        public const int CannotAddOption = 18;
        private static string TranslateReturnCode(int rc)
        {
            switch (rc)
            {
                case 0:
                    return String.Empty;
                case 1:
                    return "Underlying name not found";
                case 2:
                    return "Stock id not found";
                case 3:
                    return "Series id not found";
                case 4:
                    return "Invalid exercise type";
                case 5:
                    return "Subacct not found";
                case 6:
                    return "Invalid option type";
                case 7:
                    return "Invalid trade type";
                case 8:
                    return "Industry group not found";
                case 9:
                    return "Exchange not found";
                case 10:
                    return "Invalid underlying type";
                case 11:
                    return "Invalid account type";
                case 12:
                    return "Underlying id not found";
                case 13:
                    return "Illegal operation";
                case 14:
                    return "Invalid contra";
                case 15:
                    return "Invalid billing type";
                case 16:
                    return "Cannot enter a trade before import is done";
                case 17:
                    return "Cannot update a reconciled trade";
                case 18:
                    return "Unable to add new option to Hugo";
                case 19:
                    return "Invalid trade medium";
                default:
                    return String.Format("Undocumented error - rc={0}", rc);
            }
        }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ReturnCode", m_rc);
        }
    }

    [Serializable]
    public class ReconciliationNotInitializedException : Exception
    {
        public ReconciliationNotInitializedException() : base("Must call Utilities.Init() before calling other methods") { }
        public ReconciliationNotInitializedException(string message) : base(message) { }
        public ReconciliationNotInitializedException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationNotInitializedException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationNoConnectionException : Exception
    {
        public ReconciliationNoConnectionException() : base("No Sql connection specifed") { }
        public ReconciliationNoConnectionException(string message) : base(message) { }
        public ReconciliationNoConnectionException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationNoConnectionException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationNoAccountGroupNameException : Exception
    {
        public ReconciliationNoAccountGroupNameException() : base("No account group name specifed") { }
        public ReconciliationNoAccountGroupNameException(string message) : base(message) { }
        public ReconciliationNoAccountGroupNameException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationNoAccountGroupNameException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }
    [Serializable]
    public class ReconciliationNoClearingHouseException : Exception
    {
        public ReconciliationNoClearingHouseException() : base("No clearing house specifed") { }
        public ReconciliationNoClearingHouseException(string message) : base(message) { }
        public ReconciliationNoClearingHouseException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationNoClearingHouseException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationConversionException : ReconciliationException
    {
        public ReconciliationConversionException() : base() { }
        public ReconciliationConversionException(string message) : base(message) { }
        public ReconciliationConversionException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationConversionException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationInvalidEnumException : ReconciliationException
    {
        public ReconciliationInvalidEnumException() : base() { }
        public ReconciliationInvalidEnumException(string message) : base(message) { }
        public ReconciliationInvalidEnumException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationInvalidEnumException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationImportException : ReconciliationException
    {
        public ReconciliationImportException() : base() { }
        public ReconciliationImportException(string message) : base(message) { }
        public ReconciliationImportException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationImportException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    [Serializable]
    public class ReconciliationCommandNotFoundException : ReconciliationException
    {
        public ReconciliationCommandNotFoundException() : base() { }
        public ReconciliationCommandNotFoundException(string command) : base("Command " + command + " not found") { }
        public ReconciliationCommandNotFoundException(string command, Exception inner) : base("Command " + command + " not found", inner) { }
        protected ReconciliationCommandNotFoundException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }
}
