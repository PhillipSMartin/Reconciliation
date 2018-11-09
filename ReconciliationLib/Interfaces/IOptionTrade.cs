using System;
using System.Collections.Generic;
using System.Text;

namespace ReconciliationLib
{
    public interface IOptionTrade : IOption, ITrade
    {
        short? TraderId { get; }
        short? MediumId { get; }

        bool? SpreadFlag { get; }
        bool? TradeArchiveFlag { get; }
        bool? OptionArchiveFlag { get; }
    }
}
