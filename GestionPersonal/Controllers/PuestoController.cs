using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GPDataInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class PuestoController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public PuestoController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }
        ~PuestoController()
        {

        }
        // GET: Puesto
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto,null);
            IEnumerable<Puesto> Puestos = Puesto.Get();
            gestionPersonal.CloseConnection();
            return View(Puestos);
        }

        // GET: Puesto/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            var result = Puesto.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Puesto/Create
        public ActionResult Create()
        {
            gestionPersonal.OpenConnection();
            Departamento departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            ViewData["Departamentos"] = new SelectList(departamento.Get(), "IdDepartamento", "Nombre");
            ViewData["Ubicaciones"] = new SelectList(CatalogoOpciones.Get(1).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Puestos"] = new SelectList(Puesto.Get(), "IdPuesto", "Nombre");

            gestionPersonal.CloseConnection();
            return View();
        }

        // POST: Puesto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Puesto Puesto)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, Puesto);
                    bool result = Puesto.Add();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Puesto);
                    }
                }
                else
                {
                    Departamento departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
                    CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                    Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                    ViewData["Departamentos"] = new SelectList(departamento.Get(), "IdDepartamento", "Nombre");
                    ViewData["Ubicaciones"] = new SelectList(CatalogoOpciones.Get(1).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    ViewData["Puestos"] = new SelectList(Puesto.Get(), "IdPuesto", "Nombre");
                    return View(Puesto);
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                if(gestionPersonal != null)
                {
                    gestionPersonal.CloseConnection();
                    gestionPersonal = null;
                }
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            var result = Puesto.Get(id);
            Departamento departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            ViewData["Departamentos"] = new SelectList(departamento.Get(), "IdDepartamento", "Nombre");
            ViewData["Ubicaciones"] = new SelectList(CatalogoOpciones.Get(1).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Puestos"] = new SelectList(Puesto.Get(), "IdPuesto", "Nombre");
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            
            return View(result);
        }

        // POST: Puesto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Puesto Puesto)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, Puesto);
                    bool result = Puesto.Update();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Puesto);
                    }
                }
                else
                {
                    Departamento departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
                    CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                    Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                    ViewData["Departamentos"] = new SelectList(departamento.Get(), "IdDepartamento", "Nombre");
                    ViewData["Ubicaciones"] = new SelectList(CatalogoOpciones.Get(1).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    ViewData["Puestos"] = new SelectList(Puesto.Get(), "IdPuesto", "Nombre");
                    return View(Puesto);
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                if (gestionPersonal != null)
                {
                    gestionPersonal.CloseConnection();
                    gestionPersonal = null;
                }
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            var result = Puesto.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Puesto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Puesto Puesto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, Puesto);
                    bool result = Puesto.Delete();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Puesto);
                    }
                }
                else
                {
                    return View(Puesto);
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                if (gestionPersonal != null)
                {
                    gestionPersonal.CloseConnection();
                    gestionPersonal = null;
                }
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }
    }
}