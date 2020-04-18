using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class DireccionController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        // GET: Addresses
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetailsFacturacionB2B(string id, string CardCode)
        {
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
                SAPDataProcess.SAP_Address SAP_Document_ = new SAPDataProcess.SAP_Address(SAP_DBConnection_);
                SAP_Document_.GetByAddressName(CardCode, "B", id);
                SAP_DBConnection_.CloseDataBaseAccess();
                return PartialView(SAP_Document_);
            }
            catch (Ecom_Exception ex)
            {
                return PartialView("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }
        public ActionResult DetailsFacturacionB2C(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_DireccionFacturacion Ecom_DireccionFacturacion_ = (Ecom_DireccionFacturacion)ecomData.GetObject(ObjectSource.DireccionFacturacion);
                bool result = Ecom_DireccionFacturacion_.Get(id);
                if (result)
                {
                    return PartialView(Ecom_DireccionFacturacion_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                return PartialView("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        // GET: Addresses/Details/5
        public ActionResult DetailsEnvioB2C(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_DireccionEnvio Ecom_DireccionEnvio_ = (Ecom_DireccionEnvio)ecomData.GetObject(ObjectSource.DireccionEnvio);
                bool result = Ecom_DireccionEnvio_.Get(id);
                if (result)
                {
                    return PartialView(Ecom_DireccionEnvio_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                return PartialView("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        public ActionResult DetailsEnvioB2B(string id,string CardCode)
        {
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
                SAPDataProcess.SAP_Address SAP_Document_ = new SAPDataProcess.SAP_Address(SAP_DBConnection_);
                SAP_Document_.GetByAddressName(CardCode,"S", id);
                SAP_DBConnection_.CloseDataBaseAccess();
                return PartialView(SAP_Document_);
            }
            catch (Ecom_Exception ex)
            {
                return PartialView("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
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

        // GET: Addresses/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Addresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Addresses/Delete/5
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