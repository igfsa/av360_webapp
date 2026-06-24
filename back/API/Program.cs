using Microsoft.EntityFrameworkCore;
using System.Text;
using StackExchange.Redis;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;

using API.Hubs;
using API.Notifier;
using Application.Contracts;
using Application.Services;
using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Persistence;
using Persistence.Context;
using Persistence.Contracts;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()

    .WriteTo.Console()

    .WriteTo.File(
        new RenderedCompactJsonFormatter(),
        "logs/log-.json",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)

    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
        policy.WithOrigins("http://localhost:4000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());

    options.AddPolicy("ProductionPolicy", policy =>
        policy.WithOrigins("https://webav360.riss.com.br", "https://av360-webapp.vercel.app")
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

builder.Services.AddDbContext<APIContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention()
    );

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    Console.WriteLine($"Redis:{configuration["Redis:Connection"]}");

    var options = ConfigurationOptions.Parse(configuration["Redis:Connection"]!);

    options.AbortOnConnectFail = false;
    options.ConnectRetry = 5;
    options.ConnectTimeout = 15000;
    options.SyncTimeout = 15000;

    options.AllowAdmin = true;

    return ConnectionMultiplexer.Connect(options);
});

var mapper_config = builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["Automapper:Key"];
    _ = cfg.CreateMap<Aluno, AlunoDTO>().ReverseMap();
    _ = cfg.CreateMap<AlunoGrupo, AlunoGrupoDTO>().ReverseMap();
    _ = cfg.CreateMap<Criterio, CriterioDTO>().ReverseMap();
    _ = cfg.CreateMap<Grupo, GrupoDTO>().ReverseMap();
    _ = cfg.CreateMap<NotaFinal, NotaFinalDTO>().ReverseMap();
    _ = cfg.CreateMap<NotaParcial, NotaParcialDTO>().ReverseMap();
    _ = cfg.CreateMap<Professor, ProfessorDTO>().ForMember(dest => dest.SenhaHash, opt => opt.Ignore());
    _ = cfg.CreateMap<ProfessorDTO, Professor>();
    _ = cfg.CreateMap<Sessao, SessaoDTO>().ReverseMap();
    _ = cfg.CreateMap<Turma, TurmaDTO>().ReverseMap();
    _ = cfg.CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoSessao, ResultadoSessaoDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoNotaFinal, ResultadoNotaFinalDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoNotaParcial, ResultadoNotaParcialDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoAluno, ResultadoAlunoDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoGrupo, ResultadoGrupoDTO>().ReverseMap();
    _ = cfg.CreateMap<ResultadoCriterio, ResultadoCriterioDTO>().ReverseMap();
});

builder.Services.AddScoped<IGeralPersist, GeralPersist>();

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IAlunoPersist, AlunoPersist>();

builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();

builder.Services.AddScoped<ICriterioService, CriterioService>();
builder.Services.AddScoped<ICriterioPersist, CriterioPersist>();
builder.Services.AddScoped<ICriterioNotifier, CriterioNotifier>();

builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<IGrupoPersist, GrupoPersist>();
builder.Services.AddScoped<IGrupoNotifier, GrupoNotifier>();

builder.Services.AddScoped<INotaFinalPersist, NotaFinalPersist>();

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

builder.Services.AddScoped<IDashboardCacheService, DashboardCacheService>();
builder.Services.AddScoped<IDashboardSessaoService, DashboardSessaoService>();

builder.Services.AddScoped<IResultadoPersist, ResultadoPersist>();

builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IProfessorPersist, ProfessorPersist>();
builder.Services.AddScoped<IProfessorNotifier, ProfessorNotifier>();

builder.Services.AddScoped<IRefreshTokenPersist, RefreshTokenPersist>();

builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();

builder.Services.AddScoped<IExportService, ExportService>();

builder.Services.AddScoped<JWTToken>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
        jwtOptions.Events = new ()
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["auth_token"];

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();
    await DbInitializer.Seed(scope.ServiceProvider);
    return;
}

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

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// PROTEÇÃO SSR
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    var isApi = path.StartsWith("/api");
    var isHub = path.StartsWith("/hubs");
    var isStatic = Path.HasExtension(path);

    var rotasPublicas = new [] 
    {
        "/login",
        "/avaliacao/publica"
    };

    var isPublic = rotasPublicas.Any(p => path.StartsWith(p));

    var isHtmlRequest = context.Request.Headers.Accept.Any(h => h.Contains("text/html"));

    if (!isApi && !isHub && !isPublic && !isStatic && isHtmlRequest)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.Redirect("/login");
            return;
        }
    }

    await next();
});

app.Use(async (context, next) =>
{
    context.Response.Headers.ContentSecurityPolicy =
        "default-src 'self'; " +
        "img-src 'self' data:; " +
        "script-src 'self'; " +
        "style-src 'self' 'unsafe-inline';" +
        "connect-src 'self' https://webav360.riss.com.br http://localhost:4000;";

    await next();
});

app.UseMiddleware<ExceptionMiddleware>();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    version = "1.0"
}));

app.MapGet("/info", () => Results.Ok(new
{
    Name = "WebAV360 API",
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName,
    Time = DateTime.UtcNow
}));

app.MapControllers();

app.MapHub<TurmaHub>("/hubs/turma");
app.MapHub<CriterioHub>("/hubs/criterio");
app.MapHub<GrupoHub>("/hubs/grupo");
app.MapHub<SessaoHub>("/hubs/sessao");
app.MapHub<ProfessorHub>("/hubs/professor");

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.Seed(scope.ServiceProvider);
}

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação encerrou inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}