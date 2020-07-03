using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var owners = System.IO.File.ReadAllText(@"C:\Users\Luis Martinez\Desktop\jumpersMonomodo.json");
            ConfigurationDinamic Data = JsonConvert.DeserializeObject<ConfigurationDinamic>(owners);
            return Ok(Data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            ConfigurationValid configurationValid = new ConfigurationValid();
            configurationValid.ItemCode = id;
            configurationValid.LoadData();
            configurationValid.GetGroupsValid();
            configurationValid.ApplyRestrictions();
            return Ok(configurationValid.GetModelView());
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> ValidateCode([FromBody] string value)
        {
            string pattern = @"^(?!\s)(?<Marca>OP){1,1}(?<Producto>GA){0,1}(?<ProdType>PI){0,1}(?<Unidades>042|022){0,1}(?<Profundidad>P|O){0,1}(?<CombDoors>C|V|CPD|VPD){0,1}(?<SH>$|\s|SH$)";
            //Stopwatch sw;
            string Response = "";
            Regex expression = new Regex(pattern);
            MatchCollection mc = expression.Matches(value);
            Match ms = expression.Match(value);
            if (ms.Success)
            {
                foreach (Match m in mc)
                {
                    foreach (string group in expression.GetGroupNames())
                    {
                        Response += string.Format("{0}.{1,-15} {2,25}  {3}", m.Groups[group].Index, group, m.Groups[group].Value.Trim(), m.Groups[group].Success);
                        Response += "\n";
                    }

                }
                return Ok(Response);
            }
            else
            {
                return BadRequest("Codigo invalido");
            }
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
