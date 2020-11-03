using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation.Models;
using GPSInformation.DBManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GPSInformation;
using GestionPersonal.Models;
using Microsoft.AspNetCore.Http;
using GPSInformation.Controllers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using GPSInformation.Exceptions;
using GestionPersonal.Service;
using System.Net;

namespace GestionPersonal.Controllers
{
    public class UsuarioController : Controller
    {
        private DarkManager darkManager;
        private SelectList Generos;
        private SelectList EstadosCiviles;
        private SelectList Alergias;
        private SelectList TiposSangre;
        private SelectList TipoNomina;
        private SelectList EstatusEmpleado;
        private SelectList Puestos;
        private SelectList Sociedades;
        private SelectList Departamentos;
        private SelectList Parentezcos;
        private SelectList Personas;
        private readonly IHostingEnvironment _environment;
        private UsuarioCtrl usuarioCtrl;
        private readonly IViewRenderService _viewRenderService;


        public UsuarioController(IConfiguration configuration, IHostingEnvironment IHostingEnvironment, IViewRenderService viewRenderService)
        {
            darkManager = new DarkManager(configuration);
            //darkManager.OpenConnection();
            //darkManager.LoadObject(GpsManagerObjects.Usuario);
            //darkManager.LoadObject(GpsManagerObjects.Persona);
            //darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            //darkManager.LoadObject(GpsManagerObjects.InformacionMedica);
            //darkManager.LoadObject(GpsManagerObjects.Puesto);
            //darkManager.LoadObject(GpsManagerObjects.Sociedad);
            //darkManager.LoadObject(GpsManagerObjects.Departamento);
            //darkManager.LoadObject(GpsManagerObjects.Empleado);
            //darkManager.LoadObject(GpsManagerObjects.PersonaContacto);
            //darkManager.LoadObject(GpsManagerObjects.View_empleado);
            //darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
            usuarioCtrl = new UsuarioCtrl(darkManager);
            _environment = IHostingEnvironment;
            _viewRenderService = viewRenderService;
        }

        public IActionResult ProcessUsersRols()
        {
            try
            {
                usuarioCtrl.ProcessAllusersPermis();
                return Ok("Roles asignados");
            }
            catch (GpExceptions ex)
            {
                return BadRequest("Roles no asignados");
            }
        }

