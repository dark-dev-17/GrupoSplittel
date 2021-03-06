﻿using System;
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
    public class ConfigurableController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();

        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();

        public Ecom_DBConnection Ecom_DBConnection_ { get; private set; }

        [AccessView(IdAction = 27)]
        public ActionResult index()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_ProductoConfigurable> Ecom_Producto_ = new Ecom_ProductoConfigurable(Ecom_DBConnection_).Get();
                Ecom_DBConnection_.CloseConnection();
                return View(Ecom_Producto_);
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }

        // GET: Configurable/Details/5
        public ActionResult Detalle(string id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoConfigurable Ecom_ProductoConf_ = (Ecom_ProductoConfigurable)ecomData.GetObject(ObjectSource.ProductoConfigurable);
                bool result = Ecom_ProductoConf_.Get(id);
                if (result)
                {
                    //Ecom_Producto Ecom_Producto_ = (Ecom_Producto)ecomData.GetObject(ObjectSource.ProductoFijo);
                    //Ecom_ProductoConf_.Productos = await Ecom_Producto_.Get(Ecom_ProductoConf_.Ecom_ProductoSubCategoria_.Id_subcategoria, FiltroProducto.Subcategoria);
                    return View(Ecom_ProductoConf_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = string.Format("El producto con codigo: '{0}' no fue encontrado", id) });
                }
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 28)]
        public ActionResult DataConfig(string ItemCode, string imageName)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessBysalesEmp = new Ecom_Producto(Ecom_DBConnection_).UpdImagenPrincipal(ItemCode, imageName);
                Ecom_DBConnection_.CloseConnection();
                return Ok("good");
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 28)]
        public ActionResult DataActDescEcommerce(bool Active, string ItemCode)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_ProductoConfigurable Ecom_Producto_ = new Ecom_ProductoConfigurable(Ecom_DBConnection_);
                bool result = Ecom_Producto_.Get(ItemCode);
                if (result)
                {
                    Ecom_Producto_.IsActiveEcommerce = Active;
                    if (Ecom_Producto_.Update())
                    {
                        return Ok(Ecom_DBConnection_.Message);
                    }
                    else
                    {
                        return BadRequest(Ecom_DBConnection_.Message);
                    }
                }
                else
                {
                    return BadRequest(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 29)]
        public ActionResult DataActDescProximamente(bool Active, string ItemCode)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_ProductoConfigurable Ecom_Producto_ = new Ecom_ProductoConfigurable(Ecom_DBConnection_);
                bool result = Ecom_Producto_.Get(ItemCode);
                if (result)
                {
                    Ecom_Producto_.IsProximanente = Active;
                    if (Ecom_Producto_.Update())
                    {
                        return Ok(Ecom_DBConnection_.Message);
                    }
                    else
                    {
                        return BadRequest(Ecom_DBConnection_.Message);
                    }
                }
                else
                {
                    return BadRequest(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }
    }
}