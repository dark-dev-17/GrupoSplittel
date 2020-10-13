using GPSInformation.Models;
using GPSInformation.Reportes;
using GPSInformation.Tools;
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
        public List<Registro> Nomenclatura = new List<Registro>();
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
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
            this.darkManager.LoadObject(GpsManagerObjects.DiaFeriado);
            this.darkManager.LoadObject(GpsManagerObjects.FaltaJustificacion);


            Nomenclatura = Funciones.GetRegistrosInc();
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

            prenomina_Rep.Departamentos = prenomina_Rep.Departamentos.OrderBy(a => a.Nombre).ToList();

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
            prenomina_Rep.TipoNominas.Where(a => a.Selected).ToList().ForEach( a => keys2.Add(a.IdTipoNomina));

            List<int> keys3 = new List<int>();
            prenomina_Rep.Departamentos.Where(a => a.Selected).ToList().ForEach(a => keys3.Add(a.IdDepartamento));


            if(keys2.Count == 0 && keys3.Count == 0 || keys2.Count == 0 && keys3.Count > 0 || keys2.Count > 0 && keys3.Count == 0)
            {
                return new List<View_empleado>(); 
            }
            else
            {
                return darkManager.View_empleado.GetIn(keys2, "IdTipoNomina", keys3, "IdDepartamento").OrderBy(a => a.NombreCompleto).ToList();
            }
        }
        public List<PrenominaDias> GetPreniminaLists(Prenomina_Rep prenomina_Rep, List<View_empleado> view_Empleados)
        {
            if (prenomina_Rep is null)
                throw new GPSInformation.Exceptions.GpExceptions("Objeto prenomina_Rep vacio");

            if (view_Empleados is null)
                throw new GPSInformation.Exceptions.GpExceptions("Objeto view_Empleados vacio");

            var Lista_re = new List<PrenominaDias>();

            darkManager.OpenConnectionAcces();

            view_Empleados.ForEach(emp => {
                //extraer dias
                var diaEmp = new PrenominaDias
                {
                    IdPersona = emp.IdPersona,
                    NumeroNomina = emp.NumeroNomina,
                    Dias = new List<PreniminaList>()
                };
                DateTime InitialDate = prenomina_Rep.Inicio;
                while (InitialDate <= prenomina_Rep.Fin)
                {
                    PreniminaList preniminaList = new PreniminaList
                    {
                        Fecha = InitialDate,
                        Incidencias = new List<Registro>()
                    };
                    TimeSpan diferencia = GetDiference(InitialDate,emp.NumeroNomina);
                    if (diferencia.TotalHours > 0 && diferencia.TotalHours < 8)
                    {
                        preniminaList.Incidencias.Add(GetPermiso(emp.IdPersona, InitialDate));
                        diaEmp.Dias.Add(preniminaList);
                    }
                    else if(diferencia.TotalHours <= 0)
                    {
                        if (TieneVacaciones(emp.IdPersona, InitialDate))
                        {
                            preniminaList.Incidencias.Add(Nomenclatura.Find(a=> a.Clave.Trim() == "VAC"));
                        }
                        else
                        {
                            var Feriado = darkManager.DiaFeriado.GetByColumn("" + InitialDate.ToString("yyyy/MM/dd"), "Fecha");
                            if (Feriado is null)
                            {
                                var Justi = darkManager.FaltaJustificacion.Get("IdPersona", "" + emp.IdPersona, "Fecha", InitialDate.ToString("yyyy/MM/dd"));
                                if (Justi is null)
                                {
                                    preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave.Trim() == "FAL"));
                                }
                                else
                                {
                                    var Res = Nomenclatura.Find(a => a.Clave.Trim() == "INJ");
                                    Res.Title = string.Format("Incidencia justificada, comentarios: {0}", Justi.Comentarios);
                                    preniminaList.Incidencias.Add(Res);
                                }
                            }
                            else
                            {
                                preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave.Trim() == "FES"));
                            }
                            
                        }
                        
                        diaEmp.Dias.Add(preniminaList);
                    }
                    
                    InitialDate = InitialDate.AddDays(1);
                }
                //agregar data al dataset
                Lista_re.Add(diaEmp);
            });

            darkManager.CloseConnectionAccess();
            return Lista_re;
        }
        private TimeSpan GetDiference(DateTime Inicio, string NumeroNomina)
        {
            string statement = string.Format("" +
                "SELECT " +
                    "( " +
                        "SELECT MAX(EVN.dtEventReal) FROM tblEvents AS EVN WHERE EMP.iEmployeeNum = EVN.IdEmpNum AND EVN.dtEventReal >= '{0} 00:00:00' AND EVN.dtEventReal <= '{0} 23:59:59'" +
                    ") AS MAX_DATE, " +
                    " (" +
                        "SELECT MIN(EVN.dtEventReal) FROM tblEvents AS EVN WHERE EMP.iEmployeeNum = EVN.IdEmpNum AND EVN.dtEventReal >= '{0} 00:00:00' AND EVN.dtEventReal <= '{0} 23:59:59'" +
                    ") AS MIN_DATE " +
                "FROM[dbo].[tblEmployees] AS EMP " +
                "WHERE EMP.tIdentification = '{1}'", Inicio.ToString("yyyy/MM/dd"), NumeroNomina);

            System.Data.SqlClient.SqlDataReader Data = darkManager.ExecuteStatementAccess(statement);

            DateTime MAX_DATE = DateTime.Now;
            DateTime MIN_DATE = DateTime.Now;
            if (Data.HasRows)
            {
                while (Data.Read())
                {
                    MAX_DATE = Data.GetValue(Data.GetOrdinal("MAX_DATE")) is System.DBNull ? DateTime.Now : Data.GetDateTime(Data.GetOrdinal("MAX_DATE"));
                    MIN_DATE = Data.GetValue(Data.GetOrdinal("MIN_DATE")) is System.DBNull ? DateTime.Now : Data.GetDateTime(Data.GetOrdinal("MIN_DATE"));
                }
                TimeSpan antiguedad = MAX_DATE - MIN_DATE;
                Data.Close();
                return antiguedad;
            }
            else
            {
                TimeSpan antiguedad = MAX_DATE - MIN_DATE;
                Data.Close();
                return antiguedad;
            }
            
        }
        private Registro GetPermiso(int IdPersona, DateTime Fecha)
        {
            var Permiso_re = darkManager.IncidenciaPermiso.Get("" + IdPersona, "IdPersona").Find(per => per.Fecha == Fecha);
            if(Permiso_re is null)
            {
                var Justi = darkManager.FaltaJustificacion.Get("IdPersona", "" + IdPersona, "Fecha", Fecha.ToString("yyyy/MM/dd"));
                if(Justi is null)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "SNJ");
                }
                else
                {
                    var Res = Nomenclatura.Find(a => a.Clave.Trim() == "INJ");
                    Res.Title = string.Format("Incidencia justificada, comentarios: {0}", Justi.Comentarios);
                    return Res;
                }
            }
            if(Permiso_re.IdAsunto != 36)
            {
                // Laboral: capacitación
                if (Permiso_re.IdAsunto == 37)
                {
                    return  Nomenclatura.Find(a => a.Clave.Trim() == "SAL");
                }
                // Laboral: reunión de trabajo
                else if (Permiso_re.IdAsunto == 38)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "SAL");
                }
                // laboral: visita al cliente
                else if (Permiso_re.IdAsunto == 39)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "VIS");
                }
                else
                {
                    throw new GPSInformation.Exceptions.GpExceptions("El asunto del permiso no es valido");
                }
            }
            else
            {
                // Permiso con goce de sueldo
                if (Permiso_re.IdPagoPermiso == 40)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "PCG");
                }
                // Permiso sin goce de sueldo
                else if (Permiso_re.IdPagoPermiso == 41)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "PSG");
                }
                // permiso tiempo por tiempo
                else if (Permiso_re.IdPagoPermiso == 42)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "TXT");
                }
                // salario emocional: cumpleaños
                else if (Permiso_re.IdPagoPermiso == 43)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "DXC");
                }
                // salario emocional: medio dia libre
                else if (Permiso_re.IdPagoPermiso == 44)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "S.E.S");
                }
                else
                {
                    throw new GPSInformation.Exceptions.GpExceptions("El pago del permiso no es valido");
                }
            }
        }
        private bool TieneVacaciones(int IdPersona, DateTime Fecha)
        {
            bool Tiene = false;
            var vacacion_re = darkManager.IncidenciaVacacion.Get("" + IdPersona, "IdPersona").Find( vac => vac.Inicio <= Fecha && Fecha <= vac.Fin );
            if(vacacion_re != null)
            {
                if(vacacion_re.Estatus == 3)
                {
                    Tiene = true;
                }
            }
            else
            {
                Tiene = false;
            }
            return Tiene;
        }
        public void JustificarIncidencia(FaltaJustificacion faltaJustificacion)
        {
            
            darkManager.StartTransaction();
            try
            {
                if (faltaJustificacion is null)
                {
                    throw new GPSInformation.Exceptions.GpExceptions("vfaltaJustificacion is null");
                }
                var Justi = darkManager.FaltaJustificacion.Get("IdPersona", "" + faltaJustificacion.IdPersona, "Fecha", faltaJustificacion.Fecha.ToString("yyyy/MM/dd"));
                if (Justi != null)
                {
                    throw new GPSInformation.Exceptions.GpExceptions("Ya fue justificada la incidencia");
                }
                faltaJustificacion.Creado = DateTime.Now;
                darkManager.FaltaJustificacion.Element = faltaJustificacion;

                if (!darkManager.FaltaJustificacion.Add())
                {
                    throw new GPSInformation.Exceptions.GpExceptions("error al registrar la justificación");
                }
                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }

        }
        #endregion
    }
    public class PrenominaDias
    {
        public int IdPersona { get; set; }
        public string NumeroNomina { get; set; }
        public List<PreniminaList> Dias { get; set; }
    }
    public class PreniminaList
    {
        public DateTime Fecha { get; set; }
        public List<Registro> Incidencias { get; set; }
    }
    public class Registro
    {
        public int Tipo { get; set; }
        public string Clave { get; set; }
        public string Title { get; set; }
        public string Color { get; internal set; }
        public string TextColor { get; internal set; }
    }
}
