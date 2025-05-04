using System.Text;
using Medicare_API.Controllers;
using Medicare_API.Data;
using Medicare_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

        // Registra o DataContext com o contêiner de serviços
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SomeeConnection")));

        // Outros serviços e configurações
        builder.Services.AddControllers();

// Adiciona os serviços necessários para controllers
builder.Services.AddControllers();

// Configuração do Swagger (se estiver usando)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Habilita a validação da chave de assinatura do token
        ValidateIssuerSigningKey = true,

        // Define a chave de segurança usada para validar o token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetSection("ConfiguracaoToken:Chave").Value)),

        // Desabilita a validação do emissor (Issuer), útil quando o backend não especifica um emissor fixo
        ValidateIssuer = false,

        // Desabilita a validação do público (Audience), permitindo que qualquer cliente utilize o token
        ValidateAudience = false
    };
});


var app = builder.Build();

// Habilita o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o roteamento e mapeia as controllers
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();  // Mapeia as controllers para que a API saiba qual caminho seguir

app.Run();
