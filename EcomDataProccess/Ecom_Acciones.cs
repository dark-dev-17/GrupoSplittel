using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Acciones
    {
        #region Propiedades
        public int Id { set; get; }
        public string Description { set; get; }
        public string Route { set; get; }
        public string Action { set; get; }
        public string Text { set; get; }
        public string Icon { set; get; }
        public string ClassActive { set; get; }
        public bool isAccess { set; get; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Acciones()
        {
            
        }
        public Ecom_Acciones()
        {

        }
        public Ecom_Acciones(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public void SetConnectionMYsql(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        public List<Ecom_Acciones> Get(int idModulo, int id_user)
        {
            MySqlDataReader Data = null;
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_SubModulos where t01_pk01 = '{0}' and clienteKey = '{1}'", idModulo, id_user);
                List<Ecom_Acciones> Lista = new List<Ecom_Acciones>();
                Data = Ecom_DBConnection_.DoQuery(Statement);
                while (Data.Read())
                {
                    Ecom_Acciones acciones = new Ecom_Acciones();
                    acciones.Id = (int)Data.GetUInt32(0);
                    acciones.Description = Data.IsDBNull(1) ? "" : Data.GetString(1);
                    acciones.Route = Data.IsDBNull(2) ? "" : Data.GetString(2);
                    acciones.Action = Data.IsDBNull(3) ? "" : Data.GetString(3);
                    acciones.Text = Data.IsDBNull(4) ? "" : Data.GetString(4);
                    acciones.Icon = Data.IsDBNull(5) ? "" : Data.GetString(5);
                    acciones.isAccess = false;
                    Lista.Add(acciones);
                }
                Data.Close();
                return Lista;
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
        public List<Ecom_Acciones> Get(int idModulo)
        {
            MySqlDataReader Data = null;
            try
            {
                string Statement = string.Format("SELECT * FROM t02_admin_acciones where t01_pk01 = '{0}'", idModulo);
                List<Ecom_Acciones> Lista = new List<Ecom_Acciones>();
                Data = Ecom_DBConnection_.DoQuery(Statement);
                while (Data.Read())
                {
                    Ecom_Acciones acciones = new Ecom_Acciones();
                    acciones.Id = (int)Data.GetUInt32(0);
                    acciones.Description = Data.IsDBNull(1) ? "" : Data.GetString(1);
                    acciones.Route = Data.IsDBNull(5) ? "" : Data.GetString(5);
                    acciones.Action = Data.IsDBNull(6) ? "" : Data.GetString(6);
                    acciones.Text = Data.IsDBNull(7) ? "" : Data.GetString(7);
                    acciones.Icon = Data.IsDBNull(8) ? "" : Data.GetString(8);
                    acciones.isAccess = false;
                    Lista.Add(acciones);
                }
                Data.Close();
                return Lista;
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
        public bool CheckPermissToUser(int idUser, int IdAccion)
        {
            MySqlDataReader Data = null;
            try
            {
                bool access = false;
                string Statement = string.Format("SELECT * FROM t03_permisos where clienteKey = '{0}' and t02_pk01 = '{1}' and t03_f001 = '1';", idUser, IdAccion);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                if (Ecom_DBConnection_.CountDataReader(Data) == 1)
                    access = true;
                else
                    access = false;
                Data.Close();
                return access;
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
        public bool ChangePermissToUser(int idUser, int IdAccion, int permiss)
        {
            try
            {
                bool access = false;
                string Statement = string.Format("AdminEcom_changeAccessActions|ID_ACTION@INT={0}&ID_USER@INT={1}&ACCESS@INT={2}", IdAccion, idUser, permiss);
                bool DataReader = Ecom_DBConnection_.ExecuteProcedure(Statement, "");
                return access;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        private List<Ecom_Acciones> ReadDatReader(string Statement)
        {
            List<Ecom_Acciones> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Acciones>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Acciones
                        {

                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Usuario no encontrado";
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
