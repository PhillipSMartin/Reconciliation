﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public abstract class BookkeepingRecord
    {
        public virtual bool IsValid { get { return true; } }
    }
}
