using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GestionPersonal.Service;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class EvaluacionController : Controller
    {
        private EvaluacionCtrl EvaluacionCtrl;
        private readonly IViewRenderService _viewRenderService;

        public EvaluacionController(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
            EvaluacionCtrl = new EvaluacionCtrl(new DarkManager(configuration));
        }

        // GET: EvaluacionController
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Index()
        {
            try
            {
                return View(EvaluacionCtrl.Get());
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
           
        }

        // GET: EvaluacionController/Details/5
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Details(int id)
        {
            try
            {
                ViewData["Preguntas"] = EvaluacionCtrl.GetPreguntas(id);
                ViewData["Participantes"] = EvaluacionCtrl.GetParticipantes(id).ToList();
                ViewData["Respuestas"] = EvaluacionCtrl.GetRespuestas(id).ToList();
                ViewData["Empleados"] = new SelectList(EvaluacionCtrl.GetEmpleados().ToList(), "IdPersona", "NombreCompleto");
                ViewData["Departamentos"] = new SelectList(EvaluacionCtrl.GetDepartamentos().ToList(), "IdDepartamento", "Nombre");
                
                return View(EvaluacionCtrl.Get(id));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        public ActionResult EmailDetails(int id, int IdPersona)
        {
            try
            {
                return View(new EvaluacionEmpleados
                {
                    View_empleado = EvaluacionCtrl.GetEmpleado(IdPersona),
                    Evaluacion = EvaluacionCtrl.Get(id)
                });
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        // GET: EvaluacionController/Create
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Create()
        {
            try
            {
                ViewData["Modalidades"] = new SelectList(EvaluacionCtrl.GetModalidades().ToList(), "IdCatalogoOpcionesValores", "Descripcion");
                ViewData["Modelos"] = new SelectList(EvaluacionCtrl.GetModelos().ToList(), "IdEvaluacionTemplate", "Nombre");
                ViewData["Empleados"] = new SelectList(EvaluacionCtrl.GetEmpleados().ToList(), "IdPersona", "NombreCompleto");
                return View();
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
            
        }

        // POST: EvaluacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Create(Evaluacion Evaluacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Evaluacion);
                }

                if(Evaluacion.IdEmpleados == null || Evaluacion.IdEmpleados != null && Evaluacion.IdEmpleados.Count == 0)
                {
                    ModelState.AddModelError("IdEmpleados", "Por favor selecciona al menos un instructor");
                    return View(Evaluacion);
                }

                EvaluacionCtrl.Create(Evaluacion);
                return RedirectToAction(nameof(Index));
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Evaluacion);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        // GET: EvaluacionController/Edit/5
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Edit(int id)
        {
            try
            {
                var Evaluacion = EvaluacionCtrl.Get(id);
                ViewData["Modalidades"] = new SelectList(EvaluacionCtrl.GetModalidades().ToList(), "IdCatalogoOpcionesValores", "Descripcion", Evaluacion.IdModalidad);
                ViewData["Modelos"] = new SelectList(EvaluacionCtrl.GetModelos().ToList(), "IdEvaluacionTemplate", "Nombre", Evaluacion.IdEvaluacionTemplate);
                ViewData["Empleados"] = new SelectList(EvaluacionCtrl.GetEmpleados().ToList(), "IdPersona", "NombreCompleto", Evaluacion.IdPersona);
                return View(Evaluacion);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        // POST: EvaluacionController/Edit/5
        [AccessMultipleView(IdAction = new int[] { 37 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evaluacion Evaluacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Evaluacion);
                }
                if (Evaluacion.IdEmpleados == null || Evaluacion.IdEmpleados != null && Evaluacion.IdEmpleados.Count == 0)
                {
                    ModelState.AddModelError("IdEmpleados", "Por favor selecciona al menos un instructor");
                    return View(Evaluacion);
                }
                EvaluacionCtrl.Update(Evaluacion);
                return RedirectToAction(nameof(Index));
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Evaluacion);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        // GET: EvaluacionController/Delete/5
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Delete(int id)
        {
            try
            {
                var Evaluacion = EvaluacionCtrl.Get(id);
                ViewData["Modalidades"] = new SelectList(EvaluacionCtrl.GetModalidades().ToList(), "IdCatalogoOpcionesValores", "Descripcion", Evaluacion.IdModalidad);
                ViewData["Modelos"] = new SelectList(EvaluacionCtrl.GetModelos().ToList(), "IdEvaluacionTemplate", "Nombre", Evaluacion.IdEvaluacionTemplate);
                ViewData["Empleados"] = new SelectList(EvaluacionCtrl.GetEmpleados().ToList(), "IdPersona", "NombreCompleto", Evaluacion.IdPersona);
                return View(Evaluacion);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        // POST: EvaluacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public async Task<ActionResult> AddParticipante(EvaluacionEmpleado evaluacionEmpleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(evaluacionEmpleado);
                }

                EvaluacionCtrl.AddParticupante(evaluacionEmpleado);

                var result = await _viewRenderService.RenderToStringAsync("Evaluacion/EmailDetails", new EvaluacionEmpleados
                {
                    View_empleado = EvaluacionCtrl.GetEmpleado(evaluacionEmpleado.IdPersona),
                    Evaluacion = EvaluacionCtrl.Get(evaluacionEmpleado.IdEvaluacion)
                });
                EvaluacionCtrl.EnviarCorreo(result, evaluacionEmpleado.IdEvaluacion, evaluacionEmpleado.IdPersona);

                return RedirectToAction(nameof(Details), new { id = evaluacionEmpleado.IdEvaluacion});
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(evaluacionEmpleado);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public async Task<ActionResult> AddParticipantes(EvaluacionEmpleado evaluacionEmpleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(evaluacionEmpleado);
                }
                EvaluacionCtrl.AddParticupantes(evaluacionEmpleado);

                evaluacionEmpleado.Participantes.ForEach(async parti => 
                {
                    var result = await _viewRenderService.RenderToStringAsync("Evaluacion/EmailDetails", new EvaluacionEmpleados
                    {
                        View_empleado = EvaluacionCtrl.GetEmpleado(parti),
                        Evaluacion = EvaluacionCtrl.Get(evaluacionEmpleado.IdEvaluacion)
                    });
                    EvaluacionCtrl.EnviarCorreo(result, evaluacionEmpleado.IdEvaluacion, parti);
                });

                return RedirectToAction(nameof(Details), new { id = evaluacionEmpleado.IdEvaluacion });
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(evaluacionEmpleado);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 37 })]
        public async Task<ActionResult> DeleteParticipantes([FromBody] EvaluacionEmple evaluacionEmpleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Algunos campos estan vacios");
                }
                EvaluacionCtrl.Deleteparticipantes(evaluacionEmpleado.Empleados, evaluacionEmpleado.IdEvaluacion);

                return Ok("PArticipantes eliminados");
            }
            catch (GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult DeleteParticipante(int IdEvaluacion, int IdPersona)
        {
            try
            {

                EvaluacionCtrl.DeleteParticupante(new EvaluacionEmpleado { IdEvaluacion = IdEvaluacion, IdPersona = IdPersona });
                return RedirectToAction(nameof(Details), new { id = IdEvaluacion });
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 39 })]
        public ActionResult MisEvaluaciones(string Mode)
        {
            try
            {
                ViewData["Mode"] = string.IsNullOrEmpty(Mode) ? "list" : "card";
                return View(EvaluacionCtrl.GetEvaluacions((int)HttpContext.Session.GetInt32("user_id")));
            }
            catch (GpExceptions ex)
            {
                return View(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 39 })]
        public ActionResult Responder(int id)
        {
            try
            {
                var Eva_re = EvaluacionCtrl.GetEvaluacion((int)HttpContext.Session.GetInt32("user_id"), id);
                if(Eva_re == null)
                {
                    return NotFound();
                }
                //var preguntas_red = EvaluacionCtrl.GetPreguntas(id).ToList();
                //preguntas_red.ForEach(a => {
                //    a.Preguntas.ForEach(p => {
                //        p.Respuesta = new EvaluacionRespuestas { 
                //            IdPersona = (int)HttpContext.Session.GetInt32("user_id"),
                //            IdEvaluacion = id,
                //            IdEvaluacionSeccionPregnts = p.IdEvaluacionSeccionPregnts
                //        };
                //    });
                //});
                //Eva_re.secciones = preguntas_red;
                ViewData["EvaluacionEmpleado"] = EvaluacionCtrl.GetEvaluacionEmpleado((int)HttpContext.Session.GetInt32("user_id"), id);
                return View(Eva_re);
            }
            catch (GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 39 })]
        public ActionResult DataResponder(int id)
        {
            try
            {
                
                var preguntas_red = EvaluacionCtrl.GetPreguntas(id).ToList();
                preguntas_red.ForEach(a => {
                    a.Preguntas.ForEach(p => {
                        p.Respuesta = new EvaluacionRespuestas
                        {
                            IdPersona = (int)HttpContext.Session.GetInt32("user_id"),
                            IdEvaluacion = id,
                            IdEvaluacionSeccionPregnts = p.IdEvaluacionSeccionPregnts
                        };
                    });
                });
                return Ok(preguntas_red);
            }
            catch (GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpPost]
        [AccessMultipleView(IdAction = new int[] { 39 })]
        public ActionResult DataResponders([FromBody]EvaluacionEmp evaluacionEmp)
        {
            try
            {
                EvaluacionCtrl.AddRespuestas(evaluacionEmp.list, evaluacionEmp.IdEvaluacion, (int)HttpContext.Session.GetInt32("user_id"));
                return Ok("Evaluacion respondida existosamente");
            }
            catch (GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }

        [HttpPost]
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult AddDepartamentos(List<int> Departamentos, int IdEvaluacion)
        {
            try
            {
                var empleados = EvaluacionCtrl.Adddepartamentos(Departamentos, IdEvaluacion);
                empleados.ForEach(async parti =>
                {
                    var result = await _viewRenderService.RenderToStringAsync("Evaluacion/EmailDetails", new EvaluacionEmpleados
                    {
                        View_empleado = EvaluacionCtrl.GetEmpleado(parti.IdPersona),
                        Evaluacion = EvaluacionCtrl.Get(IdEvaluacion)
                    });
                    EvaluacionCtrl.EnviarCorreo(result, IdEvaluacion, parti.IdPersona);
                });

                return RedirectToAction(nameof(Details), new { id = IdEvaluacion });
            }
            catch (GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                EvaluacionCtrl.Terminar();
                EvaluacionCtrl = null;
            }
        }
    }
}
