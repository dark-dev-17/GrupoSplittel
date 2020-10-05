using GPSInformation.Models;
using GPSInformation.Reportes;
using GPSInformation.Views;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class PrenominaCtrl
    {
        #region Atributos
        private int IdUsuario;
        private DarkManager darkManager;
        private string Path = @"C:\Splittel\GestionPersonal";
        #endregion

        #region Constructores
        public PrenominaCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
        }
        public PrenominaCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.Departamento);
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
        }
        #endregion

        #region Metodos
        public Prenomina_Rep GetExpediente()
        {
            Prenomina_Rep prenomina_Rep = new Prenomina_Rep();
            prenomina_Rep.Inicio = DateTime.Now;
            prenomina_Rep.Fin = DateTime.Now;

            prenomina_Rep.Departamentos = new List<Reportes.Departamento>();
            darkManager.Departamento.Get().ForEach(dep => {
                prenomina_Rep.Departamentos.Add(new Reportes.Departamento { 
                    IdDepartamento = dep.IdDepartamento,
                    Nombre = dep.Nombre,
                    Selected = false
                });
            });

            prenomina_Rep.TipoNominas = new List<TipoNomina>();
            darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").ForEach(tipo => {
                prenomina_Rep.TipoNominas.Add(new TipoNomina {
                    IdTipoNomina = tipo.IdCatalogoOpcionesValores,
                    Nombre = tipo.Descripcion,
                    Selected = false
                });
            });

            return prenomina_Rep;
        }
        public List<View_empleado> GetExpediente(Prenomina_Rep prenomina_Rep)
        {
            if (prenomina_Rep is null)
                throw new GPSInformation.Exceptions.GpExceptions("Objeto prenomina_Rep vacio");
            
            List<int> keys2 = new List<int>();
            prenomina_Rep.TipoNominas.ForEach( a => keys2.Add(a.IdTipoNomina));

            List<int> keys3 = new List<int>();
            prenomina_Rep.Departamentos.ForEach(a => keys3.Add(a.IdDepartamento));

            var Emp = darkManager.View_empleado.GetIn(keys2, "IdTipoNomina", keys3, "IdDepartamento");


            return Emp;
        }
        #endregion
    }
}
