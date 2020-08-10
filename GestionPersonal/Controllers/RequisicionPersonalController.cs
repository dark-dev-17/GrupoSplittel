using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class RequisicionPersonalController : Controller
    {
        private DarkManager darkManager;
        private SelectList Departamentos;
        private SelectList Puestos;
        private SelectList Ubicaciones;
        private SelectList EstadosCiviles;
        private SelectList Generos;
        public RequisicionPersonalController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.RequisicionPersonal);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Ubicaciones = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        ~RequisicionPersonalController()
        {

        }

        // GET: RequisicionPersonal
        public ActionResult Index()
        {
            var result = darkManager.RequisicionPersonal.Get();
            return View(result);
        }

        // GET: RequisicionPersonal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RequisicionPersonal/Create
        public ActionResult Create()
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Generos"] = Generos;
            ViewData["Motivos"] = darkManager.CatalogoOpcionesValores.Get("1011", nameof(darkManager.CatalogoOpcionesValores.Element.IdCatalogoOpciones)).OrderBy(a=> a.Descripcion).ToList();
            return View(new RequisicionPuesto {
                RequisicionPersonal = new RequisicionPersonal {
                    IdPersona = (int)HttpContext.Session.GetInt32("user_id"),
                    Fecha = DateTime.Now
                },
                Puesto = new Puesto()
            });
        }

        // POST: RequisicionPersonal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequisicionPuesto RequisicionPuesto)
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Generos"] = Generos;
            ViewData["Motivos"] = darkManager.CatalogoOpcionesValores.Get("1011", nameof(darkManager.CatalogoOpcionesValores.Element.IdCatalogoOpciones)).OrderBy(a => a.Descripcion).ToList();
            try
            {
                if(RequisicionPuesto.RequisicionPersonal != null && RequisicionPuesto.RequisicionPersonal.IdPuesto == 0 && RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 2)
                {
                    ModelState.AddModelError("IdPuesto", "Por favor selecciona el puesto a cubrir");
                    return View(RequisicionPuesto);
                }
                if(RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 2)
                {
                    RequisicionPuesto.Puesto = darkManager.Puesto.Get(RequisicionPuesto.RequisicionPersonal.IdPuesto);
                }
                else
                {
                    RequisicionPuesto.Puesto.DPU = "";
                    RequisicionPuesto.Puesto.SalarioMin = 0;
                    RequisicionPuesto.Puesto.SalarioMax = 0;
                }
                
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(RequisicionPuesto);
                }

                

                if (RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 1)
                {
                    //crear puesto
                    darkManager.Puesto.Element = RequisicionPuesto.Puesto;
                    darkManager.Puesto.Element.RequisicionPersonal = 2;
                    if (!darkManager.Puesto.Add())
                    {
                        throw new GpExceptions(darkManager.GetLastMessage());
                    }
                    RequisicionPuesto.Puesto = darkManager.Puesto.Get(darkManager.Puesto.GetLastId());
                }
                RequisicionPuesto.RequisicionPersonal.IdPuesto = RequisicionPuesto.Puesto.IdPuesto;
                RequisicionPuesto.RequisicionPersonal.NombreCompleto = HttpContext.Session.GetString("user_fullname");
                RequisicionPuesto.RequisicionPersonal.Departamento = darkManager.Departamento.Get(RequisicionPuesto.Puesto.IdDepartamento).Nombre;
                RequisicionPuesto.RequisicionPersonal.Fecha = DateTime.Now;
                RequisicionPuesto.RequisicionPersonal.Comentarios = "";
               darkManager.RequisicionPersonal.Element = RequisicionPuesto.RequisicionPersonal;
                if (!darkManager.RequisicionPersonal.Add())
                {
                    darkManager.Puesto.Element = RequisicionPuesto.Puesto;
                    darkManager.Puesto.Delete();
                    throw new GpExceptions(darkManager.GetLastMessage());
                }
                return RedirectToAction("Index");
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(RequisicionPuesto);
            }
        }

        // GET: RequisicionPersonal/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Generos"] = Generos;
            ViewData["Motivos"] = darkManager.CatalogoOpcionesValores.Get("1011", nameof(darkManager.CatalogoOpcionesValores.Element.IdCatalogoOpciones)).OrderBy(a => a.Descripcion).ToList();
            var requi = darkManager.RequisicionPersonal.Get(id);
            return View(new RequisicionPuesto
            {
                RequisicionPersonal = requi,
                Puesto = (requi != null) ? darkManager.Puesto.Get(requi.IdPuesto) : null
            });
        }

        // POST: RequisicionPersonal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequisicionPuesto RequisicionPuesto)
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Generos"] = Generos;
            ViewData["Motivos"] = darkManager.CatalogoOpcionesValores.Get("1011", nameof(darkManager.CatalogoOpcionesValores.Element.IdCatalogoOpciones)).OrderBy(a => a.Descripcion).ToList();
            try
            {
                if (RequisicionPuesto.RequisicionPersonal != null && RequisicionPuesto.RequisicionPersonal.IdPuesto == 0 && RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 2)
                {
                    ModelState.AddModelError("IdPuesto", "Por favor selecciona el puesto a cubrir");
                    return View(RequisicionPuesto);
                }
                if (RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 2)
                {
                    RequisicionPuesto.Puesto = darkManager.Puesto.Get(RequisicionPuesto.RequisicionPersonal.IdPuesto);
                }
                else
                {
                    RequisicionPuesto.Puesto.DPU = "";
                    RequisicionPuesto.Puesto.SalarioMin = 0;
                    RequisicionPuesto.Puesto.SalarioMax = 0;
                }

                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(RequisicionPuesto);
                }



                if (RequisicionPuesto.RequisicionPersonal.TipoRequisicion == 1)
                {
                    //crear puesto
                    darkManager.Puesto.Element = RequisicionPuesto.Puesto;
                    darkManager.Puesto.Element.RequisicionPersonal = 2;
                    if (!darkManager.Puesto.Update())
                    {
                        throw new GpExceptions(darkManager.GetLastMessage());
                    }
                }
                RequisicionPuesto.RequisicionPersonal.IdPuesto = RequisicionPuesto.Puesto.IdPuesto;
                //RequisicionPuesto.RequisicionPersonal.NombreCompleto = HttpContext.Session.GetString("user_fullname");
                //RequisicionPuesto.RequisicionPersonal.Departamento = darkManager.Departamento.Get(RequisicionPuesto.Puesto.IdDepartamento).Nombre;
                //RequisicionPuesto.RequisicionPersonal.Fecha = DateTime.Now;
                RequisicionPuesto.RequisicionPersonal.Comentarios = "";
                darkManager.RequisicionPersonal.Element = RequisicionPuesto.RequisicionPersonal;
                if (!darkManager.RequisicionPersonal.Update())
                {
                    throw new GpExceptions(darkManager.GetLastMessage());
                }
                return RedirectToAction("Index");
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(RequisicionPuesto);
            }
        }

        // GET: RequisicionPersonal/Delete/5
        public ActionResult Aprovar(int id)
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Generos"] = Generos;
            ViewData["Motivos"] = darkManager.CatalogoOpcionesValores.Get("1011", nameof(darkManager.CatalogoOpcionesValores.Element.IdCatalogoOpciones)).OrderBy(a => a.Descripcion).ToList();
            var requi = darkManager.RequisicionPersonal.Get(id);
            return View(new RequisicionPuesto
            {
                RequisicionPersonal = requi,
                Puesto = (requi != null) ? darkManager.Puesto.Get(requi.IdPuesto) : null
            });
        }

        // POST: RequisicionPersonal/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aprovar(int id, string Estatus, string comentarios)
        {
            try
            {
                if (string.IsNullOrEmpty(Estatus) || Estatus != "Aprovar" || Estatus != "Rechazar")
                {
                    throw new GpExceptions("Estatus incorrecto");
                }
                var requi = darkManager.RequisicionPersonal.Get(id);
                requi.IdPersonaAprove = (int)HttpContext.Session.GetInt32("user_id");
                requi.PasoCompletado = Estatus == "Aprovar" ? 1 : 2;
                requi.FechaAprove = DateTime.Now;
                requi.Comentarios = comentarios;
                darkManager.RequisicionPersonal.Element = requi;

                darkManager.RequisicionPersonal.Update();
                return RedirectToAction(nameof(Index));
            }
            catch (GpExceptions ex)
            {
                return View();
            }
        }
    }
}