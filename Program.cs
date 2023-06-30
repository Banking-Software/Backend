using System.Text.Json.Serialization;
using MicroFinance.DBContext;
using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.SeedData;
using MicroFinance.ServiceExtensions.ApplicationService;
using MicroFinance.ServiceExtensions.DbService;
using MicroFinance.ServiceExtensions.IdentityService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB CONTEXT REGISTRY

builder.Services.AddDbServiceExtension(builder.Configuration);

// Identity Service /////////////////////////////////////////////////////
builder.Services.AddSuperAdminIdentityServiceAsync(builder.Configuration);
builder.Services.AddApplicationServiceExtension();
builder.Services.AddUserIdentityServiceAsync(builder.Configuration);
////////////////////////////////////////////////////////////////////////

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowCors", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();


// ADD IMPORTANT DATA

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    int retryLimit = 3;
    int currentRetry = 0;
    bool success = false;
    while (!success && currentRetry <= retryLimit)
    {
        try
        {

            var superAdminDbContext = services.GetRequiredService<SuperAdminDbContext>();
            await superAdminDbContext.Database.MigrateAsync();

            var superAdminUserManager = services.GetRequiredService<UserManager<SuperAdmin>>();
            await UserDbContextSeed.SeedSuperAdminRoleAsync(superAdminDbContext, superAdminUserManager);

            var userDbContext = services.GetRequiredService<UserDbContext>();
            await userDbContext.Database.MigrateAsync();

            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();

            await LedgerDbContextSeed.SeedMainLedgerAsync(dbContext);
            await ClientDbContextSeed.SeedClientInfoAsync(dbContext);
            await RecordsWithCode.SeedRecordsWithCode(dbContext);

            //await DepositDbContextSeed.SeedPostSchemeAsync(dbContext);
            
            success=true;
            currentRetry=4;

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError($"{DateTime.Now}: {ex}");
            currentRetry++;
        }
    }

}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
// }
app.UseCors("allowCors");
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.AddGlobalErrorHandler();
app.UseSerilogRequestLogging();

app.Run();
