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

        public decimal Calculate(string source,string detination,decimal size,decimal weight)
        {
            return 5000.0m;
        }
    }
}