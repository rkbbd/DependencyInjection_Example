using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Model
{
    public interface IAuditsBL
    {
        string SetTitle();
    }

    public class AuditsBL : IAuditsBL
    {
        public string SetTitle()
        {
            return "Title replaced";
        }
    }
}