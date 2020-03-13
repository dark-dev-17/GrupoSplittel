using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAdmin.Models;
using System.Configuration;
using EcomDataProccess;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;

namespace EcommerceAdmin.Controllers
{
    public class ClienteController : Controller
    {
        private string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();

        [AccessMultipleView(IdAction = new int[] { 9,10 })]
        public IActionResult Index()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessGeneral = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 10);
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, 9);
                Ecom_DBConnection_.CloseConnection();
                //validar acceso general
                if (AccessGeneral && !AccessBysalesEmp)
                {
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Cliente> Ecom_Cliente_ = new Ecom_Cliente(Ecom_DBConnection_).Get();
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Cliente_);
                }
                else if (!AccessGeneral && AccessBysalesEmp)
                {
                    // obtener id de sap de empleado
                    Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                    Ecom_DBConnection_.OpenConnection();
                    Ecom_Usuario Ecom_Usuario_ = new Ecom_Usuario(Ecom_DBConnection_);
                    bool IsExists = Ecom_Usuario_.Get(USR_IdSplinnet);
                    Ecom_Usuario_.GetIdSap();
                    Ecom_DBConnection_.CloseConnection();

                    // obtener clientes de sap
                    SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                    SAP_DBConnection_.OpenConnection();
                    List<SAPDataProcess.SAP_BussinessPartner> SAP_BussinessPartner_ = new SAPDataProcess.SAP_BussinessPartner(SAP_DBConnection_).GetActivesBySalesEm(Ecom_Usuario_.Id_sap);
                    SAP_DBConnection_.CloseDataBaseAccess();

                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    List<Ecom_Cliente> Ecom_Cliente_ = new List<Ecom_Cliente>();

                    SAP_BussinessPartner_.Where(bp => bp.IsActiveEcomerce && bp.IsActive).ToList().ForEach(bp => {
                        new Ecom_Cliente(Ecom_DBConnection_).Get(bp.CardCode).ForEach(cli =>
                        {
                            Ecom_Cliente_.Add(cli);
                        });
                    });
                    Ecom_DBConnection_.CloseConnection();
                    return View(Ecom_Cliente_);
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
        [AccessView(IdAction =18)]
        public IActionResult BussinessPartner(string id)
        {
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
                SAPDataProcess.SAP_BussinessPartner SAP_BussinessPartner_ = new SAPDataProcess.SAP_BussinessPartner(SAP_DBConnection_);
                SAP_BussinessPartner_.Get(id);
                SAP_DBConnection_.CloseDataBaseAccess();
                return View(SAP_BussinessPartner_);
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
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataTotal(string ModeBussiness, DateTime start, DateTime end)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                int Tota = new Ecom_Cliente(Ecom_DBConnection_).GetTotal(ModeBussiness, start, end);
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
            List<Ecom_Cliente> result;
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                result = new Ecom_Cliente(Ecom_DBConnection_).GetQuoatationsDashboard(start, end, ModeBussiness, tipoDocumento);
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
    }
}
