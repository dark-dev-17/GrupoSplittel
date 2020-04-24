using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoMPO
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Componente")]
        public string Componente { get; set; }
        [Display(Name = "Precio")]
        [RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Precio { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoMPO()
        {
            
        }
        public Ecom_ProductoMPO()
        {

        }
        public Ecom_ProductoMPO(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioMPO");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
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

                Ecom_DBConnection_.StartProcedure("Admin_PrecioMPO");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
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
            List<Ecom_ProductoMPO> List = ReadDatReader(string.Format("SELECT * FROM t24_precios_mpo where id = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Componente = item.Componente;
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
                return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM t24_precios_mpo;");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }

        public List<Ecom_ProductoMPO> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t24_precios_mpo;"));
        }
        private List<Ecom_ProductoMPO> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoMPO> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoMPO>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoMPO
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Componente = Data.IsDBNull(1) ? "--" : Data.GetString(1),
                            Precio = Data.IsDBNull(2) ? -1 : Data.GetDouble(2),
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
