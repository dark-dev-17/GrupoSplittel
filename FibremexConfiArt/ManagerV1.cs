using FibremexConfiArt.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FibremexConfiArt
{
    public class ManagerV1
    {
        private readonly string Path = @"C:\Splittel\Ecommerce\Configuraciones\";
        private Configurable Configurable;

        public ManagerV1(Configurable configurable)
        {
            this.Configurable = configurable;
        }

        public ManagerV1(string File)
        {

        }

        public ManagerV1()
        {

        }

        public void Elemento(Elemento elemento, ActionElement actionElement)
        {
            if(elemento == null)
            {
                throw new Exception("Objeto elemento is null");
            }

            if(actionElement == ActionElement.Add)
            {
                elemento.Posicion = Configurable.Elementos.Max(a => a.Posicion) + 1;
                elemento.IdElemento = Configurable.Elementos.Max(a => a.Posicion) + 1;
                elemento.Valores = new List<Valores>();
                Configurable.Elementos.Add(elemento);
            }
            else if (actionElement == ActionElement.Edit)
            {
                var Old_element =  Configurable.Elementos.FirstOrDefault(a => a.IdElemClave == elemento.IdElemClave);
                if(Old_element == null)
                {
                    throw new Exception(string.Format("Elemento '{0}' not found in the '{1}' configration", elemento.IdElemClave, Configurable.Nombre));
                }
                Old_element.Descripcion = elemento.Descripcion;
            }
            else
            {
                if(Configurable.Elementos.FirstOrDefault(a => a.IdElemClave == elemento.IdElemClave) == null)
                    throw new Exception(string.Format("Elemento '{0}' not found in the '{1}' configration", elemento.IdElemClave, Configurable.Nombre));

                int index = Configurable.Elementos.FindIndex(element => element.IdElemClave == elemento.IdElemClave);
                Configurable.Elementos.RemoveAt(index);
            }
        }

        public void CreateNewConfiguracion(string NombreConfiguraion, string Descripción)
        {
            Configurable = new Configurable();
            Configurable.Nombre = NombreConfiguraion;
            Configurable.Descripción = Descripción;
            Configurable.Elementos = new List<Elemento>();

            SaveChanges(NombreConfiguraion);
        }

        public void SaveChanges(string NombreConfiguraion)
        {
            string path = string.Format(@"{0}Confi_{1}.json", Path, NombreConfiguraion + "");
            var json = JsonConvert.SerializeObject(Configurable);
            if (File.Exists(path))
            {
                File.WriteAllText(path, json);
            }
            else
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path, 1024))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(json);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        public void LoadConfiguration(string configuracionName)
        {
            string path = string.Format(@"{0}{1}", Path, configuracionName);
            if (File.Exists(path))
            {
                Configurable = JsonConvert.DeserializeObject<Configurable>(File.ReadAllText(path));
            }
            else
            {
                throw new Exception(string.Format("el archivo: {0} no fue encontrado", path));
            }
        }
    }

    public enum ActionElement
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
