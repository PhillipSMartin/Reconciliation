using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public interface IOption : IUnderlying
    {
        string OptionSymbol { get; }
        DateTime ExpirationDate { get; }
        decimal StrikePrice { get; }
        string OptionType { get; }
    }
}
