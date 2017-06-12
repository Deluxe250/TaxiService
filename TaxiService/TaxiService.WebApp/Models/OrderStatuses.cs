using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.WebApp.Models
{
    public enum OrderStatuses
    {
        New,
        InProgress,
        Done,
        Cancelled
    }
}