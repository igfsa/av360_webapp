using Application.Contracts;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
        .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

builder.Services.AddSignalR();

string npgConnection = (
    "Host=" + Environment.GetEnvironmentVariable("DB_AV360_SERVER") +
    ";DataBase=" + Environment.GetEnvironmentVariable("DB_AV360_NAME") +
    ";Uid=" + Environment.GetEnvironmentVariable("DB_AV360_USER") +
    ";Pwd=" + Environment.GetEnvironmentVariable("DB_AV360_PASSWORD")
);

builder.Services.AddDbContext<APIContext>(options =>
    options.UseNpgsql(npgConnection)
        .UseSnakeCaseNamingConvention()
    );

var mapper_config = builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = Environment.GetEnvironmentVariable("AUTOMAPPER_KEY");
    _ = cfg.CreateMap<Aluno, AlunoDTO>().ReverseMap();
    _ = cfg.CreateMap<Criterio, CriterioDTO>().ReverseMap();
    _ = cfg.CreateMap<Grupo, GrupoDTO>().ReverseMap();
    _ = cfg.CreateMap<NotaFinal, NotaFinalDTO>().ReverseMap();
    _ = cfg.CreateMap<NotaParcial, NotaParcialDTO>().ReverseMap();
    _ = cfg.CreateMap<Sessao, SessaoDTO>().ReverseMap();
    _ = cfg.CreateMap<Turma, TurmaDTO>().ReverseMap();
});

builder.Services.AddScoped<IGeralPersist, GeralPersist>();

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAlunoPersist, AlunoPersist>();

builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IAvaliacaoNotifier, AvaliacaoNotifier>();

builder.Services.AddScoped<ICriterioService, CriterioService>();
builder.Services.AddScoped<ICriterioPersist, CriterioPersist>();
builder.Services.AddScoped<ICriterioNotifier, CriterioNotifier>();

builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<IGrupoPersist, GrupoPersist>();
builder.Services.AddScoped<IGrupoNotifier, GrupoNotifier>();

builder.Services.AddScoped<INotaFinalService, NotaFinalService>();
builder.Services.AddScoped<INotaFinalPersist, NotaFinalPersist>();

builder.Services.AddScoped<INotaParcialService, NotaParcialService>();
builder.Services.AddScoped<INotaParcialPersist, NotaParcialPersist>();

builder.Services.AddScoped<ISessaoService, SessaoService>();
builder.Services.AddScoped<ISessaoPersist, SessaoPersist>();
builder.Services.AddScoped<ISessaoNotifier, SessaoNotifier>();

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
    _ = app.MapOpenApi();

    _ = app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.MapHub<TurmaHub>("/hubs/turma");
app.MapHub<CriterioHub>("/hubs/criterio");
app.MapHub<GrupoHub>("/hubs/grupo");
app.MapHub<AvaliacaoHub>("/hubs/avaliacao");
app.MapHub<SessaoHub>("/hubs/sessao");

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
