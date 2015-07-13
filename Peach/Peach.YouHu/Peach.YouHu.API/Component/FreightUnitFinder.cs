using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API
{
    public class FreightUnitFinder
    {
        private static FreightUnitFinder _finder=new   FreightUnitFinder();

        public static FreightUnitFinder Default 
        {
            get { return _finder;
            }
        }

        public bool Match(float length,float height, float usize, float weight, float uweight, string location,string source)
        {
            if (source != location) return false;

            if (weight > uweight) return false;

            if (usize > length * height) return false;

            return true;
        }
    }
}