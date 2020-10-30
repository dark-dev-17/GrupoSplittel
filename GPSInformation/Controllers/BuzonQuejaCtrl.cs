using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace GPSInformation.Controllers
{
    public class BuzonQuejaCtrl
    {
        #region Atributos
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public BuzonQuejaCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            
            this.darkManager.LoadObject(GpsManagerObjects.BuzonQueja);
            
        }
        #endregion

        #region metodos
        /// <summary>
        /// Listar buzones
        /// </summary>
        /// <returns></returns>
        public List<BuzonQueja> GetQuejaPersonas()
        {
            var quejas = darkManager.BuzonQueja.Get();
          
            
            return quejas.OrderByDescending(a => a.Creacion).ToList();
        }


        public BuzonQueja GetLast()
        {
            var quejas = darkManager.BuzonQueja.Get(darkManager.BuzonQueja.GetLastId());

            return quejas;
        }

        /// <summary>
        /// Registrar nuevo buzón
        /// </summary>
        /// <param name="BuzonQueja"></param>
        public BuzonQueja AddQueja(BuzonQueja BuzonQueja)
        {
            darkManager.StartTransaction();
            try
            {
                BuzonQueja.Creacion = DateTime.Now;
                BuzonQueja.SourceCliente = "--";
                darkManager.BuzonQueja.Element = BuzonQueja;
                if (!darkManager.BuzonQueja.Add())
                {
                    throw new Exceptions.GpExceptions("Error al guardar");
                }
                darkManager.Commit();
                return BuzonQueja;
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        /// <summary>
        /// Enviar notificacion por correo a usuarios con autorizacion a permiso #44
        /// </summary>
        /// <param name="body"></param>
        public void EnviarCorreo(string body)
        {
            try
            {
                bool enviar = false;
                this.darkManager.LoadObject(GpsManagerObjects.Usuario);
                this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
                this.darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
                var acceos = darkManager.AccesosSistema.GetList("IdSubModulo", "44", "TieneAcceso", "1");
                acceos.ForEach(acc => {
                    var usuario = darkManager.Usuario.Get(acc.IdUsuario);

                    if(usuario != null)
                    {
                        var empleado = darkManager.View_empleado.Get(usuario.IdPersona);
                        if (empleado != null)
                        {
                            darkManager.EmailServ_.AddListTO(empleado.Correo);
                            enviar = true;
                        }
                    }
                });

                if (enviar)
                {
                    darkManager.EmailServ_.Send(body, string.Format("Se ha generado un nuevo buzón"));
                    darkManager.RestartEmail();
                }
                
            }
            catch (SmtpException ex)
            {
                //throw ex;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public void Terminar()
        {
            darkManager.CloseConnection();
            darkManager = null;
        }
        #endregion
    }
}
