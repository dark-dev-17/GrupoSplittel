﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EcomDataProccess;
using EcommerceAdmin.Models.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class ProductoController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();

        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();
        private EcomData ecomData;
        // GET: Producto
        [AccessView(IdAction = 1)]
        public async Task<ActionResult> Index()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_Producto> Ecom_Producto_ = await new Ecom_Producto(Ecom_DBConnection_).Get();
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
        [AccessView(IdAction = 47)]
        public ActionResult Precio(string tabActive)
        {
            ViewData["tabActive"] = tabActive;
            return View();
        }
        [AccessView(IdAction = 1)]
        public async Task<ActionResult> FijoConfigurable()
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                List<Ecom_Producto> Ecom_Producto_ = await new Ecom_Producto(Ecom_DBConnection_).GetConf();
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
        // GET: Producto/Details/5
        [AccessView(IdAction = 1)]
        public async Task<ActionResult> Detalle(string id)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                //id = HttpUtility.UrlDecode(id);
                if(string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                {
                    Ecom_Producto Ecom_Producto_ = new Ecom_Producto();
                    return View(Ecom_Producto_);
                }
                else
                {
                    
                    Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBConnection_.OpenConnection();
                    Ecom_Producto Ecom_Producto_ = new Ecom_Producto(Ecom_DBConnection_);
                    bool result = await Ecom_Producto_.Get(id);
                    Ecom_DBConnection_.CloseConnection();
                    if (result)
                    {
                        return View(Ecom_Producto_);
                    }
                    else
                    {
                        return View("../ErrorPages/Error", new { id = string.Format("El producto con codigo: '{0}' no fue encontrado", id) });
                    }
                }
                
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 3)]
        public async Task<ActionResult> DataCambiarFichaTecnicaAsync(string ItemCode, string Codigoficha)
        {
            try
            {
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoFichaTecnica Ecom_ProductoFichaTecnica_ = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                Ecom_Producto Ecom_Producto_ = (Ecom_Producto)ecomData.GetObject(ObjectSource.ProductoFijo);
                if (await Ecom_Producto_.Get(ItemCode))
                {
                    Ecom_Producto_.InfoTecnica = Codigoficha;
                    if (Ecom_Producto_.Update(2))
                    {
                        ecomData.SaveNotification((int)HttpContext.Session.GetInt32("USR_IdSplinnet"), (int)HttpContext.Session.GetInt32("USR_IdArea"), "info", "Ha cambiado la ficha tecnica del producto: " + Ecom_Producto_.ItemCode, "Producto", "Detalle", "", Ecom_Producto_.ItemCode + "");
                        return Ok("Ficha técnica guardada");
                    }
                    else
                    {
                        return BadRequest(ecomData.GetLastMessage(ServerSource.Ecommerce));
                    }
                }
                else
                {
                    return BadRequest("Producto no encontrado");
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
        [AccessData(IdAction = 4)]
        public ActionResult DataChangeDescription(string ItemCode, string IdDescripcionLarga)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_Tools.ValidStringParameter(ItemCode, "ItemCode");
                Ecom_Tools.ValidStringParameter(IdDescripcionLarga, "IdDescripcionLarga");
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Producto Ecom_Producto_ = new Ecom_Producto(Ecom_DBConnection_);
                Ecom_Producto_.ItemCode = ItemCode;
                Ecom_Producto_.IdDescripcionLarga = IdDescripcionLarga;
                bool result = Ecom_Producto_.Update(1);
                Ecom_DBConnection_.CloseConnection();
                if (result)
                {
                    return Ok(Ecom_DBConnection_.Message);
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
        [AccessData(IdAction = 4)]
        public ActionResult DataChangeDescriptionShortp(string ItemCode, string Description)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_Tools.ValidStringParameter(ItemCode, "ItemCode");
                Ecom_Tools.ValidStringParameter(Description, "Description");
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Producto Ecom_Producto_ = new Ecom_Producto(Ecom_DBConnection_);
                Ecom_Producto_.ItemCode = ItemCode;
                Ecom_Producto_.Description = Description;
                bool result = Ecom_Producto_.Update(3);
                Ecom_DBConnection_.CloseConnection();
                if (result)
                {
                    return Ok(Ecom_DBConnection_.Message);
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
        [AccessData(IdAction = 4)]
        public ActionResult DataChangeLargeDescr(string ItemCode, string LargeDescription)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_Tools.ValidStringParameter(ItemCode, "ItemCode");
                Ecom_Tools.ValidStringParameter(LargeDescription, "LargeDescription");
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Producto Ecom_Producto_ = new Ecom_Producto(Ecom_DBConnection_);
                bool result = Ecom_Producto_.UpdLargeDescription(ItemCode, LargeDescription);
                Ecom_DBConnection_.CloseConnection();
                if (result)
                {
                    return Ok(Ecom_DBConnection_.Message);
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
        [AccessData(IdAction = 2)]
        public ActionResult DataDesactiveActive(bool Active, string ItemCode)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Producto Ecom_Producto_ = new Ecom_Producto(Ecom_DBConnection_);
                bool result = Ecom_Producto_.UpdActive(ItemCode, Active);
                Ecom_DBConnection_.CloseConnection();
                if (result)
                {
                    return Ok(Ecom_DBConnection_.Message);
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
        [AccessData(IdAction = 1)]
        public ActionResult DataGetFiles(string ImagessType, string ItemCode)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server,FTP_User, FTP_Password);
                if (ImagessType.Trim() == "Producto")
                {
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/*.jpg", ItemCode);
                    string PathPublicItem = string.Format(@"{0}/fibra-optica/public/images/img_spl/productos/{1}/", Ecommerce_Domain,ItemCode);
                    return Ok(Ecom_FilesFtp.Getfiles(PathItem, PathPublicItem));
                }
                else if (ImagessType.Trim() == "Descripcion")
                {
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/descripcion/*.jpg", ItemCode);
                    string PathPublicItem = string.Format(@"{0}/fibra-optica/public/images/img_spl/productos/{1}/descripcion/", Ecommerce_Domain, ItemCode);
                    return Ok(Ecom_FilesFtp.Getfiles(PathItem, PathPublicItem)); 
                }
                else if (ImagessType.Trim() == "InfoAdicional")
                {
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/adicional/*.jpg", ItemCode);
                    string PathPublicItem = string.Format(@"{0}/fibra-optica/public/images/img_spl/productos/{1}/adicional/", Ecommerce_Domain, ItemCode);
                    return Ok(Ecom_FilesFtp.Getfiles(PathItem, PathPublicItem));
                }
                else if (ImagessType.Trim() == "Miniatura")
                {
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/thumbnail/*.jpg", ItemCode);
                    string PathPublicItem = string.Format(@"{0}/fibra-optica/public/images/img_spl/productos/{1}/thumbnail/", Ecommerce_Domain, ItemCode);
                    return Ok(Ecom_FilesFtp.Getfiles(PathItem, PathPublicItem));
                }
                else
                {
                    return BadRequest("Error de configuración");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataDeleteFiles(string ImagessType, string Filename, string ItemCode)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                if (ImagessType.Trim() == "Producto")
                {
                    ValidAction(7);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    return Ok("Archivo eliminado");
                }
                else if (ImagessType.Trim() == "Descripcion")
                {
                    ValidAction(6);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/descripcion/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    return Ok("Archivo eliminado");
                }
                else if (ImagessType.Trim() == "InfoAdicional")
                {
                    ValidAction(5);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/adicional/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    return Ok("Archivo eliminado");
                }
                else if (ImagessType.Trim() == "Miniatura")
                {
                    ValidAction(26);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/thumbnail/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    UpdateImageName(ItemCode, "");
                    return Ok("Archivo eliminado");
                }
                else
                {
                    return BadRequest("Error de configuración");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataUploadFiles(IFormFile FormFile, string ItemCode, string ImagessType)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                var Filename = FormFile.FileName;
                if (ImagessType.Trim() == "Producto")
                {
                    ValidAction(7);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    return Ok("Archivo cargado");
                }
                else if (ImagessType.Trim() == "Descripcion")
                {
                    ValidAction(6);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/descripcion/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    return Ok("Archivo cargado");
                }
                else if (ImagessType.Trim() == "InfoAdicional")
                {
                    ValidAction(5);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/adicional/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    return Ok("Archivo cargado");
                }
                else if (ImagessType.Trim() == "Miniatura")
                {
                    ValidAction(26);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/thumbnail/*.jpg", ItemCode);
                    string PathPublicItem = string.Format(@"{0}/fibra-optica/public/images/img_spl/productos/{1}/thumbnail/", Ecommerce_Domain, ItemCode);
                    if(Ecom_FilesFtp.Getfiles(PathItem, PathPublicItem).Count > 0)
                    {
                        return BadRequest("ya existe una imagen como miniatura predeterminada");
                    }
                    PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/thumbnail/{1}", ItemCode, Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    UpdateImageName(ItemCode, Filename);
                    return Ok("Archivo cargado");
                }
                else if (ImagessType.Trim() == "360")
                {
                    ValidAction(49);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/{1}", ItemCode, "360.zip");
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    return Ok("Archivo Zip contenedor de modelo 360 cargado");
                }
                else
                {
                    return BadRequest("Error de configuración");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public ActionResult DataRenameFiles(string ImagessType, string Filename, string ItemCode, string Newname)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                if (ImagessType.Trim() == "Producto")
                {
                    ValidAction(7);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/", ItemCode);
                    Ecom_FilesFtp.Rename(PathItem, Filename, Newname+".jpg");
                    return Ok("Archivo renombrado");
                }
                else if (ImagessType.Trim() == "Descripcion")
                {
                    ValidAction(6);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/descripcion/", ItemCode);
                    Ecom_FilesFtp.Rename(PathItem, Filename, Newname + ".jpg");
                    return Ok("Archivo renombrado");
                }
                else if (ImagessType.Trim() == "InfoAdicional")
                {
                    ValidAction(5);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/adicional/", ItemCode);
                    Ecom_FilesFtp.Rename(PathItem, Filename, Newname + ".jpg");
                    return Ok("Archivo renombrado");
                }
                else if (ImagessType.Trim() == "Miniatura")
                {
                    ValidAction(26);
                    string PathItem = string.Format(@"public_html/fibra-optica/public/images/img_spl/productos/{0}/thumbnail/", ItemCode);
                    Ecom_FilesFtp.Rename(PathItem, Filename, Newname + ".jpg");
                    UpdateImageName(ItemCode, Newname + ".jpg");
                    return Ok("Archivo renombrado");
                }
                else
                {
                    return BadRequest("Error de configuración");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private void ValidAction(int IdAction)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessBysalesEmp = new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(USR_IdSplinnet, IdAction);
                Ecom_DBConnection_.CloseConnection();
                if (!AccessBysalesEmp)
                {
                    throw new Ecom_Exception("No tienes autorización para esta acción");
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
            }
        }
        private void UpdateImageName(string ItemCode, string imageName)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            try
            {
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
                bool AccessBysalesEmp = new Ecom_Producto(Ecom_DBConnection_).UpdImagenPrincipal(ItemCode, imageName);
                Ecom_DBConnection_.CloseConnection();
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
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