using Integration.Api.Middlewares;
using Integration.Application.Interfaces.Audit;
using Integration.Application.Interfaces.Security;
using Integration.Application.Mappings.Audit;
using Integration.Application.Mappings.Security;
using Integration.Application.Services.Audit;
using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Core.MappingProfiles;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Interfaces.UnitOfWork;
using Integration.Infrastructure.Repositories.Audit;
using Integration.Infrastructure.Repositories.Security;
using Integration.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configurar conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IntegrationConnection")));

// Registro de AutoMapper
builder.Services.AddAutoMapper(typeof(LogProfile)); 
builder.Services.AddAutoMapper(typeof(ApplicationProfile));
builder.Services.AddAutoMapper(typeof(ModuleProfile));
builder.Services.AddAutoMapper(typeof(PermissionProfile));
builder.Services.AddAutoMapper(typeof(RoleProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(RoleModuleProfile));
builder.Services.AddAutoMapper(typeof(UserRoleProfile));

builder.Services.AddTransient<IApplicationDbUOW, ApplicationDbUOW>();

// Si el servicio no es genérico, registra la implementación específica
builder.Services.AddSingleton<IJwtService, JwtService>(); 
builder.Services.AddScoped<ILogService, LogService>(); 
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IRoleModuleService, RoleModuleService>();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IRoleModuleRepository, RoleModuleRepository>();

// Configurar Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// Configurar autenticación JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});
builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
          .Enrich.WithThreadId()
          .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();