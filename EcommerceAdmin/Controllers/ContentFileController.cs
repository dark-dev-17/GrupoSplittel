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
    public class ContentFileController : Controller
    {
        private Ecommerce Ecommerce_;
        public ContentFileController()
        {
            Ecommerce_ = new Ecommerce();
        }

        // GET: BlogComentario/Create
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Index(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                return View(Ecom_ContentFile_.GetContent(id));
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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
        // GET: BlogComentario/Edit
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Edit(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                if (Ecom_ContentFile_.Get(id))
                {
                    return View(Ecom_ContentFile_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "no se encontró el registro" });
                }
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Create(int id)
        {
            Ecom_ContentFile Ecom_ContentFile_ = new Ecom_ContentFile();
            Ecom_ContentFile_.IdTipoContenido = id;
            return View(Ecom_ContentFile_);
        }
        // GET: BlogComentario/Details
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Details(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                if (Ecom_ContentFile_.Get(id))
                {
                    return View(Ecom_ContentFile_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "no se encontró el registro" });
                }
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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
        [AccessMultipleView(IdAction = new int[] { 55 })]
        public ActionResult Delete(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                if (Ecom_ContentFile_.Get(id))
                {
                    return View(Ecom_ContentFile_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = "no se encontró el registro" });
                }
                
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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
        [AccessMultipleView(IdAction = new int[] { 55 })]
        [HttpPost]
        public ActionResult Edit(Ecom_ContentFile Ecom_ContentFile_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ContentFile_);
                }
                
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                //Ecom_ContentFile Ecom_ContentFile_Copy = Ecom_ContentFile_;

                //Ecom_ContentFile_Copy = (Ecom_ContentFile)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFile_, ObjectSource.ContentFile);
                Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFile_, ObjectSource.ContentFile);

                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);

                if (Ecom_ContentFileType_.Get(Ecom_ContentFile_.IdTipoContenido))
                {
                    if (string.IsNullOrEmpty(Ecom_ContentFileType_.RuteEcommerce))
                    {
                        ModelState.AddModelError(string.Empty, "El conjunto de archivos seleccionados no tiene un path definido");
                        return View(Ecom_ContentFile_);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se encontró el conjunto de archivos");
                    return View(Ecom_ContentFile_);
                }

                Ecom_ContentFile Ecom_ContentFile_Copy = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject( ObjectSource.ContentFile);
                if(!Ecom_ContentFile_Copy.Get(Ecom_ContentFile_.Id))
                {
                    ModelState.AddModelError(string.Empty, "No se encontró el archivo");
                    return View(Ecom_ContentFile_);
                }

                if (Ecom_ContentFile_.Imagen == null)
                {
                    Ecom_ContentFile_.PathFile = Ecom_ContentFile_Copy.PathFile;
                }
                else
                {
                    Ecom_ContentFile_.PathFile = Ecom_ContentFile_Copy.PathFile;
                    if (!string.IsNullOrEmpty(Ecom_ContentFile_.PathFile))
                    {
                        string Small_ = string.Format(@"public_html/{0}", Ecom_ContentFile_.PathFile);
                        if (Ecommerce_.ecomData.Ecom_FilesFtp.ExistsFile(Small_))
                        {
                            Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(Small_);
                        }
                    }
                    string ImgSmall_ = string.Format(@"public_html/{0}img_{1}.{2}", Ecom_ContentFileType_.RuteEcommerce, Ecom_ContentFile_.Id, Ecom_ContentFile_.Imagen.FileName.Split('.')[1]);
                    Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgSmall_, Ecom_ContentFile_.Imagen);
                    Ecom_ContentFile_.PathFile = string.Format(@"{0}img_{1}.{2}", Ecom_ContentFileType_.RuteEcommerce, Ecom_ContentFile_.Id, Ecom_ContentFile_.Imagen.FileName.Split('.')[1]);
                }
                if (Ecom_ContentFile_.Edit())
                {
                    return RedirectToAction("Files", "ContentFileType", new { id = Ecom_ContentFile_Copy.IdTipoContenido });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFile_);
                }

                
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFile_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFile_);
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
        [AccessMultipleView(IdAction = new int[] { 55 })]
        [HttpPost]
        public ActionResult Create(Ecom_ContentFile Ecom_ContentFile_)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_ContentFile_);
                }
                if (Ecom_ContentFile_.Imagen == null)
                {
                    ModelState.AddModelError("Imagen", "Please choose a file");
                    return View(Ecom_ContentFile_);
                }
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFile_, ObjectSource.ContentFile);

                Ecom_ContentFileType Ecom_ContentFileType_ = (Ecom_ContentFileType)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFileType);

                if (!Ecom_ContentFileType_.Get(Ecom_ContentFile_.IdTipoContenido))
                {
                    ModelState.AddModelError(string.Empty, "No se encontró el conjunto de archivos");
                    return View(Ecom_ContentFile_);
                }

                string ImgSmall_ = string.Format(@"public_html/{0}img_{1}.{2}", Ecom_ContentFileType_.RuteEcommerce, Ecom_ContentFile_.LastId() + 1 , Ecom_ContentFile_.Imagen.FileName.Split('.')[1]);
                Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgSmall_, Ecom_ContentFile_.Imagen);

                Ecom_ContentFile_.PathFile = string.Format(@"{0}img_{1}.{2}", Ecom_ContentFileType_.RuteEcommerce, Ecom_ContentFile_.LastId() + 1, Ecom_ContentFile_.Imagen.FileName.Split('.')[1]);

                if (Ecom_ContentFile_.Add())
                {
                    return RedirectToAction("Files", "ContentFileType", new { id = Ecom_ContentFile_.IdTipoContenido });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFile_);
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFile_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_ContentFile_);
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
        [AccessMultipleView(IdAction = new int[] { 55 })]
        [HttpPost]
        public ActionResult Delete(Ecom_ContentFile Ecom_ContentFile_)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.SetObjectConnection(Ecom_ContentFile_,ObjectSource.ContentFile);
                if (Ecom_ContentFile_.Get(Ecom_ContentFile_.Id))
                {
                    string Small_ = string.Format(@"public_html/{0}", Ecom_ContentFile_.PathFile);
                    Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(Small_);
                    if (Ecom_ContentFile_.Delete())
                    {
                        return RedirectToAction("Files", "ContentFileType", new { id = Ecom_ContentFile_.IdTipoContenido });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                        return View(Ecom_ContentFile_);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                    return View(Ecom_ContentFile_);
                }
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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

        [AccessDataSession]
        [HttpPost]
        public ActionResult Order(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                return View(Ecom_ContentFile_.GetContent(id));
            }
            catch (Ecom_Exception ex)
            {
                return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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
        [AccessData(IdAction = 55)]
        [HttpPost]
        public ActionResult DataUpdatePositions([FromBody]List<Ecom_ContentFile> Ecom_ContentFile_)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                //Ecom_ContentFile Ecom_ContentFile_ = (Ecom_ContentFile)Ecommerce_.ecomData.GetObject(ObjectSource.ContentFile);
                //return Ok(Ecom_ContentFile_.GetContent(id));
                if (Ecom_ContentFile_.Count > 0)
                {
                    Ecom_ContentFile_.ForEach(content => {
                        content = (Ecom_ContentFile)Ecommerce_.ecomData.SetObjectConnection(content,ObjectSource.ContentFile);
                        if (content.UpdatePosition())
                        {

                        }
                        else
                        {

                        }
                    });
                    return Ok("Datos guardados");
                }
                else
                {
                    return BadRequest("sin elementos que ordenar");
                }
            }
            catch (Ecom_Exception ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                 return View("../ErrorPages/Error", new { id = ex.Message });
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