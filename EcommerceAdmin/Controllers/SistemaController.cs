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
    public class SistemaController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        // GET: Sistema
        [AccessView(IdAction = 25)]
        public ActionResult Acceso()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_Usuario> Ecom_Usuario_ = new Ecom_Usuario(Ecom_DBConnection_).Get();
                Ecom_DBConnection_.CloseConnection();
                return View(Ecom_Usuario_);
            }
            catch (Ecom_Exception ex)
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
        [AccessView(IdAction = 44)]
        public ActionResult ProcesoEmail()
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProcesoEmail Ecom_ProcesoEmail_ = (Ecom_ProcesoEmail)ecomData.GetObject(ObjectSource.ProcesoEmail);
                return View(Ecom_ProcesoEmail_.Get());
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
        [AccessView(IdAction = 44)]
        public ActionResult DataGetProcesoEmail(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProcesoEmail Ecom_ProcesoEmail_ = (Ecom_ProcesoEmail)ecomData.GetObject(ObjectSource.ProcesoEmail);
                if (Ecom_ProcesoEmail_.Get(id))
                {
                    return Ok(Ecom_ProcesoEmail_);
                }
                else
                {
                    return BadRequest("Proceso no encontrado");
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
        [AccessView(IdAction = 44)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataUpdateProcesoEmail([FromBody]Ecom_ProcesoEmail Ecom_ProcesoEmail_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProcesoEmail_ = (Ecom_ProcesoEmail)ecomData.SetObjectConnection(Ecom_ProcesoEmail_,ObjectSource.ProcesoEmail);
                if (Ecom_ProcesoEmail_.Update(1))
                {
                    return Ok(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest("Proceso no encontrado");
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
        [AccessViewSession]
        public ActionResult InfoNotification(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Notificacion Ecom_Notificacion_ = (Ecom_Notificacion)ecomData.GetObject(ObjectSource.Notificacion);
                if (Ecom_Notificacion_.Get(id))
                {
                    Ecom_Notificacion_.Update(2);
                    return RedirectToAction(Ecom_Notificacion_.Action, Ecom_Notificacion_.Controller, new { id = Ecom_Notificacion_.Parameter });
                }
                else
                {
                    throw new Ecom_Exception("No se contro la notificación");
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
                    ecomData.Disconect(ServerSource.Splitnet);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataGetPermissByUser(int id)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_Modelo> Ecom_Modelo_ = new Ecom_Modelo(Ecom_DBConnection_).Get();
                Ecom_Modelo_.ForEach(p1 => {
                    p1.IdUser = id;
                    p1.Acciones.ForEach(a => a.isAccess = new Ecom_Acciones(Ecom_DBConnection_).CheckPermissToUser(p1.IdUser, a.Id));
                });
                Ecom_DBConnection_.CloseConnection();
                return Ok(Ecom_Modelo_);
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
        public ActionResult DataGetPermiss()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_Modelo> Ecom_Modelo_ = new Ecom_Modelo(Ecom_DBConnection_).Get();
                Ecom_Modelo_.ForEach(p1 => {
                    p1.IdUser = 0;
                    p1.Acciones.ForEach(a => a.isAccess = false);
                });
                Ecom_DBConnection_.CloseConnection();
                return Ok(Ecom_Modelo_);
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
        public ActionResult DataChangePermissByUser([FromBody]List<Ecom_Modelo> Permissions)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                Permissions.ForEach(m => m.Acciones.ForEach(a => {
                    a.SetConnectionMYsql(Ecom_DBConnection_);
                    a.ChangePermissToUser(m.IdUser, a.Id, (a.isAccess ? 1 : 0));
                }));
                Ecom_DBConnection_.CloseConnection();
                return Ok("Cambios guardados");
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
        public ActionResult DataAccessByArea([FromBody]AccionesArea AccionesArea_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                if(AccionesArea_ == null)
                {
                    throw new Ecom_Exception("Por favor selecciona una area");
                }
                ecomData.Connect(ServerSource.Splitnet);
                Ecom_Tools.ValidIntParameter(AccionesArea_.IdArea, "Area");
                Ecom_Usuario Ecom_Usuario_ = (Ecom_Usuario)ecomData.GetObject(ObjectSource.Usuario);
                Ecom_Usuario_.GetByArea(AccionesArea_.IdArea).ForEach(usuario => {
                    AccionesArea_.Permissions.ForEach(m => m.Acciones.ForEach(a => {
                        a.SetConnectionMYsql(ecomData.GetConnection(ServerSource.Splitnet));
                        a.ChangePermissToUser(usuario.IdSplinnet, a.Id, (a.isAccess ? 1 : 0));
                    }));
                });
                
                return Ok("Cambios guardados");
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Splitnet);
                }
            }
        }
    }
}