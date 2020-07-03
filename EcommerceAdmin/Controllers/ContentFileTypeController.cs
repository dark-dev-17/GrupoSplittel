using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAdmin.Models;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class ContentFileTypeController : Controller
    {
        private Ecommerce Ecommerce_;
        public ContentFileTypeController()
        {
            Ecommerce_ = new Ecommerce();
        }

        [AccessMultipleView(IdAction = new int[] { 54,55 })]
        // GET: ContentFileType
        public ActionResult Index()
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);
                return View(Ecom_ContentFileType_.Get());
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // GET: ContentFileType/Details/5
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Files(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Get(id))
                {
                    return View(Ecom_ContentFileType_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "registro no encontrado" });
                }
                
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // GET: ContentFileType/Create
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContentFileType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Create(Ecom_ContentFileType Ecom_ContentFileType_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ContentFileType_);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFileType_, ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Add())
                {
                    return RedirectToAction(nameof(Files), new { id = Ecom_ContentFileType_.LastId() });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFileType_);
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // GET: ContentFileType/Edit/5
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Edit(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Get(id))
                {
                    return View(Ecom_ContentFileType_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "registro no encontrado" });
                }
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // POST: ContentFileType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Edit(Ecom_ContentFileType Ecom_ContentFileType_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ContentFileType_);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFileType_, ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Edit())
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFileType_);
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // GET: ContentFileType/Delete/5
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Delete(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Get(id))
                {
                    return View(Ecom_ContentFileType_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "registro no encontrado" });
                }
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // POST: ContentFileType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 54 })]
        public ActionResult Delete(Ecom_ContentFileType Ecom_ContentFileType_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ContentFileType_);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFileType_, ObjectSource.ContentFileType);
                if (Ecom_ContentFileType_.Delete())
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFileType_);
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFileType_);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }
    }
}