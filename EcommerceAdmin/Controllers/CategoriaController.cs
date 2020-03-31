using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        // GET: Categoria
        [AccessView(IdAction = 30)]
        public ActionResult Index()
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoCategoria Ecom_ProductoCategoria_ = (Ecom_ProductoCategoria)ecomData.GetObject(ObjectSource.ProductoCategoria);
                List<Ecom_ProductoCategoria> result = Ecom_ProductoCategoria_.Get();
                return View(result);
            }
            catch (Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: Categoria/Details/5
        [AccessView(IdAction = 30)]
        public ActionResult Details(string id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoCategoria Ecom_ProductoCategoria_ = (Ecom_ProductoCategoria)ecomData.GetObject(ObjectSource.ProductoCategoria);
                bool result = Ecom_ProductoCategoria_.Get(id);
                if (result)
                {
                    return View(Ecom_ProductoCategoria_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = string.Format("El producto con codigo: '{0}' no fue encontrado", id) });
                }
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
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

        // GET: Categoria/Edit/5
        [AccessMultipleView(IdAction = new int[] { 31, 32 })]
        public ActionResult Edit(string id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoCategoria Ecom_ProductoCategoria_ = (Ecom_ProductoCategoria)ecomData.GetObject(ObjectSource.ProductoCategoria);
                bool result = Ecom_ProductoCategoria_.Get(id);
                if (result)
                {
                    return View(Ecom_ProductoCategoria_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = string.Format("El producto con codigo: '{0}' no fue encontrado", id) });
                }
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ecom_ProductoCategoria Ecom_ProductoCategoria_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                ecomData.Connect(ServerSource.Splitnet);
                bool AdminPermiss = ecomData.validPermissAction(USR_IdSplinnet, 31);
                bool basicPermiss = ecomData.validPermissAction(USR_IdSplinnet, 32);
                Ecom_ProductoCategoria_ = (Ecom_ProductoCategoria)ecomData.SetObjectConnection(Ecom_ProductoCategoria_, ObjectSource.ProductoCategoria);
                bool result = false;
                if (AdminPermiss && !basicPermiss)
                {
                    result = Ecom_ProductoCategoria_.Update(2);
                }
                else if (!AdminPermiss && basicPermiss)
                {
                    result = Ecom_ProductoCategoria_.Update(3);
                }
                else
                {
                    throw new Ecom_Exception("Error en la configuración de permisos para esta sección, Contacta al departamento De TI");
                }

                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ProductoCategoria_);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                    ecomData.Disconect(ServerSource.Splitnet);
                }
            }
        }

        // GET: Categoria/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Categoria/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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