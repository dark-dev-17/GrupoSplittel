using GPSInformation.Views;
using GPSInformation.Reportes.ProduccionV3;
using System;
using System.Collections.Generic;
using System.Text;
using GPSInformation.Models.Produccion;
using System.Linq;
using GPSInformation.Tools;

namespace GPSInformation.Controllers
{
    public class ProduccionV4Ctrl
    {
        #region Atributos
        public DarkManager darkManager;
        #endregion

        #region Constructores
        public ProduccionV4Ctrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.View_empleadoEnsamble);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoHorario);

            this.darkManager.LoadObject(GpsManagerObjects.GrupoCambios);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoArreglo);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoProdIncidencia);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoProdCorte);
            this.darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
            this.darkManager.LoadObject(GpsManagerObjects.Usuario);
            this.darkManager.LoadObject(GpsManagerObjects.SubModulo);

            darkManager.OpenConnectionAcces();
            darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);
        }
        #endregion

        #region Metodos
        public void ChangePermisos(List<PermisosBloq> Permisos)
        {
            Permisos.ForEach(a => {
                var permiso = darkManager.AccesosSistema.GetOpenquerys($"where " +
                        $"IdUsuario = {a.IdUsuario} " +
                        $"and IdSubModulo = {a.IdSubModulo}");
                if(permiso != null)
                {
                    permiso.TieneAcceso = a.Autorization;
                    permiso.Modificado = DateTime.Now;
                    darkManager.AccesosSistema.Element = permiso;
                    if (!darkManager.AccesosSistema.Update())
                    {
                        throw new Exceptions.GpExceptions("Error al guardar los cambios");
                    }
                }
                else
                {
                    permiso = new Models.AccesosSistema { 
                        IdUsuario = a.IdUsuario,
                        IdSubModulo = a.IdSubModulo,
                        TieneAcceso = a.Autorization,
                        Modificado = DateTime.Now,
                        Forzado = false
                    };
                    darkManager.AccesosSistema.Element = permiso;
                    if (!darkManager.AccesosSistema.Add())
                    {
                        throw new Exceptions.GpExceptions("Error al guardar los cambios");
                    }

                }
            });
        }
        public List<PermisosBloq> VerPermisos(int IdPersona_)
        {
            List<PermisosBloq> permis = new List<PermisosBloq>();

            var usuario = darkManager.Usuario.GetOpenquerys($"where IdPersona = {IdPersona_}");
            var permisos = darkManager.SubModulo.GetOpenquery($"Where IdSubModulo in (53,55,56,57,58)");
            permisos.ForEach(a => {

                var permiso = darkManager.AccesosSistema.GetOpenquerys($"where " +
                    $"IdUsuario = {usuario.IdUsuario} " +
                    $"and IdSubModulo = {a.IdSubModulo}");

                permis.Add(new PermisosBloq {
                    IdUsuario = usuario.IdUsuario,
                    IdSubModulo = a.IdSubModulo,
                    Descripcion = a.Nombre,
                    Autorization = permiso is null ? false : permiso.TieneAcceso
                });

            });
            return permis;
        }
        /// <summary>
        /// Eliminar evento de incidencia
        /// </summary>
        /// <param name="IdGrupoProdIncidencia"></param>
        /// <param name="IdPersona"></param>
        public void DeleteInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            var res = darkManager.GrupoProdIncidencia.Get(IdGrupoProdIncidencia);
            if(res is null)
            {
                throw new Exceptions.GpExceptions("Error, no se encontro la incidencia");
            }
            darkManager.GrupoProdIncidencia.Element = res;
            if (!darkManager.GrupoProdIncidencia.Delete())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        /// <summary>
        /// Detalles de incidencia
        /// </summary>
        /// <param name="IdGrupoProdIncidencia"></param>
        /// <param name="IdPersona"></param>
        /// <returns></returns>
        public GrupoProdIncidencia DetailsInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            var res = darkManager.GrupoProdIncidencia.Get(IdGrupoProdIncidencia);
            return res;
        }
        /// <summary>
        /// Registrar incidencias
        /// </summary>
        /// <param name="GrupoProdIncidencia"></param>
        public void RegisterIncidencia(GrupoProdIncidencia GrupoProdIncidencia)
        {
            if(GrupoProdIncidencia is null)
                throw new Exceptions.GpExceptions("Datos incorrectos");
            if (string.IsNullOrEmpty(GrupoProdIncidencia.Comentarios))
                throw new Exceptions.GpExceptions("Por favor introduce algun comentario");

            darkManager.GrupoProdIncidencia.Element = GrupoProdIncidencia;
            if (GrupoProdIncidencia.IdGrupoProdIncidencia == 0)
            {
                darkManager.GrupoProdIncidencia.Element.Creado = DateTime.Now;
                darkManager.GrupoProdIncidencia.Element.Modificado = DateTime.Now;
                if (!darkManager.GrupoProdIncidencia.Add())
                {
                    throw new Exceptions.GpExceptions("Error al guardar los cambios");
                }
            }
            else
            {
                darkManager.GrupoProdIncidencia.Element.Modificado = DateTime.Now;
                if (!darkManager.GrupoProdIncidencia.Update())
                {
                    throw new Exceptions.GpExceptions("Error al guardar los cambios");
                }
            }
        }
        /// <summary>
        /// Eliminar evento
        /// </summary>
        /// <param name="IdEvent"></param>
        /// <param name="IdPersona"></param>
        public void DeleteArreglo(int IdEvent, int IdPersona)
        {
            darkManager.GrupoArreglo.Element = darkManager.GrupoArreglo.GetOpenquerys($"where IdPersona = {IdPersona} and IdGrupoArreglo = {IdEvent}");
            if(darkManager.GrupoArreglo.Element is null)
            {
                throw new Exceptions.GpExceptions("No se encontro el evento de tipo arreglo");
            }

            if (!darkManager.GrupoArreglo.Delete())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        /// <summary>
        /// Agregar evento de chequeo
        /// </summary>
        /// <param name="IdEvent"></param>
        /// <param name="IdPersona"></param>
        /// <param name="Fecha"></param>
        /// <param name="Comentarios"></param>
        public void AddArreglo(int IdEvent, int IdPersona, DateTime Fecha, string Comentarios)
        {
            darkManager.GrupoArreglo.Element = new GrupoArreglo
            {
                IdEvent = IdEvent,
                IdPersona = IdPersona,
                FechaHora = Fecha,
                Comentarios = Comentarios,
                EsIgnorado = true,
                Creado = DateTime.Now,
                Modificado = DateTime.Now,
            };
            if (string.IsNullOrEmpty(Comentarios))
                throw new Exceptions.GpExceptions("Por favor introduce algun comentario");
            if (!darkManager.GrupoArreglo.Add())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        /// <summary>
        /// Procesar reporte de empleados
        /// </summary>
        /// <param name="Inicio_"></param>
        /// <returns></returns>
        public ReporteProd ProcesarEmpleados(DateTime Inicio_)
        {
            ReporteProd reporteProd = new ReporteProd { 
                Inicio = Inicio_,
                Fin = Inicio_.AddDays(7),
                EmpleadoProds = new List<EmpleadoProd>()
            };
            darkManager.View_empleadoEnsamble.Get().ForEach(a => {
                reporteProd.EmpleadoProds.Add(ProcesarEmpleado(a, 0, Inicio_));
            });
            return reporteProd;
        }
        /// <summary>
        /// Procesar reporte de horas trabajadas por empleado
        /// </summary>
        /// <param name="Empleado"></param>
        /// <param name="IdPersona"></param>
        /// <param name="Inicio_"></param>
        /// <returns></returns>
        public Reportes.ProduccionV3.EmpleadoProd ProcesarEmpleado(View_empleadoEnsamble Empleado, int IdPersona, DateTime Inicio_)
        {
            if(IdPersona != 0)
            {
                Empleado = darkManager.View_empleadoEnsamble.Get(IdPersona);
            }
            EmpleadoProd EmpleadoProd = new EmpleadoProd { 
                IdPersona = Empleado.IdPersona,
                NombreCompleto = Empleado.NombreCompleto,
                NumeroNomina = Empleado.NumeroNomina,
                PuestoNombre = Empleado.PuestoNombre,
                Antiguedad = Empleado.Antiguedad,
                Incio = Inicio_,
                Fin = Inicio_.AddDays(7),
                Accessos = new List<AccessLog>(),
                JornadaGrupos = new List<JornadaGrupo>(),
                GrupoCambios = ListCambiosgrupo(Empleado.IdPersona, Inicio_)
            };

            #region Procesar logs
            var Logs = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                                    $"where tIdentification = '{Empleado.NumeroNomina}' " +
                                    $"AND dtEventReal >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
                                    $"AND dtEventReal <= '{EmpleadoProd.Fin.ToString("yyyy-MM-dd 05:45:00")}' " +
                                    $"AND IdReader = 17",
                                    $"ORDER BY iEmployeeNum , dtEventReal");

            Logs.ForEach(log => {
                //validar ignorar log
                var logIgnore = darkManager.GrupoArreglo.GetOpenquerys($"where IdEvent = '{log.IdAutoEvents}' and IdPersona = { Empleado.IdPersona}");

                EmpleadoProd.Accessos.Add(new AccessLog
                {
                    IdEventChec = log.IdAutoEvents,
                    IdGrupoArreglo = logIgnore != null ? logIgnore.IdGrupoArreglo : 0,
                    Descripcion = logIgnore != null ? logIgnore.Comentarios : log.tDesc,
                    Fecha = log.dtEventReal,
                    Activo = logIgnore is null ? true : false,
                    Forzado = false,
                    IdAccessLog = 0
                });
            });
            #endregion

            #region procesar arreglos a logs
            var arreglos = darkManager.GrupoArreglo.GetOpenquery($"where " +
               $"IdEvent is null and IdPersona = { Empleado.IdPersona} " +
               $"AND FechaHora >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaHora <= '{EmpleadoProd.Fin.ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaHora");

            arreglos.ForEach(arr => {
                EmpleadoProd.Accessos.Add(new AccessLog
                {
                    IdEventChec = 0,
                    Descripcion = arr.Comentarios,
                    Fecha = arr.FechaHora,
                    Activo = true,
                    Forzado = true,
                    IdAccessLog = arr.IdGrupoArreglo
                });
            });
            #endregion

            #region Procesar dias por grupo asginado
            DateTime Corte = DateTime.Parse(EmpleadoProd.Incio.ToString("yyyy-MM-dd 00:00:00"));
            while (Corte <= EmpleadoProd.Fin)
            {
                var UltimoCambio = GetUltimoCambio(Corte, Empleado.IdPersona);

                if (UltimoCambio != null)
                {
                    // obtener dia de acuerdo al grupo
                    var DiaHorario_re = darkManager.GrupoHorario.GetOpenquerys($"where IdGrupo = '{UltimoCambio.IdGrupo}' and dia = '{(int)Corte.DayOfWeek}'");
                    var diaLog = new JornadaGrupo
                    {
                        IdGrupo = DiaHorario_re.IdGrupo,
                        HorasMeta = DiaHorario_re.Horas,
                        TipoJornada = DiaHorario_re.TipoDia,
                        Fecha = Corte + DiaHorario_re.Entrada,
                        ComentariosSistema = $"cambio de turno aplicado desde el dia: {UltimoCambio.Creado.ToString("F")}"
                    };

                    diaLog.Salida = diaLog.Fecha.AddHours(diaLog.HorasMeta);
                    EmpleadoProd.JornadaGrupos.Add(diaLog);
                }

                Corte = Corte.AddDays(1);
            }
            #endregion

            #region Validando entradas
            EmpleadoProd.Accessos = EmpleadoProd.Accessos.OrderBy(a => a.Fecha).ToList();

            int Pos = 1;

            EmpleadoProd.Accessos.ForEach(a => {
                if (a.Activo)
                {
                    a.Position = Pos;
                    Pos++;
                }
            });

            #endregion

            #region procesar horas meta
            DateTime Entrada = DateTime.Parse("0001-01-01 00:00:00");

            if ((EmpleadoProd.Accessos.Count(a => a.Activo) % 2 == 0))
            {
                EmpleadoProd.Accessos.ForEach(a => {
                    if (a.Activo)
                    {
                        if (a.TipoAcceso == TipoAcceso.Entrada)
                        {
                            if (Entrada < a.Fecha)
                            {
                                Entrada = a.Fecha;
                            }
                        }
                        else
                        {
                            EmpleadoProd.HorasReal += Funciones.DifFechashoras((DateTime)a.Fecha, (DateTime)Entrada);
                        }
                    }
                });
            }

            EmpleadoProd.JornadaGrupos.ForEach(a => {
                EmpleadoProd.HorasMeta += a.HorasMeta;
            });
            #endregion

            #region obtener incidencias
            var Incidencias = darkManager.GrupoProdIncidencia.GetOpenquery($"where " +
               $"IdPersona = { Empleado.IdPersona}  and TipoIncidecia = 'Per'" +
               $"AND FechaPermiso >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaPermiso <= '{EmpleadoProd.Fin.ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaPermiso");

            EmpleadoProd.GrupoProdIncidencias = Incidencias;

            var vacaciones = darkManager.GrupoProdIncidencia.GetOpenquery($"where " +
               $"IdPersona = { Empleado.IdPersona}  and TipoIncidecia = 'Vac'" +
               $"AND FechaSalVac >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaSalVac <= '{EmpleadoProd.Fin.ToString("yyyy-MM-dd 05:45:00")}' " +
               $"AND FechaRegVac >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaRegVac <= '{EmpleadoProd.Fin.ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaRegVac");

            vacaciones.ForEach(eve => {
                EmpleadoProd.GrupoProdIncidencias.Add(eve);
            });


            EmpleadoProd.GrupoProdIncidencias.ForEach(a=> { 
                if(a.TipoIncidecia == "Per")
                {
                    var opc = darkManager.CatalogoOpcionesValores.Get(a.TipoPermiso);
                    if(opc != null)
                    {
                        a.NameTipoPermiso = opc.Descripcion;
                    }
                }

                EmpleadoProd.HorasAprobadas += a.HorasJustific;
            });

            #endregion

            EmpleadoProd.GrupoProdCorteAct = darkManager.GrupoProdCorte.GetOpenquerys($"where IdPersona = {Empleado.IdPersona} and FechaCorte = '{EmpleadoProd.Fin.ToString("yyyy-MM-dd")}'");
            EmpleadoProd.GrupoProdCorteLast = darkManager.GrupoProdCorte.GetOpenquerys($"where IdPersona = {Empleado.IdPersona} and FechaCorte = '{EmpleadoProd.Incio.ToString("yyyy-MM-dd")}'");

            return EmpleadoProd;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Corte"></param>
        /// <param name="IdPersona_"></param>
        /// <returns></returns>
        private GrupoCambios GetUltimoCambio(DateTime Corte, int IdPersona_)
        {
            var UltimoCambio = darkManager.GrupoCambios.GetUnicSatatment($"select top 1 * from GrupoCambios where IdPersona = {IdPersona_} and Fecha <= '{Corte.ToString("yyyy-MM-dd")}' order by Fecha desc ");

            if (UltimoCambio != null)
            {
                UltimoCambio.GrupoName = GetNameGrup(UltimoCambio.IdGrupo);
            }
            return UltimoCambio;
        }
        /// <summary>
        /// Listar cambios realizados durante la semana
        /// </summary>
        /// <param name="IdPersona"></param>
        /// <param name="Inicio"></param>
        /// <returns></returns>
        public List<GrupoCambios> ListCambiosgrupo(int IdPersona, DateTime Inicio)
        {
            var Incidencias = darkManager.GrupoCambios.GetOpenquery($"where " +
               $"IdPersona = { IdPersona}  " +
               $"AND Fecha >= '{Inicio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND Fecha <= '{Inicio.AddDays(6).ToString("yyyy-MM-dd 23:59:59")}' ",
               $"Order by Fecha");
            Incidencias.ForEach(a=>a.GrupoName = GetNameGrup(a.IdGrupo));
            return Incidencias;
        }
        /// <summary>
        /// Eliminar un cambio de turno
        /// </summary>
        /// <param name="IdGrupoCambios"></param>
        /// <param name="IdPersona"></param>
        public void DeleteCambio(int IdGrupoCambios, int IdPersona)
        {
            var UltimoCambio = darkManager.GrupoCambios.GetOpenquerys($"where IdPersona = {IdPersona} and IdGrupoCambios = {IdGrupoCambios}");
            if (UltimoCambio is null)
            {
                throw new Exceptions.GpExceptions("Error, no se encontro el cambio");
            }

            darkManager.GrupoCambios.Element = UltimoCambio;
            if (!darkManager.GrupoCambios.Delete())
            {
                throw new Exceptions.GpExceptions("Error al remover el cambio de turno seleccionado");
            }
        }
        /// <summary>
        /// registrar nuevos cambios
        /// </summary>
        /// <param name="grupoCambios"></param>
        public void AddCambio(GrupoCambios grupoCambios)
        {
            if(grupoCambios is null)
            {
                throw new Exceptions.GpExceptions("Error, datos incorrectos");
            }

            
            if(grupoCambios.Fecha < grupoCambios.FechaInicio && grupoCambios.Fecha > grupoCambios.FechaInicio.AddDays(6))
            {
                throw new Exceptions.GpExceptions("Error, por favor introduce una fecha de acuerdo al bloque del reporte");
            }

            var UltimoCambio = darkManager.GrupoCambios.GetOpenquerys($"where IdPersona = {grupoCambios.IdPersona} and Fecha = '{grupoCambios.Fecha.ToString("yyyy-MM-dd")}'");
            if (string.IsNullOrEmpty(grupoCambios.Comentarios))
            {
                throw new Exceptions.GpExceptions("Error, por favor introduce algun comentario");
            }

            if (UltimoCambio != null)
            {
                throw new Exceptions.GpExceptions($"Ya existe un cambio de grupo, el grupo definido fue '{ GetNameGrup(UltimoCambio.IdGrupo)}', por favor elimina dicho cambo y vuelve a intentar");
            }
            grupoCambios.Creado = DateTime.Now;
            grupoCambios.Modificado = DateTime.Now;
            darkManager.GrupoCambios.Element = grupoCambios;
            if (!darkManager.GrupoCambios.Add())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        /// <summary>
        /// Obtener nombre del Grupo
        /// </summary>
        /// <param name="IdGrupo"></param>
        /// <returns></returns>
        public string GetNameGrup(int IdGrupo)
        {
            return IdGrupo == 86 ? "Gris" : IdGrupo == 87 ? "Rojo" : IdGrupo == 88 ? "Verde" : "Sin asginar";
        }
        /// <summary>
        /// Generar corte
        /// </summary>
        /// <param name="IdPersona"></param>
        /// <param name="Corte"></param>
        public void CreateCorte(int IdPersona, DateTime Corte)
        {
            var Procesa = ProcesarEmpleado(new View_empleadoEnsamble(), IdPersona, Corte);
            if(Procesa is null)
            {
                throw new Exceptions.GpExceptions("Error, no se proceso el cliente solicitado");
            }
            var CorteAnterio = darkManager.GrupoProdCorte.GetOpenquerys($"where IdPersona = {IdPersona} and FechaCorte = '{Corte.ToString("yyyy-MM-dd")}'");
            if(CorteAnterio is null)
            {
                throw new Exceptions.GpExceptions($"Error, aun no se ha creado el corte de la semana anterior: semana faltante {Corte.AddDays(-7).ToString("F")}");
            }
            var CorteActual = darkManager.GrupoProdCorte.GetOpenquerys($"where IdPersona = {IdPersona} and FechaCorte = '{Corte.AddDays(7).ToString("yyyy-MM-dd")}'");
            if(CorteActual != null)
            {
                throw new Exceptions.GpExceptions("Error, ya fue creado un corte");
            }

            GrupoProdCorte grupoProdCorte = new GrupoProdCorte { 
                IdPersona = IdPersona,
                Comentarios = "Generacion del corte",
                Creado = DateTime.Now,
                FechaCorte = Corte.AddDays(7),
                Modificado = DateTime.Now,
                HorasJusti = Procesa.HorasAprobadas,
                HorasMeta = Procesa.HorasMeta,
                HorasReal = Procesa.HorasReal,
                Score = CorteAnterio.Score - (Procesa.HorasScore)
            };

            darkManager.GrupoProdCorte.Element = grupoProdCorte;
            if (!darkManager.GrupoProdCorte.Add())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        /// <summary>
        /// Terminar controlador 
        /// </summary>
        public void Terminar()
        {
            darkManager.CloseConnection();
            darkManager.CloseConnectionAccess();
            darkManager.CatalogoOpcionesValores = null;
            darkManager.View_empleadoEnsamble = null;
            darkManager.GrupoHorario = null;
            darkManager.View_gps_ensambleSinFiltro = null;
            darkManager.GrupoCambios = null;
            darkManager.GrupoArreglo = null;
        }
        #endregion
    }
}
