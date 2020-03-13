using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ProductoCategoria
    {
        #region Propiedades
        public int Id { get; set; }
        public string Id_categoria { get; set; }
        public string Description { get; set; }
        public int Total { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoCategoria()
        {
            
        }
        public Ecom_ProductoCategoria()
        {

        }
        public Ecom_ProductoCategoria(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public List<Ecom_ProductoCategoria> GetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            string Statement = string.Format("Admin_QuotationsDashboard|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}&ModeQuery@INT={4}",
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"),
                tipoDocumento,
                ModeBussiness,
                2);
            MySqlDataReader data = null;
            List<Ecom_ProductoCategoria> List;
            try
            {
                data = Ecom_DBConnection_.ExecuteStoreProcedureReader(Statement);
                List = new List<Ecom_ProductoCategoria>();
                while (data.Read())
                {
                    Ecom_ProductoCategoria categoria = new Ecom_ProductoCategoria();
                    categoria.Id_categoria = data.IsDBNull(0) ? "" : (data.GetString(0) + "");
                    categoria.Description = data.IsDBNull(1) ? "" : data.GetString(1);
                    categoria.Total = data.IsDBNull(2) ? 0 : (int)data.GetDouble(2);
                    List.Add(categoria);
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
        private List<Ecom_ProductoCategoria> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoCategoria> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoCategoria>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoCategoria
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
