using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;

namespace VisitorsManagement.Controllers
{
    [BasicAuthentication]
    public class SessionHeartbeatController : Controller
    {
        // GET: SessionHeartbeat
        public void ProcessRequest()
        {
            Session["Heartbeat"] = DateTime.Now;
        }
    }
}