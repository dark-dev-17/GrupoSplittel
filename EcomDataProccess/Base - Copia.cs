using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Base
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        public int Idblog { get; set; }
        public int IdCliente { get; set; }
        public int IdComentario { get; set; }
        public string Tipo { get; set; }
        public string Fecha { get; set; }
        public bool Activo { get; set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Base()
        {
            
        }
        public Base()
        {

        }
        public Base(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_sp");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Idblog, "Idblog", "INT");
                Ecom_DBConnection_.AddParameter(IdCliente, "IdCliente", "INT");
                Ecom_DBConnection_.AddParameter(IdComentario, "IdComentario", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Fecha, "Fecha", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Activo, "Activo", "VARCHAR");
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

                Ecom_DBConnection_.StartProcedure("Admin_sp");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Idblog, "Idblog", "INT");
                Ecom_DBConnection_.AddParameter(IdCliente, "IdCliente", "INT");
                Ecom_DBConnection_.AddParameter(IdComentario, "IdComentario", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Fecha, "Fecha", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Activo, "Activo", "VARCHAR");
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
        public bool Get(int idDescripcion)
        {
            List<Base> List = ReadDatReader(string.Format("", idDescripcion));
            if (List.Count > 0)
            {
                List.ForEach(item => {

                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Base> Get()
        {
            return ReadDatReader(string.Format(""));
        }
        private List<Base> ReadDatReader(string Statement)
        {
            List<Base> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Base>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Base
                        {

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
