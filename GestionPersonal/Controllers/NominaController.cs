using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using GPDataInformation.Mods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class NominaController : Controller
    {
        private string PathNews = @"C:\Splittel\GestionPersonal\Nomina\Nuevos\";
        private Files files;
        private string Xml_Namespace_prefix_SAT = "cfdi";
        private string Xml_Namespace_uri_SAT = "http://www.sat.gob.mx/cfd/3";
        private Models.GPSManager GPSManager;

        public NominaController(IConfiguration configuration)
        {
            files = new Files(PathNews);
            GPSManager = new Models.GPSManager();
        }
        ~NominaController()
        {

        }
        // GET: Nomina
        public ActionResult ProccessFiles()
        {
            return Ok(MoveCFDI());
        }
        [HttpPost]
        public ActionResult GetByRFC(String RFC)
        {
            GPSManager.OpenConnection();
            var lista = new Models.Nomina(GPSManager.getConnection()).Get(RFC);
            GPSManager.CloseConnection();
            return Ok(lista);
        }
        [HttpGet]
        public FileResult GetByRFC_Id(String RFC, int id, string Type)
        {
            GPSManager.OpenConnection();
            var lista = new Models.Nomina(GPSManager.getConnection()).Get(RFC, id);
            GPSManager.CloseConnection();

            if (Type.Trim() == "XML")
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, lista.NombreArchivo));
                string fileName = lista.NombreArchivo;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                string FilePDF = lista.NombreArchivo.Split('.')[0] + ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, FilePDF));
                string fileName = FilePDF;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        private List<Models.Nomina> ProcessXML()
        {
            var archivos = files.Get("*.xml");
            List<Models.Nomina> nominas = new List<Models.Nomina>();
            archivos.ForEach(a => {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathNews + a.Name);
                    System.Xml.XmlNamespaceManager xmlmanager = new XmlNamespaceManager(doc.NameTable);
                    xmlmanager.AddNamespace(this.Xml_Namespace_prefix_SAT, Xml_Namespace_uri_SAT);
                    xmlmanager.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");
                    xmlmanager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                    Models.Nomina nomina = new Models.Nomina();
                    nomina.RFC = doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", xmlmanager).Attributes.GetNamedItem("Rfc").Value;

                    nomina.FechaEmision = DateTime.Parse(doc.SelectSingleNode("/cfdi:Comprobante", xmlmanager).Attributes.GetNamedItem("Fecha").Value);
                    nomina.Folio = doc.SelectSingleNode("/cfdi:Comprobante", xmlmanager).Attributes.GetNamedItem("Folio").Value;
                    nomina.FechaInicialPago = DateTime.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("FechaInicialPago").Value);
                    nomina.FechaFinalPago = DateTime.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("FechaFinalPago").Value);
                    nomina.FechaTimbrado = DateTime.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", xmlmanager).Attributes.GetNamedItem("FechaTimbrado").Value);
                    nomina.FechaPago = DateTime.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("FechaPago").Value);
                    double TotalPercepciones = Double.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("TotalPercepciones").Value);
                    double TotalDeducciones = Double.Parse(doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("TotalDeducciones").Value);
                    nomina.TotalNeto = TotalPercepciones - TotalDeducciones;
                    nomina.NumeroNomina = 0;
                    nomina.Comentarios = "";
                    nomina.NombreArchivo = a.Name;
                    nominas.Add(nomina);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} - {1}", PathNews + a.Name, ex.Message));
                }
               
            });
            return nominas;
        }

        private string MoveCFDI()
        {
            try
            {
                int CountAdd = 0;
                var lista = ProcessXML();
                GPSManager.OpenConnection();
                lista.ForEach(nomina => {
                    files.CreateFolder(@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC);
                    nomina.SetConnection(GPSManager.getConnection());
                    bool result = nomina.Add();
                    if (result)
                    {
                        files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), nomina.NombreArchivo);
                        files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), (nomina.NombreArchivo.Split('.')[0] + ".pdf"));

                        CountAdd++;
                    }

                    if (GPSManager.GetErrorCode() == 10)
                    {
                        files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), nomina.NombreArchivo);
                        files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), (nomina.NombreArchivo.Split('.')[0] + ".pdf"));
                    }
                });
                GPSManager.CloseConnection();

                return string.Format("Se han agregado {0} documentos", CountAdd);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}