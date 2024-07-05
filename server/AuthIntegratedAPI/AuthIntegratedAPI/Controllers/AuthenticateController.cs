using AuthIntegratedAPI.Helpers;
using AuthIntegratedAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthIntegratedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private IConfiguration _config;
        public IConfiguration Configuration { get; }
        public AuthenticateController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] TokenModel token)
        {
            IActionResult response = Unauthorized();
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token.accessToken))
            {
                Console.WriteLine("Invalid token.");
                return response;
            }

            var jwtToken = handler.ReadJwtToken(token.accessToken);

            var claims = jwtToken.Claims.Select(claim => new
            {
                claim.Type,
                claim.Value
            }).ToList();

            var header = jwtToken.Header.Select(h => new
            {
                h.Key,
                h.Value
            }).ToList();

            var payload = jwtToken.Payload.Select(p => new
            {
                p.Key,
                p.Value
            }).ToList();

            //var user = AuthenticateUser(login);

            //if (user != null)
            //{
            //    var tokenString = GenerateJSONWebToken(user);
            //    response = Ok(new { token = tokenString });
            //}
            var tokenString = GenerateJSONWebToken();
            response = Ok(new { token = tokenString });

            return response;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim("username", "test"));
            claims.Add(new Claim("displayname", "test"));
            claims.Add(new Claim("subject", "WEPApp"));
            var token = JwtHelper.GetJwtToken(
                "test",
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])).ToString(),
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                TimeSpan.FromMinutes(120),
                securityKey.ToString(),
                claims.ToArray());

            //var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            //  _config["Jwt:Issuer"],
            //  null,
            //  expires: DateTime.Now.AddMinutes(120),
            //  signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
