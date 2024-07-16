using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var jwtSettings = builder.Configuration.GetSection("Jwt");
// var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
// var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

var key = jwtSettings["Key"];
// RSA rsa = RSA.Create();
// var rsaSecurityKey = new RsaSecurityKey(rsa)
// {
//     KeyId = key
// };

var rsa = new RSACryptoServiceProvider(2048);
rsa.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);

var signingKey = new RsaSecurityKey(rsa);
// RSAParameters rsaParams = rsa.ExportParameters(true); // includePrivateParameters: true

// Create signing credentials
var signingCredentials = new SigningCredentials(
    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
    SecurityAlgorithms.RsaSha256);
    
builder.Services.AddIdentityServer()
        //.AddSigningCredential(signingCredentials)
        .AddSigningCredential(new X509Certificate2("C:\\Program Files\\OpenSSL-Win64\\bin\\localhost.pfx", "Exp@123"))
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        .AddInMemoryApiResources(Config.GetApiResources());

// builder.WebHost.ConfigureKestrel(serverOptions =>
// {
//     serverOptions.ConfigureEndpointDefaults(listenOptions =>
//     {
//         listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
//     });
// });


// builder.Services.AddIdentityServer(options =>
//     {
//         options.IssuerUri = "http://localhost:5212";
//     })
//     .AddInMemoryIdentityResources(Config.IdentityResources)
//     //.AddInMemoryApiScopes(new List<ApiScope>
//     //{
//     //    new ApiScope("api1", "My API")
//     //})
//     .AddInMemoryClients(Config.Clients)
//     .AddTestUsers(Config.Users)
//     .AddSigningCredential(new X509Certificate2("C:\\Program Files\\OpenSSL-Win64\\bin\\localhost.pfx", "Exp@123"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});

// builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");
app.UseIdentityServer();

//  app.UseAuthentication();
//  app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
