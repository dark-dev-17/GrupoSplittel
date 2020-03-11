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
    public class PedidoController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        [AccessMultipleView(IdAction = new int[] { 22 })]
        public ActionResult Detalle(int id)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Pedido Ecom_Pedido_ = new Ecom_Pedido(Ecom_DBConnection_);
                bool result = Ecom_Pedido_.GetById(id);
                Ecom_DBConnection_.CloseConnection();
                if (result)
                {
                    if(Ecom_Pedido_.StatusProcessWS == 0)
                    {
                        SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                        SAP_DBConnection_.OpenConnection();
                        SAPDataProcess.SAP_Document sAP_Document = new SAPDataProcess.SAP_Document(SAP_DBConnection_);
                        sAP_Document.GetSapEstatus(Ecom_Pedido_.DocNumEcommerce);
                        Ecom_Pedido_.TrackNo = sAP_Document.TrackNo;
                        Ecom_Pedido_.DocEntry = sAP_Document.DocEntry;
                        Ecom_Pedido_.SAP_Estatus = Int32.Parse(sAP_Document.Status);
                        SAP_DBConnection_.CloseDataBaseAccess();
                    }
                }
                return View(Ecom_Pedido_);
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }
        [AccessMultipleView(IdAction = new int[] { 23, 24 })]
        public ActionResult Pendiente()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 23);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 24);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Pedido> Ecom_Pedido_ = new Ecom_Pedido(Ecom_DBConnection_).GetPending();
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Pedido_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new Ecom_SAP().GetBussPartByEmp(USR_IdSplinnet);
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Pedido> Ecom_Pedido_ = new List<Ecom_Pedido>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new Ecom_Pedido(Ecom_DBConnection_).GetPending(bp.CardCode).ForEach(cli =>
                        {
                            Ecom_Pedido_.Add(cli);
                        });
                    });
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Pedido_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }

        [AccessMultipleView(IdAction = new int[] { 11, 12 })]
        public ActionResult Cotizacion()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 12);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet,11);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Pedido> Ecom_Pedido_ = new Ecom_Pedido(Ecom_DBConnection_).GetCotizacion();
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Pedido_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new Ecom_SAP().GetBussPartByEmp(USR_IdSplinnet);

                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Pedido> Ecom_Pedido_ = new List<Ecom_Pedido>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new Ecom_Pedido(Ecom_DBConnection_).GetCotizacion(bp.CardCode).ForEach(cli =>
                        {
                            Ecom_Pedido_.Add(cli);
                        });
                    });
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Pedido_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }

        [AccessMultipleView(IdAction = new int[] { 15, 16 })]
        public ActionResult EnProceso()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
       
                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 16);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 15);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetInProcess();
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new Ecom_SAP().GetBussPartByEmp(USR_IdSplinnet);

                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new List<SAPDataProcess.SAP_Document>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetInProcess(bp.CardCode).ForEach(cli =>
                        {
                            SAP_Document_.Add(cli);
                        });
                    });
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }

        [AccessMultipleView(IdAction = new int[] { 20, 21 })]
        public ActionResult Historico()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();

                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 21);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 20);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetHistoric();
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new Ecom_SAP().GetBussPartByEmp(USR_IdSplinnet);

                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new List<SAPDataProcess.SAP_Document>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetHistoric(bp.CardCode).ForEach(cli =>
                        {
                            SAP_Document_.Add(cli);
                        });
                    });
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }

        [AccessMultipleView(IdAction = new int[] { 13, 14 })]
        public ActionResult Rechazado()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();

                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 14);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 13);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetRejected();
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new Ecom_SAP().GetBussPartByEmp(USR_IdSplinnet);

                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_Document> SAP_Document_ = new List<SAPDataProcess.SAP_Document>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new SAPDataProcess.SAP_Document(SAP_DBConnection_).GetRejected(bp.CardCode).ForEach(cli =>
                        {
                            SAP_Document_.Add(cli);
                        });
                    });
                    SAP_DBConnection_.CloseDataBaseAccess();
                    return View(SAP_Document_);
                }
                else
                {
                    return RedirectToAction("Error", "ErrorPages", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }
    }
}