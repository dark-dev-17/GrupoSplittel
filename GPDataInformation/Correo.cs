using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GPDataInformation
{
    public class Correo
    {
        #region Propiedades
        private string Server;
        private string From;
        private int Port;
        private string User;
        private string Password;
        private bool UserSSL;
        private string Message;

        private MailMessage Email;
        private SmtpClient SmtpServer;
        #endregion

        #region Constructores
        ~Correo()
        {

        }
        public Correo()
        {

        }
        public Correo(string Server, string From, int Port, string User, string Password, bool UserSSL)
        {
            this.Server = Server;
            this.From = From;
            this.Port = Port;
            this.User = User;
            this.Password = Password;
            this.UserSSL = UserSSL;
        }
        #endregion

        #region Metodos
        public void SendErrorMail(string BodyHTML, string AddressesoT, string AddressesCC, string AddressesBCC)
        {
            try
            {
                Email = new MailMessage();
                SmtpServer = new SmtpClient(this.Server.Trim());
                Email.From = new MailAddress(this.From.Trim());
                Email.IsBodyHtml = true;
                Email.Body = BodyHTML;
                Email.Subject = "Gestión Personal -- Error";
                Email.Priority = MailPriority.High;
                AddAddress(AddressesoT, EmailList.To);
                AddAddress(AddressesCC, EmailList.CC);
                AddAddress(AddressesBCC, EmailList.BCC);
                SmtpServer.Port = this.Port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(this.User, this.Password);
                SmtpServer.EnableSsl = this.UserSSL;
                SmtpServer.Send(Email);
            }
            catch (SmtpException ex)
            {
                throw new GpExceptions(ex.Message);
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(ex.Message);
            }
            catch (Exception ex)
            {
                throw new GpExceptions(ex.Message);
            }
            finally
            {
                Email.Dispose();
            }
        }
        public bool SendMailNotification(string BodyHTML, string AddressesoT, string AddressesCC, string AddressesBCC)
        {
            try
            {
                Email = new MailMessage();
                SmtpServer = new SmtpClient(this.Server.Trim());
                Email.From = new MailAddress(this.From.Trim());
                Email.IsBodyHtml = true;

                string script = File.ReadAllText(@"C:\Splittel\Ecommerce\EmailTemplates\EmailNotification.html").ToString()
                            .Replace("@MESSAGE", BodyHTML);
                Email.Body = script;
                Email.Subject = "Gestión Personal";
                Email.Priority = MailPriority.Normal;
                AddAddress(AddressesoT, EmailList.To);
                AddAddress(AddressesCC, EmailList.CC);
                AddAddress(AddressesBCC, EmailList.BCC);
                SmtpServer.Port = this.Port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(this.User, this.Password);
                SmtpServer.EnableSsl = this.UserSSL;
                SmtpServer.Send(Email);
                return true;
            }
            catch (SmtpException ex)
            {
                Message = ex.Message;
                return false;
            }
            catch (GpExceptions ex)
            {
                Message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
            finally
            {
                Email.Dispose();
            }
        }
        public string GetMessage()
        {
            return Message;
        }
        private void IsValidEmail(string email)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(email.Trim()))
                    throw new GpExceptions(String.Format("El correo ['{0}'] no es valido", email));
            }
            catch (GpExceptions ex)
            {
                throw ex;
            }


        }
        private void AddAddress(string data, EmailList EmailList_)
        {
            try
            {
                if (data != null)
                {
                    List<string> Emails = GetEmails(data);
                    if (Emails.Count != 0)
                    {
                        foreach (string EmailDir in Emails)
                        {
                            IsValidEmail(EmailDir);
                            if (EmailList_ == EmailList.To)
                            {
                                Email.To.Add(EmailDir);
                            }
                            else if (EmailList_ == EmailList.BCC)
                            {
                                Email.Bcc.Add(EmailDir);
                            }
                            else if (EmailList_ == EmailList.CC)
                            {
                                Email.CC.Add(EmailDir);
                            }
                        }
                    }
                }
            }
            catch (GpExceptions ex)
            {
                throw ex;
            }

        }
        private List<string> GetEmails(string dataset)
        {
            List<string> list = new List<string>();
            string[] allAddresses = dataset.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string emailAddress in allAddresses)
            {
                list.Add(emailAddress);
            }

            return list;
        }
        #endregion
    }
    public enum EmailList
    {
        To = 0,
        CC = 1,
        BCC = 2,
    }
}
