using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin.Models.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessData : ActionFilterAttribute
    {
        public int IdAction { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {
                int USR_IdSplinnet = (int)filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet");
                string EcomConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
                EcomDataProccess.Ecom_DBConnection Ecom_DBConnection_ = new EcomDataProccess.Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessAuth = new EcomDataProccess.Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, IdAction);
                Ecom_DBConnection_.CloseConnection();
                if (!AccessAuth)
                {
                    filterContext.Result = new BadRequestObjectResult("no access to this action, please check your permissions");
                    return;
                }
            }
            else
            {
                filterContext.Result = new BadRequestObjectResult("Please log in");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessDataSession : ActionFilterAttribute
    {
        public int IdAction { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {

            }
            else
            {
                filterContext.Result = new BadRequestObjectResult("Please log in");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessView : ActionFilterAttribute
    {
        public int IdAction { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {
                int USR_IdSplinnet = (int)filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet");
                string EcomConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
                EcomDataProccess.Ecom_DBConnection Ecom_DBConnection_ = new EcomDataProccess.Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessAuth = new EcomDataProccess.Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, IdAction);
                Ecom_DBConnection_.CloseConnection();
                if (!AccessAuth)
                {
                    filterContext.Result = new RedirectResult("~/ErrorPages/NoAccess");
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessMultipleView : ActionFilterAttribute
    {
        public int [] IdAction { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {
                int USR_IdSplinnet = (int)filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet");
                string EcomConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
                EcomDataProccess.Ecom_DBConnection Ecom_DBConnection_ = new EcomDataProccess.Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessAuth = new EcomDataProccess.Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, IdAction);
                Ecom_DBConnection_.CloseConnection();
                if (!AccessAuth)
                {
                    filterContext.Result = new RedirectResult("~/ErrorPages/NoAccess");
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
