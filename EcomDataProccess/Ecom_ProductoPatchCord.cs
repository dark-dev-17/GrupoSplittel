using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoPatchCord
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Base")]
        [RegularExpression(@"^\d+\.\d{0,3}$",ErrorMessage ="Solo se permiten 3 decimales")]
        [Required]
        public double Base { get; set; }
        [Display(Name = "Factor")]
        [RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Factor { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoPatchCord()
        {
            
        }
        public Ecom_ProductoPatchCord()
        {

        }
        public Ecom_ProductoPatchCord(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioPatchCord");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Base, "Base_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Factor, "Factor_", "DOUBLE");
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

                Ecom_DBConnection_.StartProcedure("Admin_PrecioPatchCord");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Base, "Base_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Factor, "Factor_", "DOUBLE");
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
        public int GetlastId()
        {
            try
            {
                return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM t07_precios_patchcord");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Get(int IdElemento)
        {
            List<Ecom_ProductoPatchCord> List = ReadDatReader(string.Format("SELECT * FROM t07_precios_patchcord where id = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Tipo = item.Tipo;
                    Base = item.Base;
                    Factor = item.Factor;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProductoPatchCord> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t07_precios_patchcord;"));
        }
        private List<Ecom_ProductoPatchCord> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoPatchCord> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoPatchCord>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoPatchCord
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Tipo = Data.IsDBNull(1) ? "--" : Data.GetString(1),
                            Base = Data.IsDBNull(2) ? -1 : Data.GetDouble(2),
                            Factor = Data.IsDBNull(3) ? -1 : Data.GetDouble(3),
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
