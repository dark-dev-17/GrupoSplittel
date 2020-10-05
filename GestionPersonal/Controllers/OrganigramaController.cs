using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class OrganigramaController : Controller
    {
        private DarkManager darkManager;

        public OrganigramaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.OrganigramaVersion);
            darkManager.LoadObject(GpsManagerObjects.OrganigramaStructura);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
        }

        ~OrganigramaController()
        {

        }

        [AccessMultipleView(IdAction = new int[] { 4,5,9 })]
        public ActionResult Index()
        {
            var result = darkManager.OrganigramaVersion.Get();
            return View(result);
        }
        [AccessMultipleView(IdAction = new int[] { 4, 5, 9 })]
        public ActionResult Details(int id)
        {
            return View();
        }
        [AccessMultipleView(IdAction = new int[] { 5 })]
        public ActionResult Edit(int id)
        {
            var result = darkManager.OrganigramaVersion.Get(id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                return NotFound();
            }
        }
        [AccessMultipleView(IdAction = new int[] { 5 })]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult<List<PuestoOrg>> GetPuestos()
        {
            List<PuestoOrg> puestoOrgs =  ListPuestos();

            return Ok(puestoOrgs);
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult CreateNewVersion()
        {
            darkManager.OrganigramaVersion.Element = new GPSInformation.Models.OrganigramaVersion();
            darkManager.OrganigramaVersion.Element.FechaCreacion = DateTime.Now;
            darkManager.OrganigramaVersion.Element.FechaActualizacion = DateTime.Now;
            darkManager.OrganigramaVersion.Element.Autirizada = 1;
            var result = darkManager.OrganigramaVersion.Add();
            if (result)
            {
                return RedirectToAction("Edit", new { id = darkManager.OrganigramaVersion.GetLastId() });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult AddFirstNode(int IdPuesto, int IdVersion)
        {

            List<PuestoOrg> puestoOrgs = ListPuestos();


            var puestoChild = puestoOrgs.Find( a=> a.IdPuesto == IdPuesto);
            if(puestoChild == null)
                return BadRequest("El puesto hijo no existe");


            darkManager.OrganigramaStructura.Element = new OrganigramaStructura();
            darkManager.OrganigramaStructura.Element.FechaCreacion = DateTime.Now;
            darkManager.OrganigramaStructura.Element.IdOrganigramaVersion = IdVersion;
            darkManager.OrganigramaStructura.Element.IdPuesto = IdPuesto;
            darkManager.OrganigramaStructura.Element.IdPuestoParent = 0; // add null to the database
            darkManager.OrganigramaStructura.Element.DPU = puestoChild.DPU;
            darkManager.OrganigramaStructura.Element.Descripcion = puestoChild.Descripcion;

            if(darkManager.OrganigramaStructura.Get("" + IdPuesto, "IdPuesto").Count > 0)
            {
                return BadRequest("ya existe este puesto en el organigrama");
            }

            var result = darkManager.OrganigramaStructura.Add();
            if (result)
            {
                return Ok("Puesto agregado!");
            }
            else
            {
                return BadRequest("Puesto no agregado");
            }
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult AddNode(int IdPuesto, int IdPuestoParent, int IdVersion)
        {

            List<PuestoOrg> puestoOrgs = ListPuestos();


            var puestoChild = puestoOrgs.Find(a => a.IdPuesto == IdPuesto);
            if (puestoChild == null)
                return BadRequest("El puesto hijo no existe");

            var puestoparent = puestoOrgs.Find(a => a.IdPuesto == IdPuestoParent);
            if (puestoChild == null)
                return BadRequest("El puesto padre no existe");

            darkManager.OrganigramaStructura.Element = new OrganigramaStructura();
            darkManager.OrganigramaStructura.Element.FechaCreacion = DateTime.Now;
            darkManager.OrganigramaStructura.Element.IdOrganigramaVersion = IdVersion;
            darkManager.OrganigramaStructura.Element.IdPuesto = IdPuesto;
            darkManager.OrganigramaStructura.Element.IdPuestoParent = IdPuestoParent;
            darkManager.OrganigramaStructura.Element.DPU = puestoChild.DPU;
            darkManager.OrganigramaStructura.Element.Descripcion = puestoChild.Descripcion;

            if (darkManager.OrganigramaStructura.Get("" + IdPuesto, "IdPuesto").Count > 0)
            {
                return BadRequest("ya existe este puesto en el organigrama");
            }

            var result = darkManager.OrganigramaStructura.Add();
            if (result)
            {
                return Ok("Puesto agregado!");
            }
            else
            {
                return BadRequest("Puesto no agregado");
            }
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult GetNodes(int IdVersion)
        {
            var result = darkManager.OrganigramaStructura.Get(""+ IdVersion, "IdOrganigramaVersion");
            return Ok(result);
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 9 })]
        public ActionResult Autorizar(int IdVersion)
        {
            var result = darkManager.OrganigramaVersion.Get();
            var Autorizado = result.Find(a => a.IdOrganigramaVersion == IdVersion);
            if(Autorizado == null)
            {
                return NotFound("No fue contrado la version a autorizar");
            }

            darkManager.OrganigramaVersion.Element = Autorizado;
            darkManager.OrganigramaVersion.Element.Autirizada = 2; // estatus de version autorizada, dicha version se utlizara para el flujo de provaciones en incidencias y otro tipo de solicitudes

            if (darkManager.OrganigramaVersion.Update() ==false)
            {
                return NotFound("Error al autorizar esta versión solicitada");
            }
            result.Where(a => a.Autirizada == 2 && a.IdOrganigramaVersion != IdVersion).ToList().ForEach(organigrama => {
                darkManager.OrganigramaVersion.Element = organigrama;
                darkManager.OrganigramaVersion.Element.Autirizada = 1; // estatus de version no autorizada
            });

            result = darkManager.OrganigramaVersion.Get();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult ChangeNode(int IdOrganigramaStructura, int IdPuesto, int IdPuestoParent)
        {
            var puestoChild = darkManager.OrganigramaStructura.Get(IdOrganigramaStructura);

            if (puestoChild == null)
                return BadRequest("El puesto no ha sido agregado al organigrama");


            List<PuestoOrg> puestoOrgs = ListPuestos();


            var puestoChilds= puestoOrgs.Find(a => a.IdPuesto == IdPuesto);
            if (puestoChild == null)
                return BadRequest("El puesto hijo no existe");

            var puestoparent = puestoOrgs.Find(a => a.IdPuesto == IdPuestoParent);
            if (puestoChild == null)
                return BadRequest("El puesto padre no existe");



            darkManager.OrganigramaStructura.Element = puestoChild;
            darkManager.OrganigramaStructura.Element.IdPuesto = IdPuesto;
            darkManager.OrganigramaStructura.Element.IdPuestoParent = IdPuestoParent;
            darkManager.OrganigramaStructura.Element.FechaCreacion = DateTime.Now;

            var result = darkManager.OrganigramaVersion.Update();
            if (result)
            {
                return Ok("Puesto eliminado!");
            }
            else
            {
                return BadRequest("Puesto no eliminado!");
            }
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 5 })]
        public ActionResult Remove(int IdPuesto, int IdVersion)
        {
            var result = darkManager.OrganigramaStructura.Get("" + IdVersion, "IdOrganigramaVersion");
            var puestoChild = result.Find(a=> a.IdPuesto == IdPuesto);

            if (puestoChild == null)
                return BadRequest("El puesto no ha sido agregado al organigrama");

            darkManager.OrganigramaStructura.Element = puestoChild;
            
            var result2 = darkManager.OrganigramaStructura.Delete();
            if (result2)
            {
                return Ok("Puesto eliminado!");
            }
            else
            {
                return BadRequest("Puesto no eliminado!");
            }
        }
        
        [AccessDataSession(IdAction = new int[] { 5 })]
        private List<PuestoOrg> ListPuestos()
        {
            List<PuestoOrg> puestoOrgs = new List<PuestoOrg>();
            var departamentos = darkManager.Departamento.Get();

            darkManager.Puesto.Get().ForEach(puesto => {
                puestoOrgs.Add(new PuestoOrg
                {
                    IdPuesto = puesto.IdPuesto,
                    DPU = string.Format("{0}-DPU-{1}", departamentos.Find(a => a.IdDepartamento == puesto.IdDepartamento).ClaveDPU, puesto.DPU),
                    Descripcion = puesto.Nombre
                });
            });

            return puestoOrgs.OrderBy(a => a.Descripcion).ToList();
        }


    }
}