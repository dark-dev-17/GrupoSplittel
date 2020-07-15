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
    public class DireccionOrganizacionalController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public DireccionOrganizacionalController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }
        ~DireccionOrganizacionalController()
        {

        }
        // GET: DireccionOrganizacional
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            DireccionOrganizacional DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional,null);
            IEnumerable<DireccionOrganizacional> DireccionOrganizacionals = DireccionOrganizacional.Get();
            gestionPersonal.CloseConnection();
            return View(DireccionOrganizacionals);
        }

        // GET: DireccionOrganizacional/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            DireccionOrganizacional DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
            var result = DireccionOrganizacional.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: DireccionOrganizacional/Create
        public ActionResult Create()
        {
            gestionPersonal.OpenConnection();
            Sociedad Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
            var result = Sociedad.Get();
            gestionPersonal.CloseConnection();
            ViewData["Sociedades"] = new SelectList(result, "IdSociedad", "Descripcion");
            return View();
        }

        // POST: DireccionOrganizacional/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DireccionOrganizacional DireccionOrganizacional)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, DireccionOrganizacional);
                    bool result = DireccionOrganizacional.Add();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(DireccionOrganizacional);
                    }
                }
                else
                {
                    Sociedad Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
                    ViewData["Sociedades"] = new SelectList(Sociedad.Get(), "IdSociedad", "Descripcion");
                    return View(DireccionOrganizacional);
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
                return View(DireccionOrganizacional);
            }
        }

        // GET: DireccionOrganizacional/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            DireccionOrganizacional DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
            var result = DireccionOrganizacional.Get(id);
            Sociedad Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
            var resulte = Sociedad.Get();
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            ViewData["Sociedades"] = new SelectList(resulte, "IdSociedad", "Descripcion");
            return View(result);
        }

        // POST: DireccionOrganizacional/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DireccionOrganizacional DireccionOrganizacional)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, DireccionOrganizacional);
                    bool result = DireccionOrganizacional.Update();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(DireccionOrganizacional);
                    }
                }
                else
                {
                    Sociedad Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
                    ViewData["Sociedades"] = new SelectList(Sociedad.Get(), "IdSociedad", "Descripcion");
                    return View(DireccionOrganizacional);
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
                return View(DireccionOrganizacional);
            }
        }

        // GET: DireccionOrganizacional/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            DireccionOrganizacional DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
            var result = DireccionOrganizacional.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: DireccionOrganizacional/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DireccionOrganizacional DireccionOrganizacional)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    DireccionOrganizacional = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, DireccionOrganizacional);
                    bool result = DireccionOrganizacional.Delete();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(DireccionOrganizacional);
                    }
                }
                else
                {
                    return View(DireccionOrganizacional);
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
                return View(DireccionOrganizacional);
            }
        }
    }
}