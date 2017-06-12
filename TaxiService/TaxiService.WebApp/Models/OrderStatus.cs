using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.WebApp.Models
{
    public class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Mnemonic { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}