﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthIntegratedAPI.Helpers
{
    public class JwtHelper
    {
       public static JwtSecurityToken GetJwtToken(
           string username,
           string signingKey,
           string issuer,
           string audience,
           TimeSpan expiration,
           string securityKey,
           Claim[] additionalClaims = null)
       {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,username),
                // this guarantees the token is unique
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }

        internal static object GetJwtToken(string username, SymmetricSecurityKey symmetricSecurityKey, string v1, string v2, DateTime dateTime, Claim[] claims, string securityKey)
        {
            throw new NotImplementedException();
        }
    }
}
