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
    public class ConsultorPreguntaController : Controller
    {
        private readonly string SMTP_Server = ConfigurationManager.AppSettings["SMTP_Server"].ToString();
        private readonly string SMTP_Port = ConfigurationManager.AppSettings["SMTP_Port"].ToString();
        private readonly string SMTP_ssl = ConfigurationManager.AppSettings["SMTP_ssl"].ToString();
        private readonly string SMTP_account = ConfigurationManager.AppSettings["SMTP_account"].ToString();
        private readonly string SMTP_user = ConfigurationManager.AppSettings["SMTP_user"].ToString();
        private readonly string SMTP_pass = ConfigurationManager.AppSettings["SMTP_pass"].ToString();
        private Ecommerce Ecommerce_;
        public ConsultorPreguntaController()
        {
            Ecommerce_ = new Ecommerce();
        }

        #region Respuestas
        [AccessData(IdAction = 56)]
        public ActionResult DataGet(int IdBlog)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_RespuestaPregunta Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_RespuestaPregunta);
                return Ok(Ecom_RespuestaPregunta_.Get(IdBlog));
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
        [AccessData(IdAction = 56)]
        public ActionResult DataCreate([FromBody]Ecom_RespuestaPregunta Ecom_RespuestaPregunta_)
        {
            try
            {
                if (Ecom_RespuestaPregunta_ == null)
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
                Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_RespuestaPregunta_, ObjectSource.Ecom_RespuestaPregunta);
                if (Ecom_RespuestaPregunta_.Actions(Ecom_RespuestaPreguntaActions.Agregar))
                {
                    Ecom_RespuestaPregunta_.Get(Ecom_RespuestaPregunta_.LastId());

                    Ecommerce_.ecomData.Ecom_Email_ = new Ecom_Email(SMTP_Server, SMTP_account, Int32.Parse(SMTP_Port), SMTP_user, SMTP_pass, (SMTP_ssl == "true" ? true : false));
                    Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                    var result = Ecom_Pregunta.Get(Ecom_RespuestaPregunta_.IdPregunta);
                    string htmls = string.Format("" +
                        "<p align='left'>Su pregunta ya ha sido respondida por un consultor</p>" +
                        " Para poder visualizar la  respuesta ingresa a: <a href='https://fibremex.com/fibra-optica/views/Consultecnico/index.php?Categoria={0}'> fibremex.com </a> ", result.IdCategoria);

                    Ecommerce_.ecomData.SendMailNotification(3, htmls, result.Correo);



                    return Ok(Ecom_RespuestaPregunta_);
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
        [AccessData(IdAction = 56)]
        public ActionResult DataUpdate([FromBody]Ecom_RespuestaPregunta Ecom_RespuestaPregunta_)
        {
            try
            {
                if (Ecom_RespuestaPregunta_ == null)
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
                Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_RespuestaPregunta_, ObjectSource.Ecom_RespuestaPregunta);
                if (Ecom_RespuestaPregunta_.Actions(Ecom_RespuestaPreguntaActions.Update))
                {
                    Ecom_RespuestaPregunta_.Get(Ecom_RespuestaPregunta_.LastId());
                    return Ok(Ecom_RespuestaPregunta_);
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
        [AccessData(IdAction = 56)]
        public ActionResult DataDelete([FromBody]Ecom_RespuestaPregunta Ecom_RespuestaPregunta_)
        {
            try
            {
                if (Ecom_RespuestaPregunta_ == null)
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
                Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_RespuestaPregunta_, ObjectSource.Ecom_RespuestaPregunta);
                if (Ecom_RespuestaPregunta_.Actions(Ecom_RespuestaPreguntaActions.Eliminar))
                {
                    Ecom_RespuestaPregunta_.Get(Ecom_RespuestaPregunta_.LastId());
                    return Ok(Ecom_RespuestaPregunta_);
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
        #endregion

        #region Pregunta
        // GET: ConsultorPregunta
        [AccessView(IdAction = 56)]
        public ActionResult Index()
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                var list = Ecom_Pregunta.Get();
                return View(list.OrderByDescending( a=> a.Creado));
            }
            catch (Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_ != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: ConsultorPregunta/Details/5
        [AccessView(IdAction = 56)]
        public ActionResult Responder(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                var result = Ecom_Pregunta.Get(id);
                if (result != null)
                {
                    if(result.Active == false)
                    {
                        return View("../ErrorPages/Error", new { id = string.Format("La pregunta fue eliminada", id) });
                    }
                    return View(result);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = string.Format("Pregunta no encontrada", id) });
                }
            }
            catch (Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_ != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        // GET: ConsultorPregunta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConsultorPregunta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ConsultorPregunta/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                var result = Ecom_Pregunta.Get(id);
                if (result != null)
                {
                    return View(Ecom_Pregunta);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = string.Format("Pregunta no encontrada", id) });
                }
            }
            catch (Ecom_Exception ex)
            {
                return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            }
            finally
            {
                if (Ecommerce_ != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 56)]
        public ActionResult DataDeletPregunta(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Pregunta Ecom_Pregunta_ = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                Ecom_Pregunta_ = Ecom_Pregunta_.Get(id);
                if (Ecom_Pregunta_ == null)
                {
                    throw new Ecom_Exception("Sin información");
                }


                Ecom_Pregunta_ = (Ecom_Pregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_Pregunta_, ObjectSource.Ecom_Pregunta);
                Ecom_Pregunta_.Active = false;
                if (Ecom_Pregunta_.Actions(Ecom_PreguntaActions.Eliminar))
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
        #endregion



    }
}