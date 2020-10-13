using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{
    public class Prenomina_Rep
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<TipoNomina> TipoNominas { get; set; }
    }

    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }
        public bool Selected { get; set; }
    }

    public class TipoNomina
    {
        public int IdTipoNomina { get; set; }
        public string Nombre { get; set; }
        public bool Selected { get; set; }
    }

    public class Prenomina_RepProd
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public List<Turno> Turnos { get; set; }
    }
    public class Turno
    {
        public int IdTurnosProduccion { get; set; }
        public string Nombre { get; set; }
        public bool Selected { get; set; }
    }
}
