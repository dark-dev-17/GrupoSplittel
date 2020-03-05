using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceSAP.Models
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessSAP : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string CardCode = filterContext.HttpContext.Request.Headers["CardCode"].ToString();
            string Society = filterContext.HttpContext.Request.Headers["Society"].ToString();
            string Password = filterContext.HttpContext.Request.Headers["Password"].ToString();

            //if (!ValidString(CardCode) || !ValidString(Society) || !ValidString(Password))
            //{
            //    filterContext.Result = new BadRequestObjectResult("The credentials are missing");
            //    return;
            //}

            string StringConnection = "Data Source=192.168.2.17;User ID=USR_LECTURA;Password=splitel.lectura16;Initial Catalog=FIBREMX_TEST;Connect Timeout=9000;Persist Security Info=True;MultipleActiveResultSets=true;";
            //string StringConnection = "Data Source=192.168.2.17;User ID=USR_LECTURA;Password=splitel.lectura16;Initial Catalog=FIBREMEX;Connect Timeout=9000;Persist Security Info=True;MultipleActiveResultSets=true;";

            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(StringConnection);
                SAP_DBConnection_.OpenConnection();
                SAPDataProcess.SAP_Access SAP_Access_ = new SAPDataProcess.SAP_Access(SAP_DBConnection_);
                SAP_Access_.CardCode = CardCode;
                SAP_Access_.Society = Society;
                SAP_Access_.Password = Password;
                SAP_Access_.ValidCredentials();
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                filterContext.Result = new BadRequestObjectResult(ex.Message);
            }
            finally
            {
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }

            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        private bool ValidString(string paramt)
        {
            return (string.IsNullOrEmpty(paramt) && string.IsNullOrWhiteSpace(paramt)) ? false : true;
        }
        
    }
}
