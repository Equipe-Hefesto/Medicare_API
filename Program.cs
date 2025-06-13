using System.Text;
using System.Text.Json;
using Medicare_API.Controllers;
using Medicare_API.Data;
using Medicare_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Registra o DataContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SomeeConnection"))
           .EnableSensitiveDataLogging());

// 2. Configura Identity
//builder.Services.AddIdentity<Utilizador, IdentityRole<int>>()
//  .AddEntityFrameworkStores<DataContext>()
//.AddDefaultTokenProviders();

// 3. Configura envio de e-mails (se implementado)
builder.Services.AddTransient<IEmailSender, EmailSender>();

// 4. Configura Serialização JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// 5. Configuração do JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(builder.Configuration["ConfiguracaoToken:Chave"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// 6. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------

var app = builder.Build();

// 7. Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication(); // JWT vem antes do Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
