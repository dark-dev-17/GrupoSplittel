using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoFichaTecnica
    {
        #region Propiedades
        public int Id { get; set; }
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        public string Ruta { get; set; }
        [Display(Name = "PDF")]
        [DataType(DataType.Upload)]
        [Required]
        public IFormFile FichaPDF { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoFichaTecnica()
        {
            
        }
        public Ecom_ProductoFichaTecnica()
        {

        }
        public Ecom_ProductoFichaTecnica(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_FichaTecnica");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Codigo, "Codigo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Ruta, "Rutaa", "TEXT");
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
                Ecom_DBConnection_.StartProcedure("Admin_FichaTecnica");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Codigo, "Codigo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Ruta, "Rutaa", "TEXT");
                Ecom_DBConnection_.AddParameter(ModeProcedure, "ModeProcedure", "INT");
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
        public string GetLastCodigo()
        {
            try
            {
               return Ecom_DBConnection_.ExecuteScalarString("");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            
        }
        public bool GetByRute(string Rute)
        {
            List<Ecom_ProductoFichaTecnica>  List = ReadDatReader(string.Format("SELECT * FROM catalogo_fichas_tecnicas where ruta like '{0}'", Rute));
            if(List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Codigo = item.Codigo;
                    Ruta = item.Ruta;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProductoFichaTecnica> Get()
        {
            return ReadDatReader("SELECT * FROM catalogo_fichas_tecnicas;");
        }
        private List<Ecom_ProductoFichaTecnica> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoFichaTecnica> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoFichaTecnica>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoFichaTecnica
                        {
                           Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                           Codigo = Data.IsDBNull(1) ? "" : Data.GetString(1),
                           Ruta = Data.IsDBNull(1) ? "" : Data.GetString(2),
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
