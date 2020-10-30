using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class QuejasCtrl
    {
        #region Atributos
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public QuejasCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.QuejaPersona);
        }
        #endregion

        #region metodos
        public List<View_empleado> GetEmpleados()
        {
            return darkManager.View_empleado.Get().OrderBy( a => a.NombreCompleto).ToList();
        }
        public List<QuejaPersona> GetQuejaPersonas()
        {
            var quejas = darkManager.QuejaPersona.Get();
            quejas.ForEach(a => {
                a.Empleado = darkManager.View_empleado.Get(a.IdPersona);
            });
            
            return quejas.OrderByDescending(a => a.Creacion).ToList();
        }
        public void AddQueja(QuejaPersona QuejaPersona)
        {
            darkManager.StartTransaction();
            try
            {
                QuejaPersona.Creacion = DateTime.Now;
                QuejaPersona.SourceCliente = "--";
                darkManager.QuejaPersona.Element = QuejaPersona;
                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void Terminar()
        {
            darkManager = null;
        }
        #endregion
    }
}