        [AccessView]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FotoPerfil(IFormFile foto)
        {
            darkManager.StartTransaction();
            try
            {
                int UsuarioIdPer = (int)HttpContext.Session.GetInt32("user_id_permiss");

                var Usuario_re = darkManager.Usuario.Get(UsuarioIdPer);

                if (Usuario_re == null)
                    throw new Exception("Error usuario no encontrado");

                if (foto.Length <= 0)
                    throw new Exception("Error, la foto esta dañada");

                if (Usuario_re.ImagenDefault == true)
                {
                    Usuario_re.ImagenPerfil = string.Format("usuario_perfil_{0}.{1}", Usuario_re.IdUsuario, foto.FileName.Split('.')[1]);
                    string Directorio = string.Format(@"{0}\Perfil\{1}", _environment.WebRootPath, Usuario_re.ImagenPerfil);


                    using (FileStream fs = System.IO.File.Create(Directorio))
                    {
                        foto.CopyTo(fs);
                        fs.Flush();
                    }

                    Usuario_re.ImagenDefault = false;
                    darkManager.Usuario.Element = Usuario_re;
                    if (!darkManager.Usuario.Update())
                    {
                        throw new Exception("Error, Error al actualizar datos del perfil");
                    }
                }
                else
                {
                    string Directorio = string.Format(@"{0}\{1}", _environment.WebRootPath, Usuario_re.ImagenPerfil);
                    if (System.IO.File.Exists(Directorio))
                    {
                        System.IO.File.Delete(Directorio);
                    }

                    Usuario_re.ImagenPerfil = string.Format("usuario_perfil_{0}.{1}", Usuario_re.IdUsuario, foto.FileName.Split('.')[1]);
                    Directorio = string.Format(@"{0}\Perfil\{1}", _environment.WebRootPath, Usuario_re.ImagenPerfil);

                    using (FileStream fs = System.IO.File.Create(Directorio))
                    {
                        foto.CopyTo(fs);
                        fs.Flush();
                    }

                    Usuario_re.ImagenDefault = false;
                    darkManager.Usuario.Element = Usuario_re;
                    if (!darkManager.Usuario.Update())
                    {
                        throw new Exception("Error, Error al actualizar datos del perfil");
                    }
                }
                HttpContext.Session.SetString("user_imagenPerfil", Usuario_re.ImagenPerfil);
                darkManager.Commit();
                darkManager.CloseConnection();
                return RedirectToAction("Perfil");
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
            finally
            {
                darkManager.CloseConnection();
            }

        }

        // GET: Direccion
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Index()
        {
            try
            {
                var List = darkManager.Usuario.Get();
                darkManager.CloseConnection();
                return View(List);
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        #region Usuario
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Create(int id)
        {
            try
            {
                Personas = GetDictionary(1018, 0);
                ViewData["Roles"] = Personas;
                var usuario = darkManager.Usuario.GetByColumn(""+  id, "IdPersona");
                if(usuario is null)
                {
                    var empleado = darkManager.View_empleado.GetByColumn("" + id, "IdPersona"); ;
                    Usuario ususario = new Usuario { 
                        IdPersona = empleado.IdPersona,
                        Activo = true,
                        UserName = empleado.NumeroNomina
                    };
                    return View(ususario);
                }
                else
                {
                    return RedirectToAction("Edit", new { id = id });
                }
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
            
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario Usuario)
        {

            Personas = GetDictionary(1018, 0);
            ViewData["Roles"] = Personas;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Usuario);
                }
                if (Usuario.IdRol == 0)
                {
                    ModelState.AddModelError("IdRol", "Por favor selecciona un rol de acceso");
                    return View(Usuario);
                }
                usuarioCtrl.AddUsuario(Usuario);
                if (Usuario.EnviarCorreo)
                {
                    var us_re = usuarioCtrl.GetUsuarioRe(Usuario.IdPersona);
                    us_re.Ipserver = HttpContext.Connection.LocalIpAddress?.ToString();
                    us_re.Port = HttpContext.Connection.LocalPort.ToString();
                    var result = await _viewRenderService.RenderToStringAsync("Usuario/DetailsEmail", us_re);
                    usuarioCtrl.SendEmail(us_re, result);
                }


                return RedirectToAction(nameof(Index),"Empleado");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Edit(int id)
        {
            try
            {
                var usuario = darkManager.Usuario.GetByColumn("" + id, "IdPersona");

                if (usuario is null)
                {
                    return NotFound();
                }
                Personas = GetDictionary(1018, usuario.IdRol);
                ViewData["Roles"] = Personas;
                return View(usuario);
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        [AccessView]
        public IActionResult CambioPass()
        {
            try
            {
                int UsuarioIdPer = (int)HttpContext.Session.GetInt32("user_id_permiss");
                var usuario = darkManager.Usuario.GetByColumn("" + UsuarioIdPer, "IdUsuario");

                if (usuario is null)
                {
                    return NotFound();
                }
                //Personas = GetDictionary(1018, usuario.IdRol);
                //ViewData["Roles"] = Personas;
                return View(usuario);
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        [AccessView]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambioPass(Usuario Usuario)
        {
            darkManager.StartTransaction();
            try
            {
                int UsuarioIdPer = (int)HttpContext.Session.GetInt32("user_id_permiss");
                var usuario_ = darkManager.Usuario.GetByColumn("" + UsuarioIdPer, "IdUsuario");

                if (Usuario.Pass != Usuario.Pass2)
                {
                    ModelState.AddModelError("Pass", "Deben de ser iguales ambas contraseñas");
                    ModelState.AddModelError("Pass2", "Deben de ser iguales ambas contraseñas");

                    return View(Usuario);
                }

                usuario_.Pass = Usuario.Pass;
                usuario_.Pass2 = Usuario.Pass2;
                darkManager.Usuario.Element = usuario_;
                if (!darkManager.Usuario.Update())
                {
                    throw new GpExceptions("Error al guardar");
                }
                darkManager.Commit();
                return RedirectToAction(nameof(Perfil));
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult DetailsEmail(int id)
        {
            try
            {
                return View(usuarioCtrl.GetUsuarioRe(id));
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
            
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario Usuario)
        {
            Personas = GetDictionary(1018, Usuario.IdRol);
            ViewData["Roles"] = Personas;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Usuario);
                }
                if (Usuario.IdRol == 0)
                {
                    ModelState.AddModelError("IdRol", "Por favor selecciona un rol de acceso");
                    return View(Usuario);
                }
                usuarioCtrl.AddUsuario(Usuario);


                if (Usuario.EnviarCorreo)
                {
                    var us_re = usuarioCtrl.GetUsuarioRe(Usuario.IdPersona);
                    us_re.Ipserver = HttpContext.Connection.LocalIpAddress?.ToString();
                    us_re.Port = HttpContext.Connection.LocalPort.ToString();
                    var result = await _viewRenderService.RenderToStringAsync("Usuario/DetailsEmail", us_re);
                    usuarioCtrl.SendEmail(us_re,result);
                }


                return RedirectToAction(nameof(Index), "Empleado");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
            finally
            {
                darkManager.CloseConnection();
            }
        }

        // GET: Empleado/Edit
        [AccessView]
        public ActionResult Perfil()
        {
            try
            {
                GetSelects();
                var result = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                if (result == null)
                    return NotFound();
                ViewData["Generos"] = GetDictionary(2, result.IdGenero);
                ViewData["EstadosCiviles"] = GetDictionary(3, result.IdEstadoCivil); ;


                var InforMedica = darkManager.InformacionMedica.GetByColumn("" + result.IdPersona, "IdPersona");
                ViewData["InfoMedica"] = InforMedica;
                if (InforMedica == null)
                {
                    ViewData["Alergias"] = GetDictionary(5, 0);
                    ViewData["TiposSangre"] = GetDictionary(4, 0); ;
                }
                else
                {
                    ViewData["Alergias"] = GetDictionary(5, InforMedica.Alergias);
                    ViewData["TiposSangre"] = GetDictionary(4, InforMedica.TipoSangre); ;
                }


                var InforEmpleado = darkManager.Empleado.GetByColumn("" + result.IdPersona, "IdPersona");
                ViewData["InforEmpleado"] = InforEmpleado;


                if (InforEmpleado == null)
                {
                    ViewData["TipoNomina"] = GetDictionary(6, 0); ;
                    ViewData["EstatusEmpleado"] = GetDictionary(7, 0); ;
                    ViewData["Departamentos"] = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", 0);
                    ViewData["Puestos"] = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", 0);
                    ViewData["Sociedades"] = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", 0);
                }
                else
                {
                    ViewData["TipoNomina"] = GetDictionary(6, InforEmpleado.TipoNomina); ;
                    ViewData["EstatusEmpleado"] = GetDictionary(7, InforEmpleado.IdEstatus); ;
                    ViewData["Departamentos"] = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", InforEmpleado.IdDepartamento);
                    ViewData["Puestos"] = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", InforEmpleado.IdPuesto);
                    ViewData["Sociedades"] = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", InforEmpleado.IdSociedad);
                }



                var PersonaContacto = darkManager.PersonaContacto.Get("" + result.IdPersona, "IdPersona");
                ViewData["PersonaContacto"] = PersonaContacto;
                ViewData["Parentezcos"] = GetDictionary(9, 0);



                darkManager.CloseConnection();
                return View(result);
            }
            catch (GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
            }
            
        }

        private void GetSelects()
        {
            //    Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            //Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            //Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            //Parentezcos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 9, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        private SelectList GetDictionary(int id, int IdSelected)
        {
            return new SelectList(darkManager.CatalogoOpcionesValores.Get("" + id, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IdSelected);
        }


        #endregion
    }
}
