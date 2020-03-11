using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class ErrorPagesController : Controller
    {
        // GET: ErrorPages
        public ActionResult NoAccess()
        {
            return View();
        }
        public ActionResult Error(string id)
        {
            ViewData["Message"] = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? "Error" : id;
            return View();
        }
    }
}