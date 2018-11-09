using System;
using System.Runtime.Serialization;

namespace ReconciliationClient
{
    [Serializable]
    public class ReconciliationException : Exception
    {
        public ReconciliationException() : base() { }
        public ReconciliationException(string message) : base(message) { }
        public ReconciliationException(string message, Exception inner) : base(message, inner) { }
        protected ReconciliationException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }
}
