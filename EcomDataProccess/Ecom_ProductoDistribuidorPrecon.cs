using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoDistribuidorPrecon
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Componente")]
        public string Componente { get; set; }
        [Display(Name = "Cantidad")]
        public double Cantidad { get; set; }
        [Display(Name = "Precio")]
        //[RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Precio { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoDistribuidorPrecon()
        {
            
        }
        public Ecom_ProductoDistribuidorPrecon()
        {

        }
        public Ecom_ProductoDistribuidorPrecon(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioDistribuidorPrecon");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Cantidad, "Cantidad_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
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
        public bool Update(int modeUpdate)
        {
            try
            {

                Ecom_DBConnection_.StartProcedure("Admin_PrecioDistribuidorPrecon");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Cantidad, "Cantidad_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(modeUpdate, "ModeProcedure", "INT");
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
        public bool Get(int IdElemento)
        {
            List<Ecom_ProductoDistribuidorPrecon> List = ReadDatReader(string.Format("SELECT * FROM t30_precios_distribuidores_preconectorizados where id = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Tipo = item.Tipo;
                    Componente = item.Componente;
                    Cantidad = item.Cantidad;
                    Precio = item.Precio;
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetlastId()
        {
            try
            {
                return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM t30_precios_distribuidores_preconectorizados;");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }

        public List<Ecom_ProductoDistribuidorPrecon> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t30_precios_distribuidores_preconectorizados;"));
        }
        private List<Ecom_ProductoDistribuidorPrecon> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoDistribuidorPrecon> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoDistribuidorPrecon>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoDistribuidorPrecon
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Tipo = Data.IsDBNull(1) ? "--" : Data.GetString(1),
                            Componente = Data.IsDBNull(2) ? "--" : Data.GetString(2),
                            Cantidad = Data.IsDBNull(3) ? -1 : Data.GetDouble(3),
                            Precio = Data.IsDBNull(4) ? -1 : Data.GetDouble(4),
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
        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion
    }
}
