namespace Peah.YouHu.API.Models
{
    public class FreightUnitViewModel
    {
        public int Id { get; set; }

        public int? Rank { get; set; }

        public string Driver { get; set; }

        public string Location { get; set; }

        public decimal Cost { get; set; }

        public FreightUnitState State { get; set; }


        public FreightUnitViewModel() { }

        public FreightUnitViewModel(int id, int? rank, string driver,string location)
        {
            this.Id = id;
            this.Rank = rank;
            this.Driver = driver;
            this.Location = location;
        }
    }
}