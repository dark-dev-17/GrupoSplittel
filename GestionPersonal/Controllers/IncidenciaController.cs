using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class IncidenciaController : Controller
    {
        private DarkManager darkManager;

        public IncidenciaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Persona);
        }

        ~IncidenciaController()
        {

        }
        public ActionResult Index(int id)
        {

            id = (int)HttpContext.Session.GetInt32("user_id");
            Incidencias incidencias = new Incidencias();
            incidencias.persona = darkManager.Persona.GetByColumn(""+id,"IdPersona");
            incidencias.permisos = darkManager.IncidenciaPermiso.Get("" + id, "IdPersona");
            incidencias.permisos.ForEach(permiso => {
                var empleado = darkManager.Persona.Get(permiso.IdPersona);
                permiso.EmpleadoNombre = string.Format("{0} {1} {2}", empleado.Nombre, empleado.ApellidoPaterno, empleado.ApellidoMaterno);
                var YipoAsunto = darkManager.CatalogoOpcionesValores.Get(permiso.IdAsunto);
                permiso.DEscripcionTipo = YipoAsunto.Descripcion;
            });
            return View(incidencias);
        }
    }
}