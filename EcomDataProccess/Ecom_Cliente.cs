using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Cliente {
        #region Propiedades
        public int Id_cliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaReistro { get; set; }
        public DateTime LastLogin { get; set; }
        public string TipoCliente { get; set; }
        public string CardCode { get; set; }
        public string Sociedad { get; set; }
        public int NoDocs { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Cliente()
        {
            
        }
        public Ecom_Cliente()
        {

        }
        public Ecom_Cliente(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Get(int id_cliente)
        {
            string Statement = string.Format("select * from admin_clientes where id_cliente = '{0}' ", id_cliente);
            MySqlDataReader Data = null;
            bool result = false;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        Id_cliente = Data.IsDBNull(0) ? 0 : Data.GetInt32(0);
                        Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1);
                        Apellidos = Data.IsDBNull(2) ? "" : Data.GetString(2);
                        Telefono = Data.IsDBNull(3) ? "" : Data.GetString(3);
                        Email = Data.IsDBNull(4) ? "" : Data.GetString(4);
                        FechaReistro = Data.IsDBNull(5) ? DateTime.Now : Data.GetDateTime(5);
                        LastLogin = Data.IsDBNull(6) ? DateTime.Now : Data.GetDateTime(6);
                        TipoCliente = Data.IsDBNull(7) ? "" : Data.GetString(7);
                        CardCode = Data.IsDBNull(8) ? "" : Data.GetString(8);
                        Sociedad = Data.IsDBNull(9) ? "" : Data.GetString(9);
                    }
                    result = true;
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Cliente no encontrado";
                }
                return result;
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
        public List<Ecom_Cliente> Get()
        {
            string Statement = string.Format("select * from admin_clientes order by last_login desc");
            return ReadDatReader(Statement);
        }
        public List<Ecom_Cliente> Get(string CardCode_)
        {
            string Statement = string.Format("select * from admin_clientes where cardcode = '{0}' order by last_login desc", CardCode_);
            return ReadDatReader(Statement);
        }
        private List<Ecom_Cliente> ReadDatReader(string Statement)
        {
            MySqlDataReader Data = null;
            List<Ecom_Cliente> List = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Cliente>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Cliente
                        {
                            Id_cliente = Data.IsDBNull(0) ? 0 : Data.GetInt32(0),
                            Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Apellidos = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Telefono = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            Email = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            FechaReistro = Data.IsDBNull(5) ? DateTime.Now : Data.GetDateTime(5),
                            LastLogin = Data.IsDBNull(6) ? DateTime.Now : Data.GetDateTime(6),
                            TipoCliente = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            CardCode = Data.IsDBNull(8) ? "" : Data.GetString(8),
                            Sociedad = Data.IsDBNull(9) ? "" : Data.GetString(9)
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
