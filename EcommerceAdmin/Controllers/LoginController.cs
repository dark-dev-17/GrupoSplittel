using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAdmin.Models;
using EcomDataProccess;
using System.Configuration;
using Microsoft.AspNetCore.Http;

namespace EcommerceAdmin.Controllers
{
    public class LoginController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataDoLogin(string Username, string Password)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                ecomData.Connect(ServerSource.Splitnet);
                Ecom_Usuario Ecom_Usuario_ = (Ecom_Usuario)ecomData.GetObject(ObjectSource.Usuario);
                bool Result = Ecom_Usuario_.ValidLogin(Username, Password);
                if (Result)
                {
                    StartSessions(Ecom_Usuario_);
                    bool General = ecomData.validPermissAction(Ecom_Usuario_.IdSplinnet, 19);
                    bool Empleado = ecomData.validPermissAction(Ecom_Usuario_.IdSplinnet, 18);
                    ecomData.SaveNotification(Ecom_Usuario_.IdSplinnet, Ecom_Usuario_.IdArea, "success", "Ha iniciado sesión", "", "", "", "");
                    if (General && !Empleado)
                    {
                        return Ok("../Home/General");
                    }
                    else if (!General && Empleado)
                    {
                        return Ok("../Home/Empleado");
                    }
                    else
                    {
                        return Ok("../Home/");
                    }
                }
                else
                {
                    return BadRequest("Usuario o contraseña incorrectas");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                    ecomData.Disconect(ServerSource.Splitnet);
                }
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        private void StartSessions(Ecom_Usuario Ecom_Usuario_)
        {
            HttpContext.Session.SetInt32("USR_IdSplinnet", Ecom_Usuario_.IdSplinnet);
            HttpContext.Session.SetString("USR_Nombre", Ecom_Usuario_.Nombre);
            HttpContext.Session.SetString("USR_ApellidoPaterno", Ecom_Usuario_.ApellidoPaterno);
            HttpContext.Session.SetString("USR_Apellidomaterno", Ecom_Usuario_.Apellidomaterno);
            HttpContext.Session.SetString("USR_Correo", Ecom_Usuario_.Correo);
            HttpContext.Session.SetInt32("USR_IdArea", Ecom_Usuario_.IdArea);
            HttpContext.Session.SetString("USR_Sociedad", Ecom_Usuario_.Sociedad);
            HttpContext.Session.SetString("USR_Foto", Ecom_Usuario_.Foto);
        }
    }
}
