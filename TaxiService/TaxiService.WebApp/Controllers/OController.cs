using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxiService.WebApp.Controllers
{
    public class OController : ApiController
    {
        // GET: api/O
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/O/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/O
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/O/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/O/5
        public void Delete(int id)
        {
        }
    }
}
