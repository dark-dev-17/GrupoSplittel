using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAdmin.Models;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class BlogComentarioController : Controller
    {
        private Ecommerce Ecommerce_;
        public BlogComentarioController()
        {
            Ecommerce_ = new Ecommerce();
        }

        // GET: BlogComentario/Create
        [AccessData(IdAction = 36)]
        public ActionResult DataGet(int IdBlog)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_BlogComentario Ecom_BlogComentario_ = (Ecom_BlogComentario)Ecommerce_.ecomData.GetObject(ObjectSource.BlogComentario);
                return Ok(Ecom_BlogComentario_.GetByBlog(IdBlog));
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

        // POST: BlogComentario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataCreate([FromBody]Ecom_BlogComentario Ecom_BlogComentario_)
        {
            try
            { 
                if(Ecom_BlogComentario_ == null)
                {
                    throw new Ecom_Exception("Sin información");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_BlogComentario_ = (Ecom_BlogComentario)Ecommerce_.ecomData.SetObjectConnection(Ecom_BlogComentario_, ObjectSource.BlogComentario);
                if (Ecom_BlogComentario_.Add())
                {
                    Ecom_BlogComentario_.Get(Ecom_BlogComentario_.LastId());
                    return Ok(Ecom_BlogComentario_);
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataUpdate([FromBody]Ecom_BlogComentario Ecom_BlogComentario_)
        {
            try
            {
                if (Ecom_BlogComentario_ == null)
                {
                    throw new Ecom_Exception("Sin información");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_BlogComentario_ = (Ecom_BlogComentario)Ecommerce_.ecomData.SetObjectConnection(Ecom_BlogComentario_, ObjectSource.BlogComentario);
                if (Ecom_BlogComentario_.Update(Ecom_BlogComentarioActions.EditarAll))
                {
                    return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataDelete([FromBody]Ecom_BlogComentario Ecom_BlogComentario_)
        {
            try
            {
                if (Ecom_BlogComentario_ == null)
                {
                    throw new Ecom_Exception("Sin información");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_BlogComentario_ = (Ecom_BlogComentario)Ecommerce_.ecomData.SetObjectConnection(Ecom_BlogComentario_, ObjectSource.BlogComentario);
                if (Ecom_BlogComentario_.Update(Ecom_BlogComentarioActions.Eliminar))
                {
                    return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataUpdateEstatus([FromBody]Ecom_BlogComentario Ecom_BlogComentario_)
        {
            try
            {
                if (Ecom_BlogComentario_ == null)
                {
                    throw new Ecom_Exception("Sin información");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_BlogComentario_ = (Ecom_BlogComentario)Ecommerce_.ecomData.SetObjectConnection(Ecom_BlogComentario_, ObjectSource.BlogComentario);
                if (Ecom_BlogComentario_.Update(Ecom_BlogComentarioActions.EditarEstatus))
                {
                    return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
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