using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class Hugo : ClearingHouseBase, IClearingHouse
    {
        public Hugo() : base(ClearingHouse.Hugo) { }

    }
}
