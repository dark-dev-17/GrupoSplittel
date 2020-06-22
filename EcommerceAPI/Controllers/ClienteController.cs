using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EcomDataProccess;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly EcommerceAPI.Services.Cliente Cliente;
        private readonly IConfiguration configuration;

        // TRAEMOS EL OBJETO DE CONFIGURACIÓN (appsettings.json)
        // MEDIANTE INYECCIÓN DE DEPENDENCIAS.
        public ClienteController(IConfiguration configuration)
        {
            this.configuration = configuration;
            Cliente = new Services.Cliente();
        }

        // POST: api/Login
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserCredentials Credentials)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _userInfo = Cliente.Get(Credentials.Email, Credentials.Password);
                    if (_userInfo != null)
                    {
                        return Ok(new { token = GenerarTokenJWT(_userInfo) });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                   return BadRequest(ModelState); 
                }
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Ecom_Cliente>> Get()
        {
            try
            {
                var currentUser = HttpContext.User;
                if (currentUser.HasClaim(c => c.Type == "rol") && currentUser.Claims.FirstOrDefault(c => c.Type == "rol").Value == "cliente")
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(Cliente.Get());
                }
                    
            }
            catch (Ecom_Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Ecom_Cliente> Get(int id)
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "rol") && currentUser.Claims.FirstOrDefault(c => c.Type == "rol").Value == "cliente")
            {
                //
                Ecom_Cliente Cliente_ = Cliente.Get(id);
                if (Cliente_ == null)
                {
                    return BadRequest("No se encontró el cliente");
                }
                if (currentUser.HasClaim(c => c.Type == "id") && currentUser.Claims.FirstOrDefault(c => c.Type == "id").Value == Cliente_.Id_cliente + "")
                {
                    return Ok(Cliente_);
                }
                else
                {
                    //return BadRequest("Esta información no te pertenece");
                    return Unauthorized();
                }
            }
            //else if(currentUser.HasClaim(c => c.Type == "rol") && currentUser.Claims.FirstOrDefault(c => c.Type == "rol").Value == "administrador")
            //{
            //    return Ok(Cliente_);
            //}
            else
            {
                return Unauthorized();
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

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(Ecom_Cliente usuarioInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:FibremexKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id_cliente.ToString()),
                new Claim("nombre", usuarioInfo.Nombre),
                new Claim("apellidos", usuarioInfo.Apellidos),
                new Claim("rol", "cliente"),
                new Claim("id", usuarioInfo.Id_cliente.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
            //// CREAMOS EL HEADER //
            //var _symmetricSecurityKey = new SymmetricSecurityKey(
            //        Encoding.UTF8.GetBytes(configuration["JWT:FibremexKey"])
            //    );
            //var _signingCredentials = new SigningCredentials(
            //        _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
            //    );
            //var _Header = new JwtHeader(_signingCredentials);

            //// CREAMOS LOS CLAIMS //
            //var _Claims = new[] {
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id_cliente.ToString()),
            //    new Claim("nombre", usuarioInfo.Nombre),
            //    new Claim("apellidos", usuarioInfo.Apellidos),
            //    new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email),
            //    //new Claim(ClaimTypes.Role, usuarioInfo.Rol)
            //};

            //// CREAMOS EL PAYLOAD //
            //var _Payload = new JwtPayload(
            //        issuer: configuration["JWT:Issuer"],
            //        audience: configuration["JWT:Audience"],
            //        claims: _Claims,
            //        notBefore: DateTime.UtcNow,
            //        // Exipra a la 24 horas.
            //        expires: DateTime.UtcNow.AddHours(24)
            //    );

            //// GENERAMOS EL TOKEN //
            //var _Token = new JwtSecurityToken(
            //        _Header,
            //        _Payload
            //    );

            //return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}
