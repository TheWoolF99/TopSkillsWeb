namespace TopSkillsWeb.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder)
        {
            return builder; /*.UseSerilog((context, services, configuration) =>
            {
                var config = context.Configuration;

                configuration
                    .ReadFrom.Configuration(config)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithProperty("Application", nameof(TopSkillsWeb))
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

                // Глобальное переопределение для логирования ВСЕХ необработанных исключений
                ConfigureGlobalExceptionLogging();
            });*/
        }

        public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app)
        {
            return app;/*.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    if (ex != null || httpContext.Response.StatusCode > 499)
                        return LogEventLevel.Error;

                    if (elapsed > 10000) // Логировать медленные запросы как Warning
                        return LogEventLevel.Warning;

                    return LogEventLevel.Information;
                };
            });*/
        }

        private static void ConfigureGlobalExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                //Log.Fatal(e.ExceptionObject as Exception,
                //    "Необработанное исключение в домене приложения: {ExceptionObject}", e.ExceptionObject);
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                //Log.Error(e.Exception, "Необработанное исключение в задаче: {Exception}", e.Exception);
                //e.SetObserved();
            };
        }
    }
}