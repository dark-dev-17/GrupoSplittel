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
    public class DepartamentoController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public DepartamentoController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }
        ~DepartamentoController()
        {

        }
        // GET: Departamento
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            Departamento Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento,null);
            IEnumerable<Departamento> Departamentos = Departamento.Get();
            gestionPersonal.CloseConnection();
            return View(Departamentos);
        }

        // GET: Departamento/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Departamento Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
            var result = Departamento.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Departamento/Create
        public ActionResult Create()
        {
            gestionPersonal.OpenConnection();
            DireccionOrganizacional Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
            ViewData["DireccionOrganizacional"] = new SelectList(Sociedad.Get(), "IdDireccion", "Nombre");
            gestionPersonal.CloseConnection();
            return View();
        }

        // POST: Departamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departamento Departamento)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, Departamento);
                    bool result = Departamento.Add();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Departamento);
                    }
                }
                else
                {
                    DireccionOrganizacional Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
                    ViewData["DireccionOrganizacional"] = new SelectList(Sociedad.Get(), "IdDireccion", "Nombre");
                    return View(Departamento);
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
                return View(Departamento);
            }
        }

        // GET: Departamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Departamento Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
            var result = Departamento.Get(id);
            DireccionOrganizacional Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
            ViewData["DireccionOrganizacional"] = new SelectList(Sociedad.Get(), "IdDireccion", "Nombre");
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            
            return View(result);
        }

        // POST: Departamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Departamento Departamento)
        {
            try
            {
                gestionPersonal.OpenConnection();
                if (ModelState.IsValid)
                {
                    
                    Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, Departamento);
                    bool result = Departamento.Update();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Departamento);
                    }
                }
                else
                {
                    DireccionOrganizacional Sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.DireccionOrganizacional, null);
                    ViewData["DireccionOrganizacional"] = new SelectList(Sociedad.Get(), "IdDireccion", "Nombre");
                    return View(Departamento);
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
                return View(Departamento);
            }
        }

        // GET: Departamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            gestionPersonal.OpenConnection();
            Departamento Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, null);
            var result = Departamento.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Departamento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Departamento Departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    Departamento = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Departamento, Departamento);
                    bool result = Departamento.Delete();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Departamento);
                    }
                }
                else
                {
                    return View(Departamento);
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
                return View(Departamento);
            }
        }
    }
}