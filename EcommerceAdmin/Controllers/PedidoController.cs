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
            {   if(id != 0)
                {
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    Ecom_Pedido Ecom_Pedido_ = new Ecom_Pedido(Ecom_DBConnection_);
                    bool result = Ecom_Pedido_.GetById(id);
                    Ecom_DBConnection_.CloseConnection();
                    if (result)
                    {
                        if (Ecom_Pedido_.StatusProcessWS == 0)
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
                        return View(Ecom_Pedido_);
                    }
                    else
                    {
                        return View("../ErrorPages/Error", new { id = string.Format("El pedido '{0}' no fue encontrado", id) });
                    }
                }
                else
                {
                    return View(new Ecom_Pedido { DocNumEcommerce = 0});
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
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
                    return View("../ErrorPages/Error", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
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
                    return View("../ErrorPages/Error", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
                    return View(SAP_Document_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
                    return View(SAP_Document_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
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
                    Ecom_SAP ecom_SAP = new Ecom_SAP();
                    ecom_SAP.DocumetsSAP_GetCustomerEcom(SAP_Document_);
                    return View(SAP_Document_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "Error en la configuración de permisos de usuario" });
                }
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
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

        [AccessViewSession]
        public ActionResult Seguimiento()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataTotalDocument(DateTime start, DateTime end, string Currency, string ModeBussiness)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                double Tota = new Ecom_Pedido(Ecom_DBConnection_).GetTotal(start, end, Currency, ModeBussiness);
                Ecom_DBConnection_.CloseConnection();
                return Ok(Tota);
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
        [AccessDataSession]
        public ActionResult DataNoDocument(DateTime start, DateTime end, string Currency, string ModeBussiness, string TypeDoc)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                int Tota = new Ecom_Pedido(Ecom_DBConnection_).GetNoDoumentos(start, end, ModeBussiness, TypeDoc);
                Ecom_DBConnection_.CloseConnection();
                return Ok(Tota);
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
        public IActionResult DataGetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            List<Ecom_ProductoCategoria> result;
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                result = new Ecom_ProductoCategoria(Ecom_DBConnection_).GetQuoatationsDashboard(start, end, ModeBussiness, tipoDocumento);
                Ecom_DBConnection_.CloseConnection();
                return Ok(result);
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
        [AccessDataSession]
        public IActionResult DataGetPendientes()
        {
            Ecommerce Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.SAPBussinessOne);
                Ecommerce_.ecomData.Connect(ServerSource.Splitnet);
                bool AccessGeneral = Ecommerce_.ValidActionUser(23);
                bool AccessSpecial = Ecommerce_.ValidActionUser(24);
                if (AccessGeneral && !AccessSpecial)
                {
                    //documentos generales
                    Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_Pedido ecom_Pedido = (Ecom_Pedido)Ecommerce_.ecomData.GetObject(ObjectSource.Pedido);
                    return Ok(ecom_Pedido.GetPending());
                }
                else if (!AccessGeneral && AccessSpecial)
                {
                    Ecommerce_.sAPData.OpenConnection(SAPDataProcess.ConnectionSAP.Database);
                    SAPDataProcess.SAP_BussinessPartner sAP_BussinessPartner = (SAPDataProcess.SAP_BussinessPartner)Ecommerce_.sAPData.GetObject(SAPDataProcess.SAPDataBaseObj.BussinesPartner);

                    Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_Pedido ecom_Pedido = (Ecom_Pedido)Ecommerce_.ecomData.GetObject(ObjectSource.Pedido);
                    List<Ecom_Pedido> ecom_Pedidos = new List<Ecom_Pedido>();
                    Ecommerce_.GetBussinessPartnerByUser().Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        ecom_Pedido.GetPending(bp.CardCode).ForEach(cli =>
                        {
                            ecom_Pedidos.Add(cli);
                        });
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
            catch(SAPDataProcess.SAP_Excepcion ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if(Ecommerce_.ecomData != null)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public IActionResult ListInProcesss()
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
                    ecom_Pedidos.ForEach(doc => {
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
                    Ecommerce_.GetBussinessPartnerByUser().Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        sAP_Document.GetInProcess(bp.CardCode).ForEach(cli =>
                        {
                            ecom_Pedidos.Add(cli);
                        });
                    });
                    ecom_Pedidos.ForEach(doc => {
                        Ecom_Pedido ecom_Pedido = (Ecom_Pedido)Ecommerce_.ecomData.GetObject(ObjectSource.Pedido);
                        ecom_Pedido.GetById(Int32.Parse(doc.DocNumEcommerce));
                        doc.ObjetoAux = ecom_Pedido;
                    });
                    return Ok("");
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
        public IActionResult DataTest()
        {
            return Ok("Metodo eontraddo");
        }
    }
}