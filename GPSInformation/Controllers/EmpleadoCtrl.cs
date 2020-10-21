using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class EmpleadoCtrl
    {
        #region Atributos
        private DarkManager darkManager;
        public DarkManager GetDarkManager { get { return darkManager; } }
        #endregion

        #region Constructores
        public EmpleadoCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.EmpleadoContrato);
        }
        #endregion

        #region Metodos
        public List<EmpleadoContrato> GetContratos(int IdPersona)
        {
            var Emp_re = darkManager.EmpleadoContrato.Get(""+ IdPersona, "IdPersona");
            return Emp_re;
        }
        public EmpleadoContrato GetContrato(int IdEmpleadoContrato)
        {
            var Emp_re = darkManager.EmpleadoContrato.Get(IdEmpleadoContrato);
            return Emp_re;
        }
        public void Add(EmpleadoContrato EmpleadoContrato)
        {
            darkManager.StartTransaction();
            try
            {
                EmpleadoContrato.Created = DateTime.Now;
                darkManager.EmpleadoContrato.Element = EmpleadoContrato;
                if (!darkManager.EmpleadoContrato.Add())
                {
                    throw new GPSInformation.Exceptions.GpExceptions("No se pudo guardar el contrado");
                }
                darkManager.Commit();
            }
            catch (GPSInformation.Exceptions.GpExceptions )
            {
                darkManager.RolBack();
                throw;
            }
            
        }

        #endregion
    }
}
