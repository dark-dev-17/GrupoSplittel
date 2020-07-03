using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAdmin.Models;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class PedidoDetalleController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();

        private readonly string SMTP_Server = ConfigurationManager.AppSettings["SMTP_Server"].ToString();
        private readonly string SMTP_Port = ConfigurationManager.AppSettings["SMTP_Port"].ToString();
        private readonly string SMTP_ssl = ConfigurationManager.AppSettings["SMTP_ssl"].ToString();
        private readonly string SMTP_account = ConfigurationManager.AppSettings["SMTP_account"].ToString();
        private readonly string SMTP_user = ConfigurationManager.AppSettings["SMTP_user"].ToString();
        private readonly string SMTP_pass = ConfigurationManager.AppSettings["SMTP_pass"].ToString();
        private readonly string SMTP_list_Sistemas = ConfigurationManager.AppSettings["SMTP_list_Sistemas"].ToString();
        private readonly string ProductionMode = ConfigurationManager.AppSettings["ProductionMode"].ToString();


        // POST: PedidoDetalle/DataAddCostoEnvio/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 39)]
        public ActionResult DataAddCostoEnvio(int id, double price, double PorcentDiscount )
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                // TODO: Add delete logic here
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_PedidoLine Ecom_PedidoLine_ = (Ecom_PedidoLine)ecomData.GetObject(ObjectSource.PedidoLine);
                Ecom_PedidoLine_.DocNumEcommerce = id;
                Ecom_PedidoLine_.Price = price;
                Ecom_PedidoLine_.PorcentDiscount = PorcentDiscount;
                bool result = Ecom_PedidoLine_.Add("CostoEnvio", 4);
                if (result)
                {
                    ecomData.Ecom_Email_ = new Ecom_Email(SMTP_Server, SMTP_account, Int32.Parse(SMTP_Port), SMTP_user, SMTP_pass, (SMTP_ssl == "true" ? true : false));
                    Ecom_Pedido Ecom_Pedido_ = (Ecom_Pedido)ecomData.GetObject(ObjectSource.Pedido);
                    Ecom_Pedido_.GetById(id);
                    string htmls = string.Format("" +
                        "<p align='left'>Se ha asignado el costo de envio a tu pedido : <strong></strong>{0}</strong></strong></p>" +
                        " Para poder adquirir tu pedido ingresa a <a href='https://fibremex.com/fibra-optica/views/Home/'> fibremex.com </a> en la sección de mis cotizaciones del apartado de <strong>Mi cuenta</strong> ", id);

                    //verificar que exista una proceso de email



                    bool emailStatus = emailStatus = ecomData.SendMailNotification(1,htmls, Ecom_Pedido_.Ecom_Cliente_.Email);

                    string Respuesta = ecomData.GetLastMessage(ServerSource.Ecommerce);

                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", string.Format("Ha definido costos de envio de la orden: {0}", id), "Pedido", "Detalle", id + "", "");

                    return Ok(Respuesta + " " + (!emailStatus ? ecomData.Ecom_Email_.GetMessage() : ""));
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        // POST: PedidoDetalle/DataUpdCostoEnvioPrice/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 39)]
        public ActionResult DataUpdCostoEnvioPrice(int id, double price, double PorcentDiscount)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                // TODO: Add delete logic here
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_PedidoLine Ecom_PedidoLine_ = (Ecom_PedidoLine)ecomData.GetObject(ObjectSource.PedidoLine);
                Ecom_PedidoLine_.DocNumEcommerce = id;
                Ecom_PedidoLine_.Price = price;
                Ecom_PedidoLine_.PorcentDiscount = PorcentDiscount;
                bool result = Ecom_PedidoLine_.Add("CostoEnvio", 5);
                if (result)
                {
                    string Respuesta = ecomData.GetLastMessage(ServerSource.Ecommerce);
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", string.Format("Ha actualizado costos de envio de la orden: {0}",id), "Pedido", "Detalle", id + "", "");

                    return Ok(Respuesta);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        public IActionResult DataGetInProccess()
        {
            Ecommerce Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.SAPBussinessOne);
                Ecommerce_.ecomData.Connect(ServerSource.Splitnet);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);

                bool AccessGeneral = Ecommerce_.ValidActionUser(16);
                bool AccessSpecial = Ecommerce_.ValidActionUser(15);
                if (AccessGeneral && !AccessSpecial)
                {
                    //documentos generales
                    Ecommerce_.sAPData.OpenConnection(SAPDataProcess.ConnectionSAP.Database);
                    SAPDataProcess.SAP_Document sAP_Document = (SAPDataProcess.SAP_Document)Ecommerce_.sAPData.GetObject(SAPDataProcess.SAPDataBaseObj.Document);
                    List<SAPDataProcess.SAP_Document> ecom_Pedidos = sAP_Document.GetInProcess();
                    ecom_Pedidos.ForEach(doc =>
                    {
                        Ecom_Pedido ecom_Pedido = (Ecom_Pedido)Ecommerce_.ecomData.GetObject(ObjectSource.Pedido);
                        ecom_Pedido.GetById(Int32.Parse(doc.DocNumEcommerce));
                        doc.ObjetoAux = ecom_Pedido;
                    });
                    return Ok(ecom_Pedidos);
                }
                else if (!AccessGeneral && AccessSpecial)
                {
                    Ecommerce_.sAPData.OpenConnection(SAPDataProcess.ConnectionSAP.Database);
                    SAPDataProcess.SAP_Document sAP_Document = (SAPDataProcess.SAP_Document)Ecommerce_.sAPData.GetObject(SAPDataProcess.SAPDataBaseObj.Document);

                    List<SAPDataProcess.SAP_Document> ecom_Pedidos = new List<SAPDataProcess.SAP_Document>();

                    Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                    Ecommerce_.GetBussinessPartnerByUser().Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp =>
                    {
                        sAP_Document.GetInProcess(bp.CardCode).ForEach(cli =>
                        {
                            ecom_Pedidos.Add(cli);
                        });
                    });
                    ecom_Pedidos.ForEach(doc =>
                    {
                        Ecom_Pedido ecom_Pedido = (Ecom_Pedido)Ecommerce_.ecomData.GetObject(ObjectSource.Pedido);
                        ecom_Pedido.GetById(Int32.Parse(doc.DocNumEcommerce));
                        doc.ObjetoAux = ecom_Pedido;
                    });
                    return Ok(ecom_Pedidos);
                }
                else
                    return BadRequest("Error en la configuración de permisos de usuario");
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }

        }
    }
}