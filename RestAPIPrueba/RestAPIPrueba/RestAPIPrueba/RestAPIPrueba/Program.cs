using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestAPIPrueba.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*INICIO - ESTA SECCION ES PARA el JWT TOKEN*/
var key = Encoding.ASCII.GetBytes("kN#e8R*VZ9m!P7xq^W@d4G$Tfq2L!a1b");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});
/*FIN - ESTA SECCION ES PARA el JWT TOKEN*/

/*INICIO - ESTA SECCION ES PARA LA CONEXION DE BASE DE DATOS.*/
builder.Services.AddDbContext<ActividadUnidad4Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*FIN - ESTA SECCION ES PARA LA CONEXION DE BASE DE DATOS.*/

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/*INICIO - ESTA SECCION ES PARA el JWT TOKEN*/
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});
/*FIN - ESTA SECCION ES PARA el JWT TOKEN*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
