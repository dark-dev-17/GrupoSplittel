using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EcommerceApiLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceFibremexApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private EcommerceApiLogic.DarkDev darkDev;
        private readonly IConfiguration configuration;
        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
            darkDev = new EcommerceApiLogic.DarkDev(configuration, DbManagerDark.DarkMode.Ecommerce);
            darkDev.OpenConnection();
            darkDev.LoadObject(EcommerceApiLogic.MysqlObject.Usuario);
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            return darkDev.Usuario.Get();
        }

        // GET api/values/5
        [HttpGet("{usuario}/{password}")]
        [Route("[action]/{usuario}/{password}")]
        [AllowAnonymous]
        public ActionResult<Usuario> Get(string usuario, string password)
        {
            var Result = darkDev.Usuario.Get(
                darkDev.Usuario.ColumName(nameof(darkDev.Usuario.Element.Email)), usuario,
                darkDev.Usuario.ColumName(nameof(darkDev.Usuario.Element.Password)), password
            );

            if(Result == null)
            {
                return Unauthorized();
            }
            return Ok(new { token = darkDev.tokenValidationAction.GenerateToken(Result) });
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
