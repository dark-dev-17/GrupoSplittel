using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace GPSInformation.Tools
{
    public class EmailServ
    {
        #region Propiedades
        private string Server { get; set; }
        private string Account { get; set; }
        private string Port { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private bool ActiveSSL { get; set; }

        public List<string> ListTo { get; set; }
        public List<string> ListCC { get; set; }
        public List<string> ListBCC { get; set; }
        public List<string> ListReply { get; set; }

        private List<FileInfo> Attachments { get; set; }
        private MailMessage Email { get; set; }
        private MailPriority MailPryority_ { get; set; }
        private SmtpClient SmtpClient_ { get; set; }
        private Attachment Attachment_ { get; set; }
        #endregion

        #region Contructores
        public EmailServ(string Server, string Account, string Port, string User, string Password, bool ActiveSSL)
        {
            this.Server = Server;
            this.Account = Account;
            this.Port = Port;
            this.User = User;
            this.Password = Password;
            this.Password = Password;
        }
        #endregion

        #region Metodos
        public void Send(string Body, string SubJect)
        {
            try
            {
                this.Email = new MailMessage();
                this.SmtpClient_ = new SmtpClient(this.Server);
                Email.From = new MailAddress(this.Account.Trim());
                Email.IsBodyHtml = true;
                Email.Subject = SubJect;
                Email.Priority = MailPriority.Normal;

                if (ListReply != null)
                {
                    ListReply.ForEach(a => Email.ReplyToList.Add(a));
                }
                if (ListBCC != null)
                {
                    ListBCC.ForEach(a => Email.Bcc.Add(a));
                }
                if (ListCC != null)
                {
                    ListCC.ForEach(a => Email.CC.Add(a));
                }
                if (ListTo != null)
                {
                    ListTo.ForEach(a => Email.To.Add(a));
                }

                SmtpClient_.Port = int.Parse(this.Port);
                SmtpClient_.Credentials = new System.Net.NetworkCredential(this.User, this.Password);
                SmtpClient_.EnableSsl = ActiveSSL;
                Email.Body = Body;
                SmtpClient_.Send(Email);
            }
            catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;

                if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                {
                    SmtpClient_.Send(Email);
                }
                else
                {
                    throw ex;
                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                Email.Dispose();

            }
        }

        public void AddListReply(string correos)
        {
            if (ListReply == null)
                ListReply = new List<string>();

            GetEmails(correos).ForEach(a =>
            {
                if (!IsValidEmail(a))
                    throw new Exceptions.GpExceptions(string.Format("El correo '{0}' no es valido para destinatario", a));
                ListReply.Add(a);
            });
        }

        public void AddListBCC(string correos)
        {
            if (ListBCC == null)
                ListBCC = new List<string>();

            GetEmails(correos).ForEach(a =>
            {
                if (!IsValidEmail(a))
                    throw new Exceptions.GpExceptions(string.Format("El correo '{0}' no es valido para destinatario", a));
                ListBCC.Add(a);
            });
        }

        public void AddListCC(string correos)
        {
            if (ListCC == null)
                ListCC = new List<string>();

            GetEmails(correos).ForEach(a =>
            {
                if (!IsValidEmail(a))
                    throw new Exceptions.GpExceptions(string.Format("El correo '{0}' no es valido para destinatario", a));
                ListCC.Add(a);
            });
        }

        public void AddListTO(string correos)
        {
            if(ListTo == null)
                ListTo = new List<string>();

            GetEmails(correos).ForEach(a =>
            {
                if (!IsValidEmail(a))
                    throw new Exceptions.GpExceptions(string.Format("El correo '{0}' no es valido para destinatario", a));
                ListTo.Add(a);
            });
        }

        private bool IsValidEmail(string email)
        {
            bool errorStatus = false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (!re.IsMatch(email.Trim()))
                errorStatus = false;
            else
            {
                errorStatus = true;
            }
            return errorStatus;
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
}
