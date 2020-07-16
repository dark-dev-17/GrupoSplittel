using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPDataInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class SplittelEmpleadoController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public SplittelEmpleadoController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }
        // GET: SplittelEmpleado
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            Persona persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, null);
            var Data = persona.Get();
            gestionPersonal.CloseConnection();
            return View(Data);
        }

        // GET: SplittelEmpleado/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: SplittelEmpleado/Create
        public ActionResult Create()
        {
            gestionPersonal.OpenConnection();
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            gestionPersonal.CloseConnection();
            return View(new SplittelEmpleado());
        }
        
        // GET: SplittelEmpleado/Create
        public ActionResult CreateValid(SplittelEmpleado splittelEmpleado)
        {
            if(splittelEmpleado == null)
            {
                splittelEmpleado = new SplittelEmpleado();
            }
            gestionPersonal.OpenConnection();
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            gestionPersonal.CloseConnection();
            return View(splittelEmpleado);
        }
        public ActionResult EditValid(SplittelEmpleado splittelEmpleado)
        {
            if (splittelEmpleado == null)
            {
                splittelEmpleado = new SplittelEmpleado();
            }
            gestionPersonal.OpenConnection();
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            gestionPersonal.CloseConnection();
            return View(splittelEmpleado);
        }
        // POST: SplittelEmpleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SplittelEmpleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            SplittelEmpleado SplittelEmpleado = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.SplittelEmpleado, null);
            var Data = SplittelEmpleado.Get(id);
            if (Data == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
            //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
            ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
            gestionPersonal.CloseConnection();
            return View(SplittelEmpleado);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersona(Persona Persona)
        {
            try
            {
                gestionPersonal.OpenConnection();

                if (ModelState.IsValid)
                {

                    Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, Persona);
                    bool result = Persona.Add();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                        splittelEmpleado.persona = Persona;
                        gestionPersonal.OpenConnection();
                        CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                        //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                        ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                        ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                        gestionPersonal.CloseConnection();
                        return View("CreateValid", splittelEmpleado);
                    }
                }
                else
                {
                    SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                    splittelEmpleado.persona = Persona;
                    gestionPersonal.OpenConnection();
                    CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                    //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                    ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    gestionPersonal.CloseConnection();
                    return View("CreateValid", splittelEmpleado);
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
                SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                splittelEmpleado.persona = Persona;
                gestionPersonal.OpenConnection();
                CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                gestionPersonal.CloseConnection();
                return View("CreateValid", splittelEmpleado);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersona(Persona Persona)
        {
            try
            {
                gestionPersonal.OpenConnection();

                if (ModelState.IsValid)
                {

                    Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, Persona);
                    bool result = Persona.Update();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                        splittelEmpleado.persona = Persona;
                        gestionPersonal.OpenConnection();
                        CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                        //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                        ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                        ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                        gestionPersonal.CloseConnection();
                        return View("EditValid", splittelEmpleado);
                    }
                }
                else
                {
                    SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                    splittelEmpleado.persona = Persona;
                    gestionPersonal.OpenConnection();
                    CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                    //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                    ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                    gestionPersonal.CloseConnection();
                    return View("EditValid", splittelEmpleado);
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
                SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                splittelEmpleado.persona = Persona;
                gestionPersonal.OpenConnection();
                CatalogoOpciones CatalogoOpciones = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.CatalogoOpciones, null);
                //Puesto Puesto = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Puesto, null);
                ViewData["Generos"] = new SelectList(CatalogoOpciones.Get(2).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                ViewData["EstadosCiviles"] = new SelectList(CatalogoOpciones.Get(3).Opciones, "IdCatalogoOpcionesValores", "Descripcion");
                gestionPersonal.CloseConnection();
                return View("EditValid", splittelEmpleado);
            }
        }
    }
}