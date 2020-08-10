using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.ViewComponents
{
    public class ValidPuestoEnOrganigramaViewComponent : ViewComponent
    {
        private DarkManager darkManager;

        public ValidPuestoEnOrganigramaViewComponent(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.OrganigramaVersion);
            darkManager.LoadObject(GpsManagerObjects.OrganigramaStructura);
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var listaPuestos = ListPuestos();

            EmpleadoInfor2 empleadoInfor = new EmpleadoInfor2();

            empleadoInfor.persona = darkManager.Persona.Get(id);
            if (empleadoInfor.persona != null)
            {
                empleadoInfor.Empleado = darkManager.Empleado.GetByColumn("" + id, "IdPersona");
                if (empleadoInfor.Empleado == null)
                {
                    empleadoInfor.Empleado = new GPSInformation.Models.Empleado();
                    empleadoInfor.Puesto = new PuestoOrg();
                }
                else
                {
                    empleadoInfor.Puesto = listaPuestos.Find(a => a.IdPuesto == empleadoInfor.Empleado.IdPuesto);
                }
            }
            empleadoInfor.IsActiveVersionOgg = false;
            empleadoInfor.IsPuestoOrg = false;

            var resultOrgActive = darkManager.OrganigramaVersion.GetByColumn("" + 2, "Autirizada");
            if (resultOrgActive != null)
            {
                empleadoInfor.IsActiveVersionOgg = true;
                //extraer puesto de organigrama del empleado a checar
                var ResultStructura = darkManager.OrganigramaStructura.Get("" + resultOrgActive.IdOrganigramaVersion, "IdOrganigramaVersion").Find(a => a.IdPuesto == empleadoInfor.Puesto.IdPuesto);

                

                if(ResultStructura != null)
                {
                    empleadoInfor.IsPuestoOrg = true;
                    //extrar jefe del empleado
                    empleadoInfor.PuestoBoos = listaPuestos.Find(a => a.IdPuesto == ResultStructura.IdPuestoParent);
                    var Jefes = darkManager.Empleado.Get("" + empleadoInfor.PuestoBoos.IdPuesto, "IdPuesto");
                    empleadoInfor.personaBoos = new List<GPSInformation.Models.Persona>();
                    Jefes.ForEach(a => {
                        empleadoInfor.personaBoos.Add(darkManager.Persona.Get(id));
                    });
                }
            }

            return await Task.FromResult((IViewComponentResult)View("ValidPuestoEnOrganigrama", empleadoInfor));
        }
        private List<PuestoOrg> ListPuestos()
        {
            List<PuestoOrg> puestoOrgs = new List<PuestoOrg>();
            var departamentos = darkManager.Departamento.Get();

            darkManager.Puesto.Get().ForEach(puesto => {
                puestoOrgs.Add(new PuestoOrg
                {
                    IdPuesto = puesto.IdPuesto,
                    DPU = string.Format("{0}-DPU-{1}", departamentos.Find(a => a.IdDepartamento == puesto.IdDepartamento).ClaveDPU, puesto.DPU),
                    Descripcion = puesto.Nombre
                });
            });

            return puestoOrgs.OrderBy(a => a.Descripcion).ToList();
        }
    }
}
