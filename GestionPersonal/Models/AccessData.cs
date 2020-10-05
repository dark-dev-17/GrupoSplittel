using GPSInformation;
using Microsoft.AspNetCore.Http;
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
            darkManager = new DarkManager("Data Source=FMXLTLMARTI01;User ID=sa;Password=C0nnect+1;Initial Catalog=GestionPersonal; Integrated Security=True; Connect timeout=30;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
           
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
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
            darkManager = new DarkManager("Data Source=FMXLTLMARTI01;User ID=sa;Password=C0nnect+1;Initial Catalog=GestionPersonal; Integrated Security=True; Connect timeout=30;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
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
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
    #endregion

}
