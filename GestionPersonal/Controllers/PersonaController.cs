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
    public class PersonaController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public PersonaController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }
        ~PersonaController()
        {

        }
        // GET: Persona
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            Persona Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona,null);
            IEnumerable<Persona> Personas = Persona.Get();
            gestionPersonal.CloseConnection();
            return View(Personas);
        }

        // GET: Persona/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Persona Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, null);
            var result = Persona.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Persona/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Persona/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona Persona)
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
                        return RedirectToAction("Index", "SplittelEmpleado");
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                        splittelEmpleado.persona = Persona;
                        return RedirectToAction("CreateValid", "SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
                    }
                }
                else
                {
                    SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                    splittelEmpleado.persona = Persona;
                    return RedirectToAction("CreateValid", "SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
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
                SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                splittelEmpleado.persona = Persona;
                return RedirectToAction("CreateValid", "SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
            }
        }

        // GET: Persona/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Persona Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, null);
            var result = Persona.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            
            return View(result);
        }

        // POST: Persona/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Persona Persona)
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
                        return RedirectToAction("CreateValid","SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
                    }
                }
                else
                {
                    SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
                    splittelEmpleado.persona = Persona;
                    return RedirectToAction("CreateValid", "SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
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
                return RedirectToAction("CreateValid", "SplittelEmpleado", new { splittelEmpleado = splittelEmpleado });
            }
        }

        // GET: Persona/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Persona Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, null);
            var result = Persona.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Persona/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Persona Persona)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    Persona = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Persona, Persona);
                    bool result = Persona.Delete();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Persona);
                    }
                }
                else
                {
                    return View(Persona);
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
                return View(Persona);
            }
        }
    }
}