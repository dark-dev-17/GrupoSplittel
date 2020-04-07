using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAdmin.Models;
using EcommerceAdmin.Models.Filters;

namespace EcommerceAdmin.Controllers
{
    public class HomeController : Controller
    {
        [AccessViewSession]
        public IActionResult Index()
        {
            return View();
        }
        [AccessViewSession]
        public IActionResult General()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [AccessViewSession]
        public IActionResult Empleado()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [AccessViewSession]
        public IActionResult GetConfiguracion()
        {
            List<EcomDataProccess.Ecom_ProducProp> Caractersticas = new List<EcomDataProccess.Ecom_ProducProp>();
            Caractersticas.Add(new EcomDataProccess.Ecom_ProducProp { Label = "Diametro", Tipo = "select", Values = new List<EcomDataProccess.Ecom_propiedades>()
                {
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "900um",
                        Value = "CF"
                    },
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "250um",
                        Value = "SF"
                    },
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "2mm",
                        Value = "CF2"
                    }
                }
            });
            Caractersticas.Add(new EcomDataProccess.Ecom_ProducProp
            {
                Label = "Conector Entrada",
                Tipo = "select",
                Values = new List<EcomDataProccess.Ecom_propiedades>()
                {
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "SC/APC",
                        Value = "SCA"
                    },
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "Sin connector",
                        Value = "",
                        Default = false
                    }
                }
            });
            Caractersticas.Add(new EcomDataProccess.Ecom_ProducProp
            {
                Label = "Conector Salida",
                Tipo = "select",
                Values = new List<EcomDataProccess.Ecom_propiedades>()
                {
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "SC/APC",
                        Value = "SCA"
                    },
                    new EcomDataProccess.Ecom_propiedades{
                        Text = "Sin connector",
                        Value = "",
                        Default = false
                    }
                }
            });

            EcomDataProccess.Ecom_ConfProd Configuración = new EcomDataProccess.Ecom_ConfProd
            {
                Producto = "Mini cassette coupler",
                ProducProps = Caractersticas
            };
            return Ok(Configuración);
        }
        public IActionResult Configurar()
        {
            return View();
        }
    }
}
