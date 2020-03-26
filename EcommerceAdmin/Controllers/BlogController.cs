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
    public class BlogController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        // GET: Blog
        [AccessView(IdAction = 36)]
        public ActionResult Index()
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                List<Ecom_Blog> result = Ecom_Blog_.Get();
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

        // GET: Blog/Details/5
        [AccessView(IdAction = 36)]
        public ActionResult Details(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                bool result = Ecom_Blog_.Get(id);
                if (result)
                {
                    return View(Ecom_Blog_);
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

        // GET: Blog/Create
        [AccessView(IdAction = 37)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 37)]
        public ActionResult Create(Ecom_Blog Ecom_Blog_)
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

        // GET: Blog/Edit/5
        [AccessView(IdAction = 37)]
        public ActionResult Edit(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                bool result = Ecom_Blog_.Get(id);
                if (result)
                {
                    return View(Ecom_Blog_);
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

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 37)]
        public ActionResult Edit(Ecom_Blog Ecom_Blog_)
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

        // GET: Blog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Blog/Delete/5
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