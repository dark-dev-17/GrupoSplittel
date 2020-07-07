using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurables.Configurador

{
    public class Conf_Files
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Path { get; private set; }

        public Conf_Files(string Path)
        {
            this.Path = Path;
            this.CreateFolder();
        }

        public Conf_Files()
        {

        }

        public void Create(string FileContent)
        {
            string path = string.Format(@"{0}{1}.json", Path, Name);

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path, 1024))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(FileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public void Delete()
        {
            string path = string.Format(@"{0}{1}", Path, Name);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new Exception(string.Format("el archivo: {0} no fue encontrado", path));
            }
        }

        public string Open()
        {
            string Result = "";
            string path = string.Format(@"{0}{1}", Path, Name);
            if (File.Exists(path))
            {
                Result = File.ReadAllText(path);
            }
            else
            {
                throw new Exception(string.Format("el archivo: {0} no fue encontrado", path));
            }
            return Result;
        }

        public List<Conf_Files> Get()
        {
            List<Conf_Files> conf_Files = new List<Conf_Files>();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path);
            foreach (var fi in directoryInfo.GetFiles(string.Format("*.json")))
            {
                conf_Files.Add(new Conf_Files()
                {
                    Name = fi.Name,
                    Created = fi.CreationTime,
                    Updated = fi.LastWriteTime
                });
            }
            directoryInfo = null;
            return conf_Files;
        }
        public void SaveChanges(string content)
        {
            string path = string.Format(@"{0}{1}", Path, Name);
            if (File.Exists(path))
            {
                File.WriteAllText(path, content);
            }
            else
            {
                throw new Exception(string.Format("el archivo: {0} no fue encontrado", path));
            }

        }
        private void CreateFolder()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }
    }
}
