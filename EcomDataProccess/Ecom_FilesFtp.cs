using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace EcomDataProccess
{
    public class Ecom_FilesFtp
    {
        #region Propiedades
        public string Ruta { get; private set; }
        public string Name { get; private set; }
        private string FTP_server { set; get; }
        private string FTP_user { set; get; }
        private string FTP_password { set; get; }
        private string FTP_Directory { set; get; }
        private string DomainSite { set; get; }
        #endregion

        #region Constructores
        ~Ecom_FilesFtp()
        {
            
        }
        public Ecom_FilesFtp()
        {

        }
        public Ecom_FilesFtp(string FTP_server, string FTP_user, string FTP_password)
        {
            this.FTP_server = FTP_server;
            this.FTP_user = FTP_user;
            this.FTP_password = FTP_password;
        }
        #endregion

        #region Metodos
        public List<Ecom_FilesFtp> Getfiles(string pattern, string publicRoute)
        {
            string Uri = "ftp://" + FTP_server + pattern;
            
            Stream responseStream = null;
            StreamReader reader = null;
            FtpWebResponse response = null;
            //StreamWriter writeStream = null;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
                List<Ecom_FilesFtp> lista = new List<Ecom_FilesFtp>();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                while (reader.Peek() >= 0)
                {
                    string nameFile = reader.ReadLine();
                    lista.Add(new Ecom_FilesFtp { Ruta = publicRoute + nameFile, Name = nameFile });
                }
                return lista;
            }
            catch(WebException ex)
            {
                if(ex.Status == WebExceptionStatus.ProtocolError)
                    throw new Ecom_Exception(string.Format("No se encontraron archivos"));
                else
                    throw new Ecom_Exception(string.Format("WebException FTP - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        public void DeleteFile(string path)
        {
            string Uri = "ftp://" + FTP_server  + path;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            try
            {
                request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        public void UpdateFile(string path, IFormFile FormFile)
        {
            string Uri = "ftp://" + FTP_server + path;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            StreamReader reader = null;
            try
            {

                if (ExistsFile(path))
                {
                    throw new Ecom_Exception(string.Format("Ya existe un archivo con el mismo nombre"));
                }
                request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                using (Stream ftpStream = request.GetRequestStream())
                {
                    FormFile.CopyTo(ftpStream);
                }
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }

        }
        public void Rename(string path,string oldFile, string newName)
        {
            string UriNew =  path + newName;
            string UriOld = "ftp://" + FTP_server + path + oldFile;
            FtpWebResponse response = null;
            try
            {
                if (ExistsFile(UriNew))
                {
                    throw new Ecom_Exception(string.Format("Ya existe un archivo con el mismo nombre"));
                }
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(UriOld));
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = newName;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        public bool ExistsFile(string PathFile)
        {
            string Uri = "ftp://" + FTP_server + PathFile;

            Stream responseStream = null;
            FtpWebResponse response = null;
            //StreamWriter writeStream = null;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        public List<Ecom_Archivos> ListDirectory(string pattern)
        {
                string Uri = "ftp://" + FTP_server + pattern;

                Stream responseStream = null;
                StreamReader reader = null;
                FtpWebResponse response = null;
                //StreamWriter writeStream = null;
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                    response = (FtpWebResponse)request.GetResponse();
                    List<Ecom_Archivos> lista = new List<Ecom_Archivos>();
                    responseStream = response.GetResponseStream();
                    reader = new StreamReader(responseStream);
                    Regex re = new System.Text.RegularExpressions.Regex(@".{10}\s*\d\s.+\s.+\s+\d+\s([a-zA-Z]{3})(\s\d\d|\s\s\d)(\s.{5})\s*(.+)");
                    string regex =
                        @"^" +                          //# Start of line
                        @"(?<dir>[\-ld])" +             //# File size          
                        @"(?<permission>[\-rwx]{9})" +  //# Whitespace          \n
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<filecode>\d+)" +
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<owner>\w+)" +
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<group>\w+)" +
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<size>\d+)" +
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<month>\w{3})" +            //# Month (3 letters)   \n
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<day>\d{1,2})" +            //# Day (1 or 2 digits) \n
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<timeyear>[\d:]{4,5})" +    //# Time or year        \n
                        @"\s+" +                        //# Whitespace          \n
                        @"(?<filename>(.*))" +          //# Filename            \n
                        @"$";                           //# End of line
                    Match match;
                    while (reader.Peek() >= 0)
                    {
                        var split = new Regex(regex).Match(reader.ReadLine());
                        string dir = split.Groups["dir"].ToString();
                        string filename = split.Groups["filename"].ToString();
                        bool isDirectory = !string.IsNullOrWhiteSpace(dir) && dir.Equals("d", StringComparison.OrdinalIgnoreCase);
                        if (filename != "." && filename != ".." && filename != ".ftpquota")
                        {
                            lista.Add(new Ecom_Archivos
                            {
                                Name = filename,
                                IsDirectory = isDirectory
                            });
                        }
                        
                    }
                    return lista;
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        throw new Ecom_Exception(string.Format("No se encontraron archivos"));
                    else
                        throw new Ecom_Exception(string.Format("WebException FTP - {0}", ex.Message));
                }
                catch (Exception ex)
                {
                    throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }
        public string ListDirectoryDetails(string pattern)
        {
            string Uri = "ftp://" + FTP_server + pattern;

            Stream responseStream = null;
            StreamReader reader = null;
            FtpWebResponse response = null;
            //StreamWriter writeStream = null;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                
                return reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                    throw new Ecom_Exception(string.Format("No se encontraron archivos"));
                else
                    throw new Ecom_Exception(string.Format("WebException FTP - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception FTP - {0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        #endregion
    }
    public class Ecom_Archivos
    {
        public bool IsDirectory { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string PathAux { get; set; }
        public string PathFolder { get; set; }
        public object Objecto { get; set; }

    }
}
