using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Configurables.Models;
using Configurables.Configurador;
using Newtonsoft.Json;
using Configurables.Render;

namespace Configurables.Controllers
{
    public class ConfigurableController : Controller
    {
        private Conf_Files conf_Files;
        private ConfigurableConf Data;
        private EditorConfigurable Editor;

        public ConfigurableController()
        {
            conf_Files = new Conf_Files(@"C:\Splittel\Ecommerce\Configuraciones\");
        }

        public ActionResult Index()
        {
            return View(conf_Files.Get().OrderBy(a => a.Updated));
        }
        [HttpGet]
        public ActionResult GetConfig()
        {
            return Ok(conf_Files.Get().OrderBy(a => a.Updated));
        }
        public ActionResult Edit(string id)
        {
            Conf_Files conf_Files1 = conf_Files.Get().Find(a => a.Name == id);
            return View(conf_Files1);
        }

        public ActionResult Create(string NewName, string File)
        {
            GetData(File);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
            Data.Configurable = NewName;
            conf_Files.Name = NewName;
            conf_Files.Create(json);
            return RedirectToAction("Configurar", new { id = Data.Configurable + ".json" });
        }
        public ActionResult CreateNew(string NewName)
        {
            Data = new ConfigurableConf();
            Data.Configurable = NewName;
            Data.Expresion = "";
            Data.ItemCode = "";
            Data.ItemCodeexample = "";
            Data.Blocks = new List<ElementCode>();
            Data.Rectrictions = new List<RestriccionElemento>();
            Data.FieldsFree = new List<RestriccionCampoUsuario>();
            conf_Files.Name = NewName;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
            conf_Files.Create(json);
            return RedirectToAction("Configurar", new { id = Data.Configurable + ".json" });
        }

        public ActionResult Make(string id)
        {
            Conf_Files conf_Files1 = conf_Files.Get().Find(a => a.Name == id);
            return View(conf_Files1);
        }

        public ActionResult Configurar(string id)
        {
            Conf_Files conf_Files1 = conf_Files.Get().Find(a => a.Name == id);
            return View(conf_Files1);
        }

        public FileResult Download(string id)
        {
            Conf_Files conf_Files1 = conf_Files.Get().Find(a => a.Name == id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\Ecommerce\Configuraciones\{0}", id));
            string fileName = conf_Files1.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public IActionResult DataMake([FromBody]ConfigurationUserRe ConfigurationUserRe)
        {
            try
            {
                GetData(ConfigurationUserRe.Nombre);
                if (ConfigurationUserRe.configurationUser == null || ConfigurationUserRe.configurationUser.Blocks == null)
                {
                    ProcesatorConfig procesatorConfig = new ProcesatorConfig(ProcesatorConfigMode.WithoutCode, Data);
                    procesatorConfig.ValidCode();
                    procesatorConfig.ConfigureForm();
                    return Ok(procesatorConfig.GetConfigurationUser());
                }
                else
                {
                    ProcesatorConfig procesatorConfig = new ProcesatorConfig(ConfigurationUserRe.configurationUser, Data);
                    procesatorConfig.ApplyRuleForElement();
                    return Ok(procesatorConfig.GetConfigurationUser());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult DattaMaker([FromBody]ConfigurationUserRe ConfigurationUserRe)
        {
            try
            {
                GetData(ConfigurationUserRe.Nombre);
                if (ConfigurationUserRe.configurationUser == null || ConfigurationUserRe.configurationUser.Blocks == null)
                {
                    if (string.IsNullOrEmpty(Data.ItemCodeexample))
                    {
                        throw new Exception("Está configuración no puede ser usada, dato faltante 'ItemCodeexample'");
                    }
                    Maker maker = new Maker(Data);
                    return Ok(maker.GetConfigurationUser());
                }
                else
                {
                    Maker maker = new Maker(Data, ConfigurationUserRe.configurationUser);
                    return Ok(maker.GetConfigurationUser());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}
