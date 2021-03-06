﻿using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace GPSInformation.Controllers
{
    public class EvaluacionCtrl
    {
        #region Atributos
        private int IdUsuario;
        public DarkManager darkManager { get; internal set; }
        #endregion

        #region Constructores
        public EvaluacionCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.Evaluacion);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionSeccionPregnts);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionTemplate);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacioSeccion);
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionInstructor);
        }
        public EvaluacionCtrl( DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.Evaluacion);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionSeccionPregnts);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionTemplate);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacioSeccion);
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionRespuestas);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionEmpleado);
            this.darkManager.LoadObject(GpsManagerObjects.Departamento);
            this.darkManager.LoadObject(GpsManagerObjects.EvaluacionInstructor);
        }
        #endregion

        #region Metodos
        public IEnumerable<CatalogoOpcionesValores> GetModalidades()
        {
            return darkManager.CatalogoOpcionesValores.Get("1017", "IdCatalogoOpciones").OrderBy(a => a.Descripcion);
        }
        public IEnumerable<EvaluacionTemplate> GetModelos()
        {
            return darkManager.EvaluacionTemplate.Get().OrderBy(a => a.Nombre);
        }
        public IEnumerable<View_empleado> GetEmpleados()
        {
            return darkManager.View_empleado.Get().Where(a => a.IdEstatus == 19 || a.IdEstatus == 18).OrderBy(a => a.NombreCompleto);
        }

        public IEnumerable<Departamento> GetDepartamentos()
        {
            return darkManager.Departamento.Get().OrderBy(a => a.Nombre);
        }
        public EvaluacionRespuestas GetRespuesta(int Idevaluacion, int IdPersona, int Idpregunta)
        {
            return darkManager.EvaluacionRespuestas.GetOpenquerys($"where IdEvaluacion = '{Idevaluacion}' and IdPersona = '{IdPersona}' and IdEvaluacionSeccionPregnts = '{Idpregunta}'");
        }
        public View_empleado GetEmpleado(int idPersona)
        {
            return darkManager.View_empleado.Get(idPersona);
        }
        public IEnumerable<EvaluacionEmpleado> GetParticipantes(int IdEvaluacion)
        {
            var Empl_re = darkManager.EvaluacionEmpleado.Get("" + IdEvaluacion, "IdEvaluacion");
            Empl_re.ForEach(a => {
                a.EmpleadoDatos = darkManager.View_empleado.Get(a.IdPersona);
            });
            return Empl_re.OrderBy(a => a.Enviada);
        }

        public IEnumerable<EvaluacionRespuestas> GetRespuestas(int IdEvaluacion, int IdPersona)
        {
            return darkManager.EvaluacionRespuestas.GetList(
                "IdEvaluacion", "" + IdEvaluacion,
                "IdPersona", "" + IdPersona
            );
        }

        public IEnumerable<EvaluacionRespuestas> GetRespuestas(int IdEvaluacion)
        {
            return darkManager.EvaluacionRespuestas.Get(
                "" + IdEvaluacion, "IdEvaluacion"
            );
        }

        public IEnumerable<EvaluacioSeccion> GetPreguntas(int idEvaluacion)
        {
            var Evaluacion_re = darkManager.Evaluacion.Get(idEvaluacion);
            var Secciones_re = darkManager.EvaluacioSeccion.Get(Evaluacion_re.IdEvaluacionTemplate + "", "IdEvaluacionTemplate");
            Secciones_re.ForEach(a =>
            {
                a.Preguntas = darkManager.EvaluacionSeccionPregnts.Get("" + a.IdEvaluacioSeccion, "IdEvaluacioSeccion");
            });
            return Secciones_re;
        }

        public List<Evaluacion> GetEvaluacions(int IdPersona)
        {
            List<Evaluacion> evaluacions = new List<Evaluacion>();
            var EvaEmp_re = darkManager.EvaluacionEmpleado.Get("" + IdPersona, "IdPersona");
            EvaEmp_re.ForEach(a => {
                var Evaluacion_re = darkManager.Evaluacion.Get(a.IdEvaluacion);
                Evaluacion_re.PersonaName = "";
                Evaluacion_re.ModeloName = darkManager.EvaluacionTemplate.Get(Evaluacion_re.IdEvaluacionTemplate).Nombre;
                Evaluacion_re.ModalidadName = darkManager.CatalogoOpcionesValores.Get(Evaluacion_re.IdModalidad).Descripcion;
                Evaluacion_re.EvaluacionEmpleado = darkManager.EvaluacionEmpleado.Get("IdPersona", "" + IdPersona, "IdEvaluacion", a.IdEvaluacion + "");

                //extraer instructores
                Evaluacion_re.IdEmpleados = new List<int>();
                darkManager.EvaluacionInstructor.Get("" + Evaluacion_re.IdEvaluacion, "IdEvaluacion").ForEach(emp => {
                    Evaluacion_re.IdEmpleados.Add(emp.IdPersona);
                    Evaluacion_re.PersonaName += darkManager.View_empleado.Get(emp.IdPersona).NombreCompleto + ", ";
                });


                evaluacions.Add(Evaluacion_re);
            });

            return evaluacions;
        }

        public Evaluacion GetEvaluacion(int IdPersona, int IdEvaluacion)
        {
            var EvaEmp_re = darkManager.EvaluacionEmpleado.Get(
                "IdPersona", "" + IdPersona,
                "IdEvaluacion", "" + IdEvaluacion);
            if(EvaEmp_re == null)
            {
                throw new Exceptions.GpExceptions(string.Format("La evaluacion E{0:0000} no fue encontrada", IdEvaluacion));
            }
            var Evaluacion_re = darkManager.Evaluacion.Get(IdEvaluacion);
            Evaluacion_re.PersonaName = "";
            Evaluacion_re.ModeloName = darkManager.EvaluacionTemplate.Get(Evaluacion_re.IdEvaluacionTemplate).Nombre;
            Evaluacion_re.ModalidadName = darkManager.CatalogoOpcionesValores.Get(Evaluacion_re.IdModalidad).Descripcion;

            //extraer instructores
            Evaluacion_re.IdEmpleados = new List<int>();
            darkManager.EvaluacionInstructor.Get("" + Evaluacion_re.IdEvaluacion, "IdEvaluacion").ForEach(emp => {
                Evaluacion_re.IdEmpleados.Add(emp.IdPersona);
                Evaluacion_re.PersonaName += darkManager.View_empleado.Get(emp.IdPersona).NombreCompleto + ", ";
            });


            return Evaluacion_re;
        }

        public IEnumerable<Evaluacion> Get()
        {
            var List_re = darkManager.Evaluacion.Get();
            List_re.ForEach(Evaluacion_re => {
                Evaluacion_re.PersonaName = "";
                Evaluacion_re.ModeloName = darkManager.EvaluacionTemplate.Get(Evaluacion_re.IdEvaluacionTemplate).Nombre;
                Evaluacion_re.ModalidadName = darkManager.CatalogoOpcionesValores.Get(Evaluacion_re.IdModalidad).Descripcion;

                //extraer instructores
                Evaluacion_re.IdEmpleados = new List<int>();
                darkManager.EvaluacionInstructor.Get("" + Evaluacion_re.IdEvaluacion, "IdEvaluacion").ForEach(emp => {
                    Evaluacion_re.IdEmpleados.Add(emp.IdPersona);
                    Evaluacion_re.PersonaName += darkManager.View_empleado.Get(emp.IdPersona).NombreCompleto + ", ";
                });


            });
            return List_re.OrderBy(a => a.Creada);
        }

        public EvaluacionEmpleado GetEvaluacionEmpleado(int IdPersona, int IdEvaluacion)
        {
            var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                    "IdEvaluacion", "" + IdEvaluacion,
                    "IdPersona", "" + IdPersona);
            return Participantes_re;
        }

        public Evaluacion Get(int idEvaluacion)
        {
            var Evaluacion_re = darkManager.Evaluacion.Get(idEvaluacion);
            Evaluacion_re.PersonaName = "";
            Evaluacion_re.ModeloName = darkManager.EvaluacionTemplate.Get(Evaluacion_re.IdEvaluacionTemplate).Nombre;
            Evaluacion_re.ModalidadName = darkManager.CatalogoOpcionesValores.Get(Evaluacion_re.IdModalidad).Descripcion;
            //extraer instructores
            Evaluacion_re.IdEmpleados = new List<int>();
            darkManager.EvaluacionInstructor.Get("" + Evaluacion_re.IdEvaluacion, "IdEvaluacion").ForEach(emp => {
                Evaluacion_re.IdEmpleados.Add(emp.IdPersona);
                Evaluacion_re.PersonaName += darkManager.View_empleado.Get(emp.IdPersona).NombreCompleto + ", ";
                });
            return Evaluacion_re;
        }

        public void Create(Evaluacion evaluacion)
        {
            darkManager.StartTransaction();
            try
            {
                evaluacion.Actualizada = DateTime.Now;
                evaluacion.Creada = DateTime.Now;
                darkManager.Evaluacion.Element = evaluacion;
                if (!darkManager.Evaluacion.Add())
                    throw new Exceptions.GpExceptions("Error al crear la evaluación");

                if(evaluacion.IsInterno == false)
                {
                    AddParticipantes(darkManager.Evaluacion.GetLastId(), evaluacion.IdEmpleados);
                }

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void Update(Evaluacion evaluacion)
        {
            darkManager.StartTransaction();
            try
            {
                evaluacion.Actualizada = DateTime.Now;
                darkManager.Evaluacion.Element = evaluacion;
                if (!darkManager.Evaluacion.Update())
                    throw new Exceptions.GpExceptions("Error al crear la evaluación");

               
                if (evaluacion.IsInterno == false)
                {
                    AddParticipantes(evaluacion.IdEvaluacion, evaluacion.IdEmpleados);
                }

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void AddParticupante(EvaluacionEmpleado evaluacionEmpleado)
        {
            darkManager.StartTransaction();
            try
            {
                var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                    "IdEvaluacion", "" + evaluacionEmpleado.IdEvaluacion, 
                    "IdPersona", "" + evaluacionEmpleado.IdPersona);
                if(Participantes_re == null)
                {
                    evaluacionEmpleado.Enviada = DateTime.Now;
                    evaluacionEmpleado.Respondio = false;
                    darkManager.EvaluacionEmpleado.Element = evaluacionEmpleado;
                    if (!darkManager.EvaluacionEmpleado.Add())
                    {
                        throw new Exceptions.GpExceptions("Error al enviar evaluación");
                    }
                }
                else
                {

                }
                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void AddParticupantes(EvaluacionEmpleado evaluacionEmpleado)
        {
            darkManager.StartTransaction();
            try
            {
                if (evaluacionEmpleado.Participantes == null)
                {
                    throw new Exceptions.GpExceptions("Por favor selecciona a los participantes");
                }
                evaluacionEmpleado.Participantes.ForEach(parti => {
                    var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                        "IdEvaluacion", "" + evaluacionEmpleado.IdEvaluacion,
                        "IdPersona", "" + parti);
                    if (Participantes_re == null)
                    {
                        evaluacionEmpleado.Enviada = DateTime.Now;
                        evaluacionEmpleado.Respondio = false;
                        evaluacionEmpleado.IdPersona = parti;
                        darkManager.EvaluacionEmpleado.Element = evaluacionEmpleado;
                        if (!darkManager.EvaluacionEmpleado.Add())
                        {
                            throw new Exceptions.GpExceptions("Error al enviar evaluación");
                        }
                    }

                });

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        public void Deleteparticipantes(List<int> Empleados, int IdEvaluacion)
        {
            darkManager.StartTransaction();
            try
            {
                if (Empleados == null)
                {
                    throw new Exceptions.GpExceptions("Por favor selecciona a los participantes");
                }
                Empleados.ForEach(parti => {
                    var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                        "IdEvaluacion", "" + IdEvaluacion,
                        "IdPersona", "" + parti);
                    if (Participantes_re != null)
                    {
                        darkManager.EvaluacionEmpleado.Element = Participantes_re;
                        if (!darkManager.EvaluacionEmpleado.Delete())
                        {
                            throw new Exceptions.GpExceptions("Error al eliminar evaluación");
                        }

                        var respuestas = darkManager.EvaluacionRespuestas.GetList(
                        "IdEvaluacion", "" + IdEvaluacion,
                        "IdPersona", "" + parti);

                        respuestas.ForEach(re =>
                        {
                            darkManager.EvaluacionRespuestas.Element = re;
                            if (!darkManager.EvaluacionRespuestas.Delete())
                            {
                                throw new Exceptions.GpExceptions("Error al eliminar respuestas");
                            }
                        });
                    }

                });

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        public List<View_empleado> Adddepartamentos(List<int> Departamentos, int IdEvaluacion)
        {
            darkManager.StartTransaction();
            try
            {
                if (Departamentos == null)
                {
                    throw new Exceptions.GpExceptions("Por favor selecciona a los departamentos");
                }
                if (IdEvaluacion == 0)
                {
                    throw new Exceptions.GpExceptions("Por favor selecciona una evaluacion");
                }


                var empleados = darkManager.View_empleado.GetIn(Departamentos.ToArray(), "IdDepartamento");

                empleados.ForEach(parti =>
                {
                    var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                        "IdEvaluacion", "" + IdEvaluacion,
                        "IdPersona", "" + parti.IdPersona);
                    if (Participantes_re == null)
                    {

                        if(parti.IdEstatus != 20)
                        {
                            EvaluacionEmpleado evaluacionEmpleado = new EvaluacionEmpleado
                            {
                                Enviada = DateTime.Now,
                                Respondio = false,
                                IdPersona = parti.IdPersona,
                                IdEvaluacion = IdEvaluacion
                            };
                            darkManager.EvaluacionEmpleado.Element = evaluacionEmpleado;
                            if (!darkManager.EvaluacionEmpleado.Add())
                            {
                                throw new Exceptions.GpExceptions("Error al enviar evaluación");
                            }
                        }
                    }

                });

                darkManager.Commit();

                return empleados;
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void EnviarCorreo(string body, int IdEvaluacion, int IdPersona )
        {
            try
            {
                var Evaluacion_re = darkManager.Evaluacion.Get(IdEvaluacion);
                var Emplead_re = darkManager.View_empleado.Get(IdPersona);
                darkManager.EmailServ_.AddListTO(Emplead_re.Correo);

                this.darkManager.LoadObject(GpsManagerObjects.Usuario);
                this.darkManager.LoadObject(GpsManagerObjects.AccesosSistema);

                var acceos = darkManager.AccesosSistema.GetList("IdSubModulo", "45", "TieneAcceso", "1");
                acceos.ForEach(acc => {
                    var usuario = darkManager.Usuario.Get(acc.IdUsuario);

                    if (usuario != null)
                    {
                        var empleado = darkManager.View_empleado.Get(usuario.IdPersona);
                        if (empleado != null)
                        {
                            darkManager.EmailServ_.AddListCC(empleado.Correo);
                        }
                    }
                });
                darkManager.EmailServ_.Send(body, string.Format("Evaluacion: {0}", Evaluacion_re.Nombre));
                darkManager.RestartEmail();
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteParticupante(EvaluacionEmpleado evaluacionEmpleado)
        {
            darkManager.StartTransaction();
            try
            {
                var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                    "IdEvaluacion", "" + evaluacionEmpleado.IdEvaluacion,
                    "IdPersona", "" + evaluacionEmpleado.IdPersona);
                if (Participantes_re != null)
                {
                    darkManager.EvaluacionEmpleado.Element = Participantes_re;
                    if (!darkManager.EvaluacionEmpleado.Delete())
                    {
                        throw new Exceptions.GpExceptions("Error al enviar evaluación");
                    }
                }
                else
                {

                }
                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        public void AddRespuestas(List<EvaluacioSeccion> list, int IdEvaluacion, int IdPersona)
        {
            darkManager.StartTransaction();
            try
            {
                if (list == null)
                {
                    throw new Exceptions.GpExceptions("Sin respuestas");
                }
                if (list.Count == 0)
                {
                    throw new Exceptions.GpExceptions("Sin respuestas");
                }

                var Participantes_re = darkManager.EvaluacionEmpleado.Get(
                    "IdEvaluacion", "" + IdEvaluacion,
                    "IdPersona", "" + IdPersona);

                if(Participantes_re == null)
                {
                    throw new Exceptions.GpExceptions("No estas activo para esta evaluación");
                }

                if (Participantes_re.Respondio)
                {
                    throw new Exceptions.GpExceptions("Esta evaluacion ya ha sido respondida");
                }


                list.ForEach(seccion => {
                    seccion.Preguntas.ForEach(Pregunta => {
                        var OldRespuesta = darkManager.EvaluacionRespuestas.GetList(
                            "IdPersona", "" + Pregunta.Respuesta.IdPersona,
                            "IdEvaluacion", "" + Pregunta.Respuesta.IdEvaluacion);
                        var res = OldRespuesta.Find(a =>
                               a.IdPersona == Pregunta.Respuesta.IdPersona
                           && a.IdEvaluacion == Pregunta.Respuesta.IdEvaluacion
                           && a.IdEvaluacionSeccionPregnts == Pregunta.IdEvaluacionSeccionPregnts);
                        if (res == null)
                        {
                            Pregunta.Respuesta.IdPersona = IdPersona;
                            darkManager.EvaluacionRespuestas.Element = Pregunta.Respuesta;
                            if (!darkManager.EvaluacionRespuestas.Add())
                            {
                                throw new Exceptions.GpExceptions("Error al guardar la respuesta");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(res.Respuesta))
                            {
                                res.IdPersona = IdPersona;
                                res.Respuesta = Pregunta.Respuesta.Respuesta;
                                darkManager.EvaluacionRespuestas.Element = res;
                                if (!darkManager.EvaluacionRespuestas.Update())
                                {
                                    throw new Exceptions.GpExceptions("Error al actualizar la respuesta");
                                }
                            }
                        }
                    });
                });
                Participantes_re.Respondio = true;
                Participantes_re.Contestada = DateTime.Now;
                darkManager.EvaluacionEmpleado.Element = Participantes_re;
                if (!darkManager.EvaluacionEmpleado.Update())
                {
                    throw new Exceptions.GpExceptions("Error al actualizar la evaluación");
                }

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }

        }

        public void Reactivar(int IdEvaluacion, List<int> Partcipantes)
        {
            darkManager.StartTransaction();
            try
            {
                Partcipantes.ForEach(parti => {
                    var result = darkManager.EvaluacionEmpleado.GetOpenquerys($"where IdEvaluacion = '{IdEvaluacion}' and IdPersona = '{parti}'");
                    if (result != null)
                    {
                        darkManager.EvaluacionEmpleado.Element = result;
                        darkManager.EvaluacionEmpleado.Element.Respondio = false;
                        if (!darkManager.EvaluacionEmpleado.Update())
                        {
                            throw new Exceptions.GpExceptions("Error al reactivar la evaluacion");
                        }
                    }
                });
                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
            
        }
        private void AddParticipantes(int IdEvaluacion, List<int> Partcipantes)
        {
            


            darkManager.EvaluacionInstructor.Get("" + IdEvaluacion, "IdEvaluacion").ForEach(emp => {
                darkManager.EvaluacionInstructor.Element = emp;
                darkManager.EvaluacionInstructor.Delete();
            });

            Partcipantes.ForEach(IdPersona => {
                var EvaluacionInstructor = new EvaluacionInstructor();
                EvaluacionInstructor.IdEvaluacion = IdEvaluacion;
                EvaluacionInstructor.IdPersona = IdPersona;
                darkManager.EvaluacionInstructor.Element = EvaluacionInstructor;
                if (!darkManager.EvaluacionInstructor.Add())
                {
                    throw new Exceptions.GpExceptions("Error al agregar respuestas");
                }
            });


            
        }

        public void Terminar(bool closeDark = true)
        {
            darkManager.CloseConnection();
            if (closeDark)
            {
                darkManager = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion
    }
}
