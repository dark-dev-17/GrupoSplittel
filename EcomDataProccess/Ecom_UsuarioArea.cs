using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_UsuarioArea
    {
        #region Propiedades
        public int Id { get; set; }
        public string Descripcion { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_UsuarioArea()
        {
            
        }
        public Ecom_UsuarioArea()
        {

        }
        public Ecom_UsuarioArea(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Get(int id)
        {
            try
            {
                List<Ecom_UsuarioArea> List = ReadDatReader(string.Format("SELECT * FROM areas where id_area = '{0}';",id));
                if (List.Count > 0)
                {
                    List.ForEach(item => {
                        Id = item.Id;
                        Descripcion = item.Descripcion;
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Ecom_Exception)
            {
                throw;
            }
        }
        public List<Ecom_UsuarioArea> Get()
        {
            try
            {
                return ReadDatReader(string.Format("SELECT * FROM areas;"));
            }
            catch (Ecom_Exception)
            {
                throw;
            }
        }
        private List<Ecom_UsuarioArea> ReadDatReader(string Statement)
        {
            List<Ecom_UsuarioArea> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_UsuarioArea>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_UsuarioArea
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Descripcion = Data.IsDBNull(1) ? "" : Data.GetString(1),
                        }); ;

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
