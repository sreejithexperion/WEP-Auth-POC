using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class AuthServerConfigs
{
    public static List<TokenValidationParameters> GetTokenValidationParameters(string? key)
    {
        return new List<TokenValidationParameters>
        {
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:5212",
                ValidateAudience = true,
                ValidAudience = "testIdentityServer4mvc",
                ValidateLifetime = true,
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuerSigningKey = false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                {
                    var jwt = new JwtSecurityToken(token);
                    return jwt;
                },
            }
            // new TokenValidationParameters
            // {
            //     ValidateIssuer = true,
            //     ValidIssuer = "https://trusted-issuer-2.com",
            //     ValidateAudience = true,
            //     ValidAudience = "your-api",
            //     ValidateLifetime = true,
            //     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("signing-key-2")),
            //     ValidateIssuerSigningKey = true,
            // },
            // new TokenValidationParameters
            // {
            //     ValidateIssuer = true,
            //     ValidIssuer = "https://trusted-issuer-3.com",
            //     ValidateAudience = true,
            //     ValidAudience = "your-api",
            //     ValidateLifetime = true,
            //     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("signing-key-3")),
            //     ValidateIssuerSigningKey = true,
            // }
        };
    }
}