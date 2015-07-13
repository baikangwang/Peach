using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API
{
    public class FreightCostCalculator
    {
        private static FreightCostCalculator _default=new FreightCostCalculator();

        public static FreightCostCalculator Default
        {
            get
            {
                return _default;
            }
        }

        public decimal Calculate(string source,string detination,float size,float weight)
        {
            return 5000.0m;
        }
    }
}