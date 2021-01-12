using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class PuestoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Departamentos;
        private SelectList Puestos;
        private SelectList Ubicaciones;



        public PuestoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            AddSelects(0, 0, 0);
        }

        private void AddSelects(int idDepartamentos, int idPuestos, int idUbicaciones)
        {
            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", idDepartamentos);
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", idPuestos);
            Ubicaciones = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", idUbicaciones);
        }

        ~PuestoController()
        {

        }

        // GET: Puesto
        [AccessMultipleView(IdAction = new int[] { 16,17 })]
        public ActionResult Index()
        {
            var result = darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList();
            result.ForEach(a => {
                a.Departamento = darkManager.Departamento.Get(a.IdDepartamento);
                a.DPU = string.Format("{0}-DPU-{1}", a.Departamento.ClaveDPU, a.NumeroDPU);
            });
            return View(result);
        }

        // GET: Puesto/Details/5
        [AccessMultipleView(IdAction = new int[] { 16, 17 })]
        public ActionResult Details(int id)
        {
            var result = darkManager.Puesto.Get(id);
            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Puesto/Create
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Create()
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            return View();
        }

        // POST: Puesto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Create(Puesto Puesto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    return View(Puesto);
                }
                if(Puesto.NumeroDPU != 0)
                {
                    //var puesto_re = darkManager.Puesto.GetByColumn("" + Puesto.NumeroDPU, "NumeroDPU");
                    var puesto_re = darkManager.Puesto.GetOpenquerys($"where NumeroDPU = '{Puesto.NumeroDPU}' and IdDepartamento = '{Puesto.IdDepartamento}'");
                    if (puesto_re != null)
                    {
                        int max = (int)darkManager.Puesto.GetMax("NumeroDPU", "IdDepartamento", Puesto.IdDepartamento + "");
                        ModelState.AddModelError("NumeroDPU", string.Format("El numero de DPU {0} ya esta siendo usado en otro puesto, numero DPU dispobible {1}", Puesto.NumeroDPU, max));
                        return View(Puesto);
                    }
                }
                else if(Puesto.NumeroDPU == 0)
                {
                    object max = darkManager.Puesto.GetMax("NumeroDPU", "IdDepartamento", Puesto.IdDepartamento + "");
                    if (max is null)
                    {
                        Puesto.NumeroDPU = 1;
                    }
                    else
                    {
                        Puesto.NumeroDPU = (int)max + 1;
                    }
                }

                darkManager.Puesto.Element = Puesto;
                darkManager.Puesto.Element.RequisicionPersonal = 1;
                bool result = darkManager.Puesto.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Puesto);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                ViewData["Departamentos"] = Departamentos;
                ViewData["Puestos"] = Puestos;
                ViewData["Ubicaciones"] = Ubicaciones;
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Edit/5
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Edit(int id)
        {
            
            var result = darkManager.Puesto.Get(id);
            AddSelects(result.IdDepartamento, 0, result.IdUbicacion);
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Puesto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Edit(Puesto Puesto)
        {
            AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            try
            {

                if (!ModelState.IsValid)
                {
                    //AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                    //ViewData["Departamentos"] = Departamentos;
                    //ViewData["Puestos"] = Puestos;
                    //ViewData["Ubicaciones"] = Ubicaciones;
                    return View(Puesto);
                }

                if (Puesto.NumeroDPU != 0)
                {
                    var puesto_re = darkManager.Puesto.GetOpenquerys($"where NumeroDPU = '{Puesto.NumeroDPU}' and IdDepartamento = '{Puesto.IdDepartamento}'");
                    if (puesto_re != null && puesto_re.IdPuesto != Puesto.IdPuesto)
                    {
                        int max = (int)darkManager.Puesto.GetMax("NumeroDPU", "IdDepartamento", Puesto.IdDepartamento + "");
                        ModelState.AddModelError("NumeroDPU", string.Format("El numero de DPU {0} ya esta siendo usado en otro puesto, numero DPU dispobible {1}", Puesto.NumeroDPU, max));
                        return View(Puesto);
                    }
                }
                else if(Puesto.NumeroDPU == 0)
                {
                    object max = darkManager.Puesto.GetMax("NumeroDPU", "IdDepartamento", Puesto.IdDepartamento + "");
                    if(max is null)
                    {
                        Puesto.NumeroDPU = 1;
                    }
                    else
                    {
                        Puesto.NumeroDPU = (int)max + 1;
                    }
                }

                darkManager.Puesto.Element = Puesto;
                bool result = darkManager.Puesto.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                    //ViewData["Departamentos"] = Departamentos;
                    //ViewData["Puestos"] = Puestos;
                    //ViewData["Ubicaciones"] = Ubicaciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Puesto);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                //AddSelects(Puesto.IdDepartamento, 0, Puesto.IdUbicacion);
                //ViewData["Departamentos"] = Departamentos;
                //ViewData["Puestos"] = Puestos;
                //ViewData["Ubicaciones"] = Ubicaciones;
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Delete/5
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Puesto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}