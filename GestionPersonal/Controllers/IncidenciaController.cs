using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class IncidenciaController : Controller
    {
        private DarkManager darkManager;
        private IncidenciaCtrl IncidenciaCtrl;

        public IncidenciaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.OrganigramaStructura);
            darkManager.LoadObject(GpsManagerObjects.OrganigramaVersion);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
            darkManager.LoadObject(GpsManagerObjects.VacionesPeriodo);

            IncidenciaCtrl = new IncidenciaCtrl(new DarkManager(configuration));
        }

        ~IncidenciaController()
        {

        }
        [AccessMultipleView(IdAction = new int[] { 30 })]
        // mis solicitudes
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
            incidencias.vacaciones = darkManager.IncidenciaVacacion.Get("" + id, "IdPersona");
            ViewData["tab"] = "Permisos";
            ViewData["periodos"] = new VacacionesCtrl((int)HttpContext.Session.GetInt32("user_id"), darkManager).Get();
            return View(incidencias);
        }

        [AccessMultipleView(IdAction = new int[] { 32 })]
        public ActionResult AprobarJefe(string tab)
        {
            int id = (int) HttpContext.Session.GetInt32("user_id");


            var VersionOgr = darkManager.OrganigramaVersion.GetByColumn("2", nameof(darkManager.OrganigramaVersion.Element.Autirizada));

            var structuraOrg = darkManager.OrganigramaStructura.Get(
                VersionOgr.IdOrganigramaVersion + "",
                nameof(darkManager.OrganigramaStructura.Element.IdOrganigramaVersion)
                ).Where(a => a.IdPuestoParent == darkManager.Empleado.GetByColumn("" + id, "IdPersona").IdPuesto).ToList();

            Incidencias incidencias = new Incidencias { permisos =  new List<GPSInformation.Models.IncidenciaPermiso>(), vacaciones =  new List<GPSInformation.Models.IncidenciaVacacion>()};

            structuraOrg.ForEach(a => {
                darkManager.Empleado.Get("" + a.IdPuesto, "IdPuesto").ForEach(emp => {
                    var solicitante = darkManager.Persona.GetByColumn("" + emp.IdPersona, "IdPersona");
                    incidencias.persona = darkManager.Persona.GetByColumn("" + solicitante.IdPersona, "IdPersona");
                    var permisos = darkManager.IncidenciaPermiso.Get("" + solicitante.IdPersona, "IdPersona");
                    permisos.ForEach(permiso => {
                        var empleado = darkManager.Persona.Get(permiso.IdPersona);
                        permiso.EmpleadoNombre = string.Format("{0} {1} {2}", empleado.Nombre, empleado.ApellidoPaterno, empleado.ApellidoMaterno);
                        var YipoAsunto = darkManager.CatalogoOpcionesValores.Get(permiso.IdAsunto);
                        permiso.DEscripcionTipo = YipoAsunto.Descripcion;
                    });
                    var vacaciones = darkManager.IncidenciaVacacion.Get("" + solicitante.IdPersona, "IdPersona");
                    permisos.ForEach(p => incidencias.permisos.Add(p));
                    vacaciones.ForEach(p => incidencias.vacaciones.Add(p));

                });
                
            });

            incidencias.permisos.ForEach(a => a.Proceso = darkManager.IncidenciaProcess.Get(a.IdIncidenciaPermiso+"", "IdIncidenciaPermiso"));
            incidencias.vacaciones.ForEach(a => a.Proceso = darkManager.IncidenciaProcess.Get(a.IdIncidenciaVacacion+"", "IdIncidenciaVacacion"));
            ViewData["tab"] = "Permisos";

            darkManager.CloseConnection();
            return View(incidencias);
        }
        [AccessMultipleView(IdAction = new int[] { 36 })]
        public ActionResult AprobarGPS()
        {
            int id = (int)HttpContext.Session.GetInt32("user_id");

            Incidencias incidencias = new Incidencias { permisos = new List<GPSInformation.Models.IncidenciaPermiso>(), vacaciones = new List<GPSInformation.Models.IncidenciaVacacion>() };

            var permisos = darkManager.IncidenciaPermiso.GetIn(new int[] {1,2,3 } , "Estatus");
            permisos.ForEach(permiso => {
                var empleado = darkManager.Persona.Get(permiso.IdPersona);
                permiso.EmpleadoNombre = string.Format("{0} {1} {2}", empleado.Nombre, empleado.ApellidoPaterno, empleado.ApellidoMaterno);
                var YipoAsunto = darkManager.CatalogoOpcionesValores.Get(permiso.IdAsunto);
                permiso.DEscripcionTipo = YipoAsunto.Descripcion;
            });
            var vacaciones = darkManager.IncidenciaVacacion.Get("1", "Estatus");
            permisos.ForEach(p => incidencias.permisos.Add(p));
            vacaciones.ForEach(p => incidencias.vacaciones.Add(p));

            incidencias.permisos.ForEach(a => a.Proceso = darkManager.IncidenciaProcess.Get(a.IdIncidenciaPermiso + "", "IdIncidenciaPermiso"));
            incidencias.vacaciones.ForEach(a => a.Proceso = darkManager.IncidenciaProcess.Get(a.IdIncidenciaVacacion + "", "IdIncidenciaVacacion"));
            ViewData["tab"] = "Permisos";
            darkManager.CloseConnection();
            return View(incidencias);
        }

        [AccessMultipleView(IdAction = new int[] { 36 })]
        public ActionResult ProcesarIncidencias(DateTime Fecha)
        {
            IncidenciaCtrl.ProcessPermisosComplete(Fecha);
            IncidenciaCtrl.ProcessVacacionesComplete(Fecha);
            return Ok("Ok");
        }


    }
}