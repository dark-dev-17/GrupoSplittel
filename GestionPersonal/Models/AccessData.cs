using GPSInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessDataSession : ActionFilterAttribute
    {
        private DarkManager darkManager;
        public int[] IdAction { get; set; }
        public AccessDataSession()
        {
            darkManager = new DarkManager("Data Source=192.168.3.160;User ID=infra;Password=SAPca+Ah*76U19$*;Initial Catalog=GestionPersonal_dev; Integrated Security=false; Connect timeout=60;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
           
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("user_id_permiss") != null)
            {
                if (IdAction.Length > 0)
                {
                    darkManager.OpenConnection();
                    darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
                    var result = darkManager.AccesosSistema.Get("" + filterContext.HttpContext.Session.GetInt32("user_id_permiss"), nameof(darkManager.AccesosSistema.Element.IdUsuario));
                    bool autorize = false;

                    foreach (var item in IdAction)
                    {
                        if (result.Find(a => a.IdSubModulo == item && a.TieneAcceso) != null)
                        {
                            autorize = true;
                        }
                    }

                    if (!autorize)
                    {
                        filterContext.Result = new BadRequestObjectResult("Sin permisos");
                        return;
                    }
                    darkManager.CloseConnection();
                }
            }
            else
            {
                filterContext.Result = new BadRequestObjectResult("Por favor inicia sesión");
                return;
            }
            
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }

    #region AccessMultipleView
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessMultipleView : ActionFilterAttribute
    {
        private DarkManager darkManager;
        public int[] IdAction { get; set; }
        

        public AccessMultipleView()
        {
            darkManager = new DarkManager("Data Source=192.168.3.160;User ID=infra;Password=SAPca+Ah*76U19$*;Initial Catalog=GestionPersonal_dev; Integrated Security=false; Connect timeout=60;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
            //darkManager.OpenConnection();
            //darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("user_id_permiss") != null)
            {
                if(IdAction.Length > 0)
                {
                    darkManager.OpenConnection();
                    darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
                    var result = darkManager.AccesosSistema.Get("" + filterContext.HttpContext.Session.GetInt32("user_id_permiss"), nameof(darkManager.AccesosSistema.Element.IdUsuario));
                    bool autorize = false;

                    foreach (var item in IdAction)
                    {
                        if (result.Find(a => a.IdSubModulo == item && a.TieneAcceso) != null)
                        {

                            autorize = true;
                        }
                    }

                    if (!autorize)
                    {
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "../ErrorPages/NoAccess",
                        };
                        return;
                    }
                    darkManager.CloseConnection();
                }
            }
            else
            {
                var isHtps = filterContext.HttpContext.Request.IsHttps;
                var Host = filterContext.HttpContext.Request.Host;
                var Path = filterContext.HttpContext.Request.Path;
                string url = string.Format("{0}//{1}{2}",(isHtps ? "https:" : "http:"), Host, Path);
                filterContext.HttpContext.Session.SetString("url_next",url);
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
    #endregion

    #region AccessView
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessView : ActionFilterAttribute
    {
        private DarkManager darkManager;

        public AccessView()
        {
            darkManager = new DarkManager("Data Source=192.168.3.160;User ID=infra;Password=SAPca+Ah*76U19$*;Initial Catalog=GestionPersonal_dev; Integrated Security=false; Connect timeout=60;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
            //darkManager.OpenConnection();
            //darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("user_id_permiss") != null)
            {
                
            }
            else
            {
                filterContext.Result = new RedirectResult("~/");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
    #endregion

}
