using Application;
using Application.Validators.Products;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Filters;
using Infrastructure.Services.Storage.Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Serilog;
using Serilog.Core;
using System.Text;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using WebAPI.Configurations.ColumnWriters;
using System.Security.Claims;
using Persistence.Contexts;
using Serilog.Context;
using Microsoft.AspNetCore.HttpLogging;
using WebAPI.Extensions;
using SignalR;
using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistenceServices();

builder.Services.AddInfrastructureServices();

builder.Services.AddApplicationServices();

//builder.Services.AddStorageWithEnums(StorageType.Local);//Switch-case yerine generic yapý daha doðru
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddSignalRServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
                //Fluent Validationu devreye soktuk
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);


SqlColumn sqlColumn = new SqlColumn();
sqlColumn.ColumnName = "UserName";
sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
sqlColumn.PropertyName = "UserName";
sqlColumn.DataLength = 50;
sqlColumn.AllowNull = true;
ColumnOptions columnOpt = new ColumnOptions();
columnOpt.Store.Remove(StandardColumn.Properties);
columnOpt.Store.Add(StandardColumn.LogEvent);
columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };



Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
    connectionString: builder.Configuration.GetConnectionString("MsSql"),
     sinkOptions: new MSSqlServerSinkOptions
     {
         AutoCreateSqlTable = true,
         TableName = "logs",
     },
     appConfiguration: null,
     columnOptions: columnOpt
    )
    .Enrich.FromLogContext()
    .Enrich.With<CustomUserNameColumn>()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin", options =>
{
    options.TokenValidationParameters = new()
    {//Token doðrulanýrken hangi verilerle doðrulanacaðý burda yer alýyor
        ValidateAudience = true,//oluþturulacak token deðerini kimlerin kullanýcý belirlediðimiz deðerdir => www.bilmemne.com
        ValidateIssuer = true,//Oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr. => www.myapi.com
        ValidateLifetime = true,//Oluþturulan token deðerinin süresini kontrol edecek doðrulamadýr.
        ValidateIssuerSigningKey = true,//Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulanmasýdýr.

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, ValidationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        //expires null olmadýðý sürece expires datetimenowdan büyük olduðu sürece geçerlidir. küçük olduðu anda false olur.

        NameClaimType = ClaimTypes.Name //Jwt üzerinde name claimine karþýlýk gelen deðeri User.Identity.Name ile elde edebiliriz.
    };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (EntityFrameworkDbContext, next) =>
{
    var username = EntityFrameworkDbContext.User?.Identity?.IsAuthenticated != null || true ? EntityFrameworkDbContext.User.Identity.Name : null;
    LogContext.PushProperty("UserName", username);
    await next();
});

app.MapControllers();
app.MapHubs();


app.Run();
