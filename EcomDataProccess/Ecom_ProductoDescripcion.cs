using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoDescripcion
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }
        [Display(Name = "Copygriting para SEO")]
        [Required]
        public string DescripcionCEO { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoDescripcion()
        {
            
        }
        public Ecom_ProductoDescripcion()
        {

        }
        public Ecom_ProductoDescripcion(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ProductoDescripcion");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Codigo, "Codigo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Descripcion, "Descripcion", "TEXT");
                Ecom_DBConnection_.AddParameter(DescripcionCEO, "DescripcionCEO", "TEXT");
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

                Ecom_DBConnection_.StartProcedure("Admin_ProductoDescripcion");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Codigo, "Codigo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Descripcion, "Descripcion", "TEXT");
                Ecom_DBConnection_.AddParameter(DescripcionCEO, "DescripcionCEO", "TEXT");
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
        public bool Get(string idDescripcion)
        {
            List<Ecom_ProductoDescripcion> List = ReadDatReader(string.Format("SELECT * FROM catalogo_descripciones where id_desc_larga = '{0}'", idDescripcion));
            if(List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Codigo = item.Codigo;
                    Descripcion = item.Descripcion;
                    DescripcionCEO = item.DescripcionCEO;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProductoDescripcion> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM catalogo_descripciones;"));
        }
        private List<Ecom_ProductoDescripcion> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoDescripcion> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoDescripcion>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoDescripcion
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Codigo = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Descripcion = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            DescripcionCEO = Data.IsDBNull(3) ? "" : Data.GetString(3),
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
