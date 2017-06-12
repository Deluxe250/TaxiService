using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiService.DataAccess.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int DriverId { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string StartTime { get; set; }

        public DateTime Created { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
