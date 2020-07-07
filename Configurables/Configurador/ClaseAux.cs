using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class UpdateCodeexample
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
    }
    #region RestriccionFree
    public class AddRestriccionFree
    {
        public RestriccionCampoUsuario restriccionCampoUsuario { get; set; }
        public string Nombre { get; set; }
    }
    public class DeleteRestriccionFree
    {
        public RestriccionCampoUsuario restriccionCampoUsuario { get; set; }
        public string Nombre { get; set; }
    }
    public class UpdateRestriccionFree
    {
        public RestriccionCampoUsuario restriccionCampoUsuario { get; set; }
        public int IndexRestriccion { get; set; }
        public string Nombre { get; set; }
    }
    #endregion

    #region Reglas
    public class AddRegla
    {
        public Regla regla { get; set; }
        public int IndexRestriccion { get; set; }
        public string Nombre { get; set; }
    }
    public class DeleteRegla
    {
        public int IndexRestriccion { get; set; }
        public Regla regla { get; set; }
        public string Nombre { get; set; }
    }
    public class UpdateRegla
    {
        public Regla regla { get; set; }
        public int IndexRestriccion { get; set; }
        public int IndexRelga { get; set; }
        public string Nombre { get; set; }
    }
    #endregion

    #region Restricciones
    public class AddRestriction
    {
        public RestriccionElemento restriccionElemento { get; set; }
        public string Nombre { get; set; }
    }
    public class DeleteRestriction
    {
        public RestriccionElemento restriccionElemento { get; set; }
        public string Nombre { get; set; }
    }
    public class UpdateRestriction
    {
        public RestriccionElemento restriccionElemento { get; set; }
        public int IndexRestriccion { get; set; }
        public string Nombre { get; set; }
    }
    #endregion

    #region restricciones elementos del codigo
    public class AddElementoCodigo
    {
        public ElementCode element { get; set; }
        public string Nombre { get; set; }
    }
    public class DeleteElementoCodigo
    {
        public ElementCode element { get; set; }
        public string Nombre { get; set; }
    }
    public class UpdateElementoCodigo
    {
        public ElementCode element { get; set; }
        public int elementCode { get; set; }
        public string Nombre { get; set; }
    }
    #endregion

    #region Valores
    public class AddValorElemCodigo
    {
        public int IndexPartCodigo { get; set; }
        public OpcionesSelect opcionesSelect { get; set; }
        public string Nombre { get; set; }
    }
    public class UpdateValorElemCodigo
    {
        public int IndexValue { get; set; }
        public int IndexPartCodigo { get; set; }
        public OpcionesSelect opcionesSelect { get; set; }
        public string Nombre { get; set; }
    }
    public class DeleteValorElemCodigo
    {
        public OpcionesSelect opcionesSelect { get; set; }
        public int IndexPartCodigo { get; set; }
        public string Nombre { get; set; }
    }
    #endregion

}
