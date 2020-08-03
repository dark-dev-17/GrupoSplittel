using GestionPersonal.Models;
using GPSInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.ViewComponents
{
    public class EmpleadoBasicViewComponent : ViewComponent
    {
        private DarkManager darkManager;
        public EmpleadoBasicViewComponent(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            EmpleadoInfor empleadoInfor = new EmpleadoInfor();

            empleadoInfor.persona = darkManager.Persona.Get(id);
            if(empleadoInfor.persona != null)
            {
                empleadoInfor.Empleado = darkManager.Empleado.GetByColumn("" + id, "IdPersona");
                if(empleadoInfor.Empleado == null)
                {
                    empleadoInfor.Empleado = new GPSInformation.Models.Empleado();
                    empleadoInfor.Puesto = new GPSInformation.Models.Puesto();
                }
                else
                {
                    empleadoInfor.Puesto = darkManager.Puesto.Get(empleadoInfor.Empleado.IdPuesto);
                }
            }

            return await Task.FromResult((IViewComponentResult)View("EmpleadoBasic", empleadoInfor));
        }
    }
}
