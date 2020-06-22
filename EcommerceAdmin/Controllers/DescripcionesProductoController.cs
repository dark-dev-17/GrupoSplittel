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
    public class DescripcionesProductoController : Controller
    {
        #region Propiedades
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private EcomData ecomData;
        private Ecom_ProductoDescripcion Ecom_ProductoDescripcion_;
        #endregion

        #region Constructores
        public DescripcionesProductoController()
        {
            ecomData = new EcomData(EcomConnection, SplitConnection);
            
            ecomData.Connect(ServerSource.Splitnet);
        }
        #endregion

        #region Metodos
        // GET: DescripcionesProducto
        public ActionResult Index()
        {
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.GetObject(ObjectSource.ProductoDescripcion);
                List<Ecom_ProductoDescripcion> result = Ecom_ProductoDescripcion_.Get();
                return View(result);
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
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

        // GET: DescripcionesProducto/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.GetObject(ObjectSource.ProductoDescripcion);
                bool result = Ecom_ProductoDescripcion_.Get(id);
                if (result)
                {
                    return View(Ecom_ProductoDescripcion_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
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

        // GET: DescripcionesProducto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DescripcionesProducto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ecom_ProductoDescripcion Ecom_ProductoDescripcion_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ProductoDescripcion_);
                }
                else
                {
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.SetObjectConnection(Ecom_ProductoDescripcion_, ObjectSource.ProductoDescripcion);
                    bool result = Ecom_ProductoDescripcion_.Add();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ProductoDescripcion_);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: DescripcionesProducto/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.GetObject(ObjectSource.ProductoDescripcion);
                bool result = Ecom_ProductoDescripcion_.Get(id);
                if (result)
                {
                    return View(Ecom_ProductoDescripcion_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
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

        // POST: DescripcionesProducto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ecom_ProductoDescripcion Ecom_ProductoDescripcion_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ProductoDescripcion_);
                }
                else
                {
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.SetObjectConnection(Ecom_ProductoDescripcion_, ObjectSource.ProductoDescripcion);
                    bool result = Ecom_ProductoDescripcion_.Update(2);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ProductoDescripcion_);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: DescripcionesProducto/Edit/5
        public ActionResult Editt(string id)
        {
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.GetObject(ObjectSource.ProductoDescripcion);
                bool result = Ecom_ProductoDescripcion_.Get(id);
                if (result)
                {
                    return View(Ecom_ProductoDescripcion_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
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

        // POST: DescripcionesProducto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt(Ecom_ProductoDescripcion Ecom_ProductoDescripcion_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Editt", Ecom_ProductoDescripcion_);
                }
                else
                {
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.SetObjectConnection(Ecom_ProductoDescripcion_, ObjectSource.ProductoDescripcion);
                    bool result = Ecom_ProductoDescripcion_.Update(2);
                    if (result)
                    {
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
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ProductoDescripcion_);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: DescripcionesProducto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DescripcionesProducto/Delete/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataGet()
        {
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoDescripcion_ = (Ecom_ProductoDescripcion)ecomData.GetObject(ObjectSource.ProductoDescripcion);
                List<Ecom_ProductoDescripcion> result = Ecom_ProductoDescripcion_.Get();
                return Ok(result);
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
        #endregion
    }
}