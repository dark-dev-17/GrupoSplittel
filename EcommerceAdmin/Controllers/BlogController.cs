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
    public class BlogController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();
        // GET: Blog
        [AccessView(IdAction = 36)]
        public ActionResult Index()
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                List<Ecom_Blog> result = Ecom_Blog_.Get();
                return View(result);
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

        // GET: Blog/Details/5
        [AccessView(IdAction = 36)]
        public ActionResult Details(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                bool result = Ecom_Blog_.Get(id);
                if (result)
                {
                    return View(Ecom_Blog_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
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

        // GET: Blog/Create
        [AccessView(IdAction = 37)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 37)]
        public ActionResult Create(Ecom_Blog Ecom_Blog_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_Blog_);
                }
                else
                {
                    if (Ecom_Blog_.BlogImage == null || Ecom_Blog_.BlogCover == null)
                    {
                        if(Ecom_Blog_.BlogImage == null)
                            ModelState.AddModelError("BlogImage", "Please choose a file");
                        if (Ecom_Blog_.BlogCover == null)
                            ModelState.AddModelError("BlogCover", "Please choose a file");
                        return View(Ecom_Blog_);
                    }
                    ecomData.Connect(ServerSource.Ecommerce);
                    ecomData.Connect(ServerSource.Splitnet);
                    bool AdminPermiss = ecomData.validPermissAction(USR_IdSplinnet, 37);
                    Ecom_Blog_ = (Ecom_Blog)ecomData.SetObjectConnection(Ecom_Blog_, ObjectSource.Blog);
                    bool result = false;
                    if (AdminPermiss)
                    {
                        Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                        var FilenameBlogImage = Ecom_Blog_.BlogImage.FileName;
                        var FilenameBlogCover = Ecom_Blog_.BlogCover.FileName;
                        if (FilenameBlogImage == FilenameBlogCover)
                        {
                            throw new Ecom_Exception("Los archivos no pueden tener el mismo nombre");
                        }
                        string PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/{0}", FilenameBlogImage);
                        Ecom_FilesFtp.UpdateFile(PathItem, Ecom_Blog_.BlogImage);

                        PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/{0}", FilenameBlogCover);
                        Ecom_FilesFtp.UpdateFile(PathItem, Ecom_Blog_.BlogCover);

                        Ecom_Blog_.ImageCoverPage = FilenameBlogCover;
                        Ecom_Blog_.ImageBlog = FilenameBlogImage;

                        result = Ecom_Blog_.Add();
                    }
                    else
                    {
                        throw new Ecom_Exception("Error en la configuración de permisos para esta sección, Contacta al departamento De TI");
                    }
                    if (result)
                    {
                        ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha creado un nuevo blog", "Blog", "Details", "" + Ecom_Blog_.GetLastId(), "");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_Blog_);
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

        // GET: Blog/Edit/5
        [AccessView(IdAction = 37)]
        public ActionResult Edit(int id)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                bool result = Ecom_Blog_.Get(id);
                if (result)
                {
                    return View(Ecom_Blog_);
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
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

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 37)]
        public ActionResult Edit(Ecom_Blog Ecom_Blog_)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Ecom_Blog_);
                }
                else
                {
                    ecomData.Connect(ServerSource.Ecommerce);
                    ecomData.Connect(ServerSource.Splitnet);
                    bool AdminPermiss = ecomData.validPermissAction(USR_IdSplinnet, 37);
                    Ecom_Blog_ = (Ecom_Blog)ecomData.SetObjectConnection(Ecom_Blog_, ObjectSource.Blog);
                    bool result = false;
                    if (AdminPermiss)
                    {
                        result = Ecom_Blog_.Update(2);
                    }
                    else
                    {
                        throw new Ecom_Exception("Error en la configuración de permisos para esta sección, Contacta al departamento De TI");
                    }
                    if (result)
                    {
                        ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha editado el blog " + Ecom_Blog_.Title.Substring(0,15), "Blog", "Detalle", "" + Ecom_Blog_.Id, "");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "warning", ex.Message, "", "", "", ex.StackTrace);
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                return View(Ecom_Blog_);
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
        [AccessData(IdAction = 37)]
        public ActionResult DataDeleteFiles(int id, string NameFile, string TypeFile)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                Ecom_Tools.ValidStringParameter(NameFile, "Nombre nuevo del archivo");
                Ecom_Tools.ValidStringParameter(TypeFile, "Tipo del archivo");

                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                if (Ecom_Blog_.Get(id))
                {
                    // borrar archivo
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    string PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/{0}", NameFile);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    UpdateImages(id, "", TypeFile);
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha eliminado la imagen "+ TypeFile + " del blog " + Ecom_Blog_.Title.Substring(0, 15), "Blog", "Details", "" + Ecom_Blog_.Id, "");
                    return Ok("Imagen eliminada");
                }
                else
                {
                    return BadRequest("No existe el blog seleccionado");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataUploadFiles(IFormFile FormFile, int id, string TypeFile)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                if (Ecom_Blog_.Get(id))
                {
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    var Filename = FormFile.FileName;
                    string PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/*.jpg");
                    string PathPublicItem = string.Format(@"{0}/store/public/images/img_spl/blog/", Ecommerce_Domain);

                    PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/{0}", Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    UpdateImages(id, Filename, TypeFile);
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha cargado nueva imagen " + TypeFile + " del blog " + Ecom_Blog_.Title.Substring(0, 15), "Blog", "Details", "" + Ecom_Blog_.Id, "");
                    return Ok("Imagen  cargada");
                }
                else
                {
                    return BadRequest("No existe el blog seleccionado");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 37)]
        public ActionResult DataRenameFiles(string TypeFile, string Filename, int id, string Newname)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                if (Ecom_Blog_.Get(id))
                {
                    Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                    string PathItem = string.Format(@"public_html/store/public/images/img_spl/blog/");
                    Ecom_FilesFtp.Rename(PathItem, Filename, Newname + ".jpg");
                    UpdateImages(id, Newname + ".jpg", TypeFile);
                    ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha renombrado la imagen " + TypeFile + " del blog " + Ecom_Blog_.Title.Substring(0, 15), "Blog", "Details", "" + Ecom_Blog_.Id, "");
                    return Ok("Archivo renombrado");
                }
                else
                {
                    return BadRequest("No existe el blog seleccionado");
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
        private void UpdateImages(int id, string NameFile, string TypeFile)
        {
            EcomData ecomData = new EcomData(EcomConnection, SplitConnection);
            try
            {
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_Blog Ecom_Blog_ = (Ecom_Blog)ecomData.GetObject(ObjectSource.Blog);
                if (Ecom_Blog_.Get(id))
                {
                    if (TypeFile.Trim() == "Caratula")
                    {
                        Ecom_Blog_.ImageCoverPage = NameFile;
                    }
                    else if (TypeFile.Trim() == "Blog")
                    {
                        Ecom_Blog_.ImageBlog = NameFile;
                    }
                    else
                    {
                        throw new Ecom_Exception("Tipo de imagen no valida");
                    }
                    bool result = Ecom_Blog_.Update(3);
                    if (!result)
                    {
                        throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
                else
                {
                    throw new Ecom_Exception(ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
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

    }
}