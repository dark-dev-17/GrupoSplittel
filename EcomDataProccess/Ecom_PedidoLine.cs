using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;

namespace EcomDataProccess
{
    public class Ecom_PedidoLine
    {
        #region Propiedades
        [Display(Name = "Num Ecom")]
        public int DocNumEcommerce { get; set; }
        [Display(Name = "Codigo")]
        public string ItemCode { get; set; }
        [Display(Name = "Descripción")]
        public string Dscription { get; set; }
        [Display(Name = "Descuento")]
        public double PorcentDiscount { get; set; }
        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }
        [Display(Name = "Precio")]
        public double Price { get; set; }
        [Display(Name = "Moneda")]
        public string Currency { get; set; }
        public string Imageprincipal { get; set; }
        [Display(Name = "Configurable class")]
        public string Code_confgurable { get; set; }
        public string ImageLink { get { return ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString() + string.Format(@"/store/public/images/img_spl/productos/{0}/thumbnail/{1}", ItemCode, Imageprincipal); } }
        [Display(Name = "Total")]
        public double LineTotal { get; set; }
        [Display(Name = "SubTotal")]
        public double LineSubTotal { get; set; }
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
        public bool Update(string TypeItem, int mode)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PedidoDetalle");
                Ecom_DBConnection_.AddParameter(DocNumEcommerce, "DocNumEcommerce", "INT");
                Ecom_DBConnection_.AddParameter(ItemCode, "ItemCode", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Dscription, "Dscription", "DOUBLE");
                Ecom_DBConnection_.AddParameter(PorcentDiscount, "PorcentDiscount", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Price, "Price", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Quantity, "Quantity", "INT");
                Ecom_DBConnection_.AddParameter(Currency, "Currency", "VARCHAR");
                Ecom_DBConnection_.AddParameter(TypeItem, "TypeItem", "VARCHAR");
                Ecom_DBConnection_.AddParameter(mode, "ModeProcedure", "INT");
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
        public bool Add(string TypeItem,int mode)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PedidoDetalle");
                Ecom_DBConnection_.AddParameter(DocNumEcommerce, "DocNumEcommerce", "INT");
                Ecom_DBConnection_.AddParameter(ItemCode, "ItemCode", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Dscription, "Dscription", "DOUBLE");
                Ecom_DBConnection_.AddParameter(PorcentDiscount, "PorcentDiscount", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Price, "Price", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Quantity, "Quantity", "INT");
                Ecom_DBConnection_.AddParameter(Currency, "Currency", "VARCHAR");
                Ecom_DBConnection_.AddParameter(TypeItem, "TypeItem", "VARCHAR");
                Ecom_DBConnection_.AddParameter(mode, "ModeProcedure", "INT");
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
        public bool Get(string CodeItem)
        {
            string Statement = string.Format("SELECT * FROM Admin_CotizacionesDetalle  where codigo = '{0}';", CodeItem);
            try
            {
                List<Ecom_PedidoLine> List = ReadDatReader(Statement);
                if(List.Count == 1)
                {
                    List.ForEach(item => {
                        ItemCode = ItemCode;
                        Dscription = Dscription;
                        Quantity = Quantity;
                        Currency = Currency;
                        LineSubTotal = LineSubTotal;
                        LineTotal = LineTotal;
                        PorcentDiscount = PorcentDiscount;
                        Imageprincipal = Imageprincipal;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("No se ha podido encontrar la partida seleccionada");
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_PedidoLine> GetByPedido(int DocNumEcommerce)
        {
            string Statement = string.Format("SELECT * FROM Admin_CotizacionesDetalle  where id_cotizacion = '{0}';", DocNumEcommerce);
            try
            {
                return ReadDatReader(Statement);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_PedidoLine> ReadDatReader(string Statement)
        {
            List<Ecom_PedidoLine> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_PedidoLine>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_PedidoLine
                        {
                            ItemCode = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Dscription = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Quantity = Data.IsDBNull(3) ? 0 : (double)Data.GetDouble(3),
                            Currency = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            LineSubTotal = Data.IsDBNull(4) ? 0 : (double)Data.GetDouble(4),
                            LineTotal = Data.IsDBNull(5) ? 0 : (double)Data.GetDouble(5),
                            PorcentDiscount = Data.IsDBNull(7) ? 0 : (double)Data.GetDouble(7),
                            Imageprincipal = Data.IsDBNull(10) ? "" : Data.IsDBNull(10) ? "" : Data.GetString(10),
                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Sin registros";
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
