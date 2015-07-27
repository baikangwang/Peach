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

        public bool Match(decimal ulength,decimal uheight, decimal osize, decimal weight, decimal uweight, string location,string source)
        {
            if (source != location) return false;

            if (weight > uweight) return false;

            if (osize > ulength * uheight) return false;

            return true;
        }
    }
}