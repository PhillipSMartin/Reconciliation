namespace ReconciliationLib
{
    public abstract class ConfirmationRecord
    {
        public virtual bool IsValid { get { return true; } }
    }
}
