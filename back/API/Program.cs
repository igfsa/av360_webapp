using System.Text.Json.Serialization;
using Application.Contracts;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

using Persistence;
using Persistence.Context;
using Persistence.Contracts;
using Domain.Entities;
using Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowlocalhost",
        policy => policy
            .WithOrigins("http://localhost:4200") // seu front-end
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configurando string de conexão. Usadas variáveis de ambiente locais.

string mySqlConnection = (
    "Server=" + Environment.GetEnvironmentVariable("DB_AV360_SERVER") +
    ";DataBase=" + Environment.GetEnvironmentVariable("DB_AV360_NAME") +
    ";Uid=" + Environment.GetEnvironmentVariable("DB_AV360_USER") +
    ";Pwd=" + Environment.GetEnvironmentVariable("DB_AV360_PASSWORD")
);

builder.Services.AddDbContext<APIContext>(options =>
    options.UseMySql(mySqlConnection,
    ServerVersion.AutoDetect(mySqlConnection)
));

var mapper_config = builder.Services.AddAutoMapper(cfg => 
{
    cfg.LicenseKey = Environment.GetEnvironmentVariable("AUTOMAPPER_KEY");
    cfg.CreateMap<Aluno, AlunoDTO>().ReverseMap();
    cfg.CreateMap<Turma, TurmaDTO>().ReverseMap();
    cfg.CreateMap<Criterio, CriterioDTO>().ReverseMap();
});

builder.Services.AddScoped<IGeralPersist, GeralPersist>();

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAlunoPersist, AlunoPersist>();

builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<ITurmaPersist, TurmaPersist>();

builder.Services.AddScoped<IAlunoTurmaPersist, AlunoTurmaPersist>();

builder.Services.AddScoped<ICriterioService, CriterioService>();
builder.Services.AddScoped<ICriterioPersist, CriterioPersist>();

var app = builder.Build();

app.UseCors("Allowlocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
