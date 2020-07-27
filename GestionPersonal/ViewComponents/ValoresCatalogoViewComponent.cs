using GPSInformation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.ViewComponents
{
    public class ValoresCatalogoViewComponent : ViewComponent
    {
        private readonly List<CatalogoOpcionesValores> catalogoOpcionesValores;

        public ValoresCatalogoViewComponent(List<CatalogoOpcionesValores> catalogoOpcionesValores)
        {
            this.catalogoOpcionesValores = catalogoOpcionesValores;
        }

        public IViewComponentResult Invoke()
        {
            return View(catalogoOpcionesValores);
        }
    }
}
