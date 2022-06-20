using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Model;

namespace WebApplication.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        private readonly IAuditsBL _auditsBL;

        public ValuesController(IAuditsBL audits)
        {
           _auditsBL = audits;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { $"{_auditsBL.SetTitle()}" };
        }
    }
}
