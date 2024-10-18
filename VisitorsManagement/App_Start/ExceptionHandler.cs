using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VisitorsManagement.App_Start
{
    public class ExceptionHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled)
            //{
            //    return;
            //}
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error2"
            //};
            DB.insertErrorlog("Global", "", e.Message, 0);
        }
    }
}