using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

public class TokenExchangeGrantValidator : IExtensionGrantValidator
{
    private readonly ITokenValidator _tokenValidator;
    private readonly IConfiguration _config;
    public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";

    public TokenExchangeGrantValidator(ITokenValidator tokenValidator, IConfiguration configuration)
    {
        _tokenValidator = tokenValidator;
        _config = configuration;
    }

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var request = context.Request.Raw;
        var subjectToken = request.Get("subject_token");
        var subjectTokenType = request.Get("subject_token_type");
        //var requestedTokenType = context.Request.Raw.Get("requested_token_type");

        // Perform validation of the subject token
        if (subjectToken != null && subjectTokenType != null)
        {
             //var validationResult = await _tokenValidator.ValidateAccessTokenAsync(subjectToken);
            var principal = ValidateJwt(subjectToken);
            if (principal == null)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, 
                    "Invalid subject token");
                return;
            }

            // if (validationResult.IsError)
            // {
            //     context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid subject token");
            //     return;
            // }

            var subjectClaims = principal.Claims.ToList();

            var modifiedClaims = ModifyClaims(subjectClaims);
            
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Issuer, "wep_identity"),
                new Claim(JwtClaimTypes.Audience, "wep_client"),
                //new Claim(JwtClaimTypes.Subject, subjectClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value ?? ""),
                //new Claim(JwtClaimTypes.AuthenticationTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                // Add other claims as necessary
            };

            context.Result = new GrantValidationResult(
                subject: "exc_server",
                authenticationMethod: GrantType,
                claims: claims);
        }
        else
        {
            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant, 
                "Invalid subject token");
        }
    }

    private ClaimsPrincipal ValidateJwt(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = _config["Jwt:Key"];
        var validationParametersList = AuthServerConfigs.GetTokenValidationParameters(jwtKey);

        foreach (var validationParameters in validationParametersList)
        {
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return null;
    }

    private List<Claim> ModifyClaims(List<Claim> claims)
    {
        var issuerClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Issuer);
        if (issuerClaim != null)
        {
            claims.Remove(issuerClaim); // Remove existing issuer claim
            claims.Add(new Claim(JwtClaimTypes.Issuer, "wep_identity")); // Add new issuer claim
        }

        var audienceClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Audience);
        if (audienceClaim != null)
        {
            claims.Remove(audienceClaim); // Remove existing audience claim
            claims.Add(new Claim(JwtClaimTypes.Audience, "wep_client")); // Add new audience claim
        }

        return claims;
    }
}
