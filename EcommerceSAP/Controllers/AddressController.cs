using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceSAP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private SAPDataProcess.SAP_Address SAP_Address_;
        private readonly string StringConnection = "Data Source=192.168.2.17;User ID=USR_LECTURA;Password=splitel.lectura16;Initial Catalog=FIBREMEX;Connect Timeout=9000;Persist Security Info=True;MultipleActiveResultSets=true;";
        
        // GET api/values
        [HttpGet]
        [AccessSAP]
        //[HttpGet("/products/{id}", Name = "Products_List")]
        [HttpGet("[controller]/[action]/{TypeAddres}")]
        public ActionResult<IEnumerable<SAPDataProcess.SAP_Address>> GetByBussinesPartner(string TypeAddres)
        {
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(StringConnection);
                SAP_DBConnection_.OpenConnection();
                SAP_Address_ = new SAPDataProcess.SAP_Address(SAP_DBConnection_);
                return SAP_Address_.GetList(HttpContext.Request.Headers["CardCode"].ToString(), TypeAddres);
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if(SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
                
            }
        }

        // GET api/values/5
        [HttpGet]
        [AccessSAP]
        [HttpGet("[controller]/[action]/{TypeAddres}/{AddressName}")]
        public ActionResult<SAPDataProcess.SAP_Address> GetByAddressName(string TypeAddres, string AddressName)
        {
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            try
            {
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(StringConnection);
                SAP_DBConnection_.OpenConnection();
                SAP_Address_ = new SAPDataProcess.SAP_Address(SAP_DBConnection_);
                bool exists = SAP_Address_.GetByAddressName(HttpContext.Request.Headers["CardCode"].ToString(), TypeAddres, AddressName);
                if (exists)
                {
                    return Ok(SAP_Address_);
                }
                else
                {
                    return BadRequest(SAP_Address_.GetMessage());
                }
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }

            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
