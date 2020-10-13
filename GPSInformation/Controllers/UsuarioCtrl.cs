using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace GPSInformation.Controllers
{
    public class UsuarioCtrl
    {
        #region Atributos
        private int IdUsuario;
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public UsuarioCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteArchivo);
            this.darkManager.LoadObject(GpsManagerObjects.ExpedienteEmpleado);
            this.darkManager.LoadObject(GpsManagerObjects.View_EmpleadoExpediente);
        }
        public UsuarioCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Usuario);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.InformacionMedica);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Sociedad);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.PersonaContacto);
            darkManager.LoadObject(GpsManagerObjects.View_empleado);
            darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
            darkManager.LoadObject(GpsManagerObjects.SubModulo);
        }
        #endregion

        #region Metodos

        public void AddUsuario(Usuario Usuario)
        {
            darkManager.StartTransaction();
            try
            {
                var Usuario_re = darkManager.Usuario.GetByColumn("" + Usuario.IdPersona, nameof(darkManager.Usuario.Element.IdPersona));
                if(Usuario_re is null)
                {
                    Usuario.UltimoIngreso = DateTime.Now;
                    Usuario_re.ImagenDefault = true;
                    Usuario_re.ImagenPerfil = "";
                    darkManager.Usuario.Element = Usuario;
                    if (!darkManager.Usuario.Add())
                    {
                        throw new Exceptions.GpExceptions(string.Format("Error al guardar el acceso"));
                    }
                    Usuario_re = darkManager.Usuario.GetByColumn("" + Usuario.IdPersona, nameof(darkManager.Usuario.Element.IdPersona));
                    AddPermisos(Usuario_re.IdUsuario, Usuario.IdRol);
                }
                else
                {
                    Usuario_re.UltimoIngreso = DateTime.Now;
                    Usuario_re.Pass = Usuario.Pass;
                    Usuario_re.Activo = Usuario.Activo;
                    Usuario_re.IdRol = Usuario.IdRol;
                    
                    darkManager.Usuario.Element = Usuario_re;
                    if (!darkManager.Usuario.Update())
                    {
                        throw new Exceptions.GpExceptions(string.Format("Error al guardar el acceso"));
                    }

                    AddPermisos(Usuario_re.IdUsuario, Usuario.IdRol);
                }

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        private void AddPermisos(int IdUsuario, int IdPermiso)
        {
            if (IdUsuario == 0)
            {
                throw new GPSInformation.Exceptions.GpExceptions("Por favor selecciona un empleado");
            }
            if (IdPermiso == 0)
            {
                throw new GPSInformation.Exceptions.GpExceptions("Por favor selecciona un rol de acceso");
            }

            List<int> Permisos;

            if (IdPermiso == 80)
            {
                Permisos = new List<int> { 2, 4, 5, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 28, 30, 32, 36, 33, 35, 37, 39, 40,42,43 };
            }
            else if (IdPermiso == 81)
            {
                //Con personal a cargo
                Permisos = new List<int> { 2, 18, 26, 30, 32, 33, 39,43 };
            }
            else if (IdPermiso == 82)
            {
                //rol sin personal a cargo
                Permisos = new List<int> { 2, 18, 30, 33, 39,43 };
            }
            else
            {
                Permisos = new List<int> { };
            }
            var accesos = darkManager.AccesosSistema.Get("" + IdUsuario, nameof(darkManager.AccesosSistema.Element.IdUsuario));

            darkManager.SubModulo.Get().ForEach(Act => {
                var acces = accesos.Find(n => n.IdSubModulo == Act.IdSubModulo);
                if(acces is null)
                {
                    //agregar
                    AccesosSistema AccesosSistema = new AccesosSistema
                    {
                        IdSubModulo = Act.IdSubModulo,
                        IdUsuario = IdUsuario,
                        Modificado = DateTime.Now,
                        TieneAcceso = Permisos.Contains(Act.IdSubModulo)
                    };

                    darkManager.AccesosSistema.Element = AccesosSistema;
                    if (!darkManager.AccesosSistema.Add())
                    {
                        throw new GPSInformation.Exceptions.GpExceptions("Error al agregar permiso");
                    }
                }
                else
                {
                    //actualizar
                    acces.Modificado = DateTime.Now;
                    acces.TieneAcceso = Permisos.Contains(Act.IdSubModulo);

                    darkManager.AccesosSistema.Element = acces;
                    if (!darkManager.AccesosSistema.Update())
                    {
                        throw new GPSInformation.Exceptions.GpExceptions("Error al actualizar permiso");
                    }
                }

                acces = null;
            });
            accesos.Clear();
        }
        public SelectList GetDictionary(int id, int IdSelected)
        {
            return new SelectList(darkManager.CatalogoOpcionesValores.Get("" + id, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IdSelected);
        }
        #endregion
    }
}
