using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
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
                        " Para poder adquirir tu pedido ingresa a <a href='https://fibremex.com/store/views/Home/'> fibremex.com </a> en la sección de mis cotizaciones del apartado de <strong>Mi cuenta</strong> ", id);
                    bool emailStatus = emailStatus = ecomData.Ecom_Email_.SendMailNotification(htmls, Ecom_Pedido_.Ecom_Cliente_.Email, "", SMTP_list_Sistemas);

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


    }
}