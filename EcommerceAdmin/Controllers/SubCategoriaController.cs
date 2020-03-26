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
    public class SubCategoriaController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        // GET: SubCategoria
        [AccessView(IdAction = 33)]
        public ActionResult Index()
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoSubCategoria Ecom_ProductoSubCategoria_ = (Ecom_ProductoSubCategoria)ecomData.GetObject(ObjectSource.ProductoSubcategoria);
                List<Ecom_ProductoSubCategoria> result = Ecom_ProductoSubCategoria_.Get();
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

        // GET: SubCategoria/Details/5
        [AccessView(IdAction = 33)]
        public ActionResult Details(string id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoSubCategoria Ecom_ProductoSubCategoria_ = (Ecom_ProductoSubCategoria)ecomData.GetObject(ObjectSource.ProductoSubcategoria);
                bool result = Ecom_ProductoSubCategoria_.GetById(id);
                if (result)
                {
                    return View(Ecom_ProductoSubCategoria_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = ecomData.GetLastMessage(ServerSource.Ecommerce) });
                }
                
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

        // GET: SubCategoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubCategoria/Create
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

        // GET: SubCategoria/Edit/5
        [AccessMultipleView(IdAction = new int[] { 34, 35 })]
        public ActionResult Edit(string id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoSubCategoria Ecom_ProductoSubCategoria_ = (Ecom_ProductoSubCategoria)ecomData.GetObject(ObjectSource.ProductoSubcategoria);
                bool result = Ecom_ProductoSubCategoria_.GetById(id);
                if (result)
                {
                    return View(Ecom_ProductoSubCategoria_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = ecomData.GetLastMessage(ServerSource.Ecommerce) });
                }

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

        // POST: SubCategoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ecom_ProductoSubCategoria Ecom_ProductoSubCategoria_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                ecomData.Connect(ServerSource.Splitnet);
                bool AdminPermiss = ecomData.validPermissAction(USR_IdSplinnet, 34);
                bool basicPermiss = ecomData.validPermissAction(USR_IdSplinnet, 35);
                Ecom_ProductoSubCategoria_ = (Ecom_ProductoSubCategoria)ecomData.SetObjectConnection(Ecom_ProductoSubCategoria_, ObjectSource.ProductoSubcategoria);
                bool result = false;
                if (AdminPermiss && !basicPermiss)
                {
                    result = Ecom_ProductoSubCategoria_.Update(2);
                }
                else if (!AdminPermiss && basicPermiss)
                {
                    result = Ecom_ProductoSubCategoria_.Update(3);
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
                return View(Ecom_ProductoSubCategoria_);
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

        // GET: SubCategoria/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubCategoria/Delete/5
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