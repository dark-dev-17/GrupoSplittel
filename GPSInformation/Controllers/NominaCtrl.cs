using GPSInformation.Models;
using GPSInformation.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GPSInformation.Controllers
{
    public class NominaCtrl
    {
        #region Atributos
        private readonly string PathNews = @"C:\Splittel\GestionPersonal\Nomina\Nuevos\";
        private readonly string Xml_Namespace_prefix_SAT = "cfdi";
        private readonly string Xml_Namespace_uri_SAT = "http://www.sat.gob.mx/cfd/3";
        private DarkManager darkManager;
        private FilesCFDI filesCFDI;
        #endregion

        #region Constructores
        public NominaCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.Nomina);
            filesCFDI = new FilesCFDI(PathNews);
        }
        #endregion
        #region Metodos

        public void ProccessFiles()
        {
            List<Nomina> nominas = ProccessFilesXML();
            nominas.ForEach(nomina => {
                filesCFDI.CreateFolder(@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC);
                filesCFDI.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), nomina.NombreArchivo);
                filesCFDI.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), (nomina.NombreArchivo.Split('.')[0] + ".pdf"));

                //nomina.SetConnection(GPSManager.getConnection());
                //bool result = nomina.Add();
                //if (result)
                //{
                //    files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), nomina.NombreArchivo);
                //    files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), (nomina.NombreArchivo.Split('.')[0] + ".pdf"));

                //    CountAdd++;
                //}

                //if (GPSManager.GetErrorCode() == 10)
                //{
                //    files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), nomina.NombreArchivo);
                //    files.Move(@"C:\Splittel\GestionPersonal\Nomina\Nuevos\", (@"C:\Splittel\GestionPersonal\Nomina\Actuales\" + nomina.RFC + @"\"), (nomina.NombreArchivo.Split('.')[0] + ".pdf"));
                //}   
            });
        }

        private List<Nomina> ProccessFilesXML()
        {
            var archivos = filesCFDI.Get("*.xml");
            List<Nomina> nominas = new List<Nomina>();
            archivos.ForEach(a =>
            {
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
                    
                    var TotPercep = doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("TotalPercepciones");
                    double TotalPercepciones = Double.Parse(TotPercep == null ? "0" : TotPercep.Value);

                    var TotDeduc = doc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina", xmlmanager).Attributes.GetNamedItem("TotalDeducciones");
                    double TotalDeducciones = Double.Parse(TotDeduc == null ? "0" : TotDeduc.Value);
                    nomina.TotalNeto = TotalPercepciones - TotalDeducciones;
                    nomina.NumeroNomina = 1;
                    nomina.Comentarios = "";
                    nomina.NombreArchivo = a.Name;
                    nominas.Add(nomina);

                    var Nominaexists = darkManager.Nomina.Get("Folio", nomina.Folio, "RFC", nomina.RFC);

                    if(Nominaexists is null)
                    {
                        darkManager.Nomina.Element = nomina;
                        if (!darkManager.Nomina.Add())
                        {
                            throw new GPSInformation.Exceptions.GpExceptions(string.Format("{0} - {1}", PathNews + a.Name, "No se pudo guardar la nomina"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new GPSInformation.Exceptions.GpExceptions(string.Format("{0} - {1}", PathNews + a.Name, ex.ToString()));
                }

            });
            return nominas;
        }

        public byte[] GetCFDI(string RFC, int id, string Type)
        {
            var lista = darkManager.Nomina.Get("Folio", "" + id, "RFC", RFC);
            if (Type.Trim() == "XML")
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, lista.NombreArchivo));
                string fileName = lista.NombreArchivo;
                return System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, lista.NombreArchivo));
            }
            else
            {
                string FilePDF = lista.NombreArchivo.Split('.')[0] + ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, FilePDF));
                string fileName = FilePDF;
                return System.IO.File.ReadAllBytes(string.Format(@"C:\Splittel\GestionPersonal\Nomina\Actuales\{0}\{1}", lista.RFC, FilePDF));
            }
        }
        public Nomina GetNomina(string RFC, int id)
        {
            return darkManager.Nomina.Get("Folio", "" + id, "RFC", RFC);
        }
        public List<Nomina> GetNOminas(string RFC)
        {
            return darkManager.Nomina.Get(RFC, "RFC").OrderByDescending(a => a.FechaEmision).ToList();
        }



        #endregion
    }
}
