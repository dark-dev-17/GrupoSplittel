using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Notificacion
    {
        #region Propiedades
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descripcion{ get; set; }
        public DateTime Creacion { get; set; }
        public DateTime Revisado { get; set; }
        public string Estatus { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Parameter { get; set; }
        public string ExceptionMessage { get; set; }
        public int Area { get; set; }
        public int Usuario { get; set; }
        public Ecom_Usuario Ecom_Usuario_ { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Notificacion()
        {
            
        }
        public Ecom_Notificacion()
        {

        }
        public Ecom_Notificacion(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_Notificacion");
                Ecom_DBConnection_.AddParameter(Area, "Area", "INT");
                Ecom_DBConnection_.AddParameter(Id, "Id", "INT");
                Ecom_DBConnection_.AddParameter(Usuario, "Usuario", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Descripcion, "Descripcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Controller, "Controller", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Action, "Actionn", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Parameter, "Parameter", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ExceptionMessage, "ExceptionMessage", "TEXT");
                Ecom_DBConnection_.AddParameter(1, "ModeProcedure", "INT");
                int result = Ecom_DBConnection_.ExecProcedure();
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(int ModeProcedure)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_Notificacion");
                Ecom_DBConnection_.AddParameter(Area, "Area", "INT");
                Ecom_DBConnection_.AddParameter(Id, "Id", "INT");
                Ecom_DBConnection_.AddParameter(Usuario, "Usuario", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Descripcion, "Descripcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Controller, "Controller", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Action, "Actionn", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Parameter, "Parameter", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ModeProcedure, "ModeProcedure", "INT");
                Ecom_DBConnection_.AddParameter(ExceptionMessage, "ExceptionMessage", "TEXT");
                int result = Ecom_DBConnection_.ExecProcedure();
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }

        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }

        public bool Get(int idNotificacion)
        {
            List<Ecom_Notificacion> List = ReadDatReader(string.Format("SELECT * FROM t25_AdminNotificaciones where t25_pk01 = '{0}'", idNotificacion));
            if(List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Tipo = item.Tipo;
                    Descripcion = item.Descripcion;
                    Creacion = item.Creacion;
                    Revisado = item.Revisado;
                    Estatus = item.Estatus;
                    Controller = item.Controller;
                    Action = item.Action;
                    Parameter = item.Parameter;
                    Area = item.Area;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_Notificacion> GetTipo(string TipoNotificacion)
        {
            return ReadDatReader(string.Format("SELECT * FROM t25_AdminNotificaciones where t25_f001 = '{0}'", TipoNotificacion));
        }
        public List<Ecom_Notificacion> GetUsuario(int IdUsuario)
        {
            return ReadDatReader(string.Format("SELECT * FROM t25_AdminNotificaciones where t25_f001 = 'info'  and  t25_f003 = '{0}'", IdUsuario));
        }
        public List<Ecom_Notificacion> GetArea(int IdArea)
        {
            return ReadDatReader(string.Format("SELECT * FROM t25_AdminNotificaciones where t25_f001 = 'info' and t25_f010 = '{0}'", IdArea));
        }
        public List<Ecom_Notificacion> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t25_AdminNotificaciones;"));
        }
        private List<Ecom_Notificacion> ReadDatReader(string Statement)
        {
            List<Ecom_Notificacion> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Notificacion>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Notificacion
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Tipo = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Descripcion = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Usuario = Data.IsDBNull(3) ? 0 : Data.GetInt32(3),
                            Creacion = Data.IsDBNull(4) ? DateTime.Parse("2000-01-01") : Data.GetDateTime(4),
                            Revisado = Data.IsDBNull(5) ? DateTime.Parse("2000-01-01") : Data.GetDateTime(5),
                            Estatus = Data.IsDBNull(5) ? "Creado" : "Visto",
                            Controller = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            Action = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            Parameter = Data.IsDBNull(8) ? "" : Data.GetString(8),
                            ExceptionMessage = Data.IsDBNull(9) ? "" : Data.GetString(9),
                            Area = Data.IsDBNull(10) ? 0 : Data.GetInt32(10),
                        });
                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Registro no encontrado";
                }
                return List;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                {
                    Data.Close();
                }
            }
        }
        #endregion
    }
    }
