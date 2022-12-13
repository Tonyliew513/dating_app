using angular_API.DbContexts;
using angular_API.ModelsFromDB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace angular_API.Util
{
    public class ProgramStartUpUtil
    {
        public static void Init_Serilog(IConfiguration configurationAppSettingsEnvironment, string DBconnectionString_Serilog, Microsoft.Extensions.Logging.ILogger logger)
        {
            try
            {
                var serilogLoggingTableName = configurationAppSettingsEnvironment.GetSection("Serilog:WriteTo:0:Args:tableName").Value;

                using var dbSerilog = new SeriLogDbContext(DBconnectionString_Serilog);

                dbSerilog.Database.EnsureCreated();

                Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

                var sinkOpts = new MSSqlServerSinkOptions { TableName = serilogLoggingTableName, AutoCreateSqlTable = true };

                var columnOpts = new ColumnOptions();

                Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .Enrich.FromLogContext() //importan alanpei
                .WriteTo.Debug(new RenderedCompactJsonFormatter())
                .WriteTo.Console()
                //.Filter.ByExcluding((le) => le.Level == LogEventLevel.Information)
                //.AuditTo.MSSqlServer(
                .WriteTo.MSSqlServer(
                    connectionString: DBconnectionString_Serilog,
                    sinkOptions: sinkOpts,
                    columnOptions: null,
                    appConfiguration: configurationAppSettingsEnvironment,
                    restrictedToMinimumLevel: LogEventLevel.Warning
                ).CreateLogger();
                logger.LogInformation("this is 8");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "The Application failed to start.");
                logger.LogError(ex, Environment.StackTrace, ex.InnerException);
            }
           
        }

        public static void TestSqlServerDBConnection(string dbConnectionString, string dbConnection_SeriLog, Microsoft.Extensions.Logging.ILogger logger)
        {
            string? projectName = Assembly.GetCallingAssembly().GetName().Name;
            if (IsSQLServerConnected(dbConnectionString, logger, projectName) == false)
            {

                logger.LogError($"{projectName} ----> : The SQL server is not accesible at connectionstring : {dbConnectionString}");
            }
            else
            {
                logger.LogInformation($"{projectName} ----> :  The SQL server is accesible at connectionstring : {dbConnectionString}");

            }
            if (IsSQLServerConnected(dbConnection_SeriLog, logger, projectName) == false)
            {

                logger.LogError($"{projectName} , SeriLogDB ----> : The SQL server is not accesible at connectionstring : {dbConnection_SeriLog}");
            }
            else
            {
                logger.LogInformation($"{projectName} , SeriLogDB ----> : The SQL server is accessible at connectionstring : {dbConnection_SeriLog}");
            }
        }

        static bool IsSQLServerConnected(string dbConnectionString, Microsoft.Extensions.Logging.ILogger logger, string projectName)
        {
            var ret = false;
            try
            {
                using SqlConnection connection = new SqlConnection(dbConnectionString);
                connection.Open();
                ret = true;
            }
            catch (SqlException ex)
            {

                logger.LogError($"{projectName} ----> :  The SQL server is not accesible at connectionstring : {dbConnectionString}");
            }
            catch (Exception ex)
            {
                logger.LogError($"{projectName} ---->  : {ex.Message} \r\n {Convert.ToString(ex.InnerException)}");
            }
            return ret;
        }

        public static Action<DbContextOptionsBuilder> DbOptionsBuilder(string connString)
        {
            //var a = 1;
            return ConfigureSqlServer;

            void ConfigureSqlServer(DbContextOptionsBuilder optionsBuilder)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                       .AddDebug()
                      //.AddConsole()
                      .AddFilter(
                        category: DbLoggerCategory.Database.Command.Name,
                        level: LogLevel.Information);
                });
                optionsBuilder.UseLoggerFactory(loggerFactory);
                optionsBuilder.EnableDetailedErrors(true);
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlServer(connString);
            }
        }

    }
}
