using AspireDBTestApplication.PeopleDB;
using Microsoft.EntityFrameworkCore;

namespace AspireDBTestApplication.PeopleDbManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.AddSqlServerDbContext<PeopleDbContext>("sql", null,
    optionsBuilder => optionsBuilder.UseSqlServer(sqlBuilder =>
        sqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.AddSource(PeopleDbInitializer.ActivitySourceName));

        builder.Services.AddSingleton<PeopleDbInitializer>();
        builder.Services.AddHostedService(sp => sp.GetRequiredService<PeopleDbInitializer>());
        builder.Services.AddHealthChecks()
            .AddCheck<PeopleDbInitializerHealthCheck>("DbInitializer", null);


        var app = builder.Build();

        app.MapDefaultEndpoints();

        
        app.Run();
    }
}
