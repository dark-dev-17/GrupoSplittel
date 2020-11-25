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
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();
        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
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
                var Respuestas = Ecom_RespuestaPregunta_.Get(IdBlog);
                Respuestas.ForEach(a => a.RutaArchivo = a.RutaArchivo != "" ? string.Format(@"{0}/fibra-optica/public/images/img_spl/consultecnico/{1}", Ecommerce_Domain, a.RutaArchivo) : "");
                return Ok(Respuestas);
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
        public ActionResult DataCreate(Ecom_RespuestaPregunta Ecom_RespuestaPregunta_)
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
                Ecom_RespuestaPregunta_.Actualizado = DateTime.Now;
                Ecom_RespuestaPregunta_.Creado = DateTime.Now;
                Ecom_RespuestaPregunta_.IdConsultor = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");

                Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_RespuestaPregunta_, ObjectSource.Ecom_RespuestaPregunta);

                if (Ecom_RespuestaPregunta_.Adjunto != null)
                {
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    List<string> Aceptados = new List<string> { "PDF", "pdf", "rar", "RAR" };
                    if (!Aceptados.Contains(Ecom_RespuestaPregunta_.Adjunto.ContentType.Split("/")[1]))
                    {
                        throw new Ecom_Exception("El archivo adjunto no es valido, valores aceptados(pdf, rar)");
                    }
                    string FilePaths = string.Format(@"public_html/fibra-optica/public/images/img_spl/consultecnico/{0}_{1}", Ecom_RespuestaPregunta_.IdPregunta, Ecom_RespuestaPregunta_.Adjunto.FileName);
                    Ecom_FilesFtp.UpdateFile(FilePaths, Ecom_RespuestaPregunta_.Adjunto);
                    Ecom_RespuestaPregunta_.RutaArchivo = Ecom_RespuestaPregunta_.IdPregunta+"_" +Ecom_RespuestaPregunta_.Adjunto.FileName;
                }

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
        public ActionResult DataUpdate(Ecom_RespuestaPregunta Ecom_RespuestaPregunta_)
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

                var respuesta = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_RespuestaPregunta);
                respuesta = respuesta.GetByid(Ecom_RespuestaPregunta_.IdRespuesta);
                if(Ecom_RespuestaPregunta_.Adjunto != null)
                {
                    List<string> Aceptados = new List<string> { "PDF", "pdf", "rar", "RAR" };
                    if (!Aceptados.Contains(Ecom_RespuestaPregunta_.Adjunto.ContentType.Split("/")[1]))
                    {
                        throw new Ecom_Exception("El archivo adjunto no es valido, valores aceptados(pdf, rar)");
                    }
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    if (respuesta.RutaArchivo != "")
                    {
                        string FilePath = string.Format(@"public_html/fibra-optica/public/images/img_spl/consultecnico/{0}", respuesta.RutaArchivo);
                        if (Ecom_FilesFtp.ExistsFile(FilePath))
                        {
                            Ecom_FilesFtp.DeleteFile(FilePath);
                        }
                        
                    }

                    string FilePaths = string.Format(@"public_html/fibra-optica/public/images/img_spl/consultecnico/{0}_{1}_{2}", Ecom_RespuestaPregunta_.IdPregunta, Ecom_RespuestaPregunta_.IdRespuesta, Ecom_RespuestaPregunta_.Adjunto.FileName);
                    if (Ecom_FilesFtp.ExistsFile(FilePaths))
                    {
                        Ecom_FilesFtp.DeleteFile(FilePaths);
                    }
                    Ecom_FilesFtp.UpdateFile(FilePaths, Ecom_RespuestaPregunta_.Adjunto);

                    Ecom_RespuestaPregunta_.RutaArchivo = string.Format(@"{0}_{1}_{2}", Ecom_RespuestaPregunta_.IdPregunta, Ecom_RespuestaPregunta_.IdRespuesta, Ecom_RespuestaPregunta_.Adjunto.FileName);
                }

                Ecom_RespuestaPregunta_ = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.SetObjectConnection(Ecom_RespuestaPregunta_, ObjectSource.Ecom_RespuestaPregunta);
                Ecom_RespuestaPregunta_.Actualizado = DateTime.Now;
                Ecom_RespuestaPregunta_.Creado = DateTime.Now;
                Ecom_RespuestaPregunta_.IdConsultor = respuesta.IdConsultor;
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

                var respuesta = (Ecom_RespuestaPregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_RespuestaPregunta);
                respuesta = respuesta.GetByid(Ecom_RespuestaPregunta_.IdRespuesta);

                if(respuesta.RutaArchivo != "")
                {
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    string FilePath = string.Format(@"public_html/fibra-optica/public/images/img_spl/consultecnico/{0}", respuesta.RutaArchivo);
                    if (Ecom_FilesFtp.ExistsFile(FilePath))
                    {
                        Ecom_FilesFtp.DeleteFile(FilePath);
                    }
                }
                

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
            //try
            //{
            //    Ecommerce_ = new Ecommerce(HttpContext.Session);
            //    Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
            //    Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
            //    Ecommerce_.ecomData.Connect(ServerSource.Splitnet);
            //    Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
            //    Ecom_Usuario Ecom_Usuario = (Ecom_Usuario)Ecommerce_.ecomData.GetObject(ObjectSource.Usuario);
            //    EcomDataProccess.Foro.Ecom_ConsultConsult Ecom_ConsultConsult = (EcomDataProccess.Foro.Ecom_ConsultConsult)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_ConsultConsult);
            //    var list = Ecom_Pregunta.Get();

            //    ViewData["Consultores"] = Ecom_Usuario.GetConsultor();

            //    return View(list.OrderByDescending(a => a.Creado));
            //}
            //catch (Ecom_Exception ex)
            //{
            //    return RedirectToAction("Error", "ErrorPages", new { id = ex.Message });
            //}
            //finally
            //{
            //    if (Ecommerce_ != null)
            //    {
            //        Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
            //        Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
            //    }
            //}
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);

                List<Ecom_Pregunta> list = new List<Ecom_Pregunta>();

                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");

                EcomDataProccess.Foro.Ecom_ConsultConsult Ecom_ConsultConsult = (EcomDataProccess.Foro.Ecom_ConsultConsult)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_ConsultConsult);
                Ecom_ConsultConsult.getByConsultorr(USR_IdSplinnet).ForEach(a =>
                {
                    Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                    list.Add(Ecom_Pregunta.Get(a));
                });
                if (list.Count > 0)
                {
                    return View(list.OrderByDescending(a => a.Creado));
                }
                else
                {
                    return View(list);
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

        [AccessView(IdAction = 57)]
        public ActionResult Asignar()
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                EcomDataProccess.Foro.Ecom_InternosUser Ecom_Usuario = (EcomDataProccess.Foro.Ecom_InternosUser)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_InternosUser);
                EcomDataProccess.Foro.Ecom_ConsultConsult Ecom_ConsultConsult = (EcomDataProccess.Foro.Ecom_ConsultConsult)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_ConsultConsult);
                var list = Ecom_Pregunta.Get();
                list.ForEach(a => {
                    a.NumberConsut = Ecom_ConsultConsult.GetConsultores(a.IdPregunta).Count;
                });
                ViewData["Consultores"] = Ecom_Usuario.GetConsultores();

                return View(list.OrderByDescending(a => a.Creado));
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
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                }
            }
        }
        [AccessData(IdAction = 56)]
        [HttpPost]
        public ActionResult GetConsultores(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                EcomDataProccess.Foro.Ecom_ConsultConsult Ecom_ConsultConsult = (EcomDataProccess.Foro.Ecom_ConsultConsult)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_ConsultConsult);
                return Ok(Ecom_ConsultConsult.GetConsultores(id));
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecommerce_ != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }

        [AccessData(IdAction = 56)]
        [HttpPost]
        public ActionResult AddConsultores([FromBody]ConsultorConsultorEco consultorConsultorEco)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                EcomDataProccess.Foro.Ecom_ConsultConsult Ecom_ConsultConsult = (EcomDataProccess.Foro.Ecom_ConsultConsult)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_ConsultConsult);
                Ecom_ConsultConsult.Agregar(consultorConsultorEco.IdConsultores, consultorConsultorEco.IdPregunta);

                Ecom_Pregunta Ecom_Pregunta = (Ecom_Pregunta)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_Pregunta);
                var result = Ecom_Pregunta.Get(consultorConsultorEco.IdPregunta);

                consultorConsultorEco.IdConsultores.ForEach(a =>
                {

                    EcomDataProccess.Foro.Ecom_InternosUser Ecom_Usuario = (EcomDataProccess.Foro.Ecom_InternosUser)Ecommerce_.ecomData.GetObject(ObjectSource.Ecom_InternosUser);
                    var resulta = Ecom_Usuario.GetConsultore(a);
                    Ecommerce_.ecomData.Ecom_Email_ = new Ecom_Email(SMTP_Server, SMTP_account, Int32.Parse(SMTP_Port), SMTP_user, SMTP_pass, (SMTP_ssl == "true" ? true : false));
                    
                    string htmls = string.Format("" +
                        "<p align='left'>Estimado: <strong>{0}</strong></p>" +
                        " Usted ha sido asignado como consultor dentro de nuestro E-commerce <br>" +
                        " <br>" +
                        " <strong>Pregunta: </strong> {1} <br>" +
                        " <strong>Categoria: </strong> {2} " +
                        " <br>" +
                        " Por favor ingresa a <a href='192.168.2.29:2622'>Administrador E-commerce</a> en la seccion <strong>Consultor Técnico</strong> <br>" +
                        " <br>" +
                        " <br>" +
                        " Gracias!", resulta.NombreCompleto, result.Pregunta, result.CategoriaNombre);

                    Ecommerce_.ecomData.SendMailNotification(4, htmls, resulta.Correo);
                });
               



                return Ok("Información guardada");
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
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
                    ViewData["IdUsuario"] = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
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