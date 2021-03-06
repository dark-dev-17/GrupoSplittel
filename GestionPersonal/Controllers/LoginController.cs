﻿using System;
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
            darkManager.LoadObject(GpsManagerObjects.View_empleado);
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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("url_next")))
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return Redirect(HttpContext.Session.GetString("url_next"));
                }
                
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
            HttpContext.Session.SetString("user_RFC", ResultUser.RFC);
            HttpContext.Session.SetString("user_imagenPerfil", usuario.ImagenDefault ? "imagenperfil.png" : usuario.ImagenPerfil);
            HttpContext.Session.SetString("user_fullname", ResultUser.NombreCompelto);
            HttpContext.Session.SetString("user_puesto", darkManager.View_empleado.Get(usuario.IdPersona).PuestoNombre);
            HttpContext.Session.SetString("user_accesos", "");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("user_id");
            HttpContext.Session.Remove("user_id_permiss");
            HttpContext.Session.Remove("user_name");
            HttpContext.Session.Remove("user_appP");
            HttpContext.Session.Remove("user_appM");
            HttpContext.Session.Remove("user_RFC");
            HttpContext.Session.Remove("user_imagenPerfil");
            HttpContext.Session.Remove("user_fullname");
            HttpContext.Session.Remove("user_puesto");
            HttpContext.Session.Remove("user_accesos");
            return RedirectToAction("DoLogin");
        }
    }
}