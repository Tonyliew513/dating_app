using angular_API.DbContexts;
using angular_API.Repositories;
using angular_API.Util;
using Serilog;
using SharedModel.SerilogModels;


string environmentName = string.Empty;
string? projectName = string.Empty;

#if DEBUG
environmentName = "Development";
#elif STAGING
environmentName = "Staging";
#elif RELEASE
environmentName = "Production";
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//adding serilog
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.AddDebug();
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger("Program");
try
{
    projectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
    logger.LogInformation($"{projectName} ------->  Application Starting..............................");
    var appSettings = Init(logger);
    logger.LogInformation($"{projectName}-------> into CreateHostBuilder method...............");

    builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Debug()
    .ReadFrom.Configuration(ctx.Configuration));

    //builder.WebHost
    //    .CaptureStartupErrors(true)
    //    .UseEnvironment(environmentName)
    //    .UseStaticWebAssets()
    //    .UseKestrel(options =>
    //    {
    //        Uri uri = new Uri(appSettings?.KestrelWebServer?.Endpoints?.Http?.IPAddress);
    //        options.Listen(IPAddress.Parse(uri.Host), Convert.ToInt32(uri.Port));
    //        //options.ListenAnyIP(Convert.ToInt32(uri.Port));
    //        options.Limits.MaxConcurrentConnections = 100;
    //        options.Limits.MaxConcurrentUpgradedConnections = 100;
    //        options.Limits.MaxRequestBodySize = 52428800;
    //        options.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
    //        options.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
    //        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    //        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);

    //    });
}
catch (Exception ex)
{
    logger.LogCritical(ex, "The Application failed to start.");
    logger.LogError(ex, Environment.StackTrace, ex.InnerException);
}

//DbContext Setup
var dbConnectionString = GlobalVariable.DBConnectionString;
var dbOptionsBuilder = ProgramStartUpUtil.DbOptionsBuilder(dbConnectionString);
builder.Services.AddDbContext<dating_appContext>(dbOptionsBuilder, ServiceLifetime.Transient);


builder.Services.AddScoped<IUsersRepository, UsersRepository>();

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

AppSettings_Service? Init(Microsoft.Extensions.Logging.ILogger logger)
{

    //Console.ForegroundColor = ConsoleColor.Green;
    //Console.WriteLine("Application Starting...............");

    logger.LogInformation($"{projectName} ------->  in Init method...............");

    var appSettings = new AppSettings_Service();

    appSettings.EnvironmentName = environmentName;

    var _configurationSettings = new ConfigurationBuilder().AddJsonFile($"appsettings.{appSettings.EnvironmentName}.json").Build();

    _configurationSettings.GetSection("AppSettings").Bind(appSettings);

    logger.LogInformation($"{projectName} ------->  strIPAddress.................. {appSettings?.KestrelWebServer?.Endpoints?.Http?.IPAddress}...........");

    logger.LogInformation($"{projectName} ------->  strPort............ {appSettings?.KestrelWebServer?.Endpoints?.Http?.Port}.................");


    GlobalVariable.DBConnectionString = _configurationSettings.GetValue<string>("ConnectionStrings:DefaultConnection");

    GlobalVariable.DBconnectionString_Serilog = _configurationSettings.GetSection("Serilog:WriteTo:0:Args:connectionString").Value;

    GlobalVariable.HostingEnvironment = appSettings?.EnvironmentName;

    //GlobalVariable.ContentRootPath = localconfiguration.GetValue<string>(WebHostDefaults.ContentRootKey);


    logger.LogInformation($"{projectName} -------  The Current Hosting Environment(build) is --->  : {appSettings?.EnvironmentName} ");

    logger.LogInformation($"{projectName} -------  Appsettings json file is ---> : appsettings.{appSettings?.EnvironmentName}.json");

    logger.LogInformation($"{projectName} -------  Appsettings credential json file is ---> :  appsettings.{appSettings?.EnvironmentName}.json");

    logger.LogInformation($"{projectName}------->  KestrelWebServer hosted on --->: {appSettings?.KestrelWebServer?.Endpoints?.Http?.IPAddress}");


    ProgramStartUpUtil.Init_Serilog(_configurationSettings, GlobalVariable.DBconnectionString_Serilog, logger);
    ProgramStartUpUtil.TestSqlServerDBConnection(GlobalVariable.DBConnectionString, GlobalVariable.DBconnectionString_Serilog, logger);
    return appSettings;
}
