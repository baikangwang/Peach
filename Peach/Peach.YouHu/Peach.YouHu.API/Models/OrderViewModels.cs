namespace Peah.YouHu.API.Models
{
    using System;

    public class OwnerOrderViewModel
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public OrderState State { get; set; }

        public OwnerOrderViewModel()
        {
        }

        public OwnerOrderViewModel(int id,string driver,string destination, string description,DateTime publishDate,OrderState state)
        {
            this.Id = id;
            this.Driver = driver;
            this.Destination = destination;
            this.Description = description;
            this.PublishedDate = publishDate;
            this.State = state;
        }
    }

    public class DriverOrderViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public DateTime PublishedDate { get; set; }
        public OrderState State { get; set; }

        public DriverOrderViewModel() { }

        public DriverOrderViewModel(int id, string ownerName, string source, string destination, string description, DateTime publishedDate, OrderState state)
        {
            this.Id = id;
            this.OwnerName = ownerName;
            this.Source = source;
            this.Destination = destination;
            this.Description = description;
            this.PublishedDate = publishedDate;
            this.State = state;
        }
    }
}