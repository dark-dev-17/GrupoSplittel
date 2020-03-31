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
                    return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
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
                    return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
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