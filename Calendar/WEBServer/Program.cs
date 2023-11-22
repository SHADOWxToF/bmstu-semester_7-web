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
// ��� ���
// Logger

Logger.Logger.SetNLogConfig();
LogManager.GetCurrentClassLogger().Info("���������� ��������");

// �������� ������������

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

// �����������



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ��������, ����� �� �������������� �������� ��� ��������� ������
                        ValidateIssuer = true,
                        // ������, �������������� ��������
                        ValidIssuer = AuthOptions.ISSUER,

                        // ����� �� �������������� ����������� ������
                        ValidateAudience = true,
                        // ��������� ����������� ������
                        ValidAudience = AuthOptions.AUDIENCE,
                        // ����� �� �������������� ����� �������������
                        ValidateLifetime = true,

                        // ��������� ����� ������������
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                });

// ����� ����� ����

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
        // ������ ��������� 'X-Base-Path' �� �������
        var basePath = httpReq.Headers["X-Base-Path"].FirstOrDefault();

        // ���� ��������� ���������� � �� ������, ����������� ��� ��������
        if (!string.IsNullOrEmpty(basePath))
        {
            swagger.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = $"/{basePath}" }
        };
        }
        else // ����� ����������� ��������� ����
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
