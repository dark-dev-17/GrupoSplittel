using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcomDataProccess;
using EcommerceAdmin.Models.Filters;

namespace EcommerceAdmin.Controllers
{
    public class FichaTecnicaController : Controller
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();

        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();
        private EcomData ecomData;

        // GET: FichaTecnica
        [AccessView(IdAction = 46)]
        public ActionResult List(string Folder)
        {
            try
            {
                if (string.IsNullOrEmpty(Folder) || string.IsNullOrWhiteSpace(Folder))
                {
                    Folder = @"public_html/fibra-optica/public/images/img_spl/FICHAS TÉCNICAS/";
                }
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                List<Ecom_Archivos> List = Ecom_FilesFtp.ListDirectory(Folder);
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                
                List.ForEach(item => {
                    item.Path = string.Format("{0}{1}", Folder,item.Name);
                    int tam = item.Path.Length;
                    item.PathAux = item.Path.Substring(40,tam-40);
                    item.PathFolder = string.Format("{0}", Folder);
                    if (!item.IsDirectory)
                    {
                        Ecom_ProductoFichaTecnica Ecom_ProductoFichaTecnica_ = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                        item.PathAux = item.PathAux.Substring(0, item.PathAux.Length - 4);
                        if (Ecom_ProductoFichaTecnica_.GetByRute(item.PathAux))
                        {
                            item.Objecto = Ecom_ProductoFichaTecnica_;
                        }
                        else
                        {
                            item.Objecto = null;
                        }
                    }
                    else
                    {
                        item.Path = item.Path + "/";
                    }
                });
                ViewData["Folder"] = Folder;
                return View(List);
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if(ecomData != null)
                {
                    ecomData.Disconect(ServerSource.Ecommerce);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 46)]
        public ActionResult DataList(string Folder)
        {
            try
            {
                if (string.IsNullOrEmpty(Folder) || string.IsNullOrWhiteSpace(Folder))
                {
                    Folder = @"public_html/fibra-optica/public/images/img_spl/FICHAS TÉCNICAS/";
                }
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                List<Ecom_Archivos> List = Ecom_FilesFtp.ListDirectory(Folder);
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);

                List.ForEach(item => {
                    item.Path = string.Format("{0}{1}", Folder, item.Name);
                    int tam = item.Path.Length;
                    item.PathAux = item.Path.Substring(40, tam - 40);
                    item.PathFolder = string.Format("{0}", Folder);
                    if (!item.IsDirectory)
                    {
                        Ecom_ProductoFichaTecnica Ecom_ProductoFichaTecnica_ = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                        item.PathAux = item.PathAux.Substring(0, item.PathAux.Length - 4);
                        if (Ecom_ProductoFichaTecnica_.GetByRute(item.PathAux))
                        {
                            item.Objecto = Ecom_ProductoFichaTecnica_;
                        }
                        else
                        {
                            item.Objecto = null;
                        }
                    }
                    else
                    {
                        item.Path = item.Path + "/";
                    }
                });
                return Ok(List);
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
        [AccessData(IdAction = 46)]
        public ActionResult ShowDirectory(string Folder)
        {
            try
            {
                if (string.IsNullOrEmpty(Folder) || string.IsNullOrWhiteSpace(Folder))
                {
                    Folder = @"public_html/fibra-optica/public/images/img_spl/FICHAS TÉCNICAS/";
                }
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                return Ok(Ecom_FilesFtp.ListDirectoryDetails(Folder));
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
        [AccessView(IdAction = 46)]
        public ActionResult DataRegisterFiles(string FileName, string Folder)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                if (Folder.Trim() != "")
                {
                    string PathItem = string.Format(@"{0}{1}", Folder, FileName);
                    string NewPath = PathItem.Substring(40, PathItem.Length - 40);
                    ecomData = new EcomData(EcomConnection, SplitConnection);
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoFichaTecnica ecom_ProductoFichaTecnica = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                    ecom_ProductoFichaTecnica.Ruta = NewPath.Substring(0, NewPath.Length - 4); ;
                    if (ecom_ProductoFichaTecnica.Add())
                    {
                        return Ok("Archivo cargado");
                    }
                    else
                    {
                        return BadRequest("Archivo no cargado");
                    }
                }
                else
                {
                    return BadRequest("Folder no valido");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 46)]
        public ActionResult DataRenameFiles(string Folder,string ActualName, string Newname)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                Ecom_FilesFtp.Rename(Folder, ActualName, Newname + ".pdf");
                string PathFile = string.Format("{0}{1}", Folder, ActualName);
                string Path = PathFile.Substring(40, PathFile.Length - 40);
                Path = Path.Substring(0, Path.Length - 4);
                ecomData = new EcomData(EcomConnection, SplitConnection);
                ecomData.Connect(ServerSource.Ecommerce);
                Ecom_ProductoFichaTecnica Ecom_ProductoFichaTecnica_ = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                if (Ecom_ProductoFichaTecnica_.GetByRute(Path))
                {
                    Ecom_ProductoFichaTecnica_.Ruta = string.Format("{0}{1}", Folder, Newname);
                    Ecom_ProductoFichaTecnica_.Ruta = Ecom_ProductoFichaTecnica_.Ruta.Substring(40, Ecom_ProductoFichaTecnica_.Ruta.Length - 40);
                    Ecom_ProductoFichaTecnica_.Update(2);
                }
                return Ok("Archivo renombrado");
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 46)]
        public ActionResult DataUploadFiles(IFormFile FormFile, string Folder)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                var Filename = FormFile.FileName;
                if (Folder.Trim() != "")
                {
                    string PathItem = string.Format(@"{0}{1}", Folder, Filename);
                    string NewPath = PathItem.Substring(40, PathItem.Length - 40);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);
                    ecomData = new EcomData(EcomConnection, SplitConnection);
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoFichaTecnica ecom_ProductoFichaTecnica = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                    ecom_ProductoFichaTecnica.Ruta = NewPath.Substring(0, NewPath.Length - 4); ;
                    if (ecom_ProductoFichaTecnica.Add())
                    {
                        return Ok("Archivo cargado");
                    }
                    else
                    {
                        return BadRequest("Archivo no cargado");
                    }
                }
                else
                {
                    return BadRequest("Folder no valido");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessView(IdAction = 46)]
        public ActionResult DataChangeFiles(IFormFile FormFile, string Folder, string FileActual)
        {
            try
            {
                Ecom_FilesFtp Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
                var Filename = FormFile.FileName;
                if (Folder.Trim() != "")
                {
                    string PathItem = string.Format(@"{0}{1}", Folder, FileActual);
                    Ecom_FilesFtp.DeleteFile(PathItem);
                    string OldFile = PathItem.Substring(40, PathItem.Length - 40);
                    OldFile = OldFile.Substring(0, OldFile.Length - 4);
                    //subir nuevo archivo
                    PathItem = string.Format(@"{0}{1}", Folder, Filename);
                    Ecom_FilesFtp.UpdateFile(PathItem, FormFile);

                    ecomData = new EcomData(EcomConnection, SplitConnection);
                    ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_ProductoFichaTecnica ecom_ProductoFichaTecnica = (Ecom_ProductoFichaTecnica)ecomData.GetObject(ObjectSource.ProductoFichaTecnica);
                    if (ecom_ProductoFichaTecnica.GetByRute(OldFile))
                    {
                        string NewPath = string.Format(@"{0}{1}", Folder, Filename);
                        NewPath = PathItem.Substring(40, PathItem.Length - 40);
                        ecom_ProductoFichaTecnica.Ruta = NewPath.Substring(0, NewPath.Length - 4);
                        ecom_ProductoFichaTecnica.Update(2);
                    }
                    return Ok("PDF remplazado");
                }
                else
                {
                    return BadRequest("Folder no valido");
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: FichaTecnica/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: FichaTecnica/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: FichaTecnica/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
        
    }
}