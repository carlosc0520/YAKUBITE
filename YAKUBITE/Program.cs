using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YKT.CONFIG;
using Microsoft.AspNetCore.Http.Features;
using YKT_DATOS_CONSULTAS.ADMIN;
using YKT_DATOS_CONSULTAS.LOGIN;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins(
        "http://localhost:3000"
        )
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddScoped<CustomPageFilter>();

builder.Services.AddMediatR(Assembly.Load("YKT_DATOS_EVENTOS"));
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfiguracionProyecto.CAPTCHA.SecretKey))
      };
  });


builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = 1L * 1024 * 1024 * 1024; // 1GB en bytes
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Configurar Kestrel para permitir archivos más grandes
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1L * 1024 * 1024 * 1024; // 1GB en bytes
});

builder.Services.AddTransient<ITokenValidationService, TokenValidationService>();


#region ADMINISTRADOR
builder.Services.AddTransient<IConsultasRestaurant, ConsultasRestaurant>();
builder.Services.AddTransient<IConsultasUsuario, ConsultasUsuario>();
builder.Services.AddTransient<IConsultasComunidad, ConsultasComunidad>();
#endregion ADMINISTRADOR

#region SEGURIDAD
builder.Services.AddTransient<IConsultasLogin, ConsultasLogin>();
#endregion SEGURIDAD


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


