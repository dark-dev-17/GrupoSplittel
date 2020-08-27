using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class LoginController : Controller
    {
        private DarkManager darkManager;

        public LoginController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Usuario);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
        }

        ~LoginController()
        {

        }
        // GET: Login
        public ActionResult DoLogin()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DoLogin");
        }


        // POST: Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoLogin(Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }
               var ResultUser = darkManager.Usuario.Get().Find(a => a.Pass == usuario.Pass && a.UserName == usuario.UserName);
                if(ResultUser == null)
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectas");
                    return View(usuario);
                }
                if (!ResultUser.Activo)
                {
                    ModelState.AddModelError("", "Tu cuenta ha sido desactivada");
                    return View(usuario);
                }
                darkManager.Usuario.Element = ResultUser;
                darkManager.Usuario.Element.UltimoIngreso = DateTime.Now;

                darkManager.Usuario.Update();

                StartSessions(ResultUser);
                return RedirectToAction("Index","Home");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        private void StartSessions(Usuario usuario)
        {
            var ResultUser = darkManager.Persona.Get(usuario.IdPersona);

            HttpContext.Session.SetInt32("user_id", usuario.IdPersona);

            HttpContext.Session.SetInt32("user_id_permiss", usuario.IdUsuario);
            HttpContext.Session.SetString("user_name", ResultUser.Nombre);
            HttpContext.Session.SetString("user_appP", ResultUser.ApellidoPaterno);
            HttpContext.Session.SetString("user_appM", ResultUser.ApellidoMaterno);
            HttpContext.Session.SetString("user_fullname", ResultUser.NombreCompelto);
        }
    }
}