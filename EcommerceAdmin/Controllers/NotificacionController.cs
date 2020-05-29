using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAdmin.Models;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class NotificacionController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [AccessViewSession]
        public ActionResult List()
        {
            Ecommerce Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Splitnet);
                List<Ecom_Notificacion> NotificacionesNotificaciones = Ecommerce_.ecomData.getNotifications((int)Ecommerce_.session.GetInt32("USR_IdSplinnet"), (int)Ecommerce_.session.GetInt32("USR_IdArea"));
                return PartialView(NotificacionesNotificaciones);
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
            }

            
        }

    }
}