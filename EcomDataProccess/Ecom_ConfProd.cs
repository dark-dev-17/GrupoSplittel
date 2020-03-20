using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ConfProd
    {
        #region Propiedades
        private Ecom_DBConnection Ecom_DBConnection_;
        public string Producto { get; set; }
        public List<Ecom_ProducProp> ProducProps { get;  set; }
        #endregion

        #region Constructores
        ~Ecom_ConfProd()
        {
            
        }
        public Ecom_ConfProd()
        {

        }
        public Ecom_ConfProd(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        
        #endregion
    }
    public class Ecom_ProducProp
    {
        public string Label { get; set; }
        public string Tipo { get; set; }
        public List<Ecom_propiedades> Values { get; set; }
        public bool IsActive { get; set; }
    }
    public class Ecom_propiedades
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public bool Default { get; set; }
    }
}
