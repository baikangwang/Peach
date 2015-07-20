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

        public bool Match(decimal length,decimal height, decimal usize, decimal weight, decimal uweight, string location,string source)
        {
            if (source != location) return false;

            if (weight > uweight) return false;

            if (usize > length * height) return false;

            return true;
        }
    }
}