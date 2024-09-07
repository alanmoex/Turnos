using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static Infrastructure.Services.AuthenticationService;
//API-CheTurnosBearerAuth

//se traba aca.
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen(setupAction =>
{
    //Esto va a permitir usar swagger con el token.
    setupAction.AddSecurityDefinition("API-CheTurnosBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Pegar el token generado"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "API-CheTurnosBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
                }, new List<string>() }
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

string connectionString = builder.Configuration["ConnectionStrings:DBConnectionString"]!;

var connection = new SqliteConnection(connectionString);
connection.Open();

//--Base de datos--
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connection, b => b.MigrationsAssembly("Infrastructure"));
    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));
});

//Bearer es el tipo de autenticacion en el postman para pasarle el token
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"]))
        };
    });

//--Repositories--
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();


//--Service--
builder.Services.Configure<AuthenticationServiceOptions>(
    builder.Configuration.GetSection(AuthenticationServiceOptions.AuthenticationService));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();



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
