using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            string strRegex = @"^(OP)+(GA)+(PI)+((042|022)(P|O)(C|V|CPD|VPD)$|(042|022)(P|O)(C|V|CPD|VPD)(SH)$)";

            Regex re = new Regex(strRegex);

            if (re.IsMatch(id))
                return Ok("valido");
            else
                return Ok("invalido");
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> ValidateCode([FromBody] string value)
        {
            string strRegex = @"^(OP)+(GA)+(PI)+((042|022)(P|O)(C|V|CPD|VPD)$|(042|022)(P|O)(C|V|CPD|VPD)(SH)$)";

            Regex re = new Regex(strRegex);

            if (re.IsMatch(value))
                return Ok("valido");
            else
                return Ok("invalido");
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
