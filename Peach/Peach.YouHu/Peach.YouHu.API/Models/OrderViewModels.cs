using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
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
}