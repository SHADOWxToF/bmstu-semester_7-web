using NLog;
using BL.Models.Interfaces;
using BL.Models.Implementations;
using Npgsql;
using DataAccess.DA.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WEBServer.APIControllers.Implementations;

var builder = WebApplication.CreateBuilder(args);
// мой код
// Logger

Logger.Logger.SetNLogConfig();
LogManager.GetCurrentClassLogger().Info("Приложение запущено");

// инъекция зависимостей

string? DBConnection = Environment.GetEnvironmentVariable("DBConnection");
if (DBConnection == null)
    return;

NpgsqlConnection connection = new(DBConnection);
DisciplineRepository dRepository = new(connection);
TaskRepository tRepository = new(connection);
NotificationRepository nRepository = new(connection);
UserRepository uRepository = new(connection);

IDiscipline discipline = new Discipline(dRepository, tRepository);
INotification calendar = new Notification(dRepository, tRepository, nRepository);
IUser user = new User(uRepository);

builder.Services.AddSingleton<INotification>(calendar);
builder.Services.AddSingleton<IDiscipline>(discipline);
builder.Services.AddSingleton<IUser>(user);

// авторизация



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,

                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

// конец моего кода

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger => 
{ 
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
              new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger(options => options.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        // Чтение заголовка 'X-Base-Path' из запроса
        var basePath = httpReq.Headers["X-Base-Path"].FirstOrDefault();

        // Если заголовок существует и не пустой, используйте его значение
        if (!string.IsNullOrEmpty(basePath))
        {
            swagger.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = $"/{basePath}" }
        };
        }
        else // Иначе используйте дефолтный путь
        {
            swagger.Servers = new List<OpenApiServer>
        {
                  new OpenApiServer { Url = $""}
        };
        }
    }));
app.UseSwaggerUI();
//}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
