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
    public class HomeAnuncioController : Controller
    {
        private Ecommerce Ecommerce_;
        
        // GET: SitioSlide
        [AccessView(IdAction = 50)]
        public ActionResult Index()
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                return View(Ecom_HomeAnuncio_.Get());
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

        // GET: SitioSlide/Details/5
        [AccessView(IdAction = 50)]
        public ActionResult Details(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                if (Ecom_HomeAnuncio_.Get(id))
                {
                    return View(Ecom_HomeAnuncio_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce) });
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

        // GET: SitioSlide/Create
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Create()
        {
            return View();
        }

        [AccessView(IdAction = 53)]
        public ActionResult Order()
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                return View(Ecom_HomeAnuncio_.Get().Where(item => item.IsActive).OrderBy(item => item.Position));
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

        // POST: SitioSlide/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Create(Ecom_HomeAnuncio Ecom_HomeAnuncio_)
        {
            Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {
                if(Ecom_HomeAnuncio_.ShowBy == "PUBLIC")
                {
                    Ecom_HomeAnuncio_.RuleTopic = "ning";
                    Ecom_HomeAnuncio_.RuleOperator = "<";
                    Ecom_HomeAnuncio_.Quantity = 0;
                }
                if (Ecom_HomeAnuncio_.RuleTopic == "ning")
                {
                    Ecom_HomeAnuncio_.RuleOperator = "<";
                    Ecom_HomeAnuncio_.Quantity = 0;
                }
                if (!ModelState.IsValid)
                {
                    return View(Ecom_HomeAnuncio_);
                }
                else
                {
                    if (Ecom_HomeAnuncio_.ImgSmall_ == null || Ecom_HomeAnuncio_.ImgLarge_ == null)
                    {
                        if (Ecom_HomeAnuncio_.ImgSmall_ == null)
                            ModelState.AddModelError("ImgSmall_", "Please choose a file");
                        if (Ecom_HomeAnuncio_.ImgLarge_ == null)
                            ModelState.AddModelError("ImgLarge_", "Please choose a file");
                        return View(Ecom_HomeAnuncio_);
                    }
                    else
                    {
                        Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                        Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                        Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                        Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.SetObjectConnection(Ecom_HomeAnuncio_, ObjectSource.HomeAnuncio);

                        ValidPermis(Ecom_HomeAnuncio_);


                        int LastIde = Ecom_HomeAnuncio_.LastId();
                        LastIde = LastIde + 1;

                        string ImgSmall_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img1/anuncio_{0}.{1}", LastIde, Ecom_HomeAnuncio_.ImgSmall_.FileName.Split('.')[1]);
                        Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgSmall_, Ecom_HomeAnuncio_.ImgSmall_);

                        string ImgLarge_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img2/anuncio_{0}.{1}", LastIde, Ecom_HomeAnuncio_.ImgLarge_.FileName.Split('.')[1]);
                        Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgLarge_, Ecom_HomeAnuncio_.ImgLarge_);

                        Ecom_HomeAnuncio_.ImgSmall = string.Format("anuncio_{0}.{1}", LastIde, Ecom_HomeAnuncio_.ImgSmall_.FileName.Split('.')[1]);
                        Ecom_HomeAnuncio_.ImgLarge = string.Format("anuncio_{0}.{1}", LastIde, Ecom_HomeAnuncio_.ImgLarge_.FileName.Split('.')[1]);

                        

                        if (Ecom_HomeAnuncio_.Add())
                        {
                            Ecommerce_.SaveNotificationLog("success", "Se ha creado un nuevo slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_.LastId()+"", "");
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                            return View(Ecom_HomeAnuncio_);
                        }
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return View(Ecom_HomeAnuncio_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return View(Ecom_HomeAnuncio_);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                    if (Ecommerce_.ecomData.Ecom_FilesFtp != null)
                    {
                        Ecommerce_.ecomData.Ecom_FilesFtp = null;
                    }
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        // GET: SitioSlide/Edit/5
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Edit(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                if (Ecom_HomeAnuncio_.Get(id))
                {
                    ValidPermis(Ecom_HomeAnuncio_);
                    return View(Ecom_HomeAnuncio_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce) });
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

        // GET: SitioSlide/Delete/5
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Delete(int id)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                
                if (Ecom_HomeAnuncio_.Get(id))
                {
                    ValidPermis(Ecom_HomeAnuncio_);
                    return View(Ecom_HomeAnuncio_);
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce) });
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

        // POST: Articulo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Ecommerce_ = new Ecommerce(HttpContext.Session);
                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                Ecom_HomeAnuncio Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                if (Ecom_HomeAnuncio_.Get(id))
                {
                    ValidPermis(Ecom_HomeAnuncio_);
                    if (Ecom_HomeAnuncio_.ImgSmall.Contains("jpg") || Ecom_HomeAnuncio_.ImgSmall.Contains("JPG") || Ecom_HomeAnuncio_.ImgSmall.Contains("png") || Ecom_HomeAnuncio_.ImgSmall.Contains("PNG"))
                    {
                        string Small_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img1/{0}", Ecom_HomeAnuncio_.ImgSmall);
                        Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(Small_);
                    }
                    if (Ecom_HomeAnuncio_.ImgLarge.Contains("jpg") || Ecom_HomeAnuncio_.ImgLarge.Contains("JPG") || Ecom_HomeAnuncio_.ImgLarge.Contains("png") || Ecom_HomeAnuncio_.ImgLarge.Contains("PNG"))
                    {
                        string Small_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img2/{0}", Ecom_HomeAnuncio_.ImgLarge);
                        Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(Small_);
                    }

                    if (Ecom_HomeAnuncio_.Update(HomeAnuncioActionsDB.Delete))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(new { id = Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce) });
                    }
                }
                else
                {
                    return View("../ErrorPages/Error", new { id = Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce) });
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

        // POST: SitioSlide/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 51,52 })]
        public ActionResult Edit(Ecom_HomeAnuncio Ecom_HomeAnuncio_)
        {
            Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {
                if(Ecom_HomeAnuncio_.ShowBy == "PUBLIC")
                {
                    Ecom_HomeAnuncio_.RuleTopic = "ning";
                    Ecom_HomeAnuncio_.RuleOperator = "<";
                    Ecom_HomeAnuncio_.Quantity = 0;
                }
                if (Ecom_HomeAnuncio_.RuleTopic == "ning")
                {
                    Ecom_HomeAnuncio_.RuleOperator = "<";
                    Ecom_HomeAnuncio_.Quantity = 0;
                }
                if (!ModelState.IsValid)
                {
                    return View(Ecom_HomeAnuncio_);
                }
                else
                {
                    Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                    Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                    Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.SetObjectConnection(Ecom_HomeAnuncio_, ObjectSource.HomeAnuncio);
                    ValidPermis(Ecom_HomeAnuncio_);
                    if (Ecom_HomeAnuncio_.Update(HomeAnuncioActionsDB.UpdateData))
                    {
                        Ecommerce_.SaveNotificationLog("success", "Se ha editado un slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_.Id + "", "");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Format("{0}", Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce)));
                        return View(Ecom_HomeAnuncio_);
                    }
                }
            }
            catch (Ecom_Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return View(Ecom_HomeAnuncio_);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0}", ex.Message));
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return View(Ecom_HomeAnuncio_);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                    if (Ecommerce_.ecomData.Ecom_FilesFtp != null)
                    {
                        Ecommerce_.ecomData.Ecom_FilesFtp = null;
                    }
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
        public ActionResult EditChangeImg(IFormFile Img, string type, int Id)
        {
            Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {

                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                if (Img == null)
                    throw new Ecom_Exception("Por favor selecciona un archivo");

                Ecom_HomeAnuncio Ecom_HomeAnuncio_2 = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                if (Ecom_HomeAnuncio_2.Get(Id))
                {
                    ValidPermis(Ecom_HomeAnuncio_2);
                    if (type.Trim() == "left")
                    {
                        if (Ecom_HomeAnuncio_2.ImgSmall.Contains("jpg") || Ecom_HomeAnuncio_2.ImgSmall.Contains("JPG") || Ecom_HomeAnuncio_2.ImgSmall.Contains("png") || Ecom_HomeAnuncio_2.ImgSmall.Contains("PNG"))
                        {
                            string Small_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img1/{0}", Ecom_HomeAnuncio_2.ImgSmall);
                            Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(Small_);
                        }

                        string ImgSmall_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img1/{0}", Ecom_HomeAnuncio_2.ImgSmall.Split('.')[0]+"."+ Img.FileName.Split('.')[1]);
                        Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgSmall_, Img);
                        Ecom_HomeAnuncio_2.ImgSmall = Ecom_HomeAnuncio_2.ImgSmall.Split('.')[0] + "." + Img.FileName.Split('.')[1];
                        if (Ecom_HomeAnuncio_2.Update(HomeAnuncioActionsDB.UpdateImgLeft))
                        {
                            Ecommerce_.SaveNotificationLog("success", "Se ha cambiado la imgen izquierda un slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_2.Id + "", "");
                            return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                        else
                        {
                            return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }

                    }
                    else if(type.Trim() == "right")
                    {
                        if (Ecom_HomeAnuncio_2.ImgLarge.Contains("jpg") || Ecom_HomeAnuncio_2.ImgLarge.Contains("JPG") || Ecom_HomeAnuncio_2.ImgLarge.Contains("png") || Ecom_HomeAnuncio_2.ImgLarge.Contains("PNG"))
                        {
                            string ImgLarge = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img2/{0}", Ecom_HomeAnuncio_2.ImgLarge);
                            Ecommerce_.ecomData.Ecom_FilesFtp.DeleteFile(ImgLarge);
                        }

                        string ImgLarge_ = string.Format(@"public_html/fibra-optica/public/images/img_spl/slide/img2/{0}", Ecom_HomeAnuncio_2.ImgLarge.Split('.')[0] + "." + Img.FileName.Split('.')[1]);
                        Ecommerce_.ecomData.Ecom_FilesFtp.UpdateFile(ImgLarge_, Img);
                        Ecom_HomeAnuncio_2.ImgLarge = Ecom_HomeAnuncio_2.ImgLarge.Split('.')[0] + "." + Img.FileName.Split('.')[1];
                        if (Ecom_HomeAnuncio_2.Update(HomeAnuncioActionsDB.UpdateImgRight))
                        {
                            Ecommerce_.SaveNotificationLog("success", "Se ha cambiado la imgen derecha un slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_2.Id + "", "");
                            return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                        else
                        {
                            return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                    }
                    else
                    {
                        return BadRequest(string.Format("tipo de imagen {0} no es valido",type));
                    }
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                    if (Ecommerce_.ecomData.Ecom_FilesFtp != null)
                    {
                        Ecommerce_.ecomData.Ecom_FilesFtp = null;
                    }
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
        public ActionResult EditRenameImg(string NewName, string type, int Id)
        {
            Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {

                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                if (string.IsNullOrEmpty(NewName) || string.IsNullOrWhiteSpace(NewName))
                    throw new Ecom_Exception("Por favor introduce un nombre valido para el archivo");

                Ecom_HomeAnuncio Ecom_HomeAnuncio_2 = (Ecom_HomeAnuncio)Ecommerce_.ecomData.GetObject(ObjectSource.HomeAnuncio);
                if (Ecom_HomeAnuncio_2.Get(Id))
                {
                    ValidPermis(Ecom_HomeAnuncio_2);
                    if (type.Trim() == "left")
                    {
                        if (Ecom_HomeAnuncio_2.ImgSmall.Contains("jpg") || Ecom_HomeAnuncio_2.ImgSmall.Contains("JPG") || Ecom_HomeAnuncio_2.ImgSmall.Contains("png") || Ecom_HomeAnuncio_2.ImgSmall.Contains("PNG"))
                        {
                            Ecommerce_.ecomData.Ecom_FilesFtp.Rename("public_html/fibra-optica/public/images/img_spl/slide/img1/", Ecom_HomeAnuncio_2.ImgSmall, NewName +"." + Ecom_HomeAnuncio_2.ImgSmall.Split('.')[1]);
                        }
                        Ecom_HomeAnuncio_2.ImgSmall = NewName + "." + Ecom_HomeAnuncio_2.ImgSmall.Split('.')[1];
                        if (Ecom_HomeAnuncio_2.Update(HomeAnuncioActionsDB.UpdateImgLeft))
                        {
                            Ecommerce_.SaveNotificationLog("success", "Se ha renombrado la imagen izquierda un slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_2.Id + "", "");
                            return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                        else
                        {
                            return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                    }
                    else if (type.Trim() == "right")
                    {
                        if (Ecom_HomeAnuncio_2.ImgLarge.Contains("jpg") || Ecom_HomeAnuncio_2.ImgLarge.Contains("JPG") || Ecom_HomeAnuncio_2.ImgLarge.Contains("png") || Ecom_HomeAnuncio_2.ImgLarge.Contains("PNG"))
                        {
                            Ecommerce_.ecomData.Ecom_FilesFtp.Rename("public_html/fibra-optica/public/images/img_spl/slide/img2/", Ecom_HomeAnuncio_2.ImgLarge, NewName + "." + Ecom_HomeAnuncio_2.ImgLarge.Split('.')[1]);
                        }
                        Ecom_HomeAnuncio_2.ImgLarge = NewName + "." + Ecom_HomeAnuncio_2.ImgLarge.Split('.')[1];
                        if (Ecom_HomeAnuncio_2.Update(HomeAnuncioActionsDB.UpdateImgRight))
                        {
                            Ecommerce_.SaveNotificationLog("success", "Se ha renombrado la imagen derecha un slide", "HomeAnuncio", "Details", Ecom_HomeAnuncio_2.Id + "", "");
                            return Ok(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                        else
                        {
                            return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                        }
                    }
                    else
                    {
                        return BadRequest(string.Format("tipo de imagen {0} no es valido", type));
                    }
                }
                else
                {
                    return BadRequest(Ecommerce_.ecomData.GetLastMessage(ServerSource.Ecommerce));
                }
            }
            catch (Ecom_Exception ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                    if (Ecommerce_.ecomData.Ecom_FilesFtp != null)
                    {
                        Ecommerce_.ecomData.Ecom_FilesFtp = null;
                    }
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 53)]
        public ActionResult EditPositions([FromBody]List<Ecom_HomeAnuncio> Listt)
        {
            Ecommerce_ = new Ecommerce(HttpContext.Session);
            try
            {

                Ecommerce_.StartLib(LibraryEcommerce.Ecommerce);
                Ecommerce_.StartLib(LibraryEcommerce.FTP_Ecommerce);
                Ecommerce_.ecomData.Connect(ServerSource.Ecommerce);
                bool process = true;
                Listt.ForEach(Ecom_HomeAnuncio_ => {
                    Ecom_HomeAnuncio_ = (Ecom_HomeAnuncio)Ecommerce_.ecomData.SetObjectConnection(Ecom_HomeAnuncio_, ObjectSource.HomeAnuncio);
                    if(!Ecom_HomeAnuncio_.Update(HomeAnuncioActionsDB.UpdatePosition))
                    {
                        process = false;
                    }
                });
                if (process)
                {
                    Ecommerce_.SaveNotificationLog("success", "ha moficado el orden de los slides", "", "",  "", "");
                    return Ok("Se ha guardado el orden");
                }
                else
                {
                    return BadRequest("No se ha guardado el orden");
                }
            }
            catch (Ecom_Exception ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                Ecommerce_.SaveNotificationLog("warning", ex.Message, "", "", "", ex.ToString());
                return BadRequest(ex.Message);
            }
            finally
            {
                if (Ecommerce_.ecomData != null)
                {
                    Ecommerce_.ecomData.Disconect(ServerSource.Ecommerce);
                    Ecommerce_.ecomData.Disconect(ServerSource.Splitnet);
                    if (Ecommerce_.ecomData.Ecom_FilesFtp != null)
                    {
                        Ecommerce_.ecomData.Ecom_FilesFtp = null;
                    }
                }
                if (Ecommerce_.sAPData != null)
                {
                    Ecommerce_.sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
                }
            }
        }

        private void ValidPermis(Ecom_HomeAnuncio Ecom_HomeAnuncio_)
        {
            Ecommerce_.ecomData.Connect(ServerSource.Splitnet);
            bool CtrlPublic = Ecommerce_.ValidActionUser(52);
            bool CtrlDirigidos = Ecommerce_.ValidActionUser(51);

            if (!CtrlPublic && Ecom_HomeAnuncio_.ShowBy == "PUBLIC")
            {
                throw new Ecom_Exception("No tienes permisos para slides publicos");
            }

            if (!CtrlDirigidos && Ecom_HomeAnuncio_.ShowBy == "B2C" || !CtrlDirigidos && Ecom_HomeAnuncio_.ShowBy == "B2B")
            {
                throw new Ecom_Exception("No tienes permisos para slides dirigidos");
            }
        }
    }
}