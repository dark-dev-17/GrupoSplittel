using GestionPersonal.Models;
using GPSInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private DarkManager darkManager;
        public MenuViewComponent(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Modulo);
            darkManager.LoadObject(GpsManagerObjects.SubModulo);
            darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
        }

        public async Task<IViewComponentResult> InvokeAsync(string controller, string action)
        {
            EmpleadoInfor empleadoInfor = new EmpleadoInfor();
            if (string.IsNullOrEmpty(action))
            {
                action = "Index";
            }
            var menu = darkManager.Modulo.Get().OrderBy(a => a.Posicion).ToList();
            var accesos = darkManager.AccesosSistema.Get("" + (int)HttpContext.Session.GetInt32("user_id_permiss"), nameof(darkManager.AccesosSistema.Element.IdUsuario));
            menu.ForEach(a => {
                a.SubModulos = new List<GPSInformation.Models.SubModulo>();
                var Submodulos = darkManager.SubModulo.Get("" + a.IdModulo, nameof(darkManager.SubModulo.Element.IdModulo)).Where(b => b.Tipo == 1).ToList().OrderBy(b => b.Posicion).ToList();

                Submodulos.ForEach(b => {
                    if(b.Controllador == controller && b.Accion == action)
                    {
                        b.Activemenu = true;
                    }
                    var acces = accesos.Find(n => n.IdSubModulo == b.IdSubModulo);
                    if(acces == null)
                    {
                        b.AccesosSistema = new GPSInformation.Models.AccesosSistema() { IdUsuario = (int)HttpContext.Session.GetInt32("user_id_permiss"), IdSubModulo = b.IdSubModulo };
                    }
                    else
                    {
                        b.AccesosSistema = acces;
                    }
                    a.SubModulos.Add(b);
                });
            });

            return await Task.FromResult((IViewComponentResult)View("Menu", menu));
        }

        
    }
}
