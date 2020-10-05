using GPSInformation.Models;
using GPSInformation.Views;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class ExpedienteCtrl
    {
        #region Atributos
        private int IdUsuario;
        private DarkManager darkManager;
        private string Path = @"C:\Splittel\GestionPersonal";
        #endregion

        #region Constructores
        public ExpedienteCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteArchivo);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteEmpleado);
            this.darkManager.LoadObject(GpsManagerObjects.View_EmpleadoExpediente);
        }
        public ExpedienteCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteArchivo);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteEmpleado);
            this.darkManager.LoadObject(GpsManagerObjects.View_EmpleadoExpediente);
        }
        #endregion

        #region Metodos
        public IEnumerable<View_EmpleadoExpediente> GetExpediente(int IdPersona)
        {
            string PathEmp = string.Format(@"{0}\{1}\Expediente\", Path, IdPersona);

            if (!Directory.Exists(PathEmp))
                Directory.CreateDirectory(PathEmp);

            return darkManager.View_EmpleadoExpediente.Get("" + IdPersona, "IdPersona");
        }
        public IEnumerable<ExpedienteArchivo> GetTipoDocumentos()
        {
            return darkManager.ExpedienteArchivo.Get().OrderBy(a => a.Nombre);
        }
        public byte[] GetFile(int IdPersona, int IdExpedienteArchivo)
        {
            var Archivo_re = darkManager.ExpedienteEmpleado.Get(
                     "IdPersona", "" + IdPersona,
                    "IdExpedienteArchivo", "" + IdExpedienteArchivo);
            if (Archivo_re is null)
            {
                throw new Exceptions.GpExceptions(string.Format("No se encontró el registro"));
            }
            string PathEmp = string.Format(@"{0}\{1}\Expediente\", Path, IdPersona);
            return  System.IO.File.ReadAllBytes(string.Format(@"{0}{1}", PathEmp, Archivo_re.Ruta));
        }
        public ExpedienteEmpleado GetFileDetails(int IdPersona, int IdExpedienteArchivo)
        {
            var Archivo_re = darkManager.ExpedienteEmpleado.Get(
                     "IdPersona", "" + IdPersona,
                    "IdExpedienteArchivo", "" + IdExpedienteArchivo);
            return Archivo_re;
        }
        public async System.Threading.Tasks.Task AddArchivoAsync(int IdPersona, int IdExpedienteArchivo, IFormFile Archivo)
        {
            darkManager.StartTransaction();
            try
            {
                var Emp_re = darkManager.View_empleado.Get(IdPersona);
                if (Emp_re is null)
                {
                    throw new Exceptions.GpExceptions(string.Format("No se encontró el empleado"));
                }

                var Archivo_re = darkManager.ExpedienteEmpleado.Get(
                     "IdPersona", "" + IdPersona,
                    "IdExpedienteArchivo", "" + IdExpedienteArchivo);

                string PathEmp = string.Format(@"{0}\{1}\Expediente\", Path, IdPersona);

                if (!Directory.Exists(PathEmp))
                    Directory.CreateDirectory(PathEmp);

                if (Archivo_re is null)
                {
                    //agregar Archivo
                    if (Archivo.Length <= 0)
                        throw new Exceptions.GpExceptions(string.Format("El archivo '{0}' esta dañado", Archivo.FileName));

                    if (File.Exists(string.Format(@"{0}{1}", PathEmp, Archivo.FileName)))
                    {
                        File.Delete(string.Format(@"{0}{1}", PathEmp, Archivo.FileName));
                    }

                    using (var stream = System.IO.File.Create(string.Format(@"{0}{1}", PathEmp, Archivo.FileName)))
                    {
                        await Archivo.CopyToAsync(stream);
                    }
                    ExpedienteEmpleado expedienteEmpleado = new ExpedienteEmpleado();
                    expedienteEmpleado.IdPersona = IdPersona;
                    expedienteEmpleado.IdExpedienteArchivo = IdExpedienteArchivo;
                    expedienteEmpleado.Ruta = Archivo.FileName;
                    expedienteEmpleado.TipoFile = Archivo.ContentType;
                    expedienteEmpleado.Creado = DateTime.Now;
                    expedienteEmpleado.Actualizado = DateTime.Now;
                    darkManager.ExpedienteEmpleado.Element = expedienteEmpleado;

                    if (!darkManager.ExpedienteEmpleado.Add())
                    {
                        throw new Exceptions.GpExceptions(string.Format("Error al guardar archivo", Archivo.FileName));
                    }
                }
                else
                {
                    //eliminar y actualizar Archivo
                    if (Archivo.Length <= 0)
                        throw new Exceptions.GpExceptions(string.Format("El archivo '{0}' esta dañado", Archivo.FileName));


                    if (File.Exists(string.Format(@"{0}{1}", PathEmp, Archivo.FileName)))
                    {
                        File.Delete(string.Format(@"{0}{1}", PathEmp, Archivo.FileName));
                    }

                    Archivo_re.Actualizado = DateTime.Now;
                    Archivo_re.Ruta = Archivo.FileName;
                    Archivo_re.TipoFile = Archivo.ContentType;
                    darkManager.ExpedienteEmpleado.Element = Archivo_re;

                    if (!darkManager.ExpedienteEmpleado.Update())
                    {
                        throw new Exceptions.GpExceptions(string.Format("Error al guardar archivo", Archivo.FileName));
                    }
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
}
