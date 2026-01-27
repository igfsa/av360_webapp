using System.Text.Json.Serialization;
using Application.Contracts;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

using Persistence;
using Persistence.Context;
using Persistence.Contracts;
using Domain.Entities;
using Application.DTOs;
using API.Hubs;
using API.Notifier;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowlocalhost",
        policy => policy
            .WithOrigins("http://localhost:4200") // seu front-end
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        )
        .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

// builder.Services.AddControllers().AddNewtonsoftJson(options =>
// {
//     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
// });

// builder.Services.AddControllers().AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
// });

// Configurando string de conexão. Usadas variáveis de ambiente locais.

builder.Services.AddSignalR();

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
    cfg.CreateMap<Criterio, CriterioDTO>().ReverseMap();
    cfg.CreateMap<Grupo, GrupoDTO>().ReverseMap();
    cfg.CreateMap<Sessao, SessaoDTO>().ReverseMap();
    cfg.CreateMap<Turma, TurmaDTO>().ReverseMap();
});

builder.Services.AddScoped<IGeralPersist, GeralPersist>();

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAlunoPersist, AlunoPersist>();

builder.Services.AddScoped<ICriterioService, CriterioService>();
builder.Services.AddScoped<ICriterioPersist, CriterioPersist>();
builder.Services.AddScoped<ICriterioNotifier, CriterioNotifier>();

builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<IGrupoPersist, GrupoPersist>();

builder.Services.AddScoped<ISessaoService, SessaoService>();
builder.Services.AddScoped<ISessaoPersist, SessaoPersist>();

builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<ITurmaPersist, TurmaPersist>();
builder.Services.AddScoped<ITurmaNotifier, TurmaNotifier>();

builder.Services.AddScoped<IAlunoTurmaPersist, AlunoTurmaPersist>();
builder.Services.AddScoped<ICriterioTurmaPersist, CriterioTurmaPersist>();
builder.Services.AddScoped<IAlunoGrupoPersist, AlunoGrupoPersist>();

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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

app.MapHub<TurmaHub>("/hubs/turma");
app.MapHub<CriterioHub>("/hubs/criterio");

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
