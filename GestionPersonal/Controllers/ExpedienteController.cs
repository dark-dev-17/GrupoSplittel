using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class ExpedienteController : Controller
    {
        private ExpedienteCtrl ExpedienteCtrl;

        public ExpedienteController(IConfiguration configuration)
        {
            ExpedienteCtrl = new ExpedienteCtrl(new DarkManager(configuration));
        }

        // GET: ExpedienteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExpedienteController/Details/5
        public ActionResult Expediente(int id)
        {
            try
            {
                ViewData["Documentos"] = new SelectList(ExpedienteCtrl.GetTipoDocumentos().ToList(), "IdExpedienteArchivo","Nombre");
                ViewData["IdPersona"] = id;
                return View(ExpedienteCtrl.GetExpediente(id));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
        }

        // GET: ExpedienteController/Create
        public FileResult Dowload(int IdPersona, int IdExpedienteArchivo)
        {
            byte[] fileBytes = ExpedienteCtrl.GetFile(IdPersona, IdExpedienteArchivo);
            string fileName = ExpedienteCtrl.GetFileDetails(IdPersona, IdExpedienteArchivo).Ruta;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // POST: ExpedienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(int IdPersona, int IdExpedienteArchivo, IFormFile Archivo)
        {
            try
            {
                await ExpedienteCtrl.AddArchivoAsync(IdPersona, IdExpedienteArchivo, Archivo);
                return RedirectToAction(nameof(Index), new { id = IdPersona });
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
        }

        // GET: ExpedienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExpedienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpedienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExpedienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
