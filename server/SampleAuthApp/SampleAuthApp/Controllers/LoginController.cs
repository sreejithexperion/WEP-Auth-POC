using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleAuthApp.Helpers;
using SampleAuthApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public IConfiguration Configuration { get; }
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials
            
            if (login.Username == "sreejith")
            {
                user = new UserModel { Username = "sreejith", Password = "abc", Name = "Sreejith" };
            }
            return user;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim("username", userInfo.Username));
            claims.Add(new Claim("displayname", userInfo.Name));
            claims.Add(new Claim("subject", "App1"));
            var token = JwtHelper.GetJwtToken(
                userInfo.Username,
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
