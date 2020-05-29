using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcomDataProccess;
using System.Configuration;
using EcommerceAdmin.Models.Filters;

namespace EcommerceAdmin.wwwroot
{
    public class ProductoDistribuidorPrecarController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private EcomData ecomData;

        // GET: FichaTecnica
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 47)]
        public ActionResult DataGet()
        {
            try
            {
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.GetObject(ObjectSource.ProductoDistribuidorPrecar);
                return Ok(Ecom_ProductoDistribuidorPrecar_.Get());
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 47)]
        public ActionResult DataGetById(int id)
        {
            try
            {
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.GetObject(ObjectSource.ProductoDistribuidorPrecar);
                Ecom_ProductoDistribuidorPrecar_.Get(id);
                return Ok(Ecom_ProductoDistribuidorPrecar_);
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 47)]
        public ActionResult DataCreate(Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_)
        {
            try
            {
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.SetObjectConnection(Ecom_ProductoDistribuidorPrecar_,ObjectSource.ProductoDistribuidorPrecar);

                if (Ecom_ProductoDistribuidorPrecar_.Add())
                {
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha creado un nuevo Componente: " + Ecom_ProductoDistribuidorPrecar_.Componente, "ProductoDistribuidorPrecar", "Detalle", "", Ecom_ProductoDistribuidorPrecar_.GetlastId() + "");
                    return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                } 
                else
                {
                    return BadRequest(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 47)]
        public ActionResult DataUpdate(Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_)
        {
            try
            {
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.SetObjectConnection(Ecom_ProductoDistribuidorPrecar_, ObjectSource.ProductoDistribuidorPrecar);

                if (Ecom_ProductoDistribuidorPrecar_.Update(2))
                {
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha editado el Componente: " + Ecom_ProductoDistribuidorPrecar_.Componente, "ProductoDistribuidorPrecar", "Detalle", "", Ecom_ProductoDistribuidorPrecar_.Id + "");
                    return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [AccessData(IdAction = 47)]
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == 0)
                {
                    throw new Ecom_Exception("Por favor selecciona un complemento");
                }
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.GetObject(ObjectSource.ProductoDistribuidorPrecar);
                if (Ecom_ProductoDistribuidorPrecar_.Get(id))
                {
                    return View(Ecom_ProductoDistribuidorPrecar_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 47)]
        public ActionResult EditForm(Ecom_ProductoDistribuidorPrecar Ecom_ProductoDistribuidorPrecar_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", Ecom_ProductoDistribuidorPrecar_);
                }
                else
                {
                    ecomData = new EcomData(EcomConnection, SplitConnection);
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoDistribuidorPrecar_ = (Ecom_ProductoDistribuidorPrecar)ecomData.SetObjectConnection(Ecom_ProductoDistribuidorPrecar_, ObjectSource.ProductoDistribuidorPrecar);
                    if (Ecom_ProductoDistribuidorPrecar_.Update(2))
                    {
                        //ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha editado el topico: " + Ecom_ProductoDistribuidorPrecar_.Tipo, "ProductoPatchCord", "Detalle", "", Ecom_ProductoDistribuidorPrecar_.Id + "");
                        //return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                        return View("../ErrorPages/Success", new { id = ecomData.GetLastMessage(ServerSource.Ecommerce) });
                    }
                    else
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                //ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View("Edit", Ecom_ProductoDistribuidorPrecar_);
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
    }
}