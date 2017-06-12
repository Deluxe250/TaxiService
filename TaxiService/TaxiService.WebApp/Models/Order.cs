using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.WebApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string DriverId { get; set; }

        public int StatusId { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime Created { get; set; }

        public virtual ApplicationUser Driver { get; set; }

        public virtual OrderStatus Status { get; set; }

        public Order()
        {
            Created = DateTime.Now;
        }
    }
}