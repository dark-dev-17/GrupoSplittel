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
        public int GetTotal(string ModeBussiness, DateTime start, DateTime end)
        {
            int total;
            try
            {
                string Statement = string.Format("Admin_totalClientes|ModeBussiness@VARCHAR={0}&startdate@DATETIME={1}&enddate@DATETIME={2}", 
                    ModeBussiness, 
                    start.ToString("yyyy-MM-dd"), 
                    end.ToString("yyyy-MM-dd 23:59:59")
                );
                total = Ecom_DBConnection_.ExecuteProcedureInttt(Statement, "TotalClientes");
                return total;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
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
        public List<Ecom_Cliente> GetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            string Statement = string.Format("Admin_QuotationsDashboard|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}&ModeQuery@INT={4}",
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"),
                tipoDocumento,
                ModeBussiness,
                1);
            MySqlDataReader data = null;
            List<Ecom_Cliente> List;
            try
            {
                data = Ecom_DBConnection_.ExecuteStoreProcedureReader(Statement);
                List = new List<Ecom_Cliente>();
                while (data.Read())
                {
                    Ecom_Cliente cliente = new Ecom_Cliente();
                    cliente.Id_cliente = data.IsDBNull(0) ? 0 : data.GetInt32(0);
                    cliente.Nombre = data.IsDBNull(1) ? "" : data.GetString(1);
                    cliente.Apellidos = data.IsDBNull(2) ? "" : data.GetString(2);
                    cliente.NoDocs = data.IsDBNull(3) ? 0 : data.GetInt32(3);
                    cliente.CardCode = data.IsDBNull(4) ? "" : data.GetString(4);
                    List.Add(cliente);
                }
                return List;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
                }
            }
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
        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion
    }
}
