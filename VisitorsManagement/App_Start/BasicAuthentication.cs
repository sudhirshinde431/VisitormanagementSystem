using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace VisitorsManagement.App_Start
{
    public class BasicAuthentication : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["UserFullName"] != null)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(HttpContext.Current.Session["UserFullName"].ToString()), null);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }

            //base.OnAuthorization(filterContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
            filterContext.Result = new RedirectResult("~/Login");
        }
    }

    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}