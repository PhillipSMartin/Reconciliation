using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationLib
{
    public class ClearingHouseFactory
    {
        private ClearingHouseFactory() { }

        public static IClearingHouse GetClearingHouse(ClearingHouse clearingHouse)
        {
            switch (clearingHouse)
            {
                case ClearingHouse.Pershing:
                    return new Pershing();
                case ClearingHouse.WellsFargo:
                    return new WellsFargo();
                case ClearingHouse.Liquid:
                    return new Liquid();
                case ClearingHouse.TCW:
                    return new TCW();
                case ClearingHouse.MorganStanley:
                    return new MorganStanley();
                case ClearingHouse.BONY:
                    return new BONY();
                case ClearingHouse.Hugo:
                    return new Hugo();
                case ClearingHouse.ICBC:
                    return new ICBC();
                case ClearingHouse.IB:
                    return new IB();
                default:
                    throw new ReconciliationNoClearingHouseException("Unsupported clearing house " + clearingHouse.ToString());
            }
        }
        internal static IClearingHouse GetClearingHouse(HugoDataSet.AccountGroupsRow accountGroup)
        {
            return GetClearingHouse(accountGroup.ClearingHouseEnum);
        }
    }
}
