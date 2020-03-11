using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAdmin.Models;

namespace EcommerceAdmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult General()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public IActionResult Empleado()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
