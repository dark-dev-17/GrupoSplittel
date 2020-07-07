using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Configurables.Models;
using Configurables.Configurador;
using Newtonsoft.Json;

namespace Configurables.Controllers
{
    public class EditorConfigurableController : Controller
    {

        private Conf_Files conf_Files;
        private ConfigurableConf Data;
        private EditorConfigurable Editor;
        public EditorConfigurableController()
        {
            conf_Files = new Conf_Files(@"C:\Splittel\Ecommerce\Configuraciones\");
        }

        #region validaciones campos opcionales
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatedRestriccionFree([FromBody]UpdateRestriccionFree updateRestriccionFree)
        {
            try
            {
                GetData(updateRestriccionFree.Nombre);
                Editor.UpdatedRestriccionFree(updateRestriccionFree.restriccionCampoUsuario, updateRestriccionFree.IndexRestriccion);
                SaveChanges();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeletedRestriccionFree([FromBody]DeleteRestriccionFree deleteRestriccionFree)
        {
            try
            {
                GetData(deleteRestriccionFree.Nombre);
                Editor.DeletedRestriccionFree(deleteRestriccionFree.restriccionCampoUsuario);
                SaveChanges();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddRestriccionFree([FromBody]AddRestriccionFree addRestriccionFree)
        {
            try
            {
                GetData(addRestriccionFree.Nombre);
                Editor.AddRestriccionFree(addRestriccionFree.restriccionCampoUsuario);
                SaveChanges();
                return Ok(Data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Restricciones reglas
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateRegla([FromBody]UpdateRegla updateRegla)
        {
            try
            {
                GetData(updateRegla.Nombre);
                Editor.UpdateRegla(updateRegla.IndexRelga, updateRegla.IndexRestriccion, updateRegla.regla);
                SaveChanges();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteRegla([FromBody]DeleteRegla deleteRegla)
        {
            try
            {
                GetData(deleteRegla.Nombre);
                Editor.DeleteRegla(deleteRegla.regla, deleteRegla.IndexRestriccion);
                SaveChanges();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddReg([FromBody]AddRegla addRegla)
        {
            try
            {
                GetData(addRegla.Nombre);
                Editor.AddRegla(addRegla.IndexRestriccion, addRegla.regla);
                SaveChanges();
                return Ok(Data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Restricciones a campos opcionales
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateRestrcition([FromBody]UpdateRestriction updateRestriction)
        {
            try
            {
                GetData(updateRestriction.Nombre);
                if (Editor.UpdateRestrcition(updateRestriction.IndexRestriccion, updateRestriction.restriccionElemento))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se actualizo el valor"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteRestrcition([FromBody]DeleteRestriction deleteRestriction)
        {
            try
            {
                GetData(deleteRestriction.Nombre);
                if (Editor.DeleteRestrcition(deleteRestriction.restriccionElemento))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se elimino el valor"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddRestrcition([FromBody]AddRestriction addRestriction)
        {
            try
            {
                GetData(addRestriction.Nombre);
                if (Editor.AddRestrcition(addRestriction.restriccionElemento))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("Existe un valor con la misma clave: {0}", addRestriction.restriccionElemento.Block));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region valors de bloque del codigo
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetElementoCodigoValueId(string id, string Nombre)
        {
            try
            {
                GetData(Nombre);
                if (Data.Blocks.Exists(elem => elem.Key == id))
                {
                    return Ok(Data.Blocks.Find(elem => elem.Key == id));
                }
                else
                {
                    return BadRequest(string.Format("No se actualizo el valor"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateElementoCodigoValue([FromBody]UpdateValorElemCodigo updateValorElemCodigo)
        {
            try
            {
                GetData(updateValorElemCodigo.Nombre);
                if (Editor.UpdateValuePartCodigo(updateValorElemCodigo.IndexValue, updateValorElemCodigo.IndexPartCodigo, updateValorElemCodigo.opcionesSelect))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se actualizo el valor"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteElementoCodigoValue([FromBody]DeleteValorElemCodigo deleteValorElemCodigo)
        {
            try
            {
                GetData(deleteValorElemCodigo.Nombre);
                if (Editor.DeleteValuePartCodigo(deleteValorElemCodigo.opcionesSelect, deleteValorElemCodigo.IndexPartCodigo))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se elimino el valor"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddElementoCodigoValue([FromBody]AddValorElemCodigo addValorElemCodigo)
        {
            try
            {
                GetData(addValorElemCodigo.Nombre);
                if (Editor.AddValuePartCodigo(addValorElemCodigo.IndexPartCodigo, addValorElemCodigo.opcionesSelect))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("Existe un valor con la misma clave: {0}", addValorElemCodigo.opcionesSelect.Key));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Elementos de codigo
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateElementoCodigo([FromBody]UpdateElementoCodigo updateElementoCodigo)
        {
            try
            {
                GetData(updateElementoCodigo.Nombre);
                if (Editor.UpdateElementoCodigo(updateElementoCodigo.element, updateElementoCodigo.elementCode))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se elimino"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteElementoCodigo([FromBody]DeleteElementoCodigo deleteElementoCodigo)
        {
            try
            {
                GetData(deleteElementoCodigo.Nombre);
                if (Editor.DeleteElementoCodigo(deleteElementoCodigo.element))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("No se elimino"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddElementoCodigo([FromBody]AddElementoCodigo addElementoCodigo)
        {
            try
            {
                GetData(addElementoCodigo.Nombre);
                if (Editor.AddElementoCodigo(addElementoCodigo.element))
                {
                    SaveChanges();
                    return Ok(Data);
                }
                else
                {
                    return BadRequest(string.Format("Existe un elemento con la misma clave: {0}", addElementoCodigo.element.Key));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Archivo
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCode([FromBody]UpdateCodeexample updateCodeexample)
        {
            try
            {
                GetData(updateCodeexample.Nombre);
                Data.ItemCodeexample = updateCodeexample.Codigo;
                Editor.validateCodeExample();
                SaveChanges();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetConfigurable(string id)
        {
            try
            {
                GetData(id);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddConfigurable(string Nombre)
        {
            try
            {
                conf_Files.Name = Nombre;
                if (string.IsNullOrEmpty(Nombre))
                {
                    throw new Exception("valor Nombre esta vacio");
                }
                ConfigurableConf ConfiguracionOp = new ConfigurableConf();
                ConfiguracionOp.Configurable = Nombre;
                ConfiguracionOp.ItemCodeexample = "";
                ConfiguracionOp.Blocks = new List<ElementCode>();
                ConfiguracionOp.Rectrictions = new List<RestriccionElemento>();
                ConfiguracionOp.FieldsFree = new List<RestriccionCampoUsuario>();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ConfiguracionOp);
                conf_Files.Create(json);
                return Ok(ConfiguracionOp.Configurable + ".json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void GetData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Nombre de archivo no valido");
            }
            string Name = conf_Files.Get().Find(fil => fil.Name == id).Name;
            conf_Files.Name = Name;
            Data = JsonConvert.DeserializeObject<ConfigurableConf>(conf_Files.Open());
            Editor = new EditorConfigurable(Data);
        }
        private void SaveChanges()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Editor.ConfigurableConf);
            conf_Files.SaveChanges(json);
        }
        #endregion

    }
}