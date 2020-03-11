using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EcomDataProccess
{
    public class Ecom_PedidoLine
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public DateTime DocDate { get; set; }
        public double PorcentDiscount { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public string ImageLink { get { return ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString() + string.Format(@"/store/public/images/img_spl/productos/{0}/1.jpg", ItemCode); } }
        public double Rate { get; set; }
        public double LineTotal { get; set; }
        public double LineSubTotal { get; set; }
        public double TotalFrgn { get; set; }
        public double VatPercent { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_PedidoLine()
        {
            
        }
        public Ecom_PedidoLine()
        {

        }
        public Ecom_PedidoLine(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public List<Ecom_PedidoLine> GetByPedido(int DocNumEcommerce)
        {
            List<Ecom_PedidoLine> List = null;
            MySqlDataReader Data = null;
            string Statement = string.Format("SELECT * FROM Admin_CotizacionesDetalle  where id_cotizacion = '{0}';", DocNumEcommerce);
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_PedidoLine>();
                while (Data.Read())
                {
                    List.Add(new Ecom_PedidoLine
                    {
                        ItemCode = Data.GetString(1),
                        Dscription = Data.GetString(2),
                        Quantity = (double)Data.GetDouble(3),
                        Currency = Data.GetString(6),
                        LineSubTotal = (double)Data.GetDouble(4),
                        LineTotal = (double)Data.GetDouble(5),
                        PorcentDiscount = (double)Data.GetDouble(7),
                    });

                }
                Data.Close();
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
