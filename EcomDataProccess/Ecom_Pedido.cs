using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Pedido
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public double DocTotal { get; set; }
        public double DocSubTotal { get; set; }
        public double DocIva { get; set; }
        public double DocRate { get; set; }
        public string DocType { get; set; }
        public string TypeCustomer { get; set; }
        public string CardCode { get; set; }
        public string Cardname { get; set; }
        public string DocCur { get; set; }
        public string TrackNo { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string CFDIUser { get; set; }
        public int PorcentDisaccount { get; set; }
        public string ShipRefences { get; private set; }
        public bool RequireInvoice { get; private set; }
        public int Id_cliente { get; set; }
        public int TransportationCode { get; set; }
        public int DocNumEcommerce { get; set; }
        public int SAP_Estatus { get; set; }
        public int StatusProcessWS { get; private set; }
        public Ecom_Cliente Ecom_Cliente_ { get; private set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        public List<Ecom_PedidoLine> Ecom_PedidoLines_ { get; private set; }
        #endregion

        #region Constructores
        ~Ecom_Pedido()
        {
            
        }
        public Ecom_Pedido()
        {

        }
        public Ecom_Pedido(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public int GetNoDoumentos(DateTime start, DateTime end, string ModeBussiness,string TypeDoc)
        {
            string Statement = string.Format("AdminNoDocuments|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}", 
                start.ToString("yyyy-MM-dd"), 
                end.ToString("yyyy-MM-dd 23:59:59"),
                TypeDoc, 
                ModeBussiness);
            try
            {
                int Result = 0;
                Result = Ecom_DBConnection_.ExecuteProcedureInttt(Statement, "totalDoc");
                return Result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public double GetTotal(DateTime start, DateTime end, string Currency, string ModeBussiness)
        {
            // 
            string Statement = string.Format("Admin_TotalPedidos|startdate@DATETIME={0}&enddate@DATETIME={1}&moneda@VARCHAR={2}&ModeBussiness@VARCHAR={3}", 
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"), 
                Currency, 
                ModeBussiness);
            try
            {
                double Result = 0;
                Result = Ecom_DBConnection_.ExecuteProcedureDouble(Statement, "totalCost");
                return Result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool GetById(int DocNumEcommerce_)
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where id = '{0}'", DocNumEcommerce_);
            bool IsExists = false;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        Id_cliente = Data.IsDBNull(1) ? 0 : Data.GetInt32(1);
                        DocSubTotal = Data.IsDBNull(2) ? 0 : Data.GetDouble(2);
                        DocIva = Data.IsDBNull(3) ? 0 : Data.GetDouble(3);
                        DocTotal = Data.IsDBNull(4) ? 0 : Data.GetDouble(4);
                        DocDate = Data.IsDBNull(7) ? DateTime.Now : Data.GetDateTime(7);
                        Status = Data.IsDBNull(9) ? "" : Data.GetString(9);
                        PaymentMethod = Data.IsDBNull(10) ? "" : Data.GetString(10);
                        DocCur = Data.IsDBNull(11) ? "" : Data.GetString(11);
                        ShipTo = Data.IsDBNull(12) ? "" : Data.GetString(12);
                        BillTo = Data.IsDBNull(13) ? "" : Data.GetString(13);
                        TransportationCode = Data.IsDBNull(15) ? 0 : Data.GetInt32(15);
                        DocRate = Data.IsDBNull(16) ? 0 : Data.GetDouble(16);
                        CFDIUser = Data.IsDBNull(18) ? "" : Data.GetString(18);
                        PorcentDisaccount = Data.IsDBNull(19) ? 0 : Data.GetInt32(19);
                        StatusProcessWS = Data.IsDBNull(21) ? -1 : Data.GetInt32(21);
                        ShipRefences = Data.IsDBNull(22) ? "" : Data.GetString(22);
                        TypeCustomer = Data.IsDBNull(20) ? "" : Data.GetString(20);
                        RequireInvoice = Data.IsDBNull(24) ? false: (Data.GetString(24) == "false" ? false : true);
                        DocNumEcommerce = DocNumEcommerce_;
                        if (DocCur != "USD")
                        {
                            DocIva *= DocRate;
                            DocTotal *= DocRate;
                            DocSubTotal *= DocRate;
                        }
                        //sentencia para traer cliente
                        
                    }
                    IsExists = true;
                    Data.Close();
                    //obtener lineas
                    Ecom_PedidoLines_ = new Ecom_PedidoLine(Ecom_DBConnection_).GetByPedido(DocNumEcommerce);
                    //obtener cliente
                    Ecom_Cliente_ = new Ecom_Cliente(Ecom_DBConnection_);
                    Ecom_Cliente_.Get(Id_cliente);
                }
                return IsExists;
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
        public List<Ecom_Pedido> GetCotizacion(string CardCode_)
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where estatus = 'C' and cardcode = '{0}' order by fecha desc", CardCode_);
            return ReadDatReader(Statement);
        }
        public List<Ecom_Pedido> GetPending(string CardCode_)
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where estatus = 'P' and estatusWS != 0 and cardcode = '{0}' order by fecha desc", CardCode_);
            return ReadDatReader(Statement);
        }
        public List<Ecom_Pedido> GetCotizacion()
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where estatus = 'C' order by fecha desc");
            return ReadDatReader(Statement);
        }
        public List<Ecom_Pedido> GetPending()
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where estatus = 'P' and estatusWS != 0 order by fecha desc");
            return ReadDatReader(Statement);
        }
        public List<Ecom_Pedido> GetCliente(int IdCliente)
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where id_cliente = '{0}' order by fecha desc", CardCode);
            return ReadDatReader(Statement);
        }
        public List<Ecom_Pedido> GetByBussinessPartner(string CardCode)
        {
            string Statement = string.Format("select * from Admin_pedidosInfo where cardcode = '{0}' order by fecha desc", CardCode);
            return ReadDatReader(Statement);
        }
        private List<Ecom_Pedido> ReadDatReader(string Statement)
        {
            List<Ecom_Pedido> List;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Pedido>();
                while (Data.Read())
                {
                    Ecom_Pedido Ecom_Cotizacion_ = new Ecom_Pedido();
                    Ecom_Cotizacion_.DocNumEcommerce = Data.IsDBNull(0) ? 0 : Data.GetInt32(0);
                    Ecom_Cotizacion_.CardCode = Data.IsDBNull(23) ? "" : Data.GetString(23);
                    Ecom_Cotizacion_.Id_cliente = Data.IsDBNull(1) ? 0 : Data.GetInt32(1);
                    Ecom_Cotizacion_.TypeCustomer = Data.IsDBNull(20) ? "" : Data.GetString(20);
                    Ecom_Cotizacion_.Status = Data.IsDBNull(11) ? "" : Data.GetString(11);
                    Ecom_Cotizacion_.Cardname = "";
                    Ecom_Cotizacion_.DocSubTotal = Data.IsDBNull(2) ? 0 : Data.GetDouble(2);
                    Ecom_Cotizacion_.DocIva = Data.IsDBNull(3) ? 0 : Data.GetDouble(3);
                    Ecom_Cotizacion_.DocTotal = Data.IsDBNull(4) ? 0 : Data.GetDouble(4);
                    Ecom_Cotizacion_.DocDate = Data.GetDateTime(7);
                    Ecom_Cotizacion_.DocRate = Data.GetDouble(16);
                    Ecom_Cotizacion_.DocCur = "USD";
                    List.Add(Ecom_Cotizacion_);
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
        public string GetStatusDescription()
        {
            if (SAP_Estatus == 1)
            {
                return "En proceso";
            }
            else if (SAP_Estatus == 2)
            {
                return "Surtiendo";
            }
            else if (SAP_Estatus == 3)
            {
                return "Embarcando";
            }
            else if (SAP_Estatus == 4)
            {
                return "Enviando";
            }
            else if (SAP_Estatus == 5)
            {
                return "Entregando";
            }
            else
            {
                return "Error";
            }
        }
        #endregion
    }
    }
